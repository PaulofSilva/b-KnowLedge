using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;
using System.IO;

namespace b_KnowLedge.ViewModels
{
    /// <summary>
    /// Class do CRUD dos Utilizadores.
    /// </summary>
    /// <remarks>
    /// Controla o create, read, update e delete da entidade Utilizadores.
    /// </remarks>
    class Utilizador
    {
        /// <summary>
        /// Verifica o nome de utilizador e a password para acesso à aplicação.
        /// </summary>
        /// <param name="Name">Recebe o nome do entidade que pretende entrar na aplicação.</param>
        /// <param name="pass">Recebe a password do entidade que pretende entrar na aplicação.</param>
        /// <returns>Retorna esse utilizador caso o encontre.</returns>
        /// <remarks>
        /// Recebe o nome de utilizador e a password e procura por um utilizador que tenha esse mesmo nome de utilizador
        /// e essa mesma password e caso o encontre retorna esse mesmo utilizador.
        /// </remarks>
        public Utilizadores GetUser(string Name, string pass)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var usr = (from u in bd.Utilizadores
                       where (u.Username == Name.ToUpper() || u.Username == Name.ToLower() || u.Username == Name)
                       select u).FirstOrDefault();

            try
            {
                if (usr.Password == pass)
                {
                    return usr;
                }
            }
            catch { }

            Classes.DataControl dataControl = new Classes.DataControl();
            string p2 = "";

            try
            {
                p2 = dataControl.DecryptStringAES(usr.Password, "BigLevel");
            }
            catch { }

            if (p2 != pass)
                usr = null;

            return usr;
        }

        /// <summary>
        /// Obtém a lista completa de utilizadores
        /// </summary>
        /// <returns>Retorna a lista dos utilizadores da Base de Dados</returns>
        /// <remarks>
        /// Faz uma pesquisa de todos os utilizadores da Base de Dados, coloca numa List de Utilizadores e retorna essa List.
        /// </remarks>
        public List<Utilizadores> GetUtilizador()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var utl = (from u in bd.Utilizadores
                       select u);

            return utl.ToList<Utilizadores>();
        }


        /// <summary>
        /// Obtém os dados de um determinado utilizador.
        /// </summary>
        /// <param name="id">Recebe o id do utilizador que se pretende obter os dados.</param>
        /// <returns>Retorna os dados do utilizador.</returns>
        /// <remarks>
        /// Recebe um id de um utilizador que se pretende obter os dados do mesmo. É feita uma pesquisa à tabela de Utilizadores
        /// da Base de Dados, e caso o encontre retorna os dados desse utilizador.
        /// </remarks>
        public Utilizadores getUtilizadoresDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var utl = (from u in bd.Utilizadores
                       where u.StampUtilizador == id
                       select u);

            return utl.FirstOrDefault();
        }


        /// <summary>
        /// Faz o update aos dados de um determinado utilizador.
        /// </summary>
        /// <param name="id">Recebe o id do utilizador que se pretende fazer o update.</param>
        /// <param name="ls">Recebe os dados que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe um id de utilizador e os respectivos dados, obtém o utilizador, de seguida percorre a List e faz um update ao utilizador do id recebido.
        /// </remarks>
        public bool UpdateUtilizador(string id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            Byte[] bytes;
            Utilizadores utl = bd.Utilizadores.Single(u => u.StampUtilizador == id);
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
                            utl.StampUtilizador = id;
                            break;
                        case 1:
                            utl.Nome = res;
                            break;
                        case 2:
                            utl.Morada = res;
                            break;
                        case 3:
                            utl.Localidade = res;
                            break;
                        case 4:
                            utl.CodPostal = res;
                            break;
                        case 5:
                            utl.Telemovel = res;
                            break;
                        case 6:
                            utl.Username = res;
                            break;
                        case 7:
                            string pass = dataControl.EncryptStringAES(res, "BigLevel");
                            utl.Password = pass;
                            break;
                        case 8:
                            utl.Email = res;
                            break;
                        case 9:
                            if (res != "")
                            {
                                try
                                {
                                    Stream fs = null;
                                    BinaryReader br = null;

                                    fs = new FileStream(res, FileMode.Open);

                                    br = new BinaryReader(fs);

                                    bytes = br.ReadBytes((Int32)fs.Length);

                                    utl.Foto = bytes;

                                }
                                catch
                                {

                                }
                            }
                            break;
                        case 10:
                            utl.Ousrinis = utl.Ousrinis;
                            break;
                        case 11:
                            utl.Ousrdata = utl.Ousrdata;
                            break;
                        case 12:
                            utl.Ousrhora = utl.Ousrhora;
                            break;
                        case 13:
                            utl.Usrinis = Global.idUser;
                            break;
                        case 14:
                            utl.Usrdata = dt;
                            break;
                        case 15:
                            utl.Usrhora = dataControl.GeraHora(dt);
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
        /// Insere um utilizador na Base de Dados.
        /// </summary>
        /// <param name="ls">Recebe os dados do utilizador que se pretende introduzir na Base de Dados.</param>
        /// <remarks>
        /// Recebe por parâmetro uma List com os dados do utilizador que se pretende adicionar à Base de Dados, de seguida essa
        /// List é percorrida e cada um dos parâmetros adicionados à Base de Dados.
        /// </remarks>
        public bool InsertUtilizador(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Utilizadores utl = new Utilizadores();
            bool done = true;
            int i = 0;
            Byte[] bytes;
            Classes.DataControl dataControl = new Classes.DataControl();
            DateTime dt = dataControl.GeraDataHora();

            try{
            foreach (string res in ls)
            {
                switch (i)
                {
                    case 0:
                        if (res.Trim() == "")
                            utl.StampUtilizador = dataControl.GenerateStamp();
                        else
                            utl.StampUtilizador = res;
                        break;
                    case 1:
                        utl.Nome = res;
                        break;
                    case 2:
                        utl.Morada = res;
                        break;
                    case 3:
                        utl.Localidade = res;
                        break;
                    case 4:
                        utl.CodPostal = res;
                        break;
                    case 5:
                        utl.Telemovel = res;
                        break;
                    case 6:
                        utl.Username = res;
                        break;
                    case 7:
                        string pass = dataControl.EncryptStringAES(res, "BigLevel");
                        utl.Password = pass;
                        break;
                    case 8:
                        utl.Email = res;
                        break;
                    case 9:
                        if (res != "")
                        {
                            try
                            {
                                Stream fs = null;
                                BinaryReader br = null;

                                fs = new FileStream(res, FileMode.Open);

                                br = new BinaryReader(fs);

                                bytes = br.ReadBytes((Int32)fs.Length);

                                utl.Foto = bytes;

                            }
                            catch
                            {
                               
                            }
                        }
                        break;
                    case 10:
                        utl.Ousrinis = Global.idUser;
                        break;
                    case 11:
                        utl.Ousrdata = dt;
                        break;
                    case 12:
                        utl.Ousrhora = dataControl.GeraHora(dt);
                        break;
                    case 13:
                        utl.Usrinis = Global.idUser;
                        break;
                    case 14:
                        utl.Usrdata = dt;
                        break;
                    case 15:
                        utl.Usrhora = dataControl.GeraHora(dt);
                        break;
                }
                i++;
            }

                bd.Utilizadores.Add(utl);

                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;
        }

        /// <summary>
        /// Elimina um utilizador da Base de Dados.
        /// </summary>
        /// <param name="id">Recebe o id do utilizador que escolheu eliminar.</param>
        /// <returns>Retorna um bool True=Removido com sucesso, False=Não conseguiu remover o utilizador.</returns>
        /// <remarks>
        /// Recebe o id do Utilizador que se pretende eliminar, pesquisa na Base de Dados se esse utilizador existe,
        ///e caso o encontre, é eliminado. Retorna true se tiver sucesso ao remover o utilizador e retorna false caso contrário.
        /// </remarks>
        public bool Delete_Utilizador(string id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Utilizadores utl = bd.Utilizadores.Single(u => u.StampUtilizador == id);
                bd.Utilizadores.Remove(utl);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }

        public bool UserExist(string nome)
        {
            bool done = false;
            BDKnowLedge bd = new BDKnowLedge();

            var name = (from u in bd.Utilizadores
                        where u.Nome.ToUpper() == nome.ToUpper()
                        select u.StampUtilizador).FirstOrDefault();

            if (name != null)
                done = true;

            return done;
        }

        public string getIDFromNomeUser(string name)
        {
            string stamp = "";
            BDKnowLedge bd = new BDKnowLedge();

            var id = (from u in bd.Utilizadores
                      where u.Username == name
                      select u);

            try
            {
                Utilizadores f = id.FirstOrDefault();
                stamp = f.StampUtilizador;
            }
            catch { }

            return stamp;
        }
    }
}
