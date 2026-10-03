using NihonVending.Domain;

namespace NihonVending.Repositories;

public interface IVendingMachineRepository
{
    VendingMachine Create(VendingMachine vendingMachine);

    IReadOnlyList<VendingMachine> GetAll();

    VendingMachine? GetById(long id);

    VendingMachine Update(long id, VendingMachine newMachineData);

    void Delete(long id);
}
