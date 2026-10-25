using NihonVending.Domain.DTOs;

namespace NihonVending.Services;

public interface IVendingMachineService
{
    VendingMachineDto Create(VendingMachineCreateRequest request);

    IReadOnlyList<VendingMachineDto> GetAll();

    VendingMachineDto? GetById(long id);

    VendingMachineDto Update(VendingMachineUpdateRequest request);

    void Delete(long id);
}
