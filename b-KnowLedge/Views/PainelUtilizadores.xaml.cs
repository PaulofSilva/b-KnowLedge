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

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelUtilizadores.xaml
    /// </summary>
    public partial class PainelUtilizadores : UserControl
    {

        ViewModels.Utilizador user = new ViewModels.Utilizador();
        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.DataGridUtilizadores du;
        Controls.ButtonsGeral buttonsGeral;

        public PainelUtilizadores(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            du = new Controls.DataGridUtilizadores(user.GetUtilizador(), PainelCentro);
            dd1.Children.Add(du);
            buttonsGeral = new Controls.ButtonsGeral();
            GridFundo.Children.Add(buttonsGeral);

            buttonsGeral.DeleteClick += new EventHandler(EliminaUser);
            buttonsGeral.AddClick += new EventHandler(AddNewUser);
            buttonsGeral.EditClick += new EventHandler(EditaUser);
            buttonsGeral.RefreshClick += new EventHandler(Refresh);
            buttonsGeral.HomeClick += new EventHandler(Home);
        }

        protected void EliminaUser(object sender, EventArgs e)
        {
            du.DeleteUtilizador();
        }

        protected void AddNewUser(object sender, EventArgs e)
        {
            PainelCentro.id_utilizador = "";
            PainelCentro.Escolhe_Painel(13, false);
        }

        protected void EditaUser(object sender, EventArgs e)
        {
            du.EditUtilizador();
        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            du.ActualizaItemSource(user.GetUtilizador());
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
                du.Height = du.MaxHeight = parentWindow.ActualHeight - 150;
                du.Width = du.MaxWidth = parentWindow.ActualWidth - 323;
                du.tamanhoDataGrid(this.ActualHeight - 175);

            }
            catch { }
        }

    }
}
