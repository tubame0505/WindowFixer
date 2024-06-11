using System;
using System.Windows.Forms;

namespace Windowfixer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var form = new Form();
            IntPtr dummy = form.Handle; //【ハンドルを確保】
            Application.Run();
        }
    }
}
