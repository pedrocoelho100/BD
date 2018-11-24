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
    /// Interaction logic for Lista_Tecnologias.xaml
    /// </summary>
    public partial class Lista_Tecnologias : Page
    {
        public Lista_Tecnologias()
        {
            InitializeComponent();

            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Title = "Digital" });
            items.Add(new TodoItem() { Title = "3D" });
            items.Add(new TodoItem() { Title = "4DX" });
            items.Add(new TodoItem() { Title = "IMAX" });

            tecnologiaslb.ItemsSource = items;
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tecnologiaslb.SelectedItem != null)
                this.Title = (tecnologiaslb.SelectedItem as TodoItem).Title;
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (!id_textbox.Text.Equals("") && !nome_textbox.Text.Equals(""))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tecnologia adicionada com sucesso!", "Sucesso!", MessageBoxButton.OK);
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
            } 
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover a tecnologia?", "Remover Tecnologia", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (TodoItem removedItem in tecnologiaslb.SelectedItems)
                {
                    (tecnologiaslb.ItemsSource as List<TodoItem>).Remove(removedItem);
                }
                tecnologiaslb.Items.Refresh();
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            if (!id_textbox.Text.Equals("") && !nome_textbox.Text.Equals(""))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tecnologia alterada com sucesso!", "Sucesso!", MessageBoxButton.OK);
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

        private void listatecnologias_button_Click(object sender, RoutedEventArgs e)
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