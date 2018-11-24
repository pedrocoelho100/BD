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
    /// Interaction logic for Lista_Bilhetes.xaml
    /// </summary>
    public partial class Lista_Bilhetes : Page
    {
        public Lista_Bilhetes()
        {
            InitializeComponent();

            bilheteslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            List<TipoBilhete> list = DatabaseHelper.AdminCommands.getTiposBilheteList();
            if (list != null)
                bilheteslb.ItemsSource = list;
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (bilheteslb.SelectedItem == null)
            {
                descricao_textbox.Text = "";
                restricoes_textbox.Text = "";
                custo_textbox.Text = "";

                Remover.IsEnabled = false;

                Alterar.Visibility = Visibility.Collapsed;
                Adicionar.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Collapsed;
                id_title.Visibility = Visibility.Collapsed;
            }
            else
            {
                TipoBilhete tb = bilheteslb.SelectedItem as TipoBilhete;
                id_textbox.Text = tb.id.ToString();
                descricao_textbox.Text = tb.Title;
                restricoes_textbox.Text = tb.restricoes;
                custo_textbox.Text = tb.custo;

                Remover.IsEnabled = true;

                Adicionar.Visibility = Visibility.Collapsed;
                Alterar.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Visible;
                id_title.Visibility = Visibility.Visible;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            TipoBilhete tb = DatabaseHelper.AdminCommands.insertTipoBilhete(descricao_textbox.Text, restricoes_textbox.Text, custo_textbox.Text);
            if (tb != null)
            {
                (bilheteslb.ItemsSource as List<TipoBilhete>).Add(tb);
                bilheteslb.Items.Refresh();
                bilheteslb.Items.SortDescriptions.Clear();
                bilheteslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                System.Windows.MessageBox.Show("Tipo de bilhete adicionado com sucesso!", "Sucesso!", MessageBoxButton.OK);
            }
            else
            {
                System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            TipoBilhete tb = (bilheteslb.SelectedItem as TipoBilhete);
            if (tb == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover o tipo de bilhete?", "Remover Tipo de Bilhete", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeTipoBilhete(tb))
                {
                    (bilheteslb.ItemsSource as List<TipoBilhete>).Remove(tb);
                    bilheteslb.Items.Refresh();
                    bilheteslb.Items.SortDescriptions.Clear();
                    bilheteslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover tipo de bilhete!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            TipoBilhete tb = (bilheteslb.SelectedItem as TipoBilhete);
            if (tb == null)
                return;

            tb.Title = descricao_textbox.Text;
            tb.restricoes = restricoes_textbox.Text;
            tb.custo = custo_textbox.Text;

            if (DatabaseHelper.AdminCommands.updateTipoBilhete(tb))
            {
                bilheteslb.Items.Refresh();
                bilheteslb.Items.SortDescriptions.Clear();
                bilheteslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                bilheteslb.SelectedItem = null;
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

        private void listabilhetes_button_Click(object sender, RoutedEventArgs e)
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

        public class TodoItem
        {
            public string Title { get; set; }
        }
    }
}