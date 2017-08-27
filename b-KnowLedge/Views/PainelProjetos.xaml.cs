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

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelProjetos.xaml
    /// </summary>
    public partial class PainelProjetos : UserControl
    {

        ViewModels.Projeto projetos = new ViewModels.Projeto();
        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.DataGridProjetos dt;
        Controls.ButtonsGeral buttonsGeral;

        public PainelProjetos(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            dt = new Controls.DataGridProjetos(projetos.getProjetos(), PainelCentro);
            dd1.Children.Add(dt);
            buttonsGeral = new Controls.ButtonsGeral();
            gridFundo.Children.Add(buttonsGeral);

            buttonsGeral.DeleteClick += new EventHandler(EliminaProjeto);
            buttonsGeral.AddClick += new EventHandler(AddNewProjeto);
            buttonsGeral.EditClick += new EventHandler(EditaProjeto);
            buttonsGeral.RefreshClick += new EventHandler(Refresh);
            buttonsGeral.HomeClick += new EventHandler(Home);           

        }

        protected void EliminaProjeto(object sender, EventArgs e)
        {
            dt.DeleteProjeto();
        }

        protected void AddNewProjeto(object sender, EventArgs e)
        {
            PainelCentro.id_projeto = "";
            PainelCentro.Escolhe_Painel(6, false);
        }

        protected void EditaProjeto(object sender, EventArgs e)
        {
            dt.EditProjeto();
        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            dt.ActualizaItemSource(projetos.getProjetos());
        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(1, true);
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                dt.Height = dt.MaxHeight = parentWindow.ActualHeight - 150;
                dt.Width = dt.MaxWidth = parentWindow.ActualWidth - 323;
                dt.tamanhoDataGrid(this.ActualHeight - 175);

            }
            catch { }
        }


        private void Forward_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(7, false);
        }


        

        
    }
}
