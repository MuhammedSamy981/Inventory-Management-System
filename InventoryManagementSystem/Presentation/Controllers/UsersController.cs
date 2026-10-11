using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Models;
using InventoryManagementSystem.Infrastructure.Identity;
using InventoryManagementSystem.Application.Interfaces;
using MediatR;
using InventoryManagementSystem.Application.Payments.Commands;
using InventoryManagementSystem.Application.Payments.Queries;
using InventoryManagementSystem.Application.Emails.Commands;
using Microsoft.AspNetCore.RateLimiting;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailService _courieremailService;
    private readonly IMediator _mediator;

    public UsersController(
        IMediator mediator,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService courieremailService,
        IJwtTokenService jwtTokenService)
    {
        _mediator = mediator;
        _courieremailService = courieremailService;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }


    [Authorize]
    [HttpGet("currentUserInfo")]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }

        var userId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        return Ok(new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!
        });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User"); // Assign default role
            bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (developmentEnvironment)
            {
                user.HasActiveSubscription = true;
                var updateResult = await _userManager.UpdateAsync(user);
                return updateResult.Succeeded ? Ok(new { Message = "Account created successfully" })
                           : BadRequest(new { Message = "Account creation failed" });
            }
            var command = new CreatePaymentOrderCommand(user.Id, 30, "$");
            var paymentOrderResult = await _mediator.Send(command);
            return Ok(new { OrderId = paymentOrderResult.orderId, Message = "Account created. Approve payment via PayPal to activate account" });

        }
        return BadRequest(result.Errors);
    }

    [EnableRateLimiting("AuthLimit")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (!developmentEnvironment)
            {
                var paymentExists = await _mediator.Send(new CheckPaymentStatusQuery(user.Id));
                if (!paymentExists)
                {
                    return BadRequest(new { Message = "No active subscription found. Please complete the payment to activate your account." });
                }
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (developmentEnvironment)
            {
                return Ok(new { email=user.Email,token=token });
            }
                var verificationLink = $"http://localhost:4200/api/Users/verification?email={user.Email}&token={token}";
                Console.WriteLine("\n\n" + verificationLink + "\n\n");

                Message message = new Message
    {
      Email = user.Email,
      Subject = "Confirm Email",
      Body = $"<h1 style=\"text-align: center;\">Confirm your email by clicking <a href=\"{verificationLink}\">here</a></h1>"
    };

                var command = new SendEmailCommand(message);

                return await _mediator.Send(command) ?
                Ok(new { Message = "To log in, you must open the verification link sent to your email" })
                : BadRequest(new { Message = "Failed to send verification link" });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    AccessToken = _jwtTokenService.GenerateToken(user.Id,
                    user.FirstName!,
                    user.LastName!,
                    user.PhoneNumber!,
                    role!),
                    RefreshToken = refreshToken
                });
            }
        }

        return Unauthorized();
    }



    [HttpPost("verification")]
    public async Task<IActionResult> ConfirmEmail([FromBody] VerificationDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return BadRequest("Invalid request.");

        var result = await _userManager.ConfirmEmailAsync(user, model.Token);
        if (result.Succeeded)
            return Ok(new { Message = "Email successfully verified" });

        return BadRequest(result.Errors);
    }


    [Authorize]
    [HttpPut("accountUpdate")]
    public async Task<IActionResult> UpdateAccount([FromBody] AccountUpdateDto account)
    {
                    var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
        var user = await _userManager.FindByIdAsync(applicationUserId);
        if (user == null)
        {
            return BadRequest();
        }

        user.UserName = account.Email;
        user.Email = account.Email;
        user.FirstName = account.FirstName;
        user.LastName = account.LastName;
        user.PhoneNumber = account.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [Authorize]
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        await _userManager.DeleteAsync(user);
        return NoContent();
    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenModel)
    {
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
        if (principal == null)
            return BadRequest("Invalid access token");

        var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var user = await _userManager.FindByIdAsync(userId!);


        if (user == null || user.RefreshToken != tokenModel.RefreshToken
        || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return BadRequest("Invalid refresh token");

        // Generate new JWT
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();
        var newJwtToken = _jwtTokenService.GenerateToken(user.Id,
                    user.FirstName!,
                    user.LastName!,
                    user.PhoneNumber!,
                    role!);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // Update refresh token in DB
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            AccessToken = newJwtToken,
            RefreshToken = newRefreshToken
        });
    }


    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return Ok(); // Don't reveal if user exists (security best practice)

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (developmentEnvironment)
            {
                return Ok(new { email=user.Email,token=token });
            }
        var resetLink = $"http://localhost:4200/api/Users/reset-password?email={user.Email}&token={token}";

Message message = new Message
    {
      Email = user.Email,
      Subject = "Reset Password",
      Body = $"<h1 style=\"text-align: center;\">Reset your password by clicking <a href=\"{resetLink}\">here</a></h1>"
    };

        var command = new SendEmailCommand(message);

        return await _mediator.Send(command) ?
        Ok(new { Message = "Password reset link sent to email" })
        : BadRequest(new { Message = "Failed to send password reset link" });
    }


    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return BadRequest("Invalid request.");

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (result.Succeeded)
            return Ok(new { Message = "Password reset successfully" });

        return BadRequest(result.Errors);
    }


    [HttpPost("capture-payment")]
    public async Task<IActionResult> CapturePayment(CapturePaymentOrderCommand command)
    {
        bool result = false;
        var userId = await _mediator.Send(command);
        if (userId != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest(new { Message = "User not found" });
            user.HasActiveSubscription = true;
            var updateResult = await _userManager.UpdateAsync(user);
            result = updateResult.Succeeded;
        }
        return result ? Ok(new { Message = "Payment captured, account activated" })
                       : BadRequest(new { Message = "Payment capture or account activate failed" });
    }

}
