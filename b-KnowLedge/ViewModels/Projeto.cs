using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;
using System.IO;

namespace b_KnowLedge.ViewModels
{

    /// <summary>
    /// Class do CRUD das Projetos.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade Projetos.
    /// </remarks>
    class Projeto
    {

        /// <summary>
        /// Obtém a lista completa de Projetos.
        /// </summary>
        /// <returns>Retorna a lista das Projetos da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todas as Projetos da Base de Dados, coloca o resultado dessa pesquisa numa List de Projetos,
        /// e retorna essa List.
        /// </remarks>
        public List<Projetos> getProjetos()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fl = (from u in bd.Projetos
                          select u);

            return fl.ToList<Projetos>();
        }


        /// <summary>
        /// Obtém os dados de uma determinada Projeto.
        /// </summary>
        /// <param name="id">Recebe o id da Projeto que se pretende obter os dados.</param>
        /// <returns>Retorna os dados da referida Projeto.</returns>
        /// <remarks>
        /// Recebe um id de uma Projeto que se pretende obter os dados da mesma. É feita uma pesquisa à tabela de Projetos
        /// da Base de Dados, e caso a encontre retorna os dados dessa Projeto.
        /// </remarks>
        public Projetos getProjetosDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var emp = (from u in bd.Projetos
                       where u.StampProjeto == id
                       select u);
           
            return emp.FirstOrDefault();
        }


        /// <summary>
        /// Faz o update aos dados de uma determinada Projeto.
        /// </summary>
        /// <param name="Id">Recebe o id da Projeto que se pretende fazer o update.</param>
        /// <param name="ls">Recebe uma List com os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe um id da Projeto que se pretende actualizar e os respectivos dados, 
        /// obtém a Projeto, de seguida percorre a List e faz um update à Projeto do id recebido.
        /// </remarks>
        public bool UpdateProjetos(string Id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            try
            {
                Projetos projeto = bd.Projetos.Single(u => u.StampProjeto == Id);


                int i = 0;


                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                            projeto.StampProjeto = Id;
                            break;
                        case 1:
                            projeto.StampEntidade = res;
                            break;
                        case 2:
                            projeto.Nome = res;
                            break;
                        case 3:
                            projeto.Morada = res;
                            break;
                        case 4:
                            projeto.Localidade = res;
                            break;
                        case 5:
                            projeto.CodPostal = res;
                            break;
                        case 6:
                            projeto.Telemovel = res;
                            break;
                        case 7:
                            projeto.Telefone = res;
                            break;
                        case 8:
                            projeto.Fax = res;
                            break;
                        case 9:
                            projeto.Email = res;
                            break;
                        case 10:
                            projeto.Site = res;
                            break;
                        case 11:
                            projeto.Descricacao = res;
                            break;
                        case 12:
                            projeto.Ousrinis = projeto.Ousrinis;
                            break;
                        case 13:
                            projeto.Ousrdata = projeto.Ousrdata;
                            break;
                        case 14:
                            projeto.Ousrhora = projeto.Ousrhora;
                            break;
                        case 15:
                            projeto.Usrinis = Global.idUser;
                            break;
                        case 16:
                            projeto.Usrdata = dt;
                            break;
                        case 17:
                            projeto.Usrhora = dataControl.GeraHora(dt);
                            break;
                        case 18:
                            try
                            {
                                Utilizador user = new Utilizador();
                                bool exist = user.UserExist(res);

                                if (exist == true)
                                    projeto.NomeConsultor = res;
                                else
                                {
                                    Utilizadores u = user.getUtilizadoresDetails(Global.idUser);
                                    projeto.NomeConsultor = u.Nome;

                                }
                            }
                            catch { }
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
        /// Insere uma Projeto na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados da Projeto que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados da Projeto que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um desses parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertProjeto(List<string> ls)
        {
          
            BDKnowLedge bd = new BDKnowLedge();
            Projetos projeto = new Projetos();
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
                            projeto.StampProjeto = dataControl.GenerateStamp();
                        else
                            projeto.StampProjeto = res;
                        break;
                    case 1:
                        projeto.StampEntidade = res;
                        break;
                    case 2:
                        projeto.Nome = res;
                        break;
                    case 3:
                        projeto.Morada = res;
                        break;
                    case 4:
                        projeto.Localidade = res;
                        break;
                    case 5:
                        projeto.CodPostal = res;
                        break;
                    case 6:
                        projeto.Telemovel = res;
                        break;
                    case 7:
                        projeto.Telefone = res;
                        break;
                    case 8:
                        projeto.Fax = res;
                        break;
                    case 9:
                        projeto.Email = res;
                        break;
                    case 10:
                        projeto.Site = res;
                        break;
                    case 11:
                        projeto.Descricacao = res;
                        break;
                    case 12:
                        projeto.Ousrinis = Global.idUser;
                        break;
                    case 13:
                        projeto.Ousrdata = dt;
                        break;
                    case 14:
                        projeto.Ousrhora = dataControl.GeraHora(dt);
                        break;
                    case 15:
                        projeto.Usrinis = Global.idUser;
                        break;
                    case 16:
                        projeto.Usrdata = dt;
                        break;
                    case 17:
                        projeto.Usrhora = dataControl.GeraHora(dt);
                        break;
                    case 18:
                        try
                        {                           
                            Utilizador user = new Utilizador();
                            bool exist = user.UserExist(res);

                            if (exist == true)
                                projeto.NomeConsultor = res;
                            else
                            {
                               Utilizadores u = user.getUtilizadoresDetails(Global.idUser);
                               projeto.NomeConsultor = u.Nome;

                            }

                        }
                        catch { }
                        break;

                }
                i++;
            }
           
            bd.Projetos.Add(projeto);

            
                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;

        }

        /// <summary>
        /// Elimina uma Projeto da Base de Dados.
        /// </summary>
        /// <param name="Id">Recebe o id da Projeto que se pretende eliminar.</param>
        /// <returns>Retorna um bool True=Removida com sucesso, False=Não conseguiu remover a Projeto.</returns>
        /// <remarks>
        /// Recebe o id da Projeto que se pretende eliminar, pesquisa na Base de Dados por essa Projeto,
        ///e caso a encontre, é eliminada. Retorna true se tiver sucesso ao remover a Projeto e retorna false caso contrário.
        /// </remarks>
        public bool Delete_Projeto(string Id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Projetos fil = bd.Projetos.Single(u => u.StampProjeto == Id);
                bd.Projetos.Remove(fil);
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
        /// Obtém as Projetos associadas a uma determinada Entidade.
        /// </summary>
        /// <param name="id_Entidade">Recebe o id da Entidade que se pretende obter as suas Projetos.</param>
        /// <returns>Retorna uma List de Projetos com a/as Projeto/ais encontrada/s.</returns>
        /// <remarks>
        /// Recebe o id da Entidade que se pretende obter as suas Projetos, faz uma pesquisa à Base de Dados pelas Projetos com o ID_EMPRESA igual ao id
        /// recebido, e retorna esse resultado numa List de Projetos.
        /// </remarks>
        public List<Projetos> Projetos_Entidade(string id_Entidade)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.Projetos
                       where u.StampEntidade == id_Entidade
                       select u);

            return fil.ToList<Projetos>();
        
        }


        public int Existe_Projetos(string id_Entidade)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.Projetos
                       where u.StampEntidade == id_Entidade
                       select u).Count();

            int num = Convert.ToInt32(fil);

            return num;

        }

        public bool ProjectExist(string nome, string idEnt)
        {
            bool done = false;
            BDKnowLedge bd = new BDKnowLedge();

            var name = (from u in bd.Projetos
                        where u.Nome.ToUpper() == nome.ToUpper()
                        && u.StampEntidade == idEnt
                        select u.StampProjeto).FirstOrDefault();

            if (name != null)
                done = true;

            return done;
        }

        public string[] NomeEntidade(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var nome = (from u in bd.Projetos
                        where u.StampProjeto == id
                        select u);


            return nome.Select(x => x.Nome).ToArray();
        }

        public string getIDFromNomeProjetos(string name)
        {
            string stamp = "";
            BDKnowLedge bd = new BDKnowLedge();

            var id = (from u in bd.Projetos
                      where u.Nome == name
                      select u);
            
            try
            {
                Projetos f = id.FirstOrDefault();
                stamp = f.StampProjeto;
            }
            catch { }

            return stamp;
        }


    }


    
}
