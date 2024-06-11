using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Windowfixer
{
    public partial class Form : System.Windows.Forms.Form
    {
        NotifyIcon notifyIcon;
        ContextMenuStrip contextMenuStrip;
        delegate void RecordWindowsDelegate();
        public Form()
        {
            this.ShowInTaskbar = false;
            setComponents();
            InitializeComponent();
        }

        private void setComponents()
        {
            /* タスクトレイアイコン */
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.monitor_computer_13980;
            notifyIcon.Visible = true;
            notifyIcon.Text = "WindowFixer";

            contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Text = "&終了";
            toolStripMenuItem.Click += ToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(toolStripMenuItem);
            notifyIcon.ContextMenuStrip = contextMenuStrip;

            /* 定期実行 */
            var timer_period = 10 * 60 * 1000; // 10min
            System.Timers.Timer timer = new System.Timers.Timer(timer_period);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            RecordWindowInfo();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripRestoreItem_Click(object sender, EventArgs e)
        {
            var item = (MyToolStripMenuItem)sender;
            var windowinfo = new WindowInfo();
            windowinfo.SetWindowInfo(item.window_info);
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            var dt = DateTime.Now;
            if (true)
            {
                try
                {
                    Invoke(new RecordWindowsDelegate(RecordWindowInfo));
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error");
                }
            }
        }

        private void RecordWindowInfo()
        {
            var timestampstr = DateTime.Now.ToString("HH:mm:ss");
            MyToolStripMenuItem toolStripMenuItem = new MyToolStripMenuItem();
            toolStripMenuItem.Text = "&" + timestampstr;
            toolStripMenuItem.Click += ToolStripRestoreItem_Click;
            var windowinfo = new WindowInfo();
            toolStripMenuItem.window_info = windowinfo.GetWindowInfo();
            contextMenuStrip.Items.Add(toolStripMenuItem);
            // 要素を20個にする
            if (contextMenuStrip.Items.Count >= 22)
            {
                contextMenuStrip.Items.RemoveAt(1);
            }
        }



        private class MyToolStripMenuItem : ToolStripMenuItem
        {
            internal List<WindowInfo.INFO> window_info;
        }
    }
}
