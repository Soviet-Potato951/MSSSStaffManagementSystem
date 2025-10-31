# MSSS Staff Management System

A Windows (WPF) application that loads **staff ID → name** records from CSV, supports **filtering by name and ID**, and provides an **Admin** panel for Create / Update / Delete (CUD).

> Built with **MVVM** and a small set of abstractions to compare backing stores (Dictionary vs SortedDictionary).

---

## Quick start

1. Open the solution: `MSSSStaffManagementSystem.sln` in **Visual Studio 2022** (or newer).
2. Set startup project to **StaffManager.UI**.
3. Press **F5** to run.
4. Use **Alt+L** to load a CSV (or the “Load” command).
5. Filter live by typing into **Filter Name** or **Filter ID**.
6. Open **Admin** with **Alt+A**.

### CSV format

```
123456789,John Smith
987654321,Jane Doe
```

- ID must be **9 digits** (UK mobile without leading 0).
- Name is any non‑empty string.

---

## Keyboard shortcuts

| Context | Keys | Action |
|---|---|---|
| General | **Alt+L** | Load CSV |
| General | **Alt+N** | Focus “Filter Name” |
| General | **Alt+I** | Focus “Filter ID” |
| General | **Tab** | Copy selected record from Filtered list to both filter text boxes |
| General | **Alt+A** | Open Admin (modal) |
| Admin | **Alt+G** | Close Admin |

> The first ListBox is display‑only; the second ListBox is the “filtered & selectable” one.

---

## Architecture (MVVM + SOLID)

**Projects**
- `StaffManager.Core` — small domain + **abstractions** (`IStaffRepository`, `ICsvSerialiser`, `IFileDialogService`), enum `StoreMode`.
- `StaffManager.Infrastructure` — implementations (`StaffDictionary` repository over `Dictionary`/`SortedDictionary`, `CsvSerialiser`).  
- `StaffManager.UI` — **Views** (WPF XAML), **ViewModels** (`MainViewModel`, `AdminViewModel`), light **services** (`FileDialogService`), and **composition** (`ServiceFactory`, `AdminSeed`).

**Patterns**
- **MVVM**: Views have no business logic; ViewModels expose state + `ICommand`s; data binding drives UI.
- **SRP**: Each type has one reason to change: e.g., `CsvSerialiser` solely handles CSV IO; `StaffDictionary` solely mediates ID/name storage.
- **DIP**: UI depends on **interfaces**; concrete impls are composed via `ServiceFactory`.
- **LSP/ISP**: Minimal interfaces respected; `IStaffRepository` is substitutable by hash or sorted backing store.

---

## How it works

- **Store mode**: `ServiceFactory.Create(StoreMode.Hash|Tree)` wires a `Dictionary` or `SortedDictionary` behind `IStaffRepository`. (Currently a single app can switch modes; see “Assessment mapping” below.)
- **General window (MainWindow)**: loads CSV, shows read‑only **Unsorted** list and live **Filtered** list; updates text boxes when you press **Tab** on a selection.
- **Admin window (AdminWindow)**: launched **modal**. Can **Add / Update / Remove** and shows a **status bar** for feedback.
- **Validation**: minimal at present — see “Gaps”.

---

## Building & packaging

- Build **Release**.
- Zip the entire solution folder, plus:  
  - `Implementation Plan` (Q9),  
  - `Optimisation Report` (Q8),  
  - Screenshots of both apps showing required behaviours.

---

## Licence

© 2025 Sam (MSSS Staff Management System). All rights reserved.
