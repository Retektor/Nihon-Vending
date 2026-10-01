using NihonVending.Repositories;
using NihonVending.Domain;
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
        _session.Save(vendingMachine);
        return vendingMachine;
    }

    public IReadOnlyList<VendingMachine> GetAll()
    {
        return _session
            .Query<VendingMachine>()
            .ToList();
    }

    public VendingMachine? GetById(int id)
    {
        return _session.Get<VendingMachine>(id);
    }

    public VendingMachine Update(VendingMachine vendingMachine)
    {
        _session.Update(vendingMachine);
        return vendingMachine;
    }

    public void Delete(VendingMachine vendingMachine)
    {
        _session.Delete(vendingMachine);
    }
}
