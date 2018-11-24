using BD___Project.Entities;
using BD___Project.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Lista_Cinemas.xaml
    /// </summary>
    public partial class Lista_Cinemas : Page
    {
        public Lista_Cinemas()
        {
            InitializeComponent();

            cinemaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            List<Cinema> list = DatabaseHelper.AdminCommands.getCinemaList();
            if (list != null)
                cinemaslb.ItemsSource = list;

            gerente_combobox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));

        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cinemaslb.SelectedItem == null)
            {
                nome_textbox.Text = "";
                telefone_textbox.Text = "";
                morada_textbox.Text = "";

                Remover.IsEnabled = false;
                Gerir.IsEnabled = false;

                Alterar.Visibility = Visibility.Collapsed;
                id_textblock.Visibility = Visibility.Collapsed;
                id_textbox.Visibility = Visibility.Collapsed;
                gerente_textblock.Visibility = Visibility.Collapsed;
                gerente_combobox.Visibility = Visibility.Collapsed;
                Adicionar.Visibility = Visibility.Visible;
            }
            else
            {
                Cinema c = cinemaslb.SelectedItem as Cinema;
                id_textbox.Text = c.id.ToString();
                nome_textbox.Text = c.Title;
                telefone_textbox.Text = c.telefone;
                morada_textbox.Text = c.morada;

                List<Empregado> list = DatabaseHelper.AdminCommands.getEmpregadoList(c.id);
                list.Add(new Empregado(-1, "", "", "", "", -1));
                gerente_combobox.ItemsSource = list;
                if (list == null)
                    gerente_combobox.IsEnabled = false;
                else
                {
                    gerente_combobox.IsEnabled = true;
                    foreach (Empregado emp in list)
                    {
                        if (emp.id == c.gerente)
                        {
                            gerente_combobox.SelectedItem = emp;
                            break;
                        }
                    }
                }

                Remover.IsEnabled = true;
                Gerir.IsEnabled = true;

                Adicionar.Visibility = Visibility.Collapsed;
                id_textblock.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Visible;
                gerente_textblock.Visibility = Visibility.Visible;
                gerente_combobox.Visibility = Visibility.Visible;
                Alterar.Visibility = Visibility.Visible;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            Cinema cinema = DatabaseHelper.AdminCommands.insertCinema(nome_textbox.Text, morada_textbox.Text, telefone_textbox.Text);
            if (cinema != null)
            {
                (cinemaslb.ItemsSource as List<Cinema>).Add(cinema);
                cinemaslb.Items.Refresh();
                cinemaslb.Items.SortDescriptions.Clear();
                cinemaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            } 
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Cinema c = (cinemaslb.SelectedItem as Cinema);
            if (c == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover o cinema?", "Remover Cinema", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeCinema(c))
                {
                    (cinemaslb.ItemsSource as List<Cinema>).Remove(c);
                    cinemaslb.Items.Refresh();
                    cinemaslb.Items.SortDescriptions.Clear();
                    cinemaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover Cinema!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            Cinema c = (cinemaslb.SelectedItem as Cinema);
            if (c == null)
                return;

            c.Title = nome_textbox.Text;
            c.telefone = telefone_textbox.Text;
            c.morada = morada_textbox.Text;
            Empregado gerente = (gerente_combobox.SelectedItem as Empregado);
            if (gerente != null)
                c.gerente = gerente.id;

            if (DatabaseHelper.AdminCommands.updateCinema(c))
            {
                cinemaslb.Items.Refresh();
                cinemaslb.Items.SortDescriptions.Clear();
                cinemaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                cinemaslb.SelectedItem = null;
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            } 
        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {
            Cinema c = (cinemaslb.SelectedItem as Cinema);
            if (c != null)
            {
                Menu_Gerente menu_Gestor_Page = new Menu_Gerente(c.id);
                this.NavigationService.Navigate(menu_Gestor_Page);
            }
        }

        private void admin_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listacinemas_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Voltar_Click(object sender, RoutedEventArgs e)
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