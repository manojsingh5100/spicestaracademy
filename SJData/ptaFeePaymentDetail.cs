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
    
    public partial class ptaFeePaymentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ptaFeePaymentDetail()
        {
            this.ptaFeeCollections = new HashSet<ptaFeeCollection>();
        }
    
        public int Id { get; set; }
        public int PTAFeeDetailId { get; set; }
        public int PTAPaymentModeId { get; set; }
        public string TransectionId { get; set; }
        public Nullable<int> PTABankDetailId { get; set; }
        public string RecieptNo { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ptaBankDetail ptaBankDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ptaFeeCollection> ptaFeeCollections { get; set; }
        public virtual ptaFeeDetail ptaFeeDetail { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual ptaPaymentMode ptaPaymentMode { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
    }
}
