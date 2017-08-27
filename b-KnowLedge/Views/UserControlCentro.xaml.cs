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
using System.IO;

namespace b_KnowLedge.Views
{
    /// <summary>
    /// Interaction logic for UserControlCentro.xaml
    /// </summary>
    public partial class UserControlCentro : UserControl
    {
        Window parentWindow;
        PainelEntidades painelEmp;
        PainelProjetos painelProjetos;
        PainelPessoas painelPessoas;
        PainelDetalhesEntidades painelDetalhesEntidades;
        PainelConhecimento painelConhecimento;
        PainelDetalhesPessoas painelDetalhesPessoas;
        PainelDetalhesProjetos painelDetalhesProjetos;
        PainelDatabases painelBasesDados;
        PainelDetalhesDatabases painelDetalhesBasesdedados;
        PainelDetalhesConhecimento painelDetalhesConhecimento;
        PainelDetalhesUtilizadores painelDetalhesUtilizadores;
        PainelImportacao painelImportacao;
        PainelTipos painelTipos;
        PainelUtilizadores painelUtilizadores;
        PainelDetalhesTabelas painelDetalhesTabelas;
        PainelSincronizacao painelSincronizacao;

        public string id_entidade = "";
        public string id_projeto = "";
        public string bi = "";
        public string id_bd = "";
        public string id_utilizador = "";
        int anterior = -1;
        int retroceder = -1;
        public bool backward = false;
        public string id_conhecimento = "";

        public UserControlCentro()
        {
            InitializeComponent();
            this.Escolhe_Painel(10, false);
            
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);

            try
            {
                //border1.MaxHeight = border1.Height = parentWindow.ActualHeight - 20;
                //border1.MaxWidth = border1.Width = parentWindow.ActualWidth - 230;
            }
            catch { }
        }


        private void Actual(int id)
        {
            switch (id)
            { 
                //case 0:
                //    home = new Home(this);
                //    painel1.Children.Add(home);
                //    break;
                case 1:
                      painelEmp = new PainelEntidades(this);
                      painel1.Children.Add(painelEmp);
                    break;
                case 2:
                    painelProjetos = new PainelProjetos(this);
                    painel1.Children.Add(painelProjetos);
                    break;
                case 3:
                    painelPessoas = new PainelPessoas(this);
                    painel1.Children.Add(painelPessoas);
                    break;
                case 4:
                    painelDetalhesEntidades = new PainelDetalhesEntidades(this);
                    painel1.Children.Add(painelDetalhesEntidades);
                        if(id_entidade.Trim() != "")
                            painelDetalhesEntidades.Preenche(id_entidade);
                    break;
                case 5:
                    painelDetalhesPessoas = new PainelDetalhesPessoas(this);
                    painel1.Children.Add(painelDetalhesPessoas);
                        if (bi.Trim() != "")
                            painelDetalhesPessoas.Preenche(bi);
                    break;
                case 6:
                    painelDetalhesProjetos = new PainelDetalhesProjetos(this, false);
                    painel1.Children.Add(painelDetalhesProjetos);
                        if (id_projeto.Trim() != "")
                            painelDetalhesProjetos.Preenche(id_projeto);
                    break;
                case 7:
                    painelBasesDados = new PainelDatabases(this);
                    painel1.Children.Add(painelBasesDados);
                    break;
                case 8:
                    painelDetalhesBasesdedados = new PainelDetalhesDatabases(this);
                    painel1.Children.Add(painelDetalhesBasesdedados);
                    if (id_bd.Trim() != "")
                        painelDetalhesBasesdedados.Preenche(id_bd);
                    break;
                case 9:
                    painelConhecimento = new PainelConhecimento(this);
                    painel1.Children.Add(painelConhecimento);
                    break;
                case 10:
                    painelDetalhesConhecimento = new PainelDetalhesConhecimento(this, false);
                    painel1.Children.Add(painelDetalhesConhecimento);
                    if (id_conhecimento.Trim() != "")
                        painelDetalhesConhecimento.Preenche(id_conhecimento);
                    break;
                case 11:
                    painelTipos = new PainelTipos(this);
                    painel1.Children.Add(painelTipos);
                    break;
                case 12:
                    painelUtilizadores = new PainelUtilizadores(this);
                    painel1.Children.Add(painelUtilizadores);
                    break;
                case 13:
                    painelDetalhesUtilizadores = new PainelDetalhesUtilizadores(this);
                    painel1.Children.Add(painelDetalhesUtilizadores);
                    if (id_utilizador.Trim() != "")
                        painelDetalhesUtilizadores.Preenche(id_utilizador);
                    break;
                case 14:
                    painelDetalhesTabelas = new PainelDetalhesTabelas(this, false);
                    painel1.Children.Add(painelDetalhesTabelas);
                    //if (id_utilizador > 0)
                    //    painelDetalhesUtilizadores.Preenche(id_utilizador);
                    break;
                case 15:
                    painelImportacao = new PainelImportacao(this);
                    painel1.Children.Add(painelImportacao);
                    //if (id_utilizador > 0)
                    //    painelDetalhesUtilizadores.Preenche(id_utilizador);
                    break;
                case 16:
                    painelSincronizacao = new PainelSincronizacao(this);
                    painel1.Children.Add(painelSincronizacao);
                    //if (id_utilizador > 0)
                    //    painelDetalhesUtilizadores.Preenche(id_utilizador);
                    break;
                default:
                    break;
            
            }
        }


        private void ActeriorPainel()
        {
            switch (anterior)
            {
                //case 0:
                //    painel1.Children.Remove(home);
                //    home = null;
                //    break;
                case 1:
                    painel1.Children.Remove(painelEmp);
                    painelEmp = null;
                    break;
                case 2:
                    painel1.Children.Remove(painelProjetos);
                    painelProjetos = null;
                    break;
                case 3:
                    painel1.Children.Remove(painelPessoas);
                    painelPessoas = null;
                    break;
                case 4:
                    painel1.Children.Remove(painelDetalhesEntidades);
                    painelDetalhesEntidades = null;
                    break;
                case 5:
                    painel1.Children.Remove(painelDetalhesPessoas);
                    painelDetalhesPessoas = null;
                    break;
                case 6:
                    painel1.Children.Remove(painelDetalhesProjetos);
                    painelDetalhesProjetos = null;
                    break;
                case 7:
                    painel1.Children.Remove(painelBasesDados);
                    painelBasesDados = null;
                    break;
                case 8:
                    painel1.Children.Remove(painelDetalhesBasesdedados);
                    painelDetalhesBasesdedados = null;
                    break;
                case 9:
                    painel1.Children.Remove(painelConhecimento);
                    painelConhecimento = null;
                    break;
                case 10:
                    painel1.Children.Remove(painelDetalhesConhecimento);
                    painelDetalhesConhecimento = null;
                    break;
                case 11:
                    painel1.Children.Remove(painelTipos);
                    painelTipos = null;
                    break;
                case 12:
                    painel1.Children.Remove(painelUtilizadores);
                    painelUtilizadores = null;
                    break;
                case 13:
                    painel1.Children.Remove(painelDetalhesUtilizadores);
                    painelDetalhesUtilizadores = null;
                    break;
                case 14:
                    painel1.Children.Remove(painelDetalhesTabelas);
                    painelDetalhesTabelas = null;
                    break;
                case 15:
                    painel1.Children.Remove(painelImportacao);
                    painelImportacao = null;
                    break;
                case 16:
                    painel1.Children.Remove(painelSincronizacao);
                    painelSincronizacao = null;
                    break;
                default:
                    break;

            }

            GC.Collect();

        }


        public void Escolhe_Painel(int i, bool back)
        {
            if (back == true && i == -1)
            {
                i = retroceder;
                backward = false;
            }
            ActeriorPainel();
            Actual(i);

            retroceder = anterior;
            anterior = i;
        }



    }
}
