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
            Console.WriteLine("Dla pierwszego dnia w miesiącu podaj jego nr porządkowy w tygodniu (pn=1, ..., nd=7):");
            int firstday = int.Parse(Console.ReadLine());

            StreamReader sr = new StreamReader("harm.in");
            int n = new StreamReader("harm.in").ReadToEnd().Split(new char[] { '\n' }).Length;
            List<Day> ListOfDays = Day.WriteDays(sr, n, firstday);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Z pliku harm.in wczytałem następujące dane: ");
            Console.WriteLine($"Liczba dni w miesiącu: {n}");
            for (int i = 0; i < n; i++)
            {
                
                Console.WriteLine($"=> dzień miesiąca: {ListOfDays[i].month}, dzień tygodnia: {ListOfDays[i].whatday()}, czy pracujący: {ListOfDays[i].working}, liczba przepracowanych godzin: {ListOfDays[i].hours}, liczba godzin: {ListOfDays[i].normal_hours}, liczba nadgodzin: {ListOfDays[i].over_hours}, czas odpoczynku: {ListOfDays[i].timebetween}.");
            }
            
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

        private static object freetime_minimum(List<Day> listOfDays)
        {
            foreach (var day in listOfDays)
            {
                if (day.timebetween < 11)
                    return "nie";
            }
            return "tak";
        }

        private static object sum_over_hours(List<Day> listOfDays)
        {
            int sumoverhours=0;
            foreach (var day in listOfDays)
            {
                sumoverhours+=day.over_hours;
            }
            return sumoverhours;
        }

        public static string overtime_month(List<Day> listOfDays)
        {
            int working_day = 0;
            int working_hours=0;
            foreach (var day in listOfDays)
            {
                working_day++;
            }
            foreach (var day in listOfDays)
            {
                working_hours += day.hours;
            }


            return (working_day*8 < working_hours) ? "tak" : "nie";
        }

        private static string sunday_work(List<Day> listOfDays)
        {
            int working_sunday = 0;
            foreach (var day in listOfDays)
            {
                if (day.week == 7 && day.hours != 0)
                    return "tak";
            }
            return "nie";
        }
    }
}