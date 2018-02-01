using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeGenerator
{
    public class BarcodeGen
    {
        private string _basisNr = "343143199";
        //private string veNr;


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
    }
}
