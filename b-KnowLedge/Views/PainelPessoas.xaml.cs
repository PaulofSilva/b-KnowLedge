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
    /// Interaction logic for PainelPessoas.xaml
    /// </summary>
    public partial class PainelPessoas : UserControl
    {

        ViewModels.Pessoa people = new ViewModels.Pessoa();
        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.DataGridPessoas dp;
        Controls.ButtonsGeral buttonsGeral;
        
        public PainelPessoas(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            dp = new Controls.DataGridPessoas(people.GetPessoas(), PainelCentro);
            dd1.Children.Add(dp);
            buttonsGeral = new Controls.ButtonsGeral();
            GridFundo.Children.Add(buttonsGeral);

            buttonsGeral.DeleteClick += new EventHandler(EliminaPessoa);
            buttonsGeral.AddClick += new EventHandler(AddNewPessoa);
            buttonsGeral.EditClick += new EventHandler(EditaPessoa);
            buttonsGeral.RefreshClick += new EventHandler(Refresh);
            buttonsGeral.HomeClick += new EventHandler(Home);

        }

        protected void EliminaPessoa(object sender, EventArgs e)
        {
            dp.DeletePessoa();
        }

        protected void AddNewPessoa(object sender, EventArgs e)
        {
            PainelCentro.bi = "";
            PainelCentro.Escolhe_Painel(5, false);
        }

        protected void EditaPessoa(object sender, EventArgs e)
        {
            dp.EditPessoa();
        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            dp.ActualizaItemSource(people.GetPessoas());
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(7, true);
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);

            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                dp.Height = dp.MaxHeight = parentWindow.ActualHeight - 150;
                dp.Width = dp.MaxWidth = parentWindow.ActualWidth - 323;
                dp.tamanhoDataGrid(this.ActualHeight - 175);

            }
            catch { }
        }

    }
}
