using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Skills.GetSkillsList
{
    public class GetSkillsListQuery : IRequest<IActionResult> {}

    public class CreateSkillCommandHandler : IRequestHandler<GetSkillsListQuery, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateSkillCommandHandler> _logger;

        public CreateSkillCommandHandler(IApplicationDbContext context, ILogger<CreateSkillCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("CreateSkillCommandHandler created successfully");
        }

        public async Task<IActionResult> Handle(GetSkillsListQuery request, CancellationToken cancellationToken)
        {
            List<Skill> skills = new();

            skills = await _context.Skills.AsNoTracking().ToListAsync();

            _logger.LogInformation($"Skills has been received succesfully");

            return new OkObjectResult(skills);
        }
    }
}
