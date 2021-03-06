using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Board
{
    public partial class ChessBoard : Form
    {
        #region Khởi tạo form
        private bool dangkeo;
        private Point diemkeo;
        private int new_t = 0;//Kiểm tra NewGame đc click chưa
        private int open_t = 0;//Kiểm tra Open đc click chưa
        private int exit_t = 0;//Kiểm tra Exit đã đc click chưa
        private int save_t = 0;
        NewGame form_NewGame = new NewGame();
        

        //Khai báo Timer đếm thời gian cho 2 người chơi
        public static Timer Timer_NguoiChoi1 = new Timer();
        public static Timer Timer_NguoiChoi2 = new Timer();
        

        public ChessBoard()
        {           
            InitializeComponent();
            
            //Đọc dữ liệu đã lưu vào Options
            StreamReader OptionsReader = File.OpenText("Options.cco");
            if (OptionsReader.ReadLine() == "1") VanCo.AmThanh = true;
            else VanCo.AmThanh = false;
            if (OptionsReader.ReadLine() == "1") VanCo.NhacNen = true;
            else VanCo.NhacNen = false;
            VanCo.Path_NhacNen = OptionsReader.ReadLine();
            OptionsReader.Close();

            //Kiểm tra và chơi nhạc nền
            VanCo.PlayNhacNen(VanCo.NhacNen);            

            VanCo.BackBuffer = new Bitmap(this.Width, this.Height);
            Bitmap bg = new Bitmap(Board.Properties.Resources.bg);
            Graphics g = Graphics.FromImage(VanCo.BackBuffer);
            g.Clear(this.BackColor);
            g.DrawImage(bg, 0, 0);
            g.Dispose();                   
            
            this.LuuVanCo.Visible = false;   
         
            //Thiết lập cho Timer
            Timer_NguoiChoi1.Tick += new EventHandler(Timer_NguoiChoi1_Tick);
            Timer_NguoiChoi2.Tick += new EventHandler(Timer_NguoiChoi2_Tick);
            Timer_NguoiChoi1.Interval = 1000;
            Timer_NguoiChoi2.Interval = 1000;

            //Flash screen
            FlashScreen FlashScr = new FlashScreen();
            FlashScr.ShowDialog();
            this.Refresh();
        }

        #region Dùng chuột di chuyển form
        private void ChessBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dangkeo = true;
                diemkeo = new Point(e.X, e.Y);
            }
            else dangkeo = false;            
        }

        private void ChessBoard_MouseUp(object sender, MouseEventArgs e)
        {
            dangkeo = false;            
        }

        private void ChessBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (dangkeo)
            {
                Point diemden;
                diemden = this.PointToScreen(new Point(e.X, e.Y));
                diemden.Offset(-diemkeo.X, -diemkeo.Y);
                this.Location = diemden;
            }
        }
        #endregion

        #region Vẽ lại form background từ buffer
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do nothing             
        }
        protected override void OnPaint(PaintEventArgs e)
        {            
            e.Graphics.DrawImage(VanCo.BackBuffer, 0, 0, this.Width, this.Height);
        }
        #endregion
        #endregion

        #region Menu Ok, Cancel, Close panel của panel LuuVanCo
        private void Ok_MouseEnter(object sender, EventArgs e)
        {
            Ok.Image = Board.Properties.Resources.Ok_MouseOver;
        }
        private void Ok_MouseLeave(object sender, EventArgs e)
        {
            Ok.Image = Board.Properties.Resources.Ok;
        }
        private void Ok_MouseClick(object sender, MouseEventArgs e)
        {

            if (new_t == 1 && open_t == 0 && exit_t == 0 && save_t == 0)
            {
                new_t = 0;
                this.LuuVanCo.Visible = false;
                Save_MouseClick(sender, e);
                form_NewGame.ShowDialog(this);
                TenQuanDo.Text = VanCo.TenNguoiChoi1;
                TenQuanDen.Text = VanCo.TenNguoiChoi2;
                AddQuanCo();
            }
            if (new_t == 0 && open_t == 1 && exit_t == 0 && save_t == 0)
            {
                open_t = 0;
                this.LuuVanCo.Visible = false;
                VanCo.Save();

                //Mở OpenDialog
                /*string SourcePath;
                openFileDialog1.Filter = "Chinese Chess Board file (*.ccb)|*.ccb";
                openFileDialog1.Title = "Load ván cờ";
                openFileDialog1.Multiselect = false;
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName != "")
                {
                    //Khởi tạo bàn cờ trống
                    BanCo.ResetBanCo();

                    SourcePath = openFileDialog1.FileName;
                    VanCo.Open(SourcePath);
                    TenQuanDo.Text = VanCo.TenNguoiChoi1;
                    TenQuanDen.Text = VanCo.TenNguoiChoi2;
                }*/
                Open form_Open = new Open();
                form_Open.ShowDialog(this);
                TenQuanDo.Text = VanCo.TenNguoiChoi1;
                TenQuanDen.Text = VanCo.TenNguoiChoi2;
                if (VanCo.NguoiChoi1_phut >= 10)
                {
                    if (VanCo.NguoiChoi1_giay >= 10)
                    {
                        VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                    }
                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                }
                if (VanCo.NguoiChoi1_phut < 10)
                {
                    if (VanCo.NguoiChoi1_giay >= 10) VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                }

                if (VanCo.NguoiChoi2_phut >= 10)
                {
                    if (VanCo.NguoiChoi2_giay >= 10)
                    {
                        VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                    }
                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                }
                if (VanCo.NguoiChoi2_phut < 10)
                {
                    if (VanCo.NguoiChoi2_giay >= 10) VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                }

                //Mở lại bộ đếm thời gian
                DemThoiGian();

                //Mở lại bàn cờ
                MoBanCo();
            }
            if (new_t == 0 && open_t == 0 && exit_t == 1 && save_t == 0)
            {
                exit_t = 0;
                this.LuuVanCo.Visible = false;
                VanCo.Save();
                this.Close();
            }
            if (new_t == 0 && open_t == 0 && exit_t == 0 && save_t == 1)
            {
                save_t = 0;
                this.LuuVanCo.Visible = false;
                VanCo.Save();
                //Mở lại bộ đếm thời gian
                DemThoiGian();

                //Mở lại bàn cờ
                MoBanCo();
            }
        }

        private void Cancel_MouseEnter(object sender, EventArgs e)
        {
            Cancel.Image = Board.Properties.Resources.Cancel_MouseOver;
        }
        private void Cancel_MouseLeave(object sender, EventArgs e)
        {
            Cancel.Image = Board.Properties.Resources.Cancel;
        }
        private void Cancel_MouseClick(object sender, MouseEventArgs e)
        {

            if (new_t == 1 && open_t == 0 && exit_t == 0 && save_t == 0)
            {
                this.LuuVanCo.Visible = false;               
                new_t = 0;          
                form_NewGame.ShowDialog(this);  
                TenQuanDo.Text = VanCo.TenNguoiChoi1;
                TenQuanDen.Text = VanCo.TenNguoiChoi2;
                AddQuanCo();                
            }
            if (new_t == 0 && open_t == 1 && exit_t == 0 && save_t == 0)
            {
                this.LuuVanCo.Visible = false;
                open_t = 0;

                //Mở OpenDialog
                /*string SourcePath;
                openFileDialog1.Filter = "Chinese Chess Board file (*.ccb)|*.ccb";
                openFileDialog1.Title = "Load ván cờ";
                openFileDialog1.Multiselect = false;
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName != "")
                {
                    //Khởi tạo bàn cờ trống
                    BanCo.ResetBanCo();

                    SourcePath = openFileDialog1.FileName;
                    VanCo.Open(SourcePath);
                    TenQuanDo.Text = VanCo.TenNguoiChoi1;
                    TenQuanDen.Text = VanCo.TenNguoiChoi2;
                }*/
                Open form_Open = new Open();
                form_Open.ShowDialog(this);
                TenQuanDo.Text = VanCo.TenNguoiChoi1;
                TenQuanDen.Text = VanCo.TenNguoiChoi2;
                if (VanCo.NguoiChoi1_phut >= 10)
                {
                    if (VanCo.NguoiChoi1_giay >= 10)
                    {
                        VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                    }
                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                }
                if (VanCo.NguoiChoi1_phut < 10)
                {
                    if (VanCo.NguoiChoi1_giay >= 10) VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                }

                if (VanCo.NguoiChoi2_phut >= 10)
                {
                    if (VanCo.NguoiChoi2_giay >= 10)
                    {
                        VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                    }
                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                }
                if (VanCo.NguoiChoi2_phut < 10)
                {
                    if (VanCo.NguoiChoi2_giay >= 10) VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                }

                //Mở lại bộ đếm thời gian
                DemThoiGian();

                //Mở lại bàn cờ
                MoBanCo();
            }
            if (new_t == 0 && open_t == 0 && exit_t == 1 && save_t == 0)
            {
                exit_t = 0;
                this.LuuVanCo.Visible = false;                
                this.Close();
            }
            if (new_t == 0 && open_t == 0 && exit_t == 0 && save_t == 1)
            {
                save_t = 0;
                this.LuuVanCo.Visible = false;
                
                //Mở lại bộ đếm thời gian
                DemThoiGian();

                //Mở lại bàn cờ
                MoBanCo();
            }
        }

        private void Closepnl_MouseEnter(object sender, EventArgs e)
        {
            Closepnl.Image = Board.Properties.Resources.Close_MouseOver;
        }
        private void Closepnl_MouseLeave(object sender, EventArgs e)
        {
            Closepnl.Image = Board.Properties.Resources.Close;
        }
        private void Closepnl_MouseClick(object sender, MouseEventArgs e)
        {
            this.LuuVanCo.Visible = false;
            new_t = 0;
            open_t = 0;
            exit_t = 0;

            //Mở lại bộ đếm thời gian
            DemThoiGian();

            //Mở lại bàn cờ
            MoBanCo();
        }
        #endregion        

        #region Menu Xin đi lại, Chịu thua của panel VanCo.ChieuBi và Chơi lại, Thoát của panel KetQua, Chấp cờ
        private void ChapXong_MouseEnter(object sender, EventArgs e)
        {
            VanCo.ChapXong.Image = Board.Properties.Resources.ChapXong_MouseOver;
        }

        private void ChapXong_MouseLeave(object sender, EventArgs e)
        {
            VanCo.ChapXong.Image = Board.Properties.Resources.ChapXong;
        }

        private void ChapXong_MouseClick(object sender, MouseEventArgs e)
        {
            VanCo.Chap = false;
            VanCo.ChapCo.Visible = false;
            DemThoiGian();
        }

        private void DiLai_MouseEnter(object sender, EventArgs e)
        {
            VanCo.DiLai.Image = Board.Properties.Resources.DiLai_MouseOver;
        }
        private void DiLai_MouseLeave(object sender, EventArgs e)
        {
            VanCo.DiLai.Image = Board.Properties.Resources.DiLai;
        }
        private void DiLai_MouseClick(object sender, MouseEventArgs e)
        {
            VanCo.Undo();
            VanCo.Undo();
            VanCo.ChieuBi.Visible = false;
            VanCo.winner = 2;
        }

        private void ChiuThua_MouseEnter(object sender, EventArgs e)
        {
            VanCo.ChiuThua.Image = Board.Properties.Resources.ChiuThua_MouseOver;
        }
        private void ChiuThua_MouseLeave(object sender, EventArgs e)
        {
            VanCo.ChiuThua.Image = Board.Properties.Resources.ChiuThua;
        }
        private void ChiuThua_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.winner == 0) VanCo.NguoiThang.Text = VanCo.TenNguoiChoi1;
            if (VanCo.winner == 1) VanCo.NguoiThang.Text = VanCo.TenNguoiChoi2;
            VanCo.ChieuBi.Visible = false;
            VanCo.KetQua.Visible = true;
            KhoaBanCo();
        }

        private void ChoiLai_MouseEnter(object sender, EventArgs e)
        {
            VanCo.ChoiLai.Image = Board.Properties.Resources.ChoiLai_MouseOver;
        }
        private void ChoiLai_MouseLeave(object sender, EventArgs e)
        {
            VanCo.ChoiLai.Image = Board.Properties.Resources.ChoiLai;
        }
        private void ChoiLai_MouseClick(object sender, MouseEventArgs e)
        {
            VanCo.KetQua.Visible = false;
            //reset thông số trên bàn cờ      
            VanCo.NguoiChoi1_phut = 15;
            VanCo.NguoiChoi1_giay = 0;
            VanCo.NguoiChoi2_phut = 15;
            VanCo.NguoiChoi2_giay = 0;
            VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 00";
            VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 00";
            ChessBoard.Timer_NguoiChoi1.Enabled = false;
            ChessBoard.Timer_NguoiChoi2.Enabled = false;

            //tạo ván cờ mới
            VanCo.NewGame();
            AddQuanCo();
            if (VanCo.TinhThoiGian) ChessBoard.Timer_NguoiChoi1.Enabled = true;
        }

        private void Thoat_MouseEnter(object sender, EventArgs e)
        {
            VanCo.Thoat.Image = Board.Properties.Resources.Thoat_MouseOver;
        }
        private void Thoat_MouseLeave(object sender, EventArgs e)
        {
            VanCo.Thoat.Image = Board.Properties.Resources.Thoat;
        }
        private void Thoat_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Menu NewGame, Undo, Save, Open, Options, Exit, Minimize của form
        private void NewGame_MouseEnter(object sender, EventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                NewGame.Image = Board.Properties.Resources.Newgame_MouseOver;
            }
        }
        private void NewGame_MouseLeave(object sender, EventArgs e)
        {
            NewGame.Image = Board.Properties.Resources.Newgame;
        }
        private void NewGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                switch (VanCo.DangChoi)
                {
                    case false:
                        form_NewGame.ShowDialog(this);
                        TenQuanDo.Text = VanCo.TenNguoiChoi1;
                        TenQuanDen.Text = VanCo.TenNguoiChoi2;
                        break;
                    case true:
                        this.LuuVanCo.Visible = true;
                        //Tạm ngưng đếm ngược
                        Timer_NguoiChoi1.Enabled = false;
                        Timer_NguoiChoi2.Enabled = false;

                        KhoaBanCo();
                        new_t = 1;
                        break;
                }
                if (VanCo.DangChoi == true)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                        {
                            this.Controls.Add(BanCo.ViTri[i, j].CanMove);
                            BanCo.ViTri[i, j].CanMove.MouseClick += new MouseEventHandler(CanMove_MouseClick);
                        }
                    }
                    AddQuanCo();
                }
            }
        }

        private void Undo_MouseEnter(object sender, EventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                this.Undo.Image = Board.Properties.Resources.Undo_MouseOver;
            }
        }
        private void Undo_MouseLeave(object sender, EventArgs e)
        {
            this.Undo.Image = Board.Properties.Resources.Undo;
        }
        private void Undo_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                //Hoàn lại nước đi
                VanCo.Undo();
            }
        }

        private void Save_MouseEnter(object sender, EventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                this.Save.Image = Board.Properties.Resources.Save_MouseOver;
            }
        }
        private void Save_MouseLeave(object sender, EventArgs e)
        {
            this.Save.Image = Board.Properties.Resources.Save;
        }
        private void Save_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.DangChoi == true && VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                /*string SourcePath;
                saveFileDialog1.Filter = "Chinese Chess Board file (*.ccb)|*.ccb";
                saveFileDialog1.Title = "Save ván cờ";
                if (VanCo.DangChoi == true)
                {
                    saveFileDialog1.ShowDialog();
                    if (saveFileDialog1.FileName != "")
                    {
                        SourcePath = saveFileDialog1.FileName;
                        VanCo.Save(SourcePath);
                    }
                }*/
                //VanCo.Save();
                //Tạm ngưng đếm ngược
                Timer_NguoiChoi1.Enabled = false;
                Timer_NguoiChoi2.Enabled = false;
                KhoaBanCo();
                save_t = 1;
                this.LuuVanCo.Visible = true;
            }
        }

        private void Open_MouseEnter(object sender, EventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                this.Open.Image = Board.Properties.Resources.Open_MouseOver;
            }
        }
        private void Open_MouseLeave(object sender, EventArgs e)
        {
            this.Open.Image = Board.Properties.Resources.Open;
        }
        private void Open_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                /*string SourcePath;
                openFileDialog1.Filter = "Chinese Chess Board file (*.ccb)|*.ccb";
                openFileDialog1.Title = "Load ván cờ";
                openFileDialog1.Multiselect = false;*/

                switch (VanCo.DangChoi)
                {
                    case false:
                        /*openFileDialog1.ShowDialog();
                        if (openFileDialog1.FileName != "")
                        {
                            //Khởi tạo bàn cờ trống
                            BanCo.ResetBanCo();

                            SourcePath = openFileDialog1.FileName;
                            VanCo.Open(SourcePath);*/
                        Open form_Open = new Open();
                        form_Open.ShowDialog(this);
                        TenQuanDo.Text = VanCo.TenNguoiChoi1;
                        TenQuanDen.Text = VanCo.TenNguoiChoi2;

                        if (VanCo.DangChoi)
                        {
                            if (VanCo.LuotDi == 0)
                            {
                                VanCo.P1_Turn.Image = Board.Properties.Resources.Turning;
                                VanCo.P2_Turn.Image = Board.Properties.Resources.NotTurn;
                            }
                            else
                            {
                                VanCo.P1_Turn.Image = Board.Properties.Resources.NotTurn;
                                VanCo.P2_Turn.Image = Board.Properties.Resources.Turning;
                            }
                            if (VanCo.TinhThoiGian == true)
                            {
                                if (VanCo.NguoiChoi1_phut >= 10)
                                {
                                    if (VanCo.NguoiChoi1_giay >= 10)
                                    {
                                        VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                                    }
                                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                                }
                                if (VanCo.NguoiChoi1_phut < 10)
                                {
                                    if (VanCo.NguoiChoi1_giay >= 10) VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                                }

                                if (VanCo.NguoiChoi2_phut >= 10)
                                {
                                    if (VanCo.NguoiChoi2_giay >= 10)
                                    {
                                        VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                                    }
                                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                                }
                                if (VanCo.NguoiChoi2_phut < 10)
                                {
                                    if (VanCo.NguoiChoi2_giay >= 10) VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                                }

                                if (VanCo.LuotDi == 0)
                                {
                                    Timer_NguoiChoi1.Enabled = true;
                                    Timer_NguoiChoi2.Enabled = false;
                                }
                                if (VanCo.LuotDi == 1)
                                {
                                    Timer_NguoiChoi2.Enabled = true;
                                    Timer_NguoiChoi1.Enabled = false;
                                }
                            }
                            for (int i = 0; i <= 9; i++)
                            {
                                for (int j = 0; j <= 8; j++)
                                {
                                    this.Controls.Add(BanCo.ViTri[i, j].CanMove);
                                    BanCo.ViTri[i, j].CanMove.MouseClick += new MouseEventHandler(CanMove_MouseClick);
                                }
                            }
                            AddQuanCo();
                        }
                        break;
                    case true:
                        this.LuuVanCo.Visible = true;
                        //Tạm ngưng đếm ngược
                        Timer_NguoiChoi1.Enabled = false;
                        Timer_NguoiChoi2.Enabled = false;
                        KhoaBanCo();
                        open_t = 1;
                        break;
                }
            }
        }

        private void Options_MouseEnter(object sender, EventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                this.Options.Image = Board.Properties.Resources.Options_MouseOver;
            }
        }
        private void Options_MouseLeave(object sender, EventArgs e)
        {
            this.Options.Image = Board.Properties.Resources.Options;
        }
        private void Options_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.Marked == false)
            {
                if (this.LuuVanCo.Visible == false)
                {
                    Sound_Options opt = new Sound_Options();
                    opt.ShowDialog(this);
                }
            }
        }

        private void Exit_MouseEnter(object sender, EventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                this.Exit.Image = Board.Properties.Resources.Exit_MouseOver;
            }
        }
        private void Exit_MouseLeave(object sender, EventArgs e)
        {
            this.Exit.Image = Board.Properties.Resources.Exit;
        }
        private void Exit_MouseClick(object sender, MouseEventArgs e)
        {
            if (VanCo.Marked == false && this.LuuVanCo.Visible == false && VanCo.KetQua.Visible == false && VanCo.ChapCo.Visible == false)
            {
                switch (VanCo.DangChoi)
                {
                    case false:
                        this.Close();
                        break;
                    case true:
                        this.LuuVanCo.Visible = true;
                        //Tạm ngưng đếm ngược
                        Timer_NguoiChoi1.Enabled = false;
                        Timer_NguoiChoi2.Enabled = false;

                        exit_t = 1;
                        KhoaBanCo();
                        break;
                }
            }
        }

        private void Mini_MouseEnter(object sender, EventArgs e)
        {
            this.Mini.Image = Board.Properties.Resources.Mini_MouseOver;
        }
        private void Mini_MouseLeave(object sender, EventArgs e)
        {
            this.Mini.Image = Board.Properties.Resources.Mini;
        }
        private void Mini_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region Phương thức di chuyển quân cờ
        private void ChessBoard_MouseClick(Object sender, MouseEventArgs e)
        {
            QuanCo temp;
            temp = new QuanCo();
            switch (VanCo.Marked)
            {
                case true:
                    VanCo.Marked = false;

                    //Kiểm tra, tham chiếu temp đến quân cờ được Đánh Dấu
                    if (VanCo.DanhDau.Ten == "tuong") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qTuong;
                    if (VanCo.DanhDau.Ten == "sy")
                    {
                        if (VanCo.DanhDau.ThuTu == "0") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qSy[0];
                        if (VanCo.DanhDau.ThuTu == "1") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qSy[1];
                    }
                    if (VanCo.DanhDau.Ten == "tinh")
                    {
                        if (VanCo.DanhDau.ThuTu == "0") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qTinh[0];
                        if (VanCo.DanhDau.ThuTu == "1") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qTinh[1];
                    }
                    if (VanCo.DanhDau.Ten == "xe")
                    {
                        if (VanCo.DanhDau.ThuTu == "0") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qXe[0];
                        if (VanCo.DanhDau.ThuTu == "1") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qXe[1];
                    }
                    if (VanCo.DanhDau.Ten == "phao")
                    {
                        if (VanCo.DanhDau.ThuTu == "0") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qPhao[0];
                        if (VanCo.DanhDau.ThuTu == "1") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qPhao[1];
                    }
                    if (VanCo.DanhDau.Ten == "ma")
                    {
                        if (VanCo.DanhDau.ThuTu == "0") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qMa[0];
                        if (VanCo.DanhDau.ThuTu == "1") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qMa[1];
                    }
                    if (VanCo.DanhDau.Ten == "chot")
                    {
                        if (VanCo.DanhDau.ThuTu == "0") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qChot[0];
                        if (VanCo.DanhDau.ThuTu == "1") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qChot[1];
                        if (VanCo.DanhDau.ThuTu == "2") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qChot[2];
                        if (VanCo.DanhDau.ThuTu == "3") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qChot[3];
                        if (VanCo.DanhDau.ThuTu == "4") temp = VanCo.NguoiChoi[VanCo.DanhDau.Phe].qChot[4];
                    }

                    if (temp.Phe == 0)
                    {
                        if (temp.Ten == "tuong") temp.picQuanCo.Image = Board.Properties.Resources._1tuong;
                        if (temp.Ten == "sy") temp.picQuanCo.Image = Board.Properties.Resources._1sy;
                        if (temp.Ten == "tinh") temp.picQuanCo.Image = Board.Properties.Resources._1tinh;
                        if (temp.Ten == "xe") temp.picQuanCo.Image = Board.Properties.Resources._1xe;
                        if (temp.Ten == "phao") temp.picQuanCo.Image = Board.Properties.Resources._1phao;
                        if (temp.Ten == "ma") temp.picQuanCo.Image = Board.Properties.Resources._1ma;
                        if (temp.Ten == "chot") temp.picQuanCo.Image = Board.Properties.Resources._1chot;
                    }
                    if (temp.Phe == 1)
                    {
                        if (temp.Ten == "tuong") temp.picQuanCo.Image = Board.Properties.Resources._2tuong;
                        if (temp.Ten == "sy") temp.picQuanCo.Image = Board.Properties.Resources._2sy;
                        if (temp.Ten == "tinh") temp.picQuanCo.Image = Board.Properties.Resources._2tinh;
                        if (temp.Ten == "xe") temp.picQuanCo.Image = Board.Properties.Resources._2xe;
                        if (temp.Ten == "phao") temp.picQuanCo.Image = Board.Properties.Resources._2phao;
                        if (temp.Ten == "ma") temp.picQuanCo.Image = Board.Properties.Resources._2ma;
                        if (temp.Ten == "chot") temp.picQuanCo.Image = Board.Properties.Resources._2chot;
                    }
                    
                    BanCo.ResetCanMove();
                    break;
                case false:
                    break;
            }
        }
        private void CanMove_MouseClick(Object sender, MouseEventArgs e)
        {
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if (sender.Equals(BanCo.ViTri[i, j].CanMove)) 
                    {
                        if (VanCo.Marked)
                        {
                            switch (BanCo.ViTri[i, j].Trong)
                            {
                                case true:
                                    if (VanCo.DanhDau.Phe == 0)
                                    {
                                        if (VanCo.DanhDau.Ten == "tuong") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1tuong;
                                        if (VanCo.DanhDau.Ten == "sy") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1sy;
                                        if (VanCo.DanhDau.Ten == "tinh") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1tinh;
                                        if (VanCo.DanhDau.Ten == "xe") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1xe;
                                        if (VanCo.DanhDau.Ten == "phao") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1phao;
                                        if (VanCo.DanhDau.Ten == "ma") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1ma;
                                        if (VanCo.DanhDau.Ten == "chot") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1chot;
                                    }
                                    if (VanCo.DanhDau.Phe == 1)
                                    {
                                        if (VanCo.DanhDau.Ten == "tuong") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2tuong;
                                        if (VanCo.DanhDau.Ten == "sy") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2sy;
                                        if (VanCo.DanhDau.Ten == "tinh") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2tinh;
                                        if (VanCo.DanhDau.Ten == "xe") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2xe;
                                        if (VanCo.DanhDau.Ten == "phao") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2phao;
                                        if (VanCo.DanhDau.Ten == "ma") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2ma;
                                        if (VanCo.DanhDau.Ten == "chot") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2chot;
                                    }
                                    //Bỏ chọn quân cờ
                                    VanCo.Marked = false;

                                    //Ghi thông tin nước đi vào GameLog
                                    VanCo.LuuVaoGameLog(this, VanCo.DanhDau);

                                    //Ô cờ trống tại ví trí ban đầu                
                                    VanCo.OCoTrong(VanCo.DanhDau.Hang, VanCo.DanhDau.Cot);

                                    //Đặt quân cờ đã chọn vào vị trí mới [i,j]
                                    VanCo.DatQuanCo(sender, VanCo.DanhDau, i, j);

                                    //Tiếng động
                                    if (VanCo.AmThanh) VanCo.ClickSound("0");

                                    //Kiểm tra chiếu tướng
                                    VanCo.KiemTraChieuTuong();       

                                    //Thay đổi lượt đi                        
                                    VanCo.DoiLuotDi();                                    

                                    //Kiểm tra chiếu bí
                                    VanCo.KiemTraChieuBi();
                                    if (VanCo.winner != 2)
                                    {
                                        VanCo.ThongBaoChieuTuong.Visible = false;
                                        VanCo.ChieuBi.Visible = true;
                                    }
                                    else VanCo.ChieuBi.Visible = false;                           

                                    BanCo.ResetCanMove();
                                    break;

                                case false:
                                    if (VanCo.DanhDau.Phe == 0)
                                    {
                                        if (VanCo.DanhDau.Ten == "tuong") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1tuong;
                                        if (VanCo.DanhDau.Ten == "sy") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1sy;
                                        if (VanCo.DanhDau.Ten == "tinh") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1tinh;
                                        if (VanCo.DanhDau.Ten == "xe") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1xe;
                                        if (VanCo.DanhDau.Ten == "phao") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1phao;
                                        if (VanCo.DanhDau.Ten == "ma") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1ma;
                                        if (VanCo.DanhDau.Ten == "chot") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._1chot;
                                    }
                                    if (VanCo.DanhDau.Phe == 1)
                                    {
                                        if (VanCo.DanhDau.Ten == "tuong") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2tuong;
                                        if (VanCo.DanhDau.Ten == "sy") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2sy;
                                        if (VanCo.DanhDau.Ten == "tinh") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2tinh;
                                        if (VanCo.DanhDau.Ten == "xe") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2xe;
                                        if (VanCo.DanhDau.Ten == "phao") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2phao;
                                        if (VanCo.DanhDau.Ten == "ma") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2ma;
                                        if (VanCo.DanhDau.Ten == "chot") VanCo.DanhDau.picQuanCo.Image = Board.Properties.Resources._2chot;
                                    }

                                    int phekia = 2;
                                    if (VanCo.DanhDau.Phe == 0) phekia = 1;
                                    else phekia = 0;
                                    QuanCo temp_c;
                                    temp_c = new QuanCo();

                                    if (BanCo.ViTri[i, j].Ten == "tuong") temp_c = VanCo.NguoiChoi[phekia].qTuong;
                                    if (BanCo.ViTri[i, j].Ten == "sy")
                                    {
                                        if (BanCo.ViTri[i, j].ThuTu == "0") temp_c = VanCo.NguoiChoi[phekia].qSy[0];
                                        if (BanCo.ViTri[i, j].ThuTu == "1") temp_c = VanCo.NguoiChoi[phekia].qSy[1];
                                    }
                                    if (BanCo.ViTri[i, j].Ten == "tinh")
                                    {
                                        if (BanCo.ViTri[i, j].ThuTu == "0") temp_c = VanCo.NguoiChoi[phekia].qTinh[0];
                                        if (BanCo.ViTri[i, j].ThuTu == "1") temp_c = VanCo.NguoiChoi[phekia].qTinh[1];
                                    }
                                    if (BanCo.ViTri[i, j].Ten == "xe")
                                    {
                                        if (BanCo.ViTri[i, j].ThuTu == "0") temp_c = VanCo.NguoiChoi[phekia].qXe[0];
                                        if (BanCo.ViTri[i, j].ThuTu == "1") temp_c = VanCo.NguoiChoi[phekia].qXe[1];
                                    }
                                    if (BanCo.ViTri[i, j].Ten == "phao")
                                    {
                                        if (BanCo.ViTri[i, j].ThuTu == "0") temp_c = VanCo.NguoiChoi[phekia].qPhao[0];
                                        if (BanCo.ViTri[i, j].ThuTu == "1") temp_c = VanCo.NguoiChoi[phekia].qPhao[1];
                                    }
                                    if (BanCo.ViTri[i, j].Ten == "ma")
                                    {
                                        if (BanCo.ViTri[i, j].ThuTu == "0") temp_c = VanCo.NguoiChoi[phekia].qMa[0];
                                        if (BanCo.ViTri[i, j].ThuTu == "1") temp_c = VanCo.NguoiChoi[phekia].qMa[1];
                                    }
                                    if (BanCo.ViTri[i, j].Ten == "chot")
                                    {
                                        if (BanCo.ViTri[i, j].ThuTu == "0") temp_c = VanCo.NguoiChoi[phekia].qChot[0];
                                        if (BanCo.ViTri[i, j].ThuTu == "1") temp_c = VanCo.NguoiChoi[phekia].qChot[1];
                                        if (BanCo.ViTri[i, j].ThuTu == "2") temp_c = VanCo.NguoiChoi[phekia].qChot[2];
                                        if (BanCo.ViTri[i, j].ThuTu == "3") temp_c = VanCo.NguoiChoi[phekia].qChot[3];
                                        if (BanCo.ViTri[i, j].ThuTu == "4") temp_c = VanCo.NguoiChoi[phekia].qChot[4];
                                    }

                                    //Bỏ chọn quân cờ
                                    VanCo.Marked = false;

                                    //Ghi thông tin nước đi vào GameLog
                                    VanCo.LuuVaoGameLog(sender, temp_c);

                                    //Ăn quân cờ của đối phương
                                    VanCo.AnQuanCo(temp_c);

                                    //Trả lại ô cờ trống
                                    VanCo.OCoTrong(VanCo.DanhDau.Hang, VanCo.DanhDau.Cot);

                                    //Thiết lập quân cờ đã chọn vào bàn cờ
                                    VanCo.DatQuanCo(sender, VanCo.DanhDau, i, j);
                                    
                                    //Tiếng động
                                    if (VanCo.AmThanh) VanCo.ClickSound(VanCo.DanhDau.Ten);

                                    //Kiểm tra chiếu tướng
                                    VanCo.KiemTraChieuTuong();          

                                    //Thay đổi lượt đi                            
                                    VanCo.DoiLuotDi();
                                    
                                    //Kiểm tra chiếu bí
                                    VanCo.KiemTraChieuBi();
                                    if (VanCo.winner != 2)
                                    {
                                        VanCo.ThongBaoChieuTuong.Visible = false;
                                        VanCo.ChieuBi.Visible = true;
                                    }
                                    else VanCo.ChieuBi.Visible = false;

                                    BanCo.ResetCanMove();
                                    break;
                            }
                        }
                    }
                }

            }
        }
        #endregion

        #region Khóa Bàn Cờ, Add Quân Cờ, TimeTick
        private void Timer_NguoiChoi1_Tick(object sender, EventArgs e)
        {
            VanCo.NguoiChoi1_giay--;
            if (VanCo.NguoiChoi1_giay < 0) 
            {
                VanCo.NguoiChoi1_phut--;
                VanCo.NguoiChoi1_giay=59;
            }
            if (VanCo.NguoiChoi1_phut >= 0)
            {
                if (VanCo.NguoiChoi1_phut >= 10)
                {
                    if (VanCo.NguoiChoi1_giay >= 10)
                    {
                        VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                    }
                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                }
                if (VanCo.NguoiChoi1_phut < 10)
                {
                    if (VanCo.NguoiChoi1_giay >= 10) VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : " + Convert.ToString(VanCo.NguoiChoi1_giay);
                    else VanCo.ThGian_NguoiChoi1.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi1_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi1_giay);
                }
            }
            else
            {
                Timer_NguoiChoi1.Enabled = false;
                KhoaBanCo();
                VanCo.NguoiThang.Text = VanCo.TenNguoiChoi2;
                VanCo.winner = 1;
                VanCo.KetQua.Visible = true;
            }
        }
        private void Timer_NguoiChoi2_Tick(object sender, EventArgs e)
        {
            VanCo.NguoiChoi2_giay--;
            if (VanCo.NguoiChoi2_giay < 0)
            {
                VanCo.NguoiChoi2_phut--;
                VanCo.NguoiChoi2_giay = 59;
            }
            if (VanCo.NguoiChoi2_phut >= 0)
            {
                if (VanCo.NguoiChoi2_phut >= 10)
                {
                    if (VanCo.NguoiChoi2_giay >= 10) VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                }
                if (VanCo.NguoiChoi2_phut < 10)
                {
                    if (VanCo.NguoiChoi2_giay >= 10) VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : " + Convert.ToString(VanCo.NguoiChoi2_giay);
                    else VanCo.ThGian_NguoiChoi2.Text = "Thời gian còn: 0" + Convert.ToString(VanCo.NguoiChoi2_phut) + " : 0" + Convert.ToString(VanCo.NguoiChoi2_giay);
                }
            }
            else
            {
                Timer_NguoiChoi2.Enabled = false;
                KhoaBanCo();
                VanCo.NguoiThang.Text = VanCo.TenNguoiChoi1;
                VanCo.winner = 0;
                VanCo.KetQua.Visible = true;
            }
        }

        private void DemThoiGian()
        {
            if (VanCo.TinhThoiGian)
            {
                if (VanCo.LuotDi == 0)
                {
                    Timer_NguoiChoi1.Enabled = true;
                    Timer_NguoiChoi2.Enabled = false;
                }
                if (VanCo.LuotDi == 1)
                {
                    Timer_NguoiChoi2.Enabled = true;
                    Timer_NguoiChoi1.Enabled = false;
                }
            }
        }

        private void MoBanCo()
        {
            if (VanCo.LuotDi == 0)
            {
                VanCo.LuotDi = 1;
                VanCo.DoiLuotDi();
            }
            else
            {
                VanCo.LuotDi = 0;
                VanCo.DoiLuotDi();
            }
        }

        private void KhoaBanCo()
        {
            VanCo.NguoiChoi[0].qTuong.Khoa = true;
            VanCo.NguoiChoi[0].qSy[0].Khoa = true;
            VanCo.NguoiChoi[0].qSy[1].Khoa = true;
            VanCo.NguoiChoi[0].qTinh[0].Khoa = true;
            VanCo.NguoiChoi[0].qTinh[1].Khoa = true;
            VanCo.NguoiChoi[0].qXe[0].Khoa = true;
            VanCo.NguoiChoi[0].qXe[1].Khoa = true;
            VanCo.NguoiChoi[0].qPhao[0].Khoa = true;
            VanCo.NguoiChoi[0].qPhao[1].Khoa = true;
            VanCo.NguoiChoi[0].qMa[0].Khoa = true;
            VanCo.NguoiChoi[0].qMa[1].Khoa = true;
            VanCo.NguoiChoi[0].qChot[0].Khoa = true;
            VanCo.NguoiChoi[0].qChot[1].Khoa = true;
            VanCo.NguoiChoi[0].qChot[2].Khoa = true;
            VanCo.NguoiChoi[0].qChot[3].Khoa = true;
            VanCo.NguoiChoi[0].qChot[4].Khoa = true;

            VanCo.NguoiChoi[1].qTuong.Khoa = true;
            VanCo.NguoiChoi[1].qSy[0].Khoa = true;
            VanCo.NguoiChoi[1].qSy[1].Khoa = true;
            VanCo.NguoiChoi[1].qTinh[0].Khoa = true;
            VanCo.NguoiChoi[1].qTinh[1].Khoa = true;
            VanCo.NguoiChoi[1].qXe[0].Khoa = true;
            VanCo.NguoiChoi[1].qXe[1].Khoa = true;
            VanCo.NguoiChoi[1].qPhao[0].Khoa = true;
            VanCo.NguoiChoi[1].qPhao[1].Khoa = true;
            VanCo.NguoiChoi[1].qMa[0].Khoa = true;
            VanCo.NguoiChoi[1].qMa[1].Khoa = true;
            VanCo.NguoiChoi[1].qChot[0].Khoa = true;
            VanCo.NguoiChoi[1].qChot[1].Khoa = true;
            VanCo.NguoiChoi[1].qChot[2].Khoa = true;
            VanCo.NguoiChoi[1].qChot[3].Khoa = true;
            VanCo.NguoiChoi[1].qChot[4].Khoa = true;
        }
        private void AddQuanCo()
        {
            this.Controls.Add(VanCo.NguoiChoi[0].qTuong.picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qSy[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qSy[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qTinh[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qTinh[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qXe[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qXe[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qPhao[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qPhao[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qMa[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qMa[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qChot[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qChot[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qChot[2].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qChot[3].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[0].qChot[4].picQuanCo);

            this.Controls.Add(VanCo.NguoiChoi[1].qTuong.picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qSy[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qSy[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qTinh[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qTinh[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qXe[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qXe[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qPhao[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qPhao[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qMa[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qMa[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qChot[0].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qChot[1].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qChot[2].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qChot[3].picQuanCo);
            this.Controls.Add(VanCo.NguoiChoi[1].qChot[4].picQuanCo);

            this.Controls.Add(VanCo.ChapCo);
            this.Controls.Add(VanCo.ChieuBi);
            this.Controls.Add(VanCo.ThongBaoChieuTuong);
            this.Controls.Add(VanCo.KetQua);
            this.Controls.Add(VanCo.P1_Turn);
            this.Controls.Add(VanCo.P2_Turn);
            this.Controls.Add(VanCo.ThGian_NguoiChoi1);
            this.Controls.Add(VanCo.ThGian_NguoiChoi2);
            
            VanCo.ChapXong.MouseEnter+=new EventHandler(ChapXong_MouseEnter);
            VanCo.ChapXong.MouseLeave+=new EventHandler(ChapXong_MouseLeave);
            VanCo.ChapXong.MouseClick+=new MouseEventHandler(ChapXong_MouseClick);
            VanCo.DiLai.MouseEnter += new System.EventHandler(DiLai_MouseEnter);
            VanCo.DiLai.MouseLeave += new System.EventHandler(DiLai_MouseLeave);
            VanCo.DiLai.MouseClick += new MouseEventHandler(DiLai_MouseClick);
            VanCo.ChiuThua.MouseEnter += new System.EventHandler(ChiuThua_MouseEnter);
            VanCo.ChiuThua.MouseLeave += new System.EventHandler(ChiuThua_MouseLeave);
            VanCo.ChiuThua.MouseClick += new MouseEventHandler(ChiuThua_MouseClick);

            VanCo.ChoiLai.MouseEnter+=new EventHandler(ChoiLai_MouseEnter);
            VanCo.ChoiLai.MouseLeave+=new EventHandler(ChoiLai_MouseLeave);
            VanCo.ChoiLai.MouseClick+=new MouseEventHandler(ChoiLai_MouseClick);
            VanCo.Thoat.MouseEnter+=new EventHandler(Thoat_MouseEnter);
            VanCo.Thoat.MouseLeave+=new EventHandler(Thoat_MouseLeave);
            VanCo.Thoat.MouseClick+=new MouseEventHandler(Thoat_MouseClick);
        }        
        #endregion
    }
}