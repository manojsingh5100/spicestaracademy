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
    
    public partial class SessionInstallmentMaster
    {
        public int Id { get; set; }
        public int InstallmentMasterId { get; set; }
        public int FeeTypeDetailId { get; set; }
        public decimal Amount { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EneteredDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual FeeTypeDetail FeeTypeDetail { get; set; }
        public virtual InstallmentMaster InstallmentMaster { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual UserLogin UserLogin1 { get; set; }
    }
}
