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
    /// Interaction logic for Lista_Empregados.xaml
    /// </summary>
    public partial class Lista_Empregados : Page
    {
        private int idCinema;

        public Lista_Empregados(int idCinema)
        {
            this.idCinema = idCinema;
            InitializeComponent();

            empregadoslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            List<Empregado> list;
            if (idCinema != -1)
            {
                list = DatabaseHelper.AdminCommands.getEmpregadoList(idCinema);
            }
            else
            {
                //list = DatabaseHelper.ManagerCommands.getOwnEmpregadoList();
                list = null;
                cinema_textblock.Visibility = Visibility.Collapsed;
                cinema_combobox.Visibility = Visibility.Collapsed;
            }

            if (list != null)
                empregadoslb.ItemsSource = list;

            cinema_combobox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));

            List<Cinema> list_c = DatabaseHelper.AdminCommands.getCinemaList();
            cinema_combobox.ItemsSource = list_c;
            if (list_c == null)
                cinema_combobox.IsEnabled = false;
            else
            {
                cinema_combobox.IsEnabled = true;
                foreach (Cinema c in list_c)
                {
                    if (c.id == idCinema)
                    {
                        cinema_combobox.SelectedItem = c;
                        break;
                    }
                }
            }
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (empregadoslb.SelectedItem == null)
            {
                nome_textbox.Text = "";
                nif_textbox.Text = "";
                email_textbox.Text = "";
                salario_textbox.Text = "";
                password_textbox.Password = "";

                if (idCinema != -1)
                {
                    List<Cinema> list = DatabaseHelper.AdminCommands.getCinemaList();
                    cinema_combobox.ItemsSource = list;
                    if (list == null)
                        cinema_combobox.IsEnabled = false;
                    else
                    {
                        cinema_combobox.IsEnabled = true;
                        foreach (Cinema c in list)
                        {
                            if (c.id == idCinema)
                            {
                                cinema_combobox.SelectedItem = c;
                                break;
                            }
                        }
                    }
                }

                Remover.IsEnabled = false;

                Alterar.Visibility = Visibility.Collapsed;
                id_textblock.Visibility = Visibility.Collapsed;
                id_textbox.Visibility = Visibility.Collapsed;
                password_textblock.Visibility = Visibility.Visible;
                password_textbox.Visibility = Visibility.Visible;
                Adicionar.Visibility = Visibility.Visible;
            }
            else
            {
                Empregado emp = empregadoslb.SelectedItem as Empregado;
                id_textbox.Text = emp.id.ToString();
                nome_textbox.Text = emp.Title;
                nif_textbox.Text = emp.nif;
                email_textbox.Text = emp.email;
                salario_textbox.Text = emp.salario.ToString();

                if (idCinema != -1)
                {
                    List<Cinema> list = DatabaseHelper.AdminCommands.getCinemaList();
                    cinema_combobox.ItemsSource = list;
                    if (list == null)
                        cinema_combobox.IsEnabled = false;
                    else
                    {
                        cinema_combobox.IsEnabled = true;
                        foreach (Cinema c in list)
                        {
                            if (c.id == emp.cinema)
                            {
                                cinema_combobox.SelectedItem = c;
                                break;
                            }
                        }
                    }
                }

                Remover.IsEnabled = true;

                Adicionar.Visibility = Visibility.Collapsed;
                id_textblock.Visibility = Visibility.Visible;
                id_textbox.Visibility = Visibility.Visible;
                password_textblock.Visibility = Visibility.Collapsed;
                password_textbox.Visibility = Visibility.Collapsed;
                Alterar.Visibility = Visibility.Visible;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp = DatabaseHelper.AdminCommands.insertEmpregado(nome_textbox.Text, nif_textbox.Text, email_textbox.Text, salario_textbox.Text, (cinema_combobox.SelectedItem as Cinema).id, password_textbox.Password);
            if (emp != null)
            {
                (empregadoslb.ItemsSource as List<Empregado>).Add(emp);
                empregadoslb.Items.Refresh();
                empregadoslb.Items.SortDescriptions.Clear();
                empregadoslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp = (empregadoslb.SelectedItem as Empregado);
            if (emp == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover o tipo de bilhete?", "Remover Tipo de Bilhete", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeEmpregado(emp))
                {
                    (empregadoslb.ItemsSource as List<Empregado>).Remove(emp);
                    empregadoslb.Items.Refresh();
                    empregadoslb.Items.SortDescriptions.Clear();
                    empregadoslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover tipo de bilhete!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp = (empregadoslb.SelectedItem as Empregado);
            if (emp == null)
                return;

            emp.Title = nome_textbox.Text;
            emp.nif = nif_textbox.Text;
            emp.email = email_textbox.Text;
            emp.salario = salario_textbox.Text;

            if (DatabaseHelper.AdminCommands.updateEmpregado(emp))
            {
                empregadoslb.Items.Refresh();
                empregadoslb.Items.SortDescriptions.Clear();
                empregadoslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                empregadoslb.SelectedItem = null;
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gestor_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listaempregados_button_Click(object sender, RoutedEventArgs e)
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