//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Entidades
    {
        public Entidades()
        {
            this.Pessoas = new HashSet<Pessoas>();
            this.Projetos = new HashSet<Projetos>();
        }
    
        public string StampEntidade { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string CodPostal { get; set; }
        public string Telemovel { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string Numero { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string Ousrinis { get; set; }
        public Nullable<System.DateTime> Ousrdata { get; set; }
        public string Ousrhora { get; set; }
        public string Usrinis { get; set; }
        public Nullable<System.DateTime> Usrdata { get; set; }
        public string Usrhora { get; set; }
        public Nullable<System.DateTime> DataSincronizacao { get; set; }
    
        public virtual Utilizadores Utilizadores { get; set; }
        public virtual Utilizadores Utilizadores1 { get; set; }
        public virtual ICollection<Pessoas> Pessoas { get; set; }
        public virtual ICollection<Projetos> Projetos { get; set; }
    }
}
