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
    
    public partial class SPRW_GENRE
    {
        public SPRW_GENRE()
        {
            this.SPRW_ARTIST = new HashSet<SPRW_ARTIST>();
            this.SPRW_POP_ARTIST = new HashSet<SPRW_POP_ARTIST>();
        }
    
        public int GENRE_ID { get; set; }
        public string GENRE { get; set; }
        public int LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
    
        public virtual ICollection<SPRW_ARTIST> SPRW_ARTIST { get; set; }
        public virtual ICollection<SPRW_POP_ARTIST> SPRW_POP_ARTIST { get; set; }
    }
}