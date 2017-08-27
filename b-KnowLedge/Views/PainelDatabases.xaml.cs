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

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDatabases.xaml
    /// </summary>
    public partial class PainelDatabases : UserControl
    {

        ViewModels.BasesdeDados database = new ViewModels.BasesdeDados();
        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.DataGridDatabases db;
        Controls.ButtonsGeral buttonsGeral;

        public PainelDatabases(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            db = new Controls.DataGridDatabases(database.GetBasesDeDados(), PainelCentro);
            dd1.Children.Add(db);
            buttonsGeral = new Controls.ButtonsGeral();
            gridFundo.Children.Add(buttonsGeral);

            buttonsGeral.DeleteClick += new EventHandler(EliminaBD);
            buttonsGeral.AddClick += new EventHandler(AddNewBD);
            buttonsGeral.EditClick += new EventHandler(EditaBD);
            buttonsGeral.RefreshClick += new EventHandler(Refresh);
            buttonsGeral.HomeClick += new EventHandler(Home);
        }


        protected void EliminaBD(object sender, EventArgs e)
        {
            db.DeleteBD();
        }

        protected void AddNewBD(object sender, EventArgs e)
        {
            PainelCentro.id_bd = "";
            PainelCentro.Escolhe_Painel(8, false);
        }

        protected void EditaBD(object sender, EventArgs e)
        {
            db.EditBD();
        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            db.ActualizaItemSource(database.GetBasesDeDados());
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(2, true);
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);

            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                db.Height = db.MaxHeight = parentWindow.ActualHeight - 150;
                db.Width = db.MaxWidth = parentWindow.ActualWidth - 323;
                db.tamanhoDataGrid(this.ActualHeight - 175);

            }
            catch { }
        }

        private void Forward_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(3, false);
        }
    }
}
