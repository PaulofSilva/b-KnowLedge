using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;

namespace b_KnowLedge.ViewModels
{

    /// <summary>
    /// Class do CRUD dos Subtipos.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade Subtipos.
    /// </remarks>
    class Subtipo
    {


        /// <summary>
        /// Obtém a lista completa de Subtipos.
        /// </summary>
        /// <returns>Retorna a lista da Entidade Subtipos da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todos os Subtipos da Base de Dados, coloca o resultado dessa pesquisa numa List de Subtipos,
        /// e retorna essa List.
        /// </remarks>
        public List<Subtipos> getSubtipo()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var subtype = (from u in bd.Subtipos
                        select u);

            return subtype.ToList<Subtipos>();
        }


        /// <summary>
        /// Obtém os dados de um determinado Subtipo.
        /// </summary>
        /// <param name="id">Recebe o id do Subtipo que se pretende obter os dados.</param>
        /// <returns>Retorna os dados do referido Subtipo.</returns>
        /// <remarks>
        /// Recebe um id de um Subtipo que se pretende obter os dados do mesmo. É feita uma pesquisa pelo Subtipo à
        /// Base de Dados, e caso o encontre retorna os dados desse Subtipo.
        /// </remarks>
        public Subtipos getSubtipoDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var subtype = (from u in bd.Subtipos
                        where u.StampSubtipo == id
                        select u);

            return subtype.FirstOrDefault();
        }


        /// <summary>
        /// Faz o update aos dados de um determinado Subtipo.
        /// </summary>
        /// <param name="id">Recebe o id do Subtipo que se pretende fazer o update.</param>
        /// <param name="ls">Recebe os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe um id do Subtipo e os respectivos dados, obtém o Subtipo, de seguida percorre a List e faz um update ao Subtipo do id recebido.
        /// </remarks>
        public bool UpdateSubtipo(string Id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Subtipos subtype = bd.Subtipos.Single(u => u.StampSubtipo == Id);
            bool done = true;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();
            int i = 0;

            try
            {
                foreach (string res in ls)
                {
                    switch (i)
                    {

                        case 0:
                            subtype.StampSubtipo = Id;
                            break;
                        case 1:
                            subtype.StampTipo = res;
                            break;
                        case 2:
                            subtype.Nome = res;
                            break;
                        case 3:
                            subtype.Descricao = res;
                            break;
                        case 4:
                            subtype.Ousrinis = subtype.Ousrinis;
                            break;
                        case 5:
                            subtype.Ousrdata = subtype.Ousrdata;
                            break;
                        case 6:
                            subtype.Ousrhora = subtype.Ousrhora;
                            break;
                        case 7:
                            subtype.Usrinis = Global.idUser;
                            break;
                        case 8:
                            subtype.Usrdata = dt;
                            break;
                        case 9:
                            subtype.Usrhora = dataControl.GeraHora(dt);
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



        /// <summary>
        /// Insere um Subtipo na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados do Subtipo que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados do Subtipo que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um dos parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertSubtipo(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Subtipos subtype = new Subtipos();
            bool done = true;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            int i = 0;

            try{

                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                            if (res.Trim() == "")
                                subtype.StampSubtipo = dataControl.GenerateStamp();
                            else
                                subtype.StampSubtipo = res;
                            break;
                        case 1:
                            subtype.StampTipo = res;
                            break;
                        case 2:
                            subtype.Nome = res;
                            break;
                        case 3:
                            subtype.Descricao = res;
                            break;
                        case 4:
                            subtype.Ousrinis = Global.idUser;
                            break;
                        case 5:
                            subtype.Ousrdata = dt;
                            break;
                        case 6:
                            subtype.Ousrhora = dataControl.GeraHora(dt);
                            break;
                        case 7:
                            subtype.Usrinis = Global.idUser;
                            break;
                        case 8:
                            subtype.Usrdata = dt;
                            break;
                        case 9:
                            subtype.Usrhora = dataControl.GeraHora(dt);
                            break;
                    }
                    i++;
                }

                    bd.Subtipos.Add(subtype);

                    bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;

        }


        public bool Delete_Subtipo(string Id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Subtipos subtype = bd.Subtipos.Single(u => u.StampSubtipo == Id);
                bd.Subtipos.Remove(subtype);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }


        public string[] NomeSubtipo(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var nome = (from u in bd.Subtipos
                        where u.StampSubtipo == id
                        select u);


            return nome.Select(x => x.Nome).ToArray();
        }


        public List<Subtipos> Tipo_Subtipo(string id_tipo)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var subtype = (from u in bd.Subtipos
                       where u.StampTipo == id_tipo
                       select u);

            return subtype.ToList<Subtipos>();
        }


        public string getIDFromNomeSubtipos(string name, string stampType)
        {
            string stamp = "";
            BDKnowLedge bd = new BDKnowLedge();

            var id = (from u in bd.Subtipos
                      where u.Nome == name && u.StampTipo==stampType
                      select u);

            try
            {
                Subtipos f = id.FirstOrDefault();
                stamp = f.StampSubtipo;
            }
            catch { }

            return stamp;
        }


        public List<Subtipos> Search_Subtipo(List<Conhecimentos> know)
        {

            BDKnowLedge bd = new BDKnowLedge();

            var subtype =
               from g in
                   (from c in know
                    group c by c.StampSubtipo)
               select new { g.Key, Count = g.Count() };

            var numeros = subtype.Select(x => x.Key).ToArray();


            var fil3 = (from u in bd.Subtipos
                        where numeros.Contains(u.StampSubtipo)
                        select u);

            return fil3.ToList<Subtipos>();

        }


        public IEnumerable<object> TipoSubtipoGrid()
        {

            BDKnowLedge bd = new BDKnowLedge();
            
            var tipo_subtipo = (from u in bd.Subtipos
                                join t in bd.Tipos on u.StampTipo equals t.StampTipo
                                select new
                                {
                                    StampTipo = t.StampTipo,
                                    StampSubtipo = u.StampSubtipo,
                                    NomeTipo = t.Nome,
                                    NomeSubtipo = u.Nome,
                                    Descricao = u.Descricao
                                }
                                ).Distinct();

            IEnumerable<object> ie = tipo_subtipo.ToList();

            return ie;
        }


        public IEnumerable<object> SearchTipoSubtipoGrid(string sub)
        {

            BDKnowLedge bd = new BDKnowLedge();

            var tipo_subtipo = (from u in bd.Subtipos
                                join t in bd.Tipos on u.StampTipo equals t.StampTipo
                                where u.Nome.Contains(sub)
                                select new
                                {
                                    StampTipo = t.StampTipo,
                                    StampSubtipo = u.StampSubtipo,
                                    NomeTipo = t.Nome,
                                    NomeSubtipo = u.Nome,
                                    Descricao = u.Descricao
                                }
                                ).Distinct();

            IEnumerable<object> ie = tipo_subtipo.ToList();

            return ie;
        }

    }
}
