using FluentValidation;

namespace SFStack.Server.Modules;

public class LoginRequestDTO
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class LoginRequestValidator : AbstractValidator<LoginRequestDTO>
{
    public LoginRequestValidator()
    {
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

        RuleFor(u => u.Email).NotEmpty()
                             .WithMessage("L'adresse e-mail est obligatoire.")
                             .EmailAddress()
                             .WithMessage("L'adresse e-mail est invalide.");
    }
}