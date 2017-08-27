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
    /// Interaction logic for ButtonsTipoAndSubtipo.xaml
    /// </summary>
    public partial class ButtonsTipoAndSubtipo : UserControl
    {

        public event EventHandler DeleteClick;
        public event EventHandler SaveClick;
        public event EventHandler HomeClick;
        public event EventHandler AddClick;
        public event EventHandler RefreshClick;
        public event EventHandler EditClick;

        public ButtonsTipoAndSubtipo()
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


        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            if (RefreshClick != null)
                RefreshClick(this, e);
        }


        private void Edit_Click_1(object sender, RoutedEventArgs e)
        {
            if (EditClick != null)
                EditClick(this, e);
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            if (AddClick != null)
                AddClick(this, e);
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
