using b_KnowLedge.ViewModels;
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
    /// Interaction logic for PainelDetalhesEntidades.xaml
    /// </summary>
    public partial class PainelDetalhesEntidades : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        public string id_Emp = "";
        private ViewModels.Entidade emp;
        ViewModels.Projeto projeto = new ViewModels.Projeto();
        ViewModels.Pessoa people = new Pessoa();
        ViewModels.Conhecimento conhecimento = new Conhecimento();
        Controls.DataGridProjetos dt;
        Controls.DataGridPessoas dp;
        Controls.DataGridConhecimento dgc;
        Controls.ButtonsDetails buttonsDetails;
        Controls.ButtonsGeral buttonsGeral;
        ViewModels.BasesdeDados databases = new BasesdeDados();
        Controls.DataGridDatabases dgbd;
        bool actualizaButtons = false;

        public PainelDetalhesEntidades(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            emp = new ViewModels.Entidade();
            PainelCentro.backward = true;

            buttonsDetails = new Controls.ButtonsDetails();
            //gridFundo.Children.Add(buttonsDetails);

           

            tabPessoas.Visibility = System.Windows.Visibility.Hidden;
            tabProjetos.Visibility = System.Windows.Visibility.Hidden;
            tabConhecimento.Visibility = System.Windows.Visibility.Hidden;
            tabDatabases.Visibility = System.Windows.Visibility.Hidden;

            DetalhesEntidade.IsSelected = true;

            NomeEntidade.BorderBrush = Brushes.Red;

        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                TabCentro.MaxWidth = TabCentro.Width = GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                TabCentro.Height = TabCentro.MaxHeight = border_centro.Height = border_centro.MaxHeight = parentWindow.ActualHeight - 150;
                border_centro.Height = border_centro.MaxHeight - 50;
                if(dgbd != null)
                    dgbd.tamanhoDataGrid(this.ActualHeight - 175);
                if(dgc != null)
                    dgc.tamanhoDataGrid(this.ActualHeight - 175);
                if (dt != null)
                    dt.tamanhoDataGrid(this.ActualHeight - 175);
                if(dp != null)
                    dp.tamanhoDataGrid(this.ActualHeight - 175);
            }
            catch { }
        }


        public void Preenche(string id)
        {

            id_Emp = id;

            var ls = emp.getentidadeDetails(id);

            NomeEntidade.Text = ls.Nome;
            MoradaEntidade.Text = ls.Morada;
            LocalidadeEntidade.Text = ls.Localidade;
            CodPostal_Entidade.Text = ls.CodPostal;
            TelemovelEntidade.Text = ls.Telemovel;
            TelefoneEntidade.Text = ls.Telefone.ToString();
            FaxEntidade.Text = ls.Fax;
            NumeroEntidade.Text = ls.Numero;
            EmailEntidade.Text = ls.Email;
            SiteEntidade.Text = ls.Site;

            if (NomeEntidade.Text.Trim() != "")
            {
                NomeEntidade.BorderBrush = Brushes.Gray;
            }

            int num = projeto.Existe_Projetos(id_Emp);

            if (num > 0)
            {
                tabProjetos.Visibility = System.Windows.Visibility.Visible;
                dt = new Controls.DataGridProjetos(projeto.Projetos_Entidade(id_Emp), PainelCentro);
                tabProjetos.Content = dt;
            }
            else
            {
                TabCentro.Items.Remove(tabProjetos);
            }

            num = people.Existe_Pessoas_Entidade(id_Emp);

            if (num > 0)
            {
                tabPessoas.Visibility = System.Windows.Visibility.Visible;
                dp = new Controls.DataGridPessoas(people.Pessoas_Entidade(id_Emp), PainelCentro);
                tabPessoas.Content = dp;

            }
            else
            {
                TabCentro.Items.Remove(tabPessoas);
            }

            num = databases.Existe_Databases_Entidade(id_Emp);

            if (num > 0)
            {
                tabDatabases.Visibility = System.Windows.Visibility.Visible;
                dgbd = new Controls.DataGridDatabases(databases.Databases_Entidade(id_Emp), PainelCentro);
                tabDatabases.Content = dgbd;
            }
            else
            {
                TabCentro.Items.Remove(tabDatabases);
            }

            num = conhecimento.Existe_Conhecimento_Entidade(id_Emp);

            if (num > 0)
            {
                tabConhecimento.Visibility = System.Windows.Visibility.Visible;
                dgc = new Controls.DataGridConhecimento(conhecimento.getConhecimentoEntidade(id_Emp), PainelCentro);
                tabConhecimento.Content = dgc;
            }
            else
            {
                TabCentro.Items.Remove(tabConhecimento);
            }

            if (NomeEntidade.Text.Trim() != "")
                buttonsDetails.AlterDataButtonSave(1);

        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(1, true);
        }


        protected void DeleteEntidade(object sender, EventArgs e)
        {

            if (id_Emp.Trim() != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar a entidade?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = emp.Delete_Entidade(id_Emp);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A entidade foi removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover a entidade!";

                        System.Windows.Forms.MessageBox.Show(warning,
                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        PainelCentro.Escolhe_Painel(1, false);

                        break;
                    default:
                        break;

                }
            }
            else
            {
                PainelCentro.Escolhe_Painel(1, false);
            }

        }


        protected void Add_Entidade(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            bool done = false;
            string msg = "";
            if (NomeEntidade.Text.Trim() == "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o Nome!",
                            "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    ls.Add("");
                    ls.Add(NomeEntidade.Text);
                    ls.Add(MoradaEntidade.Text);
                    ls.Add(LocalidadeEntidade.Text);
                    ls.Add(CodPostal_Entidade.Text);
                    ls.Add(TelemovelEntidade.Text);
                    ls.Add(TelefoneEntidade.Text);
                    ls.Add(FaxEntidade.Text);
                    ls.Add(NumeroEntidade.Text);
                    ls.Add(EmailEntidade.Text);
                    ls.Add(SiteEntidade.Text);
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");


                    Entidade entidade = new Entidade();

                    if (id_Emp.Trim() != "")
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                         "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:

                                done = entidade.UpdateEntidade(id_Emp, ls);

                                if (done == true)
                                    msg = "Editada com sucesso!";
                                else
                                    msg = "Erro ao fazer update!";

                                break;
                            default:
                                break;

                        }
                    }
                    else
                    {
                        done = entidade.InsertEntidade(ls);

                        if (done == true)
                            msg = "Guardada com sucesso!";
                        else
                            msg = "Erro ao gravar a entidade!\nVerifique se já existe essa entidade.";
                    }
                }
                catch
                {
                    done = false;
                    msg = "Erro ao gravar a entidade!\nVerifique se já existe essa entidade.";
                }

                System.Windows.Forms.MessageBox.Show(msg,
                "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                if (done == true && id_Emp.Trim() == "")
                {
                    NomeEntidade.Text = "";
                    MoradaEntidade.Text = "";
                    LocalidadeEntidade.Text = "";
                    CodPostal_Entidade.Text = "";
                    TelemovelEntidade.Text = "";
                    TelefoneEntidade.Text = "";
                    FaxEntidade.Text = "";
                    NumeroEntidade.Text = "";
                    EmailEntidade.Text = "";
                    SiteEntidade.Text = "";
                    buttonsDetails.AlterDataButtonSave(-1);
                    NomeEntidade.BorderBrush = Brushes.Red;

                }
            }
          
        }

        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetalhesEntidade.IsSelected == true)
            {
                if (buttonsDetails == null)
                    buttonsDetails = new Controls.ButtonsDetails();

                buttonsDetails.SaveClick += new EventHandler(Add_Entidade);
                buttonsDetails.HomeClick += new EventHandler(Home);
                buttonsDetails.DeleteClick += new EventHandler(DeleteEntidade);

                gridFundo.Children.Remove(buttonsGeral);
                gridFundo.Children.Add(buttonsDetails);

                actualizaButtons = true;
                
            }else
            {
                 if (actualizaButtons == true)
                {
                    if (buttonsGeral == null)
                        buttonsGeral = new Controls.ButtonsGeral();

                    gridFundo.Children.Remove(buttonsDetails);
                    gridFundo.Children.Add(buttonsGeral);

                    actualizaButtons = false;
                }

                 if (tabPessoas.IsSelected == true)
                 {
                     buttonsGeral.HomeClick += new EventHandler(Home);
                     buttonsGeral.DeleteClick += new EventHandler(DeletePessoa);
                     buttonsGeral.EditClick += new EventHandler(EditPessoa);
                     buttonsGeral.AddClick += new EventHandler(AddNewPessoa);
                     buttonsGeral.RefreshClick += new EventHandler(RefreshPessoa);
                 }
                 else if (tabProjetos.IsSelected == true)
                 {
                     buttonsGeral.HomeClick += new EventHandler(Home);
                     buttonsGeral.DeleteClick += new EventHandler(DeleteProjeto);
                     buttonsGeral.EditClick += new EventHandler(EditProjeto);
                     buttonsGeral.AddClick += new EventHandler(AddNewProjeto);
                     buttonsGeral.RefreshClick += new EventHandler(RefreshProjeto);
                 }
                 else if (tabDatabases.IsSelected == true)
                 {
                     buttonsGeral.HomeClick += new EventHandler(Home);
                     buttonsGeral.DeleteClick += new EventHandler(DeleteBD);
                     buttonsGeral.EditClick += new EventHandler(EditBD);
                     buttonsGeral.AddClick += new EventHandler(AddNewBD);
                     buttonsGeral.RefreshClick += new EventHandler(RefreshBD);
                 }
                 else if (tabConhecimento.IsSelected == true)
                 {
                     buttonsGeral.HomeClick += new EventHandler(Home);
                     buttonsGeral.DeleteClick += new EventHandler(DeleteConhecimento);
                     buttonsGeral.EditClick += new EventHandler(EditConhecimento);
                     buttonsGeral.AddClick += new EventHandler(AddNewConhecimento);
                     buttonsGeral.RefreshClick += new EventHandler(Refresh);
                 }

            }
            
           
        }

        protected void RefreshProjeto(object sender, EventArgs e)
        {
            dt.ActualizaItemSource(projeto.Projetos_Entidade(id_Emp));
        }


        protected void DeleteProjeto(object sender, EventArgs e)
        {
            dt.DeleteProjeto();
        }

        protected void EditProjeto(object sender, EventArgs e)
        {
            dt.EditProjeto();
        }

        protected void AddNewProjeto(object sender, EventArgs e)
        {
            PainelCentro.id_projeto = "";
            PainelCentro.Escolhe_Painel(6, false);
        }

        protected void RefreshPessoa(object sender, EventArgs e)
        {
           dp.ActualizaItemSource(people.Pessoas_Entidade(id_Emp));
        }


        protected void DeletePessoa(object sender, EventArgs e)
        {
            dp.DeletePessoa();
        }

        protected void EditPessoa(object sender, EventArgs e)
        {
            dp.EditPessoa();
        }

        protected void AddNewPessoa(object sender, EventArgs e)
        {
            PainelCentro.bi = "";
            PainelCentro.Escolhe_Painel(5, false);
        }

        protected void RefreshBD(object sender, EventArgs e)
        {
            dgbd.ActualizaItemSource(databases.BD_Projetos(id_Emp));
        }


        protected void DeleteBD(object sender, EventArgs e)
        {
            dgbd.DeleteBD();
        }

        protected void EditBD(object sender, EventArgs e)
        {
            dgbd.EditBD();
        }

        protected void AddNewBD(object sender, EventArgs e)
        {
            PainelCentro.id_bd = "";
            PainelCentro.Escolhe_Painel(8, false);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            dgc.ActualizaItemSource(conhecimento.getConhecimentoEntidade(id_Emp));
        }


        protected void DeleteConhecimento(object sender, EventArgs e)
        {
            dgc.DeleteConhecimento();
        }

        protected void EditConhecimento(object sender, EventArgs e)
        {
            dgc.EditConhecimento();
        }

        protected void AddNewConhecimento(object sender, EventArgs e)
        {
            PainelCentro.id_conhecimento = "";
            PainelCentro.Escolhe_Painel(10, false);
        }


        private void NomeEntidade_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (NomeEntidade.Text.Trim() != "")
            {
                buttonsDetails.AlterDataButtonSave(1);
                NomeEntidade.BorderBrush = Brushes.Gray;
            }
            else
            {
                buttonsDetails.AlterDataButtonSave(-1);
                NomeEntidade.BorderBrush = Brushes.Red;
            }
        }

        private void NomeEntidade_KeyUp(object sender, KeyEventArgs e)
        {
            if (NomeEntidade.Text.Trim() == "")
            {
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else
            {
                buttonsDetails.AlterDataButtonSave(1);
            }
        }

        


    }
}
