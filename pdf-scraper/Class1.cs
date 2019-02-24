using System;
using System.IO;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace citycrime
{
    class Class1
    {
        private string pdfDirectory;
        private string csvDirectory;
        private int year;
        private int month;
        private DataSet ds;
        private DataTable dt;
        private Dictionary<CrimeCodes, float> CodeLookup;

        public Class1()
        {
            //pdfSourceFile = @"D:\Downloads\jan-2017-rd.pdf";
            pdfDirectory = @"/Users/bol/dotnet/citycrime/pdf/";
            csvDirectory = @"/Users/bol/dotnet/citycrime/csv/";
            CodeLookup = new Dictionary<CrimeCodes, float>();
            ds = new DataSet();
            dt = new DataTable();
        }

        public void Go()
        {
            ReadDir();
        }

        private void ReadDir()
        {
            if (!Directory.Exists(pdfDirectory))
                return;

            SetDataSetColumns();

            foreach (string file in Directory.EnumerateFiles(pdfDirectory, "*.pdf"))
            {
                ReadPdf(file);
            }
        }

        private void ReadPdf(String pdfSourceFile)
        {
            PdfReader pdfReader = new PdfReader(pdfSourceFile);

            //get column x-locations on page 1
            var strategy = new MyLocationTextExtractionStrategy();
            var pageOneText = PdfTextExtractor.GetTextFromPage(pdfReader, 1, strategy);
            GetColumnXPoints(strategy);

            //loop thru grid data
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                strategy = new MyLocationTextExtractionStrategy();
                var allPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                var isContinue = FillDataSet(strategy);
                if (!isContinue)
                    break;
            }
            pdfReader.Close();

            var csv = ConvertDataSetToCSV();
            WriteFile("longbeach_crime.csv", csv);
        }

        private void GetColumnXPoints(MyLocationTextExtractionStrategy strategy)
        {
            CodeLookup.Clear();
            
            foreach(MyLocationTextExtractionStrategy.TextChunk chunk in strategy.locationalResult)
            {
                string text = chunk.text.Trim().Replace(" ", "_");
                //extract year/month
                if (text.StartsWith("REPORT_MONTH") && text.IndexOf('=') > -1 && text.IndexOf('/') > -1 && text.IndexOf('-') > -1)
                {
                    //"REPORT_MONTH_=_01/01/2017_-_01/31/2017"
                    var str1 = text.Replace("_", "").Split('=')[1];
                    //"01/01/2017-01/31/2017"
                    var monthStr = str1.Split('/')[0];
                    var yearStr = str1.Split('/')[2].Split('-')[0];
                    Int32.TryParse(monthStr, out month);
                    Int32.TryParse(yearStr, out year);
                    continue;
                }
                CrimeCodes crimeCode;
                if (Enum.TryParse<CrimeCodes>(text, true, out crimeCode) && Enum.IsDefined(typeof(CrimeCodes), text))
                {
                    //special considerations
                    if (text.Equals("AUTO") && !CodeLookup.ContainsKey(CrimeCodes.BIKE))
                        continue;
                    else if (text.Equals("TOTAL") && !CodeLookup.ContainsKey(CrimeCodes.PART_2))
                        continue;
                    //Add enum and float to Lookup List
                    if (!CodeLookup.ContainsKey(crimeCode))
                        CodeLookup.Add(crimeCode, chunk.startLocation[0].Truncate(0));
                }
            }
            //order columns by x location
            CodeLookup = CodeLookup.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        private void SetDataSetColumns()
        {
            dt.Columns.Add("Year", typeof(int));
            dt.Columns.Add("Month", typeof(int));
            foreach (string name in Enum.GetNames(typeof(CrimeCodes)))
            {
                if (name.Equals("NONE"))
                    continue;
                dt.Columns.Add(name, (name.Equals("RD") ? typeof(string) : typeof(int)));
            }
            ds.Tables.Add(dt);
        }

        private bool FillDataSet(MyLocationTextExtractionStrategy strategy)
        {
            var isContinue = true;
            var row = new Dictionary<CrimeCodes, int>();
            var shouldStart = false;
            foreach (MyLocationTextExtractionStrategy.TextChunk chunk in strategy.locationalResult)
            {
                try
                {
                    //get current text (should be string of integer)
                    var text = chunk.text.Trim();
                    //time to end
                    if (text.Equals("SHPD") || text.Equals("Unk"))
                    {
                        //if the end is reached we don't want to iterate more pages in ReadPdf()
                        isContinue = false;
                        break;
                    }
                    //loop until first 3 digit region code appears
                    if (text.Length == 3)
                        shouldStart = true;
                    //skip over first few rows of header text column names
                    if (!shouldStart)
                        continue;
                    //if text is 3 digit region code, write row and clear for new row
                    if (text.Length == 3)
                        WriteRow(ref row);
                    //convert to integer, default to zero
                    var count = 0;
                    if (!Int32.TryParse(text, out count))
                        continue;
                    //get x location of current text
                    float xloc = chunk.startLocation[0];
                    //get correct columnt o use given the text's x location
                    var crimeCode = GetCrimeCode(xloc);
                    row.Add(crimeCode, count);
                    Console.WriteLine(text);
                }
                catch(Exception ex)
                {
                    Console.Write($"Error: {chunk.text}, {ex.Message}");
                }
            }
            WriteRow(ref row);
            return isContinue;
        }

        private CrimeCodes GetCrimeCode(float xloc)
        {
            CrimeCodes curCode = CrimeCodes.NONE;
            foreach (KeyValuePair<CrimeCodes, float> item in CodeLookup)
            {
                if (xloc > item.Value)
                    curCode = item.Key;
                else
                    return curCode;
            }
            return curCode;
        }

        private void WriteRow(ref Dictionary<CrimeCodes, int> row)
        {
            //if text is 3 digit region code, write row to dataset and clear for new row
            if (!row.Any())
                return;
            if (!row.ContainsKey(CrimeCodes.RD) || String.IsNullOrEmpty(Convert.ToString(row[CrimeCodes.RD])))
            {
                row.Clear();
                Console.WriteLine("");
                return;
            }
            //write row
            var r1 = dt.NewRow();
            r1[0] = year;
            r1[1] = month;
            r1[2] = (row.ContainsKey(CrimeCodes.RD) ? Convert.ToString(row[CrimeCodes.RD]) : string.Empty); //Region
            r1[3] = (row.ContainsKey(CrimeCodes.MURDER) ? row[CrimeCodes.MURDER] : 0); //Murder
            r1[4] = (row.ContainsKey(CrimeCodes.MANSLTR) ? row[CrimeCodes.MANSLTR] : 0); //Manslaughter
            r1[5] = (row.ContainsKey(CrimeCodes.RAPE) ? row[CrimeCodes.RAPE] : 0); //Rape
            r1[6] = (row.ContainsKey(CrimeCodes.ROBBERY) ? row[CrimeCodes.ROBBERY] : 0); //Robbery
            r1[7] = (row.ContainsKey(CrimeCodes.ASSAULT) ? row[CrimeCodes.ASSAULT] : 0); //Aggravated Assault
            r1[8] = (row.ContainsKey(CrimeCodes.RES) ? row[CrimeCodes.RES] : 0); //Residential Burglary
            r1[9] = (row.ContainsKey(CrimeCodes.COM) ? row[CrimeCodes.COM] : 0); //Commerical Burglary
            r1[10] = (row.ContainsKey(CrimeCodes.BURG) ? row[CrimeCodes.BURG] : 0); //Auto Bruglary
            r1[11] = (row.ContainsKey(CrimeCodes.GRAND) ? row[CrimeCodes.GRAND] : 0); //Grand Theft;
            r1[12] = (row.ContainsKey(CrimeCodes.PETTY) ? row[CrimeCodes.PETTY] : 0); //Petty Theft
            r1[13] = (row.ContainsKey(CrimeCodes.BIKE) ? row[CrimeCodes.BIKE] : 0); //Bike Theft 
            r1[14] = (row.ContainsKey(CrimeCodes.AUTO) ? row[CrimeCodes.AUTO] : 0); //Auto Theft
            r1[15] = (row.ContainsKey(CrimeCodes.ARSON) ? row[CrimeCodes.ARSON] : 0); //Arson
            r1[16] = (row.ContainsKey(CrimeCodes.PART_1) ? row[CrimeCodes.PART_1] : 0); //Totals Part 1
            r1[17] = (row.ContainsKey(CrimeCodes.PART_2) ? row[CrimeCodes.PART_2] : 0); //Totals Part 2
            r1[18] = (row.ContainsKey(CrimeCodes.TOTAL) ? row[CrimeCodes.TOTAL] : 0); //Grand Totals
            dt.Rows.Add(r1);
            //empty list
            row.Clear();
        }
        
        private string ConvertDataSetToCSV()
        {
            if (!Directory.Exists(csvDirectory))
                Directory.CreateDirectory(csvDirectory);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            var index = 1;
            var content = new StringBuilder();
            var intColumnCount = ds.Tables[0].Rows[0].Table.Columns.Count;

            //add column names
            foreach (DataColumn item in ds.Tables[0].Rows[0].Table.Columns)
            {
                content.Append(String.Format("\"{0}\"", item.ColumnName));
                if (index < intColumnCount)
                    content.Append(",");
                else
                    content.Append("\r\n");
                index++;
            }

            //add column data
            foreach (DataRow currentRow in ds.Tables[0].Rows)
            {
                var strRow = string.Empty;
                for (int y = 0; y <= intColumnCount - 1; y++)
                {
                    strRow += "\"" + currentRow[y].ToString() + "\"";

                    if (y < intColumnCount - 1 && y >= 0)
                        strRow += ",";
                }
                content.Append(strRow + "\r\n");
            }

            return content.ToString();
        }

        private void WriteFile(String filename, String data)
        {
            if (String.IsNullOrEmpty(data))
                return;
            File.WriteAllText($"{csvDirectory.TrimEnd('/')}/{filename}", data);
        }
    }

    enum CrimeCodes
    {
        RD,         //region id
        MURDER,
        MANSLTR,
        RAPE,
        ROBBERY,   
        ASSAULT,    //Aggravated Assault
        RES,        //Residential Burglary
        COM,        //Commerical Burglary
        BURG,       //Auto Bruglary
        GRAND,      //Grand Theft           BAD Y
        PETTY,      //Petty Theft           BAD Y
        BIKE,       //Bike Theft            BAD Y
        AUTO,       //Auto Theft            BAD Y, DUPLICATE
        ARSON,
        PART_1,     //Totals Part 1         THESE NUMBERS ARE BOGUS
        PART_2,     //Totals Part 2         THESE NUMBERS ARE BOGUS
        TOTAL,      //Grand Totals          THESE NUMBERS ARE BOGUS, DUPLICATE
        NONE
    }
}
