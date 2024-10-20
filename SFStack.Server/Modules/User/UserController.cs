using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SFStack.Server.Modules;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(SFStackContext context, IMapper mapper, ILogger<UserController> logger) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetUserResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllUsers()
    {
        logger.LogInformation("Récupération de tous les utilisateurs");
        var users = context.User.AsNoTracking().Select(mapper.Map<GetUserResponseDTO>);
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetUserResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int id)
    {
        logger.LogInformation("Récupération de l'utilisateur avec ID : {IdUser}", id);

        var user = await context.User.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            logger.LogInformation("Utilisateur avec ID : {IdUser} non trouvé", id);
            return NotFound();
        }

        var userResponse = mapper.Map<GetUserResponseDTO>(user);
        return Ok(userResponse);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetUserResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request, [FromServices] IValidator<CreateUserRequestDTO> validator)
    {
        var validationResults = await validator.ValidateAsync(request);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults);
        }

        logger.LogInformation("Création l'utilisateur avec l'e-mail : {Email}", request.Email);
        var user = mapper.Map<User>(request);

        var passwordHasher = new PasswordHasher<User>();
        user.Password = passwordHasher.HashPassword(user, request.Password);

        context.User.Add(user);

        try
        {
            await context.SaveChangesAsync();
            var userResponse = mapper.Map<GetUserResponseDTO>(user);
            logger.LogInformation("Utilisateur avec l'e-mail : {Email} créé avec succès", request.Email);
            return CreatedAtAction(nameof(GetUserById), new { id = userResponse.Id }, userResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur survenue lors de la création de l'utilisateur");
            return StatusCode(500, "Une erreur est survenue lors de la création de l'utilisateur.");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GetUserResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDTO request, [FromServices] IValidator<UpdateUserRequestDTO> validator)
    {
        var validationResults = await validator.ValidateAsync(request);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults);
        }

        logger.LogInformation("Mise à jour de l'utilisateur avec ID : {IdUser}", id);
        var user = await context.User.FindAsync(id);

        if (user == null)
        {
            logger.LogWarning("Utilisateur avec ID : {IdUser} non trouvé", id);
            return NotFound();
        }

        logger.LogDebug("Mise à jour des détails de l'utilisateur pour ID : {IdUser}", id);
        context.Entry(user).CurrentValues.SetValues(request);

        try
        {
            await context.SaveChangesAsync();
            logger.LogInformation("Utilisateur avec ID : {IdUser} mis à jour avec succès", id);
            var userResponse = mapper.Map<GetUserResponseDTO>(user);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur survenue lors de la mise à jour de l'utilisateur avec ID : {IdUser}", id);
            return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'utilisateur.");
        }
    }

    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int[] ids)
    {
        logger.LogInformation("Suppression de(s) utilisateur(s) avec ID(s): {IdUser}", ids);

        try
        {
            await context.User.Where(u => ids.Contains(u.Id)).ExecuteDeleteAsync();
            logger.LogInformation("Utilisateur(s) supprimé avec succès");
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur survenue lors de la suppression de(s) utilisateur(s) avec ID : {IdUser}", ids);
            return StatusCode(500, "Une erreur est survenue lors de la suppression de(s) utilisateur(s).");
        }
    }
}