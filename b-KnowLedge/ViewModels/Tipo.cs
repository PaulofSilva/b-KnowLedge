using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;

namespace b_KnowLedge.ViewModels
{

    /// <summary>
    /// Class do CRUD dos Tipos.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade TIPO.
    /// </remarks>
    class Tipo
    {

        /// <summary>
        /// Obtém a lista completa de Tipos.
        /// </summary>
        /// <returns>Retorna a lista da Entidade Tipos da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todos os Tipos da Base de Dados, coloca o resultado dessa pesquisa numa List de Tipos,
        /// e retorna essa List.
        /// </remarks>
        public List<Tipos> getTipo()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var type = (from u in bd.Tipos
                                select u);

            return type.ToList<Tipos>();
        }


        /// <summary>
        /// Obtém os dados de um determinado Tipo.
        /// </summary>
        /// <param name="id">Recebe o id do Tipo que se pretende obter os dados.</param>
        /// <returns>Retorna os dados do referido Tipo.</returns>
        /// <remarks>
        /// Recebe um id de um Tipo que se pretende obter os dados do mesmo. É feita uma pesquisa pelo Tipo à
        /// Base de Dados, e caso o encontre retorna os dados desse Tipo.
        /// </remarks>
        public Tipos getTipoDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var type = (from u in bd.Tipos
                                where u.StampTipo == id
                                select u);

            return type.FirstOrDefault();
        }


        /// <summary>
        /// Faz o update aos dados de um determinado Tipo.
        /// </summary>
        /// <param name="id">Recebe o id do Tipo que se pretende fazer o update.</param>
        /// <param name="ls">Recebe os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe um id do Tipo e os respectivos dados, obtém o Tipo, de seguida percorre a List e faz um update ao Tipo do id recebido.
        /// </remarks>
        public bool UpdateTipo(string Id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Tipos type = bd.Tipos.Single(u => u.StampTipo == Id);
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
                            type.StampTipo = Id;
                            break;
                        case 1:
                            type.Nome = res;
                            break;
                        case 2:
                            type.Descricao = res;
                            break;
                        case 3:
                            type.Ousrinis = type.Ousrinis;
                            break;
                        case 4:
                            type.Ousrdata = type.Ousrdata;
                            break;
                        case 5:
                            type.Ousrhora = type.Ousrhora;
                            break;
                        case 6:
                            type.Usrinis = Global.idUser;
                            break;
                        case 7:
                            type.Usrdata = dt;
                            break;
                        case 8:
                            type.Usrhora = dataControl.GeraHora(dt);
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
        /// Insere um Tipo na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados do Tipo que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados do Tipo que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um dos parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertTipo(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Tipos type = new Tipos();
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
                                type.StampTipo = dataControl.GenerateStamp();
                            else
                                type.StampTipo = res;
                            break;
                        case 1:
                            type.Nome = res;
                            break;
                        case 2:
                            type.Descricao = res;
                            break;
                        case 3:
                            type.Ousrinis = Global.idUser;
                            break;
                        case 4:
                            type.Ousrdata = dt;
                            break;
                        case 5:
                            type.Ousrhora = dataControl.GeraHora(dt);
                            break;
                        case 6:
                            type.Usrinis = Global.idUser;
                            break;
                        case 7:
                            type.Usrdata = dt;
                            break;
                        case 8:
                            type.Usrhora = dataControl.GeraHora(dt);
                            break;                       
                    }
                    i++;
                }

                bd.Tipos.Add(type);

           
                    bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;
        }


        /// <summary>
        /// Elimina um Tipo da Base de Dados.
        /// </summary>
        /// <param name="id">Recebe o id do Tipo que escolheu eliminar.</param>
        /// <returns>Retorna um bool True=Removido com sucesso, False=Não conseguiu remover o Tipo.</returns>
        /// <remarks>
        /// Recebe o id do Tipo que se pretende eliminar, pesquisa na Base de Dados se esse Tipo existe,
        ///e caso o encontre, é eliminado. Retorna true se tiver sucesso ao remover o Tipo e retorna false caso contrário.
        /// </remarks>
        public bool Delete_Tipo(string Id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Tipos type = bd.Tipos.Single(u => u.StampTipo == Id);
                bd.Tipos.Remove(type);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }


        public string[] NomeTipo(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var nome = (from u in bd.Tipos
                        where u.StampTipo == id
                        select u);


            return nome.Select(x => x.Nome).ToArray();
        }


        public string getIDFromNomeTipo(string name)
        {
            string stamp = "";
            BDKnowLedge bd = new BDKnowLedge();

            var id = (from u in bd.Tipos
                      where u.Nome == name
                      select u);

            try
            {
                Tipos f = id.FirstOrDefault();
                stamp = f.StampTipo;
            }
            catch { }

            return stamp;
        }


        public List<Tipos> Search_Tipo(List<Conhecimentos> know)
        {

            BDKnowLedge bd = new BDKnowLedge();

            var type =
               from g in
                   (from c in know
                    group c by c.StampTipo)
               select new { g.Key, Count = g.Count() };

            var numeros = type.Select(x => x.Key).ToArray();


            var fil3 = (from u in bd.Tipos
                        where numeros.Contains(u.StampTipo)
                        select u);

            return fil3.ToList<Tipos>();

        }
        
    }
}
