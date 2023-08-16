using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walidator
{
    public class Day
    {
        public int month;
        public int week;
        public bool working;
        public int hours;
        public int normal_hours;
        public int over_hours;
        public int timebetween;

        internal static List<Day> WriteDays(StreamReader sr, int n, int firstday)
        {
            string line = sr.ReadLine();
            string[] table = line.Split(", ");

            List<Day> listofdays = new List<Day>();
            int i = 0;
            while(i<=n)
            {
                Day daynew = new Day();
                daynew.month = int.Parse(table[0]);
                daynew.hours = int.Parse(table[1]);
                daynew.week = dayofweek(firstday, i); 
                daynew.working = (daynew.week >= 1 && daynew.week <=5) ? true : false;
                daynew.normal_hours = (daynew.hours <= 8) ? daynew.hours : 8;
                daynew.over_hours = (daynew.hours <= 8) ? 0 : daynew.hours-8;
                daynew.timebetween = 24 - daynew.hours;
                listofdays.Add(daynew);
                i++;
                if(i<n)
                {
                    line = sr.ReadLine();
                    table = line.Split(", ");
                }
            }
            return listofdays;
        }

        public static int dayofweek(int firstday, int i)
        {
            if (i == 0)
                return firstday;
            else
            {
                return (firstday + i) % 7;
            }
        }

        public string whatday()
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
                case 7: return "niedziela";
                default: return "Nieznany dzień";
            }
        }
    }
}
