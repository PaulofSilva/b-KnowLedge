using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using b_KnowLedge.Models;
using b_KnowLedge.ViewModels;
using System.Data;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelImportacao.xaml
    /// </summary>
    public partial class PainelImportacao : UserControl
    {
        UserControlCentro PainelCentro;
        Window parentWindow;
        ViewModels.Projeto projecto = new ViewModels.Projeto();
        ViewModels.BasesdeDados basesDados = new BasesdeDados();
        ViewModels.Tipo tipo = new Tipo();
        ViewModels.Subtipo subtipo = new Subtipo();
        ViewModels.Tabela tabelas = new Tabela();
        ViewModels.Conhecimento conhecimento = new Conhecimento();
        List<Subtipos> subtype;
        List<Tipos> type;
        List<Projetos> pro2;
        List<BasesDados> bd2;
        List<Tabelas> tab;
        List<String> ComboListBD = new List<string>();
        List<CheckBox> ch;
        ViewModels.ImportDados importarDados;
        string idType = "";
        string idPro = "";


        public PainelImportacao(UserControlCentro userControlCentro)
        {
            InitializeComponent();
            PainelCentro = userControlCentro;
            propertyGridComboBox.SelectedIndex = 2;
            Preenche_Controls();
            ButtonSelectAll.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Preenche_Controls()
        {
            pro2 = projecto.getProjetos();
            string[] name2 = new string[pro2.Count];
            int i = 0;

            foreach (Projetos fl in pro2)
            {
                name2[i] = fl.Nome;
                i++;
            }

            AutoComplete1.ItemsSource = name2;

            type = tipo.getTipo();
            string[] name = new string[type.Count];
            i = 0;

            foreach (Tipos t1 in type)
            {
                name[i] = t1.Nome;
                i++;
            }

            AutoCompleteTipos.ItemsSource = name;

        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                TabCentro.MaxWidth = TabCentro.Width = GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                TabCentro.Height = TabCentro.MaxHeight = parentWindow.ActualHeight - 150;
                BorderPainelSubtipo.Height = TabCentro.ActualHeight - 200;
            }
            catch { }
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(1, true);
        }

        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {

        }


        private void combo_BD_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            this.VerifyControls();
        }


        void saveFileClick(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".txt";
            if (dlg.ShowDialog() ?? false)
            {

            }
            else
            {
                return;
            }
        }


        private void Classifica_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            AutoComplete1.Width = Classifica.ActualWidth - 100;
            combo_BD.Width = AutoComplete1.Width;
        }


        void propertyGridComboBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (propertyGrid == null)
                return;
            switch (propertyGridComboBox.SelectedIndex)
            {
                case 0:
                    propertyGrid.SelectedObject = textEditor;
                    break;
                case 1:
                    propertyGrid.SelectedObject = textEditor.TextArea;
                    break;
                case 2:
                    propertyGrid.SelectedObject = textEditor.Options;
                    break;
            }
        }


        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            this.ActualizaControls();
        }


        private void AutoComplete1_KeyUp_1(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.ActualizaControls();
            }

            this.VerifyControls();
        }


        private void ActualizaControls()
        {
            try
            {
                idPro = projecto.getIDFromNomeProjetos(AutoComplete1.Text);

                if (idPro.Trim() == "")
                {
                    combo_BD.SelectedIndex = 0;
                    combo_BD.IsHitTestVisible = false;
                    return;
                }
                else
                {
                    int count = basesDados.Existe_Databases_Projetos(idPro);
                    combo_BD.IsHitTestVisible = true;
                    if (count < 1)
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Não existem Bases de Dados associadas a esse projeto!\n Deseja criar uma?",
                                "Aviso!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:
                                PainelCentro.Escolhe_Painel(1, false);
                                PainelCentro.Escolhe_Painel(8, true);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        try
                        {
                            bd2 = basesDados.BD_Projetos(idPro);
                            lock (combo_BD)
                            {
                                ComboListBD.Clear();
                                combo_BD.ItemsSource = ComboListBD;
                                combo_BD.SelectedIndex = 0;
                            }

                            ComboListBD.Add(" ");

                            foreach (BasesDados b in bd2)
                            {
                                ComboListBD.Add(b.Initialcatalog.ToString());
                            }

                            combo_BD.ItemsSource = ComboListBD;
                            combo_BD.SelectedIndex = 0;
                        }
                        catch
                        {
                            combo_BD.SelectedIndex = 0;
                            combo_BD.IsHitTestVisible = false;
                        }
                    }
                }

            }
            catch
            {

            }
        }


        private void AutoCompleteTipos_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.PreenchePainel();
            }

            this.VerifyControls();
        }


        private void AutoCompleteTipos_LostFocus_1(object sender, RoutedEventArgs e)
        {
            this.PreenchePainel();
        }


        private void PreenchePainel()
        {
            try
            {
                painelSubtipo.Children.Clear();
                ButtonSelectAll.Visibility = System.Windows.Visibility.Hidden;
                idType = tipo.getIDFromNomeTipo(AutoCompleteTipos.Text);

                if (idType.Trim() == "")
                {
                    return;
                }
                else
                {
                    try
                    {
                        ch = new List<CheckBox>();
                        subtype = subtipo.Tipo_Subtipo(idType);
                        int i = 0;
                        foreach (Subtipos s in subtype)
                        {
                            bool done = tabelas.ExisteTabelasTipoSubtipo(idType, s.StampSubtipo);

                            try
                            {
                                if (done == true)
                                {
                                    CheckBox checkboxSubtipo = new CheckBox();
                                    checkboxSubtipo.Name = "checkboxSubtipo" + i;
                                    checkboxSubtipo.Content = s.Nome;
                                    checkboxSubtipo.Checked += new RoutedEventHandler(CheckBox_CheckedChanged);
                                    checkboxSubtipo.Unchecked += new RoutedEventHandler(CheckBox_CheckedChanged);
                                    ch.Add(checkboxSubtipo);
                                    painelSubtipo.Children.Add(checkboxSubtipo);
                                }
                            }
                            catch { }

                            i++;
                        }

                        ButtonSelectAll.Visibility = System.Windows.Visibility.Visible;

                    }
                    catch
                    {
                        ButtonSelectAll.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
            catch
            {

            }

        }


        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.VerifyControls();
        }


        private void ButtonSelectAll_Click_1(object sender, RoutedEventArgs e)
        {
            int num = 0;

            foreach (CheckBox c in ch)
            {
                if (c.IsChecked == true)
                {
                    num++;
                }
            }

            if (num != ch.Count)
            {
                foreach (CheckBox c in ch)
                {
                    c.IsChecked = true;
                }
            }
            else
            {
                foreach (CheckBox c in ch)
                {
                    c.IsChecked = false;
                }
            }


        }


        private void Home_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }


        private void VerifyControls()
        {
            int num = 0;

            if (idPro.Trim() != "")
            {
                num++;
            }

            if (idType.Trim() != "")
            {
                num++;
            }

            try
            {
                if (combo_BD.SelectedItem.ToString().Trim() != "")
                {
                    num++;
                }
            }
            catch
            { }

            try
            {
                bool check = false;
                if (ch != null)
                {
                    foreach (CheckBox c in ch)
                    {
                        if (c.IsChecked == true)
                        {
                            check = true;
                        }
                    }

                    if (check == true)
                    {
                        num++;
                    }
                }
            }
            catch
            { }

            if (num < 4)
            {
                ButtonImport.IsEnabled = false;
                ImageImport.Opacity = 0.3;
            }
            else
            {
                ButtonImport.IsEnabled = true;
                ImageImport.Opacity = 1.0;
            }



        }

        private int NumChecked()
        {
            int conta = 0;
            foreach (CheckBox c in ch)
            {
                if (c.IsChecked == true)
                {
                    conta++;
                }
            }

            return conta;
        }

        private void Work()
        {
            Updater uiUpdater = new Updater(UpdateUI);
            Dispatcher.BeginInvoke(DispatcherPriority.Send, uiUpdater, 0);
        }

        private delegate void Updater(int UI);

        private void UpdateUI(int i)
        {
            textEditor.Text = "Projeto: " + AutoComplete1.Text + "\n";
            textEditor.Text = textEditor.Text + "Base de Dados: " + teste + "\n\n";
            textEditor.Text = textEditor.Text + "Tipo: " + AutoCompleteTipos.Text + "\n\n";
            textEditor.Text = textEditor.Text + "A importar...";
        }

        private void Work2()
        {
            Updater2 uiUpdater2 = new Updater2(UpdateUI2);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, uiUpdater2, 0);
        }

        private delegate void Updater2(int UI);

        private void UpdateUI2(int i)
        {

            textEditor.Text = "";
            textEditor.Text = "Projeto: " + AutoComplete1.Text + "\n";
            textEditor.Text = textEditor.Text + "Base de Dados: " + teste + "\n\n";
            textEditor.Text = textEditor.Text + "Tipo: " + AutoCompleteTipos.Text + "\n\n";

            bool done = false;

            foreach (CheckBox c in ch)
            {
                if (c.IsChecked == true)
                {
                    try
                    {
                        string IDSubtype = subtipo.getIDFromNomeSubtipos(c.Content.ToString(), idType);
                        Tabelas tab1 = tabelas.TabelasDadoOTipoAndSubtipo(idType, IDSubtype);

                        DataTable dt = importarDados.ObtainDados(tab1.QueryString);
                        string st = "", uHora = "";
                        DateTime usrData = DateTime.Now;
                        List<string> ls = new List<string>();
                        int inserts = 0, updates = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            ls.Clear();

                            if (dt.Columns.Contains("stamp"))
                            {
                                st = dr["stamp"].ToString();
                                ls.Add(dr["stamp"].ToString());
                            }

                            ls.Add(idPro.ToString());
                            ls.Add(IDSubtype.ToString());
                            ls.Add(idType.ToString());


                            if (dt.Columns.Contains("descricao"))
                                ls.Add(dr["descricao"].ToString());
                            else
                                ls.Add(" ");

                            if (dt.Columns.Contains("codigo"))
                                ls.Add(dr["codigo"].ToString());
                            else
                                ls.Add(" ");

                            if (dt.Columns.Contains("metadados"))
                                ls.Add(dr["metadados"].ToString());
                            else
                                ls.Add(" ");

                            if (dt.Columns.Contains("ecra"))
                                ls.Add(dr["ecra"].ToString());
                            else
                                ls.Add(" ");

                            if (dt.Columns.Contains("mensagem"))
                                ls.Add(dr["mensagem"].ToString());
                            else
                                ls.Add(" ");

                            if (dt.Columns.Contains("teclas"))
                                ls.Add(dr["teclas"].ToString());
                            else
                                ls.Add(" ");

                            if (dt.Columns.Contains("tabela"))
                                ls.Add(dr["tabela"].ToString());
                            else
                                ls.Add(" ");

                            ls.Add("0");

                            ls.Add("1");

                            if (dt.Columns.Contains("ousrdata"))
                            {
                                ls.Add(dr["ousrdata"].ToString());
                            }
                            else
                            {
                                ls.Add(" ");
                            }

                            if (dt.Columns.Contains("ousrhora"))
                            {
                                ls.Add(dr["ousrhora"].ToString());
                            }
                            else
                            {
                                ls.Add(" ");
                            }

                            ls.Add("1");

                            if (dt.Columns.Contains("usrdata"))
                            {
                                usrData = Convert.ToDateTime(dr["usrdata"].ToString());
                                ls.Add(dr["usrdata"].ToString());
                            }
                            else
                            {
                                ls.Add(" ");
                            }

                            if (dt.Columns.Contains("usrhora"))
                            {
                                uHora = dr["usrhora"].ToString();
                                ls.Add(dr["usrhora"].ToString());
                            }
                            else
                            {
                                ls.Add(" ");
                            }


                            string existe = "";
                            int stampExiste = -1;
                            DataTable dtConhecimento = conhecimento.DadosImport(idPro, idType, IDSubtype);

                            if (dtConhecimento.Rows.Count > 0)
                            {
                                string formatted = usrData.ToString("dd/M/yyyy");
                                DateTime dateTime = Convert.ToDateTime(formatted + " " + uHora);
                                var results = (from myRow in dtConhecimento.AsEnumerable()
                                               where myRow.Field<string>("StampConhecimento") == st && (Convert.ToDateTime(myRow.Field<string>("data")) < dateTime)
                                               select new
                                               {
                                                   IDCon = myRow.Field<string>("StampConhecimento")
                                               }.IDCon).FirstOrDefault();

                                var results2 = (from myRow in dtConhecimento.AsEnumerable()
                                                where myRow.Field<string>("StampConhecimento") == st
                                                select new
                                                {
                                                    IDCon = myRow.Field<string>("StampConhecimento")
                                                }.IDCon).FirstOrDefault();

                                try
                                {
                                    existe = results.ToString();
                                }
                                catch
                                {
                                    existe = "";
                                }

                                try
                                {
                                    stampExiste = Convert.ToInt32(results2.ToString());
                                }
                                catch
                                {
                                    stampExiste = -1;
                                }

                            }

                            done = false;

                            if (existe.Trim() != "")
                            {
                                done = conhecimento.UpdateConhecimento(existe, ls);

                                if (done == true)
                                    updates++;
                            }
                            else if (stampExiste < 1)
                            {
                                done = conhecimento.InsertConhecimento(ls);

                                if (done == true)
                                    inserts++;
                            }

                        }

                        textEditor.Text = textEditor.Text + "Subtipo: " + c.Content.ToString() + "\n";
                        textEditor.Text = textEditor.Text + "Importados/as: " + inserts + "\n";
                        textEditor.Text = textEditor.Text + "Actualizados/as: " + updates + "\n\n";
                    }
                    catch { }
                }
            }
        }

        string teste = "";
        private void Import_Click_1(object sender, RoutedEventArgs e)
        {
            string id = basesDados.IDBDByName(combo_BD.SelectedItem.ToString());
            BasesDados bd = basesDados.getBDDetails(id);
            importarDados = new ViewModels.ImportDados();
            Classes.DataControl dataControl = new Classes.DataControl();
            string pass = dataControl.DecryptStringAES(bd.Password, "BigLevel");
            bool conn = importarDados.ConstroiConnString(bd.Servername, bd.Initialcatalog, bd.UserID, pass);

            if (conn == false)
            {

            }
            else
            {
                teste = bd.Initialcatalog;
                bool done = false;
                Thread t = new Thread(new ThreadStart(Work));
                t.Start();

                Thread t2 = new Thread(new ThreadStart(Work2));
                t2.Start();

            }
        }
    }
}




