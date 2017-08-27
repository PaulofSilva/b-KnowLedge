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
using b_KnowLedge.Models;
using b_KnowLedge.ViewModels;
using Elmah;
using Microsoft.Win32;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Windows.Threading;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for PainelDetalhesConhecimento.xaml
    /// </summary>
    public partial class PainelDetalhesConhecimento : UserControl
    {

        Window parentWindow;
        UserControlCentro PainelCentro;
        Controls.ButtonsDetails buttonsDetails;
        Conhecimento conhe;
        Controls.TreeView treeView;
        Controls.PropertyGrid propertyGridInfo;
        Anexo anexo;
        string id_conn = "";
        int numButtons = 0;
        

        public PainelDetalhesConhecimento(UserControlCentro ucc, bool back)
        {
            InitializeComponent();
            PainelCentro = ucc;
            conhe = new Conhecimento();
            anexo = new Anexo();

            buttonsDetails = new Controls.ButtonsDetails();
            gridFundo.Children.Add(buttonsDetails);

            buttonsDetails.SaveClick += new EventHandler(Add_Conhecimento);
            buttonsDetails.HomeClick += new EventHandler(Home);
            buttonsDetails.DeleteClick += new EventHandler(DeleteConhecimento);

            treeView = new Controls.TreeView(this);
            gridPesquisa.Children.Add(treeView);
            propertyGridInfo = new Controls.PropertyGrid(this);
            gridPropertyInfo.Children.Add(propertyGridInfo);
            
            propertyGridComboBox.SelectedIndex = 2;

            textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();

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


        public void LimpaCampos()
        {
            textEditor.Text = "";
            propertyGridInfo.LimpaCampos();
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
                propertyGridInfo.VerificaButtons();
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
                        //foldingStrategy = new BraceFoldingStrategy();
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


        public string Verify_ComboProjetos_PropertyGrid()
        {
            string nr = propertyGridInfo.Verify_ComboProjetos();

            return nr;
        }

        public string Verify_ComboEntidades_PropertyGrid()
        {
            string nr = propertyGridInfo.Verify_ComboEntidade();

            return nr;
        }

        public string Verify_ComboSubtipos_PropertyGrid()
        {
            string nr = propertyGridInfo.Verify_ComboSubtipo();

            return nr;
        }

        public string Verify_ComboTipos_PropertyGrid()
        {
            string nr = propertyGridInfo.Verify_ComboTipo();

            return nr;
        }


        public void Preenche(string con)
        {
            id_conn = con;
            var ls = conhe.getConhecimentoDetails(id_conn);
            Conhecimentos conn = ls;
            propertyGridInfo.Preenche_UserControl(conn);

            textEditor.Text = ls.Codigo;

            this.VerifyButton(numButtons);
        }


        protected void DeleteConhecimento(object sender, EventArgs e)
        {

            if (id_conn != "")
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende eliminar?",
                 "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        bool done = conhe.Delete_Conhecimento(id_conn);
                        string warning = "";
                        if (done == true)
                        {
                            warning = "Removido com sucesso!";
                        }
                        else
                            warning = "Falha ao remover!";

                        System.Windows.Forms.MessageBox.Show(warning,
                        "Aviso!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        PainelCentro.Escolhe_Painel(10, false);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                PainelCentro.Escolhe_Painel(9, false);
            }

        }

        protected void Add_Conhecimento(object sender, EventArgs e)
        {

            List<string> ls1 = propertyGridInfo.GuardarConhecimento();
            List<string> lsFiles = new List<string>();
             List<string> ls = new List<string>();
             bool done = true, window = true;
             string msg = "";
             int i = 0;

             try
             {

                 foreach (string sr in ls1)
                 {
                     if (i == 5)
                     {
                         ls.Add(textEditor.Text);
                     }

                     ls.Add(sr);
                     i++;
                 }

                 
             }
             catch
             {
                 done = false;
             }


             if (done == true)
             {
                 if (id_conn != "")
                 {
                     System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Tem a certeza que pretende alterar os dados?",
                      "Pergunta!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                     switch (result)
                     {
                         case System.Windows.Forms.DialogResult.Yes:

                             done = conhe.UpdateConhecimento(id_conn, ls);

                         var t = propertyGridInfo.Files;
                         string h = "";
                         string num = id_conn;
                         foreach (string r in t)
                         {
                             lock (lsFiles)
                             {
                                 lsFiles.Clear();
                             }
                             lsFiles.Add("");
                             h = r;
                             string extension = System.IO.Path.GetFileName(h);
                             lsFiles.Add(Convert.ToString(num));
                             lsFiles.Add(extension);
                             lsFiles.Add(h);
                             lsFiles.Add(h);
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");

                             anexo.InsertAnexos(lsFiles);
                         }

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
                         done = conhe.InsertConhecimento(ls);

                         string num = "";
                         if (id_conn != "")
                             num = id_conn;
                         else
                             num = conhe.RetornaStamp();


                         var t = propertyGridInfo.Files;
                         string h = "";

                         foreach (string r in t)
                         {
                             lock (lsFiles)
                             {
                                 lsFiles.Clear();
                             }
                             lsFiles.Add("");
                             h = r;
                             string extension = System.IO.Path.GetFileName(h);
                             lsFiles.Add(num);
                             lsFiles.Add(extension);
                             lsFiles.Add(h);
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");
                             lsFiles.Add("");

                             anexo.InsertAnexos(lsFiles);
                         }
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


             if (done == true && id_conn == "")
             {
                 textEditor.Text = "";
                 propertyGridInfo.LimpaCampos();
             }


        }


        public void VerifyButton(int num)
        {
            numButtons = num;
            if(textEditor.Text.Trim()!="")
                num++;

            if (num == 4)
                buttonsDetails.AlterDataButtonSave(1);
            else
                buttonsDetails.AlterDataButtonSave(-1);
        }

        private void textEditor_KeyUp_1(object sender, KeyEventArgs e)
        {
            propertyGridInfo.VerificaButtons();
        }

        private void AnimatedExpander_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

       
       
    }
}
