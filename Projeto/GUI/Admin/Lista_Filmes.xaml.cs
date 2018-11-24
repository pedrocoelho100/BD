using BD___Project.Entities;
using BD___Project.Helpers;
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

namespace BD___Project.GUI
{
    /// <summary>
    /// Interaction logic for Lista_Filmes.xaml
    /// </summary>
    public partial class Lista_Filmes : Page
    {
        public Lista_Filmes()
        {
            InitializeComponent();

            filmeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            List<Filme> list = DatabaseHelper.AdminCommands.getFilmeList();
            if (list != null)
                filmeslb.ItemsSource = list;

            List<Distribuidora> l = DatabaseHelper.AdminCommands.getDistribuidoraList();
            if (l == null)
            {
                distribuidora_combobox.IsEnabled = false;
            }
            else
            {
                distribuidora_combobox.IsEnabled = true;
                distribuidora_combobox.ItemsSource = l;
            }

            distribuidora_combobox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (filmeslb.SelectedItem == null)
            {
                titulo_textbox.Text = "";
                idade_textbox.Text = "";
                duracao_textbox.Text = "";
                data_textbox.Text = "";
                idioma_textbox.Text = "";
                distribuidora_combobox.SelectedItem = null;

                Remover.IsEnabled = false;

                Alterar.Visibility = Visibility.Collapsed;
                Adicionar.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Collapsed;
                id_title.Visibility = Visibility.Collapsed;
            }
            else
            {
                Filme f = filmeslb.SelectedItem as Filme;
                id_textbox.Text = f.id.ToString();
                titulo_textbox.Text = f.Title;
                idade_textbox.Text = f.idadeMin.ToString();
                duracao_textbox.Text = f.duracao.ToString();
                data_textbox.Text = f.estreia.ToString();
                idioma_textbox.Text = f.idioma.ToString();

                List<Distribuidora> list = DatabaseHelper.AdminCommands.getDistribuidoraList();
                distribuidora_combobox.ItemsSource = list;
                if (list == null)
                    distribuidora_combobox.IsEnabled = false;
                else
                {
                    distribuidora_combobox.IsEnabled = true;
                    foreach (Distribuidora dist in list)
                    {
                        if (dist.id == f.dist)
                        {
                            distribuidora_combobox.SelectedItem = dist;
                            break;
                        }
                    }
                }

                Remover.IsEnabled = true;

                Adicionar.Visibility = Visibility.Collapsed;
                Alterar.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Visible;
                id_title.Visibility = Visibility.Visible;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            Distribuidora dist = distribuidora_combobox.SelectedItem as Distribuidora;
            if (dist != null)
            {
                Filme filme = DatabaseHelper.AdminCommands.insertFilme(dist.id, titulo_textbox.Text, int.Parse(idade_textbox.Text), int.Parse(duracao_textbox.Text), data_textbox.Text, idioma_textbox.Text.ToUpper());
                if (filme != null)
                {
                    (filmeslb.ItemsSource as List<Filme>).Add(filme);
                    filmeslb.Items.Refresh();
                    filmeslb.Items.SortDescriptions.Clear();
                    filmeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                    System.Windows.MessageBox.Show("Filme adicionado com sucesso!", "Sucesso!", MessageBoxButton.OK);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Filme f = (filmeslb.SelectedItem as Filme);
            if (f == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover o filme?", "Remover Filme", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeFilme(f))
                {
                    (filmeslb.ItemsSource as List<Filme>).Remove(f);
                    filmeslb.Items.Refresh();
                    filmeslb.Items.SortDescriptions.Clear();
                    filmeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover Filme!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            Filme f = (filmeslb.SelectedItem as Filme);
            Distribuidora dist = distribuidora_combobox.SelectedItem as Distribuidora;
            if (f == null || dist == null)
                return;

            f.Title = titulo_textbox.Text;
            f.idadeMin = int.Parse(idade_textbox.Text);
            f.duracao = int.Parse(duracao_textbox.Text);
            f.estreia = data_textbox.Text;
            f.idioma = idioma_textbox.Text.ToUpper();
            f.dist = dist.id;

            if (DatabaseHelper.AdminCommands.updateFilme(f))
            {
                filmeslb.Items.Refresh();
                filmeslb.Items.SortDescriptions.Clear();
                filmeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                filmeslb.SelectedItem = null;
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void admin_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listafilmes_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende terminar a sessão?", "Terminar Sessão", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Login login_Page = new Login();
                this.NavigationService.Navigate(login_Page);
            }
        }
    }
}