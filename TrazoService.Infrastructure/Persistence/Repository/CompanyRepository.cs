using Microsoft.EntityFrameworkCore;
using TrazoService.Application.Interfaces.Repositories;
using TrazoService.Domain.Entities;
using TrazoService.Infrastructure.Persistence.Repository.Base;

namespace TrazoService.Infrastructure.Persistence.Repository;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Company?> GetByRazonSocialAsync(string razonSocial)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.RazonSocial == razonSocial);
    }
}
