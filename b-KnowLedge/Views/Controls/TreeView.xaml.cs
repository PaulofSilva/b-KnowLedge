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
using b_KnowLedge.ViewModels;

namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class TreeView : UserControl
    {

        Projeto projeto = new Projeto();
        ViewModels.Tipo tipo = new ViewModels.Tipo();
        Subtipo subtipo = new Subtipo();
        Conhecimento conhecimento = new Conhecimento();
        PainelDetalhesConhecimento painelDetalhesConhecimento;
        Window parentWindow;


        public TreeView(PainelDetalhesConhecimento painelCon)
        {
            InitializeComponent();
            this.Preenche_TreeView();
            painelDetalhesConhecimento = painelCon;
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);

            try
            {
                search_Conhecimento.Width = treeView.ActualWidth - 40;
                rectangulo.Width = treeView.ActualWidth;
                treeView.Height = this.ActualHeight - 40;
            }
            catch { }
        }


        private void Preenche_TreeView()
        {
            try
            {

                //var f2 = projeto.getProjetos();

                //foreach (Projetos f in f2)
                //{
                //    TreeViewItem Parent = new TreeViewItem();
                //    Parent.Header = f.Designacao;

                //    int id = f.ID_Projeto;

                List<Conhecimentos> know = conhecimento.GetConhecimento();
                this.Preenche_TreeView_Pesquisa(know);

                //foreach (Tipos t in t2)
                //{
                //    TreeViewItem Child = new TreeViewItem();
                //    Child.Header = t.Nome;


                //    var s2 = subtipo.getSubtipo();

                //    foreach (Subtipos s in s2)
                //    {
                //        TreeViewItem Child2 = new TreeViewItem();
                //        Child2.Header = s.Nome;

                //        var c2 = conhecimento.getConhecimento();

                //        foreach (Conhecimentos con in c2)
                //        {
                //            TreeViewItem Child3 = new TreeViewItem();
                //            Child3.Header = con.Metadados;
                //            Child3.Tag = con.ID_Conhecimento;
                //            Child2.Items.Add(Child3);
                //        }

                //        Child.Items.Add(Child2);

                //    }

                ////    Parent.Items.Add(Child);

                ////}

                //treeView.Items.Add(Child);

                //}
            }
            catch { }
        }

        private void treeView_SelectedItemChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string num = "";

            try
            {
                TreeViewItem tr = (TreeViewItem)treeView.SelectedItem;
                var nr = tr.Tag;
                num = Convert.ToString(nr);
            }
            catch { }

            if (num != "")
            {
                painelDetalhesConhecimento.Preenche(num);
            }

        }



        private void Button_Click_Search_1(object sender, RoutedEventArgs e)
        {
            string fil = painelDetalhesConhecimento.Verify_ComboProjetos_PropertyGrid();
            string emp = painelDetalhesConhecimento.Verify_ComboEntidades_PropertyGrid();
            string tip = painelDetalhesConhecimento.Verify_ComboTipos_PropertyGrid();
            string subtip = painelDetalhesConhecimento.Verify_ComboSubtipos_PropertyGrid();
            List<Conhecimentos> know = new List<Conhecimentos>();
            bool code = false;
            if (search_Conhecimento.Text.Trim() != "")
            {
                if (CheckInCode.IsChecked == true)
                    code = true;

                know = conhecimento.Search(search_Conhecimento.Text, fil, emp, subtip, tip, code);
                this.Preenche_TreeView_Pesquisa(know);
            }
            else if (fil.Trim() != "" || emp.Trim() != "" || tip.Trim() != "" || subtip.Trim() != "")
            {
                know = conhecimento.Search(search_Conhecimento.Text, fil, emp, subtip, tip, code);
                this.Preenche_TreeView_Pesquisa(know);
            }
            
        }


        private void Preenche_TreeView_Pesquisa(List<Conhecimentos> know)
        {
            lock (treeView)
            {
                treeView.Items.Clear();
            }

            try
            {
                var t2 = tipo.Search_Tipo(know);

                foreach (Tipos t in t2)
                {
                    TreeViewItem Child = new TreeViewItem();
                    Child.Header = t.Nome;


                    var s2 = subtipo.Search_Subtipo(know);

                    foreach (Subtipos s in s2)
                    {
                        TreeViewItem Child2 = new TreeViewItem();
                        Child2.Header = s.Nome;

                        var c2 = know;

                        foreach (Conhecimentos con in c2)
                        {
                            TreeViewItem Child3 = new TreeViewItem();
                            Child3.Header = con.Metadados;
                            Child3.Tag = con.StampConhecimento;

                            if (s.StampSubtipo == con.StampSubtipo)
                                Child2.Items.Add(Child3);
                        }

                        if (t.StampTipo == s.StampTipo)
                            Child.Items.Add(Child2);

                    }


                    treeView.Items.Add(Child);

                }
            }
            catch
            {

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RightClickMenu.PlacementTarget = this;
            RightClickMenu.IsOpen = true;
        }

        private void Button_Click_Clear_1(object sender, RoutedEventArgs e)
        {
                painelDetalhesConhecimento.LimpaCampos();
        }


    }
}
