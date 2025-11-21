using MyScheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MyScheduler.Infraestructure.Localizer
{
    public class Localizer
    {
        public static Dictionary<string, int> Language = new Dictionary<string, int>
        {
            { "en-US", 0 },
            { "en-UK", 1 },
            { "es", 2 }
        };

        public static Dictionary<string, string[]> Translations = new Dictionary<string, string[]>
        {
            { "OccursOnce", new[] {
                "Occurs once. Schedule on {0} at {1}, starting {2}",
                "Occurs once. Schedule on {0} at {1}, starting {2}",
                "Ocurre una vez. Programado para {0} a las {1}, comienza el {2}" 
            }},
            { "DescriptionNotAvailable", new[] {
                "Description not available for this task type.",
                "Description not available for this task type.",
                "Descripción no disponible para este tipo de tarea."
            }},
            { "OccursEveryDayAt", new[] {
                "Occurs every {0} day(s) at {1}, starting {2}",
                "Occurs every {0} day(s) at {1}, starting {2}",
                "Ocurre cada {0} día(s) a las {1}, comienza el {2}"
            }},
            { "OccursEveryDayEveryUnit", new[] {
                "Occurs every {0} day(s) every {1} {2} between {3} and {4}, starting {5}",
                "Occurs every {0} day(s) every {1} {2} between {3} and {4}, starting {5}",
                "Ocurre cada {0} día(s) cada {1} {2} entre las {3} y las {4}, comienza el {5}"
            }},
            { "OccursEveryWeekOnAt", new[] {
                "Occurs every {0} week(s) on {1} at {2}, starting {3}",
                "Occurs every {0} week(s) on {1} at {2}, starting {3}",
                "Ocurre cada {0} semana(s) los {1} a las {2}, comienza el {3}"
            }},
            { "OccursEveryWeekOnEveryUnit", new[] {
                "Occurs every {0} week(s) on {1} every {2} {3} between {4} and {5}, starting {6}",
                "Occurs every {0} week(s) on {1} every {2} {3} between {4} and {5}, starting {6}",
                "Ocurre cada {0} semana(s) los {1} cada {2} {3} entre las {4} y las {5}, comienza el {6}"
            }},
            { "OccursDayEveryMonth", new[] {
                "Occurs day {0} every {1} month(s)",
                "Occurs day {0} every {1} month(s)",
                "Ocurre el día {0} cada {1} mes(es)"
            }},
            { "OccursTheOfEveryMonth", new[] {
                "Occurs the {0} {1} of every {2} month(s)",
                "Occurs the {0} {1} of every {2} month(s)",
                "Ocurre el {0} {1} de cada {2} mes(es)"
            }},
            { "AtTime", new[] {
                " at {0}",
                " at {0}",
                " a las {0}"
            }},
            { "EveryUnitBetween", new[] {
                " every {0} {1} between {2} and {3}",
                " every {0} {1} between {2} and {3}",
                " cada {0} {1} entre las {2} y las {3}"
            }},
            { "StartingOn", new[] {
                ", starting {0}",
                ", starting {0}",
                ", comienza el {0}"
            }},
            { "RecurringDescriptionNotAvailable", new[] {
                "Recurring schedule description not available.",
                "Recurring schedule description not available.",
                "Descripción de programación recurrente no disponible."
            }},
            { "NoDaysSpecified", new[] {
                "no days specified",
                "no days specified",
                "no se especificaron días"
            }},
            { "DayAndDay", new[] {
                "{0} and {1}",
                "{0} and {1}",
                "{0} y {1}"
            }},
            { "DaysAndDay", new[] {
                "{0} and {1}",
                "{0} and {1}",
                "{0} y {1}"
            }},
            { "First", new[] {
                "first",
                "first",
                "primer"
            }},
            { "Second", new[] {
                "second",
                "second",
                "segundo"
            }},
            { "Third", new[] {
                "third",
                "third",
                "tercer"
            }},
            { "Fourth", new[] {
                "fourth",
                "fourth",
                "cuarto"
            }},
            { "Last", new[] {
                "last",
                "last",
                "último"
            }},
            { "Day", new[] {
                "day",
                "day",
                "día"
            }},
            { "Weekday", new[] {
                "weekday",
                "weekday",
                "día laborable"
            }},
            { "WeekendDay", new[] {
                "weekend day",
                "weekend day",
                "fin de semana"
            }},
            { "Monday", new[] {
                "Monday",
                "Monday",
                "lunes"
            }},
            { "Tuesday", new[] {
                "Tuesday",
                "Tuesday",
                "martes"
            }},
            { "Wednesday", new[] {
                "Wednesday",
                "Wednesday",
                "miércoles"
            }},
            { "Thursday", new[] {
                "Thursday",
                "Thursday",
                "jueves"
            }},
            { "Friday", new[] {
                "Friday",
                "Friday",
                "viernes"
            }},
            { "Saturday", new[] {
                "Saturday",
                "Saturday",
                "sábado"
            }},
            { "Sunday", new[] {
                "Sunday",
                "Sunday",
                "domingo"
            }},
            { "minutes", new[] {
                "minutes",
                "minutes",
                "minutos"
            }},
            { "hours", new[] {
                "hours",
                "hours",
                "horas"
            }},
            { "seconds", new[] {
                "seconds",
                "seconds",
                "segundos"
            }}
        };

        public string GetString(string key, string language)
        {
            int languageID = Language[language];

            if (Translations.ContainsKey(key))
            {
                string[] translations = Translations[key];

                return translations[languageID];
            }

            return key;
        }

        public string FormatDate(DateTimeOffset date, ScheduleEntity scheduleConfig, string language)
        { 
            return date.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo(language));
        }

        public string FormatTime(DateTimeOffset date, string language)
        {
            return date.ToString("HH:mm", CultureInfo.GetCultureInfo(language));
        }
    }
}
