using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace MovieOrganiser2
{
    public partial class Form6 : Form
    {
        WebClient wc = new WebClient();
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.opensubtitles.org/nl/116");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
            Uri imageurl = new Uri(textBox1.Text);
            wc.DownloadFileAsync(imageurl, "Superman.sub");
        }

        private void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download compleet!");
        }
    }
}
