﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace b_KnowLedge.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BDKnowLedge : DbContext
    {
        public BDKnowLedge()
            : base("name=BDKnowLedge")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Anexos> Anexos { get; set; }
        public DbSet<BasesDados> BasesDados { get; set; }
        public DbSet<Conhecimentos> Conhecimentos { get; set; }
        public DbSet<Entidades> Entidades { get; set; }
        public DbSet<Pessoas> Pessoas { get; set; }
        public DbSet<Projetos> Projetos { get; set; }
        public DbSet<Subtipos> Subtipos { get; set; }
        public DbSet<Tabelas> Tabelas { get; set; }
        public DbSet<Tipos> Tipos { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
    }
}