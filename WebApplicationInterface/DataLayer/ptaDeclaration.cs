//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationInterface.DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class ptaDeclaration
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public string ParentName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int ptaRegistrationInfoId { get; set; }
    
        public virtual ptaRegistrationInfo ptaRegistrationInfo { get; set; }
    }
}