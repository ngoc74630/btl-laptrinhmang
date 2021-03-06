namespace Board
{
    partial class SoundOptions
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
            this.Ok = new System.Windows.Forms.Button();
            this.Sound = new System.Windows.Forms.CheckBox();
            this.BgMusic = new System.Windows.Forms.CheckBox();
            this.Browse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.path = new System.Windows.Forms.MaskedTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(82, 147);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 0;
            this.Ok.Text = "Lưu";
            this.Ok.UseVisualStyleBackColor = true;
            // 
            // Sound
            // 
            this.Sound.AutoSize = true;
            this.Sound.BackColor = System.Drawing.Color.Transparent;
            this.Sound.Location = new System.Drawing.Point(14, 48);
            this.Sound.Name = "Sound";
            this.Sound.Size = new System.Drawing.Size(71, 17);
            this.Sound.TabIndex = 2;
            this.Sound.Text = "Âm thanh";
            this.Sound.UseVisualStyleBackColor = false;
            // 
            // BgMusic
            // 
            this.BgMusic.AutoSize = true;
            this.BgMusic.BackColor = System.Drawing.Color.Transparent;
            this.BgMusic.Location = new System.Drawing.Point(14, 71);
            this.BgMusic.Name = "BgMusic";
            this.BgMusic.Size = new System.Drawing.Size(73, 17);
            this.BgMusic.TabIndex = 3;
            this.BgMusic.Text = "Nhạc nền";
            this.BgMusic.UseVisualStyleBackColor = false;
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(199, 109);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(26, 23);
            this.Browse.TabIndex = 5;
            this.Browse.Text = "...";
            this.Browse.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(11, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Đường dẫn tới file nhạc nền";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(82, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Lưu";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // path
            // 
            this.path.BackColor = System.Drawing.Color.Peru;
            this.path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.path.Location = new System.Drawing.Point(14, 111);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(185, 20);
            this.path.TabIndex = 7;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SoundOptions
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "SoundOptions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.CheckBox Sound;
        private System.Windows.Forms.CheckBox BgMusic;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MaskedTextBox path;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}