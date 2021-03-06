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
    
    public partial class FeePaymentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FeePaymentDetail()
        {
            this.FeeCollections = new HashSet<FeeCollection>();
        }
    
        public int Id { get; set; }
        public int FeeDetailId { get; set; }
        public int PaymentModeId { get; set; }
        public string TransectionId { get; set; }
        public Nullable<int> FIN_BankDetailId { get; set; }
        public string RecieptNo { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeCollection> FeeCollections { get; set; }
        public virtual FeeDetail FeeDetail { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual FIN_BankDetail FIN_BankDetail { get; set; }
        public virtual PaymentMode PaymentMode { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
    }
}
