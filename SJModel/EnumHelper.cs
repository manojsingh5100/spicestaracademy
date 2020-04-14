using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SJModel
{
    public enum RemarksOfMedical
    {
        Medical_not_cleared = 1,
        Withdrawal = 2,
        Others_with_Free_Text = 3
    }

    public enum MedicalStatus
    {
        FIT,
        Withdrawn,
        TMU,
        SBY
    }

    public enum MonthList
    {
        All,
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public enum CompareType
    {
        Month_By_Month = 1,
        Year_By_Year = 2,
        Quraterly_Month_By_Month = 3
        //Quarterly_Year_By_Year = 4
    }

    public enum MonthQurterList
    {
        Jan_Feb_Mar = 1,
        Apr_May_Jun = 2,
        Jul_Aug_Sep = 3,
        Oct_Nov_Dec = 4
    }

    public enum PrameterType
    {
        Punctuality = 1,
        Grooming,
        Body_language,
        Discipline,
        Communication,
        Assessments
    }

    public enum PrameterList
    {
        SSAP00 = 1,
        SSAG00,
        SSAB00,
        SSAD00,
        SSAC00,
        SSAA00
    }
}
