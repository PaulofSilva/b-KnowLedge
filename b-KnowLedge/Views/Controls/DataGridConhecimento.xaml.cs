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
    /// Interaction logic for DataGridConhecimento.xaml
    /// </summary>
    public partial class DataGridConhecimento : UserControl
    {

        UserControlCentro PainelCentro;
        ViewModels.Conhecimento conhecimento = new ViewModels.Conhecimento();
        IQueryable<object> tr;
        int valCheck = 0;

        public DataGridConhecimento(IQueryable<object> tr1, UserControlCentro ucc)
        {
            InitializeComponent();
            PainelCentro = ucc;
            tr = tr1;
            ActualizaItemSource(tr);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AutoComplete1.ItemsSource = conhecimento.getConhecimentoAutoComplete(0);
            CheckInMetadados.IsChecked = true;
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
                data1.ItemsSource = null;
                data1.ItemsSource = conhecimento.GetSearchConhecimentoGrid(AutoComplete1.Text, valCheck).ToList();
            }
            else
            {
                ActualizaItemSource(tr);
            }
        }

        public void ActualizaItemSource(IQueryable<object> tr)
        {
            data1.ItemsSource = tr.ToList();
        }


        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                string id_tab = (string)type2.GetProperty("ID_Conhecimento").GetValue(selectedItem, null);

                if (id_tab != "")
                {
                    PainelCentro.id_conhecimento = id_tab;
                    PainelCentro.Escolhe_Painel(10, false);
                }
            }
            catch { }
        }


        public void DeleteConhecimento()
        {
            string id_tab = "";
            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                id_tab = (string)type2.GetProperty("ID_Conhecimento").GetValue(selectedItem, null);
            }
            catch { }

            try
            {

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar? ",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = conhecimento.Delete_Conhecimento(id_tab);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "Foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        tr = conhecimento.GetConhecimentoGrid();
                        this.ActualizaItemSource(tr);

                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma linha para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }


        public void EditConhecimento()
        {
            string id_tab = "";
            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                id_tab = (string)type2.GetProperty("ID_Conhecimento").GetValue(selectedItem, null);
            }
            catch { }

            try
            {
                PainelCentro.id_conhecimento = id_tab;
                PainelCentro.Escolhe_Painel(10, false);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma linha para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        public void tamanhoDataGrid(double value)
        {
            this.data1.MaxHeight = value;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RightClickMenu.PlacementTarget = this;
            RightClickMenu.IsOpen = true;
        }

        private void CheckInMetadados_Checked_1(object sender, RoutedEventArgs e)
        {
            AutoComplete1.ItemsSource = conhecimento.getConhecimentoAutoComplete(0);
            CheckInMetadados.IsChecked = true;
            CheckInCode.IsChecked = false;
            CheckInEntidade.IsChecked = false;
            CheckInProjeto.IsChecked = false;
            valCheck = 0;
        }

        private void CheckInCode_Checked_1(object sender, RoutedEventArgs e)
        {
            AutoComplete1.ItemsSource = null;
            CheckInMetadados.IsChecked = false;
            CheckInCode.IsChecked = true;
            CheckInEntidade.IsChecked = false;
            CheckInProjeto.IsChecked = false;
            valCheck = 3;
        }

        private void CheckInProjeto_Checked_1(object sender, RoutedEventArgs e)
        {
            AutoComplete1.ItemsSource = conhecimento.getConhecimentoAutoComplete(2);
            CheckInMetadados.IsChecked = false;
            CheckInCode.IsChecked = false;
            CheckInEntidade.IsChecked = false;
            CheckInProjeto.IsChecked = true;
            valCheck = 2;
        }

        private void CheckInEntidade_Checked_1(object sender, RoutedEventArgs e)
        {
            AutoComplete1.ItemsSource = conhecimento.getConhecimentoAutoComplete(1);
            CheckInMetadados.IsChecked = false;
            CheckInCode.IsChecked = false;
            CheckInEntidade.IsChecked = true;
            CheckInProjeto.IsChecked = false;
            valCheck = 1;
        }

    }
}
