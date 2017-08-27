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
using System.Collections;


namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelEntidades.xaml
    /// </summary>
    public partial class PainelEntidades : UserControl
    {

        PainelDetalhesEntidades painelDetalhesEntidade;
        ViewModels.Entidade entidades = new ViewModels.Entidade();
        List<Entidades> entidade;
        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.ButtonsGeral bg;

        public PainelEntidades(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            entidade = entidades.GetEntidade();
            data1.ItemsSource = entidade;
            Loaded += OnLoaded;
            
            
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            string[] name2 = new string[entidade.Count];
            int i = 0;

            List<string> ent3 = entidade.Select(u => u.Nome).Distinct().ToList<string>();

            foreach (string c in ent3)
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
                var ent1 = entidade.Where(u => u.Nome.ToUpper().Contains(AutoComplete1.Text.ToUpper()));
                List<Entidades> ent = ent1.ToList<Entidades>();

                data1.ItemsSource = ent1;
                data1.IsReadOnly = true;
            }
            else
            {
                ActualizaItemSource(entidade);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {

                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                data1.Height = data1.MaxHeight = parentWindow.ActualHeight - 210;
                data1.Width = data1.MaxWidth = parentWindow.ActualWidth - 323;
                
            }
            catch { }
        }


        /// <summary>
        /// Evento despoletado quando há um duplo-click numa linha do Datagrid.
        /// </summary>
        /// <remarks>
        /// Sempre que há um duplo click numa determinada linha do Datagrid é despoletado este evento, e é obtido o id da entidade dessa mesma linha,
        /// e usado posteriormente na pesquisa.
        /// </remarks>
        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;

            try
            {
                Entidades e2 = (Entidades)data1.SelectedItem;

                if (e2.StampEntidade.Trim() != "")
                {
                    PainelCentro.id_entidade= e2.StampEntidade;
                    PainelCentro.Escolhe_Painel(4, false);
                }
            }
            catch { }

        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(9, true);
        }


        public void visible()
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }


        private void Forward_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(2, false);
        }


        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.id_entidade = "";
            PainelCentro.Escolhe_Painel(4, false);
        }


        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void ActualizaItemSource(List<Entidades> ls)
        {
            data1.ItemsSource = ls;
        }


        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;
            Entidades e2 = null;

            try
            {
                e2 = (Entidades)data1.SelectedItem;

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar a entidade " + e2.Nome + "?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = entidades.Delete_Entidade(e2.StampEntidade);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A entidade " + e2.Nome + " foi removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover a entidade " + e2.Nome + "!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        data1.ItemsSource = entidades.GetEntidade();

                        break;
                    default:
                        break;


                }
            }
            catch 
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma entidade para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void Edit_Click_1(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;
            Entidades e2 = null;

            try
            {
                e2 = (Entidades)data1.SelectedItem;
                PainelCentro.id_entidade = e2.StampEntidade;
                PainelCentro.Escolhe_Painel(4, false);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma entidade para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            
        }


        private void Home_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }
        


    }
}
