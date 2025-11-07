# 🗓️ MyScheduler

**MyScheduler** is a .NET class library that provides a robust and extensible framework for task scheduling.  
It enables developers to manage and automate the execution of one-time and recurring tasks with clear, modular configuration.

---

## ✨ Features

- ✅ Supports separated scheduling model via two enums:
  - `ScheduleType`: `Once`, `Recurring`
  - `Occurs`: `Daily`, `Weekly`, `Monthly`
- 🔘 Flexible daily frequency configuration:
  - `DailyFrequencyOnceCheckbox`
  - `DailyFrequencyEveryCheckbox`
- 🔍 Thorough validation of scheduling parameters
- ⏱️ Automatic calculation of next execution time through `ScheduleManager`
- 🧩 Extensible architecture for adding custom recurrence logic
- 🧪 Fully unit and integration tested with xUnit for reliability

---

## 🚀 What's New

### 🗓️ Monthly Recurrence Support

MyScheduler now supports **monthly recurrence** for task execution, greatly enhancing scheduling flexibility.

- **Two monthly scheduling options:**
  - **By day:** Schedule a task on a specific day of the month (e.g., every 10th day, every 2 months).
  - **By pattern:** Schedule based on ordinal and weekday (e.g., "first Monday every 3 months", "last Friday every month").
- **Configuration options:**
  - `MonthlyDayCalculator` and related classes perform scheduling for the "by day" approach.
  - `MonthlyTheCalculator` and related classes handle "by pattern" monthly scheduling.
  - Fully compatible with daily frequencies: once per day or at regular intervals within the day.
- **Validation and description:**
  - Validations ensure monthly scheduling is used with correct parameters (cannot mix day/pattern options; values within proper ranges, etc).
  - Scheduling descriptions are generated to clearly explain monthly recurrences.
- **Unit tests** for monthly logic are included.

**Example schedule descriptions:**
- "Occurs day 10 every 2 month(s) at 08:00, starting 01/01/2025"
- "Occurs the first Monday of every 1 month(s) every 2 hours between 09:00 and 17:00, starting 01/01/2025"

---

## 🖼 Example (Monthly Recurrence UI)

![image1](image1)

---

## 🧠 Architecture Overview

| Component                  | Responsibility                                                                         |
|----------------------------|----------------------------------------------------------------------------------------|
| **ScheduleEntity**         | Holds scheduling configuration (type, recurrence, frequency, and timing)                |
| **ScheduleType**           | Defines if a task runs `Once` or `Recurring`                                           |
| **Occurs**                 | Defines recurrence context (`Daily`, `Weekly`, `Monthly`)                              |
| **DailyFrequencyOnceCheckbox / DailyFrequencyEveryCheckbox** | Choose whether the recurrence runs once per day or at regular intervals |
| **Validator**              | Validates all scheduling combinations according to the selected configuration           |
| **ScheduleManager**        | Calculates next execution date/time and builds a human-readable description             |

---

## 🛠️ Getting Started

### 📋 Requirements

- [.NET SDK 7.0 or later](https://dotnet.microsoft.com/en-us/download)

### 📦 Installation

You can add **MyScheduler** to your project via NuGet (once published) or include the project manually in your solution.

#### Manual Cloning

```bash
git clone https://github.com/daavidpelaeez/MyScheduler.git
```

---

## 🧪 Testing

- The project contains comprehensive unit and integration tests for all recurrence types (Daily, Weekly, Monthly).
- Check the `SchedulerTests` project for example usage and test cases.

---

## 📚 Documentation

- See source code for configuration examples and further details.
- [Browse the codebase on GitHub](https://github.com/daavidpelaeez/MyScheduler).

---
