namespace ECE.Application;

static public class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(ExceptionLoggingBehaviour<,>)); // logs exception and rethrows it
            cfg.AddOpenBehavior(typeof(ConcurrencyExceptionHandler<,>)); // Order Is Super Important , return result and swallow the exception
                                                                         //Dont Log Concurrency Exception Because It Is Handled By ConcurrencyExceptionHandler

            cfg.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            cfg.AddOpenBehavior(typeof(CachingBehaviour<,>));
        });

        return services;
    }
}

