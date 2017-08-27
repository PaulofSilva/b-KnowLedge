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

namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for ButtonsGeral.xaml
    /// </summary>
    public partial class ButtonsGeral : UserControl
    {

        public event EventHandler DeleteClick;
        public event EventHandler AddClick;
        public event EventHandler RefreshClick;
        public event EventHandler EditClick;
        public event EventHandler HomeClick;
        

        public ButtonsGeral()
        {
            InitializeComponent();
        }


        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            if (AddClick != null)
                AddClick(this, e);
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            if (RefreshClick != null)
                RefreshClick(this, e);
        }


        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            if (DeleteClick != null)
                DeleteClick(this, e);
        }


        private void Edit_Click_1(object sender, RoutedEventArgs e)
        {
            if (EditClick != null)
                EditClick(this, e);
        }

        private void Home_Click_1(object sender, RoutedEventArgs e)
        {
            if (HomeClick != null)
                HomeClick(this, e);
        }


    }
}
