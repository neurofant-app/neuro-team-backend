using contabee.identity.api.models;
using contabee.identity.api.viewmodels;
using identidad.api;
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
    private readonly IConfiguration _configuration;
    private static bool _databaseChecked;
    private readonly IDependencyResolver _dependencyResolver;

    public AccountController(ILogger<AccountController> logger,
        IDependencyResolver dependencyResolver,
        IConfiguration configuration)
    {
        this.logger = logger;
        _dependencyResolver = dependencyResolver;
        _configuration = configuration;
    }


    [SwaggerOperation("Actualiza la contraseña de un usuario utilizando su email para la búsqueda")]
    [SwaggerResponse(statusCode: 200, description: "La contraseña ha sido actualizada")]
    [SwaggerResponse(statusCode: 404, description: "Cuenta no localizada")]
    [HttpPost("password/token")]
    public async Task<IActionResult> EstablecePasswordToken([FromBody] ActualizarContrasena actualizarContrasena)
    {
        IdentityResult result = new();
        var dbtype = _configuration["dbtype"];
        switch (dbtype)
        {
            case "mysql":
                var _userManager = _dependencyResolver.GetService<UserManager<ApplicationUser>>();
                var user = await _userManager.FindByNameAsync(actualizarContrasena.Email);
                if (user == null)
                {
                    logger.LogDebug($"Cuenta inexistente {actualizarContrasena.Email}");
                    return NotFound();
                }

                result = await _userManager.ResetPasswordAsync(user, actualizarContrasena.Token, actualizarContrasena.Password);
                break;
            case "mongo":
                var _userManagerMongo = _dependencyResolver.GetService<UserManager<ApplicationUserMongo>>();
                var userMongo = await _userManagerMongo.FindByNameAsync(actualizarContrasena.Email);
                if (userMongo == null)
                {
                    logger.LogDebug($"Cuenta inexistente {actualizarContrasena.Email}");
                    return NotFound();
                }

                result = await _userManagerMongo.ResetPasswordAsync(userMongo, actualizarContrasena.Token, actualizarContrasena.Password);
                break;
        }
        
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }


    [SwaggerOperation("Busca una cuenta por emaul")]
    [SwaggerResponse(statusCode: 200, description: "La cuenta existe y devuelve la entidad")]
    [SwaggerResponse(statusCode: 404, description: "La cuenta no existe")]
    [HttpGet("password/recuperar")]
    public async Task<ActionResult<DTORecuperacionPassword>> RecuperaPasswordEmail([FromQuery] string email)
    {
        DTORecuperacionPassword cuenta = new();
        var dbtype = _configuration["dbtype"];
        switch (dbtype)
        {
            case "mysql":
                var _userManager = _dependencyResolver.GetService<UserManager<ApplicationUser>>();
                var user = await _userManager.FindByNameAsync(email);
                if (user == null)
                {
                    logger.LogDebug($"Cuenta inexistente {email}");
                    return NotFound();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user!);

                DTORecuperacionPassword cuentaMysql = new()
                {
                    Email = user!.Email,
                    UserName = user!.UserName,
                    TokenRecuperacion = token
                };
                cuenta = cuentaMysql;
                break;
            case "mongo":
                var _userManagerMongo = _dependencyResolver.GetService<UserManager<ApplicationUserMongo>>();
                var userMongo = await _userManagerMongo.FindByNameAsync(email);
                if (userMongo == null)
                {
                    logger.LogDebug($"Cuenta inexistente {email}");
                    return NotFound();
                }

                var tokenMongo = await _userManagerMongo.GeneratePasswordResetTokenAsync(userMongo!);

                DTORecuperacionPassword cuentaMongo = new()
                {
                    Email = userMongo!.Email,
                    UserName = userMongo!.UserName,
                    TokenRecuperacion = tokenMongo
                };
                cuenta = cuentaMongo;
                break;
        }
        
        return Ok(cuenta);
    }


    [SwaggerOperation("Registra un nuevo usuario")]
    [SwaggerResponse(statusCode: 200, description: "Usuario Registrado satisfactoriamente")]
    [SwaggerResponse(statusCode: 500, description: "No se pudo registrar usuario")]
    [SwaggerResponse(statusCode: 409, description: "El usuario ya existe")]
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var dbtype = _configuration["dbtype"];
        if (dbtype.Equals("mysql"))
        {
            var _applicationDbContext = _dependencyResolver.GetService<ApplicationDbContext>();
            EnsureDatabaseCreated(_applicationDbContext);
        }

        if (ModelState.IsValid)
        {
            IdentityResult identityResult = new();
            switch (dbtype)
            {
                case "mysql":
                    var _userManager = _dependencyResolver.GetService<UserManager<ApplicationUser>>();
                    var user = await _userManager.FindByNameAsync(model.Email);
                    if (user != null)
                    {
                        logger.LogDebug($"Cuenta existente {model.Email}");
                        return StatusCode(StatusCodes.Status409Conflict);
                    }

                    user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    identityResult = await _userManager.CreateAsync(user, model.Password);
                    break;
                case "mongo":
                    var _userManagerMongo = _dependencyResolver.GetService<UserManager<ApplicationUserMongo>>();
                    var userMongo = await _userManagerMongo.FindByNameAsync(model.Email);
                    if (userMongo != null)
                    {
                        logger.LogDebug($"Cuenta existente {model.Email}");
                        return StatusCode(StatusCodes.Status409Conflict);
                    }

                    userMongo = new ApplicationUserMongo { UserName = model.Email, Email = model.Email };
                    identityResult = await _userManagerMongo.CreateAsync(userMongo, model.Password);
                    break;
            }
            if (identityResult.Succeeded)
            {
                return Ok();
            }
            AddErrors(identityResult);
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
    private static void EnsureDatabaseCreated(ApplicationDbContext context)
    {
        if (!_databaseChecked)
        {
            _databaseChecked = true;
            context.Database.EnsureCreated();
        }
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    #endregion
}
