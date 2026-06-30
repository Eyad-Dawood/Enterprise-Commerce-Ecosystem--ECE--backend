# ECE.Api â€” Controller Generation Master Instruction Set

> **Authority:** This is the canonical reference for creating or extending controllers in `Code\ECE.Api\Controllers`. Follow it strictly to keep endpoints, metadata, and behavior aligned with the existing codebase.

---

## Â§1 Role & Non-Negotiable Constraints

You are implementing API controllers in a **CQRS + Result Pattern + Clean Architecture** backend.

- **ALWAYS** read existing patterns before writing: `ApiController`, similar feature controller(s), and request contracts.
- **ALWAYS** route business execution through `ISender` (MediatR). Controllers orchestrate; handlers own business rules.
- **ALWAYS** map failures to `Problem(...)` using `ApiController.Problem(List<Error>, HttpContext)`.
- **NEVER** swallow `Result` failures. Every send call that can fail must short-circuit to `Problem(...)` Using Match , dont return Status Code Always Use Match.
- **NEVER** invent endpoint metadata. Build it from validator rules, handler logic, and domain methods.
- **NEVER** break versioning/rate-limit conventions. Every endpoint must use `[MapToApiVersion("1.0")]` and rate limiting.
- **ALWAYS** Ask User for every endpoint authorization policy
---

## Â§2 Inputs You Must Collect Before Writing

For the selected command/query, inspect all of the following first:

1. `Code\ECE.Application\Features\...\Commands|Queries\<Feature>\<Name>.cs`
2. `...<Name>Validator.cs`
3. `...<Name>Handler.cs`
4. Related Application error classes (`<Entity>ApplicationErrors`, `ApplicationCommonErrors`, etc.)
5. Related Domain methods called by handler (`entity.Update`, `MarkAs...`, `Create`, value object `Create(...)`, etc.)
6. Related request contract(s) in `Code\ECE.Api\Contracts\Requests\...`
7. A similar existing controller endpoint in `Code\ECE.Api\Controllers\...`

If any rule is ambiguous, ask for clarification before writing.

---

## Â§3 Canonical Controller Placement & Skeleton

Place endpoints inside the existing feature controller folder:

```
Code\ECE.Api\Controllers\<Feature>\<Feature>Controller.cs
```

Base skeleton:

```csharp
namespace ECE.Api.Controllers.<Feature>;

[Route("api/v{version:apiVersion}/<resource-route>")]
[ApiVersion("1.0")]
[Authorize] // keep if controller is protected by default
public class <Feature>Controller(ISender sender, ...optional services...) : ApiController
{
    // endpoint methods
}
```

Use primary constructor injection only. Add services only when endpoint flow requires them (e.g., `ICurrentUser`, image service).

---

## Â§4 Endpoint Metadata Derivation (CRITICAL)

You must derive metadata from **validator + handler + domain**, not guesswork.

### 4.1 Response status rules

- **Query success** (`Result<TDto>`, paged query): `200 OK`
- **Create success** (`Result<TDto>` for create command): `201 Created` (prefer `CreatedAtRoute` when a by-id route exists)
- **Update/State/Delete success** (`Result<Updated|Deleted>`): `204 NoContent`
- **Action success returning payload** (OTP/token-like flows): `200 OK`

### 4.2 Failure status rules from discovered errors

Map potential failure kinds to OpenAPI metadata:

- `ErrorKind.Validation` -> `400`
- `ErrorKind.NotFound` -> `404`
- `ErrorKind.Conflict` -> `409`
- `ErrorKind.Unauthorized` -> `401`
- `ErrorKind.Forbidden` -> `403`

Because base API behavior can still return infra errors, keep defaults:

- `[ProducesDefaultResponseType]`
- Base controller already standardizes `500` and `429`.

### 4.3 How to discover which failure codes to include

1. **Validator** -> include `400`.
2. **Handler guarded lookups** (`if (x is null) return ...NotFound`) -> include `404`.
3. **Business/state checks** (`AlreadyExists`, invalid transition, ownership conflicts) -> include `409` or `400` based on returned `ErrorKind`.
4. **Policy-based authorization** (`[Authorize(Policy = ...)]`) -> include `401` + `403`.
5. **Pre-steps in controller** (e.g., resolve current student, verify OTP) -> include any status from those sends too.

Do not add statuses that cannot occur from real flow.

---

## Â§5 Endpoint Documentation Rules (Summary/Description/Name)

For each endpoint, include:

- `[EndpointSummary("...")]`
- `[EndpointDescription("...")]`
- `[EndpointName("...")]`

Generation rules:

1. **Summary:** imperative one-line action (e.g., "Mark a borrowing request as accepted.").
2. **Description:** explain business effect and constraints using handler/domain behavior.
   - If state machine transition exists, mention valid-state requirement.
   - If query supports filter/sort/search, mention supported capabilities and major sort fields from query handler sort mappings.
3. **EndpointName:** stable PascalCase verb+resource, matching existing naming style (`GetXById`, `CreateX`, `MarkXAsY`).

---

## Â§6 HTTP Verb, Route, and Binding Rules

Use existing feature conventions:

- `GET` for queries (`by id`, paged list, `me/*` variants)
- `POST` for creation and side-effect actions that are not idempotent (send OTP, create transaction, token endpoints)
- `PATCH` for updates/state transitions
- `DELETE` for removals

### 6.1 Request contract is mandatory for every endpoint

- **MUST** create a dedicated request class for every endpoint input (EveryRequest rule), even when the shape is small.
- Place request contracts under `Code\BookOrbit.Api\Contracts\Requests\<Feature>\`.
- Reuse shared base contracts only when appropriate (for example `PagedFilterRequest`, `ConcurrencyRequest`).
- Do not bind API methods directly to command/query types unless the feature already intentionally follows that pattern.

Binding:

- `[FromRoute]` for identifiers in route.
- `[FromQuery]` for paged/filter inputs.
- `[FromBody]` for JSON command payload.
- `[FromForm]` for multipart/file payload.

If request inherits `ConcurrencyRequest` or Command inherits `ConcurrencyCommand` , pass `RowVersion` into command and include conflict metadata.

### 6.2 Request-to-command/query mapping is mandatory

- Controllers **MUST** map request contracts into command/query constructors explicitly.
- Mapping must be field-by-field and preserve nullability/optionality from the API contract.
- Route and contextual values (for example `studentId`, `currentUser.Id`) must be merged with request values when constructing the command/query.
- Any preprocessing step (for example file upload, enum mapping, lookup query) must finish before creating the command/query object.

### 6.3 How to create the request contract from command/query requirements (MUST FOLLOW)

1. Read the selected command/query constructor and list only API-supplied inputs (exclude values resolved from route/context/pre-steps).
2. Read validator rules to confirm required vs optional fields and data shape.
3. If the query is paged/filterable, create a request that inherits:
   - `Code\BookOrbit.Api\Contracts\Requests\Common\PagedFilterRequest`
4. If the command/query requires concurrency token (`RowVersion`), create a request that inherits:
   - `Code\BookOrbit.Api\Contracts\Requests\Common\ConcurrencyRequest`
5. If both are needed, keep the request model aligned with existing patterns (prefer a dedicated contract with required fields; if multiple inheritance is impossible, include equivalent properties explicitly and preserve `RowVersion` contract semantics).
6. Add only feature-specific fields beyond base contract members.
7. Use correct binding target in controller (`[FromQuery]`, `[FromBody]`, `[FromForm]`) and map request to command/query explicitly.

### 6.4 In-repo request examples you must mirror

**Paged requests (inherit `PagedFilterRequest`):**

```csharp
public record BookPagedFilterRequest : PagedFilterRequest
{
    public List<BookCategory>? Categories { get; set; } = null;
    public List<BookStatus>? Statuses { get; set; } = null;
}

public record BorrowingRequestPagedFilterRequest : PagedFilterRequest
{
    public Guid? BorrowingStudentId { get; set; } = null;
    public Guid? LendingListRecordId { get; set; } = null;
    public Guid? LendingStudentId { get; set; } = null;
    public List<BorrowingRequestState>? States { get; set; } = null;
}
```

**Concurrency requests (inherit `ConcurrencyRequest`):**

```csharp
public sealed record ApproveStudentRequest : ConcurrencyRequest
{
    public override string RowVersion { get; set; } = string.Empty;
}

public sealed record BorrowingRequestStateChangeRequest : ConcurrencyRequest
{
    public override string RowVersion { get; set; } = string.Empty;
}

public record OtpRequest : ConcurrencyRequest
{
    public string OtpCode { get; set; } = string.Empty;
    public override string RowVersion { get; set; } = string.Empty;
}
```

**Non-paged/non-concurrency request:**

```csharp
public record CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public List<BookCategory>? Categories { get; set; } = null;
    public string Author { get; set; } = string.Empty;
    public IFormFile CoverImage { get; set; } = default!;
}
```

---

## Â§7 Controller Flow Pattern (MUST FOLLOW)

Canonical method body:

```csharp
public async Task<ActionResult<TResponse>> Method(..., CancellationToken ct)
{
    // optional pre-step sends (current user -> student, OTP verification, etc.)

    var result = await sender.Send(new SomeCommandOrQuery(...), ct);

    return result.Match(
        success => /* Ok | CreatedAtRoute | NoContent */,
        e => Problem(e, HttpContext));
}
```

If there are pre-step sends, each must short-circuit on failure:

```csharp
if (preResult.IsFailure)
    return Problem(preResult.Errors, HttpContext);
```

---

## Â§8 Authorization + Rate Limit + Versioning Rules

Each endpoint must explicitly include:

- `[MapToApiVersion("1.0")]`
- `[EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]`  
  (or sensitive/once-per-minute policy only when endpoint is security-sensitive)

Auth rules:

- Use controller-level `[Authorize]` for protected groups.
- Override with `[AllowAnonymous]` only when required (token, register, email confirmation-like flows).
- Use narrow policy per endpoint (`AdminOnlyPolicy`, `ActiveStudentPolicy`, ownership/related-resource policies).

---

## Â§9 Minimal Endpoint Template (Copy/Adapt)

```csharp
[HttpPatch("{entityId:guid}/approve")]
[Authorize(Policy = PoliciesNames.AdminOnlyPolicy)]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
[ProducesDefaultResponseType]
[EndpointSummary("Approve a pending entity.")]
[EndpointDescription("Approves the specified entity when validation and state-transition rules pass.")]
[EndpointName("ApproveEntity")]
[MapToApiVersion("1.0")]
[EnableRateLimiting(ApiConstants.NormalRateLimitingPolicyName)]
public async Task<ActionResult> ApproveEntity([FromRoute] Guid entityId, [FromBody] ApproveEntityRequest request, CancellationToken ct)
{
    var result = await sender.Send(new ApproveEntityCommand(entityId, request.RowVersion), ct);

    return result.Match(
        _ => NoContent(),
        e => Problem(e, HttpContext));
}
```

---

## Â§10 Quality Checklist (Strict)

- [ ] Endpoint placed in correct controller file/folder.
- [ ] Uses `ISender` and returns `Problem(...)` on every failure path.
- [ ] Metadata reflects real validator/handler/domain behavior (not generic guesses).
- [ ] Response types and status codes match command/query result shape.
- [ ] Authorization policy is correct and least-privileged.
- [ ] Rate limit policy + API version mapping included.
- [ ] Route and binding attributes align with request shape.
- [ ] Every endpoint has a dedicated API request contract in `Contracts\Requests\...`.
- [ ] Controller maps request contract fields explicitly into command/query constructor inputs.
- [ ] `EndpointSummary`, `EndpointDescription`, `EndpointName` are clear and behavior-accurate.
- [ ] `CreatedAtRoute` used where create endpoint has matching get-by-id route.
- [ ] Concurrency flows include `RowVersion` and conflict response metadata.

