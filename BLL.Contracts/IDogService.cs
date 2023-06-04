using BLL.Models.Models;
using BLL.Models.Params;

namespace BLL.Contracts;
public interface IDogService
{
    Task<IEnumerable<DogDTO>> List(FilterParams @filterParams);

    void Create(DogDTO dogDTO);
}
