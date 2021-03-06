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
    
    public partial class FIN_FeeReceiptMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FIN_FeeReceiptMaster()
        {
            this.FIN_FeeReceiptDetail = new HashSet<FIN_FeeReceiptDetail>();
        }
    
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public Nullable<int> AdmissionNo { get; set; }
        public Nullable<int> LocationCode { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string PaymentMode { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public string Remarks { get; set; }
    
        public virtual AddmissionMaster AddmissionMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FIN_FeeReceiptDetail> FIN_FeeReceiptDetail { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
    }
}
