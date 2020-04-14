using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationInterface.Models
{


    public enum LeadSource
    {
        Television = 1,
        Newspaper,
        Radio,
        Hoarding,
        Google,
        Facebook,
        Email,
        Female,
        SMS,
        Referral,
        Employee,
        Friend,
        Consultant
    }

    public enum ResultStatus
    {
        Awaited = 1,
        Declared = 2,
        Pursuing
    }

    public enum Stream
    {
        Science = 1
    }

    public enum Title
    {
        Mr = 1,
        Ms,
        Mrs,
        Dr
    }

    public enum EvaluationType
    {
        Percentage = 1,
        CGPAoutof10,
        CGPAoutof9,
        CGPAoutof7,
        CGPAoutof4
    }

    public enum Gender
    {
        Male = 1,
        Female,
        ThirdGender
    }

    public enum MedicalStatus
    {
        Class1MedicalAssessmentAvailable = 1,
        Class2MedicalAssessmentAvailable,
        NotApplied
    }

    public enum Nationality
    {
        IndianNational = 1,
        OverseasCitizenofIndia
    }

}