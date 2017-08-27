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

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelConhecimento.xaml
    /// </summary>
    public partial class PainelConhecimento : UserControl
    {
        Conhecimento conhecimento = new Conhecimento();
        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.DataGridConhecimento dc;
        Controls.ButtonsGeral buttonsGeral;


        public PainelConhecimento(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            dc = new Controls.DataGridConhecimento(conhecimento.GetConhecimentoGrid(), PainelCentro);
            dd1.Children.Add(dc);

            buttonsGeral = new Controls.ButtonsGeral();
            gridFundo.Children.Add(buttonsGeral);

            buttonsGeral.DeleteClick += new EventHandler(EliminaConhecimento);
            buttonsGeral.AddClick += new EventHandler(AddNewConhecimento);
            buttonsGeral.EditClick += new EventHandler(EditaConhecimento);
            buttonsGeral.RefreshClick += new EventHandler(Refresh);
            buttonsGeral.HomeClick += new EventHandler(Home);
        }


        protected void EliminaConhecimento(object sender, EventArgs e)
        {
            dc.DeleteConhecimento();
        }

        protected void AddNewConhecimento(object sender, EventArgs e)
        {
            PainelCentro.id_conhecimento = "";
            PainelCentro.Escolhe_Painel(10, false);
        }

        protected void EditaConhecimento(object sender, EventArgs e)
        {
            dc.EditConhecimento();
        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            dc.ActualizaItemSource(conhecimento.GetConhecimentoGrid());
        }

        protected void OnOkHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Painel Conhecimento");
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);

            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                dc.Height = dc.MaxHeight = parentWindow.ActualHeight - 150;
                dc.Width = dc.MaxWidth = parentWindow.ActualWidth - 323;
                dc.tamanhoDataGrid(this.ActualHeight - 175);

            }
            catch { }
        }

        
    }
}
