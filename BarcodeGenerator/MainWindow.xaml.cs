using System;
using System.Collections.Generic;
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

namespace BarcodeGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _basisNr = TbFirma.Text;
            int anz = Convert.ToInt16( TbAnz.Text);
            int veNr = Convert.ToInt32(TbLfdNr.Text);
            string codes = "";
            for (int i = 0; i < anz; i++)
            {
                codes += GenerateBarcode(String.Format("{0:00000000}",veNr)) +"\n";
                veNr++;
            }

            Textblck.Text = codes;
        }

        private string _basisNr;

        public string GenerateBarcode(string veNr)
        {
            string barcode = "00";
            
            string chkDigit;
            try
            {
                chkDigit = calculateCheckDigit(_basisNr + veNr);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
            return barcode + _basisNr + veNr + chkDigit;
        }

        private string calculateCheckDigit(string code)
        {
            if (code.Length == 17)
            {
                int sum = 0;
                //Alle stellen der Nummer werden von hinten nach vorne abwechselnd mit 3 und 1 multipliziert und summiert.
                for (int i = 16; i >= 0; i--)
                {
                    if (i % 2 == 0)
                    {
                        sum += Convert.ToInt16(code[i].ToString()) * 3;
                    }
                    else
                    {
                        sum += Convert.ToInt16(code[i].ToString());
                    }
                }
                //Die Prüfziffer ist die Differenz der Summe zur nächsten vollen Zehnerstelle
                int digit = 10;
                digit -= sum % 10;
                if (digit == 10)
                    digit = 0;
                return digit.ToString();
            }
            else
            {
                throw new Exception("Für einen SSCC-18 Barcode werden 17 Nutzziffern erwartet.");
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TbFirma_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
