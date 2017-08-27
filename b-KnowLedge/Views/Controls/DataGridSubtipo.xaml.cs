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
    /// Interaction logic for DataGridSubtipo.xaml
    /// </summary>
    public partial class DataGridSubtipo : UserControl
    {

        UserControlCentro PainelCentro;
        ViewModels.Tipo type = new ViewModels.Tipo();
        ViewModels.Subtipo subtype = new ViewModels.Subtipo();
        Window parentWindow;
        string id_subtype = "";
        string id_tab = "";
        List<string> lsTipo = new List<string>();
        List<Tipos> lstype = new List<Tipos>();
        List<Subtipos> subt;
        PainelTipos painelTipos;


        public DataGridSubtipo(List<Subtipos> ls, UserControlCentro ucc, PainelTipos pt)
        {
            InitializeComponent();
            PainelCentro = ucc;
            subt = ls;
            ActualizaItemSource(ls);
            painelTipos = pt;
            lstype = type.getTipo();
            lsTipo.Add(" ");

            foreach (Tipos s in lstype)
            {
                lsTipo.Add(s.Nome);
            }

            Loaded += OnLoaded;

            AutoComplete1.BorderBrush = Brushes.Red;
            textbox_Nome.BorderBrush = Brushes.Red;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            lstype = type.getTipo();
            string[] name2 = new string[lstype.Count];
            int i = 0;

            foreach (Tipos em in lstype)
            {
                name2[i] = em.Nome;
                i++;
            }

            AutoComplete1.ItemsSource = name2;

            string[] name3 = new string[subt.Count];
            i = 0;

            List<string> sub3 = subt.Select(u => u.Nome).Distinct().ToList<string>();

            foreach (string c in sub3)
            {
                name3[i] = c;
                i++;
            }

            AutoComplete2.ItemsSource = name3;

        }

        private void AutoComplete1_KeyUp_2(object sender, System.Windows.Input.KeyEventArgs e)
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
            if (AutoComplete2.Text.Trim() != "")
            {
                IEnumerable<object> tabelas1 = subtype.SearchTipoSubtipoGrid(AutoComplete2.Text);
                data1.ItemsSource = tabelas1;
            }
            else
            {
                ActualizaItemSource(subt);
            }
        }


        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {

            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                id_tab = (string)type2.GetProperty("StampSubtipo").GetValue(selectedItem, null);
                Subtipos tip = subtype.getSubtipoDetails(id_tab);
                if (tip.StampSubtipo.Trim() != "")
                {

                    Tipos t2 = type.getTipoDetails(tip.StampTipo);
                    int i = 0;

                    id_subtype = tip.StampSubtipo;
                    AutoComplete1.Text = t2.Nome;

                    textbox_Nome.Text = tip.Nome;
                    textbox_Nome.IsReadOnly = true;
                    DescricaoTipo.Text = tip.Descricao;
                    DescricaoTipo.IsReadOnly = true;

                    if (textbox_Nome.Text.Trim() != "")
                    {
                        textbox_Nome.BorderBrush = Brushes.Gray;
                    }

                    if (AutoComplete1.Text.Trim() != "")
                    {
                        AutoComplete1.BorderBrush = Brushes.Gray;
                    }

                }
            }
            catch { }

        }

        public void ActualizaItemSource(List<Subtipos> ls)
        {
            IEnumerable<object> lt = subtype.TipoSubtipoGrid();
            data1.ItemsSource = lt;
        }


        public void DeleteSubipo()
        {

            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                id_tab = (string)type2.GetProperty("StampSubtipo").GetValue(selectedItem, null);
                Subtipos s2 = subtype.getSubtipoDetails(id_tab);

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar o subtipo " + s2.Nome + "?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = subtype.Delete_Subtipo(s2.StampSubtipo);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "O subtipo " + s2.Nome + " foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover o subtipo " + s2.Nome + "!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        ActualizaItemSource(subt);

                        AutoComplete1.Text = "";
                        textbox_Nome.Text = "";
                        DescricaoTipo.Text = "";
                        AutoComplete1.BorderBrush = Brushes.Red;
                        textbox_Nome.BorderBrush = Brushes.Red;

                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um subtipo para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }


        public void EditSubtipo()
        {


            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                id_tab = (string)type2.GetProperty("StampSubtipo").GetValue(selectedItem, null);
                Subtipos s2 = subtype.getSubtipoDetails(id_tab);
                if (s2.StampSubtipo != "")
                {

                    Tipos t2 = type.getTipoDetails(s2.StampTipo);
                    int i = 0;

                    AutoComplete1.Text = t2.Nome;

                    id_subtype = s2.StampSubtipo;
                    textbox_Nome.Text = s2.Nome;
                    textbox_Nome.IsReadOnly = false;
                    DescricaoTipo.Text = s2.Descricao;
                    DescricaoTipo.IsReadOnly = false;

                    if (textbox_Nome.Text.Trim() != "")
                    {
                        textbox_Nome.BorderBrush = Brushes.Gray;
                    }

                    if (AutoComplete1.Text.Trim() != "")
                    {
                        AutoComplete1.BorderBrush = Brushes.Gray;
                    }

                    if (AutoComplete1.Text.Trim() != "" && textbox_Nome.Text.Trim() != "")
                    {
                        painelTipos.AlterButtonSubtipo(1);
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Tem que seleccionar um subtipo para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um subtipo para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
                rectangleCentre.Width = parentWindow.ActualWidth / 10;
                borderLabelTipo.Width = parentWindow.ActualWidth / 4;
                textboxTipo.Width = DescricaoTipo.Width = textbox_Nome.Width = borderLabelTipo.Width - borderTamanho.ActualWidth;
                gridDatagrid.MaxHeight = parentWindow.ActualHeight - 245;
            }
            catch { }
        }

        public void NewSubtipo()
        {
            AutoComplete1.Text = " ";
            textbox_Nome.Text = " ";
            textbox_Nome.IsReadOnly = false;
            DescricaoTipo.Text = " ";
            textbox_Nome.Focusable = true;
            DescricaoTipo.IsReadOnly = false;
            id_subtype = "";

            textbox_Nome.BorderBrush = Brushes.Red;
            AutoComplete1.BorderBrush = Brushes.Red;
            painelTipos.AlterButtonSubtipo(-1);
        }


        public void Add_Edit_Subtype()
        {

            if (AutoComplete1.Text.Trim() == "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o tipo!\n Deseja criar um tipo novo?",
                                "Aviso!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        PainelCentro.Escolhe_Painel(1, false);
                        PainelCentro.Escolhe_Painel(11, true);
                        break;
                    default:
                        break;

                }

            }
            else
            {
                if (textbox_Nome.Text.Trim() == "")
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o Nome!",
                            "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    if (textbox_Nome.IsReadOnly == false)
                    {
                        bool done = true, window = true;
                        string msg = "";
                        List<string> ls = new List<string>();

                        if (AutoComplete1.Text != " ")
                        {

                            string nr = type.getIDFromNomeTipo(AutoComplete1.Text);

                            if (nr != "")
                            {
                                ls.Add("");
                                ls.Add(Convert.ToString(nr));
                                ls.Add(textbox_Nome.Text);
                                ls.Add(DescricaoTipo.Text);
                                ls.Add("");
                                ls.Add("");
                                ls.Add("");
                                ls.Add("");
                                ls.Add("");
                                ls.Add("");
                                ls.Add("");

                                if (id_subtype == "")
                                {
                                    done = subtype.InsertSubtipo(ls);

                                    if (done == true)
                                        msg = "Adicionado com sucesso!";
                                    else
                                        msg = "Erro ao adicionar!\nVerifique se já existe esse subtipo.";

                                }
                                else
                                {
                                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                                     "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                                    switch (result)
                                    {
                                        case System.Windows.Forms.DialogResult.Yes:

                                            done = subtype.UpdateSubtipo(id_subtype, ls);

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


                                if (window == true)
                                {
                                    System.Windows.Forms.MessageBox.Show(msg,
                                    "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                                    ActualizaItemSource(subtype.getSubtipo());

                                    if (done == true)
                                    {
                                        AutoComplete1.Text = "";
                                        textbox_Nome.Text = "";
                                        DescricaoTipo.Text = "";
                                        AutoComplete1.BorderBrush = Brushes.Red;
                                        textbox_Nome.BorderBrush = Brushes.Red;
                                    }
                                }
                            }
                            else
                            {
                                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("O tipo inserido é inválido!\n Deseja criar um tipo novo?",
                                "Aviso!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);

                                switch (result)
                                {
                                    case System.Windows.Forms.DialogResult.Yes:
                                        PainelCentro.Escolhe_Painel(1, false);
                                        PainelCentro.Escolhe_Painel(11, true);
                                        break;
                                    default:
                                        break;

                                }
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Tem que seleccionar um tipo!",
                                    "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void textbox_Nome_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (textbox_Nome.Text.Trim() == "")
            {
                textbox_Nome.BorderBrush = Brushes.Red;
                painelTipos.AlterButtonSubtipo(-1);
            }
            else
            {
                textbox_Nome.BorderBrush = Brushes.Gray;
                if (AutoComplete1.Text.Trim() != "")
                    painelTipos.AlterButtonSubtipo(1);
            }
        }

        private void AutoComplete1_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (AutoComplete1.Text.Trim() == "")
            {
                painelTipos.AlterButtonSubtipo(-1);
            }
            else if (textbox_Nome.Text.Trim() != "")
            {
                painelTipos.AlterButtonSubtipo(1);
            }
        }

        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (AutoComplete1.Text.Trim() == "")
            {
                AutoComplete1.BorderBrush = Brushes.Red;
                painelTipos.AlterButtonSubtipo(-1);
            }
            else
            {
                AutoComplete1.BorderBrush = Brushes.Gray;
                if (textbox_Nome.Text.Trim() != "")
                    painelTipos.AlterButtonSubtipo(1);
            }
        }

        private void textbox_Nome_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (textbox_Nome.Text.Trim() == "")
            {
                painelTipos.AlterButtonSubtipo(-1);
            }
            else if (AutoComplete1.Text.Trim() != "")
            {
                painelTipos.AlterButtonSubtipo(1);
            }
        }

    }
}