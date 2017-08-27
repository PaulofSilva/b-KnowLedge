using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;
using System.Data;

namespace b_KnowLedge.ViewModels
{

    /// <summary>
    /// Class do CRUD das Tabelas.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade Tabelas.
    /// </remarks>
    class Tabela
    {

        /// <summary>
        /// Obtém a lista completa de Tabelas.
        /// </summary>
        /// <returns>Retorna a lista da Entidade Tabelas da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todas as Pessoas da Base de Dados, coloca o resultado dessa pesquisa numa List de Pessoas,
        /// e retorna essa List.
        /// </remarks>
        public List<Tabelas> getTabela()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var tabela = (from u in bd.Tabelas
                        select u);

            return tabela.ToList<Tabelas>();
        }


        /// <summary>
        /// Obtém os dados de uma determinada Tabela.
        /// </summary>
        /// <param name="id">Recebe o id da Tabela que se pretende obter os dados.</param>
        /// <returns>Retorna os dados da referida Tabela.</returns>
        /// <remarks>
        /// Recebe um id de uma Tabela que se pretende obter os dados da mesma. É feita uma pesquisa pela Tabela à
        /// Base de Dados, e caso a encontre retorna os dados dessa Tabela.
        /// </remarks>
        public Tabelas getTabelaDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var tabela = (from u in bd.Tabelas
                        where u.StampTabela == id
                        select u);

            return tabela.FirstOrDefault();
        }


        /// <summary>
        /// Faz o update aos dados de uma determinada Tabela.
        /// </summary>
        /// <param name="Id">Recebe o id da Tabela que se pretende fazer o update.</param>
        /// <param name="ls">Recebe uma List com os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe um id da Tabela que se pretende actualizar e os respectivos dados, 
        /// obtém a Tabela, de seguida percorre a List e faz um update à Tabela do id recebido.
        /// </remarks>
        public bool UpdateTabela(string Id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            int i = 0, nr = 0;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            try
            {

                Tabelas tabela = bd.Tabelas.Single(u => u.StampTabela == Id);

                foreach (string res in ls)
                {
                    switch (i)
                    {
                        //case 0:
                        //    tabela.StampTabela = res;
                        //    break;
                        case 1:
                            tabela.StampTipo = res;
                            break;
                        case 2:
                            tabela.StampSubtipo = res;
                            break;
                        case 3:
                            tabela.Descricao = res;
                            break;
                        case 4:
                            tabela.QueryString = res;
                            break;
                        case 5:
                            tabela.Ousrinis = tabela.Ousrinis;
                            break;
                        case 6:
                            tabela.Ousrdata = tabela.Ousrdata;
                            break;
                        case 7:
                            tabela.Ousrhora = tabela.Ousrhora;
                            break;
                        case 8:
                            tabela.Usrinis = Global.idUser;
                            break;
                        case 9:
                            tabela.Usrdata = dt;
                            break;
                        case 10:
                            tabela.Usrhora = dataControl.GeraHora(dt);
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
        /// Insere uma Tabela na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados da Tabela que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados da Tabela que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um desses parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertTabela(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Tabelas tabela = new Tabelas();
            bool done = true;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();
          
            int i = 0, nr = 0;

            try
            {
                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                            if (res.Trim() == "")
                                tabela.StampTabela = dataControl.GenerateStamp();
                            else
                                tabela.StampTabela = res;
                            break;
                        case 1:
                            tabela.StampTipo = res;              
                            break;
                        case 2:
                            tabela.StampSubtipo = res;
                            break;
                        case 3:
                            tabela.Descricao = res;
                            break;
                        case 4:
                            tabela.QueryString = res;
                            break;
                        case 5:
                            tabela.Ousrinis = Global.idUser;
                            break;
                        case 6:
                            tabela.Ousrdata = dt;
                            break;
                        case 7:
                            tabela.Ousrhora = dataControl.GeraHora(dt);
                            break;
                        case 8:
                            tabela.Usrinis = Global.idUser;
                            break;
                        case 9:
                            tabela.Usrdata = dt;
                            break;
                        case 10:
                            tabela.Usrhora = dataControl.GeraHora(dt);
                            break;             
           
                    }
                    i++;
                }

                bd.Tabelas.Add(tabela);
            
                bd.SaveChanges();
            }
            catch 
            {
                done = false;
            }

            return done;

        }


        /// <summary>
        /// Elimina uma Tabela da Base de Dados.
        /// </summary>
        /// <param name="Id">Recebe o id da Tabela que se pretende eliminar.</param>
        /// <returns>Retorna um bool True=Removida com sucesso, False=Não conseguiu remover a Projetos.</returns>
        /// <remarks>
        /// Recebe o id da Tabela que se pretende eliminar, pesquisa na Base de Dados por essa Tabela,
        ///e caso a encontre, é eliminada. Retorna true se tiver sucesso ao remover a Tabela e retorna false caso contrário.
        /// </remarks>
        public bool Delete_Tabela(string Id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Tabelas tabela = bd.Tabelas.Single(u => u.StampTabela == Id);
                bd.Tabelas.Remove(tabela);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }


        /// <summary>
        /// Obtém as Tabelas associadas a um determinado Tipo.
        /// </summary>
        /// <param name="id_Entidade">Recebe o id do Tipo que se pretende obter as suas Tabelas.</param>
        /// <returns>Retorna uma List de Tabelas com a/as Tabela/s encontrada/s.</returns>
        /// <remarks>
        /// Recebe o id do Tipo que se pretende obter as suas Tabelas, faz uma pesquisa à Base de Dados pelas Tabelas com o ID_TIPO igual ao id
        /// recebido, e retorna esse resultado numa List de Tabelas.
        /// </remarks>
        public List<Tabelas> Tabela_Tipo(string id_tipo)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var tabela = (from u in bd.Tabelas
                       where u.StampTipo == id_tipo
                       select u);

            return tabela.ToList<Tabelas>();

        }


        /// <summary>
        /// Obtém as Tabelas associadas a um determinado Subtipo.
        /// </summary>
        /// <param name="id_Entidade">Recebe o id do Subtipo que se pretende obter as suas Tabelas.</param>
        /// <returns>Retorna uma List de Tabelas com a/as Tabela/s encontrada/s.</returns>
        /// <remarks>
        /// Recebe o id do Subtipo que se pretende obter as suas Tabelas, faz uma pesquisa à Base de Dados pelas Tabelas com o ID_SUBTIPO igual ao id
        /// recebido, e retorna esse resultado numa List de Tabelas.
        /// </remarks>
        public List<Tabelas> Tabela_Subtipo(string id_subtipo)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var tabela = (from u in bd.Tabelas
                          where u.StampSubtipo == id_subtipo
                          select u);


            return tabela.ToList<Tabelas>();

        }


        public IEnumerable<object> Tabelas_TipoSubtipo()
        {

            BDKnowLedge bd = new BDKnowLedge();

            var tipo_subtipo = (from u in bd.Subtipos
                                from t in bd.Tipos
                                select new
                                { 
                                    t.StampTipo,
                                    u.StampSubtipo,
                                    t.Nome,
                                    Name = u.Nome
                                }
                                );

             var tab = (from u in bd.Tabelas
                       from t in tipo_subtipo
                       where u.StampTipo == t.StampTipo && u.StampSubtipo == t.StampSubtipo
                       select new 
                       { 
                            STAMPTABELA = u.StampTabela,
                            t.Nome,
                            t.Name
                       }
                );

             IEnumerable<object> ie = tab.ToList();

             return ie;
        }


        public IEnumerable<object> Tabelas_TipoSubtipoID(string nr)
        {

            BDKnowLedge bd = new BDKnowLedge();

            var tipo_subtipo = (from u in bd.Subtipos
                                from t in bd.Tipos
                                select new
                                { 
                                    t.StampTipo,
                                    u.StampSubtipo,
                                    t.Nome,
                                    Name = u.Nome
                                }
                                );

             var tab = (from u in bd.Tabelas
                       from t in tipo_subtipo
                       where u.StampTipo == t.StampTipo && u.StampSubtipo == t.StampSubtipo && (u.StampTipo == nr || u.StampSubtipo == nr)
                       select new 
                       {
                           STAMPTABELA = u.StampTabela,
                            t.Nome,
                            t.Name
                       }
                );

             IEnumerable<object> ie = tab.ToList();

             return ie;
        }

        public Tabelas TabelasDadoOTipoAndSubtipo(string id_type, string id_subtype)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var tabela = (from u in bd.Tabelas
                          where u.StampTipo==id_type && u.StampSubtipo==id_subtype
                          select u).FirstOrDefault();

            return tabela;
        }

        public bool ExisteTabelasTipoSubtipo(string type, string subtype)
        {
            BDKnowLedge bd = new BDKnowLedge();

            bool done = false;

            var nr = (from u in bd.Tabelas
                where u.StampSubtipo == subtype && u.StampTipo == type
                select u.StampTabela).Count();

            try
            {
                int num = Convert.ToInt32(nr);

                if (num > 0)
                    done = true;
                else
                    done = false;
            }
            catch 
            {
                done = false;
            }


            return done;
        }

    }
}
