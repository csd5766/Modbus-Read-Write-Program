using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ModbusTCP;
using System.Text;
using System.Runtime.InteropServices;
using ModbusTester;
using System.Threading;


namespace Modbus
{

    public class frmStart : System.Windows.Forms.Form
    {

        #region ini파일 값 가져올때 씀 
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);// ini연동을 위한 선언

        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileSectionNames(byte[] IpSections, uint nSize, string IpFileName);
        #endregion

        private ModbusTCP.Master MBmaster;
        private TextBox txtData;
        private Label labData;
        private byte[] data;


        public System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStartAdress;
        private System.Windows.Forms.GroupBox grpExchange;
        private System.Windows.Forms.Button btnReadHoldReg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radBits;
        private System.Windows.Forms.RadioButton radBytes;
        private System.Windows.Forms.RadioButton radWord;
        private System.Windows.Forms.Button btnWriteMultipleReg;
        private Label label4;
        private TextBox txtUnit;
        private System.ComponentModel.IContainer components;

        private int flag = 0;
        private int runflag = 0;

        private string FilePath = "D:\\C#\\SESettingProgram\\Modbus-Read-Write-Program\\c# ini.ini";

        private string[] AddrList = new string[51];
        #region inifile 변수
        public string
            REV_ONOFF,
            Delay_Threshold,
            Delay_Time,
            OC_Status,
            OC_ThreShold,
            OC_Duration,
            OC_INV_Threshold,
            OC_INV_Class,
            UC_ThreShold,
            UC_Duration,
            UC_INV_Threshold,
            UC_INV_Class,
            SC_Threshold,
            SC_Duration,
            Current_PL_ON_OFF,
            Current_PL_Duration,
            Current_UB_Threshold,
            Current_UB_Duration,
            STALL_Threshold,
            STALL_DTIM,
            JAM_Threshold,
            JAM_Duration,
            EF_Threshold,
            EF_Duration,
            EF_InEX,
            EF_CT_Numer,
            EF_CT_Dennom,
            CT_Numer,
            CT_Dennom,
            Voltage_REV_Rated,
            Voltage_REV_ONOFF,
            OV_Threshold,
            OV_Duration,
            UV_Threshold,
            UV_Duration,
            Voltage_PL_Threshold,
            Voltage_PL_Duration,
            Voltage_UB_Threshold,
            Voltage_UB_Duration,
            OverFREQ,
            POWER_RATE,
            OP_Threshold,
            OP_Duration,
            UP_Threshold,
            UP_Duration,
            OPF_Threshold,
            OPF_Duration,
            UPF_Threshold,
            UPF_Duration,
            Har_FREQ,
            Har_DISPAY,
            Har_DISPAY_Voltage;
        #endregion

        private void restartBtn_Click(object sender, EventArgs e) // Device Restart 버튼 
        {
            try
            {
                byte[] singleData = new byte[2];
                ushort ID = 7;
                byte unit = Convert.ToByte(txtUnit.Text);
                ushort StartAddress = 1927;
                //data = GetData(1);
                singleData[0] = 0;
                singleData[1] = 1;
                //txtSize.Text = "1";
                //txtData.Text = data[0].ToString();


                MBmaster.WriteSingleRegister(ID, unit, StartAddress, singleData);
                btnConnect.Visible = true;
                btndiscon.Visible = false;
                stopbtn.Visible = false;
                btnReadHoldReg.Visible = true;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private Button restartBtn;

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Thread(new ThreadStart(() =>
            {
                try
                {
                    var WriteRegister = new WriteRegister();
                    WriteRegister.Show();
                    WriteRegister.ShowDialog();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            ))).Start();

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void btndiscon_Click(object sender, EventArgs e)
        {
            MBmaster.Dispose();
            btnConnect.Visible = true;
            btndiscon.Visible = false;
            stopbtn.Visible = false;
            btnReadHoldReg.Visible = true;
        }

        private Button btndiscon;

        private void stopbtn_Click(object sender, EventArgs e)
        {
            //         try
            //         {
            //	MBmaster.Dispose();
            //}
            //catch(Exception ex)
            //         {
            //	Console.WriteLine(ex);
            //         }


            runflag = 0;
            stopbtn.Visible = false;
            btnReadHoldReg.Visible = true;


        }

        private Button stopbtn;

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private TextBox txtIP;
        private Button btnConnect;
        private Label label1;
        private Button addrBtn;
        private Button loginBtn;
        private Label iplabel;
        private GroupBox grpStart;
        private PictureBox pictureBox2;

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 51; i++)
            {
                string name = i + "txt" + ".Text";
                name = string.Empty;
            }
        }
        private int loginFlag = 0;


        public frmStart()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code
        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStart));
            this.grpData = new System.Windows.Forms.GroupBox();
            this.grpExchange = new System.Windows.Forms.GroupBox();
            this.stopbtn = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.btnWriteMultipleReg = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radWord = new System.Windows.Forms.RadioButton();
            this.radBytes = new System.Windows.Forms.RadioButton();
            this.radBits = new System.Windows.Forms.RadioButton();
            this.btnReadHoldReg = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStartAdress = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.addrBtn = new System.Windows.Forms.Button();
            this.loginBtn = new System.Windows.Forms.Button();
            this.iplabel = new System.Windows.Forms.Label();
            this.grpStart = new System.Windows.Forms.GroupBox();
            this.btndiscon = new System.Windows.Forms.Button();
            this.restartBtn = new System.Windows.Forms.Button();
            this.grpExchange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpData
            // 
            this.grpData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpData.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpData.Location = new System.Drawing.Point(8, 225);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(813, 192);
            this.grpData.TabIndex = 9;
            this.grpData.TabStop = false;
            this.grpData.Text = "Data";
            this.grpData.Visible = false;
            // 
            // grpExchange
            // 
            this.grpExchange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpExchange.Controls.Add(this.stopbtn);
            this.grpExchange.Controls.Add(this.pictureBox2);
            this.grpExchange.Controls.Add(this.label4);
            this.grpExchange.Controls.Add(this.txtUnit);
            this.grpExchange.Controls.Add(this.btnWriteMultipleReg);
            this.grpExchange.Controls.Add(this.groupBox1);
            this.grpExchange.Controls.Add(this.btnReadHoldReg);
            this.grpExchange.Controls.Add(this.label3);
            this.grpExchange.Controls.Add(this.txtSize);
            this.grpExchange.Controls.Add(this.label2);
            this.grpExchange.Controls.Add(this.txtStartAdress);
            this.grpExchange.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpExchange.Location = new System.Drawing.Point(8, 89);
            this.grpExchange.Name = "grpExchange";
            this.grpExchange.Size = new System.Drawing.Size(813, 134);
            this.grpExchange.TabIndex = 12;
            this.grpExchange.TabStop = false;
            this.grpExchange.Text = "Data Exchange";
            this.grpExchange.Visible = false;
            // 
            // stopbtn
            // 
            this.stopbtn.BackColor = System.Drawing.Color.Red;
            this.stopbtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.stopbtn.Location = new System.Drawing.Point(273, 12);
            this.stopbtn.Name = "stopbtn";
            this.stopbtn.Size = new System.Drawing.Size(139, 116);
            this.stopbtn.TabIndex = 28;
            this.stopbtn.Text = "Stop";
            this.stopbtn.UseVisualStyleBackColor = false;
            this.stopbtn.Visible = false;
            this.stopbtn.Click += new System.EventHandler(this.stopbtn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ModbusTester.Properties.Resources.UYeGLogo;
            this.pictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.InitialImage")));
            this.pictureBox2.Location = new System.Drawing.Point(563, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(244, 116);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(7, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "Unit";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(85, 27);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(60, 21);
            this.txtUnit.TabIndex = 24;
            this.txtUnit.Text = "0";
            this.txtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnWriteMultipleReg
            // 
            this.btnWriteMultipleReg.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWriteMultipleReg.Location = new System.Drawing.Point(418, 12);
            this.btnWriteMultipleReg.Name = "btnWriteMultipleReg";
            this.btnWriteMultipleReg.Size = new System.Drawing.Size(139, 116);
            this.btnWriteMultipleReg.TabIndex = 23;
            this.btnWriteMultipleReg.Text = "Write multiple register";
            this.btnWriteMultipleReg.Click += new System.EventHandler(this.btnWriteMultipleReg_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radWord);
            this.groupBox1.Controls.Add(this.radBytes);
            this.groupBox1.Controls.Add(this.radBits);
            this.groupBox1.Location = new System.Drawing.Point(156, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 97);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show as";
            // 
            // radWord
            // 
            this.radWord.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radWord.Location = new System.Drawing.Point(16, 67);
            this.radWord.Name = "radWord";
            this.radWord.Size = new System.Drawing.Size(80, 23);
            this.radWord.TabIndex = 2;
            this.radWord.Text = "Word";
            this.radWord.CheckedChanged += new System.EventHandler(this.ShowAs);
            // 
            // radBytes
            // 
            this.radBytes.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radBytes.Location = new System.Drawing.Point(16, 45);
            this.radBytes.Name = "radBytes";
            this.radBytes.Size = new System.Drawing.Size(80, 22);
            this.radBytes.TabIndex = 1;
            this.radBytes.Text = "Bytes";
            this.radBytes.CheckedChanged += new System.EventHandler(this.ShowAs);
            // 
            // radBits
            // 
            this.radBits.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radBits.Location = new System.Drawing.Point(16, 22);
            this.radBits.Name = "radBits";
            this.radBits.Size = new System.Drawing.Size(80, 23);
            this.radBits.TabIndex = 0;
            this.radBits.Text = "Bits";
            this.radBits.CheckedChanged += new System.EventHandler(this.ShowAs);
            // 
            // btnReadHoldReg
            // 
            this.btnReadHoldReg.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReadHoldReg.Location = new System.Drawing.Point(274, 12);
            this.btnReadHoldReg.Name = "btnReadHoldReg";
            this.btnReadHoldReg.Size = new System.Drawing.Size(139, 116);
            this.btnReadHoldReg.TabIndex = 17;
            this.btnReadHoldReg.Text = "Read holding register";
            this.btnReadHoldReg.Click += new System.EventHandler(this.btnReadHoldReg_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(7, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Size";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(85, 94);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(60, 21);
            this.txtSize.TabIndex = 14;
            this.txtSize.Text = "100";
            this.txtSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(7, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Start Adress";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtStartAdress
            // 
            this.txtStartAdress.Location = new System.Drawing.Point(85, 61);
            this.txtStartAdress.Name = "txtStartAdress";
            this.txtStartAdress.Size = new System.Drawing.Size(60, 21);
            this.txtStartAdress.TabIndex = 12;
            this.txtStartAdress.Text = "0";
            this.txtStartAdress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(112, 27);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(104, 21);
            this.txtIP.TabIndex = 5;
            this.txtIP.Text = "192.168.100.1";
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConnect.Location = new System.Drawing.Point(224, 22);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(104, 31);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "IP Address";
            // 
            // addrBtn
            // 
            this.addrBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.addrBtn.Location = new System.Drawing.Point(355, 23);
            this.addrBtn.Name = "addrBtn";
            this.addrBtn.Size = new System.Drawing.Size(104, 31);
            this.addrBtn.TabIndex = 9;
            this.addrBtn.Text = "AddrLoad";
            this.addrBtn.Click += new System.EventHandler(this.readBtn_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.loginBtn.Location = new System.Drawing.Point(470, 23);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(104, 31);
            this.loginBtn.TabIndex = 10;
            this.loginBtn.Text = "Admin Login";
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // iplabel
            // 
            this.iplabel.AutoSize = true;
            this.iplabel.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.iplabel.ForeColor = System.Drawing.Color.Red;
            this.iplabel.Location = new System.Drawing.Point(584, -15);
            this.iplabel.Name = "iplabel";
            this.iplabel.Size = new System.Drawing.Size(0, 16);
            this.iplabel.TabIndex = 12;
            // 
            // grpStart
            // 
            this.grpStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStart.Controls.Add(this.restartBtn);
            this.grpStart.Controls.Add(this.iplabel);
            this.grpStart.Controls.Add(this.btndiscon);
            this.grpStart.Controls.Add(this.loginBtn);
            this.grpStart.Controls.Add(this.addrBtn);
            this.grpStart.Controls.Add(this.label1);
            this.grpStart.Controls.Add(this.btnConnect);
            this.grpStart.Controls.Add(this.txtIP);
            this.grpStart.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpStart.Location = new System.Drawing.Point(8, 25);
            this.grpStart.Name = "grpStart";
            this.grpStart.Size = new System.Drawing.Size(813, 60);
            this.grpStart.TabIndex = 11;
            this.grpStart.TabStop = false;
            this.grpStart.Text = "Start communication";
            // 
            // btndiscon
            // 
            this.btndiscon.BackColor = System.Drawing.Color.Red;
            this.btndiscon.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btndiscon.Location = new System.Drawing.Point(224, 21);
            this.btndiscon.Name = "btndiscon";
            this.btndiscon.Size = new System.Drawing.Size(104, 31);
            this.btndiscon.TabIndex = 13;
            this.btndiscon.Text = "DisConnect";
            this.btndiscon.UseVisualStyleBackColor = false;
            this.btndiscon.Visible = false;
            this.btndiscon.Click += new System.EventHandler(this.btndiscon_Click);
            // 
            // restartBtn
            // 
            this.restartBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.restartBtn.Location = new System.Drawing.Point(583, 23);
            this.restartBtn.Name = "restartBtn";
            this.restartBtn.Size = new System.Drawing.Size(131, 31);
            this.restartBtn.TabIndex = 14;
            this.restartBtn.Text = "Device Restart";
            this.restartBtn.Click += new System.EventHandler(this.restartBtn_Click);
            // 
            // frmStart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(833, 419);
            this.Controls.Add(this.grpExchange);
            this.Controls.Add(this.grpStart);
            this.Controls.Add(this.grpData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStart";
            this.Text = "UYeGSE Modbus R/W";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmStart_Closing);
            this.Load += new System.EventHandler(this.frmStart_Load);
            this.SizeChanged += new System.EventHandler(this.frmStart_Resize);
            this.grpExchange.ResumeLayout(false);
            this.grpExchange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.grpStart.ResumeLayout(false);
            this.grpStart.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        #region Main
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Modbus.frmStart());
        }
        #endregion


        private string[] GetIniSectionNames()
        {
            byte[] ba = new byte[255];
            uint Flag = GetPrivateProfileSectionNames(ba, 255, FilePath);
            return Encoding.Default.GetString(ba).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetIniValue(string Section, string Key, string iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "NOT VALUE", temp, 255, iniPath);
            return temp.ToString();
        }


        // ------------------------------------------------------------------------
        // Programm start
        // ------------------------------------------------------------------------
        private void frmStart_Load(object sender, System.EventArgs e)
        {

            // Set standard format byte, make some textboxes
            radBytes.Checked = true;
            txtIP.Text = "192.168.100.31";
            txtStartAdress.Text = "1300";
            txtSize.Text = "50";
            data = new byte[0];
            ResizeData();
        }

        // ------------------------------------------------------------------------
        // Programm stop
        // ------------------------------------------------------------------------
        private void frmStart_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MBmaster != null)
            {
                MBmaster.Dispose();
                MBmaster = null;
            }
            Application.Exit();
        }

        // ------------------------------------------------------------------------
        // ini파일 세팅
        // ------------------------------------------------------------------------

        private void IniSet(string ip)
        {


            AddrList[0] = GetIniValue(txtIP.Text, "REV_ONOFF", FilePath);
            AddrList[1] = GetIniValue(txtIP.Text, "Delay_Threshold", FilePath);
            AddrList[2] = GetIniValue(txtIP.Text, "Delay_Time", FilePath);
            AddrList[3] = GetIniValue(txtIP.Text, "OC_Status", FilePath);
            AddrList[4] = GetIniValue(txtIP.Text, "OC_ThreShold", FilePath);
            AddrList[5] = GetIniValue(txtIP.Text, "OC_Duration", FilePath);
            AddrList[6] = GetIniValue(txtIP.Text, "OC_INV_Threshold", FilePath);
            AddrList[7] = GetIniValue(txtIP.Text, "OC_INV_Class", FilePath);
            AddrList[8] = GetIniValue(txtIP.Text, "UC_ThreShold", FilePath);
            AddrList[9] = GetIniValue(txtIP.Text, "UC_Duration", FilePath);
            AddrList[10] = GetIniValue(txtIP.Text, "SC_Threshold", FilePath);
            AddrList[11] = GetIniValue(txtIP.Text, "SC_Duration", FilePath);
            AddrList[12] = GetIniValue(txtIP.Text, "Current_PL_ON_OFF", FilePath);
            AddrList[13] = GetIniValue(txtIP.Text, "Current_PL_Duration", FilePath);
            AddrList[14] = GetIniValue(txtIP.Text, "Current_UB_Threshold", FilePath);
            AddrList[15] = GetIniValue(txtIP.Text, "Current_UB_Duration", FilePath);
            AddrList[16] = GetIniValue(txtIP.Text, "STALL_Threshold", FilePath);
            AddrList[17] = GetIniValue(txtIP.Text, "STALL_DTIM", FilePath);
            AddrList[18] = GetIniValue(txtIP.Text, "JAM_Threshold", FilePath);
            AddrList[19] = GetIniValue(txtIP.Text, "JAM_Duration", FilePath);
            AddrList[20] = GetIniValue(txtIP.Text, "EF_Threshold", FilePath);
            AddrList[21] = GetIniValue(txtIP.Text, "EF_Duration", FilePath);
            AddrList[22] = GetIniValue(txtIP.Text, "EF_InEX", FilePath);
            AddrList[23] = GetIniValue(txtIP.Text, "EF_CT_Numer", FilePath);
            AddrList[24] = GetIniValue(txtIP.Text, "EF_CT_Dennom", FilePath);
            AddrList[25] = GetIniValue(txtIP.Text, "CT_Numer", FilePath);
            AddrList[26] = GetIniValue(txtIP.Text, "CT_Dennom", FilePath);
            AddrList[27] = GetIniValue(txtIP.Text, "Voltage_REV_Rated", FilePath);
            AddrList[28] = GetIniValue(txtIP.Text, "Voltage_REV_ONOFF", FilePath);
            AddrList[29] = GetIniValue(txtIP.Text, "OV_Threshold", FilePath);
            AddrList[30] = GetIniValue(txtIP.Text, "OV_Duration", FilePath);
            AddrList[31] = GetIniValue(txtIP.Text, "UV_Threshold", FilePath);
            AddrList[32] = GetIniValue(txtIP.Text, "UV_Duration", FilePath);
            AddrList[33] = GetIniValue(txtIP.Text, "Voltage_PL_Threshold", FilePath);
            AddrList[34] = GetIniValue(txtIP.Text, "Voltage_PL_Duration", FilePath);
            AddrList[35] = GetIniValue(txtIP.Text, "Voltage_UB_Threshold", FilePath);
            AddrList[36] = GetIniValue(txtIP.Text, "Voltage_UB_Duration", FilePath);
            AddrList[37] = GetIniValue(txtIP.Text, "OverFREQ", FilePath);
            AddrList[38] = GetIniValue(txtIP.Text, "POWER_RATE", FilePath);
            AddrList[39] = GetIniValue(txtIP.Text, "OP_Threshold", FilePath);
            AddrList[40] = GetIniValue(txtIP.Text, "OP_Duration", FilePath);
            AddrList[41] = GetIniValue(txtIP.Text, "UP_Threshold", FilePath);
            AddrList[42] = GetIniValue(txtIP.Text, "UP_Duration", FilePath);
            AddrList[43] = GetIniValue(txtIP.Text, "OPF_Threshold", FilePath);
            AddrList[44] = GetIniValue(txtIP.Text, "OPF_Duration", FilePath);
            AddrList[45] = GetIniValue(txtIP.Text, "UPF_Threshold", FilePath);
            AddrList[46] = GetIniValue(txtIP.Text, "UPF_Duration", FilePath);
            AddrList[47] = GetIniValue(txtIP.Text, "Har_FREQ", FilePath);
            AddrList[48] = GetIniValue(txtIP.Text, "Har_DISPAY", FilePath);
            AddrList[49] = GetIniValue(txtIP.Text, "Har_DISPAY_Voltage", FilePath);
        }
        // ------------------------------------------------------------------------
        // Button connect
        // ------------------------------------------------------------------------
        private void btnConnect_Click(object sender, System.EventArgs e)
        {
            //(new Thread(new ThreadStart(() => // 소켓 통신을 실시간으로 돌리기 위해서 Thread 사용 ! 
            //{

            try
            {
                IniSet(txtIP.Text);

                string[] section = GetIniSectionNames();
                foreach (string a in section)
                {
                    Console.WriteLine(a);
                }
                // Create new modbus master and add event functions
                MBmaster = new Master(txtIP.Text, 502, true);
                MBmaster.OnResponseData += new ModbusTCP.Master.ResponseData(MBmaster_OnResponseData);
                MBmaster.OnException += new ModbusTCP.Master.ExceptionData(MBmaster_OnException);
                // Show additional fields, enable watchdog

                if (MBmaster.conflag == 1)
                {
                    MessageBox.Show("연결 성공");
                    grpExchange.Visible = true;
                    grpData.Visible = true;
                    flag = 1;
                    //loginFlag = 0;
                    iplabel.Text = "IP Status :" + txtIP.Text;
                    iplabel.Visible = true;
                    btndiscon.Visible = true;
                    btnConnect.Visible = false;
                }
                else
                {
                    MessageBox.Show("연결 상태를 확인 해 주세요");
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //}
            //))).Start();
        }

        // ------------------------------------------------------------------------
        // Athentification 
        // ------------------------------------------------------------------------
        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                LoginForm dlg = new LoginForm();
                DialogResult Result = dlg.ShowDialog();


                byte[] singleData = new byte[2];
                if (Result == DialogResult.OK)
                {
                    ushort ID = 7;
                    byte unit = Convert.ToByte(txtUnit.Text);
                    ushort StartAddress = 718;
                    //data = GetData(1);
                    singleData[0] = 30;
                    singleData[1] = 125;
                    //txtSize.Text = "1";
                    //txtData.Text = data[0].ToString();

                    loginFlag = 1;
                    loginBtn.Text = "로그온";
                    loginBtn.Enabled = false;
                    MBmaster.WriteSingleRegister(ID, unit, StartAddress, singleData);
                }
            }
            else MessageBox.Show("연결 상태를 확인 해 주세요");

        }

        // ------------------------------------------------------------------------
        // Read iniFile
        // ------------------------------------------------------------------------ 
        private void readBtn_Click(object sender, EventArgs e)
        {
            if (flag == 1 && loginFlag == 0)
            {
                ResizeData();
                MessageBox.Show("관리자 인증번호를 입력 해 주세요");
            }

        }



        // ------------------------------------------------------------------------
        // Button read holding register
        // ------------------------------------------------------------------------
        private void btnReadHoldReg_Click(object sender, System.EventArgs e)
        {
            ushort ID = 3;
            byte unit = Convert.ToByte(txtUnit.Text);
            ushort StartAddress = ReadStartAdr();
            UInt16 Length = Convert.ToUInt16(txtSize.Text);
            runflag = 1;
            btnReadHoldReg.Visible = false;
            stopbtn.Visible = true;
            int b = 0;
            (new Thread(new ThreadStart(() => // 소켓 통신을 실시간으로 돌리기 위해서 Thread 사용 ! 
            {
                while (true)
                {
                    Thread.Sleep(100);
                    try
                    {
                        MBmaster.ReadHoldingRegister(ID, unit, StartAddress, Length);
                    }
                    catch (Exception a)
                    {
                        Console.WriteLine(ID + "-" + unit + "-" + StartAddress + "-" + Length);
                        Console.WriteLine(a);
                    }

                    if (runflag == 0) break;


                }
            }
            ))).Start();


        }
        // ------------------------------------------------------------------------
        // Button write multiple register
        // ------------------------------------------------------------------------	
        private void btnWriteMultipleReg_Click(object sender, System.EventArgs e)
        {
            if (loginFlag == 1)
            {
                ushort ID = 8;
                byte unit = Convert.ToByte(txtUnit.Text);
                ushort StartAddress = ReadStartAdr();

                data = GetData(Convert.ToByte(txtSize.Text));
                MBmaster.WriteMultipleRegister(ID, unit, StartAddress, data);
            }
            else
            {
                MessageBox.Show("관리자 인증을 해주세요");
            }

        }

        // ------------------------------------------------------------------------
        // Event for response data
        // ------------------------------------------------------------------------
        private void MBmaster_OnResponseData(ushort ID, byte unit, byte function, byte[] values)
        {
            // ------------------------------------------------------------------
            // Seperate calling threads
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Master.ResponseData(MBmaster_OnResponseData), new object[] { ID, unit, function, values });
                return;
            }

            // ------------------------------------------------------------------------
            // Identify requested data

            switch (ID)
            {

                case 1:
                    grpData.Text = "Read coils";
                    data = values;

                    ShowAs(null, null);
                    break;
                case 2:
                    grpData.Text = "Read discrete inputs";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 3:
                    grpData.Text = "Read holding register";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 4:
                    grpData.Text = "Read input register";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 5:
                    grpData.Text = "Write single coil";
                    break;
                case 6:
                    grpData.Text = "Write multiple coils";
                    break;
                case 7:
                    grpData.Text = "Write single register";
                    break;
                case 8:
                    grpData.Text = "Write multiple register";
                    break;
            }
        }

        // ------------------------------------------------------------------------
        // Modbus TCP slave exception
        // ------------------------------------------------------------------------
        private void MBmaster_OnException(ushort id, byte unit, byte function, byte exception)
        {
            string exc = "Modbus says error: ";
            switch (exception)
            {
                case Master.excIllegalFunction: exc += "Illegal function!"; break;
                case Master.excIllegalDataAdr: exc += "Illegal data adress!"; break;
                case Master.excIllegalDataVal: exc += "Illegal data value!"; break;
                case Master.excSlaveDeviceFailure: exc += "Slave device failure!"; break;
                case Master.excAck: exc += "Acknoledge!"; break;
                case Master.excGatePathUnavailable: exc += "Gateway path unavailbale!"; break;
                case Master.excExceptionTimeout: exc += "Slave timed out!"; break;
                case Master.excExceptionConnectionLost: exc += "Connection is lost!"; break;
                case Master.excExceptionNotConnected: exc += "Not connected!"; break;
            }

            MessageBox.Show(exc, "Modbus slave exception");
        }

        // ------------------------------------------------------------------------
        // Generate new number of text boxes
        // ------------------------------------------------------------------------
        private void ResizeData() // Connect 후 라벨과 텍스트박스 생성 해주는 곳 
        {
            // Create as many textboxes as fit into window
            grpData.Controls.Clear();
            int x = 0;
            int y = 10;
            int z = 20;
            while (y < grpData.Size.Width - 100)
            {
                labData = new Label();
                grpData.Controls.Add(labData);
                labData.Size = new System.Drawing.Size(30, 20);
                labData.Location = new System.Drawing.Point(y, z);
                labData.Text = Convert.ToString(x + 1);

                txtData = new TextBox();
                grpData.Controls.Add(txtData);
                txtData.Size = new System.Drawing.Size(50, 20);
                txtData.Location = new System.Drawing.Point(y + 30, z);
                txtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                txtData.Tag = x;
                txtData.Name = x + "txt";

                if (x < AddrList.Length && loginFlag == 1)
                    txtData.Text = AddrList[x];
                Console.WriteLine(AddrList[x]);

                x++;
                z = z + txtData.Size.Height + 5;
                if (z > grpData.Size.Height - 40)
                {
                    y = y + 100;
                    z = 20;
                }
            }
        }

        // ------------------------------------------------------------------------
        // Resize form elements
        // ------------------------------------------------------------------------
        private void frmStart_Resize(object sender, System.EventArgs e)
        {
            if (grpData.Visible == true) ResizeData();
        }

        // ------------------------------------------------------------------------
        // Read start address
        // ------------------------------------------------------------------------
        private ushort ReadStartAdr()
        {
            // Convert hex numbers into decimal
            if (txtStartAdress.Text.IndexOf("0x", 0, txtStartAdress.Text.Length) == 0)
            {
                string str = txtStartAdress.Text.Replace("0x", "");
                ushort hex = Convert.ToUInt16(str, 16);
                return hex;
            }
            else
            {
                return Convert.ToUInt16(txtStartAdress.Text);
            }
        }

        // ------------------------------------------------------------------------
        // Read values from textboxes
        // ------------------------------------------------------------------------
        public byte[] GetData(int num)
        {
            bool[] bits = new bool[num];
            byte[] data = new Byte[num];
            int[] word = new int[num];

            // ------------------------------------------------------------------------
            // Convert data from text boxes
            foreach (Control ctrl in grpData.Controls) // GetUpperBound length랑 비슷함 근데 length가 +1 많음 
            {
                if (ctrl is TextBox)
                {
                    int x = Convert.ToInt16(ctrl.Tag);
                    if (radBits.Checked)
                    {
                        if ((x <= bits.GetUpperBound(0)) && (ctrl.Text != "")) bits[x] = Convert.ToBoolean(Convert.ToByte(ctrl.Text));
                        else break;
                    }
                    if (radBytes.Checked)
                    {
                        if ((x <= data.GetUpperBound(0)) && (ctrl.Text != "")) data[x] = Convert.ToByte(ctrl.Text);
                        else break;
                    }
                    if (radWord.Checked) // Word가 체크 되어 있고 
                    {
                        if ((x <= data.GetUpperBound(0)) && (ctrl.Text != "")) // data Text가 비어있지 않으면 
                        {
                            try { word[x] = Convert.ToInt16(ctrl.Text); }
                            catch (SystemException) { word[x] = Convert.ToUInt16(ctrl.Text); };
                        }
                        else break;
                    }
                }
            }
            if (radBits.Checked)
            {
                int numBytes = (num / 8 + (num % 8 > 0 ? 1 : 0));
                data = new Byte[numBytes];
                BitArray bitArray = new BitArray(bits);
                bitArray.CopyTo(data, 0);
            }
            if (radWord.Checked)
            {
                data = new Byte[num * 2]; // byte닌까 2배 곱함 
                for (int x = 0; x < num; x++)
                {
                    //1.int의 호스트 오더를 네트웍 오더로 바꾼다. - IPAddress.HostToNetworkOrder();

                    //2.int를 Byte Stream으로 바꾼다. - BitConverter.GetBytes();
                    byte[] dat = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)word[x]));
                    //label5.Text = Convert.ToString(IPAddress.HostToNetworkOrder((short)word[x]) + "-" + dat[1]);

                    data[x * 2] = dat[0];
                    data[x * 2 + 1] = dat[1];

                }
            }
            return data;
        }

        // ------------------------------------------------------------------------
        // Show values in selected way
        // ------------------------------------------------------------------------
        private void ShowAs(object sender, System.EventArgs e)
        {
            RadioButton rad;
            if (sender is RadioButton)
            {
                rad = (RadioButton)sender;
                if (rad.Checked == false) return;
            }

            bool[] bits = new bool[1];
            int[] word = new int[1];

            // Convert data to selected data type
            if (radBits.Checked == true)
            {
                BitArray bitArray = new BitArray(data);
                bits = new bool[bitArray.Count];
                bitArray.CopyTo(bits, 0);
            }
            if (radWord.Checked == true)
            {
                if (data.Length < 2) return;
                int length = data.Length / 2 + Convert.ToInt16(data.Length % 2 > 0);
                word = new int[length];
                for (int x = 0; x < length; x += 1)
                {
                    word[x] = data[x * 2] * 256 + data[x * 2 + 1];
                }
            }

            // ------------------------------------------------------------------------
            // Put new data into text boxes
            foreach (Control ctrl in grpData.Controls)
            {
                if (ctrl is TextBox)
                {
                    int x = Convert.ToInt16(ctrl.Tag);
                    if (radBits.Checked)
                    {
                        if (x <= bits.GetUpperBound(0))
                        {
                            ctrl.Text = Convert.ToByte(bits[x]).ToString();
                            ctrl.Visible = true;
                        }
                        else ctrl.Text = "";
                    }
                    if (radBytes.Checked)
                    {
                        if (x <= data.GetUpperBound(0))
                        {
                            ctrl.Text = data[x].ToString();
                            ctrl.Visible = true;
                        }
                        else ctrl.Text = "";
                    }
                    if (radWord.Checked)
                    {
                        if (x <= word.GetUpperBound(0))
                        {
                            ctrl.Text = word[x].ToString();

                            ctrl.Visible = true;
                        }
                        else ctrl.Text = "";
                    }
                }
            }
        }
    }
}
