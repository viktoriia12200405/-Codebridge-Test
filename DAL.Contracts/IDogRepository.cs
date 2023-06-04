using Domain.Models;

namespace DAL.Contracts;
public interface IDogRepository
{
    IQueryable<Dog> GetAll();

    void Create(Dog dog);
}
