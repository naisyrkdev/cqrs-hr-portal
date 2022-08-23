using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Skills.CreateSkill
{
    public class CreateSkillCommand : IRequest<IActionResult>
    {
        public string SkillName { get; set; }
    }

    public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateSkillCommandHandler> _logger;

        public CreateSkillCommandHandler(IApplicationDbContext context, ILogger<CreateSkillCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("CreateSkillCommandHandler created successfully");
        }

        public async Task<IActionResult> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            Skill checkIfSkillAlreadyExist = await _context.Skills.Where(s => s.SkillName == request.SkillName).AsNoTracking().FirstOrDefaultAsync();   
            
            if(checkIfSkillAlreadyExist != null)
            {
                return new BadRequestObjectResult("Skill of that name already exist");
            }

            Skill skill = new()
            {
                SkillName = request.SkillName
            };

            try
            {
                await _context.Skills.AddAsync(skill);
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

            _logger.LogInformation($"Skill {skill.SkillName} has been created successfully");

            return new OkObjectResult("");
        }
    }
}
