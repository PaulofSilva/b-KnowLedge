using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using b_KnowLedge.Classes;
using b_KnowLedge.Models;

namespace b_KnowLedge.ViewModels
{
    /// <summary>
    ///     Class do CRUD do Conhecimento.
    /// </summary>
    /// ///
    /// <remarks>
    ///     Controla o create, read, update e delete da entidade Conhecimento.
    /// </remarks>
    internal class Conhecimento
    {
        private string _returnStamp = "";

        /// <summary>
        ///     Obtém a lista completa do conhecimento.
        /// </summary>
        /// <returns>Retorna uma lista completa dos conhecimentos.</returns>
        /// <remarks>
        ///     Faz uma pesquisa de todo os dados da tabela Conhecimento da Base de Dados, coloca o resultado dessa pesquisa numa List de Conhecimentos,
        ///     e retorna essa List.
        /// </remarks>
        public List<Conhecimentos> GetConhecimento()
        {
            var bd = new BDKnowLedge();

            IQueryable<Conhecimentos> conhecimento = (from u in bd.Conhecimentos
                                                      select u);

            return conhecimento.ToList();
        }

        public IQueryable<object> GetConhecimentoGrid()
        {
            var bd = new BDKnowLedge();

            IQueryable<object> tr = (from r in bd.Conhecimentos
                                     join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                     join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                     join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                     join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                     join ent in bd.Entidades on s.StampEntidade equals ent.StampEntidade
                                     select new
                                         {
                                             ID_Conhecimento = r.StampConhecimento,
                                             NomeEntidade = ent.Nome,
                                             NomeProjeto = s.Nome,
                                             NomeTipo = t.Nome,
                                             NomeSubtipo = q.Nome,
                                             r.Metadados,
                                             Codigo = r.Codigo.Substring(0, 25),
                                             dataUpdate = r.Usrdata,
                                             dataActualiza = r.DataSincronizacao,
                                             user = u.Nome
                                         }).Take(200);


            return tr;
        }


        public IQueryable<object> GetSearchConhecimentoGrid(string pal, int val)
        {
            var bd = new BDKnowLedge();

            if (val == 0)
            {
                var tr = (from r in bd.Conhecimentos
                                         join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                         join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                         join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                         join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                         join ent in bd.Entidades on s.StampEntidade equals ent.StampEntidade
                                         where r.Metadados.Contains(pal)
                                         select new
                                             {
                                                 ID_Conhecimento = r.StampConhecimento,
                                                 NomeEntidade = ent.Nome,
                                                 NomeProjeto = s.Nome,
                                                 NomeTipo = t.Nome,
                                                 NomeSubtipo = q.Nome,
                                                 r.Metadados,
                                                 Codigo = r.Codigo.Substring(0, 25),
                                                 dataUpdate = r.Usrdata,
                                                 dataActualiza = r.DataSincronizacao,
                                                 user = u.Nome
                                             }).Take(200);
                return tr;
            }
            else if (val == 1)
            {
                var tr = (from r in bd.Conhecimentos
                                         join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                         join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                         join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                         join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                         join ent in bd.Entidades on s.StampEntidade equals ent.StampEntidade
                                         where ent.Nome.Contains(pal)
                                         select new
                                             {
                                                 ID_Conhecimento = r.StampConhecimento,
                                                 NomeEntidade = ent.Nome,
                                                 NomeProjeto = s.Nome,
                                                 NomeTipo = t.Nome,
                                                 NomeSubtipo = q.Nome,
                                                 r.Metadados,
                                                 Codigo = r.Codigo.Substring(0, 25),
                                                 dataUpdate = r.Usrdata,
                                                 dataActualiza = r.DataSincronizacao,
                                                 user = u.Nome
                                             }).Take(200);

                return tr;
            }
            else if (val == 2)
            {
                var tr = (from r in bd.Conhecimentos
                                         join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                         join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                         join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                         join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                         join ent in bd.Entidades on s.StampEntidade equals ent.StampEntidade
                                         where s.Nome.Contains(pal)
                                         select new
                                             {
                                                 ID_Conhecimento = r.StampConhecimento,
                                                 NomeEntidade = ent.Nome,
                                                 NomeProjeto = s.Nome,
                                                 NomeTipo = t.Nome,
                                                 NomeSubtipo = q.Nome,
                                                 r.Metadados,
                                                 Codigo = r.Codigo.Substring(0, 25),
                                                 dataUpdate = r.Usrdata,
                                                 dataActualiza = r.DataSincronizacao,
                                                 user = u.Nome
                                             }).Take(200);
                return tr;
            }
            else
            {
                IQueryable<object> tr = (from r in bd.Conhecimentos
                                         join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                         join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                         join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                         join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                         join ent in bd.Entidades on s.StampEntidade equals ent.StampEntidade
                                         where r.Codigo.Contains(pal)
                                         select new
                                             {
                                                 ID_Conhecimento = r.StampConhecimento,
                                                 NomeEntidade = ent.Nome,
                                                 NomeProjeto = s.Nome,
                                                 NomeTipo = t.Nome,
                                                 NomeSubtipo = q.Nome,
                                                 r.Metadados,
                                                 Codigo = r.Codigo.Substring(0, 25),
                                                 dataUpdate = r.Usrdata,
                                                 dataActualiza = r.DataSincronizacao,
                                                 user = u.Nome
                                             }).Take(200);

                return tr;
            }
        }


        public string[] getConhecimentoAutoComplete(int val)
        {
            var bd = new BDKnowLedge();

            var tr = (from r in bd.Conhecimentos
                      join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                      join t in bd.Tipos on r.StampTipo equals t.StampTipo
                      join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                      join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                      join ent in bd.Entidades on s.StampEntidade equals ent.StampEntidade
                      select new
                          {
                              ID_Conhecimento = r.StampConhecimento,
                              NomeEntidade = ent.Nome,
                              NomeProjeto = s.Nome,
                              NomeTipo = t.Nome,
                              NomeSubtipo = q.Nome,
                              r.Metadados,
                              Codigo = r.Codigo.Substring(0, 25),
                              dataUpdate = r.Usrdata,
                              dataActualiza = r.DataSincronizacao,
                              user = u.Nome
                          }).Take(200);

            string[] pal = null;
            int i = 0;
            if (val == 0)
            {
                List<string> e2 = tr.Select(u => u.Metadados).Distinct().ToList();
                pal = new string[e2.Count];
                foreach (string e3 in e2)
                {
                    pal[i] = e3;
                    i++;
                }
            }
            else if (val == 1)
            {
                List<string> e2 = tr.Select(u => u.NomeEntidade).Distinct().ToList();
                pal = new string[e2.Count];
                foreach (string e3 in e2)
                {
                    pal[i] = e3;
                    i++;
                }
            }
            else if (val == 2)
            {
                List<string> e2 = tr.Select(u => u.NomeProjeto).Distinct().ToList();
                pal = new string[e2.Count];
                foreach (string e3 in e2)
                {
                    pal[i] = e3;
                    i++;
                }
            }


            return pal;
        }


        public Conhecimentos getConhecimentoDetails(string id)
        {
            var bd = new BDKnowLedge();

            IQueryable<Conhecimentos> conhecimento = (from u in bd.Conhecimentos
                                                      where u.StampConhecimento == id
                                                      select u);

            return conhecimento.FirstOrDefault();
        }


        public bool UpdateConhecimento(string Id, List<string> ls)
        {
            var bd = new BDKnowLedge();
            bool done = true;
            var dataControl = new DataControl();
            DateTime dt = dataControl.GeraDataHora();
            var encoding = new ASCIIEncoding();
            Conhecimentos conhecimento = bd.Conhecimentos.Single(u => u.StampConhecimento == Id);
            Byte[] bytes;
            int i = 0, nr = 0;


            try
            {
                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                            conhecimento.StampConhecimento = conhecimento.StampConhecimento;
                            break;
                        case 1:
                            conhecimento.StampProjeto = res;
                            break;
                        case 2:
                            conhecimento.StampSubtipo = res;
                            break;
                        case 3:
                            conhecimento.StampTipo = res;
                            break;
                        case 4:
                            conhecimento.Descricao = res;
                            break;
                        case 5:
                            conhecimento.Codigo = res;
                            break;
                        case 6:
                            conhecimento.Metadados = res;
                            break;
                        case 7:
                            conhecimento.Ecra = res;
                            break;
                        case 8:
                            conhecimento.Mensagem = res;
                            break;
                        case 9:
                            conhecimento.Teclas = res;
                            break;
                        case 10:
                            conhecimento.Tabela = res;
                            break;
                        case 11:
                            try
                            {
                                nr = Convert.ToInt32(res);
                                if (nr > 0)
                                    conhecimento.NivelAprovacao = nr;
                                else
                                    conhecimento.NivelAprovacao = conhecimento.NivelAprovacao;
                            }
                            catch
                            {
                                conhecimento.NivelAprovacao = conhecimento.NivelAprovacao;
                            }
                            break;
                        case 12:
                            conhecimento.Ousrinis = conhecimento.Ousrinis;
                            break;
                        case 13:
                            try
                            {
                                conhecimento.Ousrdata = conhecimento.Ousrdata;
                            }
                            catch
                            {
                            }
                            break;
                        case 14:
                            conhecimento.Ousrhora = conhecimento.Ousrhora;
                            break;
                        case 15:
                            conhecimento.Usrinis = Global.idUser;
                            break;
                        case 16:
                            try
                            {
                                conhecimento.Usrdata = dt;
                            }
                            catch
                            {
                            }
                            break;
                        case 17:
                            conhecimento.Usrhora = dataControl.GeraHora(dt);
                            break;
                    }
                    i++;
                }

                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;
        }

        public bool InsertConhecimento(List<string> ls)
        {
            bool done = true;
            var bd = new BDKnowLedge();
            var conhecimento = new Conhecimentos();
            var dataControl = new DataControl();
            DateTime dt = dataControl.GeraDataHora();
            var encoding = new ASCIIEncoding();
            Byte[] bytes = null;
            int nr = 0;

            int i = 0;
            try
            {
                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                            if (res.Trim() == "")
                            {
                                _returnStamp = dataControl.GenerateStamp();
                                conhecimento.StampConhecimento = _returnStamp;
                            }
                            else
                            {
                                _returnStamp = res;
                                conhecimento.StampConhecimento = _returnStamp;
                            }
                            break;
                        case 1:
                            conhecimento.StampProjeto = res;
                            break;
                        case 2:
                            conhecimento.StampSubtipo = res;
                            break;
                        case 3:
                            conhecimento.StampTipo = res;
                            break;
                        case 4:
                            conhecimento.Descricao = res;
                            break;
                        case 5:
                            conhecimento.Codigo = res;
                            break;
                        case 6:
                            conhecimento.Metadados = res;
                            break;
                        case 7:
                            conhecimento.Ecra = res;
                            break;
                        case 8:
                            conhecimento.Mensagem = res;
                            break;
                        case 9:
                            conhecimento.Teclas = res;
                            break;
                        case 10:
                            conhecimento.Tabela = res;
                            break;
                        case 11:
                            try
                            {
                                nr = Convert.ToInt32(res);
                                if (nr > 0)
                                    conhecimento.NivelAprovacao = nr;
                                else
                                    conhecimento.NivelAprovacao = 0;
                            }
                            catch
                            {
                                conhecimento.NivelAprovacao = 0;
                            }
                            break;
                        case 12:
                            conhecimento.Ousrinis = Global.idUser;
                            break;
                        case 13:
                            try
                            {
                                if (res.Trim() == "")
                                {
                                    conhecimento.Ousrdata = dt;
                                }
                                else
                                    conhecimento.Ousrdata = Convert.ToDateTime(res);
                            }
                            catch
                            {
                                conhecimento.Ousrdata = dt;
                            }
                            break;
                        case 14:
                            if (res.Trim() == "")
                                conhecimento.Ousrhora = dataControl.GeraHora(dt);
                            else
                                conhecimento.Ousrhora = res;
                            break;
                        case 15:
                            conhecimento.Usrinis = Global.idUser;
                            break;
                        case 16:
                            try
                            {
                                if (res.Trim() == "")
                                    conhecimento.Usrdata = dt;
                                else
                                    conhecimento.Usrdata = Convert.ToDateTime(res);
                            }
                            catch
                            {
                                conhecimento.Usrdata = dt;
                            }
                            break;
                        case 17:
                            if (res.Trim() == "")
                                conhecimento.Usrhora = dataControl.GeraHora(dt);
                            else
                                conhecimento.Usrhora = res;
                            break;
                    }
                    i++;
                }

                bd.Conhecimentos.Add(conhecimento);


                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;
        }

        public string RetornaStamp()
        {
            return _returnStamp;
        }


        public bool Delete_Conhecimento(string Id)
        {
            bool done = false;

            var anexo = new Anexo();
            anexo.DeleteAnexosbyConhecimento(Id);

            try
            {
                var bd = new BDKnowLedge();
                Conhecimentos conhecimento = bd.Conhecimentos.Single(u => u.StampConhecimento == Id);
                bd.Conhecimentos.Remove(conhecimento);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }

        public int Existe_Conhecimento_Projeto(string id_projeto)
        {
            var bd = new BDKnowLedge();

            int conhecimento = (from u in bd.Conhecimentos
                                where u.StampProjeto == id_projeto
                                select u.StampConhecimento).Count();

            int num = Convert.ToInt32(conhecimento);

            return num;
        }

        public int Existe_Conhecimento_Entidade(string id_entidade)
        {
            var bd = new BDKnowLedge();

            int conhecimento = (from u in bd.Conhecimentos
                                join p in bd.Projetos on u.StampProjeto equals p.StampProjeto
                                join e in bd.Entidades on p.StampEntidade equals e.StampEntidade
                                where e.StampEntidade == id_entidade
                                select u.StampConhecimento).Count();

            int num = Convert.ToInt32(conhecimento);

            return num;
        }

        public IQueryable<object> getConhecimentoEntidade(string id)
        {
            var bd = new BDKnowLedge();

            IQueryable<object> tr = (from r in bd.Conhecimentos
                                     join p in bd.Projetos on r.StampProjeto equals p.StampProjeto
                                     join e in bd.Entidades on p.StampEntidade equals e.StampEntidade
                                     where e.StampEntidade == id
                                     join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                     join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                     join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                     join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                     select new
                                         {
                                             NomeEntidade = e.Nome,
                                             ID_Conhecimento = r.StampConhecimento,
                                             NomeProjeto = s.Nome,
                                             NomeTipo = t.Nome,
                                             NomeSubtipo = q.Nome,
                                             r.Metadados,
                                             Codigo = r.Codigo.Substring(0, 25),
                                             dataUpdate = r.Usrdata,
                                             dataActualiza = r.DataSincronizacao,
                                             user = u.Nome
                                         }).Take(1000);


            return tr;
        }

        public IQueryable<object> getConhecimentoProjeto(string id)
        {
            var bd = new BDKnowLedge();

            IQueryable<object> tr = (from r in bd.Conhecimentos
                                     join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                     join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                     join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                     where r.StampProjeto == id
                                     join u in bd.Utilizadores on r.Usrinis equals u.StampUtilizador
                                     join e in bd.Entidades on s.StampEntidade equals e.StampEntidade
                                     select new
                                         {
                                             NomeEntidade = e.Nome,
                                             ID_Conhecimento = r.StampConhecimento,
                                             NomeProjeto = s.Nome,
                                             NomeTipo = t.Nome,
                                             NomeSubtipo = q.Nome,
                                             r.Metadados,
                                             Codigo = r.Codigo.Substring(0, 25),
                                             dataUpdate = r.Usrdata,
                                             dataActualiza = r.DataSincronizacao,
                                             user = u.Nome
                                         }).Take(200);


            return tr;
        }

        public List<Conhecimentos> Search(string pesquisa, string fil, string emp, string sub, string type, bool code)
        {
            var bd = new BDKnowLedge();

            IQueryable<Projetos> projeto = null;
            string[] words = pesquisa.Split(' ');
            IQueryable<Projetos> id = projeto;
            IEnumerable<Conhecimentos> con = null;
            IEnumerable<Conhecimentos> result1 = con;
            IEnumerable<Conhecimentos> conhecimento = con;
            int i;

            if (fil.Trim() == "" && emp.Trim() == "" && sub.Trim() == "" && type.Trim() == "")
            {
                if (!code)
                {
                    conhecimento = (from c in bd.Conhecimentos
                                    from w in words
                                    where c.Metadados.Contains(w)
                                    select c).ToList().Distinct();
                }
                else
                {
                    conhecimento = (from c in bd.Conhecimentos
                                    from w in words
                                    where c.Metadados.Contains(w) || c.Codigo.Contains(w)
                                    select c).ToList().Distinct();
                }


                return conhecimento.ToList();
            }
            else
            {
                string query = " ";
                bool entidade_p = false;

                if (fil.Trim() != "")
                {
                    query = "StampProjeto=" + "\"" + fil + "\"";
                }
                else if (emp.Trim() != "")
                {
                    entidade_p = true;
                    id = (from u in bd.Projetos
                          where u.StampEntidade == emp
                          select u);

                    if (fil.Trim() == "" && pesquisa.Trim() == "" && sub.Trim() == "" && type.Trim() == "")
                    {
                        result1 = (from r in bd.Conhecimentos
                                   join p in bd.Projetos on r.StampProjeto equals p.StampProjeto
                                   join e in bd.Entidades on p.StampEntidade equals e.StampEntidade
                                   where e.StampEntidade == emp
                                   join s in bd.Projetos on r.StampProjeto equals s.StampProjeto
                                   join t in bd.Tipos on r.StampTipo equals t.StampTipo
                                   join q in bd.Subtipos on r.StampSubtipo equals q.StampSubtipo
                                   select r);
                        return result1.ToList();
                    }
                }

                if (sub.Trim() != "")
                {
                    if (query == " ")
                        query = "StampSubtipo=" + "\"" + sub + "\"";
                    else
                        query += " && StampSubtipo=" + "\"" + sub + "\"";
                }
                else if (type.Trim() != "")
                {
                    if (query == " ")
                        query = "StampTipo=" + "\"" + type + "\"";
                    else
                        query += " && StampTipo=" + "\"" + type + "\"";
                }

                if (query != " ")
                {
                    //Com a Dinamic Linq permite usar query dinamica.
                    IQueryable<Conhecimentos> result = bd.Conhecimentos.Where(query);

                    if (pesquisa.Trim() == "")
                        return result.ToList();

                    if (!code)
                    {
                        result1 = (from c in result
                                   from w in words
                                   where c.Metadados.Contains(w)
                                   select c).ToList().Distinct();
                    }
                    else
                    {
                        result1 = (from c in result
                                   from w in words
                                   where c.Codigo.Contains(w)
                                   select c).ToList().Distinct();
                    }

                    if (entidade_p)
                    {
                        IEnumerable<Conhecimentos> result2 = (from u in result1
                                                              join p in id on u.StampProjeto equals p.StampProjeto
                                                              select u);

                        return result2.ToList();
                    }


                    return result1.ToList();
                }
                else
                {
                    IEnumerable<Conhecimentos> conhecimento2 = (from c in bd.Conhecimentos
                                                                from w in words
                                                                where c.Metadados.Contains(w)
                                                                select c).ToList().Distinct();

                    IEnumerable<Conhecimentos> conhecimento3 = (from u in conhecimento2
                                                                join p in id on u.StampProjeto equals p.StampProjeto
                                                                select u);

                    return conhecimento3.ToList();
                }
            }
        }

        public DataTable DadosImport(string idPro, string idType, string idSubtype)
        {
            var bd = new BDKnowLedge();

            var dt = new DataTable();

            dt.Columns.Add("StampConhecimento", typeof (String));
            dt.Columns.Add("data", typeof (String));

            var query = (from u in bd.Conhecimentos
                         where u.StampProjeto == idPro && u.StampTipo == idType && u.StampSubtipo == idSubtype
                         select new
                             {
                                 u.StampConhecimento,
                                 u.Usrdata,
                                 u.Usrhora
                             });

            DataRow row;
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                DateTime dateTime = Convert.ToDateTime(rowObj.Usrdata);
                string date = dateTime.ToString("dd/M/yyyy") + " " + rowObj.Usrhora;
                dt.Rows.Add(rowObj.StampConhecimento, date);
            }
            return dt;
        }
    }
}