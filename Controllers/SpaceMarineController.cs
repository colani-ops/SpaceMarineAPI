using Microsoft.AspNetCore.Mvc;
using SpaceMarineAPI.Models;
using SpaceMarineAPI.Services;

namespace SpaceMarineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceMarineController : ControllerBase
    {
        private readonly SpaceMarineService _marineService;

        public SpaceMarineController(SpaceMarineService marineService)
        {
            _marineService = marineService;
        }


        //CREATE MARINE
        [HttpPost]
        public IActionResult CreateMarine([FromBody] SpaceMarine marine)
        {
            _marineService.AddMarine(marine);
            return Ok(new { message = "Space Marine created!" });
        }


        //GET MARINE FROM SQUAD
        [HttpGet("bysquad/{squadId}")]
        public IActionResult GetMarinesBySquad(int squadId)
        {
            var marines = _marineService.GetMarinesBySquad(squadId);
            return Ok(marines);
        }


        //UPDATE MARINE
        [HttpPut("{id}")]
        public IActionResult UpdateMarine(int id, [FromBody] SpaceMarine marine)
        {
            _marineService.UpdateMarine(id, marine);
            return Ok(new { message = "Marine updated!" });
        }


        //DELETE MARINE
        [HttpDelete("{id}")]
        public IActionResult DeleteMarine(int id)
        {
            _marineService.DeleteMarine(id);
            return Ok(new { message = "Marine deleted!" });
        }

        [HttpGet("rank/{experience}")]
        public IActionResult GetRank(int experience)
        {
            var rank = _marineService.GetRank(experience);
            return Ok(new { rank });
        }
    }
}
