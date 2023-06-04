using DAL.Contracts;
using DAL.DbContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class DogRepository : IDogRepository
{
    private readonly DbContextBase _dbContext;
    private readonly DbSet<Dog> _dogs;

    public DogRepository(DbContextBase dbContext)
    {
        _dbContext = dbContext;
        _dogs = _dbContext.Set<Dog>();
    }

    public void Create(Dog dog)
    {
        var dogWithName = _dogs.FirstOrDefault(x => x.Name == dog.Name);
        if (dogWithName is not null)
            throw new ArgumentException("DOG_WITH_THE_SAME_NAME_ALREADY_EXISTS");

        _dogs.Add(dog);
        _dbContext.Commit();
    }

    public IQueryable<Dog> GetAll()
    {
        return _dogs.AsQueryable();
    }
}
