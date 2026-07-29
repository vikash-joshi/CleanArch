using FluentValidation;
using ProductManagement.Application.Commands;

namespace ProductManagement.Application.Validators;


public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{

    public CreateProductCommandValidator()
    {
        RuleFor(x=>x.Name)
        .NotEmpty().WithMessage("Name Is Required Validator")
        .MinimumLength(20).WithMessage("Name Cannot be Less than 20 Character");
        
    }
}
