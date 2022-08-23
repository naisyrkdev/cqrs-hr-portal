using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Candidates.GetCandidatesList
{
    public class GetCandidatesListQuery : IRequest<IActionResult> 
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string FullNameFilter { get; set; }
    }

    public class GetCandidatesListQueryHandler : IRequestHandler<GetCandidatesListQuery, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetCandidatesListQueryHandler> _logger;

        public GetCandidatesListQueryHandler(IApplicationDbContext context, ILogger<GetCandidatesListQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("GetCandidatesListQueryHandler created successfully");
        }

        public async Task<IActionResult> Handle(GetCandidatesListQuery request, CancellationToken cancellationToken)
        {
            List<Candidate> allCandidates = new();

            allCandidates = await _context.Candidates.Where(c => (c.FirstName + " " + c.LastName).Contains(request.FullNameFilter)).AsNoTracking().ToListAsync();

            List<Candidate> paginatedCandidates = allCandidates.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            PaginationWrapper<List<Candidate>> result = new()
            {
                TotalRecords = allCandidates.Count,
                Data = paginatedCandidates

            };

            _logger.LogInformation($"Candidates has been received succesfully");

            return new OkObjectResult(result);
        }
    }
}
