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
    
    public partial class Conhecimentos
    {
        public Conhecimentos()
        {
            this.Anexos = new HashSet<Anexos>();
        }
    
        public string StampConhecimento { get; set; }
        public string StampProjeto { get; set; }
        public string StampSubtipo { get; set; }
        public string StampTipo { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public string Metadados { get; set; }
        public string Ecra { get; set; }
        public string Mensagem { get; set; }
        public string Teclas { get; set; }
        public string Tabela { get; set; }
        public Nullable<int> NivelAprovacao { get; set; }
        public string Ousrinis { get; set; }
        public Nullable<System.DateTime> Ousrdata { get; set; }
        public string Ousrhora { get; set; }
        public string Usrinis { get; set; }
        public Nullable<System.DateTime> Usrdata { get; set; }
        public string Usrhora { get; set; }
        public Nullable<System.DateTime> DataSincronizacao { get; set; }
    
        public virtual ICollection<Anexos> Anexos { get; set; }
        public virtual Projetos Projetos { get; set; }
        public virtual Utilizadores Utilizadores { get; set; }
        public virtual Utilizadores Utilizadores1 { get; set; }
    }
}
