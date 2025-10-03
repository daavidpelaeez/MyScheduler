# MyScheduler

**MyScheduler** es una biblioteca de clases en C# para la gestión y planificación de tareas programadas. Actualmente soporta dos tipos principales de tareas:

- **Once:** Tareas que se ejecutan una única vez en una fecha específica.
- **Recurring:** Tareas que se ejecutan periódicamente según un intervalo definido.

---

## Descripción

Esta biblioteca permite crear y validar tareas programadas, asegurando la coherencia entre fechas de inicio, fin y ejecución. Está diseñada para integrarse fácilmente en proyectos .NET que requieran manejo de tareas temporizadas.

---

## Instalación

Clona el repositorio y compílalo con:

```bash
git clone https://github.com/daavidpelaeez/myscheduler.git
cd myscheduler
dotnet restore
dotnet build

## Tests

Los tests están escritos con xUnit y cubren:

- Validación de fechas para tareas **Once** y **Recurring**.
- Cálculo de la siguiente ejecución de tareas.
- Manejo de errores para fechas inválidas o condiciones incorrectas.

Para ejecutar los tests:

```bash
dotnet test

