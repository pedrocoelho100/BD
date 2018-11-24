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
    /// Interaction logic for Lista_Sessoes.xaml
    /// </summary>
    public partial class Lista_Sessoes : Page
    {
        private int idCinema;
        private string[] dias = {"Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado" };

        public Lista_Sessoes(int idCinema)
        {
            this.idCinema = idCinema;
            InitializeComponent();

            /*List<TodoItem> horas = new List<TodoItem>();
            horas.Add(new TodoItem() { Title = "14h20" });
            horas.Add(new TodoItem() { Title = "16h40" });
            horas.Add(new TodoItem() { Title = "19h10" });
            horas.Add(new TodoItem() { Title = "21h50" });*/

            diaslb.ItemsSource = dias;
            //horaslb.ItemsSource = horas;
        }

        public class TodoItem
        {
            public string Title { get; set; }
        }

        private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (diaslb.SelectedItem != null)
            {
                List<Sessao> list = DatabaseHelper.AdminCommands.getSessoesList(idCinema, diaslb.SelectedIndex + 1);
                if (list != null)
                {
                    horaslb.ItemsSource = list;
                    horaslb.Items.Refresh();
                    horaslb.Items.SortDescriptions.Clear();
                    horaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("hora", System.ComponentModel.ListSortDirection.Ascending));
                }
                grid.Visibility = Visibility.Visible;

            }
            else
            {
                horaslb.ItemsSource = null;
                horaslb.Items.Refresh();
                grid.Visibility = Visibility.Collapsed;
            }
        }

        private void lbTodoList_SelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (diaslb.SelectedItem != null)
            {
                grid.Visibility = Visibility.Visible;

                if (horaslb.SelectedItem != null)
                {
                    Sessao s = horaslb.SelectedItem as Sessao;
                    string[] hora = s.hora.Split(':');
                    txtHours.Text = hora[0];
                    txtMinutes.Text = hora[1];
                    desconto_textbox.Text = s.desconto;

                    Alterar.Visibility = Visibility.Visible;
                    Adicionar.Visibility = Visibility.Collapsed;
                    Remover.IsEnabled = true;
                }
                else
                {
                    Alterar.Visibility = Visibility.Collapsed;
                    Adicionar.Visibility = Visibility.Visible;
                    Remover.IsEnabled = false;
                }
            }
            else
            {
                grid.Visibility = Visibility.Collapsed;
                Remover.IsEnabled = false;
            }
        }
        
        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (diaslb.SelectedItem != null)
            {
                Sessao s = DatabaseHelper.AdminCommands.insertSessao(idCinema, diaslb.SelectedIndex + 1, txtHours.Text+":"+txtMinutes.Text, desconto_textbox.Text);
                if (s != null)
                {
                    if (horaslb.ItemsSource == null)
                        horaslb.ItemsSource = new List<Sessao>();
                    (horaslb.ItemsSource as List<Sessao>).Add(s);
                    horaslb.Items.Refresh();
                    horaslb.Items.SortDescriptions.Clear();
                    horaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("hora", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Não selecionou um dia da semana!", "Sem Sucesso!", MessageBoxButton.OK);
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Sessao s = (horaslb.SelectedItem as Sessao);
            if (s == null)
                return;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem a certeza de que pretende remover esta sessão?", "Remover Tipo de Bilhete", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatabaseHelper.AdminCommands.removeSessao(s))
                {
                    (horaslb.ItemsSource as List<Sessao>).Remove(s);
                    horaslb.Items.Refresh();
                    horaslb.Items.SortDescriptions.Clear();
                    horaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("hora", System.ComponentModel.ListSortDirection.Ascending));
                }
                else
                {
                    MessageBoxResult messageBoxErr = System.Windows.MessageBox.Show("Impossível remover sessão!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Alterar_Click(object sender, RoutedEventArgs e)
        {
            if (diaslb.SelectedItem != null)
            {
                Sessao s = (horaslb.SelectedItem as Sessao);
                if (s == null)
                    return;

                s.id_cinema = idCinema;
                s.dia_semana = diaslb.SelectedIndex + 1;
                s.desconto = desconto_textbox.Text;

                if (DatabaseHelper.AdminCommands.updateSessao(s, txtHours.Text + ":" + txtMinutes.Text))
                {
                    s.hora = txtHours.Text + ":" + txtMinutes.Text;
                    horaslb.Items.Refresh();
                    horaslb.Items.SortDescriptions.Clear();
                    horaslb.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("hora", System.ComponentModel.ListSortDirection.Ascending));
                    horaslb.SelectedItem = null;
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Dados inválidos!", "Sem Sucesso!", MessageBoxButton.OK);
                }
            }
        }

        private void Gerir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gestor_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listasessoes_button_Click(object sender, RoutedEventArgs e)
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

        public DateTime? DateTimeValue
        {
            get
            {
                string hours = this.txtHours.Text;
                string minutes = this.txtMinutes.Text;
                if (!string.IsNullOrWhiteSpace(hours) && !string.IsNullOrWhiteSpace(minutes))
                {
                    string value = string.Format("{0}:{1}", this.txtHours.Text, this.txtMinutes.Text);
                    DateTime time = DateTime.Parse(value);
                    return time;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                DateTime? time = value;
                if (time.HasValue)
                {
                    string timeString = time.Value.ToShortTimeString();
                    //9:54 AM
                    string[] values = timeString.Split(':', ' ');
                    if (values.Length == 2)
                    {
                        this.txtHours.Text = values[0];
                        this.txtMinutes.Text = values[1];
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the time span value.
        /// </summary>
        /// <value>The time span value.</value>
        public TimeSpan? TimeSpanValue
        {
            get
            {
                DateTime? time = this.DateTimeValue;
                if (time.HasValue)
                {
                    return new TimeSpan(time.Value.Ticks);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                TimeSpan? timeSpan = value;
                if (timeSpan.HasValue)
                {
                    this.DateTimeValue = new DateTime(timeSpan.Value.Ticks);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            string controlId = this.GetControlWithFocus().Name;
            if ("txtHours".Equals(controlId))
            {
                this.ChangeHours(false);
            }
            else if ("txtMinutes".Equals(controlId))
            {
                this.ChangeMinutes(false);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            string controlId = this.GetControlWithFocus().Name;
            if ("txtHours".Equals(controlId))
            {
                this.ChangeHours(true);
            }
            else if ("txtMinutes".Equals(controlId))
            {
                this.ChangeMinutes(true);
            }
        }

        /// <summary>
        /// Handles the PreviewTextInput event of the txtAmPm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.TextCompositionEventArgs"/> instance containing the event data.</param>
        private void txtAmPm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // prevent users to type text
            e.Handled = true;
        }

        /// <summary>
        /// Handles the KeyUp event of the txt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            // check for up and down keyboard presses
            if (Key.Up.Equals(e.Key))
            {
                btnUp_Click(this, null);
            }
            else if (Key.Down.Equals(e.Key))
            {
                btnDown_Click(this, null);
            }
        }

        /// <summary>
        /// Handles the MouseWheel event of the txt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        private void txt_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                btnUp_Click(this, null);
            }
            else
            {
                btnDown_Click(this, null);
            }
        }

        /// <summary>
        /// Handles the PreviewKeyUp event of the txt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txt_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            // make sure all characters are number
            bool allNumbers = textBox.Text.All(Char.IsNumber);
            if (!allNumbers)
            {
                e.Handled = true;
                return;
            }


            // make sure user did not enter values out of range
            int value;
            int.TryParse(textBox.Text, out value);
            if ("txtHours".Equals(textBox.Name) && value > 12)
            {
                EnforceLimits(e, textBox);
            }
            else if ("txtMinutes".Equals(textBox.Name) && value > 59)
            {
                EnforceLimits(e, textBox);
            }
        }

        /// <summary>
        /// Changes the hours.
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        private void ChangeHours(bool isUp)
        {
            int value = Convert.ToInt32(this.txtHours.Text);
            if (isUp)
            {
                value += 1;
                if (value == 25)
                {
                    value = 1;
                }
            }
            else
            {
                value -= 1;
                if (value == 0)
                {
                    value = 00;
                }
            }
            this.txtHours.Text = Convert.ToString(value);
        }

        /// <summary>
        /// Changes the minutes.
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        private void ChangeMinutes(bool isUp)
        {
            int value = Convert.ToInt32(this.txtMinutes.Text);
            if (isUp)
            {
                value += 1;
                if (value == 60)
                {
                    value = 0;
                }
            }
            else
            {
                value -= 1;
                if (value == -1)
                {
                    value = 59;
                }
            }

            string textValue = Convert.ToString(value);
            if (value < 10)
            {
                textValue = "0" + Convert.ToString(value);
            }
            this.txtMinutes.Text = textValue;
        }

        /// <summary>
        /// Enforces the limits.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        /// <param name="textBox">The text box.</param>
        /// <param name="enteredValue">The entered value.</param>
        private static void EnforceLimits(KeyEventArgs e, TextBox textBox)
        {
            string enteredValue = GetEnteredValue(e.Key);
            string text = textBox.Text.Replace(enteredValue, "");
            if (string.IsNullOrEmpty(text))
            {
                text = enteredValue;
            }
            textBox.Text = text;
            e.Handled = true;
        }

        /// <summary>
        /// Gets the control with focus.
        /// </summary>
        /// <returns></returns>
        private TextBox GetControlWithFocus()
        {
            TextBox txt = new TextBox();
            if (this.txtHours.IsFocused)
            {
                txt = this.txtHours;
            }
            else if (this.txtMinutes.IsFocused)
            {
                txt = this.txtMinutes;
            }
            return txt;
        }

        /// <summary>
        /// Gets the entered value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetEnteredValue(Key key)
        {
            string value = string.Empty;
            switch (key)
            {
                case Key.D0:
                case Key.NumPad0:
                    value = "0";
                    break;
                case Key.D1:
                case Key.NumPad1:
                    value = "1";
                    break;
                case Key.D2:
                case Key.NumPad2:
                    value = "2";
                    break;
                case Key.D3:
                case Key.NumPad3:
                    value = "3";
                    break;
                case Key.D4:
                case Key.NumPad4:
                    value = "4";
                    break;
                case Key.D5:
                case Key.NumPad5:
                    value = "5";
                    break;
                case Key.D6:
                case Key.NumPad6:
                    value = "6";
                    break;
                case Key.D7:
                case Key.NumPad7:
                    value = "7";
                    break;
                case Key.D8:
                case Key.NumPad8:
                    value = "8";
                    break;
                case Key.D9:
                case Key.NumPad9:
                    value = "9";
                    break;
            }
            return value;
        }
    }
}