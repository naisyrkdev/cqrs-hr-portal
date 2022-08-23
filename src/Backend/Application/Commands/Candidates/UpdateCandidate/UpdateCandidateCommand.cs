using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Candidates.UpdateCandidate
{
    public class UpdateCandidateCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UpdateCandidateCommandHandler> _logger;

        public UpdateCandidateCommandHandler(IApplicationDbContext context, ILogger<UpdateCandidateCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("UpdateCandidateCommandHandler created successfully");
        }

        public async Task<IActionResult> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
        {
            Candidate candidate = new();

            try
            {
                candidate = await _context.Candidates.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult("");
            }

            if(candidate == null)
            {
                return new BadRequestObjectResult("");
            }

            candidate.FirstName = request.FirstName;
            candidate.LastName = request.LastName;
            candidate.City = request.City;
            candidate.Email = request.Email;
            candidate.PhoneNumber = request.PhoneNumber;  

            _context.Candidates.Update(candidate);

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

            _logger.LogInformation($"Candidate {candidate.FirstName} {candidate.LastName} has been updated succesfully");

            return new OkObjectResult(candidate.Id);
        }
    }
}
