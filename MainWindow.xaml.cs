using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows11Calculator.Properties;

namespace Windows11Calculator
{
    public partial class MainWindow : Window
    {
        private double _accumulator = 0;
        private string? _pendingOp = null;
        private bool _resetDisplayOnNextDigit = true;
        private double _memory = 0;

        public MainWindow()
        {
            InitializeComponent();
            // select current language in combo
            try
            {
                var current = Thread.CurrentThread.CurrentUICulture.Name;
                for (int i = 0; i < LangCombo.Items.Count; i++)
                {
                    if (LangCombo.Items[i] is ComboBoxItem item && item.Tag is string tag && tag == current)
                    {
                        LangCombo.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch { }
            Display.Text = "0";
        }

        private void SetCulture(string cultureName)
        {
            App.SetCulture(cultureName);
            // Re-start the window to apply resource changes
            var newWindow = new MainWindow();
            newWindow.Left = this.Left;
            newWindow.Top = this.Top;
            newWindow.Show();
            this.Close();
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cb && cb.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                SetCulture(tag);
            }
        }

        private void AppendDigit(string d)
        {
            if (_resetDisplayOnNextDigit || Display.Text == "0")
            {
                Display.Text = d;
                _resetDisplayOnNextDigit = false;
            }
            else
            {
                Display.Text += d;
            }
        }

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b) AppendDigit(b.Content.ToString()!);
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (_resetDisplayOnNextDigit)
            {
                Display.Text = "0" + sep;
                _resetDisplayOnNextDigit = false;
            }
            else if (!Display.Text.Contains(sep))
            {
                Display.Text += sep;
            }
        }

        private void Op_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button b) return;
            double current = double.Parse(Display.Text, CultureInfo.CurrentCulture);
            if (_pendingOp != null)
            {
                Calculate(current);
            }
            else
            {
                _accumulator = current;
            }

            _pendingOp = b.Content.ToString();
            _resetDisplayOnNextDigit = true;
        }

        private void Calculate(double current)
        {
            try
            {
                switch (_pendingOp)
                {
                    case "+": _accumulator += current; break;
                    case "−": _accumulator -= current; break;
                    case "×": _accumulator *= current; break;
                    case "÷": _accumulator = current == 0 ? double.NaN : _accumulator / current; break;
                    case "%": _accumulator = _accumulator * current / 100.0; break;
                }
                Display.Text = _accumulator.ToString(CultureInfo.CurrentCulture);
            }
            catch
            {
                Display.Text = "NaN";
            }
            finally
            {
                _pendingOp = null;
            }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            double current = double.Parse(Display.Text, CultureInfo.CurrentCulture);
            if (_pendingOp != null)
            {
                Calculate(current);
                _resetDisplayOnNextDigit = true;
            }
        }

        private void CE_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            _resetDisplayOnNextDigit = true;
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            _accumulator = 0;
            _pendingOp = null;
            Display.Text = "0";
            _resetDisplayOnNextDigit = true;
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (!_resetDisplayOnNextDigit && Display.Text.Length > 0)
            {
                Display.Text = Display.Text.Length > 1 ? Display.Text[..^1] : "0";
            }
        }

        private void Negate_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
            {
                v = -v;
                Display.Text = v.ToString(CultureInfo.CurrentCulture);
            }
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
            {
                Display.Text = v < 0 ? "NaN" : Math.Sqrt(v).ToString(CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
            {
                Display.Text = (v * v).ToString(CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
        }

        private void Inverse_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
            {
                Display.Text = v == 0 ? "NaN" : (1.0 / v).ToString(CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
        }

        // Memory
        private void MC_Click(object sender, RoutedEventArgs e) => _memory = 0;
        private void MR_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = _memory.ToString(CultureInfo.CurrentCulture);
            _resetDisplayOnNextDigit = true;
        }
        private void MPlus_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
                _memory += v;
        }
        private void MMinus_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
                _memory -= v;
        }
        private void MS_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double v))
                _memory = v;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                AppendDigit(((int)(e.Key - Key.D0)).ToString());
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                AppendDigit(((int)(e.Key - Key.NumPad0)).ToString());
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                Decimal_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Add || (e.Key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.None))
            {
                _pendingOp = "+";
                _accumulator = double.Parse(Display.Text, CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                _pendingOp = "−";
                _accumulator = double.Parse(Display.Text, CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
            else if (e.Key == Key.Multiply)
            {
                _pendingOp = "×";
                _accumulator = double.Parse(Display.Text, CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
            else if (e.Key == Key.Divide)
            {
                _pendingOp = "÷";
                _accumulator = double.Parse(Display.Text, CultureInfo.CurrentCulture);
                _resetDisplayOnNextDigit = true;
            }
            else if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                Equals_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Back)
            {
                Backspace_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Escape)
            {
                C_Click(this, new RoutedEventArgs());
            }
        }
    }
}