using Microsoft.AspNetCore.Mvc;
using SpaceMarineAPI.Models;
using SpaceMarineAPI.Services;

namespace SpaceMarineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SquadController : ControllerBase
    {
        private readonly SquadService _squadService;

        public SquadController(SquadService squadService)
        {
            _squadService = squadService;
        }



        //CREATE SQUAD
        [HttpPost]
        public IActionResult CreateSquad([FromBody] Squad squad)
        {
            _squadService.AddSquad(squad);
            return Ok(new { message = "Squad created!" });
        }



        //GET SQUAD BY ID
        [HttpGet("{id}")]
        public IActionResult GetSquadById(int id)
        {
            var squad = _squadService.GetSquadById(id);
            if (squad == null)
                return NotFound();

            return Ok(squad);
        }



        //GET ALL SQUADS
        [HttpGet]
        public IActionResult GetAllSquads()
        {
            var squads = _squadService.GetAllSquads();
            return Ok(squads);
        }



        //UPDATE SQUAD
        [HttpPut("{id}")]
        public IActionResult UpdateSquad(int id, [FromBody] Squad squad)
        {
            _squadService.UpdateSquad(id, squad);
            return Ok(new { message = "Squad updated!" });
        }



        //DELETE SQUAD
        [HttpDelete("{id}")]
        public IActionResult DeleteSquad(int id)
        {
            _squadService.DeleteSquad(id);
            return Ok(new { message = "Squad deleted!" });
        }
    }
}
