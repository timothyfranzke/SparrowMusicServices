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
    
    public partial class SPRW_TRACK
    {
        public SPRW_TRACK()
        {
            this.SPRW_POP_INDEX = new HashSet<SPRW_POP_INDEX>();
            this.SPRW_TRACK_POPULAR_DISLIKE = new HashSet<SPRW_TRACK_POPULAR_DISLIKE>();
            this.SPRW_TRACK_POPULAR_LIKE = new HashSet<SPRW_TRACK_POPULAR_LIKE>();
            this.SPRW_TRACK_POPULAR_PLAY_THROUGH = new HashSet<SPRW_TRACK_POPULAR_PLAY_THROUGH>();
            this.SPRW_TRACK_POPULAR_PLAYS = new HashSet<SPRW_TRACK_POPULAR_PLAYS>();
            this.SPRW_TRACK_POPULAR_SELECT = new HashSet<SPRW_TRACK_POPULAR_SELECT>();
            this.SPRW_TRACK_POPULAR_SKIPS = new HashSet<SPRW_TRACK_POPULAR_SKIPS>();
        }
    
        public int TRACK_ID { get; set; }
        public Nullable<int> ALBUM_ID { get; set; }
        public int ARTIST_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRP { get; set; }
        public bool ACT_IND { get; set; }
        public System.DateTime RELEASE_DATE { get; set; }
        public string LAST_MAINT_USER_ID { get; set; }
        public System.DateTime LAST_MAINT_TIME { get; set; }
        public Nullable<decimal> POP_INDEX { get; set; }
        public Nullable<int> MINUTES { get; set; }
        public Nullable<int> SECONDS { get; set; }
    
        public virtual SPRW_ALBUM SPRW_ALBUM { get; set; }
        public virtual SPRW_ARTIST SPRW_ARTIST { get; set; }
        public virtual ICollection<SPRW_POP_INDEX> SPRW_POP_INDEX { get; set; }
        public virtual ICollection<SPRW_TRACK_POPULAR_DISLIKE> SPRW_TRACK_POPULAR_DISLIKE { get; set; }
        public virtual ICollection<SPRW_TRACK_POPULAR_LIKE> SPRW_TRACK_POPULAR_LIKE { get; set; }
        public virtual ICollection<SPRW_TRACK_POPULAR_PLAY_THROUGH> SPRW_TRACK_POPULAR_PLAY_THROUGH { get; set; }
        public virtual ICollection<SPRW_TRACK_POPULAR_PLAYS> SPRW_TRACK_POPULAR_PLAYS { get; set; }
        public virtual ICollection<SPRW_TRACK_POPULAR_SELECT> SPRW_TRACK_POPULAR_SELECT { get; set; }
        public virtual ICollection<SPRW_TRACK_POPULAR_SKIPS> SPRW_TRACK_POPULAR_SKIPS { get; set; }
    }
}
