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
    
    public partial class ptaPassport
    {
        public int Id { get; set; }
        public string PassportNo { get; set; }
        public string PassportCountry { get; set; }
        public string PlaceOfIssue { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public Nullable<System.DateTime> DateOfExpiry { get; set; }
        public int ptaRegistrationInfoId { get; set; }
    
        public virtual ptaRegistrationInfo ptaRegistrationInfo { get; set; }
    }
}
