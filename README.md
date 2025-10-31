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
| Admin | **Alt+G** | Close Admin *(project current)* → **should be Alt+L** per spec |

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

## Assessment mapping (what’s satisfied vs not yet)

### Q4 – General GUI (Dictionary)
- ✅ Data structure backed by **Dictionary** (via `StoreMode.Hash`).
- ✅ Method to **load** CSV on demand (command) and display in a **read‑only** list; live **filter** by name and by ID.
- ✅ **Keyboard**: Alt+N/Alt+I focus, **Tab** populates text boxes from the filtered list.
- ✅ **Open Admin** modal; seed passes selected ID/Name.
- ✅ **Status** messages.
- ⚠️ **Method visibility**: ViewModel methods are `private`, but repository is instance‑scoped (not a `public static Dictionary` as the brief suggests). This is cleaner, but deviates from literal spec.
- ⚠️ **Load on GUI _load_** vs on command: currently via command (Alt+L).

### Q5 – Admin GUI (Dictionary)
- ✅ **Modal**, **ControlBox removed** (`WindowStyle=None`).
- ✅ **Staff ID TextBox read‑only** for Update/Delete (toggle based on intent).
- ✅ Receive selected record from General GUI.
- ✅ Add/Update/Remove + status bar.
- ⚠️ **Hotkey**: Close uses **Alt+G** → per spec it must be **Alt+L**.
- ⚠️ **ID generation** for **Create** must be unique and start with **77xxxxxxx** — **not enforced yet** (currently defaults `77` and relies on manual input).
- ⚠️ **Persist on close**: Save happens from `AdminViewModel` by **newing** a `CsvSerialiser` and writing a **hard‑coded** `staff_master.csv` → this violates **DIP/SRP**; should delegate via injected `ICsvSerialiser` & chosen file path.

### Q6 – General GUI (SortedDictionary)
- ✅ Sorted store supported via `StoreMode.Tree` and `ServiceFactory.CreateRepository`.
- ❌ Per brief, this must be a **separate project/namespace** mirroring Q4. Currently it’s **one app** that can switch modes.

### Q7 – Admin GUI (SortedDictionary)
- ✅ Functionality mirrors Q5 when `StoreMode.Tree` is used.
- ❌ Same **separate‑project** requirement not met (currently unified UI).

### Q8 – Testing & Optimisation
- ❌ No Stopwatch/Trace timing tables or profiler screenshots included.
- ❌ No documented “Before/After” IO or data‑structure performance results.

### Q9–Q10 – Plan & Demo
- ❌ Implementation Plan table not included.
- ❌ Final demonstration checklist/doc not included.

---

## Gaps & recommended changes (to fully meet assessor rubric)

1. **Split into two UI projects** (and namespaces):  
   - `MSSS.DictionaryApp` (uses `StoreMode.Hash`) and `MSSS.SortedDictionaryApp` (uses `StoreMode.Tree`).  
   - Reuse `Core` and `Infrastructure` as shared libs. Keep identical View/XAML & ViewModel code via project‑linked files or a `StaffManager.UI.Common` project.

2. **Hotkeys per spec**: change Admin close to **Alt+L**.

3. **Create ID rule**: implement generator `int GenerateNewId()` that returns the smallest unique **77xxxxxxx** not in repository. Enforce 9‑digit rule; reject duplicates with clear status.

4. **Persist correctly**: inject `ICsvSerialiser` + last opened file path into `AdminViewModel` and call `csv.Save(path, repo.All())` on close. Remove `new CsvSerialiser()` and the hard‑coded filename.

5. **MVVM purity**: decouple `MainViewModel` from creating `AdminWindow`. Raise an event (`RequestAdmin(AdminSeed)`) and let the View handle `ShowDialog()`.

6. **Load on window load** (if required by spec) or clearly document the **Alt+L** command in the UI; assessor just wants predictability.

7. **Validation & UX**:
   - Disable **Add** unless `StaffId` starts with 77 and is unique; show message in StatusBar.  
   - Keep **StaffId** read‑only for Update/Delete; toggle writable for Create only.

8. **Testing & Optimisation docs**: add Stopwatch timings for CSV load/save and compare Dictionary vs SortedDictionary searches; include profiler notes.

9. **CITEMS coding standard**: ensure consistent comments above method signatures mapping each rubric bullet to code.

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
