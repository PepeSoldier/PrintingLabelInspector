using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace XLIB_COMMON.Model
{
    public static class StringFormatter
    {
        public static string Format(string DisplayFormat, string rawText)
        {
            List<int?> values = new List<int?>();
            List<string> decorators = new List<string>();

            if (DisplayFormat != null)
            {
                AnalyzeFormat(DisplayFormat, values, decorators);
                return PrepareFormattedText(rawText, values, decorators);
            }
            else
            {
                return rawText;
            }
                
        }
        private static void AnalyzeFormat(string DisplayFormat, List<int?> values, List<string> decorators)
        {
            int pointer = 0;
            string temp = string.Empty;
            

            while (pointer < DisplayFormat.Length)
            {
                if (DisplayFormat[pointer] == '[')
                {
                    decorators.Add(temp);
                    temp = string.Empty;
                }
                else if (DisplayFormat[pointer] == ']')
                {
                    if (temp.Length > 0)
                    {
                        values.Add(Convert.ToInt32(temp));
                        temp = string.Empty;
                    }
                    else
                    {
                        values.Add(null);
                    }
                }
                else
                {
                    temp += DisplayFormat[pointer];
                }
                pointer++;
            }
        }
        private static string PrepareFormattedText(string rawText, List<int?> values, List<string> decorators)
        {
            StringBuilder formattedText = new StringBuilder();
            
            int count = Math.Max(values.Count, decorators.Count);
            int pointer = 0;
            int charsCount = 0;

            for (int j = 0; j < count; j++)
            {
                charsCount = values[j] ?? 0;
                if (pointer + charsCount >= rawText.Length)
                {
                    charsCount = rawText.Length - pointer;
                }

                if (j < decorators.Count)
                    formattedText.Append(decorators[j]);

                if (j < values.Count && charsCount >= 0)
                    formattedText.Append(rawText.Substring(pointer, charsCount));

                pointer += charsCount;
            }

            return formattedText.ToString();
        }
        
        /// <summary>Function removes polish special characters (ą,ć,ę...) from string</summary>
        public static string RemoveDiacritics(this string s)
        {
            string asciiEquivalents = Encoding.ASCII.GetString(
                Encoding.GetEncoding("Cyrillic").GetBytes(s)
            );

            return asciiEquivalents;
        }
    }
}