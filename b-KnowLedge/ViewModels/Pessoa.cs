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
    /// Class do CRUD das Pessoas.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade Pessoas.
    /// </remarks>
    class Pessoa
    {

        /// <summary>
        /// Obtém a lista completa de Pessoas.
        /// </summary>
        /// <returns>Retorna a lista das Pessoas da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todas as Pessoas da Base de Dados, coloca o resultado dessa pesquisa numa List de Pessoas,
        /// e retorna essa List.
        /// </remarks>
        public List<Pessoas> GetPessoas()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var pes = (from u in bd.Pessoas
                       select u);

            return pes.ToList<Pessoas>();
        }


        /// <summary>
        /// Obtém os dados de uma determinada Pessoa.
        /// </summary>
        /// <param name="bi">Recebe o bi da Pessoa que se pretende obter os dados.</param>
        /// <returns>Retorna os dados da referida Pessoa.</returns>
        /// <remarks>
        /// Recebe um bi de uma Pessoa que se pretende obter os dados da mesma. É feita uma pesquisa à tabela de Pessoas
        /// da Base de Dados, e caso a encontre retorna os dados dessa Pessoa.
        /// </remarks>
        public Pessoas getPessoasDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var pes = (from u in bd.Pessoas
                       where u.StampPessoa == id
                       select u);

            return pes.FirstOrDefault();
        }


        /// <summary>
        /// Faz o update aos dados de uma determinada Pessoa.
        /// </summary>
        /// <param name="bi">Recebe o bi da Pessoa que se pretende fazer o update.</param>
        /// <param name="ls">Recebe uma List com os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe o bi da Pessoa que se pretende actualizar e os respectivos dados, 
        /// obtém a Pessoa, de seguida percorre a List e faz um update à Pessoa do bi recebido.
        /// </remarks>
        public bool UpdatePessoa(string id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            Byte[] bytes;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            try
            {

                Pessoas pes = bd.Pessoas.Single(u => u.StampPessoa == id);


                int i = 0;


                foreach (string res in ls)
                {
                    switch (i)
                    {
                        case 0:
                            pes.StampPessoa = id;
                            break;
                        case 1:
                            pes.StampEntidades = res;
                            break;
                        case 2:
                            pes.Nome = res;
                            break;
                        case 3:
                            pes.Morada = res;
                            break;
                        case 4:
                            pes.Localidade = res;
                            break;
                        case 5:
                            pes.CodPostal = res;
                            break;
                        case 6:
                            pes.Telemovel = res;
                            break;
                        case 7:
                            pes.Telefone = res;
                            break;
                        case 8:
                            pes.Fax = res;
                            break;
                        case 9:
                            pes.Numero = res;
                            break;
                        case 10:
                            pes.Email = res;
                            break;
                        case 11:
                            pes.Site = res;
                            break;
                        case 12:
                            if (res != "")
                            {
                                try
                                {
                                    Stream fs = null;
                                    BinaryReader br = null;

                                    fs = new FileStream(res, FileMode.Open);

                                    br = new BinaryReader(fs);

                                    bytes = br.ReadBytes((Int32)fs.Length);

                                    pes.Foto = bytes;

                                }
                                catch
                                {

                                }
                            }
                            break;
                        case 13:
                            pes.Ousrinis = pes.Ousrinis;
                            break;
                        case 14:
                            pes.Ousrdata = pes.Ousrdata;
                            break;
                        case 15:
                            pes.Ousrhora = pes.Ousrhora;
                            break;
                        case 16:
                            pes.Usrinis = Global.idUser;
                            break;
                        case 17:
                            pes.Usrdata = dt;
                            break;
                        case 18:
                            pes.Usrhora = dataControl.GeraHora(dt);
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
        /// Insere uma Pessoa na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados da Pessoa que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados da Pessoa que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um desses parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertPessoa(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Pessoas pes = new Pessoas();
            bool done = true;
            Byte[] bytes;
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
                            if (res.Trim() == "")
                                pes.StampPessoa = dataControl.GenerateStamp();
                            else
                                pes.StampPessoa = res;
                            break;
                        case 1:
                            pes.StampEntidades = res;
                            break;
                        case 2:
                            pes.Nome = res;
                            break;
                        case 3:
                            pes.Morada = res;
                            break;
                        case 4:
                            pes.Localidade = res;
                            break;
                        case 5:
                            pes.CodPostal = res;
                            break;
                        case 6:
                            pes.Telemovel = res;
                            break;
                        case 7:
                            pes.Telefone = res;
                            break;
                        case 8:
                            pes.Fax = res;
                            break;
                        case 9:
                            pes.Numero = res;
                            break;
                        case 10:
                            pes.Email = res;
                            break;
                        case 11:
                            pes.Site = res;
                            break;
                        case 12:
                            if (res != "")
                            {
                                try
                                {
                                    Stream fs = null;
                                    BinaryReader br = null;

                                    fs = new FileStream(res, FileMode.Open);

                                    br = new BinaryReader(fs);

                                    bytes = br.ReadBytes((Int32)fs.Length);

                                    pes.Foto = bytes;

                                }
                                catch
                                { 
                                   
                                }
                            }
                            break;
                        case 13:
                            pes.Ousrinis = Global.idUser;
                            break;
                        case 14:
                            pes.Ousrdata = dt;
                            break;
                        case 15:
                            pes.Ousrhora = dataControl.GeraHora(dt);
                            break;
                        case 16:
                            pes.Usrinis = Global.idUser;
                            break;
                        case 17:
                            pes.Usrdata = dt;
                            break;
                        case 18:
                            pes.Usrhora = dataControl.GeraHora(dt);
                            break;
                    }
                    i++;
                }

                bd.Pessoas.Add(pes);

                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;

        }


        /// <summary>
        /// Elimina uma Pessoa da Base de Dados.
        /// </summary>
        /// <param name="bi">Recebe o bi da Pessoa que se pretende eliminar.</param>
        /// <returns>Retorna um bool True=Removida com sucesso, False=Não conseguiu remover a Pessoa.</returns>
        /// <remarks>
        /// Recebe o bi da Pessoa que se pretende eliminar, pesquisa na Base de Dados por essa Pessoa,
        ///e caso a encontre, é eliminada. Retorna true se tiver sucesso ao remover a Pessoa e retorna false caso contrário.
        /// </remarks>
        public bool Delete_Pessoa(string id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Pessoas pes = bd.Pessoas.Single(u => u.StampPessoa == id);
                bd.Pessoas.Remove(pes);
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
        /// Obtém as Pessoas associadas a uma determinada Entidade.
        /// </summary>
        /// <param name="id_Entidade">Recebe o id da Entidade que se pretende obter as suas Pessoas.</param>
        /// <returns>Retorna uma List de Pessoas com a/as Pessoa/s encontrada/s.</returns>
        /// <remarks>
        /// Recebe o id da Projetos que se pretende obter as suas Pessoas, faz uma pesquisa à Base de Dados pelas Pessoas com o ID_Projetos igual ao id
        /// recebido, e retorna esse resultado numa List de Pessoas.
        /// </remarks>
        public List<Pessoas> Pessoas_Entidade(string id_projeto)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.Pessoas
                       where u.StampEntidades == id_projeto
                       select u);

            return fil.ToList<Pessoas>();

        }


        public int Existe_Pessoas_Entidade(string id_entidade)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = ( from u in bd.Pessoas
                        join p in bd.Entidades on u.StampEntidades equals p.StampEntidade 
                        join e in bd.Entidades on p.StampEntidade equals e.StampEntidade
                        where e.StampEntidade == id_entidade
                        select u.StampPessoa).Count();

            int num = Convert.ToInt32(fil);

            return num;
        
        }

        public int Existe_Pessoas_Projetos(string id_Projetos)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.Pessoas
                       join p in bd.Entidades on u.StampEntidades equals p.StampEntidade
                       where p.StampEntidade == id_Projetos
                       select u.StampPessoa).Count();

            int num = Convert.ToInt32(fil);

            return num;

        }

    }
}
