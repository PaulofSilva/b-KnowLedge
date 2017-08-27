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
using System.Windows.Shapes;
using System.IO;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelInicial.xaml
    /// </summary>
    public partial class PainelInicial : Window
    {
        LeftPainelControl tt;
        UserControlCentro userControlCentro;

        public PainelInicial()
        {
            InitializeComponent();
            userControlCentro = new UserControlCentro();
            tt = new LeftPainelControl(userControlCentro);
            teste.Children.Add(tt);
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            teste.Width = 200;
            teste.Height = this.ActualHeight;
            tt.Height = this.ActualHeight;

            stackpanelcentro.Height = this.ActualHeight;

            try
            {
               
                
                userControlCentro.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                stackpanelcentro.Children.Add(userControlCentro);
            }
            catch { }

            //Paineltab.Height = this.ActualHeight - 60;
            
        }

        private void teste_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void AnimatedExpander_SizeChanged(object sender, SizeChangedEventArgs e)
        {
                if(borderMedida.ActualWidth >500)
                    userControlCentro.Width = borderMedida.ActualWidth;
        }

        

       
    }
}
