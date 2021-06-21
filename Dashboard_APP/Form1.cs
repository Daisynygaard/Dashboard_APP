using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Dashboard_APP
{
    public partial class Form1 : Form
    {
        // define a new PCInfo object list
        private BindingList<PCInfo> PCInfos;
        // define an old PC object list
        private BindingList<PCInfo> oldPCInfos;

        //1. constructor
        public Form1()
        {
            InitializeComponent();

            // setup the form
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // created an objet of PCInfo on the new PCInfo object list.
            PCInfos = new BindingList<PCInfo>();
            // created an objet of PCInfo on the old PCInfo object list.
            oldPCInfos = new BindingList<PCInfo>();

            // call the method to initialize the form
            initData();
        }

        // 2. Create a method named initData() to initialize the form
        private void initData()
        {
            // assign PCInfos to dataGridView1
            dataGridView1.DataSource = PCInfos;

            // set the initial information of PC object to be invisible
            dataGridView1.Columns["username"].Visible = false;
            dataGridView1.Columns["password"].Visible = false;
            dataGridView1.Columns["port"].Visible = false;
            dataGridView1.Columns["isOnline"].Visible = false;
            dataGridView1.Columns["isConn"].Visible = false;
        }

        // 3. add the envent handler to button1/Add button
        private void button1_Click(object sender, EventArgs e)
        {
            // create an instances of AddPC class.
            AddPC form = new AddPC();
            // shows the AddPC form as a modal dialog box.
            DialogResult res = form.ShowDialog();

            // set Online status is false by default
            form.info.isOnline = false;
            // set the connection is false by default
            form.info.isConn = false;
            // verify if the input IP address alraedy exists or not, when the DialogResult returns the value: OK.
            if (res == DialogResult.OK)
            {
                // if the IP address exists
                if (PCInfos.Any(index => index.ip == form.info.ip))
                {
                    // show the message and return
                    MessageBox.Show("IP already exists!");
                    return;
                }
                // otherwise, assign the input information to PC object.
                PCInfo info = form.info;
                // and add the PC object to BindList PCInfos.
                PCInfos.Add(info);
            }
            // close AddPC form/window
            form.Dispose();
        }

        // 4. add event handler for edit option
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // declear a variable of the selected row, 
            //and assign the information of PC object from the row on the dataGridView1 to the variable. 
            var dataselect = this.dataGridView1.SelectedRows;
            // if the PC object is selected. 
            if (dataselect.Count > 0)
            {
                //create a new PC object of PCInfo class, 
                PCInfo info = new PCInfo();
                //update the relevant information/property of PC to the new PC object.
                info.ip = dataselect[0].Cells["ip"].Value.ToString();
                info.username = dataselect[0].Cells["username"].Value.ToString();
                info.password = dataselect[0].Cells["password"].Value.ToString();
                info.port = dataselect[0].Cells["port"].Value.ToString();
                info.isOnline = Convert.ToBoolean(dataselect[0].Cells["isOnline"].Value.ToString());
                //create a new AddPC form to,  
                AddPC frm = new AddPC();
                //update the value of the new PC object to AddPC form.
                frm.info = info;
                //show the AddPC form as a modal dialog.
                DialogResult res = frm.ShowDialog();
               //If the Save button is clicked, which indicates the value for the DialogResult is OK, 
                if (res == DialogResult.OK)
                {
                    //update the properties of PC object to dataGridView1. 
                    dataselect[0].Cells["ip"].Value = frm.info.ip;
                    dataselect[0].Cells["username"].Value = frm.info.username;
                    dataselect[0].Cells["password"].Value = frm.info.password;
                    dataselect[0].Cells["port"].Value = frm.info.port;
                    dataselect[0].Cells["isOnline"].Value = frm.info.isOnline;

                    this.dataGridView1.Invalidate();
                }
                // close the AddPC form/window
                frm.Dispose();
            }
            // Otherwise, show the message if there is no PC object is selected.
            else
            {
                MessageBox.Show("No data!");
            }
        }

        // 5. add event handler for delete option
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // declear a variable of the selected row, 
            //and assign the information of PC object from the row on the dataGridView1 to the variable. 
            var dataselect = this.dataGridView1.SelectedRows;
            // if the number of selected row is not 0, which indicates there is a row/data selected.
            if (dataselect.Count > 0)
            {
                // iterate the selected rows collection, 
                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    //If it is not a submitted row, by default, 
                    //after adding a row of data successfully, 
                    //DataGridView will create a new row as the insertion location of the new data
                    if (dr.IsNewRow == false)
                    {
                        // delete the row.
                        dataGridView1.Rows.Remove(dr);
                    }
                }
            }
            // otherwise, show "no data" message.
            else
            {
                MessageBox.Show("No data!");
            }
        }

        //6.  display the cascading menu, which is the contextMenuStrip from Form1 Designer.
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // if there is a right mouse clicking.
            if (e.Button == MouseButtons.Right)
            {
                // if the right mouse clicking is on the row of PC, not in the blank area.
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //call Clearselection() method to clear the current selection by unselecting all selected cells.
                    dataGridView1.ClearSelection();
                    // Get the selected row index
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    // Current grid
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    // show the cascading menu exactly in the position where the mouse clicking. 
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        // 7. add event handler for connect option
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // declear a variable of the selected row, 
            //and assign the information of PC object from the row on the dataGridView1 to the variable. 
            var dataselect = this.dataGridView1.SelectedRows;
            // if the number of selected row is not 0, which indicates there is a row/data selected.
            if (dataselect.Count > 0)
            {
                //create a new PC object of PCInfo class,
                PCInfo info = new PCInfo();
                //update the relevant information/property of PC to the new PC object bu signing the data for the selected row to info
                info.ip = dataselect[0].Cells["ip"].Value.ToString();
                info.username = dataselect[0].Cells["username"].Value.ToString();
                info.password = dataselect[0].Cells["password"].Value.ToString();
                info.port = dataselect[0].Cells["port"].Value.ToString();
                info.isOnline = Convert.ToBoolean(dataselect[0].Cells["isOnline"].Value.ToString());
                // if the selected row of computer is not Online.
                if (!info.isOnline)
                {
                    // show the message box and return.
                    MessageBox.Show("PC is offline!");
                    return;
                }
                //Otherwise, create an intance of ConnForm and take info(which is the PCInfo instance) as the parameter.
                ConnForm form = new ConnForm(info);
                // call the show() method to show the ConnForm form.
                form.Show();
            }
            // otherwise, show a message box with message "No data!"
            else
            {
                MessageBox.Show("No data!");
            }
        }

        //8.write a boolean method to verify status of the remote computer is online, 
        //and catch an exceotipn if it is not online. 
        private bool StatusQuery(string ip)
        {
            // declear a bool type result named res.
            bool res;
            // the initial message is an enpty string variable.
            string message = "";
            // create an instance of Ping 
            Ping p = new Ping();
            // use "try-catch-finnaly" to raise an exception while the remote computer is not reachable by Ping  
            try
            {
                // create an instance of PingReply class named r and 
                //called Ping.send(ip) method to return a value for IPStatus.
                PingReply r = p.Send(ip);
                // if the return value from Ping.send() method is Success.
                if (r.Status == IPStatus.Success)
                {
                    // assign the string "Success" to message.
                    message = "Success";
                }
            }
            //deal with the exception in catch block 
            catch (Exception ex)
            {
                // raise the exception
                throw;
            }
            //release the ressult that obtained in the try block 
            finally
            {
                // if the message is string "Success"
                if (message == "Success")
                {
                    // the resualt is true.
                    res = true;
                }
                //otherwise
                else
                {
                    // the resualt is false.
                    res = false;
                }
            }
            // return the result
            return res;
        }

        // 9. event handler for timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            // check IsBusy to see if the background task is running, and return
            if (backgroundWorker1.IsBusy)
            {
                return;
            }
            // Start the operation in the background.
            backgroundWorker1.RunWorkerAsync();
        }

        // 10. create an event handler to backgroundWorker 
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // iterate the PCInfo instances in the oldePCInfos collection
            foreach (PCInfo item in oldPCInfos)
            {
                // call StatusQuery to get the status of each PCInfo instances
                item.isOnline = StatusQuery(item.ip);
                // if PCInfo instance is online
                if (item.isOnline)
                {
                    // create the IPHostEntry instance form the ip address of PCInfo insatance.
                    IPHostEntry myScanHost = Dns.GetHostByAddress(item.ip);
                    // assign the hostname 
                    item.hostName = myScanHost.HostName.ToString();
                    // set status is online
                    item.status = "Online";
                    // set offtime is null
                    item.offlineTime = null;
                    // set the offline duration is 0.
                    item.offlineDuration = 0;
                }
                // otherwise( when the PCInfo instance is offline)
                else
                {
                    // check if  the previous ststus of PCInfo instance is also offline 
                    if (item.status == "Offline")
                    {
                        // set the offline duration is the current DateTime - offlineTime
                        item.offlineDuration = Convert.ToInt32((DateTime.Now - item.offlineTime).Value.TotalSeconds);
                    }
                    // otherwise( when the PCInfo instance was online)
                    else
                    {
                        // change the ststus to offline.
                        item.status = "Offline";
                        // update the offline time to DateTime ow.
                        item.offlineTime = DateTime.Now;
                        // set the offline duration
                        item.offlineDuration = 0;
                    }
                }
            }
            // repaint the dataGridView
            this.dataGridView1.Invalidate();
            // update the oldPCInfos collection.
            oldPCInfos = PCInfos;
        }
        //11.Create a CellFormatting Event Handler to DataGridView
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // if the index of column is 9
            if (e.ColumnIndex == 9)
            {
                // and the value is not null
                if (e.Value != null)
                {
                    // if the value is not 0 coverted from the integer
                    if (Convert.ToInt32(e.Value) != 0)
                    {
                        // call the ConvertDayHourMinuteSencond(int duration) method to convert the Integer to String
                        e.Value = ConvertDayHourMinuteSencond(Convert.ToInt32(e.Value));
                    }
                }

            }
        }

        //12. create a method to covert the offline duration time from integer to string.
        private string ConvertDayHourMinuteSencond(int duration)
        {
            // get the time interval
            TimeSpan ts = new TimeSpan(0, 0, duration);
            // declare an enpty string variable 
            string str = "";
            // add day to string, if the offline duration time is more than 1 day.
            if (ts.Days > 0)
            {
                str = ts.Days.ToString() + "d" + ts.Hours.ToString() + "h" + ts.Minutes.ToString() + "m" + ts.Seconds + "s";
            }
            //add hour to string, if the offline duration time is more than 1 hour.
            else if (ts.Hours > 0)
            {
                str = ts.Hours.ToString() + "h" + ts.Minutes.ToString() + "m" + ts.Seconds + "s";
            }
            // add minute to string, if the offline duration time is more than 1 minute.
            else if (ts.Minutes > 0)
            {
                str = ts.Minutes.ToString() + "m" + ts.Seconds + "s";
            }
            // add second to string, if the offline duration time is more than 1 second.
            else
            {
                str = ts.Seconds + "s";
            }
            // return string.
            return str;
        }

        // 13.create Form.Load event handler 
        private void Form1_Load(object sender, EventArgs e)
        {
            // get the base directory, which is the config.json file
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "config.json";
            // verify the config.json file exists or not.
            //if the config.json file exists, read the data.
            if (File.Exists(path))
                Readjson(path);
        }

        // 14. read data from config.json file
        private void Readjson(string path)
        {
            // create a text reader insatnce of StreamReader Class
            using (System.IO.StreamReader file = System.IO.File.OpenText(path))
            {
                // create a instance of Json text reader 
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    // create a Json array, and assign the data read from Config.json to Json Array. 
                    JArray jarray = (JArray)JToken.ReadFrom(reader);
                    // iterate the json Array
                    for (int i=0; i<jarray.Count(); i++)
                    {
                        // get the current element in Json Array
                        JObject temp = (JObject)jarray[i];
                        // create a instance of PCInfo, named tempPCInfo 
                        //and update the properties of the current element to tempPCInfo.
                        PCInfo tempPCInfo = new PCInfo
                        {
                            ip = temp["ip"].ToString(),
                            hostName = temp["hostName"].ToString(),
                            username = temp["username"].ToString(),
                            password = temp["password"].ToString(),
                            port = temp["port"].ToString(),
                            isOnline = Convert.ToBoolean(temp["isOnline"].ToString()),
                            status = temp["status"].ToString(),
                            isConn = Convert.ToBoolean(temp["isConn"].ToString()),
                            offlineDuration = Convert.ToInt32(temp["offlineDuration"].ToString())
                        };
                        // check the offline time
                        //if the offline time is not empty. 
                        if (temp["offlineTime"].ToString() != "")
                            // update the offline time of the current element to tempPCInfo
                            tempPCInfo.offlineTime = Convert.ToDateTime(temp["offlineTime"].ToString());
                        // add the tempPCInfo to PCInfos collection.
                        this.PCInfos.Add(tempPCInfo);
                    }
                }
            }
        }

        //15. Write JSON file.
        private void Writejson(string path)
        {
            // Creates or opens a file for writing encoded text. If the file already exists, its contents are overwritten.
            using (System.IO.StreamWriter file = System.IO.File.CreateText(path))
            {
                // create a JsonTextWriter instance
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    // Writes the beginning of an array.
                    writer.WriteStartArray();
                    // iterates the PCInfos collection, 
                    for (int i = 0; i < PCInfos.Count(); i++)
                    {
                        // get the current element in the PCInfos collection
                        //write the properties of the current element into JSON file.
                        PCInfo temp = PCInfos[i];
                        //Writes the beginning of a JSON object.
                        writer.WriteStartObject();

                        writer.WritePropertyName("ip");
                        writer.WriteValue(temp.ip);

                        writer.WritePropertyName("hostName");
                        writer.WriteValue(temp.hostName);

                        writer.WritePropertyName("username");
                        writer.WriteValue(temp.username);

                        writer.WritePropertyName("password");
                        writer.WriteValue(temp.password);

                        writer.WritePropertyName("port");
                        writer.WriteValue(temp.port);

                        writer.WritePropertyName("isOnline");
                        writer.WriteValue(temp.isOnline);

                        writer.WritePropertyName("status");
                        writer.WriteValue(temp.status);

                        writer.WritePropertyName("isConn");
                        writer.WriteValue(temp.isConn);

                        writer.WritePropertyName("offlineTime");
                        writer.WriteValue(temp.offlineTime);

                        writer.WritePropertyName("offlineDuration");
                        writer.WriteValue(temp.offlineDuration);
                        //Writes the end of a JSON object.
                        writer.WriteEndObject();
                    }
                    //Writes the end of an array.
                    writer.WriteEndArray();
                }
            }
        }

        // 16.create FormClosing event handler
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // get the base directory, which is the config.json file
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "config.json";
            // call writejson() method to create config.json in the directory.
            Writejson(path);
        }
    }

    // PC information class
    public class PCInfo
    {
        //Contructor
        public PCInfo() { }
        //create fields
        private string _ip;
        private string _hostName;
        private string _username;
        private string _password;
        private string _port;
        private bool _isOnline;
        private string _status;
        private bool _isConn;
        private DateTime? _offlineTime;
        private int _offlineDuration;
        /// <summary>
        /// 
        /// </summary>
        /// get and set method. The property
        public string ip
        {
            set { _ip = value; }
            get { return _ip; }
        }

        public string hostName
        {
            set { _hostName = value; }
            get { return _hostName; }
        }

        public string username
        {
            set { _username = value; }
            get { return _username; }
        }

        public string password
        {
            set { _password = value; }
            get { return _password; }
        }

        public string port
        {
            set { _port = value; }
            get { return _port; }
        }

        public bool isOnline
        {
            set { _isOnline = value; }
            get { return _isOnline; }
        }

        public string status
        {
            set { _status = value; }
            get { return _status; }
        }

        public bool isConn
        {
            set { _isConn = value; }
            get { return _isConn; }
        }

        public DateTime? offlineTime
        {
            set { _offlineTime = value; }
            get { return _offlineTime; }
        }

        public int offlineDuration
        {
            set { _offlineDuration = value; }
            get { return _offlineDuration; }
        }
    }
}
