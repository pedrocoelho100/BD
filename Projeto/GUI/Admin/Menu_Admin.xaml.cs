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
    /// Interaction logic for Menu_Admin.xaml
    /// </summary>
    public partial class Menu_Admin : Page
    {
        public Menu_Admin()
        {
            InitializeComponent();
        }

        private void cinemas_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Cinemas lista_cinemas = new Lista_Cinemas();
            this.NavigationService.Navigate(lista_cinemas);
        }

        private void filmes_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Filmes lista_filmes = new Lista_Filmes();
            this.NavigationService.Navigate(lista_filmes);
        }

        private void tecnologias_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Tecnologias lista_tecnologias = new Lista_Tecnologias();
            this.NavigationService.Navigate(lista_tecnologias);
        }

        private void distribuidoras_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Distribuidoras lista_distribuidoras = new Lista_Distribuidoras();
            this.NavigationService.Navigate(lista_distribuidoras);
        }

        private void bilhete_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Bilhetes lista_bilhetes = new Lista_Bilhetes();
            this.NavigationService.Navigate(lista_bilhetes);
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

        private void admin_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
