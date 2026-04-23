using TrazoService.Domain.Entities;

namespace TrazoService.Application.Interfaces.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<Company?> GetByRazonSocialAsync(string razonSocial);
}
