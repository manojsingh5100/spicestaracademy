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
    
    public partial class ExceededFeeAmountOnCourseChange
    {
        public int Id { get; set; }
        public decimal ExceedAmount { get; set; }
        public int RegistrationNo { get; set; }
        public int AdmissionId { get; set; }
        public int OldFeeDetailId { get; set; }
        public int FeeDetailId { get; set; }
        public System.DateTime LogDate { get; set; }
        public int EnteredBy { get; set; }
    
        public virtual AddmissionMaster AddmissionMaster { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual FeeDetail FeeDetail { get; set; }
        public virtual FeeDetail FeeDetail1 { get; set; }
    }
}
