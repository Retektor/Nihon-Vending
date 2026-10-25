using AutoMapper;
using NihonVending.Domain;
using NihonVending.Domain.DTOs;
using NihonVending.Repositories;

namespace NihonVending.Services;

// TODO: REFACTOR VARIABLE NAMES FOR MORE CONSISTENCY
// TODO: REFACTOR IDENTATION FOR MORE CONSISTENCY
public class VendingMachineService : IVendingMachineService
{
    private readonly IVendingMachineRepository _repo;
    private readonly IMapper _mapper;

    public VendingMachineService(
        IVendingMachineRepository repo,
        IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public VendingMachineDto Create(VendingMachineCreateRequest request)
    {
        request.Validate();

        // TODO: CHANGE TO AUTOMAPPER
        var machine = new VendingMachine
        {
            Name = request.Name,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            City = request.City,
            Category = request.Category,
            Source = request.Source
        };

        machine = _repo.Create(machine);

        return new VendingMachineDto
        {
            // TODO: CHANGE TO AUTOMAPPER
            Id = machine.Id,
            Name = machine.Name,
            Latitude = machine.Latitude,
            Longitude = machine.Longitude,
            City = machine.City,
            Category = machine.Category,
            Source = machine.Source
        };
    }

    public IReadOnlyList<VendingMachineDto> GetAll()
    {
        IReadOnlyList<VendingMachine> fetchedItemsList = _repo.GetAll();
        List<VendingMachineDto> dtoList = new List<VendingMachineDto>();

        foreach (VendingMachine item in fetchedItemsList)
        {
            dtoList.Add(
                // TODO: CHANGE TO AUTOMAPPER
                new VendingMachineDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    City = item.City,
                    Category = item.Category,
                    Source = item.Source
                }
            );
        }

        IReadOnlyList<VendingMachineDto> readOnlyDtoList = dtoList.AsReadOnly();

        return readOnlyDtoList;
    }

    public VendingMachineDto? GetById(long id)
    {
        VendingMachine? fetchedMachine = _repo.GetById(id);

        if (fetchedMachine is null) return null;

        // TODO: CHANGE TO AUTOMAPPER
        return new VendingMachineDto
        {
            Id = fetchedMachine.Id,
            Name = fetchedMachine.Name,
            Latitude = fetchedMachine.Latitude,
            Longitude = fetchedMachine.Longitude,
            City = fetchedMachine.City,
            Category = fetchedMachine.Category,
            Source = fetchedMachine.Source
        };
    }

    public VendingMachineDto Update(VendingMachineUpdateRequest request)
    {
        request.Validate();

        long id = request.Id;

        if (_repo.GetById(id) is null)
            throw new KeyNotFoundException(
                $"Machine with ID {id} was not found."
            );

        VendingMachine machine = new()
        {
            Name = request.Name,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            City = request.City,
            Category = request.Category,
            Source = request.Source
        };

        VendingMachine returnedMachine = _repo.Update(id, machine);

        // TODO: CHANGE TO AUTOMAPPER
        return new VendingMachineDto
        {
            Id = returnedMachine.Id,
            Name = returnedMachine.Name,
            Latitude = returnedMachine.Latitude,
            Longitude = returnedMachine.Longitude,
            City = returnedMachine.City,
            Category = returnedMachine.Category,
            Source = returnedMachine.Source
        };
    }

    public void Delete(long id)
    {
        if (_repo.GetById(id) is null)
            throw new KeyNotFoundException(
                $"Machine with ID {id} was not found."
            );

        _repo.Delete(id);
    }
}
