using System.Net.Mime;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcoGuardian_Backend.IAM.Domain.Services;
using EcoGuardian_Backend.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using EcoGuardian_Backend.IAM.Interfaces.REST.Resources;
using EcoGuardian_Backend.IAM.Interfaces.REST.Transform;
using EcoGuardian_Backend.Shared.Application.Helper;
using EcoGuardian_Backend.Shared.Application.Internal.CloudinaryStorage;
using EcoGuardian_Backend.Shared.Infrastructure.Cloudinary;
using Microsoft.AspNetCore.Mvc;

namespace EcoGuardian_Backend.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(IUserCommandService userCommandService, ICloudinaryStorage cloudinaryStorage) : ControllerBase
{

    [Obsolete("This endpoint is deprecated. Use Auth0 for authentication.")]
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var authenticatedUser = await userCommandService.Handle(signInCommand);
        var resource =
            AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                authenticatedUser.token);
        return Ok(resource);
    }

    [Obsolete("This endpoint is deprecated. Use Auth0 for user registration.")]
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        var user =  await userCommandService.Handle(signUpCommand);
        if (user == null)
        {
            return BadRequest("User creation failed.");
        }
        return Ok(new { message = "Usuario creado correctamente", userId = user.Id });
    }
}