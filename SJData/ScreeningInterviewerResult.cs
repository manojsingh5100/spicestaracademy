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
    
    public partial class ScreeningInterviewerResult
    {
        public int Id { get; set; }
        public int ScreeningInterviewerId { get; set; }
        public bool IsResult { get; set; }
        public string Remark { get; set; }
        public int ScreeningTestId { get; set; }
    
        public virtual ScreeningInterviewer ScreeningInterviewer { get; set; }
        public virtual ScreeningTest ScreeningTest { get; set; }
    }
}