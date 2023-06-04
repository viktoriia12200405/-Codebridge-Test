using AutoMapper;
using BLL.Contracts;
using BLL.Extensions;
using BLL.Models.Models;
using BLL.Models.Params;
using DAL.Contracts;
using Domain.Models;
using Common.Extensions;

namespace BLL.Services;
public class DogService : IDogService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;

    public DogService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void Create(DogDTO dogDTO)
    {
        var dog = _mapper.Value.Map<Dog>(dogDTO);

        _unitOfWork.Value.Dogs.Value.Create(dog);
    }

    public async Task<IEnumerable<DogDTO>> List(FilterParams filterParams)
    {
        var task = Task.Factory.StartNew(() =>
        {
            var dogs = _unitOfWork.Value.Dogs.Value.GetAll();
            if (!filterParams.Attribute.IsNullOrEmpty())
                dogs = dogs.OrderByAttribute(filterParams.Attribute, filterParams.Order);

            var dogsList = dogs.GetPage(filterParams).ToList();

            return _mapper.Value.Map<List<DogDTO>>(dogsList);
        });

        return await task;
    }
}
