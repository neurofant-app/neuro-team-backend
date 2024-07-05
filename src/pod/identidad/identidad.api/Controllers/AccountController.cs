using contabee.identity.api.models;
using contabee.identity.api.viewmodels;
using identidad.api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace contabee.identity.api.Controllers;

[ApiController]
[Route("account")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;
    private readonly UserManager<ApplicationUserMongo> _userManager;
    //private readonly ApplicationDbContext _applicationDbContext;
    private static bool _databaseChecked;

    public AccountController(ILogger<AccountController> logger,
        UserManager<ApplicationUserMongo> userManager)
    {
        this.logger = logger;
        _userManager = userManager;
        //_applicationDbContext = applicationDbContext;
    }


    //[SwaggerOperation("Actualiza la contraseña de un usuario utilizando su email para la búsqueda")]
    //[SwaggerResponse(statusCode: 200, description: "La contraseña ha sido actualizada")]
    //[SwaggerResponse(statusCode: 404, description: "Cuenta no localizada")]
    //[HttpPost("password/token")]
    //public async Task<IActionResult> EstablecePasswordToken([FromBody] ActualizarContrasena actualizarContrasena)
    //{
    //    var user = await _userManager.FindByNameAsync(actualizarContrasena.Email);
    //    if (user == null)
    //    {
    //        logger.LogDebug($"Cuenta inexistente {actualizarContrasena.Email}");
    //        return NotFound();
    //    }

    //    var result = await _userManager.ResetPasswordAsync(user, actualizarContrasena.Token, actualizarContrasena.Password);
    //    if(result.Succeeded)
    //    {
    //        return Ok();
    //    }

    //    return BadRequest(result.Errors);
    //}


    //[SwaggerOperation("Busca una cuenta por emaul")]
    //[SwaggerResponse(statusCode: 200, description: "La cuenta existe y devuelve la entidad")]
    //[SwaggerResponse(statusCode: 404, description: "La cuenta no existe")]
    //[HttpGet("password/recuperar")]
    //public async Task<ActionResult<DTORecuperacionPassword>> RecuperaPasswordEmail([FromQuery] string email)
    //{
    //    var user = await _userManager.FindByNameAsync(email);
    //    if (user == null)
    //    {
    //        logger.LogDebug($"Cuenta inexistente {email}");
    //        return NotFound();
    //    }

    //    var token = await _userManager.GeneratePasswordResetTokenAsync(user!);

    //    DTORecuperacionPassword cuenta = new ()
    //    {
    //        Email = user!.Email,
    //        UserName = user!.UserName,
    //        TokenRecuperacion = token
    //    };
    //    return Ok(cuenta);
    //}


    [SwaggerOperation("Registra un nuevo usuario")]
    [SwaggerResponse(statusCode: 200, description: "Usuario Registrado satisfactoriamente")]
    [SwaggerResponse(statusCode: 500, description: "No se pudo registrar usuario")]
    [SwaggerResponse(statusCode: 409, description: "El usuario ya existe")]
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        //EnsureDatabaseCreated(_applicationDbContext);
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                logger.LogDebug($"Cuenta existente {model.Email}");
                return StatusCode(StatusCodes.Status409Conflict);
            }

            user = new ApplicationUserMongo { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
        }

        logger.LogDebug($"No es valido el usuario");
        return BadRequest(ModelState);
    }

    #region Helpers

    // The following code creates the database and schema if they don't exist.
    // This is a temporary workaround since deploying database through EF migrations is
    // not yet supported in this release.
    // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
    // when publishing your application.
    //private static void EnsureDatabaseCreated(ApplicationDbContext context)
    //{
    //    if (!_databaseChecked)
    //    {
    //        _databaseChecked = true;
    //        context.Database.EnsureCreated();
    //    }
    //}

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    #endregion
}
