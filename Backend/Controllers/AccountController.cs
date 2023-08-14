using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using backend.DataProvider;
using backend.Model.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using backend.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SqlDataProvider _dataProvider;
        private readonly PasswordHasher<Account> _passwordHasher;

        public AccountController(SqlDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _passwordHasher = new PasswordHasher<Account>();
        }

        // GET: /Account
        [HttpGet(Endpoints.AccountController.GetAccounts, Name = "GetAccounts")]
        public IEnumerable<Account> GetAccounts()
        {
            return _dataProvider.Accounts.ToList();
        }

        // Login Method
        [HttpPost(Endpoints.AccountController.Login, Name = "Login")]
        public bool Login(LoginDTO login)
        {
            var user = _dataProvider.Accounts.FirstOrDefault(a => a.Name == login.Username);

            // Überprüfe, ob der Benutzer in der Datenbank existiert
            if (user == null)
            {
                return false;
            }

            // Das gespeicherte Salt aus der Datenbank abrufen
            byte[] salt = Convert.FromBase64String(user.PasswordSalt);

            // Verwende den PasswordHasher, um das eingegebene Passwort mit dem gespeicherten Hash zu vergleichen
            var passwordHasher = new PasswordHasher<Account>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password + Convert.ToBase64String(salt));

            // Rückgabe des Ergebnisses
            return result == PasswordVerificationResult.Success;
        }

        // GET: /Account/5
        [HttpGet(Endpoints.AccountController.GetAccountById, Name = "GetAccountById")]
        public IActionResult GetAccountById(int id)
        {
            var account = _dataProvider.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound(); // Falls der Account mit der angegebenen ID nicht gefunden wurde
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

            //Erstelle Account für die DB
            Account account = new() { Name = dto.Username};

            // Hash das Passwort unter Verwendung des Salts
            var passwordHasher = new PasswordHasher<Account>();
            string hashedPassword = passwordHasher.HashPassword(account, dto.Password + salt);

            // Speichere das Salt und den Hash in den entsprechenden Spalten der Datenbank
            account.PasswordSalt = salt;
            account.PasswordHash = hashedPassword;

            // Entferne

            _dataProvider.Accounts.Add(account); // Account der Datenquelle hinzufügen
            _dataProvider.SaveChanges(); // Speichere die Änderungen in der Datenbank

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

            // Speichere die Änderungen in der Datenbank
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
    }
}
