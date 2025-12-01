# 🗓️ MyScheduler

**MyScheduler** es una librería .NET moderna para la gestión y automatización de tareas programadas, diseñada para ser robusta, extensible y fácil de integrar en cualquier solución.

---

## ✨ Características

- **Modelo de programación flexible:**  
  - `ScheduleType`: `Once`, `Recurring`
  - `Occurs`: `Daily`, `Weekly`, `Monthly`
- **Configuración avanzada de frecuencia diaria:**  
  - `DailyFrequencyOnceCheckbox`
  - `DailyFrequencyEveryCheckbox`
- **Validación exhaustiva de parámetros de programación**
- **Cálculo automático del próximo tiempo de ejecución** con `ScheduleManager`
- **Arquitectura extensible** para lógica personalizada de recurrencia
- **Cobertura de tests >95%** con xUnit: confiabilidad garantizada

---

## 🚀 Novedades

### 🗓️ Soporte para Recurrencia Mensual

- **Dos modos de programación mensual:**
  - Por día específico (ej: día 10 cada 2 meses)
  - Por patrón (ej: primer lunes cada 3 meses, último viernes cada mes)
- **Validaciones inteligentes:**  
  - No se permite mezclar configuraciones de día y patrón
  - Rango de valores controlado
- **Descripciones automáticas:**  
  - Generación de textos claros para cada configuración
- **Totalmente testeado:**  
  - Pruebas unitarias e integración para todos los casos

**Ejemplos de descripción:**
- "Occurs day 10 every 2 month(s) at 08:00, starting 01/01/2025"
- "Occurs the first Monday of every 1 month(s) every 2 hours between 09:00 and 17:00, starting 01/01/2025"

---

## 🧠 Arquitectura

| Componente                  | Responsabilidad                                                                         |
|----------------------------|----------------------------------------------------------------------------------------|
| **ScheduleEntity**         | Configuración de la programación (tipo, recurrencia, frecuencia, horario)               |
| **ScheduleType**           | Define si la tarea es única o recurrente                                                |
| **Occurs**                 | Contexto de recurrencia (`Daily`, `Weekly`, `Monthly`)                                  |
| **DailyFrequencyOnceCheckbox / DailyFrequencyEveryCheckbox** | Configuración de frecuencia diaria                   |
| **Validator**              | Valida todas las combinaciones posibles según la configuración                          |
| **ScheduleManager**        | Calcula la próxima ejecución y genera la descripción legible                            |

---

## 🛠️ Primeros pasos

### 📋 Requisitos

- [.NET SDK 7.0 o superior](https://dotnet.microsoft.com/en-us/download)

### 📦 Instalación

Puedes agregar **MyScheduler** vía NuGet (próximamente) o incluir el proyecto manualmente en tu solución.
** git clone https://github.com/daavidpelaeez/MyScheduler.git **


---

## 🧪 Testing

- Pruebas unitarias e integración para todos los tipos de recurrencia (Daily, Weekly, Monthly)
- Ejemplos y casos en el proyecto `SchedulerTests`
- **Cobertura >95%** en lógica central y validadores

---

## 📚 Documentación

- Ejemplos de configuración en el código fuente
- [Explora el repositorio en GitHub](https://github.com/daavidpelaeez/MyScheduler)

---

## 🖼 Ejemplo UI (Recurrencia Mensual)

<img width="748" height="765" alt="image" src="https://github.com/user-attachments/assets/4096f157-f097-47f8-bdc9-db930d981e57" />

---

