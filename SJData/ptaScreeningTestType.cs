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
    
    public partial class ptaScreeningTestType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ptaScreeningTestType()
        {
            this.ptaScreeningExamfailedLogs = new HashSet<ptaScreeningExamfailedLog>();
            this.ptaScreeningExamFeeInfoes = new HashSet<ptaScreeningExamFeeInfo>();
            this.ptaScreeningTestResults = new HashSet<ptaScreeningTestResult>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> InitialMark { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaScreeningExamfailedLog> ptaScreeningExamfailedLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaScreeningExamFeeInfo> ptaScreeningExamFeeInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaScreeningTestResult> ptaScreeningTestResults { get; set; }
    }
}
