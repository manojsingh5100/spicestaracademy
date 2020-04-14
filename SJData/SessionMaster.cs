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
    
    public partial class SessionMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SessionMaster()
        {
            this.AddmissionDetails = new HashSet<AddmissionDetail>();
            this.CourseChangeHistoryDetails = new HashSet<CourseChangeHistoryDetail>();
            this.CourseChangeHistoryDetails1 = new HashSet<CourseChangeHistoryDetail>();
            this.FeeTypeDetails = new HashSet<FeeTypeDetail>();
            this.ptaAdmissionMasters = new HashSet<ptaAdmissionMaster>();
            this.ptaFeeTypeDetails = new HashSet<ptaFeeTypeDetail>();
            this.ptaPilotRegistrationMasters = new HashSet<ptaPilotRegistrationMaster>();
            this.RegistrationMasters = new HashSet<RegistrationMaster>();
        }
    
        public int Id { get; set; }
        public string SessionName { get; set; }
        public bool IsActive { get; set; }
        public int CourseMasterId { get; set; }
        public Nullable<int> SessionYr { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddmissionDetail> AddmissionDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseChangeHistoryDetail> CourseChangeHistoryDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseChangeHistoryDetail> CourseChangeHistoryDetails1 { get; set; }
        public virtual CourseMaster CourseMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeTypeDetail> FeeTypeDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaAdmissionMaster> ptaAdmissionMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaFeeTypeDetail> ptaFeeTypeDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaPilotRegistrationMaster> ptaPilotRegistrationMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegistrationMaster> RegistrationMasters { get; set; }
    }
}