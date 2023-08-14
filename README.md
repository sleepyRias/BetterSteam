# Game Management API

![Project Logo](project-logo.png) <!-- Wenn du ein Logo hast -->

## Beschreibung

Die Game Management API ermöglicht die Verwaltung von Spielen, Benutzerkonten und mehr. Sie wurde entwickelt, um Entwicklern eine einfache Schnittstelle zur Verwaltung von Spielen und Benutzerdaten zu bieten.
Sie wurde als Nebenprojekt der Autoren und Erkundung von C+ & Vue erstellt.

## Funktionen

- Verwaltung von Spielen mit Details wie Name, Genre, Preis und Veröffentlichungsdatum.
- Benutzerverwaltung mit sicherer Passwortspeicherung und Favoritenspielen.
- Filter- und Suchfunktionen für Spiele.
- Einfache Integration in andere Anwendungen über die RESTful API.

## Installation und Verwendung

1. Klone das Repository: `git clone https://github.com/dein-benutzername/game-management-api.git`
2. Installiere die Abhängigkeiten: `dotnet restore`
3. Passe die Konfigurationsdateien an: `appsettings.json`
4. Führe die Datenbankmigrationen aus: `dotnet ef database update`
5. Starte die Anwendung: `dotnet run`

## API Endpoints

| Methode | Endpoint                   | Beschreibung                                |
| ------- | -------------------------- | ------------------------------------------- |
| GET     | /api/games                 | Alle Spiele abrufen                        |
| GET     | /api/games/{id}            | Einzelnes Spiel abrufen                    |
| POST    | /api/games                 | Neues Spiel hinzufügen                     |
| PUT     | /api/games/{id}            | Spiel aktualisieren                        |
| DELETE  | /api/games/{id}            | Spiel löschen                              |
| POST    | /api/accounts              | Benutzerkonto erstellen                    |
| GET     | /api/accounts/{id}         | Benutzerkonto abrufen                      |
| PUT     | /api/accounts/{id}         | Benutzerkonto aktualisieren                |
| DELETE  | /api/accounts/{id}         | Benutzerkonto löschen                      |
| POST    | /api/accounts/authenticate | Benutzerkonto authentifizieren             |
| ...     | ...                        | ...                                       |

## Technologien

- ASP.NET Core für die API-Entwicklung
- Entity Framework Core für die Datenbankverwaltung
- SQLite als Datenbank
- ...

## Autoren

- Niklas M. - Backend
- Anton L. - Frontend
- Robert - Beratung / Ideen

## Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert. Weitere Informationen findest du in der [Lizenzdatei](LICENSE).

