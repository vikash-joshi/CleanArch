

using MediatR;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly IUnitOfWork unitOfWork;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<User>> Handle(GetAllUsersQuery com, CancellationToken cancellationToken)
    {
        var users = await unitOfWork.Users.GetUsersAsync(com.Role, cancellationToken);
        
        return users.ToList();
    }
    
}