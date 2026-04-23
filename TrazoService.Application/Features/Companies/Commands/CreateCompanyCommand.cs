using MediatR;
using TrazoService.Application.Interfaces.Repositories;
using TrazoService.Domain.Entities;

namespace TrazoService.Application.Features.Companies.Commands;

public record CreateCompanyCommand : IRequest<int>
{
    public string Code { get; set; }
    public string Type { get; set; }
    public string RazonSocial { get; set; }
    public string Nit { get; set; }
    public string LogoUrl { get; set; }
    public string Gps { get; set; }
    public DateTime OpeningDate { get; set; } 
    public string Email { get; set; }
    public string Address { get; set; }
    public string Contact { get; set; }
    public string PhotoUrl { get; set; }
    public string FundaEmpresa { get; set; }
    public string CategoryCod { get; set; }
    public string Status { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Details { get; set; }
    public string CommercialActivity { get; set; }
}

public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, int>
{
    private readonly ICompanyRepository _repository;

    public CreateCompanyHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Code = request.Code,
            Type = request.Type,
            RazonSocial = request.RazonSocial,
            Nit = request.Nit,
            LogoUrl = request.LogoUrl,
            Gps = request.Gps,
            OpeningDate = request.OpeningDate,
            Email = request.Email,
            Address = request.Address,
            Contact = request.Contact,
            PhotoUrl = request.PhotoUrl,
            FundaEmpresa = request.FundaEmpresa,
            CategoryCod = request.CategoryCod,
            Status = request.Status,
            Country = request.Country,
            City = request.City,
            Details = request.Details,
            CommercialActivity = request.CommercialActivity
        };

        await _repository.AddAsync(company);
        return company.Id;
    }
}
