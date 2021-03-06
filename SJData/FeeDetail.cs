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
    
    public partial class FeeDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FeeDetail()
        {
            this.ExceededFeeAmountOnCourseChanges = new HashSet<ExceededFeeAmountOnCourseChange>();
            this.ExceededFeeAmountOnCourseChanges1 = new HashSet<ExceededFeeAmountOnCourseChange>();
            this.FeePaymentDetails = new HashSet<FeePaymentDetail>();
            this.FeeSettlementOnCourseChnageLogs = new HashSet<FeeSettlementOnCourseChnageLog>();
            this.FeeSettlementOnCourseChnageLogs1 = new HashSet<FeeSettlementOnCourseChnageLog>();
        }
    
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public int FeeTypeDetailId { get; set; }
        public int RegistrationMasterId { get; set; }
        public int RegistrationNo { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> FeeSettlementLogId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExceededFeeAmountOnCourseChange> ExceededFeeAmountOnCourseChanges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExceededFeeAmountOnCourseChange> ExceededFeeAmountOnCourseChanges1 { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual FeeSettlementOnCourseChnageLog FeeSettlementOnCourseChnageLog { get; set; }
        public virtual FeeTypeDetail FeeTypeDetail { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual RegistrationMaster RegistrationMaster { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeePaymentDetail> FeePaymentDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeSettlementOnCourseChnageLog> FeeSettlementOnCourseChnageLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeSettlementOnCourseChnageLog> FeeSettlementOnCourseChnageLogs1 { get; set; }
    }
}
