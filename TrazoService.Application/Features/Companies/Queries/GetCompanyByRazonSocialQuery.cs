using MediatR;
using TrazoService.Application.Features.Companies.DTOs;
using TrazoService.Application.Interfaces.Repositories;

namespace TrazoService.Application.Features.Companies.Queries;

public record GetCompanyByRazonSocialQuery(string RazonSocial) : IRequest<CompanyDto?>;

public class GetCompanyByRazonSocialHandler : IRequestHandler<GetCompanyByRazonSocialQuery, CompanyDto?>
{
    private readonly ICompanyRepository _repository;

    public GetCompanyByRazonSocialHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<CompanyDto?> Handle(GetCompanyByRazonSocialQuery request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByRazonSocialAsync(request.RazonSocial);
        
        if (company == null) return null;

        return new CompanyDto
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
        };
    }
}
