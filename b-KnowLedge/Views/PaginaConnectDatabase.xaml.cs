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
    /// Interaction logic for PaginaConnectDatabase.xaml
    /// </summary>
    public partial class PaginaConnectDatabase : Window
    {
        PaginaLogin PaginaLogin;
        public PaginaConnectDatabase(PaginaLogin login)
        {
            InitializeComponent();
            PaginaLogin = login;

            if (Properties.Settings.Default.Servidor != "")
            {
                ServidorTextBox.Text = Properties.Settings.Default.Servidor;
                BDText.Text = Properties.Settings.Default.BDName;
                UserTextBox.Text = Properties.Settings.Default.Login;
                PassTextBox.Password = Properties.Settings.Default.Password;
            }

            Properties.Settings.Default.Save();
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void Cancel_Login_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PaginaLogin.Visibility = System.Windows.Visibility.Visible;
        }


        private void Entrar_Login_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Servidor = ServidorTextBox.Text;
            Properties.Settings.Default.BDName = BDText.Text;
            Properties.Settings.Default.Login = UserTextBox.Text;
            Properties.Settings.Default.Password = PassTextBox.Password;
            Properties.Settings.Default.Save();

            PaginaLogin.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }

        private void Test_Connection_Click_1(object sender, RoutedEventArgs e)
        {
            Classes.ConnDatabase connDB = new Classes.ConnDatabase();
            bool resultado = false;

            this.Cursor = Cursors.Wait;
            try
            {
                resultado = connDB.ConnectionDatabase(ServidorTextBox.Text, BDText.Text, UserTextBox.Text, PassTextBox.Password);
            }
            catch { }

            string msg = "";

            if (resultado == true)
                MessageBox.Show("Conexão bem sucedida!", "Informação!", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (resultado == false)
                MessageBox.Show("Erro na conexão!", "Informação!", MessageBoxButton.OK, MessageBoxImage.Warning);

            this.Cursor = Cursors.Arrow;



        }

        private void Limpar_Connection_Click_1(object sender, RoutedEventArgs e)
        {
            ServidorTextBox.Text = "";
            BDText.Text = "";
            UserTextBox.Text = "";
            PassTextBox.Password = "";

            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }





    }
}
