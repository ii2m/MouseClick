using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;



namespace MouseClick2
{
    public partial class Form1 : Form
    {


        #region API

        [DllImport("user32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
        [Flags]
        enum MouseEventFlag : uint//鼠标按键的ASCLL码
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        #endregion 
        #region API
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bvk, byte bskan, uint data, uint extraInfo);
        [DllImport ("user32.dll")]
        static extern Byte MapVirtualKey(int wcode, int wMap);
        [Flags]
        enum uint1:uint
        {
            keydown=0x1,
            keyup=0x2
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        // 存放着鼠标左键点击次数
        int ClickCount = 0;
        // 设置秒数
        int sec = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (cmbTime.Text.Trim() == "分")
            {
                sec = (int)(float.Parse(textInterval.Text) * 60); //设置倒计时
                timer1.Interval = (int)(float.Parse(textInterval.Text) * 60 * 1000);
            }
            else if (cmbTime.Text.Trim() == "秒")
            {
                sec = (int)(float.Parse(textInterval.Text)); //设置倒计时
                timer1.Interval = (int)(float.Parse(textInterval.Text) * 1000);
            }
            //timer1.Interval = (int)(float.Parse(textInterval.Text) * 60 * 1000);

            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.Move, 0, 0, 0, UIntPtr.Zero);

            ClickCount++;//记录鼠标点击次数
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            HotKey.RegisterHotKey(Handle, 103, HotKey.KeyModifiers.Alt, Keys.Q);
            //注册热键F3,id为100
            HotKey.RegisterHotKey(Handle, 100, 0, Keys.F3);
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.None, Keys.F4);
            HotKey.RegisterHotKey(Handle, 105, 0, Keys.F6);
            HotKey.RegisterHotKey(Handle, 106, 0, Keys.F7);
          
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:       //按下F3
                            timer1.Enabled = true;
                            if (cmbTime.Text.Trim() == "分")
                            {
                                sec = (int)(float.Parse(textInterval.Text) * 60); //设置倒计时
                            }
                            else if (cmbTime.Text.Trim() == "秒")
                            {
                                sec = (int)(float.Parse(textInterval.Text)); //设置倒计时
                            }
                            //sec = (int)(float.Parse(textInterval.Text) * 60); //设置倒计时
                            break;
                        case 101:      //按下F4
                            timer1.Enabled = false;
                            sec = 0;
                            break;
                        case 103:    //按下Alt+Q
                            ClickCount = 0;//清空鼠标点击次数
                            this.Close();
                            break;
                        case 105://按下F6,光标开始定位
                            timer4.Enabled = true;
                            timer3.Enabled = false;
                            break;
                        case 106://按下F7,光标取消定位
                            timer4.Enabled = false;
                            timer3.Enabled = true;
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #region
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point point);
        #endregion
        Point point;
        private void timer3_Tick(object sender, EventArgs e)//取消定位
        {
            GetCursorPos(ref point);
            txtX.Text = point.X.ToString();
            txtY.Text = point.Y.ToString();
        }
        #region
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x,int y);
        #endregion
        private void timer4_Tick(object sender, EventArgs e)//开始定位
        {
            SetCursorPos(point.X, point.Y);
        }
        //显示鼠标点击次数
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            //记录点击次数
            if(ClickCount < 0)
            {
                lbldjcs.Text = "0 次";
            }
            else 
            {
                lbldjcs.Text = ClickCount.ToString() + " 次";
            }
            //倒计时
            if (sec > 0)
            {
                sec--;
                lbldjs.Text = sec.ToString() + " 秒";
            }
            else
            {
                lbldjs.Text = "0 秒";
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

    }
}