using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace SFStack.Server.Modules;

public class UpdateUserRequestDTO
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDTO>
{
    private readonly SFStackContext _context;

    public UpdateUserRequestValidator(SFStackContext context)
    {
        _context = context;

        When(u => u.Email != null, () =>
        {
            RuleFor(u => u.Email).NotEmpty()
                                 .WithMessage("L'adresse e-mail est obligatoire.")
                                 .EmailAddress()
                                 .WithMessage("L'adresse e-mail est invalide.");
        });
    }
}