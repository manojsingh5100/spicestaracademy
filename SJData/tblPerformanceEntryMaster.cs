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
    
    public partial class tblPerformanceEntryMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPerformanceEntryMaster()
        {
            this.tblPerformanceParameterTypeResultMasters = new HashSet<tblPerformanceParameterTypeResultMaster>();
        }
    
        public int Id { get; set; }
        public int RegistrationNo { get; set; }
        public int ReviewId { get; set; }
        public Nullable<int> tblWeeklyReviewMaster { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public int tblPerformanceMasterId { get; set; }
        public Nullable<int> tblWeeklyReviewMasterId { get; set; }
        public Nullable<int> BatchId { get; set; }
        public Nullable<int> AddmissionMasterId { get; set; }
        public string Percentage { get; set; }
        public string PerformanceReview { get; set; }
    
        public virtual AddmissionMaster AddmissionMaster { get; set; }
        public virtual BatchMaster BatchMaster { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual tblReviewMaster tblReviewMaster { get; set; }
        public virtual tblPerformanceMaster tblPerformanceMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPerformanceParameterTypeResultMaster> tblPerformanceParameterTypeResultMasters { get; set; }
        public virtual tblWeeklyReviewMaster tblWeeklyReviewMaster1 { get; set; }
        public virtual tblWeeklyReviewMaster tblWeeklyReviewMaster2 { get; set; }
    }
}