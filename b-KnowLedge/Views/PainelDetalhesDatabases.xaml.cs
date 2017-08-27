using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDetalhesDatabases.xaml
    /// </summary>
    public partial class PainelDetalhesDatabases : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        public string id_BD = "";
        private BasesDados bd = new BasesDados();
        private ViewModels.BasesdeDados database = new ViewModels.BasesdeDados();
        List<BasesDados> b2;
        private ViewModels.Projeto projecto = new ViewModels.Projeto();
        List<Projetos> f2;
        string id_filiais = "";
        Controls.ButtonsDetails buttonsDetails;


        public PainelDetalhesDatabases(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            database = new ViewModels.BasesdeDados();

            AutoComplete1.BorderBrush = PasswordBDPass.BorderBrush = DatabaseBD.BorderBrush = ServidorBD.BorderBrush = PasswordBD.BorderBrush = usernameBD.BorderBrush = Brushes.Red;

            PasswordBD.Visibility = System.Windows.Visibility.Hidden;

            buttonsDetails = new Controls.ButtonsDetails();
            gridFundo.Children.Add(buttonsDetails);

            buttonsDetails.SaveClick += new EventHandler(Add_Database);
            buttonsDetails.HomeClick += new EventHandler(Home);
            buttonsDetails.DeleteClick += new EventHandler(DeleteBD);

            Loaded += OnLoaded;
        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            if (PainelCentro.backward == true)
                PainelCentro.Escolhe_Painel(-1, true);
            else
                PainelCentro.Escolhe_Painel(7, true);
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                TabCentro.MaxWidth = TabCentro.Width = GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                TabCentro.Height = TabCentro.MaxHeight = border_centro.Height = border_centro.MaxHeight = parentWindow.ActualHeight - 150;
                border_centro.Height = border_centro.MaxHeight - 50;
            }
            catch { }
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            f2 = projecto.getProjetos();
            string[] name2 = new string[f2.Count];
            int i = 0;

            foreach (Projetos fl in f2)
            {
                name2[i] = fl.Nome;
                i++;
            }

            AutoComplete1.ItemsSource = name2;
        }


        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            VerificaFilial();
        }


        private void VerificaFilial()
        {
            string name = "";

            try
            {
                try
                {
                    var at = (AutoComplete1.SelectedItem).ToString();
                    name = at.ToString();
                }
                catch
                {
                    name = AutoComplete1.Text;
                }


                var query = f2.Single(s => s.Nome == name || s.Nome.ToLower() == name.ToLower() || s.Nome.ToUpper() == name.ToUpper());

                id_filiais = query.StampProjeto;

                AutoComplete1.BorderBrush = Brushes.Gray;

            }
            catch
            {
                id_filiais = "";
                AutoComplete1.BorderBrush = Brushes.Red;
            }

        }

        protected void Add_Database(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            bool done = true, window = true;
            string msg = "";

            VerificaFilial();
            int conta = Verifica_Validacao();

            if (conta < 5)
            {
                System.Windows.Forms.MessageBox.Show("Corrija os campos a vermelho!",
                           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    if (id_filiais.Trim() == "")
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("O Projeto seleccionado não existe!\n Deseja criar um Projeto novo?",
                                 "Aviso!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:
                                PainelCentro.Escolhe_Painel(6, false);
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        ls.Add("");
                        ls.Add(id_filiais.ToString());
                        ls.Add(ServidorBD.Text);
                        ls.Add(usernameBD.Text);
                        if (PasswordBD.Visibility == System.Windows.Visibility.Visible)
                            ls.Add(PasswordBD.Text);
                        else
                            ls.Add(PasswordBDPass.Password);
                        ls.Add(DatabaseBD.Text);

                        if (EncryptBD.IsChecked == true)
                            ls.Add("1");
                        else
                            ls.Add("0");

                        if (CertificateBD.IsChecked == true)
                            ls.Add("1");
                        else
                            ls.Add("0");

                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                    }
                }
                catch
                {
                    done = false;
                }

                if (done == true && id_filiais.Trim() != "")
                {
                    if (id_BD.Trim() != "")
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                         "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:

                                done = database.UpdateBasedeDados(id_BD, ls);

                                if (done == true)
                                    msg = "Editada com sucesso!";
                                else
                                    msg = "Erro ao fazer update!";

                                break;
                            default:
                                window = false;
                                break;

                        }
                    }
                    else
                    {
                        done = database.InsertBasedeDados(ls);

                        if (done == true)
                            msg = "Guardada com sucesso!";
                        else
                            msg = "Erro ao gravar a base de dados!";
                    }
                }
                else
                {
                    msg = "Erro ao gravar a base de dados!";
                }

                if (window == true && id_filiais.Trim() != "")
                {
                    System.Windows.Forms.MessageBox.Show(msg,
                    "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                if (done == true && id_filiais != "" && id_BD == "")
                {
                    AutoComplete1.BorderBrush = PasswordBDPass.BorderBrush = DatabaseBD.BorderBrush = ServidorBD.BorderBrush = PasswordBD.BorderBrush = usernameBD.BorderBrush = Brushes.Red;
                    AutoComplete1.Text = "";
                    ServidorBD.Text = "";
                    usernameBD.Text = "";
                    PasswordBD.Text = "";
                    PasswordBDPass.Password = "";
                    DatabaseBD.Text = "";
                    CertificateBD.IsChecked = false;
                    EncryptBD.IsChecked = false;
                    buttonsDetails.AlterDataButtonSave(-1);

                }
            }
        }

        public void Preenche(string id_database)
        {
            int conta = 0;

            ViewModels.BasesdeDados bd_database = new ViewModels.BasesdeDados();
            ViewModels.Projeto projecto = new ViewModels.Projeto();

            id_BD = id_database;
            
            var ls = database.getBDDetails(id_BD);
            string[] nomeFilial = projecto.NomeEntidade(ls.StampProjeto);

            id_filiais = ls.StampProjeto;
            AutoComplete1.Text = nomeFilial[0];
            ServidorBD.Text = ls.Servername;
            usernameBD.Text = ls.UserID;
            Classes.DataControl dataControl = new Classes.DataControl();
            string pass = dataControl.DecryptStringAES(ls.Password, "BigLevel");
            PasswordBD.Text = pass;
            PasswordBDPass.Password = pass;
            DatabaseBD.Text = ls.Initialcatalog;

            if (ls.Encrypt == true)
                EncryptBD.IsChecked = true;

            if (ls.Trustservercertificate == true)
                CertificateBD.IsChecked = true;

            conta = Verifica_Validacao();

            if (conta >= 5)
            {
                buttonsDetails.AlterDataButtonSave(1);
                AutoComplete1.BorderBrush = Brushes.Gray;
            }

        }

        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (DetalhesBD.IsSelected == true)
            {
                buttonsDetails = new Controls.ButtonsDetails();
                gridFundo.Children.Add(buttonsDetails);

                buttonsDetails.SaveClick += new EventHandler(Add_Database);
                buttonsDetails.HomeClick += new EventHandler(Home);
                buttonsDetails.DeleteClick += new EventHandler(DeleteBD);
            }*/         

        }

        protected void DeleteBD(object sender, EventArgs e)
        {
            if (id_BD.Trim() != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar a base de dados?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = database.Delete_BasedeDados(id_BD);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A base de dados foi removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover a base de dados!";

                        System.Windows.Forms.MessageBox.Show(warning,
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        PainelCentro.Escolhe_Painel(7, false);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                PainelCentro.Escolhe_Painel(7, false);
            }

        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        private int Verifica_Validacao()
        {
            int conta = 0;

            if (PasswordBD.Visibility == System.Windows.Visibility.Visible)
                PasswordBDPass.Password = PasswordBD.Text;
            else
                PasswordBD.Text = PasswordBDPass.Password;


            if (id_filiais.Trim() != "")
            {
                AutoComplete1.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                AutoComplete1.BorderBrush = Brushes.Red;
            }

            string pass2 = PasswordBDPass.Password;
            if (pass2.Trim() != "")
            {
                PasswordBDPass.BorderBrush = Brushes.Gray;
            }
            else
            {
                PasswordBDPass.BorderBrush = Brushes.Red;
            }

            if (ServidorBD.Text.Trim() != "")
            {
                ServidorBD.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                ServidorBD.BorderBrush = Brushes.Red;
            }

            if (usernameBD.Text.Trim() != "")
            {
                usernameBD.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                usernameBD.BorderBrush = Brushes.Red;
            }

            if (PasswordBD.Text.Trim() != "")
            {
                PasswordBD.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                PasswordBD.BorderBrush = Brushes.Red;
            }

            if (DatabaseBD.Text.Trim() != "")
            {
                DatabaseBD.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                DatabaseBD.BorderBrush = Brushes.Red;
            }

            if (conta >= 5)
            {
                buttonsDetails.AlterDataButtonSave(1);
            }
            else
            {
                buttonsDetails.AlterDataButtonSave(-1);
            }

            return conta;

        }

        private void Verify_KeyUp_1(object sender, KeyEventArgs e)
        {
            VerificaFilial();
           int conta = Verifica_Validacao();           
        }

        private void MostrarPass_Checked_1(object sender, RoutedEventArgs e)
        {
            PasswordBD.Text = PasswordBDPass.Password;
            PasswordBD.Visibility = System.Windows.Visibility.Visible;
            PasswordBDPass.Visibility = System.Windows.Visibility.Hidden;
        }

        private void MostrarPass_Unchecked_1(object sender, RoutedEventArgs e)
        {
            PasswordBDPass.Password = PasswordBD.Text;
            PasswordBDPass.Visibility = System.Windows.Visibility.Visible;
            PasswordBD.Visibility = System.Windows.Visibility.Hidden;   
        }

        private void Verify_LostFocus_1(object sender, RoutedEventArgs e)
        {
            VerificaFilial();
            int conta = Verifica_Validacao(); 
        }

    }
}
