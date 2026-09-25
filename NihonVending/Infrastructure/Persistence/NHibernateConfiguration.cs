using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace NihonVending.Persistence;

public static class NHibernateConfiguration
{
    public static ISessionFactory CreateSessionFactory(string connectionString)
    {
        var configuration = new Configuration();

        configuration.DataBaseIntegration(db =>
        {
            db.ConnectionString = connectionString;
            db.Dialect<PostgreSQLDialect>();
            db.Driver<NpgsqlDriver>();

            db.LogSqlInConsole = true;
            db.LogFormattedSql = true;
        });

        configuration.AddAssembly(
            typeof(NHibernateConfiguration).Assembly);

        return configuration.BuildSessionFactory();
    }
}
