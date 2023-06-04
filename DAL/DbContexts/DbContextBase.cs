using Common.Configs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DbContexts;
public class DbContextBase : DbContext
{
    private static bool? _isForTests;

    public DbSet<Dog> Dogs { get; set; }

    public DbContextBase(ConnectionStringModel model)
        : base(string.IsNullOrEmpty(model?.ConnectingString)
            ? new DbContextOptionsBuilder()
                .UseInMemoryDatabase("TestDb")
                .Options
            : SqlServerDbContextOptionsExtensions
                .UseSqlServer(
                    new DbContextOptionsBuilder(),
                    model.ConnectingString
                ).Options
        )
    {
        if (!_isForTests.HasValue)
            _isForTests = string.IsNullOrEmpty(model?.ConnectingString);

        if (_isForTests.Value == true)
            base.Database.EnsureCreated();
    }

    public void Commit()
    {
        SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        if (_isForTests != true)
            return;

        builder.Entity<Dog>().HasData(
            new Dog()
            {
                Name = "Luntik",
                Color = "Red",
                TailLength = 10,
                Weight = 80
            },
            new Dog()
            {
                Name = "Doggy",
                Color = "Purple",
                TailLength = 15,
                Weight = 33
            },
            new Dog()
            {
                Name = "Kesha",
                Color = "Black",
                TailLength = 20,
                Weight = 5
            },
            new Dog()
            {
                Name = "Vupsen",
                Color = "Green",
                TailLength = 50,
                Weight = 50
            });
    }
}
