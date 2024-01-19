using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanqueDev.Services.Dto.Autentication;
using RanqueDev.Domain.Entities.Identity;
using SmtpRepository;
using RanqueDev.Api.Token;
using RanqueDev.Api.Shared;

namespace RanqueDev.Api.Controllers.Autentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {



        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, UserManager<User> userManager,
                             SignInManager<User> signInManager, IMapper mapper, ITokenService tokenService)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(userLogin.Email) ||
               string.IsNullOrEmpty(userLogin.Password))
                    return BadRequest("Email ou Senha não foi informado");

                var user = await _userManager.FindByEmailAsync(userLogin.Email);

                if (user == null)
                {
                    return BadRequest("Email não cadastrado");
                }
                var result = await _signInManager
                    .CheckPasswordSignInAsync(user, userLogin.Password, false);
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest("E-mail não foi confirmado");
                }
                if (result.Succeeded)
                {
                    var appUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedEmail == user.Email.ToUpper());

                    var userToReturn = _mapper.Map<UserReturnDto>(appUser);

                    return Ok(new
                    {
                        token = _tokenService.GenereteToken(appUser).Result,
                        user = userToReturn
                    });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }

        public class teste
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }


        }



        [HttpGet("teste")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            var t = new teste();
            t.Email = "teste";
            t.Password = "password";
            t.Name = "name";
            t.Id = "id";


            return Ok(t.ToString());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                OrganizationId = 1
            };

            var result = await _userManager.CreateAsync(
                user, userDto.Password);

            if (result.Succeeded)
            {
                await SendTokenConfirmationAsync(user);
                return Ok();
            }
            else
            {
                var erroList = new List<string>();
                foreach (var erro in result.Errors)
                {
                    erroList.Add(erro.Code + " , " + erro.Description);
                }
                var problemDetails = new CustomProblemDetails(status:
                    System.Net.HttpStatusCode.BadRequest
                    , request: Request
                    , errors: erroList);

                return BadRequest(problemDetails);
            }
        }

        [HttpGet("TokenConfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> TokenConfirmation(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("E-mail não cadastrado");
            }
            else
            {
                await SendTokenConfirmationAsync(user);
                return Ok();
            }

        }

        private async Task SendTokenConfirmationAsync(User user)
        {
            var emailCofirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmEmailAdressURL = Url.Action("ConfirmEmailAddress", "User",
                new { token = emailCofirmationToken, email = user.Email }, Request.Scheme);

            SMTPRepository smtp = new SMTPRepository();

            System.IO.File.WriteAllText("confirmationEmail.txt", confirmEmailAdressURL);
            _ = smtp.EnviarEmail(para: user.Email, body: confirmEmailAdressURL);
        }

        [HttpGet("ConfirmEmailAddress")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);


                if (result.Succeeded)
                {
                    return Ok("E-mail confirmado");
                }
            }

            return Ok();
        }
    }
}
