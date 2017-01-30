using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Text.RegularExpressions;



namespace FCCULSDataGrabber
{
    public partial class Form1 : Form
    {
        private List<List<string>> DataStringLists = new List<List<string>>();
        private List<List<string>> OutputDataStringLists = new List<List<string>>();
        private List<int> OutputDataMapping = new List<int>();
        private List<string> AdvanceData = new List<string>();
        private bool AdvancedSearchNeed = false;
        private string SaveFilePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = -1;
            string FileName = "";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                FileName = openFileDialog1.FileName;

                try
                {
                    ReadINCSVFile(FileName);
                }
                catch (IOException)
                {
                    //Todo: Throw an Error Here
                }
            }


        }
        private void ReadINCSVFile(string FilePath)
        {
            var reader = new StreamReader(File.OpenRead(FilePath));
            bool FirstLine = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                //Generate The Lists Based on How Many Columns we Have In The CSV File
                for (int i = 0; i < values.Length; i++)
                {
                    if (FirstLine)
                    {
                        List<string> list = new List<string>();
                        list.Add(values[i]); // The First Element in Each String Will The Name of Column
                        DataStringLists.Add(list);

                    } else
                    {
                        DataStringLists[i].Add(values[i]);
                    }
                }
                FirstLine = false;

            }
            GenerateMatchingOptions();

        }
        private void GenerateMatchingOptions()
        {


            LicFirstNameOptions.Items.Add(new CustomComboboxItem("None", -1));
            LicLastNameOptions.Items.Add(new CustomComboboxItem("None", -1));
            CallSignOptions.Items.Add(new CustomComboboxItem("None", -1));
            LicFRNOptions.Items.Add(new CustomComboboxItem("None", -1));




            for (int i = 0; i < DataStringLists.Count(); i++)
            {
                LicFirstNameOptions.Items.Add(new CustomComboboxItem(DataStringLists[i][0], i));
                LicLastNameOptions.Items.Add(new CustomComboboxItem(DataStringLists[i][0], i));
                CallSignOptions.Items.Add(new CustomComboboxItem(DataStringLists[i][0], i));
                LicFRNOptions.Items.Add(new CustomComboboxItem(DataStringLists[i][0], i));
            }
            

            LicFirstNameOptions.DisplayMember = "Text";
            LicFirstNameOptions.ValueMember = "Value";

            LicLastNameOptions.DisplayMember = "Text";
            LicLastNameOptions.ValueMember = "Value";

            CallSignOptions.DisplayMember = "Text";
            CallSignOptions.ValueMember = "Value";

            LicFRNOptions.DisplayMember = "Text";
            LicFRNOptions.ValueMember = "Value";

            LicFirstNameOptions.SelectedIndex = 0;
            LicLastNameOptions.SelectedIndex = 0;
            CallSignOptions.SelectedIndex = 0;
            LicFRNOptions.SelectedIndex = 0;
        }

        private void GenerateCSVFile()
        {
            if (!AreFieldsMapped())
            {
                MessageBox.Show("Please Map Fields Before Generating The CSV File");
                return;
            }
            if (!AreOptionsSelected())
            {
                MessageBox.Show("Please Select Options To Generate The CSV File");
                return;
            }

            //Based on The Check Boxes Selected Generate More Lists To Store The Data

            AdvancedSearchNeed = false;
            for (int u = 0; u < OutputOptions.CheckedItems.Count; u++)
            {
                List<string> list = new List<string>();
                list.Add(OutputOptions.CheckedItems[u].ToString());
                OutputDataStringLists.Add(list);
                if (OutputOptions.CheckedIndices[u] > 6)
                {
                    AdvancedSearchNeed = true;

                }
            }



            //Use The FCC ULS API To Get The Data We Need
            //Search First By Call Sign or FRN Then By Name
            bool DataFound = false;
            for (int x = 1; x < DataStringLists[0].Count; x++) //Loops Through The Rows In The CSV File
            {

                //Try and Search By Call Sign First
                //Checks T0 Make Sure We Have a Mapped Column To Callsign and The Row Isnt Blank

                CustomComboboxItem SelectCallSignMatchItem = (CustomComboboxItem)CallSignOptions.SelectedItem;
                if (!DataFound && SelectCallSignMatchItem.Value != -1 && !DataStringLists[SelectCallSignMatchItem.Value][x].Equals(""))
                {
                    CurrentlySearchingFor.Text = "Searching: " + DataStringLists[SelectCallSignMatchItem.Value][x].ToString();
                    //string url = "http://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=" + DataStringLists[SelectCallSignMatchItem.Value][x] + "&format=json&pageSize=1000";
                    string url = "http://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=N8AM&format=json";
                    Console.Write("Testing");
                    string text = GetDataReponseFromFCCAPI(url);
                    dynamic stuff = JsonConvert.DeserializeObject(text);
                    //MessageBox.Show(DataStringLists[2][x]);
                    Console.WriteLine(DataStringLists[2][x]);


                    //Check To See If We Recieved An Error Meaning No Matches Found
                    if (!stuff.status.Value.Equals("OK"))
                    {
                        //TODO: Handle Error Here
                        Console.Write("Error");
                    } else
                    {
                        //We Found Data. We Could Have Mulitple Results Returned Here
                        DataFound = true;
                        int NumOfResultReturned = stuff.Licenses.totalRows;
                        Console.Write(NumOfResultReturned);
                        if (NumOfResultReturned > 1)
                        {
                            //We Have Multiple Results
                            List<dynamic> ReturnedData = SearchInMultipleItems(stuff, DataStringLists[SelectCallSignMatchItem.Value][x], "CallSign"); //Find The Correct Element
                            for (int s = 0; s < ReturnedData.Count; s++)
                            {
                                ParseDataToLists(ReturnedData[s]);
                            }
                        }
                        else
                        {
                            //We Only Have One Result
                            dynamic LicenseData = stuff.Licenses.License.First;
                            ParseDataToLists(LicenseData);
                        }
                    }
                }

                //Try To Search By FRN 
                CustomComboboxItem SelectFRNMatchItem = (CustomComboboxItem)LicFRNOptions.SelectedItem;
                if (!DataFound && SelectFRNMatchItem.Value != -1 && !DataStringLists[SelectFRNMatchItem.Value][x].Equals(""))
                {
                    CurrentlySearchingFor.Text = "Searching For: " + DataStringLists[SelectFRNMatchItem.Value][x].ToString();
                    string url = "http://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=" + DataStringLists[SelectFRNMatchItem.Value][x] + "&format=json&pageSize=1000";
                    //string url = "http://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=0025995382&format=json";
                    string text = GetDataReponseFromFCCAPI(url);
                    dynamic stuff = JsonConvert.DeserializeObject(text);

                    //Check To See If We Recieved An Error Meaning No Matches Found
                    if (!stuff.status.Value.Equals("OK"))
                    {
                        //TODO: Handle Error Here
                    }
                    else
                    {
                        //We Found Data. We Could Have Mulitple Results Returned Here
                        DataFound = true;
                        int NumOfResultReturned = stuff.Licenses.totalRows;

                        if (NumOfResultReturned > 1)
                        {
                            //We Have Multiple Results
                            int indexOfFRNNumbersInDataString = -1; //TODO: Handle this error
                            foreach (var innerList in DataStringLists)
                            {
                                if (innerList[0].Equals("FRN"))
                                {
                                    indexOfFRNNumbersInDataString = DataStringLists.IndexOf(innerList);
                                }
                            }

                            List<dynamic> ReturnedData = SearchInMultipleItems(stuff, DataStringLists[SelectFRNMatchItem.Value][x], "FRN"); //Find The Correct Element
                            for (int s = 0; s < ReturnedData.Count; s++)
                            {
                                ParseDataToLists(ReturnedData[s]);
                            }
                        }
                        else
                        {
                            dynamic LicenseData = stuff.Licenses.License.First;
                            ParseDataToLists(LicenseData);
                            //Console.Write(OutputDataStringLists[0][1]);
                            //Console.Write(OutputDataStringLists[1][1]);
                            //Console.Write(OutputDataStringLists[2][1]);
                            //Console.Write(OutputDataStringLists[3][1]);
                            //Console.Write(OutputDataStringLists[4][1]);
                            //Console.Write(OutputDataStringLists[5][1]);
                            ///Console.Write(OutputDataStringLists[6][1]);
                            //Console.Write(OutputDataStringLists[7][1]);
                        }
                    }
                }
                //Try and Seach by Name
                /*
                TODO: Get This Working
                if (!DataFound && !LicFRNOptions.SelectedValue.Equals(-1) && !DataStringLists[Int32.Parse(LicFRNOptions.SelectedValue.ToString())][x].Equals(""))
                {
                    string url = "http://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=Ringer,%20Darrell&format=jsonp";
                    string text = GetJSONReponseFromFCCAPI(url);
                    DataFound = true;
                }
                */
                if (!DataFound)
                {
                    //TODO: Log This Error
                }
                DataFound = false;


            }

            //We Have Got Data On The Whole Input File. Print Out The Results
            PrintDataToFile();




            /*
            string url = "http://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=Ringer,%20Darrell&format=json";
            string text = GetDataReponseFromFCCAPI(url);
            MessageBox.Show(text);
            dynamic stuff = JsonConvert.DeserializeObject(text);

            MessageBox.Show(stuff.Licenses.License.First.licName.Value);
            */





        }

        private bool AreOptionsSelected()
        {
            if(OutputOptions.SelectedItems.Count > 0)
            {
                return true;
            } else
            {
                return false;
            }
                
        }

        private bool AreFieldsMapped()
        {
            CustomComboboxItem CallSignItem = (CustomComboboxItem)CallSignOptions.SelectedItem;
            CustomComboboxItem LicFRNItem = (CustomComboboxItem)LicFRNOptions.SelectedItem;

            if (CallSignItem.Text.Equals("None") && LicFRNItem.Text.Equals("None"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string GetDataReponseFromFCCAPI(string URL)
        {
            var request = WebRequest.Create(URL);
            request.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            return text;
        }
        private void ParseDataToLists(dynamic LicenseData)
        {
            string licName = LicenseData.licName.Value;
            string frn = LicenseData.frn.Value;
            string callsign = LicenseData.callsign.Value;
            string status = LicenseData.statusDesc.Value;
            string ExperationDate = LicenseData.expiredDate.Value;
            string ID = LicenseData.licenseID.Value;
            string URL = LicenseData.licDetailURL.Value;

            //Advanced Search Items
            string RadioService = "";
            string Status = "";
            string Grant = "";
            string Effective = "";
            string Expiration = "";
            string Cacellation = "";
            string Address = "";
            string OperatorClass = "";
            string Group = "";


            if (AdvancedSearchNeed)
            {
                //Get Advaced Search Data
                GetExtraData(URL);
                
                RadioService = AdvanceData[0];
                Status = AdvanceData[1];
                Grant = AdvanceData[2];
                Effective = AdvanceData[3];
                Expiration = AdvanceData[4];
                Cacellation = AdvanceData[5];
                Address = AdvanceData[6];
                OperatorClass = AdvanceData[7];
                Group = AdvanceData[8];

            }
           




            for (int y = 0; y < OutputOptions.CheckedItems.Count; y++)
            {
                if (OutputOptions.CheckedItems[y].ToString().Equals("License Name"))
                {
                    OutputDataStringLists[y].Add(licName);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("FRN"))
                {
                    OutputDataStringLists[y].Add(frn);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Call Sign"))
                {
                    OutputDataStringLists[y].Add(callsign);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Status"))
                {
                    OutputDataStringLists[y].Add(status);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Experation Date"))
                {
                    OutputDataStringLists[y].Add(ExperationDate);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("License ID"))
                {
                    OutputDataStringLists[y].Add(ID);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("License URL Page"))
                {
                    OutputDataStringLists[y].Add(URL);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("License Grant Date"))
                {
                    OutputDataStringLists[y].Add(Grant);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("License Effective Date"))
                {
                    OutputDataStringLists[y].Add(Effective);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("License Cancellation Date"))
                {
                    OutputDataStringLists[y].Add(Cacellation);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Licensee Address"))
                {
                    OutputDataStringLists[y].Add(Address);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Operator Class"))
                {
                    OutputDataStringLists[y].Add(OperatorClass);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Group"))
                {
                    OutputDataStringLists[y].Add(Group);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Radio Service"))
                {
                    OutputDataStringLists[y].Add(RadioService);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("Status"))
                {
                    OutputDataStringLists[y].Add(Status);
                }
                else if (OutputOptions.CheckedItems[y].ToString().Equals("License Expiration Date"))
                {
                    OutputDataStringLists[y].Add(Expiration);
                }

            }

            AdvanceData = new List<string>();
        }
        private List<dynamic> SearchInMultipleItems(dynamic JSONResponse, string SearchValue, string SearchType)
        {
            
            dynamic LicenseData = JSONResponse.Licenses.License;
            int Count = JSONResponse.Licenses.totalRows;
            List<dynamic> FoundResults = new List<dynamic>();
            if (LicenseData != null)
            {
                for (int u = 0; u < Count; u++)
                {  
                //Keep Looping Until We dont have anymore search results

                if (SearchType.Equals("CallSign"))
                {
                       
                    if (LicenseData[u].callsign.Value.ToString().Trim().Equals(SearchValue.Trim()))
                    {
                        //Value Found Add To The List
                        FoundResults.Add(LicenseData[u]);
                    }
                       
                }
                else if (SearchType.Equals("FRN"))
                {
                    //We Are Searching Via a FRN
                    if (LicenseData[u].frn.Value.ToString().Trim().Equals(SearchValue.Trim()))
                    {
                        //Value Found Add To The List
                        FoundResults.Add(LicenseData[u]);
                    }

                }
                else if (SearchType.Equals("Name"))
                {
                    //We Are Searching Via a Name
                    if (LicenseData[u].licName.Value.ToString().Trim().Equals(SearchValue.Trim()))
                    {
                        //Value Found Add To The List
                        FoundResults.Add(LicenseData[u]);
                    }
                }
                
            }
        }

            return FoundResults;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GenerateCSVFile();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void PrintDataToFile()
        {

            //string filePath = @"C:\Users\Oliver\Desktop\test3.csv";
            string filePath = SaveFilePath;
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
           
          
            for (int y = 0; y < OutputDataStringLists[0].Count; y++) { 
                 for (int x = 0; x < OutputDataStringLists.Count; x++) //Control The Rows
                 {
                    if (OutputDataStringLists[x][y].Contains(",")){
                        sb.Append("\""+OutputDataStringLists[x][y]+"\",");
                    }
                    else
                    {
                        sb.Append(OutputDataStringLists[x][y] + ",");
                    }
                        
                    //Console.Write(OutputDataStringLists[x][y]+",");
                 }
                //Console.WriteLine();
                sb.AppendLine("");
            }
            
           File.WriteAllText(filePath, sb.ToString());

            //Reset Global Vars
            OutputDataStringLists = new List<List<string>>();
            OutputDataMapping = new List<int>();
            AdvanceData = new List<string>();
            AdvancedSearchNeed = false;



        CurrentlySearchingFor.Text = "Not Running";
        }

        private void GetExtraData(string URL)
        {
            WebClient webClient = new WebClient();
            string page = webClient.DownloadString(URL);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            //doc.DocumentNode.SelectSingleNode("//table[@summary='License detail table']").InnerHtml;
            //List<String> LicenseDetails = doc.DocumentNode.SelectSingleNode("//table[@summary='License detail table']/tr").Descendants("td").Select(td=> td.InnerText.Replace("&nbsp;", "").Trim()).ToList();


            AdvanceData.Add(doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[2]/td/table/tr[1]/td[4]").InnerText.Replace("&nbsp;", "").Trim());
            AdvanceData.Add(doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[2]/td/table/tr[2]/td[2]").InnerText.Replace("&nbsp;", "").Trim());
            AdvanceData.Add(doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[2]/td/table/tr[4]/td[2]").InnerText.Replace("&nbsp;", "").Trim());
            AdvanceData.Add(doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[2]/td/table/tr[5]/td[2]").InnerText.Replace("&nbsp;", "").Trim());
            AdvanceData.Add(doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[2]/td/table/tr[4]/td[4]").InnerText.Replace("&nbsp;", "").Trim());
            AdvanceData.Add(doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[2]/td/table/tr[5]/td[4]").InnerText.Replace("&nbsp;", "").Trim());
            string Address = doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[4]/td/table/tr[3]/td[1]").InnerText.Replace("&nbsp;", "").Trim();
            Address = Regex.Replace(Address, "<!--Addresses are displayed on successive lines:\\s*Licensee\\s*PO Box\\s*Address1\\s*Address2\\s*City, State  ZIP\\s*ATTN\\s*If a particular field is not available, then that line is omitted.  In the case of City, state and zip, then just that field is omitted.\\s*-->", "").Trim();
            AdvanceData.Add(Address);
            string OperatorClass = doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[6]/td/table/tr[1]/td[2]").InnerText.Replace("&nbsp;", "").Trim();
            OperatorClass = Regex.Replace(OperatorClass, "<!--Example: Amateur Extra -->", "").Trim();
            AdvanceData.Add(OperatorClass);
            string OperatorGroup = doc.DocumentNode.SelectSingleNode("html/body/html/table[4]/tr/td[2]/div/table[2]/tr[6]/td/table/tr[2]/td[2]").InnerText.Replace("&nbsp;", "").Trim();
            OperatorGroup =  Regex.Replace(OperatorGroup, "<!--Example: C -->", "").Trim();
            AdvanceData.Add(OperatorGroup);
            /*
            Console.Write(AdvanceData[0]);
            Console.Write(AdvanceData[1]);
            Console.Write(AdvanceData[2]);
            Console.Write(AdvanceData[3]);
            Console.Write(AdvanceData[4]);
            Console.Write(AdvanceData[5]);
            Console.Write(AdvanceData[6]);
            Console.Write(AdvanceData[7]);
            Console.Write(AdvanceData[8]);
            */

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetExtraData("Testing");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = @"C:\";

            saveFileDialog1.Title = "Save Your CSV File";

            saveFileDialog1.CheckFileExists = false;

            saveFileDialog1.CheckPathExists = true;

            saveFileDialog1.DefaultExt = "csv";

            saveFileDialog1.Filter = "All files (*.*)|*.*";

            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)

            {

                SaveFilePath = saveFileDialog1.FileName.Split('.')[0] + ".csv";
                FilePathTextBox.Text = SaveFilePath;

            }
        }
    }

   

}
public class CustomComboboxItem
{
    private string text { get; set; }
    private int value { get; set; }

    public CustomComboboxItem(string StrText, int IntValue)
    {
        this.text = StrText;
        this.value = IntValue;

    }
    public string Text
    {
        get { return text; }
    }

    public int Value
    {
        get { return value; }
    }

}
