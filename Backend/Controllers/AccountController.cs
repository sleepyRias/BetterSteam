using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using backend.DataProvider;
using backend.Model;
using backend.Model.Account;
using backend.Model.DTO;
using Azure.Core;
using Microsoft.Identity.Client;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AccountDataProvider _dataProvider;
        private readonly PasswordHasher<Account> _passwordHasher;

        public AccountController(AccountDataProvider dataProvider, IConfiguration configuration)
        {
            _dataProvider = dataProvider;
            _passwordHasher = new PasswordHasher<Account>();
            _configuration = configuration;
        }

        // POST: /Account
        [HttpPost(Endpoints.AccountController.Login, Name = "Login")]
        public IActionResult Login(LoginDTO login)
        {
            var user = _dataProvider.Accounts.FirstOrDefault(a => a.Name == login.Username);

            // Überprüfe, ob der Benutzer in der Datenbank existiert
            if (user == null)
            {
                return BadRequest(new { message = "Benutzer nicht gefunden" });
            }

            // Das gespeicherte Salt aus der Datenbank abrufen
            byte[] salt = Convert.FromBase64String(user.PasswordSalt);

            // Verwende den PasswordHasher, um das eingegebene Passwort mit dem gespeicherten Hash zu vergleichen
            var passwordHasher = new PasswordHasher<Account>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password + Convert.ToBase64String(salt));

            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { message = "Falsches Passwort" });
            }

            var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())  // Annahme, dass user.Id ein integer oder eine andere Datenstruktur ist, die in eine Zeichenkette konvertiert werden muss
                };

            // Sie können hier zusätzliche Claims hinzufügen, falls nötig

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }


        // GET: /Account/Id
        [HttpGet(Endpoints.AccountController.GetAccountById, Name = "GetAccountById")]
        public IActionResult GetAccountById(int id)
        {
            var account = _dataProvider.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // POST: /Account
        [HttpPost(Endpoints.AccountController.CreateAccount, Name = "CreateAccount")]
        public IActionResult CreateAccount([FromBody] LoginDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(); // Ungültige Anforderung
            }

            // Generiere ein zufälliges Salt
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            Account account = new() { Name = dto.Username };

            // Hash das Passwort unter Verwendung des Salts
            var passwordHasher = new PasswordHasher<Account>();
            string hashedPassword = passwordHasher.HashPassword(account, dto.Password + salt);

            // Speichere das Salt und den Hash in den entsprechenden Spalten der Datenbank
            account.PasswordSalt = salt;
            account.PasswordHash = hashedPassword;

            // Entferne

            _dataProvider.Accounts.Add(account);
            _dataProvider.SaveChanges();

            return CreatedAtRoute("GetAccountById", new { id = account.Id }, account); // Erfolgreiches Erstellen mit 201 Created-Status und dem erstellten Account als Antwort
        }


        // PUT: /Account/5
        [HttpPut(Endpoints.AccountController.UpdateAccount, Name = "UpdateAccount")]
        public IActionResult UpdateAccount(int id, [FromBody] AccountUpdateDTO updatedAccount)
        {
            var account = _dataProvider.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound(); // Falls der Account mit der angegebenen ID nicht gefunden wurde
            }

            // Aktualisiere die Eigenschaften des Accounts mit den Werten aus updatedAccount
            if (!string.IsNullOrEmpty(updatedAccount.Name)) { account.Name = updatedAccount.Name; }
            if (!string.IsNullOrEmpty(updatedAccount.Theme)) { account.Theme = updatedAccount.Theme; }

            // Wenn das Passwort im Request angegeben wurde, hash es und aktualisiere es
            if (!string.IsNullOrEmpty(updatedAccount.Password))
            {
                account.PasswordHash = _passwordHasher.HashPassword(account, updatedAccount.Password + account.PasswordSalt);
            }

            _dataProvider.SaveChanges();

            return Ok(); // Erfolgreiche Aktualisierung
        }

        // DELETE: /Account/5
        //[HttpDelete(Endpoints.AccountController.DeleteAccount, Name = "Delete Account")]
        //public IActionResult DeleteAccount(int id)
        //{
        //    var account = _dataProvider.Accounts.Include(a => a.FavouriteGames).FirstOrDefault(a => a.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound(); // Falls der Account mit der angegebenen ID nicht gefunden wurde
        //    }

        //    // Lösche zuerst die FavouriteGames-Einträge, die mit diesem Account verknüpft sind
        //    _dataProvider.FavouriteGames.RemoveRange(account.FavouriteGames);

        //    _dataProvider.Accounts.Remove(account); // Account aus der Datenquelle entfernen
        //    _dataProvider.SaveChanges(); // Speichere die Änderungen in der Datenbank

        //    return NoContent(); // Erfolgreiches Löschen (kein Inhalt zurückgegeben)
        //}

        // True -> Available
        [HttpGet(Endpoints.AccountController.CheckUserNameAvailability, Name = "CheckUserNameAvailability")]
        public bool CheckUserNameAvailability(string username)
        {
            var user = _dataProvider.Accounts.FirstOrDefault(a => a.Name == username);
            if (user != null) { return false; }
            return true;
        }

        //POST: /Account/Verify
        [HttpPost(Endpoints.AccountController.Verify, Name = "Verify")]
        public IActionResult VerifyToken([FromBody] TokenRequest request)
        {
            var jwtHelper = new JwtHelper(_configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);

            if (jwtHelper.VerifyToken(request.Token))
            {
                return Ok(new { IsValid = true });
            }
            return Ok(new { IsValid = false });
        }

        //POST: /Account/AddFavouriteGame
        [HttpPost(Endpoints.AccountController.AddFavouriteGame, Name = "AddFavouriteGame")]
        public IActionResult AddFavouriteGame([FromBody] TokenDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var jwtHelper = new JwtHelper(_configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);
            var userId = jwtHelper.GetClaim(dto.Token, ClaimTypes.NameIdentifier).Value;
            try
            {
                _dataProvider.FavouriteGames.Add(new FavouriteGame() { AccountId = int.Parse(userId), GameId = int.Parse(dto.Value1) });
                _dataProvider.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return Ok(); // Erfolgreiches Hinzufügen
        }

        //DELETE: /Account/RemoveFavouriteGame
        [HttpDelete(Endpoints.AccountController.RemoveFavouriteGame, Name = "RemoveFavouriteGame")]
        public IActionResult RemoveFavouriteGame([FromBody] TokenDTO dto)
        {
            if (dto == null)
            {
                return NotFound(); // Ungültige Anforderung
            }

            var jwtHelper = new JwtHelper(_configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);
            var userId = jwtHelper.GetClaim(dto.Token, ClaimTypes.NameIdentifier).Value;
            
            var favGame = _dataProvider.FavouriteGames
                    .Where(f => f.AccountId == int.Parse(userId) && f.GameId == int.Parse(dto.Value1))
                    .FirstOrDefault();
            if (favGame == null)
            {
                return NotFound();
            }
            _dataProvider.FavouriteGames.Remove(favGame);
            _dataProvider.SaveChanges();
            
            return Ok(); // Erfolgreiches Löschen
        }

        //POST: /Account/GetNameFromToken
        [HttpPost(Endpoints.AccountController.GetNameFromToken, Name = "GetNameFromToken")]
        public IActionResult GetNameFromToken([FromBody] TokenRequest request)
        {
            var jwtHelper = new JwtHelper(_configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);
            var name = jwtHelper.GetClaim(request.Token, ClaimTypes.Name).Value;
            return Ok(new { Name = name });
        }
    }
} 
