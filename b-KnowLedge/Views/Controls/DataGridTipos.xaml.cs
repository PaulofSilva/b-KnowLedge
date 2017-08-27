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
    /// Interaction logic for DataGridTipos.xaml
    /// </summary>
    public partial class DataGridTipos : UserControl
    {
        UserControlCentro PainelCentro;
        ViewModels.Tipo type = new ViewModels.Tipo();
        ViewModels.Subtipo subtype = new ViewModels.Subtipo();
        PainelTipos painelTipos;
        DataGridSubtipo dgs;
        Window parentWindow;
        string id_type = "";
        List<Tipos> tipList;

        public DataGridTipos(List<Tipos> ls, UserControlCentro ucc, PainelTipos pt, DataGridSubtipo dgst)
        {
            InitializeComponent();
            PainelCentro = ucc;
            tipList = ls;
            ActualizaItemSource(ls);
            painelTipos = pt;
            Loaded += OnLoaded;
            textbox_Nome.BorderBrush = Brushes.Red;
            dgs = dgst;
            TabItemSubtipos.Content = dgs;



        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            string[] name2 = new string[tipList.Count];
            int i = 0;

            List<string> tipo3 = tipList.Select(u => u.Nome).Distinct().ToList<string>();

            foreach (string c in tipo3)
            {
                name2[i] = c;
                i++;
            }

            AutoComplete2.ItemsSource = name2;

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
                var tip1 = tipList.Where(u => u.Nome.ToUpper().Contains(AutoComplete2.Text.ToUpper()));
                List<Tipos> tip2 = tip1.ToList<Tipos>();

                data1.ItemsSource = tip2;
            }
            else
            {
                ActualizaItemSource(tipList);
            }
        }


        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {
            var o = this.data1.SelectedItem;

            try
            {
                Tipos tip = (Tipos)data1.SelectedItem;
                if (tip.StampTipo.Trim() != "")
                {
                    id_type = tip.StampTipo;
                    textbox_Nome.Text = tip.Nome;
                    textbox_Nome.IsReadOnly = true;
                    DescricaoTipo.Text = tip.Descricao;
                    DescricaoTipo.IsReadOnly = true;

                    if (textbox_Nome.Text.Trim() != "" )
                    {
                        textbox_Nome.BorderBrush = Brushes.Gray;    
                    }

                }
            }
            catch { }

        }


        public void ActualizaItemSource(List<Tipos> ls)
        {
            data1.ItemsSource = ls;
        }


        public void DeleteTipo()
        {
            var o = this.data1.SelectedItem;
            Tipos t2 = null;

            try
            {
                t2 = (Tipos)data1.SelectedItem;

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar o tipo " + t2.Nome + "?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = type.Delete_Tipo(t2.StampTipo);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "O tipo " + t2.Nome + " foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover o tipo " + t2.Nome + "!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        data1.ItemsSource = type.getTipo();
                        textbox_Nome.Text = "";
                        DescricaoTipo.Text = "";
                        textbox_Nome.BorderBrush = Brushes.Red;
                        break;
                    default:
                        break;


                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um tipo para eliminar!",
        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }


        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabItemTipos.IsSelected == true)
            {
                painelTipos.ActualizaBotoes(1);
            }
            else if (TabItemSubtipos.IsSelected == true)
            {
                painelTipos.ActualizaBotoes(2);
            }

            painelTipos.AlterButton(-1);
        }


        public void EditTipo()
        {
            var o = this.data1.SelectedItem;
            Tipos t2 = null;

            try
            {
                t2 = (Tipos)data1.SelectedItem;
                if (t2.StampTipo != "")
                {
                    id_type = t2.StampTipo;
                    textbox_Nome.Text = t2.Nome;
                    textbox_Nome.IsReadOnly = false;
                    DescricaoTipo.Text = t2.Descricao;
                    DescricaoTipo.IsReadOnly = false;

                    if (textbox_Nome.Text.Trim() != "")
                    {
                        textbox_Nome.BorderBrush = Brushes.Gray;
                        painelTipos.AlterButton(1);                        
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Tem que seleccionar um tipo para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Tem que seleccionar um tipo para editar!",
           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try{
                rectangleCentre.Width = parentWindow.ActualWidth / 10;
                borderLabelTipo.Width = parentWindow.ActualWidth / 4;
                textboxTipo.Width = DescricaoTipo.Width = textbox_Nome.Width = borderLabelTipo.Width- borderTamanho.ActualWidth;
                gridDatagrid.MaxHeight = parentWindow.ActualHeight - 245;
            }
            catch{}
        }

        public void NewTipo()
        {
            textbox_Nome.Text = " ";
            textbox_Nome.IsReadOnly = false;
            DescricaoTipo.Text = " ";
            textbox_Nome.Focusable = true;
            DescricaoTipo.IsReadOnly = false;
            id_type = "";

            textbox_Nome.BorderBrush = Brushes.Red;
            painelTipos.AlterButton(-1);
        }


        public void Add_Edit_Type()
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

                    ls.Add("");
                    ls.Add(textbox_Nome.Text);
                    ls.Add(DescricaoTipo.Text);
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");

                    if (id_type == "")
                    {
                        done = type.InsertTipo(ls);

                        if (done == true)
                            msg = "Adicionado com sucesso!";
                        else
                            msg = "Erro ao adicionar!\nVerifique se já existe esse tipo.";

                    }
                    else
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                         "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:

                                done = type.UpdateTipo(id_type, ls);

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
                        ActualizaItemSource(type.getTipo());

                        if (done == true)
                        {
                            textbox_Nome.Text = "";
                            DescricaoTipo.Text = "";
                            textbox_Nome.BorderBrush = Brushes.Red;
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
                painelTipos.AlterButton(-1);
            }
            else
            {
                textbox_Nome.BorderBrush = Brushes.Gray;
                painelTipos.AlterButton(1);
            }
        }

        public void ALteraTamanho()
        {
            this.Width = painelTipos.ActualWidth-20;
            this.Height = painelTipos.ActualHeight - 20;
            TabCentro.Width = this.ActualWidth;
        }

        private void textbox_Nome_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (textbox_Nome.Text.Trim() == "")
            {
                painelTipos.AlterButton(-1);
            }
            else
            {
                painelTipos.AlterButton(1);
            }
        }

    }
}