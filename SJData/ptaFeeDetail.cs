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
    
    public partial class ptaFeeDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ptaFeeDetail()
        {
            this.ptaFeePaymentDetails = new HashSet<ptaFeePaymentDetail>();
        }
    
        public int Id { get; set; }
        public int PTAPaymentTypeId { get; set; }
        public int PTAFeeTypeDetailId { get; set; }
        public int PTAAdmissionMasterId { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ptaAdmissionMaster ptaAdmissionMaster { get; set; }
        public virtual ptaFeeTypeDetail ptaFeeTypeDetail { get; set; }
        public virtual ptaPaymentType ptaPaymentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaFeePaymentDetail> ptaFeePaymentDetails { get; set; }
    }
}
