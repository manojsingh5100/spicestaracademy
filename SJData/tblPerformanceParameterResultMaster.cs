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
    
    public partial class tblPerformanceParameterResultMaster
    {
        public int Id { get; set; }
        public int tblPerformanceParameterTypeResultMasterId { get; set; }
        public int Rating { get; set; }
        public int tblParameterId { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public bool IsNotApplicable { get; set; }
    
        public virtual tblParameterList tblParameterList { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual tblPerformanceParameterTypeResultMaster tblPerformanceParameterTypeResultMaster { get; set; }
    }
}
