using b_KnowLedge.Models;
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
using System.Linq;
using System.Data.Linq;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDetalhesProjetos.xaml
    /// </summary>
    public partial class PainelDetalhesProjetos : UserControl
    {
        Window parentWindow;
        UserControlCentro PainelCentro;
        public string id_Projeto = "";
        private Entidade entidades = new Entidade();
        private ViewModels.Projeto projeto;
        List<Entidades> e2;
        List<Utilizadores> user2;
        string id_empre = "";
        bool retroceder = false;
        ViewModels.Pessoa people = new Pessoa();
        ViewModels.BasesdeDados databases = new BasesdeDados();
        ViewModels.Conhecimento conhecimento = new Conhecimento();
        ViewModels.Utilizador user = new Utilizador();
        Controls.DataGridPessoas dp;
        Controls.ButtonsDetails buttonsDetails;
        Controls.ButtonsGeral buttonsGeral;
        Controls.DataGridDatabases dgbd;
        Controls.DataGridConhecimento dgc;
        bool actualizaButton = false, addButtons = true;

        public PainelDetalhesProjetos(UserControlCentro controlCentro, bool back)
        {
            InitializeComponent();
            retroceder = back;
            PainelCentro = controlCentro;
            projeto = new Projeto();
            Loaded += OnLoaded;
            PainelCentro.backward = true;

            buttonsDetails = new Controls.ButtonsDetails();

            tabConhecimento.Visibility = System.Windows.Visibility.Hidden;
            tabDatabases.Visibility = System.Windows.Visibility.Hidden;

            DetalhesProjeto.IsSelected = true;

            NomeProjeto.BorderBrush = Brushes.Red;
            AutoComplete1.BorderBrush = Brushes.Red;
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
                if (dgbd != null)
                    dgbd.tamanhoDataGrid(this.ActualHeight - 175);
                if (dgc != null)
                    dgc.tamanhoDataGrid(this.ActualHeight - 175);
                if (dp != null)
                    dp.tamanhoDataGrid(this.ActualHeight - 175);

            }
            catch { }
        }


        public void Preenche(string id)
        {
            ViewModels.Projeto projeto = new ViewModels.Projeto();

            id_Projeto = id;

            var ls = projeto.getProjetosDetails(id);

            NomeProjeto.Text = ls.Nome;
            MoradaProjeto.Text = ls.Morada;
            LocalidadeProjeto.Text = ls.Localidade;
            CodPostalProjeto.Text = ls.CodPostal;
            TelemovelProjeto.Text = ls.Telemovel;
            TelefoneProjeto.Text = ls.Telefone;
            FaxProjeto.Text = ls.Fax;
            EmailProjeto.Text = ls.Email;
            SiteProjeto.Text = ls.Site;
            DescricaoProjeto.Text = ls.Descricacao;
            AutoComplete2.Text = ls.NomeConsultor;

            Entidade e2 = new Entidade();
            string[] name = e2.NomeEntidade(ls.StampEntidade);
            AutoComplete1.Text = name[0];

            int num = databases.Existe_Databases_Projetos(id_Projeto);

            if (num > 0)
            {
                tabDatabases.Visibility = System.Windows.Visibility.Visible;
                dgbd = new Controls.DataGridDatabases(databases.BD_Projetos(id_Projeto), PainelCentro);
                tabDatabases.Content = dgbd;
            }
            else
            {
                TabCentro.Items.Remove(tabDatabases);
            }

            num = conhecimento.Existe_Conhecimento_Projeto(id_Projeto);

            if (num > 0)
            {
                tabConhecimento.Visibility = System.Windows.Visibility.Visible;
                dgc = new Controls.DataGridConhecimento(conhecimento.getConhecimentoProjeto(id_Projeto), PainelCentro);
                tabConhecimento.Content = dgc;
            }
            else
            {
                TabCentro.Items.Remove(tabConhecimento);
            }

            if (AutoComplete1.Text.Trim() != "")
            {
                AutoComplete1.BorderBrush = Brushes.Gray;
            }

            if (NomeProjeto.Text.Trim() != "")
            {
                NomeProjeto.BorderBrush = Brushes.Gray;
            }

            if (AutoComplete1.Text.Trim() != "" && NomeProjeto.Text.Trim() != "")
                buttonsDetails.AlterDataButtonSave(1);

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            e2 = entidades.GetEntidade();
            string[] name2 = new string[e2.Count];
            int i = 0;

            foreach (Entidades em in e2)
            {
                name2[i] = em.Nome;
                i++;
            }

            AutoComplete1.ItemsSource = name2;

            user2 = user.GetUtilizador();
            name2 = null;
            name2 = new string[user2.Count];
            i = 0;

            foreach (Utilizadores u in user2)
            {
                name2[i] = u.Nome;
                i++;
            }
            AutoComplete2.ItemsSource = name2;           
        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            if (PainelCentro.backward == true)
                PainelCentro.Escolhe_Painel(-1, true);
            else
                PainelCentro.Escolhe_Painel(2, true);
        }


        protected void DeleteProjeto(object sender, EventArgs e)
        {
            if (id_Projeto.Trim() != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar o projeto?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = projeto.Delete_Projeto(id_Projeto);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A projeto foi removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover o projeto!";

                        System.Windows.Forms.MessageBox.Show(warning,
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        PainelCentro.Escolhe_Painel(2, false);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                PainelCentro.Escolhe_Painel(2, false);
            }

        }

        protected void Add_Projeto(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            bool done = true, window = true;
            string msg = "";


            VerificaEntidade();

           
          
                if (NomeProjeto.Text.Trim() == "" || AutoComplete1.Text.Trim() == "")
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o Nome e a Entidade!",
                                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        if (id_empre == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Corrija os campos a vermelho!",
                               "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                        }
                        else
                        {
                            ls.Add("");
                            ls.Add(id_empre.ToString());
                            ls.Add(NomeProjeto.Text);
                            ls.Add(MoradaProjeto.Text);
                            ls.Add(LocalidadeProjeto.Text);
                            ls.Add(CodPostalProjeto.Text);
                            ls.Add(TelemovelProjeto.Text);
                            ls.Add(TelefoneProjeto.Text);
                            ls.Add(FaxProjeto.Text);
                            ls.Add(EmailProjeto.Text);
                            ls.Add(SiteProjeto.Text);
                            ls.Add(DescricaoProjeto.Text);
                            ls.Add("");
                            ls.Add("");
                            ls.Add("");
                            ls.Add("");
                            ls.Add("");
                            ls.Add("");
                            ls.Add(AutoComplete2.Text);
                            ls.Add("");
                        }
                    }
                    catch
                    {
                        done = false;
                    }

                    if (done == true)
                    {
                        if (id_Projeto.Trim() != "")
                        {
                            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                             "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                            switch (result)
                            {
                                case System.Windows.Forms.DialogResult.Yes:

                                    done = projeto.UpdateProjetos(id_Projeto, ls);

                                    if (done == true)
                                        msg = "Editado com sucesso!";
                                    else
                                        msg = "Erro ao fazer update!";

                                    break;
                                default:
                                    window = false;
                                    break;

                            }
                        }
                        else
                        {
                            bool exist = projeto.ProjectExist(NomeProjeto.Text, id_empre);

                            if (exist == true)
                            {
                                done = false;
                                window = false;
                                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Já existe um projeto com essa denominação!",
                                                "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            }
                            else
                            {
                                done = projeto.InsertProjeto(ls);
                                if (done == true)
                                    msg = "Guardado com sucesso!";
                                else
                                    msg = "Erro ao gravar o Projeto!\nVerifique se já existe esse projeto.";
                            }                           
                        }
                    }
                    else
                    {
                        msg = "Erro ao gravar o projeto!\nVerifique se já existe esse projeto.";
                    }

                    if (window == true)
                    {
                        System.Windows.Forms.MessageBox.Show(msg,
                        "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }

                    if (done == true && id_Projeto.Trim() == "")
                    {
                        AutoComplete1.Text = "";
                        NomeProjeto.Text = "";
                        MoradaProjeto.Text = "";
                        LocalidadeProjeto.Text = "";
                        CodPostalProjeto.Text = "";
                        TelemovelProjeto.Text = "";
                        TelefoneProjeto.Text = "";
                        FaxProjeto.Text = "";
                        EmailProjeto.Text = "";
                        SiteProjeto.Text = "";
                        DescricaoProjeto.Text = "";
                        NomeProjeto.BorderBrush = Brushes.Red;
                        AutoComplete1.BorderBrush = Brushes.Red;
                        buttonsDetails.AlterDataButtonSave(-1);

                    }
                
            }
        }


        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            VerificaEntidade();
            if (AutoComplete1.Text.Trim() == "")
            {
                AutoComplete1.BorderBrush = Brushes.Red;
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else if (NomeProjeto.Text.Trim() != "")
            {
                string nr = entidades.getIDFromNomeEntidade(AutoComplete1.Text);

                if (nr.Trim() != "")
                {
                    buttonsDetails.AlterDataButtonSave(1);
                    AutoComplete1.BorderBrush = Brushes.Gray;
                }
                else
                    buttonsDetails.AlterDataButtonSave(-1);
            }
        }


        private void VerificaEntidade()
        {
            string name = "";

            try
            {
                try
                {
                    var at = (AutoComplete1.SelectedItem).ToString();
                    name = at.ToString();
                }
                catch
                {
                    name = AutoComplete1.Text;
                }


                var query = e2.Single(s => s.Nome == name || s.Nome.ToLower() == name.ToLower() || s.Nome.ToUpper() == name.ToUpper());

                id_empre = query.StampEntidade;

                AutoComplete1.BorderBrush = Brushes.Gray;

            }
            catch
            {
                id_empre = "";
                AutoComplete1.BorderBrush = Brushes.Red;
            }

        }



        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DetalhesProjeto.IsSelected == true)
            {
                if (addButtons == true)
                {
                    if (buttonsDetails == null)
                        buttonsDetails = new Controls.ButtonsDetails();

                    buttonsDetails.SaveClick += new EventHandler(Add_Projeto);
                    buttonsDetails.HomeClick += new EventHandler(Home);
                    buttonsDetails.DeleteClick += new EventHandler(DeleteProjeto);

                    gridFundo.Children.Remove(buttonsGeral);
                    gridFundo.Children.Add(buttonsDetails);

                    actualizaButton = true;
                    addButtons = false;
                }

            }
            else
            {
                if (actualizaButton == true)
                {
                    if (buttonsGeral == null)
                        buttonsGeral = new Controls.ButtonsGeral();

                    gridFundo.Children.Remove(buttonsDetails);
                    gridFundo.Children.Add(buttonsGeral);
                    addButtons = true;
                    actualizaButton = false;
                }

                if (tabDatabases.IsSelected == true)
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

        protected void RefreshBD(object sender, EventArgs e)
        {
            dgbd.ActualizaItemSource(databases.BD_Projetos(id_Projeto));
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
            dgc.ActualizaItemSource(conhecimento.getConhecimentoProjeto(id_Projeto));
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

        private void NomeProjeto_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (NomeProjeto.Text.Trim() == "")
            {
                NomeProjeto.BorderBrush = Brushes.Red;
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else
            {
                NomeProjeto.BorderBrush = Brushes.Gray;

                if (AutoComplete1.Text.Trim() != "")
                    buttonsDetails.AlterDataButtonSave(1);
            }
        }

        private void AutoComplete1_LostFocus_2(object sender, RoutedEventArgs e)
        {
            if (AutoComplete1.Text.Trim() == "")
            {
                AutoComplete1.BorderBrush = Brushes.Red;
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else if (NomeProjeto.Text.Trim() != "")
            {
                string nr = entidades.getIDFromNomeEntidade(AutoComplete1.Text);

                if (nr.Trim() != "")
                {
                    buttonsDetails.AlterDataButtonSave(1);
                    AutoComplete1.BorderBrush = Brushes.Gray;
                }
                else
                    buttonsDetails.AlterDataButtonSave(-1);
            }
        }

        private void NomeProjeto_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (NomeProjeto.Text.Trim() == "")
            {
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else if (AutoComplete1.Text.Trim() != "")
            {
                buttonsDetails.AlterDataButtonSave(1);
            }
        }

        private void AutoComplete1_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (AutoComplete1.Text.Trim() == "")
            {
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else if (NomeProjeto.Text.Trim() != "")
            {
                string nr = entidades.getIDFromNomeEntidade(AutoComplete1.Text);

                if (nr.Trim() != "")
                    buttonsDetails.AlterDataButtonSave(1);
                else
                    buttonsDetails.AlterDataButtonSave(-1);
            }
        }
    }
}