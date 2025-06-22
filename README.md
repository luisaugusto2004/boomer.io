# Boomer.io - Boomer Shooter Quotes API

A lightweight **RESTful API** serving quotes from classic *boomer shooters* (e.g., **Blood**, **Duke Nukem 3D**), built with **ASP.NET Core** and **SQL Server** — with a minimal static frontend for testing.

---

## 🚀 Features

- ✅ Get random quotes (`/api/Quotes/random`)
- ✅ Get all quotes (`/api/Quotes`)
- ✅ Get a quote by ID (`/api/Quotes/{id}`)
- ✅ Get quotes by character (`/api/Quotes/character/{idCharacter}`)
- ✅ Get all available characters (`/api/Characters`)
- ✅ Get all available franchises (`/api/Franchises`)
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
| GET    | `/api/Quotes/random`                   | Get a random quote                  |
| GET    | `/api/Quotes`                          | Get all quotes                     |
| GET    | `/api/Quotes/{id}`                     | Get quote by ID                    |
| GET    | `/api/Quotes/character/{idCharacter}` | Get quotes for a specific character|
| GET    | `/api/Characters`                      | Get all characters                 |
| GET    | `/api/Franchises`                       | Get all franchises                 |

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
