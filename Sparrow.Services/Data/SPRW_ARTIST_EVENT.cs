//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sparrow.Services.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class SPRW_ARTIST_EVENT
    {
        public int EVENT_ID { get; set; }
        public int ARTIST_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRP { get; set; }
        public System.DateTime EVENT_DATE { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string URL { get; set; }
        public bool ACT_IND { get; set; }
        public int LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
    
        public virtual SPRW_ARTIST SPRW_ARTIST { get; set; }
    }
}
