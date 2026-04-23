using MediatR;
using TrazoService.Application.Features.Companies.DTOs;
using TrazoService.Application.Interfaces.Repositories;

namespace TrazoService.Application.Features.Companies.Queries;

public record GetAllCompaniesQuery() : IRequest<IEnumerable<CompanyDto>>;

public class GetAllCompaniesHandler : IRequestHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly ICompanyRepository _repository;

    public GetAllCompaniesHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _repository.GetAllAsync();
        
        return companies.Select(company => new CompanyDto
        {
            Id = company.Id,
            Code = company.Code,
            Type = company.Type,
            RazonSocial = company.RazonSocial,
            Nit = company.Nit,
            LogoUrl = company.LogoUrl,
            Gps = company.Gps,
            OpeningDate = company.OpeningDate,
            Email = company.Email,
            Address = company.Address,
            Contact = company.Contact,
            PhotoUrl = company.PhotoUrl,
            FundaEmpresa = company.FundaEmpresa,
            CategoryCod = company.CategoryCod,
            Status = company.Status,
            Country = company.Country,
            City = company.City,
            Details = company.Details,
            CommercialActivity = company.CommercialActivity
        });
    }
}
