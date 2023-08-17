using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walidator
{
    public class Day
    {
        public int month; // numer dnia w miesiącu
        public int week; // numer dnia w tygodniu
        public bool working; // czy dzień pracujący
        public int hours; // liczba przepracownych godzin
        public int normal_hours; // liczba przepracowanych godzin (<= 8, pn-pt)
        public int over_hours; // liczba nadgodzin (<= 8, pn-pt) lub godziny w niedziele
        public int timebetween; // liczba buforu między końcem wstępnego, a początkiem kolejnego dnia

        internal static List<Day> WriteDays(StreamReader sr, int n, int firstday)
        {
            string line = sr.ReadLine(); // odczyt linii z pliku
            string[] table = line.Split(", "); // podział linii po przecinku, zapis do tabeli

            List<Day> listofdays = new List<Day>(); // utworzenie listy dni w miesiącu
            int i = 0;
            while(i<=n)
            {
                Day daynew = new Day(); // utworzenie nowego dnia (obiektu Day)
                daynew.month = int.Parse(table[0]); // przypisanie numeru w miesiącu, konwersja na int
                daynew.hours = int.Parse(table[1]); // przypisanie liczby przepracowanych godzin
                daynew.week = dayofweek(firstday, i); 
                daynew.working = (daynew.week >= 1 && daynew.week <=5) ? true : false;
                daynew.normal_hours = daynew.hours - daynew.over_hours;
                daynew.over_hours = overhours(daynew);
                daynew.timebetween = 24 - daynew.hours;
                listofdays.Add(daynew);  //dodanie do listy
                i++;
                if(i<n)
                {
                    line = sr.ReadLine();
                    table = line.Split(", ");
                }
            }
            return listofdays;
        }

        private static int overhours(Day dayn) // zlicza nadgodziny
        {
            if(dayn.hours > 8 && (dayn.week >= 1 && dayn.week <= 6))
            {
                return dayn.hours - 8;
            }
            else if(dayn.week == 0)
            {
                return dayn.hours;
            }
            else
                return 0;
        }

        public static int dayofweek(int firstday, int i)
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

        public string whatday() // zwraca polską nazwę dnia tygodnia
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
    }
}
