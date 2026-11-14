using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NihonVending.Domain;
using NihonVending.Domain.DTOs;
using NihonVending.Infrastructure.Mappers;
using NihonVending.Repositories;
using NihonVending.Services;

public class VendingMachineServiceTests
{
    [Fact]
    public void Create_works_correctly()
    {
        var vendingMachine = CreateTestVendingMachine();
        var request = new VendingMachineCreateRequest
        {
            Name = "Regular Vending Machine",
            Category = "snacks",
            Source = "https://www.google.com/maps"
        };

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.Create(It.IsAny<VendingMachine>()))
            .Returns(vendingMachine);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        service.Create(request);


        repository.Verify(
            r => r.Create(
                It.Is<VendingMachine>(machine =>
                    machine.Name == request.Name &&
                    machine.Category == request.Category &&
                    machine.Source == request.Source)),
            Times.Once
        );
    }

    [Theory]
    [InlineData("", "snacks", "https://www.google.com/maps")]
    [InlineData("Regular Vending Machine", "", "https://www.google.com/maps")]
    [InlineData("Regular Vending Machine", "snacks", "")]
    public void Create_throws_when_required_field_is_missing(
        string name,
        string category,
        string source)
    {
        var request = new VendingMachineCreateRequest
        {
            Name = name,
            Category = category,
            Source = source
        };

        var repository = new Mock<IVendingMachineRepository>();
        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper);

        Assert.Throws<ArgumentException>(
            () => service.Create(request));

        repository.Verify(
            r => r.Create(It.IsAny<VendingMachine>()),
            Times.Never);
    }

    [Fact]
    public void GetAll_works_correctly_with_2_objects()
    {
        var vendingMachine1 = CreateTestVendingMachine();
        var vendingMachine2 = CreateTestVendingMachine();
        vendingMachine2.Name = "Very Strange Vending Machine";

        IReadOnlyList<VendingMachine> machinesList =
        new List<VendingMachine>
        {
            vendingMachine1,
            vendingMachine2
        }
        .AsReadOnly();

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.GetAll())
            .Returns(machinesList);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        var result = service.GetAll();


        repository.Verify(
            r => r.GetAll(),
            Times.Once);

        Assert.Equal(machinesList.Count, result.Count);

        for (int i = 0; i < machinesList.Count; i++)
        {
            Assert.Equal(machinesList[i].Name, result[i].Name);
            Assert.Equal(machinesList[i].Category, result[i].Category);
            Assert.Equal(machinesList[i].Source, result[i].Source);
            Assert.Equal(machinesList[i].Latitude, result[i].Latitude);
            Assert.Equal(machinesList[i].Longitude, result[i].Longitude);
        }
    }

    [Fact]
    public void GetAll_works_correctly_with_empty()
    {
        IReadOnlyList<VendingMachine> machinesList =
        new List<VendingMachine> { }
        .AsReadOnly();

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.GetAll())
            .Returns(machinesList);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        var result = service.GetAll();


        repository.Verify(
            r => r.GetAll(),
            Times.Once);

        Assert.Empty(result);
    }

    [Fact]
    public void GetById_works_when_present()
    {
        var machine = CreateTestVendingMachine();

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.GetById(1))
            .Returns(machine);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        var result = service.GetById(1);


        repository.Verify(
            r => r.GetById(It.IsAny<long>()),
            Times.Once);

        Assert.NotNull(result);
        Assert.Equal(machine.Name, result.Name);
        Assert.Equal(machine.Category, result.Category);
        Assert.Equal(machine.Source, result.Source);
    }

    [Fact]
    public void GetById_works_correctly_when_not_present()
    {
        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.GetById(999))
            .Returns((VendingMachine?)null);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        var result = service.GetById(999);


        repository.Verify(
            r => r.GetById(It.IsAny<long>()),
            Times.Once);

        Assert.Null(result);
    }

    [Fact]
    public void Update_works_correctly()
    {
        var vendingMachine = CreateTestVendingMachine();
        var request = new VendingMachineUpdateRequest
        {
            Id = 1,
            Name = "Updated Vending Machine",
            Category = "drinks",
            Source = "https://www.google.com/maps"
        };

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.GetById(vendingMachine.Id))
            .Returns(vendingMachine);

        repository
            .Setup(r => r.Update(
                It.IsAny<long>(),
                It.IsAny<VendingMachine>()))
            .Returns(vendingMachine);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        service.Update(request);


        repository.Verify(
            r => r.GetById(request.Id),
        Times.Once);

        repository.Verify(
            r => r.Update(
                It.IsAny<long>(),
                It.Is<VendingMachine>(machine =>
                    machine.Name == request.Name &&
                    machine.Category == request.Category &&
                    machine.Source == request.Source)),
            Times.Once
        );
    }

    [Fact]
    public void Update_throws_when_object_not_present()
    {
        var request = new VendingMachineUpdateRequest
        {
            Id = 1,
            Name = "Updated Vending Machine",
            Category = "drinks",
            Source = "https://www.google.com/maps"
        };

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.GetById(request.Id))
            .Returns((VendingMachine?)null);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        Assert.Throws<KeyNotFoundException>(
            () => service.Update(request));

        repository.Verify(
            r => r.GetById(request.Id),
        Times.Once);

        repository.Verify(
        r => r.Update(
            It.IsAny<long>(),
            It.IsAny<VendingMachine>()),
        Times.Never);
    }

    [Theory]
    [InlineData(-999, "", "snacks", "https://www.google.com/maps")]
    [InlineData(1, "", "snacks", "https://www.google.com/maps")]
    [InlineData(1, "Regular Vending Machine", "", "https://www.google.com/maps")]
    [InlineData(1, "Regular Vending Machine", "snacks", "")]
    public void Update_throws_when_required_field_is_missing(
        long id,
        string name,
        string category,
        string source)
    {
        var request = new VendingMachineUpdateRequest
        {
            Id = id,
            Name = name,
            Category = category,
            Source = source
        };

        var repository = new Mock<IVendingMachineRepository>();

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        Assert.Throws<ArgumentException>(
            () => service.Update(request));


        repository.Verify(
            r => r.GetById(request.Id),
        Times.Never);

        repository.Verify(
        r => r.Update(
            It.IsAny<long>(),
            It.IsAny<VendingMachine>()),
        Times.Never);
    }

    [Fact]
    public void Delete_works_correctly()
    {
        long testId = 1;

        var repository = new Mock<IVendingMachineRepository>();
        repository
            .Setup(r => r.Delete(testId));

        var vendingMachine = CreateTestVendingMachine();

        repository
            .Setup(r => r.GetById(testId))
            .Returns(vendingMachine);

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        service.Delete(testId);


        repository.Verify(
            r => r.GetById(testId),
        Times.Once);

        repository.Verify(
            r => r.Delete(It.IsAny<long>()),
        Times.Once);
    }

    [Fact]
    public void Delete_throws_when()
    {
        var repository = new Mock<IVendingMachineRepository>();

        var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<VendingMachineProfile>(),
                NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var service = new VendingMachineService(
            repository.Object,
            mapper
        );


        Assert.Throws<KeyNotFoundException>(
            () => service.Delete(-1));


        repository.Verify(
            r => r.Delete(-1),
        Times.Never);

        repository.Verify(
            r => r.Delete(It.IsAny<long>()),
        Times.Never);
    }

    private VendingMachine CreateTestVendingMachine()
    {
        return new VendingMachine
        {
            Id = 1,
            Name = "Regular Vending Machine",
            Latitude = 1,
            Longitude = 2,
            City = "Osaka",
            Category = "snacks",
            Source = "https://www.google.com/maps",
        };
    }
}
