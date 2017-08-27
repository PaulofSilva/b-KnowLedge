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

namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for DataGridDatabases.xaml
    /// </summary>
    public partial class DataGridDatabases : UserControl
    {

        UserControlCentro PainelCentro;
        ViewModels.BasesdeDados database = new ViewModels.BasesdeDados();
        List<BasesDados> basedados;

        public DataGridDatabases(List<BasesDados> ls, UserControlCentro ucc)
        {
            InitializeComponent();
            PainelCentro = ucc;
            basedados = ls;
            ActualizaItemSource(ls);
            Loaded += OnLoaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            string[] name2 = new string[basedados.Count];
            int i = 0;

            List<string> bd3 = basedados.Select(u => u.Servername).Distinct().ToList<string>();

            foreach (string c in bd3)
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
                var bd1 = basedados.Where(u => u.Servername.ToUpper().Contains(AutoComplete1.Text.ToUpper()));
                List<BasesDados> bd2 = bd1.ToList<BasesDados>();

                data1.ItemsSource = bd2;
            }
            else
            {
                ActualizaItemSource(basedados);
            }
        }


        public void ActualizaItemSource(List<BasesDados> ls)
        {
            data1.ItemsSource = ls;
        }


        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;

            try
            {
                BasesDados bd = (BasesDados)data1.SelectedItem;

                if (bd.StampBaseDados.Trim() != "")
                {
                    PainelCentro.id_bd = bd.StampBaseDados;
                    PainelCentro.Escolhe_Painel(8, false);
                }
            }
            catch { }


        }


        public void DeleteBD()
        {
            var o = this.data1.SelectedItem;
            BasesDados b2 = null;

            try
            {
                b2 = (BasesDados)data1.SelectedItem;

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar a base de dados? ",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = database.Delete_BasedeDados(b2.StampBaseDados);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A base de dados foi removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover a base de dados!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        data1.ItemsSource = database.GetBasesDeDados();

                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma base de dados para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }


        public void EditBD()
        {
            var o = this.data1.SelectedItem;
            BasesDados b2 = null;

            try
            {
                b2 = (BasesDados)data1.SelectedItem;
                PainelCentro.id_bd = b2.StampBaseDados;
                PainelCentro.Escolhe_Painel(8, false);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma base de dados para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        public void tamanhoDataGrid(double value)
        {
            this.data1.MaxHeight = value;
        }

    }
}
