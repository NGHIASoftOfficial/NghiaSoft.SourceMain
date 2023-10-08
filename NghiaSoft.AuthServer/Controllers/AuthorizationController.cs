using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace NghiaSoft.AuthServer.Controllers;

public class AuthorizationController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthorizationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }


    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var user = await _userManager.GetUserAsync(User);

        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Check if the user is authenticated and return a challenge if not
        if (user == null)
        {
            return Challenge(
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + Request.QueryString
                });
        }

        // Signing in with the OpenIddict authentication scheme triggers OpenIddict to issue a code.
        return SignIn(CreateClaimsPrincipal(request, user),
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        ClaimsPrincipal claimsPrincipal;

        if (request.IsPasswordGrantType())
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            claimsPrincipal = CreateClaimsPrincipal(request, user,
                new Claim(OpenIddictConstants.Claims.Audience,
                    request.ClientId ?? throw new InvalidOperationException()).SetDestinations(
                    OpenIddictConstants.Destinations.AccessToken));
        }

        else if (request.IsClientCredentialsGrantType())
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.

            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Subject (sub) is a required field, we use the client id as the subject identifier here.
            _ = identity.AddClaim(OpenIddictConstants.Claims.Subject,
                request.ClientId ?? throw new InvalidOperationException(),
                OpenIddictConstants.Destinations.AccessToken);
            _ = identity.AddClaim(OpenIddictConstants.Claims.Audience,
                request.ClientId ?? throw new InvalidOperationException(),
                OpenIddictConstants.Destinations.AccessToken);

            claimsPrincipal = new ClaimsPrincipal(identity);

            claimsPrincipal.SetScopes(request.GetScopes());
        }

        else if (request.IsAuthorizationCodeGrantType())
        {
            // Retrieve the claims principal stored in the authorization code
            claimsPrincipal =
                (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                .Principal ?? throw new InvalidOperationException("13");
        }

        else if (request.IsRefreshTokenGrantType())
        {
            // Retrieve the claims principal stored in the refresh token.
            claimsPrincipal =
                (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                .Principal ?? throw new InvalidOperationException("14");
        }

        else
        {
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo")]
    public async Task<IActionResult> Userinfo()
    {
        var claimsPrincipal =
            (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

        return Ok(new
        {
            Subject = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
            Name = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Name),

            // GivenName = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.GivenName),
            // MiddleName = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.MiddleName),
            // FamilyName = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.FamilyName),
            // Nickname = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Nickname),
            EmailName = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Email),
            PhoneNumber = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.PhoneNumber),
            EmailVerified = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.EmailVerified),
            PhoneNumberVerified = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.PhoneNumberVerified),
            // Birthdate = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Birthdate),
            // Gender = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Gender),
        });
    }

    private ClaimsPrincipal CreateClaimsPrincipal(OpenIddictRequest request, IdentityUser user, params Claim[] more)
    {
        // Create a new claims principal
        var claims = new List<Claim>
        {
            // new Claim(OpenIddictConstants.Claims.GivenName, user.UserName ?? "").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            // new Claim(OpenIddictConstants.Claims.MiddleName, user.UserName ?? "").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            // new Claim(OpenIddictConstants.Claims.FamilyName, user.UserName ?? "").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            // new Claim(OpenIddictConstants.Claims.Nickname, user.UserName ?? "").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.Email, user.Email ?? "").SetDestinations(OpenIddictConstants
                .Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.EmailVerified, user.EmailConfirmed.ToString()).SetDestinations(
                OpenIddictConstants.Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.PhoneNumber, user.PhoneNumber ?? "").SetDestinations(
                OpenIddictConstants.Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString())
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            // new Claim(OpenIddictConstants.Claims.Birthdate, user.Birthday?.ToString("MM/dd/yyyy") ?? "").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            //new Claim(OpenIddictConstants.Claims.Gender, user.Gender.ToString()).SetDestinations(OpenIddictConstants
            //    .Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.Name, user.UserName).SetDestinations(
                OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken),
            new(OpenIddictConstants.Claims.Subject, user.Id),
        };

        claims.AddRange(more);

        var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Set requested scopes (this is not done automatically)
        claimsPrincipal.SetScopes(request.GetScopes());

        return claimsPrincipal;
    }
}