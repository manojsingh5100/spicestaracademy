using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class RevenueReportViewModel
    {
        public RevenueReportViewModel()
        {
            BarChartData = new BarChartViewModel();
            BarChartData.BarChartLbl = new string[] {"June", "July", "August", "Setember", "October", "November", "December" };
            //BarChartData.DataX = new int[] { 55, 40, 80, 81, 56, 55, 40 };
            //BarChartData.DataY = new int[] { 27, 90, 40, 19, 86, 27, 90 };

            DonutChartData = new int[] { 30, 20, 12 };

            //LineChartData = new List<LineChartViewModel>
            //{
            //    new LineChartViewModel{ y="2011 Q1" , item1 = 2666},
            //    new LineChartViewModel{ y="2011 Q2" , item1 = 2778},
            //    new LineChartViewModel{ y="2011 Q3" , item1 = 4912},
            //    new LineChartViewModel{ y="2011 Q4" , item1 = 3767},
            //    new LineChartViewModel{ y="2012 Q1" , item1 = 6810},
            //    new LineChartViewModel{ y="2012 Q2" , item1 = 5670},
            //    new LineChartViewModel{ y="2012 Q3" , item1 = 4820},
            //    new LineChartViewModel{ y="2012 Q4" , item1 = 15073},
            //    new LineChartViewModel{ y="2013 Q1" , item1 = 10687},
            //    new LineChartViewModel{ y="2013 Q2" , item1 = 8432},
            //};
        }
        public BarChartViewModel BarChartData { get; set; }
        public int[] DonutChartData { get; set; }
        public LineChartViewModel LineChartData { get; set; }
    }
    public class BarChartViewModel
    {
        public string[] BarChartLbl { get; set; }
        public decimal[] DataX { get; set; }
        public decimal[] DataY { get; set; }
        public decimal[] DataX1 { get; set; }
        public decimal[] DataY1 { get; set; }
        public List<BarChart1ViewModel> BarChartData1 { get; set; }
        public List<BarChart1ViewModel> BarChartData2 { get; set; }

        public string SubchartString { get; set; }
        public string SubchartString1 { get; set; }
        public decimal[] PieChartData { get; set; }
        public int RegistraionNo { get; set; }
    }
    public class Line
    {
        public string Label { get; set; }
        public int[] value { get; set; }
    }
    public class LineChartViewModel
    {
        public string[] LineChartLbl { get; set; }
        public List<Line> line { get; set; }
    }

    public class BarChart1ViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }
}
