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
using Microsoft.Win32;
using System.IO;
using b_KnowLedge.Models;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDetalhesUtilizadores.xaml
    /// </summary>
    public partial class PainelDetalhesUtilizadores : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        public string id_user = "";
        private ViewModels.Utilizador user = new ViewModels.Utilizador();
        string path_Image = " ";

        Controls.ButtonsDetails buttonsDetails;

        public PainelDetalhesUtilizadores(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            buttonsDetails = new Controls.ButtonsDetails();
            gridFundo.Children.Add(buttonsDetails);

            buttonsDetails.SaveClick += new EventHandler(Add_Utilizador);
            buttonsDetails.HomeClick += new EventHandler(Home);
            buttonsDetails.DeleteClick += new EventHandler(DeleteUser);
            PasswordUser.Visibility = System.Windows.Visibility.Hidden;
            UsernameUser.BorderBrush = PasswordUserPass.BorderBrush = PasswordUser.BorderBrush = NomeUser.BorderBrush = Brushes.Red;
        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
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


        public void Preenche(string id)
        {
           
            id_user = id;

            var ls = user.getUtilizadoresDetails(id_user);

            NomeUser.Text = ls.Nome;
            MoradaUser.Text = ls.Morada;
            LocalidadeUser.Text = ls.Localidade;
            CodPostalUser.Text = ls.CodPostal;
            TelemovelUser.Text = ls.Telemovel;
            UsernameUser.Text = ls.Username;
            Classes.DataControl dataControl = new Classes.DataControl();
            string pass = dataControl.DecryptStringAES(ls.Password, "BigLevel");
            PasswordUser.Text = pass;
            PasswordUserPass.Password = pass;
            EmailUser.Text = ls.Email;


            if (ls.Foto != null)
            {
                byte[] blob = ls.Foto;
                MemoryStream stream = new MemoryStream();
                stream.Write(blob, 0, blob.Length);
                stream.Position = 0;

                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage foto = new BitmapImage();
                foto.BeginInit();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                foto.StreamSource = ms;
                foto.EndInit();
                ImagePessoa.Source = foto;
            }

            if (UsernameUser.Text.Trim() != "")
                UsernameUser.BorderBrush = Brushes.Gray;

            if (PasswordUser.Text.Trim() != "")
                PasswordUser.BorderBrush = Brushes.Gray;

            if (NomeUser.Text.Trim() != "")
                NomeUser.BorderBrush = Brushes.Gray;

            if (PasswordUserPass.Password.Trim() != "")
                PasswordUserPass.BorderBrush = Brushes.Gray;

            if (UsernameUser.Text.Trim() != "" && (PasswordUser.Text.Trim() != "" || PasswordUserPass.Password.Trim()!="") && NomeUser.Text.Trim() != "")
               buttonsDetails.AlterDataButtonSave(1);

        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            if (PainelCentro.backward == true)
                PainelCentro.Escolhe_Painel(-1, true);
            else
                PainelCentro.Escolhe_Painel(12, true);
        }


        private void openFileClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.PNG";
            dlg.Title = "Carregue a Imagem";
            dlg.Multiselect = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.RestoreDirectory = true;
            dlg.ReadOnlyChecked = true;
            dlg.ShowReadOnly = true;
            string currentFileName;

            if (dlg.ShowDialog() ?? false)
            {
                currentFileName = dlg.FileName;
                path_Image = currentFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(currentFileName);
                bitmap.EndInit();
                ImagePessoa.Source = bitmap;
            }
        }

        protected void DeleteUser(object sender, EventArgs e)
        {

            if (id_user.Trim() != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar o utilizador?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = user.Delete_Utilizador(id_user);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "O utilizador foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover o utilizador!";

                        System.Windows.Forms.MessageBox.Show(warning,
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        PainelCentro.Escolhe_Painel(12, false);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                PainelCentro.Escolhe_Painel(12, false);
            }

        }


        protected void Add_Utilizador(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            bool done = true, window = true;
            string msg = "";

            int conta = Verifica_Validacao();

            if (conta<3)
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o Nome, Username e a Password!",
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    ls.Add("");
                    ls.Add(NomeUser.Text);
                    ls.Add(MoradaUser.Text);
                    ls.Add(LocalidadeUser.Text);
                    ls.Add(CodPostalUser.Text);
                    ls.Add(TelemovelUser.Text);
                    ls.Add(UsernameUser.Text);
                    if (PasswordUser.Visibility == System.Windows.Visibility.Visible)
                        ls.Add(PasswordUser.Text);
                    else
                        ls.Add(PasswordUserPass.Password);
                    ls.Add(EmailUser.Text);

                    if (path_Image != " ")
                        ls.Add(path_Image);

                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    
                }
                catch
                {
                    done = false;
                }

                if (done == true)
                {
                    if (id_user.Trim() != "")
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                         "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:

                                done = user.UpdateUtilizador(id_user, ls);

                                if (done == true)
                                    msg = "Editado com sucesso!";
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
                        done = user.InsertUtilizador(ls);

                        if (done == true)
                            msg = "Guardado com sucesso!";
                        else
                            msg = "Erro ao gravar o utilizador!\nVerifique se já existe esse username.";
                    }
                }
                else
                {
                    msg = "Erro ao gravar o utilizador!\nVerifique se já existe esse username.";
                }

                if (window == true)
                {
                    System.Windows.Forms.MessageBox.Show(msg,
                    "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                if (done == true && id_user.Trim() == "")
                {
                    NomeUser.Text = "";
                    MoradaUser.Text = "";
                    LocalidadeUser.Text = "";
                    CodPostalUser.Text = "";
                    TelemovelUser.Text = "";
                    UsernameUser.Text = "";
                    PasswordUser.Text = "";
                    PasswordUserPass.Password = "";
                    EmailUser.Text = "";
                    ImagePessoa.Source = null;
                    UsernameUser.BorderBrush = Brushes.Red;
                    PasswordUser.BorderBrush = Brushes.Red;
                    NomeUser.BorderBrush = Brushes.Red;
                    PasswordUserPass.BorderBrush = Brushes.Red;
                    buttonsDetails.AlterDataButtonSave(-1);
                }
            }
        }

        private int Verifica_Validacao()
        {
            int conta = 0;

            if (PasswordUser.Visibility == System.Windows.Visibility.Visible)
                PasswordUserPass.Password = PasswordUser.Text;
            else
                PasswordUser.Text = PasswordUserPass.Password;


            if (NomeUser.Text.Trim() != "")
            {
                NomeUser.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                NomeUser.BorderBrush = Brushes.Red;
            }

            string pass2 = PasswordUserPass.Password;
            if (pass2.Trim() != "")
            {
                PasswordUserPass.BorderBrush = Brushes.Gray;
            }
            else
            {
                PasswordUserPass.BorderBrush = Brushes.Red;
            }

            if (UsernameUser.Text.Trim() != "")
            {
                UsernameUser.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                UsernameUser.BorderBrush = Brushes.Red;
            }

            if (PasswordUser.Text.Trim() != "")
            {
                PasswordUser.BorderBrush = Brushes.Gray;
                conta++;
            }
            else
            {
                PasswordUser.BorderBrush = Brushes.Red;
            }

            if (conta >= 3)
            {
                buttonsDetails.AlterDataButtonSave(1);
            }
            else
            {
                buttonsDetails.AlterDataButtonSave(-1);
            }

            return conta;
        }

        private void Verify_LostFocus_1(object sender, RoutedEventArgs e)
        {
            Verifica_Validacao();
        }

        private void Verify_KeyUp_1(object sender, KeyEventArgs e)
        {
           Verifica_Validacao();
        }

        private void MostrarPass_Unchecked_1(object sender, RoutedEventArgs e)
        {
            PasswordUserPass.Password = PasswordUser.Text;
            PasswordUserPass.Visibility = System.Windows.Visibility.Visible;
            PasswordUser.Visibility = System.Windows.Visibility.Hidden; 
        }

        private void MostrarPass_Checked_1(object sender, RoutedEventArgs e)
        {
            PasswordUser.Text = PasswordUserPass.Password;
            PasswordUser.Visibility = System.Windows.Visibility.Visible;
            PasswordUserPass.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
