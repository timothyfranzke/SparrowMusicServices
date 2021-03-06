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
    
    public partial class SPRW_USER
    {
        public SPRW_USER()
        {
            this.SPRW_ARTIST_MEMBER = new HashSet<SPRW_ARTIST_MEMBER>();
            this.SPRW_ARTIST = new HashSet<SPRW_ARTIST>();
            this.SPRW_USER_FILTER = new HashSet<SPRW_USER_FILTER>();
            this.ARTIST_BLOG = new HashSet<ARTIST_BLOG>();
        }
    
        public int USER_ID { get; set; }
        public string PASSWORD { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public string CC { get; set; }
        public bool ACT_IND { get; set; }
        public string LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
        public Nullable<int> SALT { get; set; }
        public Nullable<bool> PASSWORD_RESET { get; set; }
    
        public virtual ICollection<SPRW_ARTIST_MEMBER> SPRW_ARTIST_MEMBER { get; set; }
        public virtual ICollection<SPRW_ARTIST> SPRW_ARTIST { get; set; }
        public virtual ICollection<SPRW_USER_FILTER> SPRW_USER_FILTER { get; set; }
        public virtual ICollection<ARTIST_BLOG> ARTIST_BLOG { get; set; }
    }
}
