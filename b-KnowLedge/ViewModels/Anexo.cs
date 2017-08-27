using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b_KnowLedge.Models;
using System.IO;
using Microsoft.Win32;

namespace b_KnowLedge.ViewModels
{
    class Anexo
    {

        public List<Anexos> getAnexos()
        {
            BDKnowLedge bd = new BDKnowLedge();

            var anexo = (from u in bd.Anexos
                            select u);

            return anexo.ToList<Anexos>();
        }


        public Anexos getAnexosDetails(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var anexo = (from u in bd.Anexos
                            where u.StampAnexo == id
                            select u);

            return anexo.FirstOrDefault();
        }


        public bool InsertAnexos(List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            Anexos anexo = new Anexos();
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
                                anexo.StampAnexo = dataControl.GenerateStamp();
                            else
                                anexo.StampAnexo = res;
                            break;
                        case 1:
                            anexo.StampConhecimento = res;
                            break;
                        case 2:
                            anexo.NomeFicheiro = res;
                            break;
                        case 3:
                            if (res != "")
                            {
                                try
                                {
                                    Stream fs = null;
                                    BinaryReader br = null;

                                    fs = new FileStream(res, FileMode.Open);

                                    br = new BinaryReader(fs);

                                    bytes = br.ReadBytes((Int32)fs.Length);

                                    anexo.Ficheiro = bytes;                                    
                                }
                                catch { }
                            }
                            break;
                        case 4:
                            anexo.Ousrinis = Global.idUser;
                            break;
                        case 5:
                            anexo.Ousrdata = dt;
                            break;
                        case 6:
                            anexo.Ousrhora = dataControl.GeraHora(dt);
                            break;
                        case 7:
                            anexo.Usrinis = Global.idUser;
                            break;
                        case 8:
                            anexo.Usrdata = dt;
                            break;
                        case 9:
                            anexo.Usrhora = dataControl.GeraHora(dt);
                            break;
                        
                    }
                    i++;
                }

                bd.Anexos.Add(anexo);

                bd.SaveChanges();
            }
            catch
            {
                done = false;
            }

            return done;

        }


        public bool UpdateAnexos(string id, List<string> ls)
        {

            BDKnowLedge bd = new BDKnowLedge();
            bool done = true;
            Anexos anexo = bd.Anexos.Single(u => u.StampAnexo == id);
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
                            anexo.StampAnexo = anexo.StampAnexo;
                            break;
                        case 1:
                            anexo.StampConhecimento = res;
                            break;
                        case 2:
                            anexo.NomeFicheiro = res;
                            break;
                        case 3:
                            if (res != "")
                            {
                                try
                                {
                                    Stream fs = null;
                                    BinaryReader br = null;

                                    fs = new FileStream(res, FileMode.Open);

                                    br = new BinaryReader(fs);

                                    bytes = br.ReadBytes((Int32)fs.Length);

                                    anexo.Ficheiro = bytes;
                                }
                                catch { }
                            }
                            break;
                        case 4:
                            anexo.Ousrinis = anexo.Ousrinis;
                            break;
                        case 5:
                            anexo.Ousrdata = anexo.Ousrdata;
                            break;
                        case 6:
                            anexo.Ousrhora = anexo.Ousrhora;
                            break;
                        case 7:
                            anexo.Usrinis = Global.idUser;
                            break;
                        case 8:
                            anexo.Usrdata = dt;
                            break;
                        case 9:
                            anexo.Usrhora = dataControl.GeraHora(dt);
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

        public void DeleteAnexosbyConhecimento(string id)
        {
            BDKnowLedge bd = new BDKnowLedge();
            try
            {
                var con = (from u in bd.Anexos
                           where u.StampConhecimento == id
                           select u);
                List<Anexos> anexo = con.ToList<Anexos>();

                foreach (Anexos an in anexo)
                {
                    bd.Anexos.Remove(an);
                    bd.SaveChanges();
                }
            }
            catch { }

        }

        public bool Delete_Anexo(string id, string nome)
        {
            bool done = false;

            try
            {
                BDKnowLedge bd = new BDKnowLedge();
                Anexos anexo = bd.Anexos.Single(u => u.StampConhecimento == id && u.NomeFicheiro==nome);
                bd.Anexos.Remove(anexo);
                bd.SaveChanges();
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }


        public void Download_File(string id_con1, string name)
        {

            BDKnowLedge bd = new BDKnowLedge();

            byte[] fileData = null;
            string fileName = "";

            var record = from p in bd.Anexos
                         where p.StampConhecimento == id_con1 && p.NomeFicheiro==name
                         select p;

           
                fileData = (byte[])record.First().Ficheiro.ToArray();
                fileName = record.First().NomeFicheiro.ToString();
           

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = fileName;

            if (saveFileDialog1.ShowDialog() ?? false)
            {
                System.IO.File.WriteAllBytes(saveFileDialog1.FileName, fileData);
            }

        }


        public List<Anexos> Anexos_Conhecimento(string id_con)
        {
            BDKnowLedge bd = new BDKnowLedge();

            var anexo = (from u in bd.Anexos
                       where u.StampConhecimento == id_con
                       select u);

            return anexo.ToList<Anexos>();
        }



    }
}
