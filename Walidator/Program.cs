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
            int year = Day.readfromconsole("rok", "YYYY", 1950, 2050);
            int month = Day.readfromconsole("miesiąc", "MM", 1, 12);

            // określenie danych na podstawie kalendarza
            int firstday = Day.read_year_month(year, month, true); // potrzebny pierwszy dzień miesiąca - by określić kolejne dni
            int daysinmonth = Day.read_year_month(year, month, false); // potrzebna liczba dni w miesiącu, by sprawdzić plik

            // wczytanie danych z pliku
            int n = 0;
            List<Day> ListOfDays = null;
            try
            {
                StreamReader sr = new StreamReader("harm1.in");
                n = new StreamReader("harm1.in").ReadToEnd().Split(new char[] { '\n' }).Length; // zliczenie linijek pliku
                ListOfDays = Day.WriteDays(sr, n, firstday, daysinmonth); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BŁĄD WCZYTANIA PLIKU: {ex.Message}");
            }

            Day.testy(ListOfDays, n);
            
            // wyrzucenie na ekran wyników walidacji
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Walidacje: ");
            Console.WriteLine("1) Czy suma przepracowanych godzin w miesiącu przekracza ilość [8h*liczba dni pracujących]?");
            Console.WriteLine($"Odp: {Day.overtime_month(ListOfDays)}");
            Console.WriteLine("2) Czy została zaplanowana praca na niedzielę?");
            Console.WriteLine($"Odp: {Day.sunday_work(ListOfDays)}");
            Console.WriteLine("3) Ile godzin nadgodzin?");
            Console.WriteLine($"Odp: {Day.sum_over_hours(ListOfDays)}");
            Console.WriteLine("4) Czy pomiędzy końcem jednego dnia, a początkiem następnego jest co najmniej 11h przerwy?");
            Console.WriteLine($"Odp: {Day.freetime_minimum(ListOfDays)}");
            Console.WriteLine("-----------------------------------------------------------------------");
        }
    }
}
