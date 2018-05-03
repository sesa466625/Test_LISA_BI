using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security;
using System.Web;

namespace TestMultipleHeaderRowFile
{
    public class Class1
    {
        public class RootObject //This is the main object
        {
            public Status status { get; set; }
            public List<Datum> data { get; set; }
            public pagination pagination { get; set; }

        }
        public class Status //This is the jSON status
        {
            public bool error { get; set; }
            public int code { get; set; }
            public string type { get; set; }
            public string message { get; set; }
        }

        public class Datum //This is the actual data that is returned
        {
            public string access_token { get; set; }
            public string created_at { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
            public string token_type { get; set; }
            public int account_id { get; set; }
            public int id { get; set; }
            public string email { get; set; }
            public custom_attributes custom_attributes { get; set; }

        }
        public class pagination //This is for pagination of record sets that are greater than 50 records
        {
            public string next_link { get; set; }
            public string previous_link { get; set; }
            public string before_cursor { get; set; }
            public string after_cursor { get; set; }

        }
        public class custom_attributes //These will be your custom attributes, where you'd substitute customAttribute1, etc with your real custom attribute names
        {
            public string customAttribute1 { get; set; }
            public string customAttribute2 { get; set; }

        }

        static void Main(string[] args)
        {

            GenerateBOLPDF();

            string srcfile = @"C:\BI Integration\Sample SAP Files\SO-DROP-G5_20180106.txt";
            string destfile = @"C:\BI Integration\Sample SAP Files\SO-DROP-G5_20180106_processed.txt";
            int headerRowCount = 2;

            using (StreamReader sr = new StreamReader(srcfile))
            using (StreamWriter sw = new StreamWriter(destfile))
             {

                 while (!sr.EndOfStream)
                 {
                     StringBuilder sb = new StringBuilder();
                     for (int i = 0; i < headerRowCount; i++)
                     {
                         sb.Append(sr.ReadLine());
                     }
                     sw.WriteLine(sb.ToString());
                 }
             }
            //combileFile(file);

        }

        private static void GenerateBOLPDF()
        {
            try
            {
                var converter = new HtmlToPdf();

                // set authentication cookie
                converter.Options.Authentication.Username = "limepacificsupadmin";
                converter.Options.Authentication.Password = "Welcome1234";
                
                var doc = converter.ConvertUrl("http://bdnweb3/LIME/");
                //var doc = converter.ConvertUrl("http://fusion.sg.schneider-electric.com/");
                //doc.Save(System.Web.HttpContext.Current.Response, true, "test.pdf");
                doc.Save("C:\\Backup\\test.pdf");
                doc.Close();

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        static void combileFile(string file)
        {
            Regex parser = new Regex(@"GE\d*\:\d*\r\n(?<lines>(.*?\r\n){2,3})", RegexOptions.Singleline);

            string[] lines;

            lines = File.ReadAllLines(file);
            string[] paragraphs = parser.Matches(lines.ToString()).Cast<Match>().Select(T => Regex.Replace(T.Groups["lines"].Value, @"\t|\n|\r", string.Empty)).ToArray();

            //foreach(string line in lines)
            //{
            //    string[] paragraphs = parser.Matches(line).Cast<Match>().Select(T => Regex.Replace(T.Groups["line"].Value, @"\t|\n|\r", string.Empty)).ToArray();

            //}

        }
        
    }
   
}
