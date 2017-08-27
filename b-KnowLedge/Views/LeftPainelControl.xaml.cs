using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    
    public partial class LeftPainelControl : UserControl
    {
        Window parentWindow;
        UserControlCentro PainelCentro;

        public LeftPainelControl(UserControlCentro ucc)
        {
            InitializeComponent();
            PainelCentro = ucc;
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
               // MainNavigationAccordion.Height = MainNavigationAccordion.MaxHeight = parentWindow.ActualHeight - 73;
                MainNavigationAccordion.Height = MainNavigationAccordion.MaxHeight = parentWindow.ActualHeight - 43;
            }
            catch
            { 
            
            }
            this.MainNavigationAccordion.UpdateLayout();
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(1, false);
        }

        private void Hyperlink_RequestNavigate2(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.id_entidade = "";
            PainelCentro.Escolhe_Painel(4, false);
        }

        private void Hyperlink_RequestNavigate3(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(2, false);
        }

        private void Hyperlink_RequestNavigate4(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.id_projeto = "";
            PainelCentro.Escolhe_Painel(6, false);
        }

        private void Hyperlink_RequestNavigate5(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(7, false);
        }

        private void Hyperlink_RequestNavigate6(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.id_bd = "";
            PainelCentro.Escolhe_Painel(8, false);
        }

        private void Hyperlink_RequestNavigate7(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(3, false);
        }

        private void Hyperlink_RequestNavigate8(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.bi = "";
            PainelCentro.Escolhe_Painel(5, false);
        }

        private void Hyperlink_RequestNavigate9(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(9, false);
        }

        private void Hyperlink_RequestNavigate10(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.id_conhecimento = "";
            PainelCentro.Escolhe_Painel(10, false);
            //Screen_KnowLedge.MainWindow mw = new Screen_KnowLedge.MainWindow();
            //mw.Show();
        }

        private void Hyperlink_RequestNavigate11(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(15, false);
        }

        private void Hyperlink_RequestNavigate12(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(11, false);
        }

        private void Hyperlink_RequestNavigate13(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(12, false);
        }

        private void Hyperlink_RequestNavigate14(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.id_utilizador = "";
            PainelCentro.Escolhe_Painel(13, false);
        }

        private void Hyperlink_RequestNavigate15(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(14, false);
        }

        private void Hyperlink_RequestNavigate_1(object sender, RequestNavigateEventArgs e)
        {
            PainelCentro.Escolhe_Painel(16, false);
        }

     

        
    }
}
