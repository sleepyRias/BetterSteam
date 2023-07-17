using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using backend.DataProvider;
using backend.Model.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

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

        [HttpGet("GetHash")]
        public bool GetHash(string password)
        {
            int userId = 8; // Hier solltest du die ID des gewünschten Benutzers angeben
            var user = _dataProvider.Accounts.FirstOrDefault(a => a.Id == userId);

            // Überprüfe, ob der Benutzer in der Datenbank existiert
            if (user == null)
            {
                return false;
            }

            // Das gespeicherte Salt aus der Datenbank abrufen
            byte[] salt = Convert.FromBase64String(user.PasswordSalt);

            // Verwende den PasswordHasher, um das eingegebene Passwort mit dem gespeicherten Hash zu vergleichen
            var passwordHasher = new PasswordHasher<Account>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password + Convert.ToBase64String(salt));

            // Rückgabe des Ergebnisses
            return result == PasswordVerificationResult.Success;
        }

        // GET: /Account/5
        [HttpGet("{id}", Name = "GetAccountById")]
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
        [HttpPost("Accounts", Name = "CreateAccount")]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (account == null)
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

            // Hash das Passwort unter Verwendung des Salts
            var passwordHasher = new PasswordHasher<Account>();
            string hashedPassword = passwordHasher.HashPassword(account, account.Password + salt);

            // Speichere das Salt und den Hash in den entsprechenden Spalten der Datenbank
            account.PasswordSalt = salt;
            account.PasswordHash = hashedPassword;

            _dataProvider.Accounts.Add(account); // Account der Datenquelle hinzufügen
            _dataProvider.SaveChanges(); // Speichere die Änderungen in der Datenbank

            return CreatedAtRoute("GetAccountById", new { id = account.Id }, account); // Erfolgreiches Erstellen mit 201 Created-Status und dem erstellten Account als Antwort
        }


        // PUT: /Account/5
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] Account updatedAccount)
        {
            var account = _dataProvider.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound(); // Falls der Account mit der angegebenen ID nicht gefunden wurde
            }

            // Aktualisiere die Eigenschaften des Accounts mit den Werten aus updatedAccount
            account.Name = updatedAccount.Name;
            account.Theme = updatedAccount.Theme;
            account.FavouriteGames = updatedAccount.FavouriteGames;

            // Wenn das Passwort im Request angegeben wurde, hash es und aktualisiere es
            if (!string.IsNullOrEmpty(updatedAccount.Password))
            {
                account.Password = _passwordHasher.HashPassword(account, updatedAccount.Password);
            }

            // Speichere die Änderungen in der Datenbank
            _dataProvider.SaveChanges();

            // Lösche das gehashte Passwort, bevor es in der Antwort zurückgegeben wird
            account.Password = null;

            return Ok(); // Erfolgreiche Aktualisierung
        }

        // DELETE: /Account/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _dataProvider.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return NotFound(); // Falls der Account mit der angegebenen ID nicht gefunden wurde
            }

            _dataProvider.Accounts.Remove(account); // Account aus der Datenquelle entfernen
            _dataProvider.SaveChanges(); // Speichere die Änderungen in der Datenbank

            return NoContent(); // Erfolgreiches Löschen (kein Inhalt zurückgegeben)
        }
    }
}
