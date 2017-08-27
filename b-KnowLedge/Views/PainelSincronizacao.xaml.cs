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
using System.Threading;
using System.Windows.Threading;
using Microsoft.Win32;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelSincronizacao.xaml
    /// </summary>
    public partial class PainelSincronizacao : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        int id_projetos = -1;
        ViewModels.Sincroniza sync = new ViewModels.Sincroniza();

        public PainelSincronizacao(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;

            ServidorBD.Text = Properties.Settings.Default.bServidor;
            usernameBD.Text = Properties.Settings.Default.bUsername;
            DatabaseBD.Text = Properties.Settings.Default.bBDName;
            PasswordBD.Text = Properties.Settings.Default.bPassword;
            PasswordBDPass.Password = Properties.Settings.Default.bPassword;

            PasswordBD.Visibility = System.Windows.Visibility.Hidden;

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
                textEditor.Width = ToolbarTop.Width = TabCentro.ActualWidth / 2;
                textEditor.Height = TabCentro.ActualHeight / 3;
            }
            catch { }
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

        private void Home_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string pass = "", msg = "";

            if (MostrarPass.IsChecked == true)
                pass = PasswordBD.Text;
            else
                pass = PasswordBDPass.Password;

            bool done = sync.TesteConnectionServer(ServidorBD.Text, DatabaseBD.Text, usernameBD.Text, pass);


            if (done == true)
            {
                msg = "Conexão bem sucedida!";
                Properties.Settings.Default.bServidor = ServidorBD.Text;
                Properties.Settings.Default.bUsername = usernameBD.Text;
                Properties.Settings.Default.bBDName = DatabaseBD.Text;
                Properties.Settings.Default.bPassword = pass;
                Properties.Settings.Default.Save();
                
            }
            else
            {
                msg = "Erro na conexão!";
            }

            System.Windows.Forms.MessageBox.Show(msg,
            "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (CheckBoxConhecimento.IsChecked == true && CheckBoxEntidade.IsChecked == true && CheckBoxProjeto.IsChecked == true && CheckBoxUser.IsChecked == true
                && CheckBoxTipo.IsChecked == true && CheckBoxTabela.IsChecked == true && CheckBoxPessoa.IsChecked == true && CheckBoxBD.IsChecked == true
                && CheckBoxSubtipo.IsChecked == true)
            {
                CheckBoxConhecimento.IsChecked = CheckBoxEntidade.IsChecked = CheckBoxProjeto.IsChecked = CheckBoxUser.IsChecked = false;
                CheckBoxTipo.IsChecked = CheckBoxTabela.IsChecked = CheckBoxPessoa.IsChecked = CheckBoxBD.IsChecked = false;
                CheckBoxSubtipo.IsChecked = false;
            }
            else
            {
                CheckBoxConhecimento.IsChecked = CheckBoxEntidade.IsChecked = CheckBoxProjeto.IsChecked = CheckBoxUser.IsChecked = true;
                CheckBoxTipo.IsChecked = CheckBoxTabela.IsChecked = CheckBoxPessoa.IsChecked = CheckBoxBD.IsChecked = true;
                CheckBoxSubtipo.IsChecked = true;
            }
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

        private void SincronizaButton_Click_1(object sender, RoutedEventArgs e)
        {
            string pass = "", msg = "";

            if (MostrarPass.IsChecked == true)
                pass = PasswordBD.Text;
            else
                pass = PasswordBDPass.Password;

            bool done = sync.TesteConnectionServer(ServidorBD.Text, DatabaseBD.Text, usernameBD.Text, pass);


            if (done == true)
            {
                Properties.Settings.Default.bServidor = ServidorBD.Text;
                Properties.Settings.Default.bUsername = usernameBD.Text;
                Properties.Settings.Default.bBDName = DatabaseBD.Text;
                Properties.Settings.Default.bPassword = pass;
                Properties.Settings.Default.Save();

                Thread t = new Thread(new ThreadStart(Work));
                t.Start();

                Thread t2 = new Thread(new ThreadStart(Work2));
                t2.Start();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Erro na conexão!",
            "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }
        private void Work()
        {
            Updater uiUpdater = new Updater(UpdateUI);
            Dispatcher.BeginInvoke(DispatcherPriority.Send, uiUpdater, 0);
        }

        private delegate void Updater(int UI);

        private void UpdateUI(int i)
        {
            textEditor.Text = "";
            textEditor.Text = textEditor.Text + "A sincronizar...";
        }

        private void Work2()
        {
            Updater2 uiUpdater2 = new Updater2(UpdateUI2);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, uiUpdater2, 0);
        }

        private delegate void Updater2(int UI);

        private void UpdateUI2(int i)
        {
            int conta = 0;
            bool user = false, entidade = false, projeto = false, type = false, con = false, pessoa = false, bd = false, subtype = false, tab = false;
            textEditor.Text = "";

            if (CheckBoxUser.IsChecked == true)
            {
                this.ImportUser();
                user = true;
            }
            if (CheckBoxEntidade.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                this.ImportEntidades();
                entidade = true;
            }
            if (CheckBoxProjeto.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                if (entidade == false)
                {
                    this.ImportEntidades();
                    entidade = true;
                }
                this.ImportProjetos();
                projeto = true;
            }
            if (CheckBoxPessoa.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                if (entidade == false)
                {
                    this.ImportEntidades();
                    entidade = true;
                }
                this.ImportPessoa();
                pessoa = true;
            }
            if (CheckBoxBD.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                if (bd == false)
                {
                    this.ImportProjetos();
                    projeto = true;
                }
                this.ImportBD();
                bd = true;
            }
            if (CheckBoxTipo.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                this.ImportTipo();
                type = true;
            }
            if (CheckBoxSubtipo.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                if (type == false)
                {
                    this.ImportTipo();
                    type = true;
                }
                this.ImportSubtipo();
                subtype = true;
            }
            if (CheckBoxTabela.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                if (type == false)
                {
                    this.ImportTipo();
                    type = true;
                }
                if (subtype == false)
                {
                    this.ImportSubtipo();
                    subtype = true;
                }
                this.ImportTabelas();
                tab = true;
            }
            if (CheckBoxConhecimento.IsChecked == true)
            {
                if (user == false)
                {
                    this.ImportUser();
                    user = true;
                }
                if (type == false)
                {
                    this.ImportTipo();
                    type = true;
                }
                if (subtype == false)
                {
                    this.ImportSubtipo();
                    subtype = true;
                }
                if (projeto == false)
                {
                    this.ImportProjetos();
                    projeto = true;
                }
                this.ImportExportConhecimento();
                con = true;
            }

        }

        private void ImportUser()
        {
            int conta = 0;
            conta = sync.ImportUtilizadores();
            textEditor.Text = textEditor.Text + "Utilizadores importados\\actualizados: " + conta + "\n";
        }

        private void ImportEntidades()
        {
            int conta = 0;
            conta = sync.ImportEntidades();
            textEditor.Text = textEditor.Text + "Entidades importadas\\actualizadas: " + conta + "\n";
        }

        private void ImportProjetos()
        {
            int conta = 0;
            conta = sync.ImportProjetos();
            textEditor.Text = textEditor.Text + "Projetos importados\\actualizados: " + conta + "\n";
        }

        private void ImportPessoa()
        {
            int conta = 0;
            conta = sync.ImportPessoas();
            textEditor.Text = textEditor.Text + "Pessoas importadas\\actualizadas: " + conta + "\n";
        }

        private void ImportBD()
        {
            int conta = 0;
            conta = sync.ImportBDs();
            textEditor.Text = textEditor.Text + "Bases de dados importadas\\actualizadas: " + conta + "\n";
        }

        private void ImportTipo()
        {
            int conta = 0;
            conta = sync.ImportTipo();
            textEditor.Text = textEditor.Text + "Tipos importados\\actualizados: " + conta + "\n";
        }

        private void ImportSubtipo()
        {
            int conta = 0;
            conta = sync.ImportSubtipo();
            textEditor.Text = textEditor.Text + "Subtipos importados\\actualizados: " + conta + "\n";
        }

        private void ImportTabelas()
        {
            int conta = 0;
            conta = sync.ImportTabelas();
            textEditor.Text = textEditor.Text + "Tabelas importadas\\actualizadas: " + conta + "\n";
        }

        private void ImportExportConhecimento()
        {
            int conta = 0;
            conta = sync.ImportConhecimento();
            textEditor.Text = textEditor.Text + "Conhecimento importado\\actualizado: " + conta + "\n";
            conta = sync.ExportConhecimento();
            textEditor.Text = textEditor.Text + "Conhecimento exportado\\actualizado: " + conta + "\n";
        }
    }
}
