
namespace ECE.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : IdentityDbContext<AppUser>(options), IAppDbContext
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureSequences();
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        while (true) // if domain events trigger other domain events, we need to keep dispatching until there are no more domain events to dispatch
        {

            var domainEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
                .Select(e => (Entity)e.Entity)
                .ToList();


            var domainEvents = domainEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            if (domainEvents.Count == 0) break;


            //Clear to prevent infinite loop in case of domain events triggering other domain events
            foreach (var entity in domainEntities) entity.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }

}

