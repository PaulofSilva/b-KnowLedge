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
    /// Interaction logic for ButtonsDetails.xaml
    /// </summary>
    public partial class ButtonsDetails : UserControl
    {

        public event EventHandler DeleteClick;
        public event EventHandler SaveClick;
        public event EventHandler HomeClick;

        public ButtonsDetails()
        {
            InitializeComponent();
        }


        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            if (DeleteClick != null)
                DeleteClick(this, e);
        }


        private void Save_Click_1(object sender, RoutedEventArgs e)
        {
            if (SaveClick != null)
                SaveClick(this, e);
        }


        private void Home_Click_1(object sender, RoutedEventArgs e)
        {
            if (HomeClick != null)
                HomeClick(this, e);
        }


        public void AlterDataButtonSave(int i)
        {

            if (i == 1)
            {
                Save.IsEnabled = true;
                ImageSave.Opacity = 1;
            }
            else if (i == -1)
            {
                Save.IsEnabled = false;
                ImageSave.Opacity = 0.3;
            }

        }

    }

    
}
