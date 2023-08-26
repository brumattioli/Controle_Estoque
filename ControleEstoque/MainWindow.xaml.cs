﻿using ControleEstoque.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControleEstoque
{
    /// <summary>
    /// Lógica de interação com a MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DalHelper.CriarBancoSQLite();
            DalHelper.CriarTabelaSQlite();
        }

        /// <summary>
        /// Validação de números nos campos de entrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9]+");
            e.Handled = r.IsMatch(e.Text);
        }
        /// <summary>
        /// Validação de textos nos campos de entrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextValidation(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^a-zA-ZçÇàáÀÁéÉÍíóÓãÃâÂêÊôÔ]");
            e.Handled = r.IsMatch(e.Text);
        }
        /// <summary>
        /// Validação de double nos campos de entrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleValidation(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9.,]+");
            e.Handled = r.IsMatch(e.Text);
        }
        /// <summary>
        /// Botão que faz interação com o Banco de dados e realiza o cadastro dos produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (!Valida())
            {
                MessageBox.Show("Informe os dados do produto a incluir");
                return;
            }
            try
            {
                MessageBoxResult dr = MessageBox.Show("Deseja cadastrar as informações abaixo?" + "\n\r" + "Código: " + codigo_Cadastrar.Text + "\n\r" + "Nome: " + nome_Cadastrar.Text + "\n\r" + "Preço: " + preco_Cadastrar.Text + "\n\r" + "Quantidade: " + quantidade_Cadastrar.Text, "Confirmação", MessageBoxButton.OKCancel);
                switch (dr)
                {
                    case MessageBoxResult.OK:
                        Produto produtoBD = new Produto();
                        produtoBD.Codigo = Convert.ToInt32(codigo_Cadastrar.Text);
                        Produto p = DalHelper.GetProduto_Codigo(produtoBD.Codigo);
                        if (p.Codigo == 0)
                        {
                            p = new Produto();
                            p.Codigo = Convert.ToInt32(codigo_Cadastrar.Text);
                            p.Nome = nome_Cadastrar.Text;
                            p.Preco = Convert.ToDouble(preco_Cadastrar.Text);
                            p.Quantidade = Convert.ToInt32(quantidade_Cadastrar.Text);
                            DalHelper.Add(p);
                            MessageBox.Show("Produto cadastrado com sucesso!");
                            emptyBoxesReg();
                            break;
                        }
                        else MessageBox.Show("Código já existente no Banco de Dados!");
                        break;
                    case MessageBoxResult.Cancel:
                        MessageBox.Show("Produto não cadastrado, insira as informações novamente.");
                        emptyBoxesReg();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }

        }
        /// <summary>
        /// Botão que faz interação com o banco de dados e exibe os dados dos produtos na tela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExibirDados_Click(object sender, EventArgs e)
        {
            try
            {
                List<Produto> l = DalHelper.GetProdutos();
                stockView_Buscar.ItemsSource = l;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        /// <summary>
        /// Classe que valida se todos os dados do cadastro foram preenchidos
        /// </summary>
        /// <returns></returns>
        private bool Valida()
        {
            if (string.IsNullOrEmpty(nome_Cadastrar.Text) && string.IsNullOrEmpty(codigo_Cadastrar.Text) && string.IsNullOrEmpty(preco_Cadastrar.Text) && string.IsNullOrEmpty(quantidade_Cadastrar.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void nome_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void codigo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void preco_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void quantidade_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// Classe que limpa os campos de cadastrar produtos quando o usuário solicita que o produto não seja cadastrado
        /// </summary>
        private void emptyBoxesReg()
        {
            try
            {
                nome_Cadastrar.Text = "";
                codigo_Cadastrar.Text = "";
                preco_Cadastrar.Text = "";
                quantidade_Cadastrar.Text = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString());
            }
        }

        /// <summary>
        /// Botão que limpa os campos de filtro da tela Buscar Produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noFilter_Buscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nomeBusca_Buscar.Text = "";
                codigoBusca_Buscar.Text = "";
                stockView_Buscar.ItemsSource = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString());
            }
        }
        /// <summary>
        /// Botão que limpa os campos de filtro da tela Editar Produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noFilter_Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nomeBusca_Editar.Text = "";
                codigoBusca_Editar.Text = "";
                stockView_Editar.ItemsSource = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString());
            }
        }

        /// <summary>
        /// Botão que limpa os campos de filtro da tela Remover Produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noFilter_Remover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nomeBusca_Remover.Text = "";
                codigoBusca_Remover.Text = "";
                stockView_Remover.ItemsSource = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString());
            }
        }
        /// <summary>
        /// Botão que limpa os campos de filtro da tela Simular Venda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noFilter_Simular_Venda_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nomeBusca_Simular_Venda.Text = "";
                codigoBusca_Simular_Venda.Text = "";
                stockView_Simular_Venda.ItemsSource = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString());
            }
        }

        /// <summary>
        /// Botão que filtra os Produtos da tela Buscar Produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterButton_Buscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codigoBusca_Buscar.Text) && string.IsNullOrEmpty(nomeBusca_Buscar.Text))
            {
                MessageBox.Show("Informe o código ou o nome do produto a ser localizado");
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(codigoBusca_Buscar.Text))
                {
                    int Codigo = Convert.ToInt32(codigoBusca_Buscar.Text);
                    List<Produto> l = new List<Produto>();
                    l.Add(DalHelper.GetProduto_Codigo(Codigo));
                    stockView_Buscar.ItemsSource = l;
                }
                else
                {
                    string Nome = nomeBusca_Buscar.Text;
                    List<Produto> l = DalHelper.GetProduto_Nome(Nome);
                    stockView_Buscar.ItemsSource = l;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void stockView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void codigoBusca_TextChanged(object sender, RoutedEventArgs e)
        {

        }
        private void nomeBusca_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Botão que filtra os Produtos da tela Editar Produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterButton_Editar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codigoBusca_Editar.Text) && string.IsNullOrEmpty(nomeBusca_Editar.Text))
            {
                MessageBox.Show("Informe o código ou o nome do produto a ser editado");
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(codigoBusca_Editar.Text))
                {
                    int Codigo = Convert.ToInt32(codigoBusca_Editar.Text);
                    List<Produto> l = new List<Produto>();
                    l.Add(DalHelper.GetProduto_Codigo(Codigo));
                    stockView_Editar.ItemsSource = l;
                }
                else
                {
                    string Nome = nomeBusca_Editar.Text;
                    List<Produto> l = DalHelper.GetProduto_Nome(Nome);
                    stockView_Editar.ItemsSource = l;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        //private void Selecionar_Item(object sender, RoutedEventArgs e)
        //{
        //    Produto produtoAnterior = (Produto)stockView_Editar.SelectedItem;

        //}

        /// <summary>
        /// Botão que faz a edição/atualização de dados no Banco de Dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (stockView_Editar.SelectedItem == null)
                {
                    MessageBox.Show("É necessário selecionar uma linha para editar.");
                    codigo_Editar.Text = "";
                }
                else
                {
                    if (string.IsNullOrEmpty(quantidade_Editar.Text) && string.IsNullOrEmpty(nome_Editar.Text) && string.IsNullOrEmpty(preco_Editar.Text))
                    {
                        MessageBox.Show("Para editar um produto, você precisa informar um novo nome, quantidade ou preço para o item.");
                        codigo_Editar.Text = "";
                    }
                    Produto produtoAnterior = (Produto)stockView_Editar.SelectedItem;
                    Produto produto = new Produto();

                    if (quantidade_Editar.Text != "")
                    {
                        produto.Codigo = Convert.ToInt32(produtoAnterior.Codigo);
                        produto.Quantidade = Convert.ToInt32(quantidade_Editar.Text);
                        DalHelper.Update_Quantidade(produto, produtoAnterior);
                        MessageBox.Show("A quantidade do produto foi atualizada!");
                    }
                    if (nome_Editar.Text != "")
                    {
                        produto.Codigo = Convert.ToInt32(produtoAnterior.Codigo);
                        produto.Nome = nome_Editar.Text;
                        DalHelper.Update_Nome(produto, produtoAnterior);
                        MessageBox.Show("O nome do produto foi atualizado!");
                    }
                    if (preco_Editar.Text != "")
                    {
                        produto.Codigo = Convert.ToInt32(produtoAnterior.Codigo);
                        produto.Preco = Convert.ToDouble(preco_Editar.Text);
                        DalHelper.Update_Preco(produto, produtoAnterior);
                        MessageBox.Show("O preço do produto foi atualizado!");
                    }
                    filterButton_Editar_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
        /// <summary>
        /// Botão que remove Produtos no Banco de Dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (stockView_Remover.SelectedItem == null)
                {
                    MessageBox.Show("É necessário selecionar uma linha para remover.");
                }
                else
                {
                    Produto produto = (Produto)stockView_Remover.SelectedItem;
                    produto.Codigo = Convert.ToInt32(produto.Codigo);
                    DalHelper.Remover_Item(produto);
                    MessageBox.Show("O produto de código " + produto.Codigo + " foi removido com sucesso!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        /// <summary>
        /// Botão que limpa os campos de filtro da tela Remover Produtos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterButton_Remover_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codigoBusca_Remover.Text) && string.IsNullOrEmpty(nomeBusca_Remover.Text))
            {
                MessageBox.Show("Informe o código ou o nome do produto a ser removido");
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(codigoBusca_Remover.Text))
                {
                    int Codigo = Convert.ToInt32(codigoBusca_Remover.Text);
                    List<Produto> l = new List<Produto>();
                    l.Add(DalHelper.GetProduto_Codigo(Codigo));
                    stockView_Remover.ItemsSource = l;
                }
                else
                {
                    string Nome = nomeBusca_Remover.Text;
                    List<Produto> l = DalHelper.GetProduto_Nome(Nome);
                    stockView_Remover.ItemsSource = l;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        /// <summary>
        /// Botão que simula a Venda de produtos realizando a interação com o Banco de Dados para buscar a 
        /// quantidade existente daquele produto e chama a função Update do DalHelper para atualizar a quantidade no Banco de Dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simularVenda_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult dr = MessageBox.Show("Deseja realizar a venda conforme as informações abaixo?" + "\n\r" + "Código: " + codigo_Simular_Venda.Text + "\n\r" + "Nome: " + nome_Simular_Venda.Text + "\n\r" + "Preço: " + preco_Simular_Venda.Text + "\n\r" + "Quantidade: " + quantidade_Simular_Venda.Text, "Confirmação", MessageBoxButton.OKCancel);
                switch (dr)
                {
                    case MessageBoxResult.OK:
                        Produto produtoAnterior = (Produto)stockView_Simular_Venda.SelectedItem;
                        Produto produto = DalHelper.GetProduto_Codigo(Convert.ToInt32(codigo_Simular_Venda.Text));
                        

                            if (quantidade_Simular_Venda.Text == null)
                            {
                                MessageBox.Show("Digite a quantidade para realizar a venda.");
                            }
                            else if (quantidade_Simular_Venda.Text != null)
                            {
                                int quantidade = produto.Quantidade - Convert.ToInt32(quantidade_Simular_Venda.Text);
                                if (quantidade >= 0)
                                {
                                    produto.Codigo = Convert.ToInt32(produtoAnterior.Codigo);
                                    produto.Quantidade = produto.Quantidade - Convert.ToInt32(quantidade_Simular_Venda.Text);
                                    DalHelper.Update_Quantidade(produto, produtoAnterior);
                                    string cd = DalHelper.Add_Venda(produto);
                                    MessageBox.Show("A venda foi realizada! Venda código " + cd + "!");
                                    codigo_Simular_Venda.Text = "";
                                    nome_Simular_Venda.Text = "";
                                    preco_Simular_Venda.Text = "";
                                    quantidade_Simular_Venda.Text = "";
                                    stockView_Simular_Venda.ItemsSource = "";
                            }
                            }
                        break;
                    case MessageBoxResult.Cancel:
                        MessageBox.Show("Venda não realizada, insira as informações novamente.");
                        codigo_Simular_Venda.Text = "";
                        nome_Simular_Venda.Text = "";
                        preco_Simular_Venda.Text = "";
                        quantidade_Simular_Venda.Text = "";
                        stockView_Simular_Venda.ItemsSource = "";
                        break;
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
        private void getItem(object sender, MouseButtonEventArgs e)
        {
            Produto? p = stockView_Simular_Venda.SelectedItem as Produto;
            codigo_Simular_Venda.Text = p.Codigo.ToString();
            nome_Simular_Venda.Text = p.Nome.ToString();
            preco_Simular_Venda.Text = p.Preco.ToString();
        }

        /// <summary>
        /// Botão que filtra os Produtos da tela Simular Venda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterButton_Simular_Venda_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codigoBusca_Simular_Venda.Text) && string.IsNullOrEmpty(nomeBusca_Simular_Venda.Text))
            {
                MessageBox.Show("Informe o código ou o nome do produto a ser vendido");
                return;
            }
            try
            {
                codigo_Simular_Venda.Text = "";
                nome_Simular_Venda.Text = "";
                preco_Simular_Venda.Text = "";
                quantidade_Simular_Venda.Text = "";

                if (!string.IsNullOrEmpty(codigoBusca_Simular_Venda.Text))
                {
                    int Codigo = Convert.ToInt32(codigoBusca_Simular_Venda.Text);
                    List<Produto> l = new List<Produto>();
                    l.Add(DalHelper.GetProduto_Codigo(Codigo));
                    stockView_Simular_Venda.ItemsSource = l;
                }
                else
                {
                    string Nome = nomeBusca_Simular_Venda.Text;
                    List<Produto> l = DalHelper.GetProduto_Nome(Nome);
                    stockView_Simular_Venda.ItemsSource = l;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
        private void filterButton_Buscar_Vendas_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(codigoBusca_Buscar_Vendas.Text) && string.IsNullOrEmpty(codigoBusca_Buscar_Vendas_Produto.Text) && string.IsNullOrEmpty(nomeBusca_Buscar_Vendas.Text))
            {
                MessageBox.Show("Informe o código da venda, código do produto ou o nome do produto a ser buscado");
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(codigoBusca_Buscar_Vendas.Text))
                {
                    int cod = Convert.ToInt32(codigoBusca_Buscar_Vendas.Text);
                    List<Venda> vendas = new List<Venda>();
                    vendas.Add(DalHelper.GetVenda_Codigo(cod));
                    stockView_Buscar_Vendas.ItemsSource = vendas;
                }
                else
                {
                    if (!string.IsNullOrEmpty(codigoBusca_Buscar_Vendas_Produto.Text))
                    {
                        int cod = Convert.ToInt32(codigoBusca_Buscar_Vendas_Produto.Text);
                        List<Venda> vendas = new List<Venda>();
                        vendas.Add(DalHelper.GetVenda_Codigo_Produto(cod));
                        stockView_Buscar_Vendas.ItemsSource = vendas;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(nomeBusca_Buscar_Vendas.Text))
                        {
                            string nome = nomeBusca_Buscar_Vendas.Text;
                            List<Venda> vendas = new List<Venda>();
                            vendas = DalHelper.GetVenda_Nome_Produto(nome);
                            stockView_Buscar_Vendas.ItemsSource = vendas;
                            quantidade_Registros.Content = ("Quantidade de Vendas: " + vendas.Count.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
        private void noFilter_Buscar_Vendas_Click(object sender, RoutedEventArgs e)
        {
            codigoBusca_Buscar_Vendas.Text = "";
            codigoBusca_Buscar_Vendas_Produto.Text = "";
            nomeBusca_Buscar_Vendas.Text = "";
            stockView_Buscar_Vendas.ItemsSource = "";
        }
        private void getItem_Editar(object sender, MouseButtonEventArgs e)
        {
            Produto? p = stockView_Editar.SelectedItem as Produto;
            codigo_Editar.Text = p.Codigo.ToString();
        }
    }
}