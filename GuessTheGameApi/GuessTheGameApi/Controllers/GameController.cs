using GuessTheGameApi.Converter.Converters;
using GuessTheGameApi.Converter.Models.Api;
using GuessTheGameApi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessTheGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuessTheGameController : Controller
    {
        private readonly IGameDomain _gameDomain;
        private readonly IGameConverter _gameConverter;
        private readonly IJwtAndHashGenerator _jwtAndHashGenerator;

        public GuessTheGameController(IGameDomain gameDomain, IGameConverter gameConverter, IJwtAndHashGenerator jwtAndHashGenerator)
        {
            _gameDomain = gameDomain;
            _gameConverter = gameConverter;
            _jwtAndHashGenerator = jwtAndHashGenerator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] Credentials credentials)
        {
            var passwordHashed = _jwtAndHashGenerator.HashPassword(credentials.Password); 

            var user = await _jwtAndHashGenerator.RegisterUserAsync(credentials.Username, passwordHashed);

            IActionResult response = Ok(new { token = "0", id = user.Id, username = user.Username, password = passwordHashed });

            if (user != null && user.Username != "0") //0 means the user is already in db
            {
                var tokenString = _jwtAndHashGenerator.GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString, id = user.Id, username = user.Username, password = passwordHashed });
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<ActionResult> GetCredentials([FromQuery] string username, [FromQuery] string password)
        {
            IActionResult response = Unauthorized();

            var passwordHashed = _jwtAndHashGenerator.HashPassword(password);
            var credentialsModel = await _gameDomain.GetCredentialsAsync(username, passwordHashed);

            if (credentialsModel.Id != 0)
            {
                var tokenString = _jwtAndHashGenerator.GenerateJSONWebToken(credentialsModel);
                response = Ok(new { token = tokenString, id = credentialsModel.Id, username = credentialsModel.Username, password = passwordHashed });
            }
            else
            {
                response = Ok(new { token = "0", id = credentialsModel.Id, username = credentialsModel.Username, password = passwordHashed });
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _gameDomain.CallApiAsync();

            return Ok(response);
        }

        [Authorize]
        [HttpGet("currentlevel")]
        public async Task<ActionResult> GetCurrentLevel()
        {
            var response = await _gameDomain.GetCurrentLevelsAsync();

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] int score, [FromQuery] int combo, [FromQuery] int idUser)
        {
            var result = await _gameDomain.UpdateCurrentLevelAsync(score, combo, idUser);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Game game)
        {
            var result = await _gameDomain.CreateGameAsync(_gameConverter.FromDtoToModel(game));
            return Ok(result);
        }

        [Authorize]
        [HttpPost("currentlevel")]
        public async Task<ActionResult> PostCurrentLevel([FromQuery] int idUser)
        {
            var result = await _gameDomain.CreateCurrentLevelAsync(idUser);
            return Ok(result);
        }
    }
}
