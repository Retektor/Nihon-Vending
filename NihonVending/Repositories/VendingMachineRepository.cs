using NihonVending.Domain;
using NihonVending.Repositories;
using NHSession = NHibernate.ISession;

public class VendingMachineRepository : IVendingMachineRepository
{
    private readonly NHSession _session;

    public VendingMachineRepository(NHSession session)
    {
        _session = session;
    }

    public VendingMachine Create(VendingMachine vendingMachine)
    {
        using var transaction = _session.BeginTransaction();
        _session.Save(vendingMachine);
        transaction.Commit();
        return vendingMachine;
    }

    public IReadOnlyList<VendingMachine> GetAll()
    {
        return _session
            .Query<VendingMachine>()
            .ToList();
    }

    public VendingMachine? GetById(long id)
    {
        return _session.Get<VendingMachine>(id);
    }

    public VendingMachine Update(long id, VendingMachine newMachineData)
    {
        var machine = _session.Get<VendingMachine>(id) ?? throw new KeyNotFoundException(
            $"Vending machine with ID {id} was not found.");
        machine.Name = newMachineData.Name;
        machine.Latitude = newMachineData.Latitude;
        machine.Longitude = newMachineData.Longitude;
        machine.City = newMachineData.City;
        machine.Category = newMachineData.Category;
        machine.Source = newMachineData.Source;

        return machine;
    }

    public void Delete(long id)
    {
        var machine = _session.Load<VendingMachine>(id);
        _session.Delete(machine);
    }
}
