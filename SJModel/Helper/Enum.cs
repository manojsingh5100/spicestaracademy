using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.Helper
{
    public enum BBAInstallMent
    {
        [Display(Name = "InstallMent1")]
        NotStarted = 175000,
        [Display(Name = "InstallMent2")]
        InProgress = 100000,
        [Display(Name = "InstallMent3")]
        Delayed = 100000
    }

    public enum MBAInstallMent
    {
        [Display(Name = "InstallMent1")]
        NotStarted = 150000,
        [Display(Name = "InstallMent2")]
        InProgress = 150000,
    }

    public enum CourseCapitalAmount
    {
        BBA = 375000,
        MBA = 300000,
        PHT = 200000
    }
}
