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
    
    public partial class CandidateTerminationResignationInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CandidateTerminationResignationInfo()
        {
            this.CandidateTRInfoes = new HashSet<CandidateTRInfo>();
        }
    
        public int Id { get; set; }
        public int AddmissionMasterId { get; set; }
        public int CandidateActionMasterId { get; set; }
        public bool IsActive { get; set; }
    
        public virtual AddmissionMaster AddmissionMaster { get; set; }
        public virtual CandidateActionMaster CandidateActionMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidateTRInfo> CandidateTRInfoes { get; set; }
    }
}