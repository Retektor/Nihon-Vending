using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using NihonVending.Domain.DTOs;
using NihonVending.Infrastructure.Mappers;
using NihonVending.Infrastructure.Persistence;
using NihonVending.Services;

namespace NihonVending.IntegrationTests.Services;

public class VendingMachineServiceIT
{
    [Fact]
    public void Create_works_correctly()
    {
        long id;

        var request = new VendingMachineCreateRequest
        {
            Name = "Very Interesting Vending Machine",
            Latitude = 10,
            Longitude = 25,
            City = "Nagoya",
            Category = "drinks",
            Source = "https://www.google.com/maps",
        };

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            id = service.Create(request).Id;
        }

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            var machine = service.GetById(id);

            Assert.NotNull(machine);
            AssertMachine(machine, request);
        }
    }

    [Fact]
    public void GetAll_works_correctly()
    {
        long id;

        var requestList = new List<VendingMachineCreateRequest>
        {
            new()
            {
                Name = "Usual Vending Machine",
                Latitude = 10,
                Longitude = 25,
                City = "Nagoya",
                Category = "drinks",
                Source = "https://www.google.com/maps",
            },
            new()
            {
                Name = "Different Vending Machine",
                Latitude = 32.5M,
                Longitude = 23.324M,
                City = "Osaka",
                Category = "snacks",
                Source = "https://www.google.com/maps",
            },
            new()
            {
                Name = "Different Vending Machine",
                Latitude = 32.5M,
                Longitude = 23.324M,
                City = "Osaka",
                Category = "snacks",
                Source = "https://www.google.com/maps",
            },
        };

        List<long> idList = [];

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            foreach (var r in requestList)
            {
                id = service.Create(r).Id;
                idList.Add(id);
            }
        }

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            var fetchedMachines = service.GetAll();

            Assert.True(fetchedMachines.Count >= requestList.Count);

            // We use custom iteration instead of AssertMachine
            // because DB population SQL shifts indexes of machines
            foreach (var request in requestList)
            {
                Assert.Contains(fetchedMachines, m =>
                    m.Name == request.Name &&
                    m.Latitude == request.Latitude &&
                    m.Longitude == request.Longitude &&
                    m.City == request.City &&
                    m.Category == request.Category &&
                    m.Source == request.Source);
            }
        }
    }

    [Fact]
    public void GetById_works_correctly()
    {
        long id;

        VendingMachineCreateRequest request = new()
        {
            Name = "Usual Vending Machine",
            Latitude = 10,
            Longitude = 25,
            City = "Nagoya",
            Category = "drinks",
            Source = "https://www.google.com/maps",
        };

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            id = service.Create(request).Id;
        }

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            var machine = service.GetById(id);

            Assert.NotNull(machine);
            AssertMachine(machine, request);
        }
    }

    [Fact]
    public void Update_works_correctly()
    {
        long id;

        VendingMachineCreateRequest createRequest = new()
        {
            Name = "Usual Vending Machine",
            Latitude = 10,
            Longitude = 25,
            City = "Nagoya",
            Category = "drinks",
            Source = "https://www.google.com/maps",
        };

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            id = service.Create(createRequest).Id;
        }

        VendingMachineUpdateRequest updateRequest = new()
        {
            Id = id,
            Name = "Updated Vending Machine",
            Latitude = 10,
            Longitude = 25,
            City = "Kyoto",
            Category = "drinks",
            Source = "https://www.google.com/maps",
        };

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            var machine = service.Update(updateRequest);

            Assert.NotNull(machine);
            Assert.Equal(machine.Id, updateRequest.Id);
            AssertMachine(machine, updateRequest);
        }
    }

    [Fact]
    public void Delete_works_correctly()
    {
        long id;

        VendingMachineCreateRequest createRequest = new()
        {
            Name = "Usual Vending Machine",
            Latitude = 10,
            Longitude = 25,
            City = "Nagoya",
            Category = "drinks",
            Source = "https://www.google.com/maps",
        };

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            id = service.Create(createRequest).Id;
        }

        using (var session = NHibernateSessionFactory.OpenSession())
        {
            var repository = new VendingMachineRepository(session);

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();

            var service = new VendingMachineService(repository, mapper);

            service.Delete(id);

            Assert.Null(service.GetById(id));
        }
    }

    private void AssertMachine(
        VendingMachineDto m,
        VendingMachineCreateRequest r)
    {
        Assert.Equal(r.Name, m.Name);
        Assert.Equal(r.Latitude, m.Latitude);
        Assert.Equal(r.Longitude, m.Longitude);
        Assert.Equal(r.City, m.City);
        Assert.Equal(r.Category, m.Category);
        Assert.Equal(r.Source, m.Source);
    }

    private void AssertMachine(VendingMachineDto m, VendingMachineUpdateRequest r)
    {
        Assert.Equal(r.Name, m.Name);
        Assert.Equal(r.Latitude, m.Latitude);
        Assert.Equal(r.Longitude, m.Longitude);
        Assert.Equal(r.City, m.City);
        Assert.Equal(r.Category, m.Category);
        Assert.Equal(r.Source, m.Source);
    }
}
