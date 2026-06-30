# ECE â€” Feature Implementation Master Instruction Set

> **Authority:** This is the single, canonical reference for implementing any new feature in `ECE.Application`. Follow every rule exactly. Deviation requires explicit user approval.

---

## Â§1 Role & Behavioral Constraints

You are implementing features inside a **CQRS + Result Pattern + Clean Architecture** codebase.

- **NEVER** implement infrastructure concerns (database, email, identity) in the Application layer.
- **NEVER** skip a `Result<>` failure check. Every `Result`-returning call **MUST** be followed by: `if (result.IsFailure) return result.Errors;`
- **NEVER** use `DateTime.UtcNow` directly. Inject `TimeProvider` and call `timeProvider.GetUtcNow()`.
- **NEVER** guess business rules. If rules are ambiguous, **stop and ask the user**.
- **ALWAYS** preserve existing project conventions, including known intentional spellings (e.g., `StateMachine` folder name).

---

## Â§2 Pre-Work: Interactive Rule Gathering (CRITICAL)

**Before writing any code**, ask the user for business rules in plain language dont ask about authorization since you will not work in api layer.

### Translation Workflow
1. Ask for domain constraints in business language.
2. Translate each rule into a technical check.
3. Implement checks using `context.<Entities>.AnyAsync(...)` (or equivalent) inside the handler.
4. Map each failure to a named error in `<Entity>ApplicationErrors`. (error codes should always be lowercase if the code has more than 1 word spearate using `_` ex : student_id , borrowing_request_date)

**Example:**

> **Business rule:** *"A student cannot add a request to a record that already has an active request for that same student."*

```csharp
var hasActiveRequest = await context.BorrowingRequests
    .AsNoTracking()
    .AnyAsync(
        x => x.BorrowingStudentId == command.BorrowingStudentId
          && x.LendingRecordId == command.LendingRecordId
          && (x.State == BorrowingRequestState.Pending
           || x.State == BorrowingRequestState.Accepted),
        ct);

if (hasActiveRequest)
    return BorrowingRequestApplicationErrors.AlreadyExists;
```

---

## Â§3 Canonical Folder & File Structure

```
Code\ECE.Application\Features\<Entities>\
  <Entity>GeneralValidation.cs
  <Entity>ApplicationErrors.cs
  Dtos\
    <Entity>Dto.cs
    <Entity>ListItemDto.cs
  Commands\
    Create<Entity>\
      Create<Entity>Command.cs
      Create<Entity>CommandValidator.cs
      Create<Entity>CommandHandler.cs
    Update<Entity>\
      Update<Entity>Command.cs
      Update<Entity>CommandValidator.cs
      Update<Entity>CommandHandler.cs
    StateMachine\                          â† intentional spelling; do NOT rename
      <TransitionName>\
        <TransitionName>Command.cs
        <TransitionName>CommandValidator.cs
        <TransitionName>CommandHandler.cs
  Queries\
    Get<Entity>ById\
      Get<Entity>ByIdQuery.cs
      Get<Entity>ByIdQueryValidator.cs
      Get<Entity>ByIdQueryHandler.cs
    Get<Entities>\
      Get<Entities>Query.cs
      Get<Entities>QueryHandler.cs
  EventHandlers\                           â† only if domain events exist
    <DomainEvent>Handler.cs

Code\ECE.Application\Common\Constants\
  <Entity>CachingConstants.cs
```

---

## Â§4 Required Supporting Classes (Create First)

### 4.1 `<Entity>GeneralValidation.cs`

```csharp
namespace ECE.Application.Features.<Entities>;

public static class <Entity>GeneralValidation
{
    public static IRuleBuilder<T, Guid> <Entity>IdRules<T>(this IRuleBuilder<T, Guid> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage(<Entity>Errors.IdRequired.Description)
            .Must(id => id != Guid.Empty).WithMessage(<Entity>Errors.IdRequired.Description);

    public static IRuleBuilder<T, string> <Entity>NameRules<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage(<Entity>Errors.NameRequired.Description)
            .MaximumLength(<Entity>Name.MaxLength).WithMessage(<Entity>Errors.InvalidName.Description)
            .MinimumLength(<Entity>Name.MinLength).WithMessage(<Entity>Errors.InvalidName.Description);

    // Add one extension method per domain property validated in commands/queries.
}
```

### 4.2 `<Entity>ApplicationErrors.cs`

> **Note:** `<Entity>Errors.ClassName` is a constant defined in the **Domain** layer's `<Entity>Errors` class.

```csharp
namespace ECE.Application.Features.<Entities>;

public static class <Entity>ApplicationErrors
{
    public static readonly Error NotFoundById =
        ApplicationCommonErrors.NotFoundClass(<Entity>Errors.ClassName, "Id", "Id");

    public static readonly Error AlreadyExists =
        ApplicationCommonErrors.CustomConflict(<Entity>Errors.ClassName, "AlreadyExists", "Entity already exists.");

    // Use Domain Common Errors For Any Common Error Type [Failure , Validation , Conflict , etc]
    // Example : 
       public static readonly Error CanOnlyDeletePending =
        DomainCommonErrors.CustomValidation(BookErrors.ClassName,"book","Books can only be deleted if they are in Pending status.");

    // Add one error per distinct business-rule failure or authorization check.
}
```

### 4.3 `<Entity>CachingConstants.cs`

```csharp
namespace ECE.Application.Common.Constants;

public static class <Entity>CachingConstants
{
    public const string <Entity>Tag = "<entity-lower>";

    public static string <Entity>Key(Guid id) => $"<entity-lower>:{id}";

    public static string <Entity>ListKey(Get<Entities>Query query) =>
        $"<entity-lower>:p={query.Page}:ps={query.PageSize}" +
        $":st={query.SearchTerm ?? "-"}" +
        $":sort={query.SortColumn}:{query.SortDirection ?? "-"}";

    public const int ExpirationInMinutes = 10;
}
```

---

## Â§5 Commands (Write-Side)

- Use `record` types.
- **Return types:**
  - Create â†’ `Result<<Entity>Dto>`
  - Update / Patch / State Change â†’ `Result<Updated>` (return `Result.Updated`)
  - Delete â†’ `Result<Deleted>` (return `Result.Deleted`)
- **Concurrency:** If the domain entity implements `IConcurrencyEntity`, the command **MUST** inherit `ConcurrencyCommand<Result<Updated>>` and accept `string RowVersion`.
- **Nullability:** Command properties **MUST** mirror domain nullability (`string?`, `Guid?`, etc.).

```csharp
namespace ECE.Application.Features.<Entities>.Commands.Update<Entity>;

public record Update<Entity>Command(
    Guid Id,
    string Name,
    string? OptionalNote,    // nullable because domain allows null
    string RowVersion)
    : ConcurrencyCommand<Result<Updated>>(RowVersion);
```

---

## Â§6 Validators

- Inherit `AbstractValidator<T>`.
- **MUST** use `CascadeMode.Stop` on every `RuleFor` chain.
- **MUST** use extension methods from `<Entity>GeneralValidation`.
- For nullable properties, use `.When(x => x.Property is not null)`.
- `RowVersion` on concurrency commands: `.NotEmpty()`.
- If the rule is for another entity like StudentOwnerId in BookCopy Dont Create a specific Rule in BookCopyGeneralValidation , use the rule from the EntityGeneralValidation in this case `StudentGeneralValidation`

```csharp
namespace ECE.Application.Features.<Entities>.Commands.Update<Entity>;

public class Update<Entity>CommandValidator : AbstractValidator<Update<Entity>Command>
{
    public Update<Entity>CommandValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .<Entity>IdRules();

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .<Entity>NameRules();

        RuleFor(x => x.OptionalNote)
            .MaximumLength(500)
            .When(x => x.OptionalNote is not null);

        RuleFor(x => x.RowVersion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}
```

---

## Â§7 Handlers

### Mandatory Rules
- **MUST** use primary constructor injection.
- **MUST** inject `ILogger<THandler>` and `IAppDbContext`.
- **MUST** inject `HybridCache` for cache invalidation on writes.
- **MUST** inject `IConcurrencyService` if the command inherits `ConcurrencyCommand`.
- **MUST** inject `TimeProvider` if the handler needs timestamps.
- **MAY** inject `ICurrentUser` for ownership/authorization guards.
- Use `.AsNoTracking()` for read-only lookups within command handlers.
- After every `Result<>`-returning call: `if (result.IsFailure) return result.Errors;`
- On successful write: `SaveChangesAsync` â†’ cache tag invalidation â†’ log success.

### 7A â€” Update/State-Change Handler (with Concurrency)

```csharp
namespace ECE.Application.Features.<Entities>.Commands.Update<Entity>;

public class Update<Entity>CommandHandler(
    ILogger<Update<Entity>CommandHandler> logger,
    IAppDbContext context,
    IConcurrencyService concurrencyService,
    HybridCache cache)
    : IRequestHandler<Update<Entity>Command, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(Update<Entity>Command command, CancellationToken ct)
    {
        // 1. Fetch entity
        var entity = await context.<Entities>.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (entity is null)
        {
            logger.LogWarning("{Entity} {EntityId} not found.", nameof(<Entity>), command.Id);
            return <Entity>ApplicationErrors.NotFoundById;
        }

        // 2. Business rule guards (AnyAsync)
        var duplicateExists = await context.<Entities>.AnyAsync(
            x => x.Id != command.Id && x.Name.Value == command.Name, ct);
        if (duplicateExists) return <Entity>ApplicationErrors.AlreadyExists;

        // 3. Create value objects
        var nameResult = <Entity>Name.Create(command.Name);
        if (nameResult.IsFailure) return nameResult.Errors;

        // 4. Mutate domain entity
        var updateResult = entity.Update(nameResult.Value, command.OptionalNote);
        if (updateResult.IsFailure) return updateResult.Errors;

        // 5. Set concurrency token
        var concurrencyResult = concurrencyService.SetOriginalRowVersion(entity, command.RowVersion);
        if (concurrencyResult.IsFailure) return concurrencyResult.Errors;

        // 6. Persist + invalidate cache
        await context.SaveChangesAsync(ct);
        await cache.RemoveByTagAsync(<Entity>CachingConstants.<Entity>Tag, ct);

        logger.LogInformation("{Entity} {EntityId} updated.", nameof(<Entity>), entity.Id);
        return Result.Updated;
    }
}
```

### 7B â€” Create Handler

```csharp
namespace ECE.Application.Features.<Entities>.Commands.Create<Entity>;

public class Create<Entity>CommandHandler(
    ILogger<Create<Entity>CommandHandler> logger,
    IAppDbContext context,
    TimeProvider timeProvider,
    HybridCache cache)
    : IRequestHandler<Create<Entity>Command, Result<<Entity>Dto>>
{
    public async Task<Result<<Entity>Dto>> Handle(Create<Entity>Command command, CancellationToken ct)
    {
        // 1. Business rule guards
        var exists = await context.<Entities>.AnyAsync(x => x.Name.Value == command.Name, ct);
        if (exists) return <Entity>ApplicationErrors.AlreadyExists;

        // 2. Create value objects
        var nameResult = <Entity>Name.Create(command.Name);
        if (nameResult.IsFailure) return nameResult.Errors;

        // 3. Create domain entity
        var now = timeProvider.GetUtcNow();
        var entityResult = <Entity>.Create(Guid.NewGuid(), nameResult.Value, now);
        if (entityResult.IsFailure) return entityResult.Errors;

        // 4. Persist
        context.<Entities>.Add(entityResult.Value);
        await context.SaveChangesAsync(ct);
        await cache.RemoveByTagAsync(<Entity>CachingConstants.<Entity>Tag, ct);

        logger.LogInformation("{Entity} {EntityId} created.", nameof(<Entity>), entityResult.Value.Id);
        return <Entity>Dto.FromEntity(entityResult.Value);
    }
}
```

---

## Â§8 Queries (Read-Side)

### 8A â€” Paginated List Query (Cached)

```csharp
namespace ECE.Application.Features.<Entities>.Queries.Get<Entities>;

public record Get<Entities>Query(
    int Page,
    int PageSize,
    string? SearchTerm,
    string? SortColumn = QuerySettings.DefaultSortColumn,
    string? SortDirection = QuerySettings.DefaultSortDirection)
    : IPagedQuery<<Entity>ListItemDto>
{
    public string CacheKey => <Entity>CachingConstants.<Entity>ListKey(this);
    public string[] Tags => [<Entity>CachingConstants.<Entity>Tag];
    public TimeSpan Expiration => TimeSpan.FromMinutes(<Entity>CachingConstants.ExpirationInMinutes);
}
```

### 8B â€” Paginated List Handler (`BasePagedQueryHandler`)

GetAll handlers **MUST** inherit `BasePagedQueryHandler<TEntity, TDto, TQuery>` and override:
- `GetBaseQuery()` â€” returns `IQueryable<TEntity>` from `context.<Entities>`.
- `ProjectToDto()` â€” projects to `IQueryable<TDto>`.
- `SortMappings` â€” dictionary mapping sort column strings to ordering functions.
- *(Optional)* `ApplySearch()`, `ApplyFilters()`.
- If Entity Has Value Object that search term will be applied to it in `ApplySearch()` use ValueObject.Normalize(); before searching 
- dont add a validator for GetAllQuery

```csharp
namespace ECE.Application.Features.<Entities>.Queries.Get<Entities>;

public class Get<Entities>QueryHandler(IAppDbContext context)
    : BasePagedQueryHandler<<Entity>, <Entity>ListItemDto, Get<Entities>Query>
{
    protected override Dictionary<string, Func<IQueryable<<Entity>>, bool, IOrderedQueryable<<Entity>>>> SortMappings => new()
    {
        ["createdat"] = (q, desc) => desc ? q.OrderByDescending(e => e.CreatedAt) : q.OrderBy(e => e.CreatedAt),
        ["name"]      = (q, desc) => desc ? q.OrderByDescending(e => e.Name.Value) : q.OrderBy(e => e.Name.Value),
    };

    protected override IQueryable<<Entity>> GetBaseQuery() => context.<Entities>.AsNoTracking();

    
    protected override IQueryable<<Entity>> ApplySearch(IQueryable<<Entity>> query, Get<Entities>Query request)
    {
        if (string.IsNullOrWhiteSpace(request.SearchTerm)) return query;
        var term = request.SearchTerm.ToLower();
        return query.Where(e => e.Name.Value.ToLower().Contains(term));
    }
    //OR
    protected override IQueryable<<Entity>> ApplySearch(IQueryable<<Entity>> query, Get<Entities>Query request)
    {
    if (string.IsNullOrWhiteSpace(request.SearchTerm)) return query;

    var normalizedTitle = BookTitle.Normalize(request.SearchTerm);
    var normalizedISBN = ISBN.Normalize(request.SearchTerm);
    var normalizedPublisher = BookPublisher.Normalize(request.SearchTerm);
    var normalizedAuthor = BookAuthor.Normalize(request.SearchTerm);


    return query.Where(e =>
        e.Title.Value.Contains(normalizedTitle) ||
        e.Author.Value.Contains(normalizedAuthor) ||
        e.Publisher.Value.Contains(normalizedPublisher) ||
        e.ISBN.Value.Contains(normalizedISBN));
    }
    
    protected override IQueryable<<Entity>ListItemDto> ProjectToDto(IQueryable<<Entity>> query) =>
        query.Select(e => new <Entity>ListItemDto(e.Id, e.Name.Value, e.RowVersion));
}
```

### 8C â€” GetById Query (Cached)

```csharp
public record Get<Entity>ByIdQuery(Guid Id) : ICachedQuery<Result<<Entity>Dto>>
{
    public string CacheKey => <Entity>CachingConstants.<Entity>Key(Id);
    public string[] Tags => [<Entity>CachingConstants.<Entity>Tag];
    public TimeSpan Expiration => TimeSpan.FromMinutes(<Entity>CachingConstants.ExpirationInMinutes);
}
```

---

## Â§9 DTOs

- **MUST** include a **private parameterless constructor** with `[JsonConstructor]` for `HybridCache` deserialization.
- `<Entity>Dto` â€” full detail DTO with `static FromEntity(...)` factory.
- `<Entity>ListItemDto` â€” lightweight DTO for paginated list projections.
- Use `IRouteService` to resolve media/image URLs when building DTOs.

```csharp
namespace ECE.Application.Features.<Entities>.Dtos;

public record <Entity>Dto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];

    [JsonConstructor]
    private <Entity>Dto() { }

    private <Entity>Dto(Guid id, string name, byte[] rowVersion)
    { Id = id; Name = name; RowVersion = rowVersion; }

    public static <Entity>Dto FromEntity(<Entity> entity) =>
        new(entity.Id, entity.Name.Value, entity.RowVersion);
}
```

---

## Â§10 Domain Event Handlers

If the domain entity raises events, create handlers under `Features\<Entities>\EventHandlers\`.

- Implement `INotificationHandler<TDomainEvent>`.
- Use primary constructor injection.
- Event handlers perform **side-effects only** (notifications, emails, cross-aggregate updates). They **MUST NOT** return `Result<>` â€” log failures and return gracefully.

```csharp
public class <DomainEvent>Handler(
    IAppDbContext context,
    ILogger<<DomainEvent>Handler> logger,
    ISystemNotificationService systemNotificationService)
    : INotificationHandler<<DomainEvent>>
{
    public async Task Handle(<DomainEvent> notification, CancellationToken ct)
    {
        var data = await context.<RelatedEntities>
            .AsNoTracking()
            .Where(x => x.Id == notification.RelatedId)
            .Select(x => new { x.Name, x.OwnerId })
            .FirstOrDefaultAsync(ct);

        if (data is null)
        {
            logger.LogError("Data not found for {Event}, Id={Id}.", nameof(<DomainEvent>), notification.RelatedId);
            return;
        }

        await systemNotificationService.SendNotificationAsync(
            data.OwnerId, "Title", "Message", NotificationType.Normal, ct);
    }
}
```

---

## Â§11 External Service Orchestration

If a handler needs out-of-core operations (identity, email, OTP, chat, notifications, payments):

1. Check if a suitable interface exists in `Common\Interfaces\`.
2. If yes â†’ add methods to that interface.
3. If no â†’ create a **new interface** there.
4. **NEVER** implement the interface in the Application layer.

**Key Existing Interfaces:**
| Interface | Purpose |
|---|---|
| `IAppDbContext` | Primary database gateway |
| `HybridCache` | Caching and tag-based invalidation |
| `IConcurrencyService` | Sets original `RowVersion` for optimistic concurrency |
| `IRouteService` | Resolves media/image URLs in DTOs |
| `ICurrentUser` | Provides authenticated user identity for auth guards |
| `TimeProvider` | Injectable clock for testable timestamps |

---

## Â§12 Pipeline Behaviours (Awareness)

These run **automatically** on every MediatR request. **Do NOT duplicate their logic:**

| Behaviour | What It Does |
|---|---|
| `ValidationBehaviour` | Runs FluentValidation validators before handler execution |
| `CachingBehaviour` | Serves cached responses for `ICachedQuery` implementations |
| `ConcurrencyExceptionHandler` | Catches `DbUpdateConcurrencyException` â†’ conflict Result |
| `LoggingBehaviour` | Logs request entry/exit |
| `PerformanceBehaviour` | Logs slow requests |
| `ExceptionLoggingBehaviour` | Catches and logs unhandled exceptions |

---

## Â§13 Implementation Checklist (Strict)

- [ ] `<Entity>GeneralValidation.cs` â€” extension methods for all validated properties
- [ ] `<Entity>ApplicationErrors.cs` â€” named errors for every failure path 
- [ ] `<Entity>CachingConstants.cs` â€” tag, key, list key, expiration
- [ ] DTOs created with `[JsonConstructor]` private constructor
- [ ] Commands as `record` with correct return types and nullability parity
- [ ] Concurrency commands inherit `ConcurrencyCommand<>` with `RowVersion`
- [ ] Validators use `CascadeMode.Stop` + `GeneralValidation` extensions
- [ ] Handlers use primary constructors with `ILogger`, `IAppDbContext`, `HybridCache`
- [ ] Concurrency handlers inject `IConcurrencyService` + call `SetOriginalRowVersion`
- [ ] `IsFailure` short-circuit on **every** `Result<>`-returning call
- [ ] Business rules use `AnyAsync` â†’ `<Entity>ApplicationErrors`
- [ ] Read-only lookups use `.AsNoTracking()`
- [ ] `TimeProvider` used instead of `DateTime.UtcNow`
- [ ] GetAll handler inherits `BasePagedQueryHandler` with proper overrides
- [ ] Queries implement `IPagedQuery` / `ICachedQuery` with caching properties
- [ ] Domain event handlers created if entity raises events
- [ ] External service interfaces defined (not implemented) in `Common\Interfaces\`
- [ ] All files in correct folders per Â§3
