# Boomer.io - Boomer Shooter Quotes API

A lightweight **RESTful API** serving quotes from classic _boomer shooters_ (e.g., **Blood**, **Duke Nukem 3D**), built with **ASP.NET Core** and **SQL Server** — with a minimal static frontend for testing.

---

## Features

- Get random quotes (`/api/Quotes/random`)
- Get all quotes (`/api/Quotes`)
- Get all available characters (`/api/Characters`)
- Get all available franchises (`/api/Franchises`)
- Simple frontend (HTML/JS) to test API endpoints
- Docker support with containerized database via Docker Compose
- And more!

---

## Tech Stack

- **Backend:** ASP.NET Core 8 Web API
- **ORM:** Entity Framework Core + SQL Server
- **Frontend:** Vanilla HTML + JavaScript + CSS (no frameworks)
- **API Docs:** Swagger

## Docker

This project includes Docker and Docker Compose support for easy setup and deployment.

### Running with Docker Compose

Make sure you have Docker and Docker Compose installed.

1. Copy `.env.example` to `.env` and set your environment variables (e.g., `SA_PASSWORD`).
2. Run the containers:

```bash
docker-compose up --build
```

---

## API Endpoints

| Method | Route                                 | Description                                           |
| ------ | ------------------------------------- | ----------------------------------------------------- |
| GET    | `/api/Quotes/random`                  | Get a random quote                                    |
| GET    | `/api/Quotes`                         | Get all quotes                                        |
| GET    | `/api/Quotes/{id}`                    | Get quote by ID                                       |
| GET    | `/api/Quotes/character/{idCharacter}` | Get quotes for a specific character                   |
| GET    | `/api/Quotes/search`                  | Get quotes filtered by a query                        |
| GET    | `/api/Characters`                     | Get all characters                                    |
| GET    | `/api/Characters/{id}`                | Get character by ID                                   |
| GET    | `/api/Characters/{idFranchise}`       | Get characters filtered by their respective franchise |
| GET    | `/api/Franchises`                     | Get all franchises                                    |
| GET    | `/api/Franchises/{id}`                | Get franchise by ID                                   |

---

## Running Locally

### Backend (.NET 8):

```bash
cd boomerio
dotnet restore
dotnet run
```

API available at:  
`https://localhost:7239/swagger`

---

### Frontend (Static HTML/JS):

Open `/frontend/index.html` directly in your browser.

Or serve via:

```bash
npx serve frontend
```

---

## Database

- **SQL Server** (LocalDB or full version)
- Auto-generated tables via Entity Framework migrations
- Quote Columns:
  - `Id`
  - `QuoteText`
  - `CharacterId` (FK)
  - `CreatedAt`
  - `UpdatedAt`

---

## Testing

- Manual testing via **Insomnia**
- Unit testing using xUnit

---

## Future Plans

- [x] Add Unit Tests (xUnit)
- [ ] Implement integration tests to ensure end-to-end API functionality and reliability
- [ ] Deploy backend to Azure / Railway
- [ ] Host static frontend (GitHub Pages / Vercel)

---

## About

Inspired by classic 90's shooters — a tribute to **Duke Nukem**, **Blood**, **Serious Sam** and more.

Made with love by Luis.
