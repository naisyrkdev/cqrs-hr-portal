using Application.Commands.Skills.CreateSkill;
using Application.Commands.Skills.DeleteSkill;
using Application.Queries.Skills.GetSkillsList;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class SkillsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSkillsList()
        {
            return await Mediator.Send(new GetSkillsListQuery());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            return await Mediator.Send(new DeleteSkillCommand() { SkillId = id});
        }
    }
}
