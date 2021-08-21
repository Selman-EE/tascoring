using System.Globalization;
using static System.String;

namespace Tascoring.UI.Extensions
{
    public static partial class String
    {
        public static string GetUserName(this string input)
        {
            // TODO: beymen pc de buraya ne geliyor kontrol et.
            //case0: SELMANEE\\See
            if (IsNullOrWhiteSpace(input))
                return Empty;

            if (input.Contains('\\') == false)
                return input.TitleCase();

            string[] usernames = input.Trim().Split('\\');
            return IsNullOrWhiteSpace(usernames[1]) ? Empty : usernames[1];
        }
        public static string FirstCharToUpper(this string input)
        {
            if (IsNullOrWhiteSpace(input))
                return Empty;

            char[] a = input.ToCharArray();
            a[0] = char.ToUpperInvariant(a[0]);
            return new string(a);
        }

        private static readonly CultureInfo _cultureInfo = new CultureInfo("tr-TR", false);
        public static string TitleCase(this string input)
        {
            if (IsNullOrWhiteSpace(input))
                return Empty;

            return _cultureInfo.TextInfo.ToTitleCase(input);
        }

        // TODO: burasi test edilecek 
        public static string CreateDisplayName(this string input)
        {
            //case0: SELMANEE\\See
            input = input.GetUserName();

            //case1: selman.ekici
            //case2: selman ekici
            //case3: selman 			
            if (input.Contains("."))
            {
                var arr = input.Split(".");
                if (arr.Length > 1)
                    return $"{arr[0].FirstCharToUpper()} {arr[1].FirstCharToUpper()}";
                if (arr.Length == 1)
                    return $"{arr[0].FirstCharToUpper()}";
            }
            if (input.Contains(" "))
            {
                var arr = input.Split(" ");
                if (arr.Length > 1)
                    return $"{arr[0].FirstCharToUpper()} {arr[1].FirstCharToUpper()}";
                if (arr.Length == 1)
                    return $"{arr[0].FirstCharToUpper()}";
            }
            return input.TitleCase();
        }
    }
}
