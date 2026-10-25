using AutoMapper;
using NihonVending.Domain;
using NihonVending.Domain.DTOs;

namespace NihonVending.Infrastructure.Mappers;

public class VendingMachineProfile : Profile
{
    public VendingMachineProfile()
    {
        CreateMap<VendingMachine, VendingMachineDto>();
        CreateMap<VendingMachine, VendingMachineCreateRequest>();
        CreateMap<VendingMachine, VendingMachineUpdateRequest>();
    }
}
