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
    
    public partial class tblPerformanceParameterTypeResultMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPerformanceParameterTypeResultMaster()
        {
            this.tblPerformanceParameterResultMasters = new HashSet<tblPerformanceParameterResultMaster>();
        }
    
        public int Id { get; set; }
        public int tblPerformanceEntryMasterId { get; set; }
        public int tblPerformanceMasterId { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public int ParameterTypeId { get; set; }
    
        public virtual tblParameterType tblParameterType { get; set; }
        public virtual tblPerformanceEntryMaster tblPerformanceEntryMaster { get; set; }
        public virtual tblPerformanceMaster tblPerformanceMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPerformanceParameterResultMaster> tblPerformanceParameterResultMasters { get; set; }
        public virtual UserLogin UserLogin { get; set; }
    }
}
