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
            int firstday = Day.read_year_month(); 

            // wczytanie danych z pliku
            int n = 0;
            List<Day> ListOfDays = null;
            try
            {
                StreamReader sr = new StreamReader("harm.in");
                n = new StreamReader("harm.in").ReadToEnd().Split(new char[] { '\n' }).Length; // zliczenie linijek pliku
                ListOfDays = Day.WriteDays(sr, n, firstday); 
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
                Console.WriteLine($"=> dzień miesiąca: {ListOfDays[i].month}, dzień tygodnia: {ListOfDays[i].whatday()}, czy pracujący: {ListOfDays[i].working()}, liczba przepracowanych godzin: {ListOfDays[i].hours}, liczba godzin: {ListOfDays[i].normal_hours()}, liczba nadgodzin: {ListOfDays[i].over_hours()}, czas odpoczynku: {ListOfDays[i].timebetween()}.");
            }
            
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
