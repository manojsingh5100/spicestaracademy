//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SJData
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPerformancePercentageMaster
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int tblPerformanceMasterId { get; set; }
        public int tblReviewMasterId { get; set; }
        public double FromPercentage { get; set; }
        public double TillPercentage { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual BatchMaster BatchMaster { get; set; }
        public virtual tblPerformanceMaster tblPerformanceMaster { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual tblReviewMaster tblReviewMaster { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
    }
}
