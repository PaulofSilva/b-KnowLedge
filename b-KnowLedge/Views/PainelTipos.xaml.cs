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
    /// Interaction logic for PainelTipos.xaml
    /// </summary>
    public partial class PainelTipos : UserControl
    {
        ViewModels.Tipo type = new ViewModels.Tipo();
        Window parentWindow;
        ViewModels.Subtipo subtype = new ViewModels.Subtipo();
        UserControlCentro PainelCentro;
        Controls.DataGridTipos dp;
        Controls.DataGridSubtipo ds;
        Controls.ButtonsTipoAndSubtipo bts;
        private int activaTabTipo = 1;

        public PainelTipos(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;

            bts = new Controls.ButtonsTipoAndSubtipo();
            GridFundo.Children.Add(bts);

            bts.DeleteClick += new EventHandler(Elimina);
            bts.AddClick += new EventHandler(Adiciona);
            bts.EditClick += new EventHandler(Edita);
            bts.HomeClick += new EventHandler(Home);
            bts.RefreshClick += new EventHandler(Refresh);
            bts.SaveClick += new EventHandler(Save);
           
            ds = new Controls.DataGridSubtipo(subtype.getSubtipo(), PainelCentro, this);
            dp = new Controls.DataGridTipos(type.getTipo(), PainelCentro, this, ds);
            dd1.Children.Add(dp);      

        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        protected void Elimina(object sender, EventArgs e)
        {
            if (activaTabTipo == 1)
            {
                dp.DeleteTipo();
            }
            else
            {
                ds.DeleteSubipo();
            }
        }

        protected void Adiciona(object sender, EventArgs e)
        {
            if (activaTabTipo == 1)
            {
                dp.NewTipo();
            }
            else
            {
                ds.NewSubtipo();
            }
        }

        protected void Edita(object sender, EventArgs e)
        {
            if (activaTabTipo == 1)
            {
                dp.EditTipo();
            }
            else
            {
                ds.EditSubtipo();
            }
        }

        protected void Refresh(object sender, EventArgs e)
        {
            if (activaTabTipo == 1)
            {
                dp.ActualizaItemSource(type.getTipo());
            }
            else
            {
                ds.ActualizaItemSource(subtype.getSubtipo());
            }
        }

        protected void Save(object sender, EventArgs e)
        {
            if (activaTabTipo == 1)
            {
                dp.Add_Edit_Type();
            }
            else
            {
                ds.Add_Edit_Subtype();
            }
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
                dp.Width = dp.MaxWidth = parentWindow.ActualWidth - 123;
                dp.ALteraTamanho();

            }
            catch { }
        }


        public void AlterButton(int i)
        {
            bts.AlterDataButtonSave(i);
        }

        public void AlterButtonSubtipo(int i)
        {
            bts.AlterDataButtonSave(i);
        }

        public void ActualizaBotoes(int nr)
        {
            activaTabTipo = nr;
        }

    }
}
