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
    
    public partial class SPRW_POP_ARTIST
    {
        public SPRW_POP_ARTIST()
        {
            this.SPRW_ARTIST = new HashSet<SPRW_ARTIST>();
            this.SPRW_GENRE = new HashSet<SPRW_GENRE>();
        }
    
        public int POP_ARTIST_ID { get; set; }
        public string POP_ARTIST_NAME { get; set; }
    
        public virtual ICollection<SPRW_ARTIST> SPRW_ARTIST { get; set; }
        public virtual ICollection<SPRW_GENRE> SPRW_GENRE { get; set; }
    }
}
