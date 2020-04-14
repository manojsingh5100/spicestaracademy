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
    
    public partial class ptaAdmissionMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ptaAdmissionMaster()
        {
            this.ptaDocumentDetails = new HashSet<ptaDocumentDetail>();
            this.ptaFeeDetails = new HashSet<ptaFeeDetail>();
            this.ptaRegistrationInfoes = new HashSet<ptaRegistrationInfo>();
        }
    
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lanme { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<double> Height { get; set; }
        public int CourseId { get; set; }
        public int SessionId { get; set; }
        public string ApplicationNo { get; set; }
        public string SerialNo { get; set; }
        public string OrderId { get; set; }
        public int AdmissionBy { get; set; }
        public System.DateTime AdmissionDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual CourseMaster CourseMaster { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual SessionMaster SessionMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaDocumentDetail> ptaDocumentDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaFeeDetail> ptaFeeDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaRegistrationInfo> ptaRegistrationInfoes { get; set; }
    }
}
