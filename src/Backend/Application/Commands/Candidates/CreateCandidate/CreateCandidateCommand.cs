using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Candidates.CreateCandidate
{
    public class CreateCandidateCommand : IRequest<IActionResult>
    {
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateCandidateCommandHandler> _logger;

        public CreateCandidateCommandHandler(IApplicationDbContext context, ILogger<CreateCandidateCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("CreateCandidateCommandHandler created successfully");
        }

        public async Task<IActionResult> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
        {
            Candidate candidate = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            try
            {
                await _context.Candidates.AddAsync(candidate);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ObjectResult(ex.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            _logger.LogInformation($"Candidate {candidate.FirstName} {candidate.LastName} has been created succesfully");

            return new OkObjectResult(candidate.Id);
        }
    }
}
