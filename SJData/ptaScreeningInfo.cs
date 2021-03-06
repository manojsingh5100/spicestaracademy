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
    
    public partial class ptaScreeningInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ptaScreeningInfo()
        {
            this.ptaScreeningExamfailedLogs = new HashSet<ptaScreeningExamfailedLog>();
            this.ptaScreeningExamFeeInfoes = new HashSet<ptaScreeningExamFeeInfo>();
            this.ptaScreeningTestResults = new HashSet<ptaScreeningTestResult>();
        }
    
        public int Id { get; set; }
        public int PTAPilotRegistrationMasterId { get; set; }
        public int PTARegistrationNo { get; set; }
        public string Remark { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ptaPilotRegistrationMaster ptaPilotRegistrationMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaScreeningExamfailedLog> ptaScreeningExamfailedLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaScreeningExamFeeInfo> ptaScreeningExamFeeInfoes { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaScreeningTestResult> ptaScreeningTestResults { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
    }
}
