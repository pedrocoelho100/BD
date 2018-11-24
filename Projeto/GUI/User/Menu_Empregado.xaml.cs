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
    /// Interaction logic for Menu_Empregado.xaml
    /// </summary>
    public partial class Menu_Empregado : Page
    {
        private int idCinema;
        public Menu_Empregado(int idCinema)
        {
            this.idCinema = idCinema;
            InitializeComponent();
        }

        public Menu_Empregado()
        {
            this.idCinema = -1;
            InitializeComponent();
        }


        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (filmeslb.SelectedItem != null)
            {
                List<InstanciaSessao> inst = DatabaseHelper.AdminCommands.getListInstSessoesPorDiaFilme(idCinema, calendar.SelectedDate.Value.ToString(), (filmeslb.SelectedItem as Filme).id);
                if(inst != null)
                {
                    horaslb.ItemsSource = inst;
                    horaslb.Items.Refresh();
                    horaslb.Items.SortDescriptions.Clear();
                    horaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
                }
            }
            else
            {
                horaslb.ItemsSource = null;
            }
        }

        private void lbTodoList_SelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<Lugar> lugares = DatabaseHelper.AdminCommands.getListLugaresInstSessao(idCinema, ((int) calendar.SelectedDate.Value.DayOfWeek) + 1, (horaslb.SelectedItem as InstanciaSessao).sessao.hora, calendar.SelectedDate.Value.ToString(), (horaslb.SelectedItem as InstanciaSessao).num_sala);

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
                filmeslb.ItemsSource = DatabaseHelper.AdminCommands.getFilmesPorDiaList(idCinema, date.ToString());
                filmeslb.Items.Refresh();
                filmeslb.Items.SortDescriptions.Clear();
                filmeslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Title", System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                filmeslb.ItemsSource = null;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void empregado_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void comprarbilhetes_button_Click(object sender, RoutedEventArgs e)
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

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("'OK' para confirmar a compra ou 'cancelar' para fazer alterações.", "Confirmar Compra", System.Windows.MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                MessageBoxResult messageBoxResult2 = System.Windows.MessageBox.Show("Compra efetuada com sucesso!", "Confirmar Compra", System.Windows.MessageBoxButton.OK);
                Menu_Empregado menu_empregado = new Menu_Empregado(idCinema);
                this.NavigationService.Navigate(menu_empregado);
            }
        }
    }
}