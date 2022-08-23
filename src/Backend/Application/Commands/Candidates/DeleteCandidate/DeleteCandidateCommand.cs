using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Candidates.DeleteCandidate
{

    public class DeleteCandidateCommand : IRequest<IActionResult>
    {
        public int CandidateId { get; set; }
    }

    public class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteCandidateCommandHandler> _logger;

        public DeleteCandidateCommandHandler(IApplicationDbContext context, ILogger<DeleteCandidateCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("DeleteCandidateCommandHandler created successfully");
        }

        public async Task<IActionResult> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            Candidate candidate = new();

            try
            {
                candidate = await _context.Candidates.Where(c => c.Id == request.CandidateId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            if(candidate == null)
            {
                _logger.LogError($"Candidate with {request.CandidateId} Id is trying to be removed, but this resource doesn't exist");
                return new BadRequestObjectResult("Bad request");
            }

            _context.Candidates.Remove(candidate);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ObjectResult(ex.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            _logger.LogInformation($"Candidate of {request.CandidateId} has been removed");

            return new OkObjectResult("");
        }
    }
}
