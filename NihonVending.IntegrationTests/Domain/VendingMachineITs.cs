using NihonVending.Domain;
using NHibernate.Exceptions;

namespace NihonVending.IntegrationTests;

public class VendingMachineIT
{
    [Fact]
    public void VendingMachine_can_be_saved_and_loaded()
    {
        long id;

        using (var session = NHibernateSessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var vendingMachine = new VendingMachine
            {
                Name = "Regular Vending Machine",
                Latitude = 1,
                Longitude = 2,
                City = "Osaka",
                Category = "snacks",
                Source = "https://www.google.com/maps",
            };

            session.Save(vendingMachine);
            transaction.Commit();

            id = vendingMachine.Id;
        }

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var loaded = session.Get<VendingMachine>(id);

            Assert.NotNull(loaded);
            Assert.Equal("Regular Vending Machine", loaded.Name);
            Assert.Equal(1, loaded.Latitude);
            Assert.Equal(2, loaded.Longitude);
            Assert.Equal("Osaka", loaded.City);
            Assert.Equal("snacks", loaded.Category);
            Assert.Equal("https://www.google.com/maps", loaded.Source);
        }
    }

    [Fact]
    public void VendingMachine_lat_and_lon_constraints_raise_error()
    {
        using (var session = NHibernateSessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var vendingMachine = new VendingMachine
            {
                Name = "Regular Vending Machine",
                Latitude = 91,
                Longitude = -181,
                City = "Osaka",
                Category = "snacks",
                Source = "https://www.google.com/maps",
            };

            session.Save(vendingMachine);

            var exception = Assert.Throws<GenericADOException>(
                () => session.Flush());

            transaction.Rollback();

            Assert.NotNull(exception);
            Assert.NotNull(exception.InnerException);
        }
    }
}