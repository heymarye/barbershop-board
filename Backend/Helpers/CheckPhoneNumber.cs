using System;
using System.Text.RegularExpressions;

namespace Backend.Helpers
{
    public class CheckPhoneNumber
    {
        public const string motif = @"^([\+]?372[-]?|[0])?[1-9][0-9]{8}$";

        public static bool IsPhoneNbr(string number)
        {
            if (number != null) return Regex.IsMatch(number, motif);
            else return false;
        }
    }
}