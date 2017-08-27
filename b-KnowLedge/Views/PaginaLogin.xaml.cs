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
using System.Windows.Shapes;
using b_KnowLedge.Models;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PaginaLogin.xaml
    /// </summary>
    public partial class PaginaLogin : Window
    {
        public PaginaLogin()
        {
            InitializeComponent();

            if (Properties.Settings.Default.Username_Login != "")
            {
                LoginTextBox.Text = Properties.Settings.Default.Username_Login;
                PasswordText.Password = Properties.Settings.Default.Password_Login;
                Guardar_Login.IsChecked = true;
            }

        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void Cancel_Login_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
        private void Entrar_Login_Click(object sender, RoutedEventArgs e)
        {            
            b_KnowLedge.Classes.ConnDatabase cnn = new Classes.ConnDatabase();
            
            string done = cnn.ChechDatabase();

            if (done == "conn")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Não foi possível estabelecer conexão com o servidor da Base de Dados, verifique a configuração do mesmo.\n",
                           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                return;
            }

            if (done == "false")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("A Base de Dados não existe!\n Deseja criar?",
                           "Aviso!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);

                bool criou = false;

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        criou = cnn.CreateDatabase();
                        break;
                    default:
                        return;
                        break;
                }

                if (criou == true)
                {
                    System.Windows.Forms.MessageBox.Show("A Base de Dados foi criada com sucesso!\n Utilizador: Admin\n Password: Admin\n",
                         "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao criar a Base de Dados!\n",
                         "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

            }
            else
            {
                cnn.ConnectionDatabase("", "", "", "");

                ViewModels.Utilizador loginusr = new ViewModels.Utilizador();
                
                Utilizadores usr = loginusr.GetUser(LoginTextBox.Text, PasswordText.Password);

                if (usr == null)
                    MessageBox.Show("UserName e/ou Password errados!", "Informação!", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {

                    if (Guardar_Login.IsChecked == true)
                    {
                        Properties.Settings.Default.Username_Login = LoginTextBox.Text;
                        Properties.Settings.Default.Password_Login = PasswordText.Password;
                    }
                    else
                    {
                        Properties.Settings.Default.Username_Login = "";
                        Properties.Settings.Default.Password_Login = "";
                    }

                    Properties.Settings.Default.Save();
                    Global.idUser = usr.StampUtilizador;
                    PainelInicial painel = new PainelInicial();

                    painel.Show();
                    this.Close();
                }
            }
        }

        private void LoginTextBox_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Properties.Settings.Default.Username_Login == "")
            {
                if (LoginTextBox.Text == "")
                {
                    LoginTextBox.Text = "UserName";
                    LoginTextBox.FontStyle = FontStyles.Italic;
                    LoginTextBox.Foreground = Brushes.Silver;
                }
            }

        }

        private void LoginTextBox_Initialized_1(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Username_Login == "")
            {
                if (LoginTextBox.Text == "")
                {
                    LoginTextBox.Text = "UserName";
                    LoginTextBox.FontStyle = FontStyles.Italic;
                    LoginTextBox.Foreground = Brushes.Silver;
                }
            }
        }

        private void LoginTextBox_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Properties.Settings.Default.Username_Login == "")
            {
                if (LoginTextBox != null && LoginTextBox.Text == "UserName")
                {
                    LoginTextBox.Clear();
                    LoginTextBox.FontStyle = FontStyles.Normal;
                    LoginTextBox.Foreground = Brushes.Black;
                }
            }

        }

        private void PasswordText_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Properties.Settings.Default.Username_Login == "")
            {
                if (PasswordText != null && PasswordText.Password == "Password")
                {
                    PasswordText.Clear();
                    PasswordText.FontStyle = FontStyles.Normal;
                    PasswordText.Foreground = Brushes.Black;
                }
            }
        }

        private void PasswordText_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Properties.Settings.Default.Username_Login == "")
            {
                if (PasswordText.Password == "")
                {
                    PasswordText.Password = "Password";
                    PasswordText.FontStyle = FontStyles.Italic;
                    PasswordText.Foreground = Brushes.Silver;
                }
            }
        }

        private void PasswordText_Initialized_1(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Username_Login == "")
            {
                if (PasswordText.Password == "")
                {
                    PasswordText.Password = "Password";
                    PasswordText.FontStyle = FontStyles.Italic;
                    PasswordText.Foreground = Brushes.Silver;
                }
            }
        }

        private void Settings_Click_1(object sender, RoutedEventArgs e)
        {
            PaginaConnectDatabase pagDB = new PaginaConnectDatabase(this);
            this.Visibility = System.Windows.Visibility.Hidden;
            pagDB.Show();
        }


    }



}
