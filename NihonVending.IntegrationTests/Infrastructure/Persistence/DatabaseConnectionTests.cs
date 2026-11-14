namespace NihonVending.IntegrationTests;

using System.Data;
using NihonVending.Infrastructure.Persistence;

public class DatabaseConnectionTests
{
    [Fact]
    public void Can_connect_to_database()
    {
        using var session = NHibernateSessionFactory.OpenSession();
        using var connection = session.Connection;

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    [Fact]
    public void Can_connect_to_database_and_execute_sql()
    {
        using var session = NHibernateSessionFactory.OpenSession();

        var result = session
            .CreateSQLQuery("SELECT 1")
            .UniqueResult<int>();

        Assert.Equal(1, result);
    }
}
