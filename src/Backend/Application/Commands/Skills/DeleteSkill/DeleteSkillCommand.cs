using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Skills.DeleteSkill
{

    public class DeleteSkillCommand : IRequest<IActionResult>
    {
        public int SkillId { get; set; }
    }

    public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, IActionResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteSkillCommandHandler> _logger;

        public DeleteSkillCommandHandler(IApplicationDbContext context, ILogger<DeleteSkillCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("DeleteSkillCommandHandler created successfully");
        }

        public async Task<IActionResult> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            Skill skill = new();
                
            try
            {
                skill = await _context.Skills.Where(s => s.Id == request.SkillId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult("");
            }

            if(skill == null)
            {
                _logger.LogError($"Resource of {request.SkillId} id doesn't exist");
                return new BadRequestObjectResult("Action failed.");
            }

            _context.Skills.Remove(skill);

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

            _logger.LogInformation($"Skill of {request.SkillId} Id has been deleted successfully");

            return new OkObjectResult("");
        }
    }
}
