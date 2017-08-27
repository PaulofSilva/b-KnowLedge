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
using b_KnowLedge.ViewModels;
using System.Windows.Threading;
using Microsoft.Win32;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using System.Data;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDetalhesTabelas.xaml
    /// </summary>
    public partial class PainelDetalhesTabelas : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.ButtonsDetails buttonsDetails;
        Tabela tabela;
        string id_tab = "";
        string id_tip = "";
        Subtipo subtipo = new Subtipo();
        Tipo tipo = new Tipo();
        List<String> ComboListSubtipo = new List<string>();
        List<String> ListTipo = new List<string>();
        List<Tipos> types = new List<Tipos>();
        IEnumerable<object> listaDatagrid;

        public PainelDetalhesTabelas(UserControlCentro ucc, bool back)
        {
            InitializeComponent();
            PainelCentro = ucc;
            tabela = new Tabela();

            buttonsDetails = new Controls.ButtonsDetails();
            gridFundo.Children.Add(buttonsDetails);

            buttonsDetails.SaveClick += new EventHandler(Add_Tabela);
            buttonsDetails.HomeClick += new EventHandler(Home);
            buttonsDetails.DeleteClick += new EventHandler(DeleteTabela);

            propertyGridComboBox.SelectedIndex = 2;

            ActualizaDataGrid();

            try
            {
                var subtype = subtipo.getSubtipo();
                ComboListSubtipo.Add(" ");
                foreach (Subtipos st in subtype)
                {
                    ComboListSubtipo.Add(st.Nome);
                }

                types = tipo.getTipo();

                ListTipo.Add(" ");
                foreach (Tipos t in types)
                {
                    ListTipo.Add(t.Nome);
                }

                AutoComplete1.ItemsSource = AutoComplete2.ItemsSource = ListTipo;
                combo_subtipos.ItemsSource = ComboListSubtipo;

            }
            catch { }

            textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();

        }

        private void ActualizaDataGrid()
        {
            IEnumerable<object> lt = tabela.Tabelas_TipoSubtipo();
            listaDatagrid = lt;

            data1.ItemsSource = lt;
        }

        
        private void AutoComplete1_KeyUp_2(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Search();
            }
        }


        private void Search_Click_1(object sender, RoutedEventArgs e)
        {
            this.Search();
        }

        private void Search()
        {
            if (AutoComplete2.Text.Trim() != "")
            {

                try
                {
                    string nr = tipo.getIDFromNomeTipo(AutoComplete2.Text);
                    if (nr.Trim() == "")
                        nr = subtipo.getIDFromNomeSubtipos(AutoComplete2.Text, nr);
                    IEnumerable<object> tabelas1 = tabela.Tabelas_TipoSubtipoID(nr);
                    data1.ItemsSource = tabelas1;
                }
                catch { }          
            }
            else
            {
                data1.ItemsSource = listaDatagrid;
            }
        }


        private void Row_DoubleClick(object sender, RoutedEventArgs e)
        {

            Tipos type = null;
            Subtipos subtype = null;
            
            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();

                id_tab = (string)type2.GetProperty("STAMPTABELA").GetValue(selectedItem, null);

                Tabelas tabelas = tabela.getTabelaDetails(id_tab);
                if (tabelas.StampTabela.Trim() != "")
                {
                        textEditor.Text = tabelas.QueryString;

                    if (tabelas.Descricao != null && tabelas.Descricao.Trim() != "")
                        DescricaoTabela.Text = tabelas.Descricao;

                    if (tabelas.StampTabela.Trim() != "")
                    {
                        id_tip = tabelas.StampTipo;
                        type = tipo.getTipoDetails(tabelas.StampTipo);
                        AutoComplete1.Text = type.Nome;
                    }

                    if (tabelas.StampSubtipo != "")
                    {
                        subtype = subtipo.getSubtipoDetails(tabelas.StampSubtipo);
                        combo_subtipos.SelectedValue = subtype.Nome;
                    }

                    
                }
            }
            catch { }

        }

        protected void Home(object sender, EventArgs e)
        {
            PainelCentro.Escolhe_Painel(10, true);
        }

        CompletionWindow completionWindow;

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                completionWindow = new CompletionWindow(textEditor.TextArea);
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }

        }


        void foldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (foldingStrategy != null) 
            {
                foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
            }
        }


        string currentFileName;
        void openFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "XmlDoc|*.xml|C#|*.cs|JavaScript|*.js|HTML|*.htm;*.html|ASP/XHTML|*.asp;*.aspx;*.asax;*.asmx;*.ascx;*.master|FoxPro|*.prg|SQL|*.sql|CSS|*.css|C++|*.c;*.h;*.cc;*.cpp;*.hpp|Java|*.java|Patch|*.patch;*.diff|PowerShell|*.ps1;*.psm1;*.psd1|PHP|*.php|TeX|*.tex|VBNET|*.vb|XML|*.xml;*.xsl;*.xslt;*.xsd;*.manifest;*.config;*.addin;*.xshd;*.wxs;*.wxi;*.wxl;*.proj;*.csproj;*.vbproj;*.ilproj;*.booproj;*.build;*.xfrm;*.targets;*.xaml;*.xpt;*.xft;*.map;*.wsdl;*.disco;*.ps1xml;*.nuspec|MarkDown|*.md|DOC|*.doc;*.docx|Text Files (.txt)|*.txt";

            if (dlg.ShowDialog() ?? false)
            {
                currentFileName = dlg.FileName;
                textEditor.Load(currentFileName);
                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(System.IO.Path.GetExtension(currentFileName));
            }
        }


        void propertyGridComboBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (propertyGrid == null)
                return;
            switch (propertyGridComboBox.SelectedIndex)
            {
                case 0:
                    propertyGrid.SelectedObject = textEditor;
                    break;
                case 1:
                    propertyGrid.SelectedObject = textEditor.TextArea;
                    break;
                case 2:
                    propertyGrid.SelectedObject = textEditor.Options;
                    break;
            }
        }


        FoldingManager foldingManager;
        AbstractFoldingStrategy foldingStrategy;

        void HighlightingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (textEditor.SyntaxHighlighting == null)
            {
                foldingStrategy = null;
            }
            else
            {
                switch (textEditor.SyntaxHighlighting.Name)
                {
                    case "XML":
                        foldingStrategy = new XmlFoldingStrategy();
                        textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
                        break;
                    case "C#":
                    case "C++":
                    case "PHP":
                    case "Java":
                        textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
                        break;
                    default:
                        textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
                        foldingStrategy = null;
                        break;
                }
            }
            if (foldingStrategy != null)
            {
                if (foldingManager == null)
                    foldingManager = FoldingManager.Install(textEditor.TextArea);
                foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
            }
            else
            {
                if (foldingManager != null)
                {
                    FoldingManager.Uninstall(foldingManager);
                    foldingManager = null;
                }
            }
        }


        void saveFileClick(object sender, EventArgs e)
        {

            if (currentFileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false)
                {
                    currentFileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            textEditor.Save(currentFileName);
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
            try
            {
                GridCenter.Height = GridCenter.MaxHeight = parentWindow.ActualHeight - 90;
                TabCentro.MaxWidth = TabCentro.Width = GridCenter.Width = GridCenter.MaxWidth = PainelCentro.ActualWidth - 20;
                TabCentro.Height = TabCentro.MaxHeight = parentWindow.ActualHeight - 150;
                data1.Height = GridCenter.Height - 325;
            }
            catch { }
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            PainelCentro.Escolhe_Painel(9, true);
        }

        private void TabCentro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        protected void DeleteTabela(object sender, EventArgs e)
        {
            try
            {
                object selectedItem = ((DataGrid)data1).SelectedItem;
                Type type2 = selectedItem.GetType();
                id_tab = (string)type2.GetProperty("STAMPTABELA").GetValue(selectedItem, null);

                Tabelas tabelas = tabela.getTabelaDetails(id_tab);
            }
            catch { }

            if (id_tab != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = tabela.Delete_Tabela(id_tab);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "Removida com sucesso!";
                        }
                        else
                            warning = "Falha ao remover!";

                        System.Windows.Forms.MessageBox.Show(warning,
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        break;
                    default:
                        break;
                }

                ActualizaDataGrid();

            }
            else
            {
               
            }

        }

        protected void Add_Tabela(object sender, EventArgs e)
        {

            List<string> ls = new List<string>();
            bool done = true, window = true;
            string msg = "";
            int i = 0;


            if (AutoComplete1.Text.Trim() == "" || textEditor.Text.Trim() == "" || combo_subtipos.SelectedIndex == 0)
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher o tipo, subtipo e código!",
                      "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else if (id_tip == "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem que preencher um tipo válido!",
                       "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {

                try
                {
                    ls.Add("");
                    string nr = "";// tipo.getIDFromNomeTipo(AutoComplete1.Text);
                    ls.Add(id_tip.ToString());
                    nr = subtipo.getIDFromNomeSubtipos(Convert.ToString(combo_subtipos.SelectedItem), id_tip.ToString());
                    ls.Add(nr.ToString());
                    ls.Add(DescricaoTabela.Text);
                    ls.Add(textEditor.Text);
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                    ls.Add("");
                }
                catch
                {
                    done = false;
                }


                if (done == true)
                {
                    if (id_tab != "")
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                         "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (result)
                        {
                            case System.Windows.Forms.DialogResult.Yes:

                                done = tabela.UpdateTabela(id_tab, ls);

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

                        try
                        {
                            done = tabela.InsertTabela(ls);
                        }
                        catch
                        {
                            done = false;
                        }
                        
                        if (done == true)
                            msg = "Guardada com sucesso!";
                        else
                            msg = "Erro ao gravar os dados!";
                    }
                }
                else
                {

                }
                 
                if (window == true)
                {
                    System.Windows.Forms.MessageBox.Show(msg,
                    "Informação!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                
                if (done == true)
                {
                    textEditor.Text = "";
                    DescricaoTabela.Text = "";
                    AutoComplete1.Text = "";
                    AutoComplete1.Focus();
                    AutoComplete1.Focusable = false;
                    combo_subtipos.SelectedIndex = 0;
                    ActualizaDataGrid();
                }
            }

        }


        public void VerifyButton()
        {
            int conta = 0;

            if (textEditor.Text.Trim() != "")
            { 
                conta++; 
            }

            string name = (string)combo_subtipos.SelectedItem;

            if (name != " " && name != null)
            {
                conta++;
            }

            if (AutoComplete1.Text.Trim() != "")
            {
                conta++;
            }

            if (conta == 3)
                buttonsDetails.AlterDataButtonSave(1);
            else
                buttonsDetails.AlterDataButtonSave(-1);
        }

        private void textEditor_KeyUp_1(object sender, KeyEventArgs e)
        {
            VerifyButton();
        }


        private void AutoComplete1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            string name = AutoComplete1.Text;

            try
            {
                if (name != " ")
                {
                    id_tip = tipo.getIDFromNomeTipo(name);

                    if (id_tip != "")
                    {

                        var a = subtipo.Tipo_Subtipo(id_tip);

                        if (a.Count == 0)
                        {
                            a = subtipo.getSubtipo();
                        }
                        List<string> aux = new List<string>();

                        combo_subtipos.ItemsSource = aux;

                        lock (ComboListSubtipo)
                        {
                            ComboListSubtipo.Clear();
                        }

                        try
                        {
                            ComboListSubtipo.Add(" ");
                            foreach (Subtipos f in a)
                            {
                                lock (ComboListSubtipo)
                                {
                                    ComboListSubtipo.Add(f.Nome);
                                }
                            }

                            combo_subtipos.ItemsSource = ComboListSubtipo;

                        }
                        catch
                        { }

                    }
                }
                else
                {
                    name = " ";
                    id_tip = "";
                }
            }
            catch
            {
                id_tip = "";
            }
        }

        private void AutoComplete1_KeyUp_1(object sender, KeyEventArgs e)
        {
             VerifyButton();
        }
        

        private void combo_subtipos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            VerifyButton();
        }

        private void Classifica_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            AutoComplete1.Width = Classifica.ActualWidth - 75;
            combo_subtipos.Width = DescricaoTabela.Width = AutoComplete1.Width;
        }

        
    }
}
