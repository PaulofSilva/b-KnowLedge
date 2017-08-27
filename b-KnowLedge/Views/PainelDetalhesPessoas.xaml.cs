using b_KnowLedge.ViewModels;
using b_KnowLedge.Models;
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
using Microsoft.Win32;
using System.IO;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDetalhesPessoas.xaml
    /// </summary>
    public partial class PainelDetalhesPessoas : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        public string bi = "";
        private Pessoa pessoas = new Pessoa();
        private ViewModels.Entidade entidade = new Entidade();
        List<Entidades> e2;
        string id_Entidades = "";
        string path_Image = " ";

        Controls.ButtonsDetails buttonsDetails;

        public PainelDetalhesPessoas(UserControlCentro controlCentro)
        {
            InitializeComponent();
            PainelCentro = controlCentro;
            Loaded += OnLoaded;

            buttonsDetails = new Controls.ButtonsDetails();
            gridFundo.Children.Add(buttonsDetails);

            buttonsDetails.SaveClick += new EventHandler(Add_Pessoa);
            buttonsDetails.HomeClick += new EventHandler(Home);
            buttonsDetails.DeleteClick += new EventHandler(DeletePessoa);

            NomePessoa.BorderBrush = Brushes.Red;
            AutoComplete1.BorderBrush = Brushes.Red;
            tabConhecimento.Visibility = System.Windows.Visibility.Hidden;
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
            }
            catch { }
        }


        public void Preenche(string id)
        {
            ViewModels.Pessoa pessoa = new ViewModels.Pessoa();
                        
            bi = id;

            var ls = pessoa.getPessoasDetails(bi);
            string [] nomeEntidade = entidade.NomeEntidade(ls.StampEntidades);

            NomePessoa.Text = ls.Nome;
            MoradaPessoa.Text = ls.Morada;
            LocalidadePessoa.Text = ls.Localidade;
            AutoComplete1.Text = nomeEntidade[0];
            CodPostalPessoa.Text = ls.CodPostal;
            TelemovelPessoa.Text = ls.Telemovel;
            TelefonePessoa.Text = ls.Telefone;
            FaxPessoa.Text = ls.Fax;
            NumeroPessoa.Text = ls.Numero;
            EmailPessoa.Text = ls.Email;
            SitePessoa.Text = ls.Site;

            if (ls.Foto != null)
            {
                byte[] blob = ls.Foto;
                MemoryStream stream = new MemoryStream();
                stream.Write(blob, 0, blob.Length);
                stream.Position = 0;

                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage foto = new BitmapImage();
                foto.BeginInit();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                foto.StreamSource = ms;
                foto.EndInit();
                ImagePessoa.Source = foto;
            }

            if (NomePessoa.Text.Trim() != "")
            {
                NomePessoa.BorderBrush = Brushes.Gray;
            }

            if (AutoComplete1.Text.Trim() != "")
            {
                AutoComplete1.BorderBrush = Brushes.Gray;
            }

            if (NomePessoa.Text.Trim() != "" && AutoComplete1.Text.Trim() != "")
                buttonsDetails.AlterDataButtonSave(1);

        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            e2 = entidade.GetEntidade();
            string[] name2 = new string[e2.Count];
            int i = 0;

            foreach (Entidades fl in e2)
            {
                name2[i] = fl.Nome;
                i++;
            }

            AutoComplete1.ItemsSource = name2;
        }


        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            VerificaProjetos();


            if (id_Entidades == "")
            {
                AutoComplete1.BorderBrush = Brushes.Red;
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else
            {
                AutoComplete1.BorderBrush = Brushes.Gray;

                if (NomePessoa.Text.Trim() != "")
                    buttonsDetails.AlterDataButtonSave(1);
            }
        }


        private void VerificaProjetos()
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

                id_Entidades = query.StampEntidade;

                AutoComplete1.BorderBrush = Brushes.Gray;

                if(NomePessoa.Text.Trim()!="")
                    buttonsDetails.AlterDataButtonSave(1);

            }
            catch
            {
                id_Entidades = "";
                AutoComplete1.BorderBrush = Brushes.Red;
                buttonsDetails.AlterDataButtonSave(-1);
            }

        }


        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            if (PainelCentro.backward == true)
                PainelCentro.Escolhe_Painel(-1, true);
            else
                PainelCentro.Escolhe_Painel(3, true);
        }


        private void openFileClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.PNG";
            dlg.Title = "Carregue a Imagem";
            dlg.Multiselect = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.RestoreDirectory = true;
            dlg.ReadOnlyChecked = true;
            dlg.ShowReadOnly = true;
            string currentFileName;

            if (dlg.ShowDialog() ?? false)
            {
                currentFileName = dlg.FileName;
                path_Image = currentFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                bitmap.UriSource = new Uri(currentFileName);
                bitmap.EndInit();
                ImagePessoa.Source = bitmap;                
            }
        }

        protected void DeletePessoa(object sender, EventArgs e)
        {
            if (bi.Trim() != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar a pessoa?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = pessoas.Delete_Pessoa(bi);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "A pessoa foi removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover a pessoa!";

                        System.Windows.Forms.MessageBox.Show(warning,
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        PainelCentro.Escolhe_Painel(3, false);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                PainelCentro.Escolhe_Painel(3, false);
            }

        }


        protected void Add_Pessoa(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            bool done = true, window = true;
            string msg = "";

            VerificaProjetos();


            if (NomePessoa.Text.Trim() == "" || AutoComplete1.Text.Trim() == "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o Nome e a Entidade!",
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {

                try
                {
                    if (id_Entidades == "")
                    {
                        System.Windows.Forms.MessageBox.Show("Corrija os campos a vermelho!",
                           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    }
                    else
                    {
                        ls.Add("");
                        ls.Add(id_Entidades.ToString());
                        ls.Add(NomePessoa.Text);
                        ls.Add(MoradaPessoa.Text);
                        ls.Add(LocalidadePessoa.Text);
                        ls.Add(CodPostalPessoa.Text);
                        ls.Add(TelemovelPessoa.Text);
                        ls.Add(TelefonePessoa.Text);
                        ls.Add(FaxPessoa.Text);
                        ls.Add(NumeroPessoa.Text);
                        ls.Add(EmailPessoa.Text);
                        ls.Add(SitePessoa.Text);
                        

                        if (path_Image != " ")
                            ls.Add(path_Image);

                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");
                        ls.Add("");

                    }
                }
                catch
                {
                    done = false;
                }

                if (done == true)
                {
                    if (bi.Trim() != "")
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                         "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:

                                done = pessoas.UpdatePessoa(bi, ls);

                                if (done == true)
                                    msg = "Editada com sucesso!";
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
                        done = pessoas.InsertPessoa(ls);

                        if (done == true)
                            msg = "Guardada com sucesso!";
                        else
                            msg = "Erro ao gravar a pessoa!";
                    }
                }
                else
                {
                    msg = "Erro ao gravar a pessoa!";
                }

                if (window == true)
                {
                    System.Windows.Forms.MessageBox.Show(msg,
                    "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                if (done == true && bi.Trim() == "")
                {
                    NomePessoa.Text = "";
                    MoradaPessoa.Text = "";
                    LocalidadePessoa.Text = "";
                    AutoComplete1.Text = "";
                    CodPostalPessoa.Text = "";
                    TelemovelPessoa.Text = "";
                    TelefonePessoa.Text = "";
                    FaxPessoa.Text = "";
                    NumeroPessoa.Text = "";
                    EmailPessoa.Text = "";
                    SitePessoa.Text = "";
                    NomePessoa.BorderBrush = Brushes.Red;
                    AutoComplete1.BorderBrush = Brushes.Red;
                    ImagePessoa.Source = null;
                    buttonsDetails.AlterDataButtonSave(-1);
                }
            }
        }


        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (DetalhesPessoa.IsSelected == true)
            {
                gridFundo.Children.Add(bdp);
            }
            else if (tabConhecimento.IsSelected == true)
            {

            }*/

        }

        private void NomePessoa_LostFocus_1(object sender, RoutedEventArgs e)
        {

            if (NomePessoa.Text.Trim() == "")
            {
                NomePessoa.BorderBrush = Brushes.Red;
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else
            {
                NomePessoa.BorderBrush = Brushes.Gray;

                if (AutoComplete1.Text.Trim() != "")
                    buttonsDetails.AlterDataButtonSave(1);
            }
            
        }

        private void NomePessoa_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (NomePessoa.Text.Trim() == "")
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
            this.VerificaProjetos();

            if (id_Entidades == "")
            {
                buttonsDetails.AlterDataButtonSave(-1);
            }
            else if(NomePessoa.Text.Trim()!="")
            {
                buttonsDetails.AlterDataButtonSave(1);
            }
        }


    }
}
