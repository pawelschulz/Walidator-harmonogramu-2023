using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walidator
{
    public class Day
    {
        public int month; // numer dnia w miesiącu
        public int week; // numer dnia w tygodniu
        public int hours; // liczba przepracownych godzin

        public static int read_year_month(int year, int month, bool flag)
        {
            if (flag) // flaga, by nie dublować f-cji
            {
                // znalezienie dla pierwszego dnia w miesiącu jego numeru tygodniowego (0-nd, ..., 6-sb)
                DateTime date = new DateTime(year, month, 1); // utworzenie daty w formacie typu {01.05.2023 00:00:00}
                string dayName = date.ToString("dddd", new CultureInfo("en-US")); // zwrócenie nazwy dnia typu string "Monday"
                DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayName); // nazwy dnia tygodnia na typ wyliczeniowy 'DayOfWeek'
                int dayNumber = (int)dayOfWeek; // jawna konwersja nazwy dnia na typ int
                return dayNumber;
            }
            else
                return DateTime.DaysInMonth(year, month); 
            
        }

        public static int readfromconsole(string v, string w, int start, int end)
        {
            int number;
            while (true)
            {
                Console.Write($"Podaj {v} w formacie {w}: ");
                string text = Console.ReadLine();

                if (int.TryParse(text, out number))
                {
                    if (number >= start && number <= end)
                    {
                        return number;
                    }
                    else
                    {
                        Console.WriteLine($"Niepoprawny {v}. Podaj {v} w zakresie {start}-{end}.");
                    }
                }
                else
                {
                    Console.WriteLine("Niepoprawny format danych. Podaj liczbę.");
                }
            }
        }

        public static List<Day> WriteDays(StreamReader sr, int n, int firstday, int daysinmonth)
        {
            string line = sr.ReadLine(); // odczyt linii z pliku
            string[] table = line.Split(", "); // podział linii po przecinku, zapis do tabeli

            List<Day> listofdays = new List<Day>(); // utworzenie listy dni w miesiącu
            int i = 0;
            while(i<n)
            {
                Day daynew = new Day(); // utworzenie nowego dnia (obiektu Day)
                daynew.month = int.Parse(table[0]); // przypisanie numeru w miesiącu, konwersja na int
                daynew.hours = int.Parse(table[1]); // przypisanie liczby przepracowanych godzin
                daynew.week = dayofweek(firstday, i); 
                listofdays.Add(daynew);  //dodanie do listy
                i++;
                if(i<n)
                {
                    line = sr.ReadLine();
                    table = line.Split(", ");
                }
            }

            if(testproperity(listofdays, daysinmonth))
            {
                Console.WriteLine("BŁĄD DANYCH PLIKU.");
                Environment.FailFast("Program przerwany.");
            }

            return listofdays;
        }

        private static bool testproperity(List<Day> listofdays, int daysinmonth) // test zgodności danych z pliku
        {
            int value = 0;
            value += sequencetest(listofdays);
            value += negativehours(listofdays);
            value += daynumber(listofdays, daysinmonth);
            return (value == 0) ? false : true; // jeśli gdziekolwiek wyjdzie błąd, tj. value>=1, program wyrzuci wyjątek 
        }

        private static int sequencetest(List<Day> listofdays) // sprawdzenie, czy nr dni w miesiącu są kolejne
        {
            int maxLength = listofdays.Count;
            for (int i = 0; i < maxLength; i++)
            {
                if (listofdays[i].month != i+1) // i+1, bo miesiące 1-31, a tabllice 0-30
                {
                    return 1;
                }
            }
            return 0;
        }

        private static int negativehours(List<Day> listofdays) // sprawdzenie, czy liczba przepracowanych godzin mieści się w widełkach
        {
            int maxLength = listofdays.Count;
            for (int i = 0; i < maxLength; i++)
            {
                if (listofdays[i].hours < 0 || listofdays[i].hours > 16)
                {
                    return 1;
                }
            }
            return 0;
        }

        private static int daynumber(List<Day> listofdays, int daysinmonth) // sprawdzenie, czy liczba dni odpowiada podanemu miesiącowi
        {
            if (listofdays.Count != daysinmonth)
                return 1;
            else
                return 0;
        }

        public static void testy(List<Day>? list, int n)
        {
            // wyrzucenie na ekran danych dla sprawdzenia poprawności, potrzebne do testów jednostkowych..
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Z pliku harm.in wczytałem następujące dane: ");
            Console.WriteLine($"Liczba dni w miesiącu: {n}");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"=> dzień miesiąca: {list[i].month}, dzień tygodnia: {list[i].whatday()}, czy pracujący: {list[i].working()}, liczba przepracowanych godzin: {list[i].hours}, liczba godzin: {list[i].normal_hours()}, liczba nadgodzin: {list[i].over_hours()}, czas odpoczynku: {list[i].timebetween()}.");
            }
        }

        private static int dayofweek(int firstday, int i)
        {
            if (i == 0) // jeśli pierwsza iteracja
            {
                return firstday; // przepisany pierwszy dzień miesiąca
            }
            else
            {
                return (firstday + i) % 7; // dla kolejnych dni wyliczam
            }
        }

        private string whatday() // zwraca polską nazwę dnia tygodnia
        {
            switch (this.week)
            {
                case 0: return "niedziela";
                case 1: return "poniedziałek";
                case 2: return "wtorek";
                case 3: return "środa";
                case 4: return "czwartek";
                case 5: return "piątek";
                case 6: return "sobota";
                default: return "Nieznany dzień";
            }
        }

        private bool working() // czy dzień pracujący
        {
            return (this.week >= 1 && this.week <= 5) ? true : false;
        }

        private int normal_hours() // liczba przepracowanych godzin (<= 8, pn-pt)
        {
            return this.hours - this.over_hours();
        }

        private int over_hours() // liczba nadgodzin (<= 8, pn-pt) lub godziny w niedziele
        {
            if (this.hours > 8 && (this.week >= 1 && this.week <= 6))
            {
                return this.hours - 8;
            }
            else if (this.week == 0)
            {
                return this.hours;
            }
            else
                return 0;
        }
        private int timebetween() // liczba buforu między końcem wstępnego, a początkiem kolejnego dnia
        {
            return 24 - this.hours;
        }



        // WALIDACJE
        public static string overtime_month(List<Day> listOfDays)
        {
            int working_day = 0;
            int working_hours = 0;
            foreach (var day in listOfDays)
            {
                if (day.working())
                    working_day++; // zlicza ilość dni pracujących
                working_hours += day.hours; // zlicza liczbę przepracowanych godzin
            }
            return (working_day * 8 < working_hours) ? "tak" : "nie";
        }

        public static string sunday_work(List<Day> listOfDays) // sprawdza, czy w niedziele pracowano
        {
            foreach (var day in listOfDays)
            {
                if (day.week == 0 && day.hours != 0)
                    return "tak";
            }
            return "nie";
        }

        public static int sum_over_hours(List<Day> listOfDays) // zsumowanie nadgodzin w skali miesiąca
        {
            int sumoverhours = 0;
            foreach (var day in listOfDays)
            {
                sumoverhours += day.over_hours();
            }
            return sumoverhours;
        }

        public static string freetime_minimum(List<Day> listOfDays) //sprawdza, czy między końcem pierwszego, a początiem drugiego dnia pracy mija min. 11h
        {
            foreach (var day in listOfDays)
            {
                if (day.timebetween() < 11)
                    return "nie";
            }
            return "tak";
        }
    }
}
