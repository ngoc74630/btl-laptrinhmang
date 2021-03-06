using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Board
{
    public partial class SoundOptions : Form
    {
        public SoundOptions()
        {
            InitializeComponent();
            if (VanCo.AmThanh) Sound.Checked = true;
            if (VanCo.NhacNen) BgMusic.Checked = true;
            if (BgMusic.Checked == false)
            {
                path.Enabled = false;
                Browse.Enabled = false;
            }
            path.Text = VanCo.Path_NhacNen;
        }

        private void Sound_CheckedChanged(object sender, EventArgs e)
        {
            if (Sound.Checked == true) VanCo.AmThanh = true;
            else VanCo.AmThanh = false;
        }

        private void BgMusic_CheckedChanged(object sender, EventArgs e)
        {
            if (BgMusic.Checked == true)
            {
                VanCo.NhacNen = true;
                path.Enabled = true;
                Browse.Enabled = true;
            }
            else
            {
                VanCo.NhacNen = false;
                path.Enabled = false;
                Browse.Enabled = false;
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Mp3 Files|*.Mp3";
            openFileDialog1.Title = "Chọn chọn bản nhạc";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "openFileDialog1") path.Text = openFileDialog1.FileName;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            FileStream saveOptions = File.Create("Options.cco");
            StreamWriter fileWriter = new StreamWriter(saveOptions);

            VanCo.Path_NhacNen = path.Text;
            if (VanCo.NhacNen)
            {
                VanCo.mciSendString("close MediaFile", null, 0, IntPtr.Zero);
                VanCo.mciSendString("open \"" + VanCo.Path_NhacNen + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
                VanCo.mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);
            }
            if (!VanCo.NhacNen) VanCo.mciSendString("close MediaFile", null, 0, IntPtr.Zero);
            
            //Ghi options AmThanh
            if(VanCo.AmThanh) fileWriter.WriteLine("1");
            else fileWriter.WriteLine("0");

            //Ghi options NhacNen
            if (VanCo.NhacNen) fileWriter.WriteLine("1");
            else fileWriter.WriteLine("0");
            fileWriter.WriteLine(VanCo.Path_NhacNen);
            fileWriter.Close();
            saveOptions.Close();
            this.Close();
        }        
    }
}