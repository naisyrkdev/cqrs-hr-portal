using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Candidates.GetSingleCandidate
{
    public class GetSingleCandidateQuery : IRequest<IActionResult>
    {
        public int CandidateId { get; set; }
    }

    public class GetSingleCandidateQueryHandler : IRequestHandler<GetSingleCandidateQuery, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetSingleCandidateQueryHandler> _logger;

        public GetSingleCandidateQueryHandler(IApplicationDbContext context, ILogger<GetSingleCandidateQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("GetSingleCandidateQueryHandler created successfully");
        }

        public async Task<IActionResult> Handle(GetSingleCandidateQuery request, CancellationToken cancellationToken)
        {
            Candidate candidate = new();

            candidate = await _context.Candidates.Where(c => c.Id == request.CandidateId).AsNoTracking().FirstOrDefaultAsync();

            if(candidate == null)
            {
                _logger.LogError($"Candidate that not exist is trying to be accessed");
                return new BadRequestResult();
            }

            _logger.LogInformation($"Candidate has been received succesfully");

            return new OkObjectResult(candidate);
        }
    }

}
