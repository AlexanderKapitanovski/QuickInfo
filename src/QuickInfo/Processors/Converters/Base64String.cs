using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static QuickInfo.NodeFactory;

namespace QuickInfo.Processors.Converters
{
    class Base64String : IProcessor
    {
        public object GetResult(Query query)
        {
            string base64String = query.OriginalInput;
            byte[] data = null;
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return null;
            }

            if (!Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
            {
                return null;
            }    

            try
            {
                data = System.Convert.FromBase64String(base64String);                            
            }
            catch (Exception)
            {
                return null;
            }
            if (data != null)
            {
                var pairs = new List<(string, string)>
                {
                ("UTF8:",  Encoding.UTF8.GetString(data)),
                ("Unicode:",  Encoding.Unicode.GetString(data)),
                ("ASCI:", Encoding.ASCII.GetString(data)),
                ("Byte[]:", BitConverter.ToString(data))
                };
                return NameValueTable(null, right => right.Style = "Fixed", pairs.ToArray());
            }
            return null;
        }

    }
}
