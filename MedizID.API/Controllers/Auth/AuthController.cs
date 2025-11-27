using MedizID.API.Common.Enums;
using MedizID.API.DTOs;
using MedizID.API.Models;
using MedizID.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace MedizID.API.Controllers.Auth;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IAuthService authService,
        ILogger<AuthController> logger,
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register", Name = "RegisterUser")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = Enum.Parse<UserRoleEnum>(request.Role)
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Registration failed for user {request.Email}");
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "REGISTRATION_FAILED",
                    Message = "Registration failed",
                    Details = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }

            _logger.LogInformation($"User {request.Email} registered successfully");

            return CreatedAtAction(nameof(Register), new RegisterResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Message = "User registered successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while registering user"
            });
        }
    }

    /// <summary>
    /// Google OAuth login/register
    /// </summary>
    [HttpPost("google", Name = "GoogleLogin")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GoogleLogin([FromQuery] string accessToken)
    {
        try
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_REQUEST",
                    Message = "Access token is required"
                });
            }

            // Get Google user info
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}");
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Google OAuth token validation failed");
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "INVALID_TOKEN",
                    Message = "Invalid or expired Google access token"
                });
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var googleUser = JsonSerializer.Deserialize<GoogleUserInfo>(jsonContent, options);

            if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_USER",
                    Message = "Unable to retrieve user information from Google"
                });
            }

            // Check if user exists
            var user = await _userManager.FindByEmailAsync(googleUser.Email);

            if (user == null)
            {
                // Create new user from Google profile
                user = new ApplicationUser
                {
                    UserName = googleUser.Email,
                    Email = googleUser.Email,
                    FirstName = googleUser.GivenName ?? googleUser.Name,
                    LastName = googleUser.FamilyName ?? "",
                    Role = UserRoleEnum.FacilityOwner,
                    EmailConfirmed = googleUser.VerifiedEmail
                };

                var createResult = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());
                if (!createResult.Succeeded)
                {
                    _logger.LogWarning($"Failed to create user from Google: {googleUser.Email}");
                    return BadRequest(new ErrorResponse
                    {
                        ErrorCode = "USER_CREATION_FAILED",
                        Message = "Failed to create user from Google profile"
                    });
                }

                _logger.LogInformation($"New user created from Google OAuth: {googleUser.Email}");
            }

            if (!user.LockoutEnabled || user.LockoutEnd > DateTime.UtcNow)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "USER_INACTIVE",
                    Message = "User account is locked or inactive"
                });
            }

            var token = _authService.GenerateJwtToken(user);

            _logger.LogInformation($"User {googleUser.Email} logged in via Google OAuth");

            return Ok(new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(24)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Google login");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred during Google login"
            });
        }
    }

    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost("login", Name = "LoginUser")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Login failed - user not found: {request.Email}");
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "INVALID_CREDENTIALS",
                    Message = "Invalid email or password"
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Login failed - invalid password for user: {request.Email}");
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "INVALID_CREDENTIALS",
                    Message = "Invalid email or password"
                });
            }

            var token = _authService.GenerateJwtToken(user);

            _logger.LogInformation($"User {request.Email} logged in successfully");

            return Ok(new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(24)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while logging in"
            });
        }
    }
}
