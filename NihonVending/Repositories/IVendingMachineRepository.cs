using NihonVending.Domain;

namespace NihonVending.Repositories;

public interface IVendingMachineRepository
{
    VendingMachine Create(VendingMachine vendingMachine);

    IReadOnlyList<VendingMachine> GetAll();

    VendingMachine? GetById(int id);

    VendingMachine Update(VendingMachine vendingMachine);

    void Delete(VendingMachine vendingMachine);
}
