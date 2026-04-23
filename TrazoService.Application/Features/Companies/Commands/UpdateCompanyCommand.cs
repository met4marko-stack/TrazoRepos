using MediatR;
using TrazoService.Application.Interfaces.Repositories;

namespace TrazoService.Application.Features.Companies.Commands;

public record UpdateCompanyCommand : IRequest<bool>
{
    public int Id { get; set; }
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

public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, bool>
{
    private readonly ICompanyRepository _repository;

    public UpdateCompanyHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id);
        
        if (company == null) return false;

        company.Code = request.Code;
        company.Type = request.Type;
        company.RazonSocial = request.RazonSocial;
        company.Nit = request.Nit;
        company.LogoUrl = request.LogoUrl;
        company.Gps = request.Gps;
        company.OpeningDate = request.OpeningDate;
        company.Email = request.Email;
        company.Address = request.Address;
        company.Contact = request.Contact;
        company.PhotoUrl = request.PhotoUrl;
        company.FundaEmpresa = request.FundaEmpresa;
        company.CategoryCod = request.CategoryCod;
        company.Status = request.Status;
        company.Country = request.Country;
        company.City = request.City;
        company.Details = request.Details;
        company.CommercialActivity = request.CommercialActivity;
        
        company.UpdateTimestamp();

        await _repository.UpdateAsync(company);
        return true;
    }
}
