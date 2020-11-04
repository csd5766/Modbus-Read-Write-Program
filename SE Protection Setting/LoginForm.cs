using Modbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ModbusTester
{
    public partial class LoginForm : Form
    {
        public int a = 0;
        public LoginForm()
        {
            InitializeComponent();
        }


        private void textPW_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                loginbtn_Click(sender,e);
                loginbtn.Select();
            }
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            string PW = logintxt.Text;

            if (EmptyCheck())
            {
                if(PW == "its@1234")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("비밀번호가 틀렸습니다");
                }
            }
        }

        private bool EmptyCheck()
        {
            if(string.IsNullOrEmpty(logintxt.Text))
            {
                MessageBox.Show("비밀번호를 입력해 주세요");
                return false;
            }
            return true;
        }
    }
}
