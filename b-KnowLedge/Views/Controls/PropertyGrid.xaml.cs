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
using b_KnowLedge.Models;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Win32;


namespace b_KnowLedge.Views.Controls
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    /// 

    public partial class PropertyGrid : UserControl
    {
        ViewModels.Projeto projeto = new ViewModels.Projeto();
        ViewModels.Tipo tipo = new ViewModels.Tipo();
        ViewModels.Subtipo subtipo = new ViewModels.Subtipo();
        ViewModels.Entidade entidade = new ViewModels.Entidade();
        ViewModels.Utilizador user = new ViewModels.Utilizador();
        ViewModels.Conhecimento conhecimento = new ViewModels.Conhecimento();
        ViewModels.Anexo anexo = new ViewModels.Anexo();
        Window parentWindow;
        List<String> ComboListProjetos = new List<string>();
        List<String> ComboListTipo = new List<string>();
        List<String> ComboListSubtipo = new List<string>();
        List<String> ComboListEntidades = new List<string>();
        List<string> files = new List<string>();
        List<Anexos> anexos = new List<Anexos>();
        PainelDetalhesConhecimento painelDetalhesConhecimento;
        string id_con = "";
        int nivelAprovacao = 0;
        private string idNentidade = " ", idNprojeto = " ", idtipo = " ", idsubtipo = " ";
        double tamanho_Gridgeral, tamanho_GridFiles, tamanho_GridDetalhes, tamanho_GridAdicional;

        public PropertyGrid(PainelDetalhesConhecimento pdc)
        {
            InitializeComponent();
            painelDetalhesConhecimento = pdc;
            this.DataContext = this;
            this.ResetCombos();
        }


        private void ResetCombos()
        {
            try
            {
                lock (ComboListProjetos)
                {
                    ComboListProjetos.Clear();
                }

                var a = projeto.getProjetos();
                ComboListProjetos.Add(" ");
                foreach (Projetos f in a)
                {
                    ComboListProjetos.Add(f.Nome);
                }

                lock (ComboListTipo)
                {
                    ComboListTipo.Clear();
                }

                var type = tipo.getTipo();
                ComboListTipo.Add(" ");
                foreach (Tipos t in type)
                {
                    ComboListTipo.Add(t.Nome);
                }

                lock (ComboListSubtipo)
                {
                    ComboListSubtipo.Clear();
                }

                var subtype = subtipo.getSubtipo();
                ComboListSubtipo.Add(" ");
                foreach (Subtipos st in subtype)
                {
                    ComboListSubtipo.Add(st.Nome);
                }

                lock (ComboListEntidades)
                {
                    ComboListEntidades.Clear();
                }

                var empr = entidade.GetEntidade();
                ComboListEntidades.Add(" ");
                foreach (Entidades e in empr)
                {
                    ComboListEntidades.Add(e.Nome);
                }

                AutoComplete1.ItemsSource = ComboListEntidades;
                combo_projetos.ItemsSource = ComboListProjetos;
                combo_tipo.ItemsSource = ComboListTipo;
                combo_subtipo.ItemsSource = ComboListSubtipo;
                combo_projetos.SelectedIndex = 0;
                combo_subtipo.SelectedIndex = 0;
                combo_tipo.SelectedIndex = 0;

                List<string> aux = new List<string>();

                combo_ficheiros.ItemsSource = aux;

                List<int> aprovacao = new List<int>();
                aprovacao.Add(0);
                aprovacao.Add(1);
                aprovacao.Add(3);
                aprovacao.Add(5);
            }
            catch { }
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);

            try
            {
                borderMetadados.Width = textbox_Metadados.Width = DescricaoConhecimento.Width = AutoComplete1.Width = combo_projetos.Width = combo_subtipo.Width = combo_tipo.Width = this.ActualWidth - 100;
                textbox_Ecra.Width = borderTabela.Width = borderEcra.Width = borderTeclas.Width = textbox_Tabela.Width = textbox_Teclas.Width = this.ActualWidth - 93;
                DropBox.Width = MensagemConhecimento.Width = this.ActualWidth - 82;
                combo_ficheiros.MaxWidth = DropBox.Width;
                MensagemConhecimento.Width = this.ActualWidth - 93;
                ballon.MaxWidth = this.ActualWidth - 50;
            }
            catch { }
        }


        public ObservableCollection<string> Files
        {
            get
            {
                return _files;
            }
        }


        private ObservableCollection<string> _files = new ObservableCollection<string>();


        private void DropBox_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //if (_files.Count == 3)
                //    _files.Clear();

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string filePath in files)
                {
                    _files.Add(filePath);
                }

                //UploadFiles(files);
            }

            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }


        public List<string> GuardarConhecimento()
        {

            List<string> con = new List<string>();

            string nr = projeto.getIDFromNomeProjetos(Convert.ToString(combo_projetos.SelectedItem));
            con.Add("");
            con.Add(nr.ToString());

            string IDtype = tipo.getIDFromNomeTipo(Convert.ToString(combo_tipo.SelectedItem));

            nr = subtipo.getIDFromNomeSubtipos(Convert.ToString(combo_subtipo.SelectedItem), IDtype);
            con.Add(nr.ToString());

            nr = tipo.getIDFromNomeTipo(Convert.ToString(combo_tipo.SelectedItem));
            con.Add(nr.ToString());
            con.Add(DescricaoConhecimento.Text);
            con.Add(textbox_Metadados.Text);
            con.Add(textbox_Ecra.Text);
            con.Add(MensagemConhecimento.Text);
            con.Add(textbox_Teclas.Text);
            con.Add(textbox_Tabela.Text);
            con.Add(Convert.ToString(nivelAprovacao));
            con.Add("");
            con.Add("");
            con.Add("");
            con.Add("");
            con.Add("");
            con.Add("");
            con.Add("");
            con.Add(nr.ToString());


            return con;
        }



        public void Preenche_UserControl(Conhecimentos con)
        {
            Projetos fil = null;
            Tipos type = null;
            Subtipos subtype = null;
            Entidades empre = null;

            try
            {
                _files.Clear();
                id_con = con.StampConhecimento;
                ballontext.Text = "Data da ultima actualização: " + con.Usrdata.ToString();

                textbox_Metadados.Text = con.Metadados.ToString();

                if (con.Mensagem != null && con.Mensagem != "")
                    MensagemConhecimento.Text = con.Mensagem.ToString();

                if (con.Ecra != null && con.Ecra != "")
                    textbox_Ecra.Text = con.Ecra.ToString();

                if (con.Tabela != null && con.Tabela != "")
                    textbox_Tabela.Text = con.Tabela.ToString();

                if (con.Teclas != null && con.Teclas != "")
                    textbox_Teclas.Text = con.Teclas.ToString();

                if (con.Descricao != null && con.Descricao != "")
                    DescricaoConhecimento.Text = con.Descricao.ToString();

                if (con.StampProjeto != null)
                {
                    fil = projeto.getProjetosDetails(con.StampProjeto);
                    empre = entidade.getentidadeDetails(fil.StampEntidade);
                    AutoComplete1.Text = empre.Nome;
                    combo_projetos.SelectedValue = fil.Nome;
                }

                if (con.StampTipo != null)
                {
                    type = tipo.getTipoDetails(con.StampTipo);
                    combo_tipo.SelectedValue = type.Nome;
                }

                if (con.StampSubtipo != null)
                {
                    subtype = subtipo.getSubtipoDetails(con.StampSubtipo);
                    combo_subtipo.SelectedValue = subtype.Nome;
                }

                switch (con.NivelAprovacao)
                {
                    case 0:
                        Star1.State = StarState.Off;
                        Star2.State = StarState.Off;
                        Star3.State = StarState.Off;
                        Star4.State = StarState.Off;
                        Star5.State = StarState.Off;
                        break;
                    case 1:
                        Star1.State = StarState.On;
                        Star2.State = StarState.Off;
                        Star3.State = StarState.Off;
                        Star4.State = StarState.Off;
                        Star5.State = StarState.Off;
                        break;
                    case 2:
                        Star1.State = StarState.On;
                        Star2.State = StarState.On;
                        Star3.State = StarState.Off;
                        Star4.State = StarState.Off;
                        Star5.State = StarState.Off;
                        break;
                    case 3:
                        Star1.State = StarState.On;
                        Star2.State = StarState.On;
                        Star3.State = StarState.On;
                        Star4.State = StarState.Off;
                        Star5.State = StarState.Off;
                        break;
                    case 4:
                        Star1.State = StarState.On;
                        Star2.State = StarState.On;
                        Star3.State = StarState.On;
                        Star4.State = StarState.On;
                        Star5.State = StarState.Off;
                        break;
                    case 5:
                        Star1.State = StarState.On;
                        Star2.State = StarState.On;
                        Star3.State = StarState.On;
                        Star4.State = StarState.On;
                        Star5.State = StarState.On;
                        break;
                    default:
                        break;
                }

                Preenche_Combo_Files();
                VerificaButtons();
               
            }
            catch
            {
            }

        }


        private void Preenche_Combo_Files()
        {
            anexos = anexo.Anexos_Conhecimento(id_con);
            lock (combo_ficheiros)
            {
                files.Clear();
                combo_ficheiros.ItemsSource = files;
                combo_ficheiros.SelectedIndex = 0;
            }

            files.Add(" ");

            foreach (Anexos sr in anexos)
            {
                files.Add(sr.NomeFicheiro.ToString());
            }

            combo_ficheiros.ItemsSource = files;
            combo_ficheiros.SelectedIndex = 1;
        }

        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var listbox = sender as ListBox;
                listbox.Background = new SolidColorBrush(Color.FromRgb(155, 155, 155));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }


        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }


        private void openFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "All files (*.*)|*.*";
            dlg.Title = "Seleccione o ficheiro";
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
                _files.Add(dlg.FileName);
            }
        }

        private void downloadFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                anexo.Download_File(id_con, combo_ficheiros.SelectedItem.ToString());
            }
            catch { }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (gridGeral.Height != 0)
            {
                gridGeral.Height = 0;
            }
            else
            {
                gridGeral.Height = tamanho_Gridgeral;
            }
        }

        private void TextBlock_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            if (gridDetalhes.Height != 0)
            {
                gridDetalhes.Height = 0;
            }
            else
            {
                gridDetalhes.Height = tamanho_GridDetalhes;
            }
        }

        private void TextBlock_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            if (gridFicheiros.Height != 0)
            {
                gridFicheiros.Height = 0;
            }
            else
            {
                gridFicheiros.Height = tamanho_GridFiles;
            }
        }


        private void TextBlock_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            if (gridAdicional.Height != 0)
            {
                gridAdicional.Height = 0;
            }
            else
            {
                gridAdicional.Height = tamanho_GridAdicional;
            }
        }


        public string Verify_ComboProjetos()
        {
            string nr ="";


            try
            {
                nr = projeto.getIDFromNomeProjetos(idNprojeto);
            }
            catch
            {
                nr = "";
            }


            return nr;
        }


        public string Verify_ComboEntidade()
        {
            string nr = "";


            try
            {
                nr = entidade.getIDFromNomeEntidade(idNentidade);
            }
            catch
            {
                nr = "";
            }


            return nr;
        }


        public string Verify_ComboSubtipo()
        {
            string nr = "";


            try
            {
                nr = subtipo.getIDFromNomeSubtipos(idsubtipo, idtipo);
            }
            catch
            {
                nr = "";
            }


            return nr;
        }


        public string Verify_ComboTipo()
        {
            string nr = "";


            try
            {
                nr = tipo.getIDFromNomeTipo(idtipo);
            }
            catch
            {
                nr = "";
            }


            return nr;
        }


        private void combo_projetos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string name = (string)combo_projetos.SelectedItem;

            if (name != " ")
            {
                idNprojeto = name;
            }
            else
            {
                idNprojeto = " ";
            }
        }


        private void combo_tipo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string name = (string)combo_tipo.SelectedItem;
            var subtype = subtipo.getSubtipo();

            if (name != " ")
            {
                idtipo = name;

                string id = tipo.getIDFromNomeTipo(idtipo);
                subtype = null;
                subtype = subtipo.Tipo_Subtipo(id);
            }
            else
            {
                idtipo = " ";
            }

            List<string> aux = new List<string>();

            combo_subtipo.ItemsSource = aux;

            lock (ComboListSubtipo)
            {
                ComboListSubtipo.Clear();
            }

            ComboListSubtipo.Add(" ");
            try
            {
                foreach (Subtipos st in subtype)
                {
                    lock (ComboListSubtipo)
                    {
                        ComboListSubtipo.Add(st.Nome);
                    }
                }

                combo_subtipo.ItemsSource = ComboListSubtipo;

            }
            catch
            { }

            VerificaButtons();

        }


        private void combo_subtipo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string name = (string)combo_subtipo.SelectedItem;

            if (name != " ")
            {
                idsubtipo = name;
            }
            else
            {
                idsubtipo = " ";
            }

            VerificaButtons();
        }


        private void deleteFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string ficheiro = combo_ficheiros.SelectedItem.ToString();

                if (ficheiro != " ")
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminiar o ficheiro " + ficheiro + "?",
                           "Aviso!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                    switch (result)
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            anexo.Delete_Anexo(id_con, ficheiro);
                            lock (combo_ficheiros)
                            {
                                Preenche_Combo_Files();
                            }
                            break;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Tem que seleccionar um ficheiro para eliminar!",
                           "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch { }
        }


        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            string name = AutoComplete1.Text;

            if (name != " ")
            {
                idNentidade = name;
                string em = entidade.IDEntidadeByName(name);

                if (em != "")
                {

                    var a = projeto.Projetos_Entidade(em);

                    if (a.Count == 0)
                    {
                        a = projeto.getProjetos();
                    }
                    List<string> aux = new List<string>();

                    combo_projetos.ItemsSource = aux;

                    lock (ComboListProjetos)
                    {
                        ComboListProjetos.Clear();
                    }

                    try
                    {
                        ComboListProjetos.Add(" ");
                        foreach (Projetos f in a)
                        {
                            lock (ComboListProjetos)
                            {
                                ComboListProjetos.Add(f.Nome);
                            }
                        }

                        combo_projetos.ItemsSource = ComboListProjetos;

                    }
                    catch
                    { }

                }


            }
            else
            {
                idNentidade = " ";
            }
        }


        private void Show_PopupToolTip(object sender, MouseEventArgs e)
        {
            ballon.Visibility = System.Windows.Visibility.Visible;
            ballon.Height = ballontext.ActualHeight + 10;
        }


        private void Hide_PopupToolTip(object sender, MouseEventArgs e)
        {
            ballon.Visibility = System.Windows.Visibility.Hidden;
        }


        private void Show_PopupToolTipTipo(object sender, MouseEventArgs e)
        {
            ballon.Visibility = System.Windows.Visibility.Visible;
        }


        private void Hide_PopupToolTipTipo(object sender, MouseEventArgs e)
        {
            ballon.Visibility = System.Windows.Visibility.Hidden;
        }


        private void Show_PopupToolTipSubTipo(object sender, MouseEventArgs e)
        {
            ballon.Visibility = System.Windows.Visibility.Visible;
        }


        private void Hide_PopupToolTipSubTipo(object sender, MouseEventArgs e)
        {
            ballon.Visibility = System.Windows.Visibility.Hidden;
        }


        public void VerificaButtons()
        {
            int conta = 0;

            if (textbox_Metadados.Text.Trim() != "")
            {
                conta++;
            }

            string name = (string)combo_tipo.SelectedItem;

            if (name != " " && name != null)
            {
                conta++;
            }

            name = null;
            name = (string)combo_subtipo.SelectedItem;

            if (name != " " && name != null)
            {
                conta++;
            }

            painelDetalhesConhecimento.VerifyButton(conta);

        }

        private void textbox_Metadados_KeyUp_1(object sender, KeyEventArgs e)
        {
            VerificaButtons();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            tamanho_Gridgeral = gridGeral.ActualHeight;
            tamanho_GridFiles = gridFicheiros.ActualHeight;
            tamanho_GridDetalhes = gridDetalhes.ActualHeight;
            tamanho_GridAdicional = gridAdicional.ActualHeight;
        }

        public void LimpaCampos()
        {
            DescricaoConhecimento.Text = "";
            textbox_Metadados.Text = "";
            textbox_Tabela.Text = "";
            textbox_Teclas.Text = "";
            textbox_Ecra.Text = "";
            AutoComplete1.Text = "";
            MensagemConhecimento.Text = "";
            Star1.State = StarState.Off;
            Star2.State = StarState.Off;
            Star3.State = StarState.Off;
            Star4.State = StarState.Off;
            Star5.State = StarState.Off;
            nivelAprovacao = 0;
            ballontext.Text = "";
            _files.Clear();
            ResetCombos();

        }

        private void Star_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            nivelAprovacao = 1;
            Star1.State = StarState.On;
            Star2.State = StarState.Off;
            Star3.State = StarState.Off;
            Star4.State = StarState.Off;
            Star5.State = StarState.Off;
        }

        private void Star2_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            nivelAprovacao = 2;

            Star1.State = StarState.On;
            Star2.State = StarState.On;
            Star3.State = StarState.Off;
            Star4.State = StarState.Off;
            Star5.State = StarState.Off;

        }

        private void Star3_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {
            nivelAprovacao = 3;
            Star1.State = StarState.On;
            Star2.State = StarState.On;
            Star3.State = StarState.On;
            Star4.State = StarState.Off;
            Star5.State = StarState.Off;
        }

        private void Star4_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e)
        {
            nivelAprovacao = 4;
            Star1.State = StarState.On;
            Star2.State = StarState.On;
            Star3.State = StarState.On;
            Star4.State = StarState.On;
            Star5.State = StarState.Off;

        }

        private void Star5_MouseLeftButtonUp_5(object sender, MouseButtonEventArgs e)
        {
            nivelAprovacao = 5;
            Star1.State = StarState.On;
            Star2.State = StarState.On;
            Star3.State = StarState.On;
            Star4.State = StarState.On;
            Star5.State = StarState.On;

        }



    }
}
