# EcommerceDemo

A cross-platform e-commerce app built with **.NET MAUI** and **C#**, featuring user authentication, a product catalog with live discount pricing, a multi-currency wallet, a shopping cart, and a checkout flow backed by local SQLite storage. Built as a portfolio project to demonstrate mobile/desktop app development with a real data layer, not just static UI mockups.

<div align="center">
  
![.NET 10](https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=for-the-badge)
[![Latest Release](https://img.shields.io/github/v/release/YousefMohamed101/EcommerceDemo?label=release)](https://github.com/YousefMohamed101/EcommerceDemo/releases/latest)

</div>

## Table of Contents

- [Try It Without Building](#try-it-without-building)
- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [How It Works](#how-it-works)
- [Possible Improvements](#possible-improvements)
- [License](#license)

## Try It Without Building

The [latest release](https://github.com/YousefMohamed101/EcommerceDemo/releases/tag/stable-release), **Ecommerce Demo V0.5**, ships a ready-to-install Android APK, so you don't need the MAUI toolchain set up just to try the app:

Android:
1. Grab `Ecommerce_Demo_V0.5.apk` from the [Releases page](https://github.com/YousefMohamed101/EcommerceDemo/releases).
2. Transfer it to an Android device (or drag it onto an emulator).
3. Enable "install from unknown sources" if prompted, then install and open it.

Windows:
1. Grab `Ecommerce_Demo_V0.5.rar` from the [Releases page](https://github.com/YousefMohamed101/EcommerceDemo/releases).
2. Extract it
3. Open the folder and locate the EcommerceDemo.exe then launch the application

## Overview

Ecommerce App simulates a small online store: users create an account, browse a product catalog fetched from a public API, add items to a cart, and check out using an in-app wallet balance. It runs from a single C# codebase, targeting **Android** everywhere, plus **iOS, Mac Catalyst, and Windows** when built from macOS/Windows respectively.

## Features

- 🔐 **Authentication** – Sign up and sign in, with user data persisted locally in SQLite
- 🛍️ **Product catalog** – Product list seeded from the [DummyJSON](https://dummyjson.com/) API on first launch and cached locally, so the app works offline afterward
- 🔎 **Live search** – Filter products in real time by title, description, brand, or category
- 🏷️ **Discount pricing** – Discounted products show a struck-through original price next to the discounted one, and the discount is honored all the way through to the cart and checkout total, not just on the product card
- 🖼️ **Product details** – Dedicated, mobile-first scrollable product page with an image carousel and a quantity stepper that respects available stock
- 🛒 **Shopping cart** – Add/remove items, adjust quantities, and see a running, currency-converted total
- 💳 **Checkout** – Returns a clear result (`Succeeded`, `Failed`, or `InsufficientFunds`) rather than a plain success/fail flag, deducts the order total from the wallet, and updates product stock
- 👤 **Editable profile** – Update name, email, and balance, and set a profile picture from the device's photo library
- 💱 **Multi-currency wallet** – Prices and balances convert between USD, EUR, and JPY, not just relabel the same number with a different symbol
- 🔔 **Custom alert popups** – A reusable, app-styled popup (via CommunityToolkit.Maui) instead of the platform's default alert dialog

## Tech Stack

| Layer | Technology |
|---|---|
| UI framework | .NET MAUI (XAML + C# code-behind) |
| Language / runtime | C#, .NET 10 |
| Local storage | SQLite via `sqlite-net-pcl` |
| Remote data | [DummyJSON](https://dummyjson.com/) REST API (product seed data) |
| Popups/alerts | `CommunityToolkit.Maui` |
| Platforms | Android (always) · iOS, Mac Catalyst (built on macOS) · Windows (built on Windows) |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- The **.NET MAUI** workload (`dotnet workload install maui`)
- Visual Studio 2022 (17.x+) with the MAUI workload, or JetBrains Rider with MAUI support
- An internet connection on first launch, so the app can fetch and cache the product catalog

> **Note:** the project only adds `net10.0-ios`/`net10.0-maccatalyst` as build targets when building on macOS, and `net10.0-windows10.0.19041.0` only when building on Windows. Android is always available. Build from the matching OS if you want a specific platform's target.

### Setup

```bash
git clone https://github.com/YousefMohamed101/EcommerceDemo.git
cd EcommerceDemo
dotnet restore
```

Then open `EcommerceDemo.slnx` in Visual Studio or Rider, select a target device/emulator, and run. Alternatively, from the CLI:

```bash
dotnet build -t:Run -f net10.0-android
```

(swap `net10.0-android` for `net10.0-windows10.0.19041.0`, `net10.0-ios`, or `net10.0-maccatalyst` on the matching OS)

## How It Works

- **`DatabaseService`** is a singleton that owns three separate SQLite connections — one each for users, products, and cart items — and exposes the app's data operations (register/login, add/remove from cart, checkout, wallet updates).
- On first run, `InitializeProductList()` calls the DummyJSON API and bulk-inserts the returned products into the local product database; subsequent launches skip the network call and read from the cache.
- `CurrencyHelper` converts stored USD amounts into the wallet's selected currency (USD/EUR/JPY) for display, using a small static exchange-rate table.
- Checkout validates the wallet balance, decrements it, decrements product stock, and clears the purchased items from the cart, returning a `CheckOutStatus` so the caller knows exactly what happened.
- Navigation uses **.NET MAUI Shell**, with pages pushed onto the navigation stack (Store → Product → Cart → Checkout).

## Possible Improvements

This is a demo/portfolio project, so a few things are intentionally simplified. Natural next steps toward a production-ready version:

- Hash and salt user passwords instead of storing them as plain text
- Refactor from code-behind into an MVVM architecture with data binding
- Replace the static exchange rates with a live FX API

## License

This project is available under the MIT License.

