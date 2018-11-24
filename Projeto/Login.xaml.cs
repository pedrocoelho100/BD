using BD___Project.Helpers;
using BD___Project.GUI;
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

namespace BD___Project
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void utilizadortb_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.login_button.IsEnabled = !(string.IsNullOrWhiteSpace(utilizadortb.Text) || string.IsNullOrWhiteSpace(passwordtb.Password));
        }

        private void passwordtb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.login_button.IsEnabled = !(string.IsNullOrWhiteSpace(utilizadortb.Text) || string.IsNullOrWhiteSpace(passwordtb.Password));
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.setLogin(utilizadortb.Text, passwordtb.Password);
            switch (DatabaseHelper.getOwnUserRole())
            {
                case -2:
                    MessageBox.Show("Erro na ligação à base de dados.\n Verifique o ID de utilizador e/ou palavra-passe!");
                    break;
                case 0:
                    this.NavigationService.Navigate(new Menu_Admin());
                    this.NavigationService.RemoveBackEntry();
                    break;
                case 1:
                    this.NavigationService.Navigate(new Menu_Gerente());
                    this.NavigationService.RemoveBackEntry();
                    break;
                case 2:
                    this.NavigationService.Navigate(new Menu_Empregado());
                    this.NavigationService.RemoveBackEntry();
                    break;
                default:
                    MessageBox.Show("Utilizador inválido!");
                    break;

            }
        }
    }
}
