# FoodDash

**Quick and tasty food delivered with a dash of speed.**

FoodDash is a food-ordering web app built with **Blazor (.NET 8)** and styled with **Tailwind CSS v4**. Users can browse a Filipino and Italian menu, search and filter dishes, build a cart, check out with validated delivery details, and follow their order on a live tracking page.

> Midterm project, built by Venz Virni Blanza.

---

## Table of Contents

1. [Features](#features)
2. [Tech Stack](#tech-stack)
3. [Getting Started](#getting-started)
4. [Pages and Routes](#pages-and-routes)
5. [Project Structure](#project-structure)
6. [How It Works](#how-it-works)
7. [Menu](#menu)
8. [Validation Rules](#validation-rules)
9. [Known Limitations](#known-limitations)
10. [Git Workflow](#git-workflow)

---

## Features

- **Menu browsing** with dish photos, categories, prices and star ratings that match each dish's real rating
- **Search and category filter** (Filipino, Italian, Dessert) with a "no results" state
- **Shopping cart** shared across every page: add dishes, change quantities (1 to 20 per dish), remove items, see live totals
- **Live cart badge** in the navbar that updates as you add or remove items
- **Checkout form** with validation for name, Philippine mobile number and delivery address, plus a payment method choice (Cash on Delivery or GCash / Maya)
- **Order tracking** that shows the order you just placed (ID, items, total, address, payment, rider) and moves through four delivery stages automatically
- **Friendly empty states** for an empty cart, an empty checkout, and tracking with no order
- **Custom 404 page** for unknown URLs, so users are never stranded
- **Responsive layout** that works on phones, tablets and desktops

---

## Tech Stack

| Area | Technology |
| --- | --- |
| Framework | ASP.NET Core Blazor Web App, .NET 8 |
| Render mode | Interactive Server (global) |
| Styling | Tailwind CSS v4 via the Tailwind browser build (CDN) |
| State | In-memory services registered through dependency injection |
| Forms | `EditForm` with Data Annotations validation |
| Language | C# 12, Razor components |

There are no CSS files and no npm or Node.js setup. Tailwind is loaded from one `<script>` tag in `Components/App.razor`.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- An internet connection (Tailwind is loaded from a CDN)
- Git

Check your SDK version:

```bash
dotnet --version
```

### Installation

```bash
git clone https://github.com/venzblanz/FoodDash.git
cd FoodDash
```

### Run the app

```bash
dotnet watch
```

`dotnet watch` rebuilds and reloads the page whenever you save a file. To run without hot reload:

```bash
dotnet run
```

Then open one of the addresses printed in the terminal. With the default launch profile these are:

- http://localhost:5155
- https://localhost:7079 (when using the `https` profile)

### Troubleshooting

| Problem | Fix |
| --- | --- |
| `MSB1011: more than one project or solution file` | The folder has a `.sln` next to the `.csproj`. Run `dotnet watch --project Blanza_Midterm.csproj` or delete the `.sln`. |
| Page looks unstyled | The browser could not reach the Tailwind CDN. Check your internet connection and refresh. |
| Buttons do nothing | Make sure `Components/App.razor` has `@rendermode="InteractiveServer"` on both `<HeadOutlet />` and `<Routes />`, then restart the app. |
| Cart is empty after refreshing | Expected. The cart lives in memory for the current session (see [Known Limitations](#known-limitations)). |

---

## Pages and Routes

| Route | Page | What it does |
| --- | --- | --- |
| `/` | Home | Hero section, delivery highlights and popular dishes. Each dish links to the menu with the search already filled in. |
| `/place-order` | Menu | Full menu with search box, category chips and "Add to Cart" buttons. Accepts `?search=` (for example `/place-order?search=Lasagna`). |
| `/cart` | Cart | Lists cart items with quantity controls, a remove button and a subtotal and total. Links to checkout. |
| `/checkout` | Checkout | Order summary, delivery form, payment method and the "Confirm & Place Order" button. |
| `/track-order` | Track Order | Delivery timeline, estimated time, rider, order details and an "Order Again" link. Shows an empty state if no order has been placed. |
| `/not-found` | 404 | Shown for any URL that does not exist. Also reachable directly. |
| `/Error` | Error | Shown when an unhandled server error occurs in production. |

### Typical user flow

```
Home  →  Menu  →  Add to Cart  →  Cart  →  Checkout  →  Track Order
 /      /place-order            /cart     /checkout     /track-order
```

---

## Project Structure

```
FoodDash/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor       Page shell: background, navbar, error banner
│   │   └── NavMenu.razor          Navbar with links and live cart count
│   ├── Pages/
│   │   ├── Home.razor             /
│   │   ├── PlaceOrder.razor       /place-order
│   │   ├── Cart.razor             /cart
│   │   ├── Checkout.razor         /checkout
│   │   ├── TrackOrder.razor       /track-order
│   │   ├── NotFound.razor         /not-found
│   │   └── Error.razor            /Error
│   ├── Shared/
│   │   └── MenuItemCard.razor     Reusable dish card
│   ├── App.razor                  HTML shell, Tailwind script, render mode
│   ├── Routes.razor               Router and 404 handling
│   └── _Imports.razor             Global @using directives
├── Models/
│   ├── MenuItem.cs                A dish on the menu
│   ├── CartItem.cs                A dish and its quantity in the cart
│   ├── DeliveryDetails.cs         Checkout form model with validation rules
│   └── OrderTrack.cs              A placed order and its delivery stage
├── Services/
│   ├── MenuService.cs             Menu data, categories, search and filter logic
│   └── CartService.cs             Cart state and order placement
├── Properties/launchSettings.json Local ports and profiles
├── wwwroot/                       Images and favicon
├── Program.cs                     App startup, services, middleware
├── appsettings.json               App configuration
└── Blanza_Midterm.csproj          Project file (.NET 8)
```

The code follows a simple separation of concerns:

- **Pages and components** only display data and handle clicks.
- **Services** hold the logic (searching the menu, changing the cart, placing orders).
- **Models** describe the data and its validation rules.

---

## How It Works

### Services

Both services are registered in `Program.cs`:

```csharp
builder.Services.AddSingleton<MenuService>();   // one shared menu for everyone
builder.Services.AddScoped<CartService>();      // one cart per user session
```

**`MenuService`**

| Method | Purpose |
| --- | --- |
| `GetAll()` | Returns every dish |
| `GetCategories()` | Returns the distinct categories, sorted |
| `Search(term, category)` | Filters by an optional name or category text and an optional category |

**`CartService`**

| Member | Purpose |
| --- | --- |
| `Items`, `Count`, `Total` | Current cart contents, number of items and price total |
| `Add(item)` | Adds a dish, or raises its quantity if it is already in the cart |
| `Increase(id)` / `Decrease(id)` | Changes the quantity. Decreasing to 0 removes the item. |
| `Remove(id)` / `Clear()` | Removes one dish or empties the cart |
| `MaxQuantityPerItem` | Quantity cap per dish (20) |
| `Changed` | Event raised after every change. The navbar listens to it to refresh the cart badge. |
| `PlaceOrder(details)` | Turns the cart into an `OrderTrack`, stores it as `LastOrder`, and clears the cart |
| `LastOrder` | The most recent order, read by the tracking page |

### Order tracking

Each order moves through four stages. For the demo, a new stage starts every **10 seconds** (`OrderTrack.SecondsPerStage`):

1. Order Confirmed
2. Preparing Food
3. Out for Delivery
4. Delivered

The estimated time starts at 30 minutes and drops as the order progresses. The tracking page refreshes itself every second until the order is delivered.

### Styling

Tailwind CSS v4 is loaded from the browser build:

```html
<script src="https://cdn.jsdelivr.net/npm/@@tailwindcss/browser@@4"></script>
```

The doubled `@@` is how Razor writes a literal `@`. All styling is done with Tailwind utility classes directly in the `.razor` files.

---

## Menu

| Dish | Category | Price | Rating |
| --- | --- | --- | --- |
| Pork Sisig | Filipino | ₱250.00 | 5.0 |
| Beef Kaldereta | Filipino | ₱320.00 | 4.8 |
| Chicken Inasal | Filipino | ₱220.00 | 4.7 |
| Chicken Adobo | Filipino | ₱200.00 | 4.6 |
| Halo-Halo | Dessert | ₱150.00 | 4.9 |
| Classic Lasagna | Italian | ₱350.00 | 4.9 |
| Margherita Pizza | Italian | ₱450.00 | 4.7 |

Delivery is free on every order. To add or change a dish, edit the list in `Services/MenuService.cs` and put its photo in `wwwroot/images/`.

---

## Validation Rules

**Cart**

- A dish can't go below 1: lowering the quantity from 1 removes it
- A dish can't go above 20 (the `+` button is disabled at the limit)
- Checkout and tracking pages show an empty state instead of breaking when there is nothing to show

**Checkout form** (`Models/DeliveryDetails.cs`)

| Field | Rule |
| --- | --- |
| Full name | Required, 2 to 60 characters |
| Phone number | Required, valid Philippine mobile number such as `0917 123 4567` or `+63 917 123 4567` |
| Delivery address | Required, 10 to 200 characters |
| Payment method | Cash on Delivery (default) or GCash / Maya |

Error messages appear under each field, and the order can't be placed until the form is valid.

---

## Known Limitations

- **No database.** The cart and the last order are stored in memory, so refreshing the page or restarting the app clears them.
- **No accounts or real payments.** GCash / Maya is only a selectable option.
- **Simulated tracking.** The delivery stages, rider and estimated time are generated for demonstration, not from a real delivery system.
- **One active order.** The tracking page shows only the most recent order.
- **Needs internet for styling** because Tailwind is loaded from a CDN.

### Ideas for future work

- Save carts and orders in a database (for example SQLite with Entity Framework Core)
- User accounts and order history
- Real payment integration
- Offers, About and Service pages

---

## Git Workflow

Commit messages follow the conventional style:

| Prefix | Use for |
| --- | --- |
| `feat:` | A new feature |
| `fix:` | A bug fix |
| `refactor:` | Code restructuring with no behavior change |
| `chore:` | Tooling, config and cleanup |
| `docs:` | Documentation such as this README |

Examples:

```
feat: add menu search, category filter and reusable MenuItemCard with real star ratings
fix: cap cart quantity, add remove button and accessible labels on cart page
refactor: move menu data and search logic into MenuService
```

`bin/`, `obj/` and `node_modules/` are ignored through `.gitignore` and should never be committed.