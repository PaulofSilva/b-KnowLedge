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
    /// Class do CRUD das BasesdeDados.
    /// </summary>
    /// /// <remarks>
    /// Controla o create, read, update e delete da entidade BasesdeDados.
    /// </remarks>
    class BasesdeDados
    {
        public List<BasesDados> GetBasesDeDados()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var database = (from u in bd.BasesDados
                       select u);

            return database.ToList<BasesDados>();
        }

        public BasesDados getBDDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var database = (from u in bd.BasesDados
                       where u.StampBaseDados == id
                       select u);

            return database.FirstOrDefault();
        }


        public bool UpdateBasedeDados(string id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            BasesDados database = bd.BasesDados.Single(u => u.StampBaseDados == id);
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
                            database.StampBaseDados = database.StampBaseDados;
                            break;
                        case 1:
                            database.StampProjeto = res;
                            break;
                        case 2:
                            database.Servername = res;
                            break;
                        case 3:
                            database.UserID = res;
                            break;
                        case 4:
                            string pass = dataControl.EncryptStringAES(res, "BigLevel");
                            database.Password = pass;
                            break;
                        case 5:
                            database.Initialcatalog = res;
                            break;
                        case 6:
                            if (res == "1")
                                database.Encrypt = true;
                            else
                                database.Encrypt = false;
                            break;
                        case 7:
                            if (res == "1")
                                database.Trustservercertificate = true;
                            else
                                database.Trustservercertificate = false;
                            break;
                        case 8:
                            database.Ousrinis = database.Ousrinis;
                            break;
                        case 9:
                            database.Ousrdata = database.Ousrdata;
                            break;
                        case 10:
                            database.Ousrhora = database.Ousrhora;
                            break;
                        case 11:
                            database.Usrinis = Global.idUser;
                            break;
                        case 12:
                            database.Usrdata = dt;
                            break;
                        case 13:
                            database.Usrhora = dataControl.GeraHora(dt);
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

        public bool InsertBasedeDados(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            BasesDados database = new BasesDados();
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
                            database.StampBaseDados = dataControl.GenerateStamp();
                        else
                            database.StampBaseDados = res;
                        break;
                    case 1:
                        database.StampProjeto = res;
                        break;
                    case 2:
                        database.Servername = res;
                        break;
                    case 3:
                        database.UserID = res;
                        break;
                    case 4:
                        string pass = dataControl.EncryptStringAES(res, "BigLevel");
                        database.Password = pass;
                        break;
                    case 5:
                        database.Initialcatalog = res;
                        break;
                    case 6:
                        if (res == "1")
                            database.Encrypt = true;
                        else
                            database.Encrypt = false;
                        break;
                    case 7:
                        if (res == "1")
                            database.Trustservercertificate = true;
                        else
                            database.Trustservercertificate = false;
                        break;
                    case 8:
                        database.Ousrinis = Global.idUser;
                        break;
                    case 9:
                        database.Ousrdata = dt;
                        break;
                    case 10:
                        database.Ousrhora = dataControl.GeraHora(dt);
                        break;
                    case 11:
                        database.Usrinis = Global.idUser;
                        break;
                    case 12:
                        database.Usrdata = dt;
                        break;
                    case 13:
                        database.Usrhora = dataControl.GeraHora(dt);
                        break;
                }
                i++;
            }

            bd.BasesDados.Add(database);

                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;

        }


        public bool Delete_BasedeDados(string id)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                BasesDados database = bd.BasesDados.Single(u => u.StampBaseDados == id);
                bd.BasesDados.Remove(database);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }


        public List<BasesDados> BD_Projetos(string id_projeto)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.BasesDados
                       where u.StampProjeto == id_projeto
                       select u);

            return fil.ToList<BasesDados>();

        }


        public int Existe_Databases_Projetos(string id_Projeto)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.BasesDados
                       join p in bd.Projetos on u.StampProjeto equals p.StampProjeto
                       where p.StampProjeto == id_Projeto
                       select u.StampBaseDados).Count();

            int num = Convert.ToInt32(fil);

            return num;

        }


        public int Existe_Databases_Entidade(string id_entidade)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.BasesDados
                       join p in bd.Projetos on u.StampProjeto equals p.StampProjeto
                       join e in bd.Entidades on p.StampEntidade equals e.StampEntidade
                       where e.StampEntidade == id_entidade
                       select u.StampBaseDados).Count();

            int num = Convert.ToInt32(fil);

            return num;

        }

        public List<BasesDados> Databases_Entidade(string id_entidade)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var fil = (from u in bd.BasesDados
                       join p in bd.Projetos on u.StampProjeto equals p.StampProjeto
                       join e in bd.Entidades on p.StampEntidade equals e.StampEntidade
                       where e.StampEntidade == id_entidade
                       select u);

            return fil.ToList<BasesDados>();

        }

        public string IDBDByName(string name)
        {

            BDKnowLedge bd = new BDKnowLedge();
            string retorna = "";

            var id = (from u in bd.BasesDados
                      where u.Initialcatalog == name
                      select u.StampBaseDados);

            foreach (var i in id)
            {
                retorna = i.ToString();
            }

            return retorna;

        }

    }
}
