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
    /// Interaction logic for Lista_Instancias.xaml
    /// </summary>
    public partial class Lista_Instancias : Page
    {
        private int idCinema;
        public Lista_Instancias(int idCinema)
        {
            this.idCinema = idCinema;
            InitializeComponent();

            List<Filme> filmes = DatabaseHelper.AdminCommands.getFilmeList();

            List<Sala> salas = DatabaseHelper.AdminCommands.getSalasList(idCinema);

            filmeslb.ItemsSource = filmes;
            filmeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));

            salaslb.ItemsSource = salas;
            salaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("num_sala", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get reference.
            var calendar = sender as Calendar;

            // ... See if a date is selected.
            if (calendar.SelectedDate.HasValue)
            {
                // ... Display SelectedDate in Title.
                DateTime date = calendar.SelectedDate.Value;
                sessoeslb.ItemsSource = DatabaseHelper.AdminCommands.getSessoesList(idCinema, ((int) date.DayOfWeek) + 1);
                sessoeslb.Items.Refresh();
                sessoeslb.Items.SortDescriptions.Clear();
                sessoeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("hora", System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                sessoeslb.ItemsSource = null;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (calendar.SelectedDate.HasValue && sessoeslb.SelectedItem != null && filmeslb.SelectedItem != null && salaslb.SelectedItem != null)
            {
                bool error = false;
                foreach(Sessao s in sessoeslb.SelectedItems)
                {
                    InstanciaSessao i = DatabaseHelper.AdminCommands.insertInstanciaSessao(s, calendar.SelectedDate.Value.ToString(), (salaslb.SelectedItem as Sala).num_sala, (filmeslb.SelectedItem as Filme).id);
                    if (i == null)
                    {
                        error = true;
                    }
                }
                if(error)
                    System.Windows.MessageBox.Show("Uma ou mais instâncias de sessões não puderam ser introduzidas!", "Sem Sucesso!", MessageBoxButton.OK);
            }
            else
            {
                System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gestor_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listainstancias_button_Click(object sender, RoutedEventArgs e)
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