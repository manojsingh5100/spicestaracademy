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
    
    public partial class ptaScreeningExamfailedLog
    {
        public int Id { get; set; }
        public int PTAPilotRegistrationMasterId { get; set; }
        public int PTAScreeningInfoId { get; set; }
        public int PTAScreeningTestTypeId { get; set; }
        public int PTAScreeningExamFeeInfoId { get; set; }
        public string ExamStatus { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
    
        public virtual ptaPilotRegistrationMaster ptaPilotRegistrationMaster { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual ptaScreeningInfo ptaScreeningInfo { get; set; }
        public virtual ptaScreeningTestType ptaScreeningTestType { get; set; }
        public virtual ptaScreeningExamFeeInfo ptaScreeningExamFeeInfo { get; set; }
    }
}