using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Candidate> Candidates { get; }

        DbSet<Skill> Skills { get; }

        DbSet<CandidateSkill> CandidatesSkills { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
