using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Globalization;

namespace Walidator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("To jest walidator harmonogramu czasu pracy.");

            // wczytanie od użytkownika potrzebnych informacji - roku i miesiąca (z zabezpieczeniem)
            int year;
            int month;
            do
            {
                Console.WriteLine("Podaj rok w formacie YYYY:");
                year = int.Parse(Console.ReadLine());
            } while (year <= 1980 || year >= 2050);
            do
            {
                Console.WriteLine("Podaj miesiąc w formacie MM:");
                month = int.Parse(Console.ReadLine());
            } while (month < 1 || month > 12);

            // znalezienie dla pierwszego dnia w miesiącu jego numeru tygodniowego (0-nd, ..., 6-sb)
            DateTime date = new DateTime(year, month, 1); // utworzenie daty w formacie typu {01.05.2023 00:00:00}
            string dayName = date.ToString("dddd", new CultureInfo("en-US")); // zwrócenie nazwy dnia typu string "Monday"
            DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayName); // nazwy dnia tygodnia na typ wyliczeniowy 'DayOfWeek'
            int dayNumber = (int)dayOfWeek; // jawna konwersja nazwy dnia na typ int

            // wczytanie danych z pliku
            int n = 0;
            List<Day> ListOfDays = null;
            try
            {
                StreamReader sr = new StreamReader("harm.in");
                n = new StreamReader("harm.in").ReadToEnd().Split(new char[] { '\n' }).Length; // zliczenie linijek pliku
                ListOfDays = Day.WriteDays(sr, n, dayNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BŁĄD WCZYTANIA PLIKU: {ex.Message}");
            }
            
            
            // wyrzucenie na ekran danych dla sprawdzenia poprawności, potrzebne do testów jednostkowych..
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Z pliku harm.in wczytałem następujące dane: ");
            Console.WriteLine($"Liczba dni w miesiącu: {n}");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"=> dzień miesiąca: {ListOfDays[i].month}, dzień tygodnia: {ListOfDays[i].whatday()}, czy pracujący: {ListOfDays[i].working}, liczba przepracowanych godzin: {ListOfDays[i].hours}, liczba godzin: {ListOfDays[i].normal_hours}, liczba nadgodzin: {ListOfDays[i].over_hours}, czas odpoczynku: {ListOfDays[i].timebetween}.");
            }
            
            // wyrzucenie na ekran wyników walidacji
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Walidacje: ");
            Console.WriteLine("1) Czy suma przepracowanych godzin w miesiącu przekracza ilość [8h*liczba dni pracujących]?");
            Console.WriteLine($"Odp: {Program.overtime_month(ListOfDays)}");
            Console.WriteLine("2) Czy została zaplanowana praca na niedzielę?");
            Console.WriteLine($"Odp: {Program.sunday_work(ListOfDays)}");
            Console.WriteLine("3) Ile godzin nadgodzin?");
            Console.WriteLine($"Odp: {Program.sum_over_hours(ListOfDays)}");
            Console.WriteLine("4) Czy pomiędzy końcem jednego dnia, a początkiem następnego jest co najmniej 11h przerwy?");
            Console.WriteLine($"Odp: {Program.freetime_minimum(ListOfDays)}");
            Console.WriteLine("-----------------------------------------------------------------------");
        }

        private static object freetime_minimum(List<Day> listOfDays) //sprawdza, czy między końcem pierwszego, a początiem drugiego dnia pracy mija min. 11h
        {
            foreach (var day in listOfDays)
            {
                if (day.timebetween < 11)
                    return "nie";
            }
            return "tak";
        }

        private static object sum_over_hours(List<Day> listOfDays) // zsumowanie nadgodzin w skali miesiąca
        {
            int sumoverhours = 0;
            foreach (var day in listOfDays)
            {
                sumoverhours += day.over_hours;
            }
            return sumoverhours;
        }

        public static string overtime_month(List<Day> listOfDays)
        {
            int working_day = 0;
            int working_hours = 0;
            foreach (var day in listOfDays)
            {
                if(day.working)
                    working_day++; // zlicza ilość dni pracujących
                working_hours += day.hours; // zlicza liczbę przepracowanych godzin
            }
            return (working_day*8 < working_hours) ? "tak" : "nie";
        }

        private static string sunday_work(List<Day> listOfDays) // sprawdza, czy w niedziele pracowano
        {
            foreach (var day in listOfDays)
            {
                if (day.week==0 && day.hours != 0)
                    return "tak";
            }
            return "nie";
        }
    }
}
