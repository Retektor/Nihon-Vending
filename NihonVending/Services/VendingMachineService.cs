using AutoMapper;
using NihonVending.Domain;
using NihonVending.Domain.DTOs;
using NihonVending.Repositories;

namespace NihonVending.Services;

public class VendingMachineService(
    IVendingMachineRepository repo,
    IMapper mapper) : IVendingMachineService
{
    private readonly IVendingMachineRepository _repo = repo;
    private readonly IMapper _mapper = mapper;

    public VendingMachineDto Create(VendingMachineCreateRequest request)
    {
        request.Validate();

        var machine = _mapper.Map<VendingMachine>(request);

        machine = _repo.Create(machine);

        return _mapper.Map<VendingMachineDto>(machine);
    }

    public IReadOnlyList<VendingMachineDto> GetAll()
    {
        IReadOnlyList<VendingMachine> fetchedItemsList = _repo.GetAll();
        List<VendingMachineDto> dtoList = _mapper.Map<List<VendingMachineDto>>(fetchedItemsList);

        IReadOnlyList<VendingMachineDto> readOnlyDtoList = dtoList.AsReadOnly();

        return readOnlyDtoList;
    }

    public VendingMachineDto? GetById(long id)
    {
        VendingMachine? fetchedMachine = _repo.GetById(id);

        if (fetchedMachine is null) return null;

        return _mapper.Map<VendingMachineDto>(fetchedMachine);
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

        return _mapper.Map<VendingMachineDto>(returnedMachine);
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
