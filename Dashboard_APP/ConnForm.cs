using MSTSCLib;
using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Dashboard_APP
{
    public partial class ConnForm : Form
    {
        // constructor
        public ConnForm(PCInfo info)
        {
            // initialize
            InitializeComponent();
            // create Ping object named pingsender
            Ping pingsender = new Ping();
            // create a PingReply object named reply and confim if the remote computer is online.
            PingReply reply = pingsender.Send(info.ip);
            
            if (reply.Status == IPStatus.Success)
            {
                // setup IP
                axMsRdpClient81.Server = info.ip;
                // setup username
                axMsRdpClient81.UserName = info.username;
                //setup port number
                axMsRdpClient81.AdvancedSettings2.RDPPort = Convert.ToInt16(info.port);
                // setup size 
                axMsRdpClient81.AdvancedSettings2.SmartSizing = true;
                // setup height
                axMsRdpClient81.DesktopHeight = this.Height;
                // setup width
                axMsRdpClient81.DesktopWidth = this.Width;
                // encryption is enabled
                axMsRdpClient81.AdvancedSettings9.NegotiateSecurityLayer = true;
                IMsTscNonScriptable securd = (IMsTscNonScriptable)axMsRdpClient81.GetOcx();
                // setup password
                securd.ClearTextPassword = info.password;
                axMsRdpClient81.AdvancedSettings5.ClearTextPassword = info.password;
                // setup color
                axMsRdpClient81.ColorDepth = 24;
                // establish the connection
                axMsRdpClient81.Connect();
            }
            //if there is no reply.
            else
            {
                // show the message box and close.
                MessageBox.Show("Unable to connect to the Server！");
                this.Close();
            }
          
        }

        // resize event handler
        private void ConnForm_Resize(object sender, EventArgs e)
        {
            try
            {
                // reconnect the remote computer according to the height and width.
                axMsRdpClient81.Reconnect((uint)this.Width, (uint)this.Height);
            }
            catch
            {
                throw;
            }
        }

    }
}
