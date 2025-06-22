# Boomer.io - Boomer Shooter Quotes API

A lightweight **RESTful API** serving quotes from classic *boomer shooters* (e.g., **Blood**, **Duke Nukem 3D**), built with **ASP.NET Core** and **SQL Server** — with a minimal static frontend for testing.

---

## 🚀 Features

- ✅ Get random quotes (`/api/quotes/random`)
- ✅ Get all quotes (`/api/quotes`)
- ✅ Get a quote by ID (`/api/quotes/{id}`)
- ✅ Get quotes by character (`/api/quotes/character/{idCharacter}`)
- ✅ Get all available characters (`/api/characters`)
- ✅ Get all available franchises (`/api/franchise`)
- ✅ Simple frontend (HTML/JS) to test API endpoints

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core 8 Web API
- **ORM:** Entity Framework Core + SQL Server
- **Frontend:** Vanilla HTML + JavaScript + CSS (no frameworks)
- **API Docs:** Swagger (Dark Theme)

---

## 🔗 API Endpoints

| Method | Route                                   | Description                          |
|--------|----------------------------------------|------------------------------------|
| GET    | `/api/quotes/random`                   | Get a random quote                  |
| GET    | `/api/quotes`                          | Get all quotes                     |
| GET    | `/api/quotes/{id}`                     | Get quote by ID                    |
| GET    | `/api/quotes/character/{idCharacter}` | Get quotes for a specific character|
| GET    | `/api/characters`                      | Get all characters                 |
| GET    | `/api/franchise`                       | Get all franchises                 |

---

## ⚙️ Running Locally

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

## 🗄️ Database

- **SQL Server** (LocalDB or full version)
- Auto-generated tables via Entity Framework migrations
- Columns:
  - `Id`
  - `QuoteText`
  - `CharacterId` (FK)
  - `CreatedAt`
  - `UpdatedAt`

---

## 🧪 Testing

- Manual testing via **Insomnia**
- Unit testing not yet implemented

---

## 📦 Future Plans

- [ ] Add Unit Tests (xUnit)
- [ ] Deploy backend to Azure / Railway
- [ ] Host static frontend (GitHub Pages / Vercel)

---

## 🤘 About

Inspired by classic 90's shooters — a tribute to **Duke Nukem**, **Blood**, **Serious Sam** and more.

Made with ❤️ by Luis.
