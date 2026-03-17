# ⚽ FootballHub

A real-time football scores and statistics platform built with .NET 8 microservices — inspired by FotMob and SofaScore.

---

## Overview

FootballHub is a microservices-based application that provides live match scores, league standings, and player statistics. It uses SignalR for real-time updates, MassTransit for event-driven messaging, and a clean web frontend.

---

## Architecture

```
Client (Browser)
      │
      ▼
API Gateway :5000  (YARP Reverse Proxy)
      │
      ├──▶  Match Service      :5001  →  FootballHub_Matches  (MSSQL)
      ├──▶  League Service     :5002  →  FootballHub_Leagues  (MSSQL)
      ├──▶  Player Service     :5003  →  FootballHub_Players  (MSSQL)
      └──▶  Notification Svc   :5004
                  ▲
                  └──── Message Bus (MassTransit + RabbitMQ)
```

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 8, ASP.NET Core Web API |
| Database | Microsoft SQL Server + EF Core 8 |
| Real-time | SignalR (WebSocket) |
| Messaging | MassTransit + RabbitMQ |
| API Gateway | YARP Reverse Proxy |
| Frontend | HTML, CSS, JavaScript + jQuery |

---

## Project Structure

```
FootballHub/
├── FootballHub.sln
├── README.md
└── src/
    ├── ApiGateway/
    │   └── FootballHub.ApiGateway/          # YARP Gateway — port 5000
    ├── Services/
    │   ├── MatchService/
    │   │   └── FootballHub.MatchService/    # Matches, live scores — port 5001
    │   ├── LeagueService/
    │   │   └── FootballHub.LeagueService/   # Leagues, teams, standings — port 5002
    │   ├── PlayerService/
    │   │   └── FootballHub.PlayerService/   # Players, statistics — port 5003
    │   └── NotificationService/
    │       └── FootballHub.NotificationService/ # Event consumer — port 5004
    ├── Shared/
    │   ├── FootballHub.Shared/              # Base entities, API response wrapper
    │   └── FootballHub.Contracts/           # MassTransit event contracts
    └── Web/
        ├── index.html                       # Frontend entry point
        ├── css/
        │   └── style.css
        └── js/
            └── app.js
```

---

## Services

### Match Service `:5001`
Handles all match-related data. Publishes events to the message bus when matches start, goals are scored, or matches finish. Includes a SignalR hub for real-time score updates.

| Endpoint | Description |
|---|---|
| `GET /api/matches/today` | All matches for today |
| `GET /api/matches/live` | Currently live matches |
| `GET /api/matches/{id}` | Match details with events |
| `POST /api/matches` | Create a new match |
| `POST /api/matches/{id}/start` | Start a match |
| `POST /api/matches/{id}/events` | Add a goal, card, or substitution |
| `PUT /api/matches/{id}/score` | Update the score |
| `POST /api/matches/{id}/finish` | Finish a match |
| `WS /hubs/matches` | SignalR hub for live updates |

### League Service `:5002`
Manages leagues, teams, and standings. Automatically recalculates standings positions after each update.

| Endpoint | Description |
|---|---|
| `GET /api/leagues` | All leagues |
| `GET /api/leagues/{id}` | League with teams |
| `POST /api/leagues` | Create a league |
| `GET /api/leagues/{id}/standings` | League standings table |
| `PUT /api/leagues/{id}/standings` | Update a team's standing |
| `POST /api/teams` | Create a team |

### Player Service `:5003`
Stores player profiles and per-season statistics.

| Endpoint | Description |
|---|---|
| `GET /api/players/{id}` | Player profile |
| `GET /api/players/team/{teamId}` | Players by team |
| `GET /api/players/top-scorers?leagueId=&season=` | Top scorers |
| `GET /api/players/top-assists?leagueId=&season=` | Top assists |
| `POST /api/players` | Create a player |
| `PUT /api/players/{id}/statistics` | Update player stats |

### Notification Service `:5004`
Consumes events from RabbitMQ and handles notifications. Currently logs events — ready to be extended with push notifications, email, or SMS.

**Listens for:**
- `MatchStarted`
- `GoalScored`
- `MatchFinished`

---

## Real-time Events (SignalR)

Connect to `ws://localhost:5001/hubs/matches` to receive live updates.

| Event | Trigger |
|---|---|
| `MatchStarted` | Match kicks off |
| `GoalScored` | Goal is recorded |
| `ScoreUpdated` | Score manually updated |
| `MatchEvent` | Card, substitution, etc. |
| `MatchFinished` | Full time |

**Join a group to filter updates:**
```javascript
connection.invoke("JoinMatch", "42");        // specific match
connection.invoke("JoinLiveMatches");        // all live matches
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (any edition)
- [RabbitMQ](https://www.rabbitmq.com/docs/install-windows) — optional, can use InMemory transport for development

### Configuration

Update the connection string in `appsettings.json` of each service to match your SQL Server instance:

```json
"ConnectionStrings": {
  "MatchDb": "Server=YOUR_SERVER;Database=FootballHub_Matches;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Replace `YOUR_SERVER` with your SQL Server instance name (e.g. `.\SQLEXPRESS`, `(localdb)\MSSQLLocalDB`).

### Running

Open the solution in **Visual Studio 2022**, then:

1. Right-click the Solution → **Configure Startup Projects**
2. Set **Multiple startup projects** and set all 5 to **Start**:
   - `FootballHub.ApiGateway`
   - `FootballHub.MatchService`
   - `FootballHub.LeagueService`
   - `FootballHub.PlayerService`
   - `FootballHub.NotificationService`
3. Press **F5**

Each service automatically runs EF Core migrations and creates its database on startup.

### Swagger UI

| Service | URL |
|---|---|
| Match Service | http://localhost:5001/swagger |
| League Service | http://localhost:5002/swagger |
| Player Service | http://localhost:5003/swagger |
| Notification Service | http://localhost:5004/swagger |

### Frontend

Open `http://localhost:5005` in your browser (served by `FootballHub.Web`).

---

## Database Migrations

If you need to create or re-run migrations manually, use the **Package Manager Console** in Visual Studio:

```
# Match Service
Add-Migration InitialCreate -Project FootballHub.MatchService -StartupProject FootballHub.MatchService
Update-Database -Project FootballHub.MatchService -StartupProject FootballHub.MatchService

# League Service
Add-Migration InitialCreate -Project FootballHub.LeagueService -StartupProject FootballHub.LeagueService
Update-Database -Project FootballHub.LeagueService -StartupProject FootballHub.LeagueService

# Player Service
Add-Migration InitialCreate -Project FootballHub.PlayerService -StartupProject FootballHub.PlayerService
Update-Database -Project FootballHub.PlayerService -StartupProject FootballHub.PlayerService
```

---

## Development Without RabbitMQ

To run without RabbitMQ installed, replace the MassTransit configuration in `Program.cs` of **Match Service** and **Notification Service** with InMemory transport:

```csharp
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});
```

---

## Message Bus Events

Events are defined in `FootballHub.Contracts` and shared across services.

```csharp
// Published when a match kicks off
public record MatchStarted
{
    public int MatchId { get; init; }
    public string HomeTeam { get; init; }
    public string AwayTeam { get; init; }
    public DateTime KickOff { get; init; }
}

// Published when a goal is scored
public record GoalScored
{
    public int MatchId { get; init; }
    public string Team { get; init; }
    public string PlayerName { get; init; }
    public int Minute { get; init; }
    public int HomeScore { get; init; }
    public int AwayScore { get; init; }
}

// Published at full time
public record MatchFinished
{
    public int MatchId { get; init; }
    public int HomeScore { get; init; }
    public int AwayScore { get; init; }
    public DateTime FinishedAt { get; init; }
}
```

---

## Service Ports

| Service | Port |
|---|---|
| API Gateway | 5000 |
| Match Service | 5001 |
| League Service | 5002 |
| Player Service | 5003 |
| Notification Service | 5004 |
| Web Frontend | 5005 |
