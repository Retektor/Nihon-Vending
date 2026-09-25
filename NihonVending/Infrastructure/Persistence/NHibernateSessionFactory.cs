using NHibernate;
using NHibernate.Cfg;
using Npgsql;
using DotNetEnv;

public static class NHibernateSessionFactory
{
    private static readonly Lazy<ISessionFactory> Factory =
        new(BuildSessionFactory);

    public static NHibernate.ISession OpenSession()
    {
        return Factory.Value.OpenSession();
    }

    private static ISessionFactory BuildSessionFactory()
    {
        Env.TraversePath().Load();

        var configuration = new Configuration();

        var portValue = System.Environment.GetEnvironmentVariable("POSTGRES_PORT");

        if (!int.TryParse(portValue, out var port))
        {
            throw new InvalidOperationException(
                "POSTGRES_PORT must contain a valid integer.");
        }

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = port,
            Database = System.Environment.GetEnvironmentVariable("POSTGRES_DB"),
            Username = System.Environment.GetEnvironmentVariable("POSTGRES_USER"),
            Password = System.Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
        };

        var connectionString = connectionStringBuilder.ConnectionString;

        configuration.DataBaseIntegration(db =>
        {
            db.ConnectionString = connectionString;
            db.Driver<NHibernate.Driver.NpgsqlDriver>();
            db.Dialect<NHibernate.Dialect.PostgreSQLDialect>();
        });

        return configuration.BuildSessionFactory();
    }
}