﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using contabee.identity.api.helpers;
using contabee.identity.api.models;
using identidad.api;
using identidad.api.models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace contabee.identity.api.Controllers;

public class AuthorizationController : Controller
{
    private readonly ILogger<AuthorizationController> logger;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IDependencyResolver _dependencyResolver;
    private readonly IConfiguration _configuration;
    public AuthorizationController(
        ILogger<AuthorizationController> logger,
        IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager,
        IDependencyResolver dependencyResolver,
        IConfiguration configuration)
    {
        this.logger = logger;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        _dependencyResolver = dependencyResolver;
        _configuration = configuration;
    }
    /// <summary>
    /// Gnenera un JWT
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    [SwaggerOperation("Genera JWT")]
    [SwaggerResponse(statusCode: 200, description: "La confirmación ha ocurrido satisfactoriamente")]
    [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request.IsClientCredentialsGrantType())
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.

            var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
            if (application == null)
            {
                 logger.LogDebug("AuthorizationController -Exchange - resultado {code} ", CodigosErrores.AUTHORIZATIONCONTROLLER_CLIENTE_NO_LOCALIZADO);
                throw new InvalidOperationException("The application details cannot be found in the database.");
            }

            // Create the claims-based identity that will be used by OpenIddict to generate tokens.
            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
            identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
            identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

            // Note: In the original OAuth 2.0 specification, the client credentials grant
            // doesn't return an identity token, which is an OpenID Connect concept.
            //
            // As a non-standardized extension, OpenIddict allows returning an id_token
            // to convey information about the client application when the "openid" scope
            // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
            // scope is not explicitly set, no identity token is returned to the client application.

            // Set the list of scopes granted to the client application in access_token.
            identity.SetScopes(request.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(GetDestinations);

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsPasswordGrantType())
        {
            ClaimsIdentity identity = null;
            var dbtype = _configuration["dbtype"];
            logger.LogDebug("AuthorizationController - Register - DB {dbtype}", dbtype);
            switch (dbtype)
            {
                case "mysql":
                    var _userManager = _dependencyResolver.GetService<UserManager<ApplicationUser>>();
                    var user = await _userManager.FindByNameAsync(request.Username);
                    if (user == null)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The username/password couple is invalid."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_USERNAME_PASSWORD_NOVALIDOS);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }

                    // Validate the username/password parameters and ensure the account is not locked out.
                    var _signInManager = _dependencyResolver.GetService<SignInManager<ApplicationUser>>();
                    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                    if (!result.Succeeded)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The username/password couple is invalid."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_USERNAME_PASSWORD_NOVALIDOS);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }

                    // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                    identity = new ClaimsIdentity(
                        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                        nameType: Claims.Name,
                        roleType: Claims.Role);

                    // Add the claims that will be persisted in the tokens.
                    identity.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(user))
                            .SetClaim(Claims.Email, await _userManager.GetEmailAsync(user))
                            .SetClaim(Claims.Name, await _userManager.GetUserNameAsync(user))
                            .SetClaims(Claims.Role, (await _userManager.GetRolesAsync(user)).ToImmutableArray());
                    break;
                case "mongo":
                    var _userManagerMongo = _dependencyResolver.GetService<UserManager<ApplicationUserMongo>>();
                    var userMongo = await _userManagerMongo.FindByNameAsync(request.Username);
                    if (userMongo == null)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The username/password couple is invalid."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_USERNAME_PASSWORD_NOVALIDOS);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }

                    // Validate the username/password parameters and ensure the account is not locked out.
                    var _signInManagerMongo = _dependencyResolver.GetService<SignInManager<ApplicationUserMongo>>();
                    var resultMongo = await _signInManagerMongo.CheckPasswordSignInAsync(userMongo, request.Password, lockoutOnFailure: true);
                    if (!resultMongo.Succeeded)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The username/password couple is invalid."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_USERNAME_PASSWORD_NOVALIDOS);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }

                    // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                    identity = new ClaimsIdentity(
                        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                        nameType: Claims.Name,
                        roleType: Claims.Role);

                    // Add the claims that will be persisted in the tokens.
                    identity.SetClaim(Claims.Subject, await _userManagerMongo.GetUserIdAsync(userMongo))
                            .SetClaim(Claims.Email, await _userManagerMongo.GetEmailAsync(userMongo))
                            .SetClaim(Claims.Name, await _userManagerMongo.GetUserNameAsync(userMongo))
                            .SetClaims(Claims.Role, (await _userManagerMongo.GetRolesAsync(userMongo)).ToImmutableArray());
                    break;
            }

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            identity.SetScopes(request.GetScopes());
            identity.SetDestinations(GetDestinations);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        else if (request.IsRefreshTokenGrantType())
        {
            ClaimsIdentity identity = null;
            var dbtype = _configuration["dbtype"];
            logger.LogDebug("AuthorizationController - Register - DB {dbtype}", dbtype);
            switch (dbtype)
            {
                case "mysql":
                    // Retrieve the claims principal stored in the refresh token.
                    var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    var _userManager = _dependencyResolver.GetService<UserManager<ApplicationUser>>();
                    // Retrieve the user profile corresponding to the refresh token.
                    var user = await _userManager.FindByIdAsync(result.Principal.GetClaim(Claims.Subject));
                    if (user == null)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_TOKEN_REFRESCO_NOVALIDO);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }
                    var _signInManager = _dependencyResolver.GetService<SignInManager<ApplicationUser>>();

                    // Ensure the user is still allowed to sign in.
                    if (!await _signInManager.CanSignInAsync(user))
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_USUARIO_NOPUEDE_INICIAR_SESION);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }

                    identity = new ClaimsIdentity(result.Principal.Claims,
                        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                        nameType: Claims.Name,
                        roleType: Claims.Role);

                    // Override the user claims present in the principal in case they changed since the refresh token was issued.
                    identity.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(user))
                            .SetClaim(Claims.Email, await _userManager.GetEmailAsync(user))
                            .SetClaim(Claims.Name, await _userManager.GetUserNameAsync(user))
                            .SetClaims(Claims.Role, (await _userManager.GetRolesAsync(user)).ToImmutableArray());
                    break;
                case "mongo":
                    // Retrieve the claims principal stored in the refresh token.
                    var resultMongo = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    var _userManagerMongo = _dependencyResolver.GetService<UserManager<ApplicationUserMongo>>();
                    // Retrieve the user profile corresponding to the refresh token.
                    var userMongo = await _userManagerMongo.FindByIdAsync(resultMongo.Principal.GetClaim(Claims.Subject));
                    if (userMongo == null)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_TOKEN_REFRESCO_NOVALIDO);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }
                    var _signInManagerMongo = _dependencyResolver.GetService<SignInManager<ApplicationUserMongo>>();

                    // Ensure the user is still allowed to sign in.
                    if (!await _signInManagerMongo.CanSignInAsync(userMongo))
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                        });
                        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_USUARIO_NOPUEDE_INICIAR_SESION);
                        return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }

                    identity = new ClaimsIdentity(resultMongo.Principal.Claims,
                        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                        nameType: Claims.Name,
                        roleType: Claims.Role);

                    // Override the user claims present in the principal in case they changed since the refresh token was issued.
                    identity.SetClaim(Claims.Subject, await _userManagerMongo.GetUserIdAsync(userMongo))
                            .SetClaim(Claims.Email, await _userManagerMongo.GetEmailAsync(userMongo))
                            .SetClaim(Claims.Name, await _userManagerMongo.GetUserNameAsync(userMongo))
                            .SetClaims(Claims.Role, (await _userManagerMongo.GetRolesAsync(userMongo)).ToImmutableArray());
                    break;
            }

            identity.SetDestinations(GetDestinations);

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        logger.LogDebug("AuthorizationController - Exchange - resultad {code}", CodigosErrores.AUTHORIZATIONCONTROLLER_GRAND_TYPE_ESPECIFICADO_NOESTA_IMPLEMENTADO);
        throw new NotImplementedException("The specified grant type is not implemented.");
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}
