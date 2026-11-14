using AutoMapper;
using NihonVending.Domain;
using NihonVending.Domain.DTOs;

namespace NihonVending.Infrastructure.Mappers;

public class VendingMachineProfile : Profile
{
    public VendingMachineProfile()
    {
        CreateMap<VendingMachine, VendingMachineDto>();
        CreateMap<VendingMachineDto, VendingMachine>();
        CreateMap<VendingMachine, VendingMachineCreateRequest>();
        CreateMap<VendingMachineCreateRequest, VendingMachine>();
        CreateMap<VendingMachine, VendingMachineUpdateRequest>();
        CreateMap<VendingMachineUpdateRequest, VendingMachine>();
    }
}
