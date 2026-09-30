using NihonVending.Domain;

namespace NihonVending.Repositories;

public interface IVendingMachineRepository
{
    void Create(VendingMachine vendingMachine);

    IReadOnlyList<VendingMachine> GetAll();

    VendingMachine GetById(int id);

    void Update(VendingMachine vendingMachine);

    void Delete(VendingMachine vendingMachine);
}
