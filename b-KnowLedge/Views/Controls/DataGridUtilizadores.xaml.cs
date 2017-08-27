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

namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for DataGridUtilizadores.xaml
    /// </summary>
    public partial class DataGridUtilizadores : UserControl
    {

        UserControlCentro PainelCentro;
        ViewModels.Utilizador user = new ViewModels.Utilizador();
        List<Utilizadores> utilizadores;

        public DataGridUtilizadores(List<Utilizadores> ls, UserControlCentro ucc)
        {
            InitializeComponent();
            PainelCentro = ucc;
            utilizadores = ls;
            Loaded += OnLoaded;
            ActualizaItemSource(ls);
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            string[] name2 = new string[utilizadores.Count];
            int i = 0;

            List<string> user3 = utilizadores.Select(u => u.Username).Distinct().ToList<string>();

            foreach (string c in user3)
            {
                name2[i] = c;
                i++;
            }

            AutoComplete1.ItemsSource = name2;
        }

        private void AutoComplete1_KeyUp_1(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Search();
            }
        }


        private void Search_Click_1(object sender, RoutedEventArgs e)
        {
            this.Search();
        }

        private void Search()
        {
            if (AutoComplete1.Text.Trim() != "")
            {
                var user1 = utilizadores.Where(u => u.Username.ToUpper().Contains(AutoComplete1.Text.ToUpper()));
                List<Utilizadores> user2 = user1.ToList<Utilizadores>();

                data1.ItemsSource = user2;
            }
            else
            {
                ActualizaItemSource(utilizadores);
            }
        }

        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;

            try
            {
                Utilizadores utilizador = (Utilizadores)data1.SelectedItem;
                if (utilizador.StampUtilizador.Trim() != "")
                {
                    PainelCentro.id_utilizador = utilizador.StampUtilizador;
                    PainelCentro.Escolhe_Painel(13, false);
                }
            }
            catch { }

        }


        public void ActualizaItemSource(List<Utilizadores> ls)
        {
            BDKnowLedge bd = new BDKnowLedge();
            data1.ItemsSource = bd.Utilizadores.ToList();
        }


        public void DeleteUtilizador()
        {
            var o = this.data1.SelectedItem;
            Utilizadores u2 = null;

            try
            {
                u2 = (Utilizadores)data1.SelectedItem;

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar o utilizador " + u2.Nome + "?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = user.Delete_Utilizador(u2.StampUtilizador);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "O utilizador " + u2.Nome + " foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover o utilizador " + u2.Nome + "!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        data1.ItemsSource = user.GetUtilizador();

                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um utilizador para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }


        public void EditUtilizador()
        {
            var o = this.data1.SelectedItem;
            Utilizadores u2 = null;

            try
            {
                u2 = (Utilizadores)data1.SelectedItem;
                PainelCentro.id_utilizador = u2.StampUtilizador;
                PainelCentro.Escolhe_Painel(13, false);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um utilizador para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        public void tamanhoDataGrid(double value)
        {
            this.data1.MaxHeight = value;
        }

    }
}
