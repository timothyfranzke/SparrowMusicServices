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
    
    public partial class ARTIST_BLOG
    {
        public ARTIST_BLOG()
        {
            this.SPRW_USER = new HashSet<SPRW_USER>();
        }
    
        public int BLOG_ID { get; set; }
        public string BLOG { get; set; }
        public int ARTIST_ID { get; set; }
        public bool ACT_IND { get; set; }
        public int LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
    
        public virtual SPRW_ARTIST SPRW_ARTIST { get; set; }
        public virtual ICollection<SPRW_USER> SPRW_USER { get; set; }
    }
}
