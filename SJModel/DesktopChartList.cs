using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class DesktopChartList
    {
        public int TotalRegistartion { get; set; }
        public int TotalAdmission { get; set; }
        public int TotalMedical { get; set; }
        public int TotalScreenning { get; set; }
        public List<RegisterByMonth> TotalRegByMonth { get; set; }
        public List<AxisPoint> MapList { get; set; }
        public string Map { get; set; }

        public IEnumerable<LogActivityViewModel> GetActivityList { get; set; }
    }

    public class RegisterByMonth
    {
        public int NoOfRegister { get; set; }
        public int MonthNum { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
    }
    public class AxisPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public static class Axis
    {
        public static string JConversion(dynamic list)
        {
            return JsonConvert.SerializeObject(list);
        }

        public static BarChartViewModel BarPoints(List<AxisPoint> AxList)
        {
            BarChartViewModel list = new BarChartViewModel();
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            string[] label = new string[days];
            decimal[] Value = new decimal[days];
            int j = 0;
            for (int i = 1; i <= days; i++)
            {
                label[j] = i.ToString();
                var data = AxList.Where(x => x.X == i).FirstOrDefault();
                if (data != null)
                    Value[j] = data.Y;
                else
                    Value[j] = 0;
                j++;
            }
            list.BarChartLbl = label;
            list.DataY = Value;
            return list;
        }

        public static List<AxisPoint> Points(List<AxisPoint> AxList)
        {
            List<AxisPoint> list = new List<AxisPoint>();
            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++){
                int Ay = 0;
                var data = AxList.Where(x => x.X == i).FirstOrDefault();
                if (data != null)
                    Ay = data.Y;                   
                list.Add(new AxisPoint
                {
                    X = i,
                    Y = Ay
                });
            }
            return list;
        }
    }
}
