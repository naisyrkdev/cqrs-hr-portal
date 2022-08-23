using Application.Commands.Candidates.CreateCandidate;
using Application.Commands.Candidates.DeleteCandidate;
using Application.Commands.Candidates.UpdateCandidate;
using Application.Queries.Candidates.GetCandidatesList;
using Application.Queries.Candidates.GetSingleCandidate;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("candidates")]
    [ApiController]
    public class CandidatesController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleCandidate(int id)
        {
            return await Mediator.Send(new GetSingleCandidateQuery() { CandidateId = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            return await Mediator.Send(new DeleteCandidateCommand() { CandidateId = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] UpdateCandidateCommandDto commandDto)
        {
            return await Mediator.Send(new UpdateCandidateCommand() { Id = id, FirstName = commandDto.FirstName, LastName = commandDto.LastName,
                City = commandDto.City, Email = commandDto.Email, PhoneNumber = commandDto.PhoneNumber});
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetCandidatesList([FromBody] GetCandidatesListQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
