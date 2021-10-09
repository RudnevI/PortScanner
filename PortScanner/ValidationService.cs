using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortScanner
{
    static class ValidationService
    {
        private static Regex _ipRegularExpression = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        public static bool IsIpStringValid(string ipString)
        {
            return _ipRegularExpression.IsMatch(ipString);
        }

        public static bool IsPortValid(string port)
        {
            return IsNumeric(port) && IsNotEmpty(port) && IsPortWithinPossibleRange(port)
                
                ;
        }

        private static bool IsNumeric(string value)
        {
            return new Regex(@"\d*").IsMatch(value);
        }

        private static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        

        private static bool IsPortWithinPossibleRange(string port)
        {
            try
            {
                int numericValue = int.Parse(port);
                return numericValue > 0 && numericValue <= 65535;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
