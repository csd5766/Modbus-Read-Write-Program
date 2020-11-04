using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModbusTester
{
    class Program
    {
		[STAThread]
		static void Main(String[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Modbus.frmStart());
		}
	}
}
