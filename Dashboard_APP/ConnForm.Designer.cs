
namespace Dashboard_APP
{
    partial class ConnForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnForm));
            this.axMsRdpClient81 = new AxMSTSCLib.AxMsRdpClient8();
            ((System.ComponentModel.ISupportInitialize)(this.axMsRdpClient81)).BeginInit();
            this.SuspendLayout();
            // 
            // axMsRdpClient81
            // 
            resources.ApplyResources(this.axMsRdpClient81, "axMsRdpClient81");
            this.axMsRdpClient81.Name = "axMsRdpClient81";
            this.axMsRdpClient81.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMsRdpClient81.OcxState")));
            // 
            // ConnForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axMsRdpClient81);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConnForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.ConnForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.axMsRdpClient81)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSTSCLib.AxMsRdpClient8 axMsRdpClient81;
    }
}