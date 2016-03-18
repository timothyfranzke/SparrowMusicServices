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
    
    public partial class SPRW_ALBUM
    {
        public SPRW_ALBUM()
        {
            this.SPRW_TRACK = new HashSet<SPRW_TRACK>();
            this.SPRW_ALBUM_IMG = new HashSet<SPRW_ALBUM_IMG>();
        }
    
        public int ALBUM_ID { get; set; }
        public int ARTIST_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRP { get; set; }
        public bool ACT_IND { get; set; }
        public System.DateTime RELEASE_DATE { get; set; }
        public string LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
    
        public virtual SPRW_ARTIST SPRW_ARTIST { get; set; }
        public virtual ICollection<SPRW_TRACK> SPRW_TRACK { get; set; }
        public virtual ICollection<SPRW_ALBUM_IMG> SPRW_ALBUM_IMG { get; set; }
    }
}
