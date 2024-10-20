using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;

namespace SFStack.Server.Modules;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(SFStackContext context, IMapper mapper, ILogger<AuthenticationController> logger) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(GetUserResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request, [FromServices] IValidator<LoginRequestDTO> validator)
    {
        var validationResults = await validator.ValidateAsync(request);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults);
        }

        logger.LogInformation("Début du processus de connexion pour l'utilisateur avec l'email : {Email}", request.Email);

        var user = await context.User.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            logger.LogWarning("Aucun utilisateur trouvé avec l'e-mail : {Email}", request.Email);
            return NotFound();
        }

        var verification = new PasswordHasher<User>().VerifyHashedPassword(
            user,
            user.Password,
            request.Password
        );

        if (verification == PasswordVerificationResult.Failed)
        {
            logger.LogWarning("Échec de la vérification du mot de passe pour l'utilisateur avec l'e-mail : {Email}", request.Email);
            return Problem();
        }

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.SerialNumber, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        try
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5),
            });

            logger.LogInformation("Connexion réussie pour l'utilisateur avec l'ID : {IdUser}", user.Id);

            var userResponse = mapper.Map<GetUserResponseDTO>(user);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur survenue lors de la connexion de l'utilisateur avec ID : {IdUser}", user.Id);
            return StatusCode(500, "Une erreur est survenue lors de la connexion de l'utilisateur.");
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        logger.LogInformation("Début du processus de déconnexion de l'utilisateur");

        try
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            logger.LogInformation("Déconnexion réussie");
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la déconnexion de l'utilisateur");
            return StatusCode(500, "Une erreur est survenue lors de la déconnexion de l'utilisateur");
        }
    }
}