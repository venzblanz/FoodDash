# FoodDash

A Blazor Web App (.NET 8, interactive server rendering) for ordering Filipino and Italian dishes, styled with Tailwind CSS v4.

## Features

- Browse the menu, search by name and filter by category
- Add dishes to a cart, change quantities (1-20 per dish) or remove them
- Checkout with validated delivery details and a payment method
- Order tracking page that follows the order you just placed

## Project structure

```
Components/
  Layout/    MainLayout and NavMenu (navbar with live cart count)
  Pages/     Home, PlaceOrder, Cart, Checkout, TrackOrder, NotFound, Error
  Shared/    Reusable UI such as MenuItemCard
Models/      MenuItem, CartItem, DeliveryDetails, OrderTrack
Services/    MenuService (menu data + search/filter) and CartService (cart state + placing orders)
Styles/      Tailwind input (app.css)
wwwroot/     Static files and the generated css/app.css
```

## Run it

```bash
npm install
npx @tailwindcss/cli -i ./Styles/app.css -o ./wwwroot/css/app.css --watch   # terminal 1
dotnet watch                                                                  # terminal 2
```

The cart lives in memory for the current browser session, so refreshing the page empties it.
