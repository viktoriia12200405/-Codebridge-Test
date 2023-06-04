using DAL.Contracts;

namespace DAL.UnitOfWorks;
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(Lazy<IDogRepository> dogs)
    {
        Dogs = dogs;
    }

    public Lazy<IDogRepository> Dogs { get; }
}
