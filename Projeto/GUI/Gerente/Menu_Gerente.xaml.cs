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
    /// Interaction logic for Menu_Gestor.xaml
    /// </summary>
    public partial class Menu_Gerente : Page
    {
        private int idCinema;

        public Menu_Gerente(int idCinema)
        {
            this.idCinema = idCinema;
            InitializeComponent();
        }

        public Menu_Gerente()
        {
            this.idCinema = -1;
            InitializeComponent();
            voltarBtn.Visibility = Visibility.Collapsed;
        }

        private void empregados_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Empregados lista_empregados = new Lista_Empregados(idCinema);
            this.NavigationService.Navigate(lista_empregados);
        }

        private void sessoes_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Sessoes lista_sessoes = new Lista_Sessoes(idCinema);
            this.NavigationService.Navigate(lista_sessoes);
        }

        private void salas_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Salas lista_salas = new Lista_Salas(idCinema);
            this.NavigationService.Navigate(lista_salas);
        }

        private void bilhetes_button_Click(object sender, RoutedEventArgs e)
        {
            Menu_Empregado menu_empregado = new Menu_Empregado(idCinema);
            this.NavigationService.Navigate(menu_empregado);
        }

        private void instancias_button_Click(object sender, RoutedEventArgs e)
        {
            Lista_Instancias lista_instancias = new Lista_Instancias(idCinema);
            this.NavigationService.Navigate(lista_instancias);
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

        private void gestor_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}