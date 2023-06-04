namespace DAL.Contracts;
public interface IUnitOfWork
{
    Lazy<IDogRepository> Dogs { get; }
}
