using BLL.Contracts;
using BLL.Models.Models;
using Microsoft.AspNetCore.Mvc;
using BLL.Models.Params;

namespace WebAPI.Controllers;
[Route("")]
[ApiController]
public class DogsController : ControllerBase
{
    private readonly Lazy<IDogService> _dogService;

    public DogsController(Lazy<IDogService> dogService)
	{
        _dogService = dogService;
    }

    [HttpGet("dogs")]
    public async Task<ActionResult<List<DogDTO>>> List([FromQuery]FilterParams filterParams)
    {
        var dogs = await _dogService.Value.List(filterParams);

        return Ok(dogs);
    }

    [HttpPost("dog")]
    public ActionResult CreateDog(DogDTO dogDTO)
    {
        _dogService.Value.Create(dogDTO);

        return Ok();
    }
}
