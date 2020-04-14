using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJService
{
    public abstract class InitialClogitory
    {
        public abstract string IsStudentExistByEmail(string Email, string Mobile);
        public abstract bool IsManualExistByEmail(string Email, string Mobile);
        public int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days;
        }
    }
}
