using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;
using System.Data;
using System.Data.EntityClient;
using System.Data.Linq;
using System.IO;


namespace b_KnowLedge.ViewModels
{

    /// <summary>
    /// Class do CRUD das Entidades.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade Entidades.
    /// </remarks>
    class Entidade
    {

        /// <summary>
        /// Obtém a lista completa de Entidades.
        /// </summary>
        /// <returns>Retorna a lista das Entidades da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todas as Entidades da Base de Dados, coloca o resultado dessa pesquisa numa List de Entidades,
        /// e retorna essa List.
        /// </remarks>
        public List<Entidades> GetEntidade()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var usr = (from u in bd.Entidades
                       select u);
           
            return usr.ToList<Entidades>();
        }


        /// <summary>
        /// Obtém os dados de uma determinada Entidade.
        /// </summary>
        /// <param name="id">Recebe o id da Entidade que se pretende obter os dados.</param>
        /// <returns>Retorna os dados da referida Entidade.</returns>
        /// <remarks>
        /// Recebe um id de uma Entidade que se pretende obter os dados da mesma. É feita uma pesquisa à tabela de Entidades
        /// da Base de Dados, e caso a encontre retorna os dados dessa Entidade.
        /// </remarks>
        public Entidades getentidadeDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var emp = (from u in bd.Entidades
                       where u.StampEntidade == id
                       select u);
           
            return emp.FirstOrDefault();
        }

       
        /// <summary>
        /// Faz o update aos dados de uma determinada Entidade.
        /// </summary>
        /// <param name="Id">Recebe o id da Entidade que se pretende fazer o update.</param>
        /// <param name="ls">Recebe uma List com os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe um id da entidade que se pretende actualizar e os respectivos dados, 
        /// obtém a entidade, de seguida percorre a List e faz um update à entidade do id recebido.
        /// </remarks>
        public bool UpdateEntidade(string Id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            try
            {
                Entidades nEmp = bd.Entidades.Single(u => u.StampEntidade == Id);


                int i = 0;


                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                             nEmp.StampEntidade = Id;
                            break;
                        case 1:
                            nEmp.Nome = res;
                            break;
                        case 2:
                            nEmp.Morada = res;
                            break;
                        case 3:
                            nEmp.Localidade = res;
                            break;
                        case 4:
                            nEmp.CodPostal = res;
                            break;
                        case 5:
                            nEmp.Telemovel = res;
                            break;
                        case 6:
                            nEmp.Telefone = res;
                            break;
                        case 7:
                            nEmp.Fax = res;
                            break;
                        case 8:
                            nEmp.Numero = res;
                            break;
                        case 9:
                            nEmp.Email = res;
                            break;
                        case 10:
                            nEmp.Site = res;
                            break;
                        case 11:
                            nEmp.Ousrinis = nEmp.Ousrinis;
                            break;
                        case 12:
                            nEmp.Ousrdata = nEmp.Ousrdata;
                            break;
                        case 13:
                            nEmp.Ousrhora = nEmp.Ousrhora;
                            break;
                        case 14:
                            nEmp.Usrinis = Global.idUser;
                            break;
                        case 15:
                            nEmp.Usrdata = dt;
                            break;
                        case 16:
                            nEmp.Usrhora = dataControl.GeraHora(dt);
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
        /// Insere uma Entidade na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados da Entidade que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados da Entidade que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um desses parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertEntidade(List<string> ls)
        {
          
            BDKnowLedge bd = new BDKnowLedge();
            Entidades nEmp = new Entidades();
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            bool done = true;
            int i = 0;

            try{
            foreach (string res in ls)
            {
                switch (i)
                {
                    case 0:
                        if (res.Trim() == "")
                            nEmp.StampEntidade = dataControl.GenerateStamp();
                        else
                            nEmp.StampEntidade = res;
                        break;
                    case 1:
                        nEmp.Nome = res;
                        break;
                    case 2:
                        nEmp.Morada = res;
                        break;
                    case 3:
                        nEmp.Localidade = res;
                        break;
                    case 4:
                        nEmp.CodPostal = res;
                        break;
                    case 5:
                        nEmp.Telemovel = res;
                        break;
                    case 6:
                        nEmp.Telefone = res;
                        break;
                    case 7:
                        nEmp.Fax = res;
                        break;
                    case 8:
                        nEmp.Numero = res;
                        break;
                    case 9:
                        nEmp.Email = res;
                        break;
                    case 10:
                        nEmp.Site = res;
                        break;
                    case 11:
                        nEmp.Ousrinis = Global.idUser;
                        break;
                    case 12:
                        nEmp.Ousrdata = dt;
                        break;
                    case 13:
                        nEmp.Ousrhora = dataControl.GeraHora(dt);
                        break;
                    case 14:
                        nEmp.Usrinis = Global.idUser;
                        break;
                    case 15:
                        nEmp.Usrdata = dt;
                        break;
                    case 16:
                        nEmp.Usrhora = dataControl.GeraHora(dt);
                        break;
                }
                i++;
            }
           
                bd.Entidades.Add(nEmp);

                bd.SaveChanges();
            }
            catch
            {
                done = false; 
            }

            return done;

        }


        /// <summary>
        /// Elimina uma Entidade da Base de Dados.
        /// </summary>
        /// <param name="Id">Recebe o id da Entidade que se pretende eliminar.</param>
        /// <returns>Retorna um bool True=Removida com sucesso, False=Não conseguiu remover a Entidade.</returns>
        /// <remarks>
        /// Recebe o id da Entidade que se pretende eliminar, pesquisa na Base de Dados por essa Entidade,
        ///e caso a encontre, é eliminada. Retorna true se tiver sucesso ao remover a Entidade e retorna false caso contrário.
        /// </remarks>
        public bool Delete_Entidade(string Id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Entidades emp = bd.Entidades.Single(u => u.StampEntidade == Id);
                bd.Entidades.Remove(emp);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }

        public string[] NomeEntidade(string id)
        {

            BDKnowLedge bd = new BDKnowLedge();

            var nome = (from u in bd.Entidades
                        where u.StampEntidade == id
                        select u);


            return nome.Select(x => x.Nome).ToArray();
        }


        public string IDEntidadeByName(string name)
        {

            BDKnowLedge bd = new BDKnowLedge();
            string retorna = "";

            var id = (from u in bd.Entidades
                      where u.Nome == name
                     select u.StampEntidade);

            foreach (var i in id)
            {
               retorna = i.ToString();
            }

            return retorna;

        }


        public string getIDFromNomeEntidade(string name)
        {
            string stamp = "";
            BDKnowLedge bd = new BDKnowLedge();

            var id = (from u in bd.Entidades
                      where u.Nome == name
                      select u);

            try
            {
                Entidades f = id.FirstOrDefault();
                stamp = f.StampEntidade;
            }
            catch { }

            return stamp;
        }

    }
}
