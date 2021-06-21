using System;
using System.Linq;
using System.Windows.Forms;

namespace Dashboard_APP
{
    public partial class AddPC : Form
    {
        // constructor
        public AddPC()
        {
            InitializeComponent();
        }

        //define a private field named _info
        private PCInfo _info;

        // collecting information for PCInfo
        public PCInfo info
        {
            //read the information for PCInfo object
            get 
            { //initialize the PCInfo object if there is no input. 
                if (_info == null)
                {
                    _info = new PCInfo();
                }
                //read IP from the input in the text box 1
                _info.ip = textBox1.Text;
                //read username from the input in the text box 2
                _info.username = textBox2.Text;
                // read password from the input in the text box 3
                _info.password = textBox3.Text;
                // read port from the input in the text box 4
                _info.port = textBox4.Text;
                // return
                return _info;
            }
            // write the value to the variables for each PCInfo object. 
            set
            {
                //set value to PCInfo object named _info
                _info = value;
                // write IP 
                textBox1.Text = _info.ip;
                // write username
                textBox2.Text = _info.username;
                // write password
                textBox3.Text = _info.password;
                // write port
                textBox4.Text = _info.port;
            }
        }

        //Add button2 click Event Handler
        private void button2_Click(object sender, EventArgs e)
        {
            // examine the input of each text box is string, empty,or null.  
            if (string.IsNullOrEmpty(textBox1.Text.Trim()) || 
                string.IsNullOrEmpty(textBox2.Text.Trim()) ||
                string.IsNullOrEmpty(textBox3.Text.Trim()) ||
                string.IsNullOrEmpty(textBox4.Text.Trim()))
            {
                //if any of the input is empty, show a message.
                MessageBox.Show("Cannot be empty!");
                return;
            }
            // call method IsIP to verify the IP
            if (!IsIP(textBox1.Text.Trim()))
            {
                // if it is not IP, show a message.
                MessageBox.Show("Illegal IP!");
                return;
            }
            // set the DialogResult is OK, if any of the above "If" statements is triggered.
            this.DialogResult = DialogResult.OK;
        }

        // create a boolean method named IsIP to verify the IP address is in correct format.
        public bool IsIP(string IP)
        {
            // split the IP parameter, and count the number of the elements.
            var iCount = IP.Split('.').Count();
            // if the number of the elements is not 4 return false
            if (iCount != 4)
            {
                return false;
            }
            // create an IP address object named ip
            System.Net.IPAddress ip;
            // if the string IP can be converted to integer ip successfully, 
            //return true. Otherwise return false.
            if (System.Net.IPAddress.TryParse(IP, out ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Add button1 click event handler
        private void button1_Click(object sender, EventArgs e)
        {
            // change the property value to False
            textBox3.UseSystemPasswordChar = !textBox3.UseSystemPasswordChar;
        }
    }
}
