# 🗓️ MyScheduler

**MyScheduler** is a .NET class library that provides a robust and extensible framework for task scheduling.  
It enables developers to manage and automate the execution of one-time and recurring tasks with clear, modular configuration.

---

## ✨ Features

- ✅ Supports separated scheduling model via two enums:
  - `ScheduleType`: `Once`, `Recurring`
  - `Occurs`: `Daily`, `Weekly`
- 🔘 Flexible daily frequency configuration:
  - `DailyFrequencyOnceCheckbox`
  - `DailyFrequencyEveryCheckbox`
- 🔍 Thorough validation of scheduling parameters
- ⏱️ Automatic calculation of next execution time through `ScheduleManager`
- 🧩 Extensible architecture for adding custom recurrence logic
- 🧪 Fully unit and integration tested with xUnit for reliability

---

## 🧠 New Architecture Overview

| Component | Responsibility |
|------------|----------------|
| **ScheduleEntity** | Holds scheduling configuration (type, recurrence, frequency, and timing) |
| **ScheduleType** | Defines if a task runs `Once` or `Recurring` |
| **Occurs** | Defines recurrence context (`Daily` or `Weekly`) |
| **DailyFrequencyOnceCheckbox / DailyFrequencyEveryCheckbox** | Choose whether the recurrence runs once per day or at regular intervals |
| **Validator** | Validates all scheduling combinations according to the selected configuration |
| **ScheduleManager** | Calculates next execution date/time and builds a human-readable description |

---

## 🛠️ Getting Started

### 📋 Requirements

- [.NET SDK 7.0 or later](https://dotnet.microsoft.com/en-us/download)

### 📦 Installation

You can add **MyScheduler** to your project via NuGet (once published) or include the project manually in your solution.

#### Manual Cloning

```bash
git clone https://github.com/daavidpelaeez/MyScheduler.git
