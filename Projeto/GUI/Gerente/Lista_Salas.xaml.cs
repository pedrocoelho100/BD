using BD___Project.Entities;
using BD___Project.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Lista_Salas.xaml
    /// </summary>
    public partial class Lista_Salas : Page
    {
        private int idCinema;
        private List<CheckBox> lugares = new List<CheckBox>();

        public Lista_Salas(int idCinema)
        {
            this.idCinema = idCinema;
            InitializeComponent();

            List<Sala> salas = DatabaseHelper.AdminCommands.getSalasList(idCinema);
            if(salas != null)
                salaslb.ItemsSource = salas;
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Sala sala = salaslb.SelectedItem as Sala;
            if (sala != null)
            {
                sala_textbox.Text = sala.num_sala.ToString();
                filas_textbox.Text = sala.num_filas.ToString();
                colunas_textbox_Copy.Text = sala.num_lugares_fila.ToString();

                Adicionar.IsEnabled = false;
                sala_textbox.IsEnabled = false;
            }
            else
            {
                sala_textbox.Text = "";
                filas_textbox.Text = "";
                colunas_textbox_Copy.Text = "";

                Adicionar.IsEnabled = true;
                sala_textbox.IsEnabled = true;
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            DataTable _dt = new DataTable("Lugares");
            _dt.Columns.Add("fila", typeof(int));
            _dt.Columns.Add("num_lugar", typeof(int));
            _dt.Columns.Add("lugar_def", typeof(bool));

            int num_cols = int.Parse(colunas_textbox_Copy.Text);
            int num_rows = int.Parse(filas_textbox.Text);

            for(int col = 0; col < int.Parse(colunas_textbox_Copy.Text); col++)
            {
                for(int row = 0; row < num_rows; row++)
                {
                    _dt.Rows.Add(row, col, lugares[col * num_rows + row].IsChecked);
                }
            }

            Sala s = DatabaseHelper.AdminCommands.insertSala(idCinema, sala_textbox.Text, filas_textbox.Text, colunas_textbox_Copy.Text, _dt);
            if (s != null)
            {
                (salaslb.ItemsSource as List<Sala>).Add(s);
                salaslb.Items.Refresh();
                salaslb.Items.SortDescriptions.Clear();
                salaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("num_sala", System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Sala s = (salaslb.SelectedItem as Sala);
            if (s == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover esta sala?", "Remover Tipo de Bilhete", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeSala(s))
                {
                    (salaslb.ItemsSource as List<Sala>).Remove(s);
                    salaslb.Items.Refresh();
                    salaslb.Items.SortDescriptions.Clear();
                    salaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("num_sala", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover sala!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gestor_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listasalas_button_Click(object sender, RoutedEventArgs e)
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

        private void PreviewTextInputNumberOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
                e.Handled = true;
            else if (sender != sala_textbox )
            {
                int num = int.Parse((sender as TextBox).Text + e.Text);
                e.Handled = num <= 0 || num > 10;
            }
        }

        private void filas_colunas_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filas_textbox.Text.Length > 0 && colunas_textbox_Copy.Text.Length > 0)
            {
                gridLugares.Visibility = Visibility.Visible;
                lugares_textblock.Visibility = Visibility.Visible;
                gridLugares.ColumnDefinitions.Clear();
                gridLugares.RowDefinitions.Clear();

                for (int i = 0; i < int.Parse(colunas_textbox_Copy.Text); i++)
                    gridLugares.ColumnDefinitions.Add(new ColumnDefinition());

                for (int i = 0; i < int.Parse(filas_textbox.Text); i++)
                {
                    gridLugares.RowDefinitions.Add(new RowDefinition());
                }

                lugares.Clear();
                for (int col = 0; col < int.Parse(colunas_textbox_Copy.Text); col++)
                {
                    for (int row = 0; row < int.Parse(filas_textbox.Text); row++)
                    {
                        CheckBox cb = new CheckBox();
                        cb.Margin = new Thickness(1);
                        cb.HorizontalAlignment = HorizontalAlignment.Center;
                        cb.VerticalAlignment = VerticalAlignment.Center;
                        cb.SetValue(Grid.ColumnProperty, col);
                        cb.SetValue(Grid.RowProperty, row);
                        gridLugares.Children.Add(cb);
                        lugares.Add(cb);
                    }
                }
                
            }
            else {
                gridLugares.Visibility = Visibility.Collapsed;
                lugares_textblock.Visibility = Visibility.Collapsed;
            }
        }
    }
}