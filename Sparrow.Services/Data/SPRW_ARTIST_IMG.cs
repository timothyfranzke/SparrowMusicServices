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
    
    public partial class SPRW_ARTIST_IMG
    {
        public int ARTIST_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRP { get; set; }
        public string IMG_PATH { get; set; }
        public bool IMG_PRIMARY { get; set; }
        public bool ACT_IND { get; set; }
        public string LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
        public int IMG_ID { get; set; }
    
        public virtual SPRW_ARTIST SPRW_ARTIST { get; set; }
    }
}
