﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sparrow_dbEntities : DbContext
    {
        public sparrow_dbEntities()
            : base("name=sparrow_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ARTIST_BLOG> ARTIST_BLOG { get; set; }
        public virtual DbSet<SPRW_ALBUM> SPRW_ALBUM { get; set; }
        public virtual DbSet<SPRW_ARTIST> SPRW_ARTIST { get; set; }
        public virtual DbSet<SPRW_ARTIST_EVENT> SPRW_ARTIST_EVENT { get; set; }
        public virtual DbSet<SPRW_ARTIST_IMG> SPRW_ARTIST_IMG { get; set; }
        public virtual DbSet<SPRW_ARTIST_MEMBER> SPRW_ARTIST_MEMBER { get; set; }
        public virtual DbSet<SPRW_GENRE> SPRW_GENRE { get; set; }
        public virtual DbSet<SPRW_POP_ARTIST> SPRW_POP_ARTIST { get; set; }
        public virtual DbSet<SPRW_POP_INDEX> SPRW_POP_INDEX { get; set; }
        public virtual DbSet<SPRW_ROLE> SPRW_ROLE { get; set; }
        public virtual DbSet<SPRW_TRACK> SPRW_TRACK { get; set; }
        public virtual DbSet<SPRW_USER> SPRW_USER { get; set; }
        public virtual DbSet<SPRW_TRACK_POPULAR_DISLIKE> SPRW_TRACK_POPULAR_DISLIKE { get; set; }
        public virtual DbSet<SPRW_TRACK_POPULAR_LIKE> SPRW_TRACK_POPULAR_LIKE { get; set; }
        public virtual DbSet<SPRW_TRACK_POPULAR_PLAY_THROUGH> SPRW_TRACK_POPULAR_PLAY_THROUGH { get; set; }
        public virtual DbSet<SPRW_TRACK_POPULAR_PLAYS> SPRW_TRACK_POPULAR_PLAYS { get; set; }
        public virtual DbSet<SPRW_TRACK_POPULAR_SELECT> SPRW_TRACK_POPULAR_SELECT { get; set; }
        public virtual DbSet<SPRW_TRACK_POPULAR_SKIPS> SPRW_TRACK_POPULAR_SKIPS { get; set; }
        public virtual DbSet<SPRW_ALBUM_IMG> SPRW_ALBUM_IMG { get; set; }
        public virtual DbSet<SPRW_MARKET_LOCATIONS> SPRW_MARKET_LOCATIONS { get; set; }
        public virtual DbSet<SPRW_MARKET_STATES> SPRW_MARKET_STATES { get; set; }
        public virtual DbSet<SPRW_USER_FILTER> SPRW_USER_FILTER { get; set; }
        public virtual DbSet<SPRW_ARTIST_SETTINGS> SPRW_ARTIST_SETTINGS { get; set; }
        public virtual DbSet<sprw_playlist> sprw_playlist { get; set; }
        public virtual DbSet<SPRW_PLAYLIST_PAGES> SPRW_PLAYLIST_PAGES { get; set; }
        public virtual DbSet<SPRW_TRACK_QUEUE> SPRW_TRACK_QUEUE { get; set; }
    }
}
