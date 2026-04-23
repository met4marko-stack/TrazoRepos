using MediatR;
using TrazoService.Application.Interfaces.Repositories;

namespace TrazoService.Application.Features.Companies.Commands;

public record DeleteCompanyCommand(int Id) : IRequest<bool>;

public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, bool>
{
    private readonly ICompanyRepository _repository;

    public DeleteCompanyHandler(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id);
        
        if (company == null) return false;

        await _repository.DeleteAsync(company);
        return true;
    }
}
