using MediatR;
using ProductManagement.Domain.Entities;

public record GetAllUsersQuery(string Role):IRequest<List<User>>;