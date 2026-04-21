# Rock Paper Scissors Lizard Spock Game

## Description

This repository contains a full-stack implementation of the Rock Paper Scissors Lizard Spock (RPSSL) game. The backend is built with **C# and .NET 8**, providing a clean RESTful API with a vertical-slice feature architecture, FluentValidation, and an in-memory database. The frontend is a **React + TypeScript** single-page application with no external UI libraries, featuring custom routing and a Material Design–inspired interface.

Players can compete against a computer opponent whose move is determined by a random number sourced from an external API (`codechallenge.boohma.com`). Registered players get access to a personal paginated scoreboard.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Node.js 18+](https://nodejs.org/) and npm

---

## Installation & Running

### Backend — RPSSL.API

```bash
# Navigate to the API project
cd RPSSL/RPSSL.API

# Run the API (development mode, starts on http://localhost:5148)
dotnet run
```

Swagger UI is available at `http://localhost:5148/swagger` when running in Development mode.

### Frontend — rpssl-frontend

```bash
# Navigate to the frontend project
cd rpssl-frontend

# Install dependencies
npm install

# Start the development server (starts on http://localhost:3000)
npm start
```

> The frontend expects the API at the URL defined in `.env`:
>
> ```
> REACT_APP_API_URL=http://localhost:5148/api
> ```

---

## Usage

Open `http://localhost:3000` in your browser.

- **Play anonymously** — click _Play as Guest_ to jump straight into the game without registering.
- **Create a player** — enter a username to create a persistent player account. Registered players get access to a personal scoreboard.

Once in the game room you can:

1. Select your move (Rock, Paper, Scissors, Lizard, or Spock) or let the app pick one randomly.
2. Press **Play** to face the computer — the result (win / lose / tie) is displayed immediately.
3. View your personal **scoreboard** (registered players only), paginated with 10 results per page.
4. **Reset** your scoreboard to start fresh.

---

## API Reference

All endpoints are prefixed with `/api`.

### Choices

| Method | Route                 | Description                                                 |
| ------ | --------------------- | ----------------------------------------------------------- |
| `GET`  | `/api/choices`        | Returns all five available choices with their IDs and names |
| `GET`  | `/api/choices/random` | Returns a single randomly selected choice                   |

### Games

| Method   | Route                              | Description                           |
| -------- | ---------------------------------- | ------------------------------------- |
| `POST`   | `/api/games/play`                  | Play a round against the computer     |
| `GET`    | `/api/games/scoreboard/{playerId}` | Get paginated scoreboard for a player |
| `DELETE` | `/api/games/scoreboard/{playerId}` | Reset all game history for a player   |

**POST `/api/games/play` — request body:**

```json
{
  "choice": 1,
  "playerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

`playerId` is optional (anonymous play is supported).

**POST `/api/games/play` — response:**

```json
{
  "results": "win",
  "player": 1,
  "computer": 3
}
```

**GET `/api/games/scoreboard/{playerId}?page=1` — response:**

```json
[{ "playerChoice": "rock", "computerChoice": "scissors", "result": "win" }]
```

### Players

| Method | Route          | Description         |
| ------ | -------------- | ------------------- |
| `POST` | `/api/players` | Create a new player |

**POST `/api/players` — request body:**

```json
{
  "name": "string"
}
```

**Response:**

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string"
}
```

> If a player with the same name already exists, the API returns `409 Conflict` with `{ "error": "Player with name '...' already exists." }`.

---

## Running Tests

```bash
cd RPSSL/RPSSL.Tests
dotnet test
```

Tests cover FluentValidation validators for `CreatePlayerCommand` and `PlayGameCommand`.

---

## Tech Stack

| Layer        | Technology                                                          |
| ------------ | ------------------------------------------------------------------- |
| Backend      | C#, .NET 8, ASP.NET Core Web API                                    |
| Persistence  | Entity Framework Core (in-memory)                                   |
| Validation   | FluentValidation                                                    |
| API Docs     | Swagger / Swashbuckle                                               |
| External API | `codechallenge.boohma.com` (random number source for computer move) |
| Frontend     | React 18, TypeScript                                                |
| Routing      | Custom React Context router (no react-router-dom)                   |
| Styling      | Pure CSS with CSS custom properties (no UI library)                 |
| Tests        | xUnit                                                               |

---

## Game Rules

The five choices beat/lose to each other as follows:

- **Scissors** cuts Paper, decapitates Lizard
- **Paper** covers Rock, disproves Spock
- **Rock** crushes Lizard, crushes Scissors
- **Lizard** poisons Spock, eats Paper
- **Spock** smashes Scissors, vaporizes Rock

Full rules: [The Big Bang Theory — RPSSL](https://www.samkass.com/theories/RPSSL.html)
