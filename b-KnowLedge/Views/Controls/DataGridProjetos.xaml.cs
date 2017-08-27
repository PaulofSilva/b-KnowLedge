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
using b_KnowLedge.ViewModels;

namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for DataGridProjetos.xaml
    /// </summary>
    public partial class DataGridProjetos : UserControl
    {
        UserControlCentro PainelCentro;
        ViewModels.Projeto projetos = new ViewModels.Projeto();
        List<Projetos> projeto;

        public DataGridProjetos(List<Projetos> ls, UserControlCentro ucc)
        {
            InitializeComponent();
            PainelCentro = ucc;
            projeto = ls;
            ActualizaItemSource(ls);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            string[] name2 = new string[projeto.Count];
            int i = 0;

            List<string> pro3 = projeto.Select(u => u.Nome).Distinct().ToList<string>();

            foreach (string c in pro3)
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
                var pro1 = projeto.Where(u => u.Nome.ToUpper().Contains(AutoComplete1.Text.ToUpper()));
                List<Projetos> pro = pro1.ToList<Projetos>();

                data1.ItemsSource = pro;
            }
            else
            {
                ActualizaItemSource(projeto);
            }
        }

        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;

            try
            {
                Projetos projeto = (Projetos)data1.SelectedItem;

                if (projeto.StampProjeto.Trim() != "")
                {
                    PainelCentro.id_projeto = projeto.StampProjeto;
                    PainelCentro.Escolhe_Painel(6, false);
                }
            }
            catch { }


        }

        public void ActualizaItemSource(List<Projetos> ls)
        {
            data1.ItemsSource = ls;
        }


        public void DeleteProjeto()
        {

            var o = this.data1.SelectedItem;
            Projetos f2 = null;

            try
            {
                f2 = (Projetos)data1.SelectedItem;

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar o projeto " + f2.Nome + "?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = projetos.Delete_Projeto(f2.StampProjeto);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "O projeto " + f2.Nome + " foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover o projeto " + f2.Nome + "!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        data1.ItemsSource = projetos.getProjetos();

                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um projeto para eliminar!",
         "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        
        }

        public void EditProjeto()
        {
            var o = this.data1.SelectedItem;
            Projetos f2 = null;

            try
            {
                f2 = (Projetos)data1.SelectedItem;
                PainelCentro.id_projeto = f2.StampProjeto;
                PainelCentro.Escolhe_Painel(6, false);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um projeto para editar!",
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        public void tamanhoDataGrid(double value)
        {
            this.data1.MaxHeight = value;
        }

    }
}
