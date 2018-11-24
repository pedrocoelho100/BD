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
    /// Interaction logic for Lista_Distribuidoras.xaml
    /// </summary>
    public partial class Lista_Distribuidoras : Page
    {
        public Lista_Distribuidoras()
        {
            InitializeComponent();

            distribuidoraslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            List<Distribuidora> list = DatabaseHelper.AdminCommands.getDistribuidoraList();
            if (list != null)
                distribuidoraslb.ItemsSource = list;
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (distribuidoraslb.SelectedItem == null)
            {
                nome_textbox.Text = "";
                pagamento_textbox.Text = "";
                comissao_textbox.Text = "";

                Remover.IsEnabled = false;

                Alterar.Visibility = Visibility.Collapsed;
                Adicionar.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Collapsed;
                id_title.Visibility = Visibility.Collapsed;
            }
            else
            {
                Distribuidora d = distribuidoraslb.SelectedItem as Distribuidora;
                id_textbox.Text = d.id.ToString();
                nome_textbox.Text = d.Title;
                pagamento_textbox.Text = d.precoInicial.ToString();
                comissao_textbox.Text = d.comissaoBilhete.ToString();

                List<Distribuidora> list = DatabaseHelper.AdminCommands.getDistribuidoraList();

                Remover.IsEnabled = true;

                Adicionar.Visibility = Visibility.Collapsed;
                Alterar.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Visible;
                id_title.Visibility = Visibility.Visible;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            Distribuidora dist = DatabaseHelper.AdminCommands.insertDistribuidora(nome_textbox.Text, pagamento_textbox.Text, comissao_textbox.Text);
            if (dist != null)
            {
                (distribuidoraslb.ItemsSource as List<Distribuidora>).Add(dist);
                distribuidoraslb.Items.Refresh();
                distribuidoraslb.Items.SortDescriptions.Clear();
                distribuidoraslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Distribuidora d = (distribuidoraslb.SelectedItem as Distribuidora);
            if (d == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover a distribuidora?", "Remover Distribuidora", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeDistribuidora(d))
                {
                    (distribuidoraslb.ItemsSource as List<Distribuidora>).Remove(d);
                    distribuidoraslb.Items.Refresh();
                    distribuidoraslb.Items.SortDescriptions.Clear();
                    distribuidoraslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover Distribuidora!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            Distribuidora d = (distribuidoraslb.SelectedItem as Distribuidora);
            if (d == null)
                return;

            d.Title = nome_textbox.Text;
            d.precoInicial = pagamento_textbox.Text;
            d.comissaoBilhete = comissao_textbox.Text;

            if (DatabaseHelper.AdminCommands.updateDistribuidora(d))
            {
                distribuidoraslb.Items.Refresh();
                distribuidoraslb.Items.SortDescriptions.Clear();
                distribuidoraslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                distribuidoraslb.SelectedItem = null;
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

        private void listadistribuidoras_button_Click(object sender, RoutedEventArgs e)
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