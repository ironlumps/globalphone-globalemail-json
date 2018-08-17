using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Diagnostics;


namespace GlobalCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnDont_Click(object sender, EventArgs e)
        {
            float errorCount = 0;
            float totalCounter = 0;

            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "C# Corner Open File Dialog";

            fdlg.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            fdlg.Filter = "All files (*.*)|*.*|CSV (*.csv)|*.csv";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            string results = fdlg.FileName.Replace(".txt", "Results.txt");
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GlobalEmail(fdlg);
                    GlobalPhone(fdlg);
                    Application.Exit();
                }
                catch(Exception ex)
                {
                    StreamWriter result = new StreamWriter(results, true);
                    errorCount++;
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                    result.Close();
                    result.Dispose();
                }
            }
        }
        public void GlobalEmail(OpenFileDialog fdlg)
        {
            String email = "";
            String phoneNumber = "";
            String customerID = txtCustomerID.Text;
            //String country = "United States";
            String fullName = "";
            float es01Counter = 0;
            float es02Counter = 0;
            float es03Counter = 0;
            float es04Counter = 0;
            float es07Counter = 0;
            float es08Counter = 0;
            float es09Counter = 0;
            float totalCounter = 0;
            float errorCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fdlg.FileName))
                {
                    // change globaloutput to just output when finished testing
                    string outFile = fdlg.FileName.Replace(".txt", "NewOutput.html");

                    //change true to false when finished testing
                    StreamWriter sw = new StreamWriter(outFile, true);
                    string results = fdlg.FileName.Replace(".txt", "Results.txt");


                    HttpWebRequest emailRequest;
                 
                    WebResponse emailResponse;

                    StreamReader emailSR;

                    string temp = sr.ReadLine();

                    while ((temp = sr.ReadLine()) != null)
                    {
                        //sr.ReadLine();

                        string[] split = temp.Split('\t');
                        email = split[0];
                        phoneNumber = split[1];
                        fullName = split[3] + split[2];


                        string emailJSON = $"https://globalemail.melissadata.net/V3/WEB/GlobalEmail/doGlobalEmail? &id={customerID}" +
                            $"&opt={"VERIFYMAILBOX:Premium"}&email={email}&format=json";

                        emailRequest = (HttpWebRequest)WebRequest.Create(emailJSON);


                        emailRequest.Method = "GET";
                        //  emailRequest.ContentLength = emailJSON.Length;
                        emailRequest.Accept = "application/json";

                        try
                        {
                            ///
                            emailResponse = emailRequest.GetResponse();
                            ///


                            emailSR = new StreamReader(emailResponse.GetResponseStream());
                            //  StreamReader emailRequestSR = new StreamReader(emailRequest.GetRequestStream());

                            // string emailJSONRequest = emailRequestSR.ReadToEnd();
                            string emailJSONResponse = emailSR.ReadToEnd();

                            emailSR.Close();
                            emailSR.Dispose();

                            try
                            {
                                Email.Rootobject emailRO = JsonConvert.DeserializeObject<Email.Rootobject>(emailJSONResponse);
                                // Email.Rootobject emailRequestRO = JsonConvert.DeserializeObject<Email.Rootobject>(emailJSONRequest);

                                // Email.Record[] emailReqRecords = emailRequestRO.Records;
                                Email.Record[] emailRecords = emailRO.Records;


                                // sw.WriteLine(emailRecords[0].EmailAddress + " " + phoneRecords[0].PhoneNumber + " "+ emailRecords[0].Results + " " +  phoneRecords[0].Results);
                                if (emailRecords[0].Results.Contains("ES01"))
                                {

                                    es01Counter++;
                                }
                                if (emailRecords[0].Results.Contains("ES02"))
                                {
                                    es02Counter++;
                                }
                                if (emailRecords[0].Results.Contains("ES03"))
                                {
                                    es03Counter++;
                                }
                                if (emailRecords[0].Results.Contains("ES04"))
                                {
                                    es04Counter++;
                                }
                                if (emailRecords[0].Results.Contains("ES07"))
                                {
                                    es07Counter++;
                                }
                                if (emailRecords[0].Results.Contains("ES08"))
                                {
                                    es08Counter++;
                                }
                                if (emailRecords[0].Results.Contains("ES09"))
                                {
                                    es09Counter++;
                                }
                                totalCounter++;
                            }
                            catch (Exception ex)
                            {
                                errorCount++;
                                StreamWriter result = new StreamWriter(results, true);
                                //result.WriteLine("Error Count: " + errorCount + " Error Type:  " + ex.Message + " Total Count: " + totalCounter);
                                // Get stack trace for the exception with source file information
                                var st = new StackTrace(ex, true);
                                // Get the top stack frame
                                var frame = st.GetFrame(0);
                                // Get the line number from the stack frame
                                var line = frame.GetFileLineNumber();
                                result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                                result.Close();
                                result.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            StreamWriter result = new StreamWriter(results, true);
                            errorCount++;
                            // Get stack trace for the exception with source file information
                            var st = new StackTrace(ex, true);
                            // Get the top stack frame
                            var frame = st.GetFrame(0);
                            // Get the line number from the stack frame
                            var line = frame.GetFileLineNumber();
                            result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                            result.Close();
                            result.Dispose();
                        }

                    }
                    // Outside oF While Loop
                    DataTable emailTable = new DataTable();

                    emailTable.Columns.Add("Email Codes", typeof(string));
                    emailTable.Columns.Add("Count", typeof(string));
                    emailTable.Columns.Add("Percentage", typeof(string));

                    emailTable.Rows.Add("ES01", es01Counter, String.Format("{0:P2}", es01Counter / totalCounter));
                    emailTable.Rows.Add("ES02", es02Counter, String.Format("{0:P2}", es02Counter / totalCounter));
                    emailTable.Rows.Add("ES03", es03Counter, String.Format("{0:P2}", es03Counter / totalCounter));
                    emailTable.Rows.Add("ES04", es04Counter, String.Format("{0:P2}", es04Counter / totalCounter));
                    

                    string emailResults = ConvertDataTableToHTML(emailTable);

                    sw.Write(emailResults);
                    sw.WriteLine();

                    sw.Close();
                    sw.Dispose();
                    sr.Close();
                    sr.Dispose();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void GlobalPhone(OpenFileDialog fdlg)
        {
            String email = "";
            String phoneNumber = "";
            String customerID = txtCustomerID.Text;
            //String country = "United States";
            String fullName = "";
            float ps01Counter = 0;
            float ps02Counter = 0;
            float ps07Counter = 0;
            float ps08Counter = 0;
            float ps09Counter = 0;
            float ps20Counter = 0;
            float ps21Counter = 0;
            float ps22Counter = 0;
            float pe01Counter = 0;
            float pe04Counter = 0;
            float totalCounter = 0;
            float doubleTrueCounter = 0;
            float mismatchCounter = 0;
            float trueFalseCounter = 0;
            float falseTrueCounter = 0;
            float doubleFalseCounter = 0;
            float cityCounter = 0;
            float errorCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fdlg.FileName))
                {
                    // change globaloutput to just output when finished testing
                    string outFile = fdlg.FileName.Replace(".txt", "NewOutput.html");

                    //change true to false when finished testing
                    StreamWriter sw = new StreamWriter(outFile, true);
                    string results = fdlg.FileName.Replace(".txt", "Results.txt");


                    HttpWebRequest phoneRequest;

                    WebResponse phoneResponse;

                    StreamReader phoneSR;

                    string temp = sr.ReadLine();

                    while ((temp = sr.ReadLine()) != null)
                    {
                        //sr.ReadLine();

                        string[] split = temp.Split('\t');
                        email = split[0];
                        phoneNumber = split[1];
                        fullName = split[3] + split[2];

                        string phoneJSON = $"http://globalphone.melissadata.net/V4/WEB/GlobalPhone/doGlobalPhone? &id={customerID}" +
                            $"&opt={"VERIFYPHONE:Premium,CallerID:True"}&phone={phoneNumber}";
                      
                        phoneRequest = (HttpWebRequest)WebRequest.Create(phoneJSON);


                        phoneRequest.Method = "GET";
                        //  phoneRequest.ContentLength = phoneJSON.Length;
                        phoneRequest.Accept = "application/json";

                        try
                        {
                            ///
                            phoneResponse = phoneRequest.GetResponse();
                            ///


                            phoneSR = new StreamReader(phoneResponse.GetResponseStream());
                            //  StreamReader phoneRequestSR = new StreamReader(phoneRequest.GetRequestStream());

                            //  string phoneJSONRequest = phoneRequestSR.ReadToEnd();
                            string phoneJSONResponse = phoneSR.ReadToEnd();

                            phoneSR.Close();
                            phoneSR.Dispose();

                            try
                            {
                                Phone.Rootobject phoneRO = JsonConvert.DeserializeObject<Phone.Rootobject>(phoneJSONResponse);
                                //Phone.Rootobject phoneRequestRO = JsonConvert.DeserializeObject<Phone.Rootobject>(phoneJSONRequest);
                                // Email.Rootobject emailRequestRO = JsonConvert.DeserializeObject<Email.Rootobject>(emailJSONRequest);

                                //Phone.Record[] phoneReqRecords = phoneRequestRO.Records;
                                // Email.Record[] emailReqRecords = emailRequestRO.Records;
                                Phone.Record[] phoneRecords = phoneRO.Records;

                                int space = 0;

                                if (phoneRecords[0].CallerID.Contains(" "))
                                {
                                    space = phoneRecords[0].CallerID.IndexOf(" ", 0);
                                }

                                if (fullName.TrimEnd() == "")
                                {
                                    if (phoneRecords[0].CallerID.TrimEnd() == "")
                                    {
                                        falseTrueCounter++;
                                    }
                                    else
                                    {
                                        doubleFalseCounter++;
                                    }
                                }
                                if (fullName.TrimEnd() != "")
                                {
                                    if (phoneRecords[0].CallerID.TrimEnd() == "")
                                    {
                                        trueFalseCounter++;
                                    }
                                    else if (space == (phoneRecords[0].CallerID.Length - 3))
                                    {
                                        cityCounter++;
                                    }
                                    else
                                    {
                                        if (fullName.Substring(0, 4).Equals(phoneRecords[0].CallerID.Substring(0, 4), StringComparison.OrdinalIgnoreCase))
                                        {
                                            doubleTrueCounter++;
                                        }
                                        else
                                        {
                                            mismatchCounter++;
                                        }
                                    }
                                }

                                // sw.WriteLine(emailRecords[0].EmailAddress + " " + phoneRecords[0].PhoneNumber + " "+ emailRecords[0].Results + " " +  phoneRecords[0].Results);
                               
                                if (phoneRecords[0].Results.Contains("PS01"))
                                {
                                    ps01Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS02"))
                                {
                                    ps02Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS07"))
                                {
                                    ps07Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS08"))
                                {
                                    ps08Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS09"))
                                {
                                    ps09Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS20"))
                                {
                                    ps20Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS21"))
                                {
                                    ps21Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PS22"))
                                {
                                    ps22Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PE01"))
                                {
                                    pe01Counter++;
                                }
                                if (phoneRecords[0].Results.Contains("PE04"))
                                {
                                    pe04Counter++;
                                }
                                totalCounter++;
                            }
                            catch (Exception ex)
                            {
                                errorCount++;
                                StreamWriter result = new StreamWriter(results, true);
                                //result.WriteLine("Error Count: " + errorCount + " Error Type:  " + ex.Message + " Total Count: " + totalCounter);
                                // Get stack trace for the exception with source file information
                                var st = new StackTrace(ex, true);
                                // Get the top stack frame
                                var frame = st.GetFrame(0);
                                // Get the line number from the stack frame
                                var line = frame.GetFileLineNumber();
                                result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                                result.Close();
                                result.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            StreamWriter result = new StreamWriter(results, true);
                            errorCount++;
                            // Get stack trace for the exception with source file information
                            var st = new StackTrace(ex, true);
                            // Get the top stack frame
                            var frame = st.GetFrame(0);
                            // Get the line number from the stack frame
                            var line = frame.GetFileLineNumber();
                            result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                            result.Close();
                            result.Dispose();
                        }

                    }
                    // Outside of While Loop
                    DataTable phoneTable = new DataTable();
                    DataTable nameTable = new DataTable();

                    phoneTable.Columns.Add("Phone Codes", typeof(string));
                    phoneTable.Columns.Add("Count", typeof(string));
                    phoneTable.Columns.Add("Percentage", typeof(string));
                    nameTable.Columns.Add("Result", typeof(string));
                    nameTable.Columns.Add("Count", typeof(string));
                    nameTable.Columns.Add("Percentage", typeof(string));

                    phoneTable.Rows.Add("PS01", ps01Counter, String.Format("{0:P2}", ps01Counter / totalCounter));
                   // phoneTable.Rows.Add("PS02", ps02Counter, String.Format("{0:P2}", ps02Counter / totalCounter));
                    phoneTable.Rows.Add("PS07", ps07Counter, String.Format("{0:P2}", ps07Counter / totalCounter));
                    phoneTable.Rows.Add("PS08", ps08Counter, String.Format("{0:P2}", ps08Counter / totalCounter));
                    phoneTable.Rows.Add("PS09", ps09Counter, String.Format("{0:P2}", ps09Counter / totalCounter));
                    phoneTable.Rows.Add("PS20", ps20Counter, String.Format("{0:P2}", ps20Counter / totalCounter));
                    phoneTable.Rows.Add("PS21", ps21Counter, String.Format("{0:P2}", ps21Counter / totalCounter));
                    phoneTable.Rows.Add("PS22", ps22Counter, String.Format("{0:P2}", ps22Counter / totalCounter));
                    phoneTable.Rows.Add("PE01", pe01Counter, String.Format("{0:P2}", pe01Counter / totalCounter));
                    phoneTable.Rows.Add("PE04", pe04Counter, String.Format("{0:P2}", pe04Counter / totalCounter));
                    nameTable.Rows.Add("Input Has CallerID and Database Has CallerID And They Match", doubleTrueCounter, String.Format("{0:P2}", doubleTrueCounter / totalCounter));
                    nameTable.Rows.Add("Input Has CallerID and Database Has CallerID But They Do Not Match", mismatchCounter, String.Format("{0:P2}", mismatchCounter / totalCounter));
                    nameTable.Rows.Add("Returned CallerID Is Of a City", cityCounter, String.Format("{0:P2}", cityCounter / totalCounter));
                    nameTable.Rows.Add("Input Has CallerID but Database Does Not Have CallerID", trueFalseCounter, String.Format("{0:P2}", trueFalseCounter / totalCounter));
                    nameTable.Rows.Add("Input Does Not Have CallerID but Database Does Have CallerID", falseTrueCounter, String.Format("{0:P2}", falseTrueCounter / totalCounter));
                    nameTable.Rows.Add("Input Does Not Have CallerID and Database Does Not Have CallerID", doubleFalseCounter, String.Format("{0:P2}", doubleFalseCounter / totalCounter));

                    string phoneResults = ConvertDataTableToHTML(phoneTable);
                    string nameResults = ConvertDataTableToHTMLBinary(nameTable);

                    sw.Write(phoneResults);
                    sw.WriteLine();
                    sw.Write(nameResults);

                    sw.Close();
                    sw.Dispose();
                    sr.Close();
                    sr.Dispose();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            String email = "";
            String phoneNumber = "";
            String customerID = txtCustomerID.Text;
            //String country = "United States";
            String fullName = "";
            float es01Counter = 0;
            float es02Counter = 0;
            float es03Counter = 0;
            float es04Counter = 0;
            float es07Counter = 0;
            float es08Counter = 0;
            float es09Counter = 0;
            float ps01Counter = 0;
            float ps02Counter = 0;
            float ps07Counter = 0;
            float ps08Counter = 0;

            float ps09Counter = 0;
            float ps20Counter = 0;
            float ps21Counter = 0;
            float ps22Counter = 0;
            float pe01Counter = 0;
            float pe04Counter = 0;
            float totalCounter = 0;
            float doubleTrueCounter = 0;
            float mismatchCounter = 0;
            float trueFalseCounter = 0;
            float falseTrueCounter = 0;
            float doubleFalseCounter = 0;
            float cityCounter = 0;
            float errorCount = 0;


            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "C# Corner Open File Dialog";

            fdlg.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            fdlg.Filter = "All files (*.*)|*.*|TXT (*.txt)|*.txt";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(fdlg.FileName))
                    {
                        // change globaloutput to just output when finished testing
                        string outFile = fdlg.FileName.Replace(".txt", "NewOutput.html");

                        //change true to false when finished testing
                        StreamWriter sw = new StreamWriter(outFile, true);
                        string results = fdlg.FileName.Replace(".txt", "Results.txt");


                        HttpWebRequest phoneRequest;
                        HttpWebRequest emailRequest;

                        WebResponse phoneResponse;
                        WebResponse emailResponse;

                        StreamReader phoneSR;
                        StreamReader emailSR;

                        string temp = sr.ReadLine();

                        while ((temp = sr.ReadLine()) != null)
                        {
                            //sr.ReadLine();

                            string[] split = temp.Split('|');
                            email = split[3];
                            phoneNumber = split[9];
                            fullName = split[2] + split[1];


                            string emailJSON = $"https://globalemail.melissadata.net/V3/WEB/GlobalEmail/doGlobalEmail? &id={customerID}" +
                                $"&opt={"VERIFYMAILBOX:Premium"}&email={email}&format=json";
                            string phoneJSON = $"http://globalphone.melissadata.net/V4/WEB/GlobalPhone/doGlobalPhone? &id={customerID}" +
                                $"&opt={"VERIFYPHONE:Premium,CallerID:True"}&phone={phoneNumber}";
                            int increment = 0;
                            bool timeout = false;

                            phoneRequest = (HttpWebRequest)WebRequest.Create(phoneJSON);
                            emailRequest = (HttpWebRequest)WebRequest.Create(emailJSON);



                            //do
                            //{
                            //    try
                            //    {
                            //        // Used to check into the web service and actually verify the data
                            //        phoneRequest = (HttpWebRequest)WebRequest.Create(phoneJSON);
                            //        emailRequest = (HttpWebRequest)WebRequest.Create(emailJSON);
                            //        timeout = false;
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        timeout = true;
                            //        increment++;
                            //        if (increment > 50)
                            //            timeout = false;
                            //    }
                            //} while (timeout == true);


                            phoneRequest.Method = "GET";
                            emailRequest.Method = "GET";
                            //  phoneRequest.ContentLength = phoneJSON.Length;
                            //  emailRequest.ContentLength = emailJSON.Length;
                            phoneRequest.Accept = "application/json";
                            emailRequest.Accept = "application/json";

                            try
                            {
                                ///
                                phoneResponse = phoneRequest.GetResponse();
                                emailResponse = emailRequest.GetResponse();
                                ///


                                //do
                                //{
                                //    try
                                //    {
                                //        phoneResponse = phoneRequest.GetResponse();
                                //        emailResponse = emailRequest.GetResponse();
                                //        timeout = false;
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        timeout = true;
                                //        increment++;
                                //        if (increment > 50)
                                //            timeout = false;
                                //    }
                                //} while (timeout == true);



                                phoneSR = new StreamReader(phoneResponse.GetResponseStream());
                                emailSR = new StreamReader(emailResponse.GetResponseStream());
                                //  StreamReader phoneRequestSR = new StreamReader(phoneRequest.GetRequestStream());
                                //  StreamReader emailRequestSR = new StreamReader(emailRequest.GetRequestStream());

                                //  string phoneJSONRequest = phoneRequestSR.ReadToEnd();
                                // string emailJSONRequest = emailRequestSR.ReadToEnd();
                                string phoneJSONResponse = phoneSR.ReadToEnd();
                                string emailJSONResponse = emailSR.ReadToEnd();

                                phoneSR.Close();
                                emailSR.Close();
                                phoneSR.Dispose();
                                emailSR.Dispose();

                                try
                                {
                                    Phone.Rootobject phoneRO = JsonConvert.DeserializeObject<Phone.Rootobject>(phoneJSONResponse);
                                    Email.Rootobject emailRO = JsonConvert.DeserializeObject<Email.Rootobject>(emailJSONResponse);
                                    //Phone.Rootobject phoneRequestRO = JsonConvert.DeserializeObject<Phone.Rootobject>(phoneJSONRequest);
                                    // Email.Rootobject emailRequestRO = JsonConvert.DeserializeObject<Email.Rootobject>(emailJSONRequest);

                                    //Phone.Record[] phoneReqRecords = phoneRequestRO.Records;
                                    // Email.Record[] emailReqRecords = emailRequestRO.Records;
                                    Phone.Record[] phoneRecords = phoneRO.Records;
                                    Email.Record[] emailRecords = emailRO.Records;

                                    int space = 0;

                                    if (phoneRecords[0].CallerID.Contains(" "))
                                    {
                                        space = phoneRecords[0].CallerID.IndexOf(" ", 0);
                                    }

                                    if (fullName.TrimEnd() == "")
                                    {
                                        if (phoneRecords[0].CallerID.TrimEnd() == "")
                                        {
                                            falseTrueCounter++;
                                        }
                                        else
                                        {
                                            doubleFalseCounter++;
                                        }
                                    }
                                    if (fullName.TrimEnd() != "")
                                    {
                                        if (phoneRecords[0].CallerID.TrimEnd() == "")
                                        {
                                            trueFalseCounter++;
                                        }
                                        else if (space == (phoneRecords[0].CallerID.Length - 3))
                                        {
                                            cityCounter++;
                                        }
                                        else
                                        {
                                            if (fullName.Substring(0, 4).Equals(phoneRecords[0].CallerID.Substring(0, 4), StringComparison.OrdinalIgnoreCase))
                                            {
                                                doubleTrueCounter++;
                                            }
                                            else
                                            {
                                                mismatchCounter++;
                                            }
                                        }
                                    }

                                    // sw.WriteLine(emailRecords[0].EmailAddress + " " + phoneRecords[0].PhoneNumber + " "+ emailRecords[0].Results + " " +  phoneRecords[0].Results);
                                    if (emailRecords[0].Results.Contains("ES01"))
                                    {

                                        es01Counter++;
                                    }
                                    if (emailRecords[0].Results.Contains("ES02"))
                                    {
                                        es02Counter++;
                                    }
                                    if (emailRecords[0].Results.Contains("ES03"))
                                    {
                                        es03Counter++;
                                    }
                                    if (emailRecords[0].Results.Contains("ES04"))
                                    {
                                        es04Counter++;
                                    }
                                    if (emailRecords[0].Results.Contains("ES07"))
                                    {
                                        es07Counter++;
                                    }
                                    if (emailRecords[0].Results.Contains("ES08"))
                                    {
                                        es08Counter++;
                                    }
                                    if (emailRecords[0].Results.Contains("ES09"))
                                    {
                                        es09Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS01"))
                                    {
                                        ps01Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS02"))
                                    {
                                        ps02Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS07"))
                                    {
                                        ps07Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS08"))
                                    {
                                        ps08Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS09"))
                                    {
                                        ps09Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS20"))
                                    {
                                        ps20Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS21"))
                                    {
                                        ps21Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PS22"))
                                    {
                                        ps22Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PE01"))
                                    {
                                        pe01Counter++;
                                    }
                                    if (phoneRecords[0].Results.Contains("PE04"))
                                    {
                                        pe04Counter++;
                                    }
                                    totalCounter++;
                                }
                                catch (Exception ex)
                                {
                                    errorCount++;
                                    StreamWriter result = new StreamWriter(results, true);
                                    //result.WriteLine("Error Count: " + errorCount + " Error Type:  " + ex.Message + " Total Count: " + totalCounter);
                                    // Get stack trace for the exception with source file information
                                    var st = new StackTrace(ex, true);
                                    // Get the top stack frame
                                    var frame = st.GetFrame(0);
                                    // Get the line number from the stack frame
                                    var line = frame.GetFileLineNumber();
                                    result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                                    result.Close();
                                    result.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                StreamWriter result = new StreamWriter(results, true);
                                errorCount++;
                                // Get stack trace for the exception with source file information
                                var st = new StackTrace(ex, true);
                                // Get the top stack frame
                                var frame = st.GetFrame(0);
                                // Get the line number from the stack frame
                                var line = frame.GetFileLineNumber();
                                result.WriteLine("Line Number: {0}   Error: {1}  Total Count: {2} ", line.ToString(), ex.Message, totalCounter);
                                result.Close();
                                result.Dispose();
                            }

                        }

                        //Outside of While Loop
                        DataTable emailTable = new DataTable();
                        DataTable phoneTable = new DataTable();
                        DataTable nameTable = new DataTable();

                        emailTable.Columns.Add("Email Codes", typeof(string));
                        emailTable.Columns.Add("Count", typeof(string));
                        emailTable.Columns.Add("Percentage", typeof(string));
                        phoneTable.Columns.Add("Phone Codes", typeof(string));
                        phoneTable.Columns.Add("Count", typeof(string));
                        phoneTable.Columns.Add("Percentage", typeof(string));
                        nameTable.Columns.Add("Result", typeof(string));
                        nameTable.Columns.Add("Count", typeof(string));
                        nameTable.Columns.Add("Percentage", typeof(string));


                        emailTable.Rows.Add("ES01", es01Counter, String.Format("{0:P2}", es01Counter / totalCounter));
                        emailTable.Rows.Add("ES02", es02Counter, String.Format("{0:P2}", es02Counter / totalCounter));
                        emailTable.Rows.Add("ES03", es03Counter, String.Format("{0:P2}", es03Counter / totalCounter));
                        emailTable.Rows.Add("ES04", es04Counter, String.Format("{0:P2}", es04Counter / totalCounter));
                        phoneTable.Rows.Add("PS01", ps01Counter, String.Format("{0:P2}", ps01Counter / totalCounter));
                        //phoneTable.Rows.Add("PS02", ps02Counter, String.Format("{0:P2}", ps02Counter / totalCounter));
                        phoneTable.Rows.Add("PS07", ps07Counter, String.Format("{0:P2}", ps07Counter / totalCounter));
                        phoneTable.Rows.Add("PS08", ps08Counter, String.Format("{0:P2}", ps08Counter / totalCounter));
                        phoneTable.Rows.Add("PS09", ps09Counter, String.Format("{0:P2}", ps09Counter / totalCounter));
                        phoneTable.Rows.Add("PS20", ps20Counter, String.Format("{0:P2}", ps20Counter / totalCounter));
                        phoneTable.Rows.Add("PS21", ps21Counter, String.Format("{0:P2}", ps21Counter / totalCounter));
                        phoneTable.Rows.Add("PS22", ps22Counter, String.Format("{0:P2}", ps22Counter / totalCounter));
                        phoneTable.Rows.Add("PE01", pe01Counter, String.Format("{0:P2}", pe01Counter / totalCounter));
                        phoneTable.Rows.Add("PE04", pe04Counter, String.Format("{0:P2}", pe04Counter / totalCounter));
                        nameTable.Rows.Add("Input Has CallerID and Database Has CallerID And They Match", doubleTrueCounter, String.Format("{0:P2}", doubleTrueCounter / totalCounter));
                        nameTable.Rows.Add("Input Has CallerID and Database Has CallerID But They Do Not Match", mismatchCounter, String.Format("{0:P2}", mismatchCounter / totalCounter));
                        nameTable.Rows.Add("Returned CallerID Is Of a City", cityCounter, String.Format("{0:P2}", cityCounter / totalCounter));
                        nameTable.Rows.Add("Input Has CallerID but Database Does Not Have CallerID", trueFalseCounter, String.Format("{0:P2}", trueFalseCounter / totalCounter));
                        nameTable.Rows.Add("Input Does Not Have CallerID but Database Does Have CallerID", falseTrueCounter, String.Format("{0:P2}", falseTrueCounter / totalCounter));
                        nameTable.Rows.Add("Input Does Not Have CallerID and Database Does Not Have CallerID", doubleFalseCounter, String.Format("{0:P2}", doubleFalseCounter / totalCounter));
                        

                        string emailResults = ConvertDataTableToHTML(emailTable);
                        string phoneResults = ConvertDataTableToHTML(phoneTable);
                        string nameResults = ConvertDataTableToHTMLBinary(nameTable);
                       
                        

                        sw.Write(emailResults);
                        sw.WriteLine();
                        sw.Write(phoneResults);
                        sw.WriteLine();
                        sw.Write(nameResults);
                       
                       

                        sw.Close();
                        sw.Dispose();
                        sr.Close();
                        sr.Dispose();
                        

                        

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }

              
            }
            Application.Exit();
           

        }
        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table border = 1 style = width:30%>";
            //add header row
            html += "<tr align = center>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr align = center>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //if (j == 1 && (int.Parse(dt.Rows[i][j].ToString()) >= 7000))
                    //{
                    //    html += "<td bgcolor = #7AFE76>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else if (j == 1 && (int.Parse(dt.Rows[i][j].ToString()) <= 7000))
                    //{
                    //    html += "<td bgcolor = #FF6161>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else if (j == 2 && (double.Parse(dt.Rows[i][j].ToString().Replace("%", "")) / 100) >= 0.70)
                    //{
                    //    html += "<td bgcolor = #7AFE76>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else if (j == 2 && (double.Parse(dt.Rows[i][j].ToString().Replace("%", "")) / 100) < 0.70)
                    //{
                    //    html += "<td bgcolor = #FF6161>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else
                    {
                        html += "<td >" + dt.Rows[i][j].ToString() + "</td>";
                    }

                }
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
        public static string ConvertDataTableToHTMLBinary(DataTable dt)
        {
            string html = "<table border = 1 style= width:35% >";
            //add header row
            html += "<tr align = center>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //if (j == 1 && (int.Parse(dt.Rows[i][j].ToString()) >= 7000))
                    //{
                    //    html += "<td bgcolor = #7AFE76>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else if (j == 1 && (int.Parse(dt.Rows[i][j].ToString()) <= 7000))
                    //{
                    //    html += "<td bgcolor = #FF6161>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else if (j == 2 && (double.Parse(dt.Rows[i][j].ToString().Replace("%", "")) / 100) >= 0.70)
                    //{
                    //    html += "<td bgcolor = #7AFE76>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else if (j == 2 && (double.Parse(dt.Rows[i][j].ToString().Replace("%", "")) / 100) < 0.70)
                    //{
                    //    html += "<td bgcolor = #FF6161>" + dt.Rows[i][j].ToString() + "</td>";
                    //}
                    //else
                    {
                        html += "<td >" + dt.Rows[i][j].ToString() + "</td>";
                    }

                }
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }


    }
}
