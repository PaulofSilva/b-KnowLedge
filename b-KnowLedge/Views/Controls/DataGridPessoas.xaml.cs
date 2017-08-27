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
using b_KnowLedge.ViewModels;
using b_KnowLedge.Models;

namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for DataGridPessoas.xaml
    /// </summary>
    public partial class DataGridPessoas : UserControl
    {

        UserControlCentro PainelCentro;
        ViewModels.Pessoa people = new Pessoa();
        List<Pessoas> pessoas;

        public DataGridPessoas(List<Pessoas> ls, UserControlCentro ucc)
        {
            InitializeComponent();
            PainelCentro = ucc;
            ActualizaItemSource(ls);
            pessoas = ls;
            Loaded += OnLoaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            string[] name2 = new string[pessoas.Count];
            int i = 0;

            List<string> pes3 = pessoas.Select(u => u.Nome).Distinct().ToList<string>();

            foreach (string c in pes3)
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
                var pes1 = pessoas.Where(u => u.Nome.ToUpper().Contains(AutoComplete1.Text.ToUpper()));
                List<Pessoas> pes = pes1.ToList<Pessoas>();

                data1.ItemsSource = pes;
            }
            else
            {
                ActualizaItemSource(pessoas);
            }
        }

        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;

            try
            {
                Pessoas pessoa = (Pessoas)data1.SelectedItem;
                if (pessoa.StampPessoa.Trim() != "")
                {
                    PainelCentro.bi = pessoa.StampPessoa;
                    PainelCentro.Escolhe_Painel(5, false);
                }
            }
            catch { }

        }


        public void ActualizaItemSource(List<Pessoas> ls)
        {
            data1.ItemsSource = ls;
        }


        public void DeletePessoa()
        {
            var o = this.data1.SelectedItem;
            Pessoas p2 = null;

            try
            {
                p2 = (Pessoas)data1.SelectedItem;

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar a pessoa " + p2.Nome + "?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = people.Delete_Pessoa(p2.StampPessoa);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A pessoa " + p2.Nome + " foi removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover a pessoa " + p2.Nome + "!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        data1.ItemsSource = people.GetPessoas();

                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma pessoa para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }


        public void EditPessoa()
        {
            var o = this.data1.SelectedItem;
            Pessoas p2 = null;

            try
            {
                p2 = (Pessoas)data1.SelectedItem;
                PainelCentro.bi = p2.StampPessoa;
                PainelCentro.Escolhe_Painel(5, false);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar uma pessoa para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        public void tamanhoDataGrid(double value)
        {
            this.data1.MaxHeight = value;
        }

        
    }
}
