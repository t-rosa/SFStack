using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace SFStack.Server.Modules;

public class CreateUserRequestDTO
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDTO>
{
    private readonly SFStackContext _context;

    public CreateUserRequestValidator(SFStackContext context)
    {
        _context = context;

        RuleFor(u => u.Password).NotEmpty()
                                .WithMessage("Le mot de passe est obligatoire.")
                                .MinimumLength(8)
                                .WithMessage("Le mot de passe doit contenir au moins 8 caractères.")
                                .Matches("[A-Z]")
                                .WithMessage("Le mot de passe doit contenir au moins une lettre majuscule.")
                                .Matches("[a-z]")
                                .WithMessage("Le mot de passe doit contenir au moins une lettre minuscule.")
                                .Matches("[0-9]")
                                .WithMessage("Le mot de passe doit contenir au moins un chiffre.")
                                .Matches("[^a-zA-Z0-9]")
                                .WithMessage("Le mot de passe doit contenir au moins un caractère spécial.");

        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
                             .NotEmpty()
                             .WithMessage("L'adresse e-mail est obligatoire.")
                             .EmailAddress()
                             .WithMessage("L'adresse e-mail est invalide.")
                             .MustAsync(BeUnique)
                             .WithMessage("L'adresse e-mail est déjà utilisé.");
    }

    private async Task<bool> BeUnique(string email, CancellationToken token)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email, cancellationToken: token) == null;
    }
}