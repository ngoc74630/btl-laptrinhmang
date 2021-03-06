using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Board
{
    class VanCo
    {
        public struct NuocDi
        {
            public QuanCo Dau;
            public QuanCo Cuoi;
            //----------------
            public int Hang_Dau;
            public int Cot_Dau;
            //----------------
            public int Hang_Cuoi;
            public int Cot_Cuoi;
        }
        public struct QuanBiAn
        {
            public int Hang;
            public int Cot;
            public PictureBox picQuanCo;
        }
        public static NguoiChoi[] NguoiChoi = new NguoiChoi[2];
        public static string TenNguoiChoi1;
        public static string TenNguoiChoi2;
        public static bool DangChoi = false;
        public static int LuotDi = 0;
        public static int winner = 2;
        public static bool Chap = false;

        //Thông báo chiếu tướng, chiếu bí, kết quả, chọn cờ chấp
        public static PictureBox ThongBaoChieuTuong = new PictureBox();
        //--------------------------------
        public static Panel ChieuBi = new Panel();
        public static PictureBox DiLai = new PictureBox();
        public static PictureBox ChiuThua = new PictureBox();
        //--------------------------------
        public static Panel KetQua = new Panel();
        public static Label NguoiThang = new Label();
        public static PictureBox ChoiLai = new PictureBox();
        public static PictureBox Thoat = new PictureBox();
        //--------------------------------
        public static Panel ChapCo = new Panel();
        public static PictureBox ChapXong = new PictureBox();
        public static PictureBox P1_Turn = new PictureBox();
        public static PictureBox P2_Turn = new PictureBox();
        public static Bitmap BackBuffer = null;

        //Chọn quân cờ 
        public static bool Marked = false; //Kiểm tra đã có quân cờ nào được chọn chưa
        public static QuanCo DanhDau; //Quân cờ DanhDau tham chiếu đến quân cờ được chọn trong 1 nước đi

        //Nhật kí các nước đi (dùng cho Undo)
        public static NuocDi[] GameLog;
        public static int turn = 0;//Lưu tổng số lượt đi của ván cờ        

        //Các quân cờ bị ăn
        public static QuanBiAn[] QuanDoBiAn;
        public static int count_do = 0;
        public static QuanBiAn[] QuanDenBiAn;
        public static int count_den = 0;

        //Bộ đếm thời gian & chấp cờ
        public static bool TinhThoiGian = false;
        public static int NguoiChoi1_phut, NguoiChoi1_giay = 0;
        public static Label ThGian_NguoiChoi1 = new Label();
        public static int NguoiChoi2_phut, NguoiChoi2_giay = 0;
        public static Label ThGian_NguoiChoi2 = new Label();


        //Options
        public static bool AmThanh = true;
        public static bool NhacNen = true;
        public static string Path_NhacNen = @Application.StartupPath + "\\a.mp3";

        //Play mp3 files
        [DllImport("winmm.dll")]
        public static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        static VanCo()
        {
            //Khởi tạo 2 người chơi: NguoiChoi[0] phe 0, NguoiChoi[1] phe 1;
            NguoiChoi[0] = new NguoiChoi(0);
            NguoiChoi[1] = new NguoiChoi(1);
            // 
            // ThongBaoChieuTuong
            // 
            ThongBaoChieuTuong.BackColor = Color.Transparent;
            ThongBaoChieuTuong.Image = Board.Properties.Resources.ChieuTuong;
            ThongBaoChieuTuong.Width = 266;
            ThongBaoChieuTuong.Height = 93;
            ThongBaoChieuTuong.Top = 9;
            ThongBaoChieuTuong.Left = 551;
            ThongBaoChieuTuong.Visible = false;
            // 
            // P1_Turn
            // 
            P1_Turn.BackColor = Color.Transparent;
            P1_Turn.Width = 41;
            P1_Turn.Height = 41;
            P1_Turn.Top = 154;
            P1_Turn.Left = 757;
            P1_Turn.Image = Board.Properties.Resources.Turning;
            // 
            // P2_Turn
            // 
            P2_Turn.BackColor = Color.Transparent;
            P2_Turn.Width = 41;
            P2_Turn.Height = 41;
            P2_Turn.Top = 414;
            P2_Turn.Left = 757;
            P2_Turn.Image = Board.Properties.Resources.NotTurn;
            // 
            // ChieuBi
            // 
            ChieuBi.BackColor = System.Drawing.Color.Transparent;
            ChieuBi.BackgroundImage = Board.Properties.Resources.ChieuBi;
            ChieuBi.Controls.Add(ChiuThua);
            ChieuBi.Controls.Add(DiLai);
            ChieuBi.Top = 9;
            ChieuBi.Left = 551;
            ChieuBi.Size = new System.Drawing.Size(266, 93);
            ChieuBi.Visible = false;
            // 
            // DiLai
            // 
            DiLai.Image = Board.Properties.Resources.DiLai;
            DiLai.Location = new System.Drawing.Point(67, 60);
            DiLai.Size = new System.Drawing.Size(68, 20);
            // 
            // ChiuThua
            // 
            ChiuThua.Image = Board.Properties.Resources.ChiuThua;
            ChiuThua.Location = new System.Drawing.Point(149, 60);
            ChiuThua.Size = new System.Drawing.Size(68, 20);
            // 
            // KetQua
            // 
            KetQua.BackColor = System.Drawing.Color.Transparent;
            KetQua.BackgroundImage = Board.Properties.Resources.Ketquabg;
            KetQua.Controls.Add(NguoiThang);
            KetQua.Controls.Add(ChoiLai);
            KetQua.Controls.Add(Thoat);
            KetQua.Top = 9;
            KetQua.Left = 551;
            KetQua.Size = new System.Drawing.Size(266, 93);
            KetQua.Visible = false;
            // 
            // NguoiThang
            // 
            NguoiThang.Text = "";
            NguoiThang.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NguoiThang.Location = new System.Drawing.Point(130, 20);
            // 
            // ChoiLai
            // 
            ChoiLai.Image = Board.Properties.Resources.ChoiLai;
            ChoiLai.Location = new System.Drawing.Point(75, 65);
            ChoiLai.Size = new System.Drawing.Size(54, 20);
            // 
            // Thoat
            // 
            Thoat.Image = Board.Properties.Resources.Thoat;
            Thoat.Location = new System.Drawing.Point(160, 65);
            Thoat.Size = new System.Drawing.Size(45, 20);
            // 
            // ChapCo
            // 
            ChapCo.BackColor = System.Drawing.Color.Transparent;
            ChapCo.BackgroundImage = Board.Properties.Resources.Chap;
            ChapCo.Controls.Add(ChapXong);
            ChapCo.Top = 9;
            ChapCo.Left = 551;
            ChapCo.Size = new System.Drawing.Size(266, 93);
            ChapCo.Visible = false;
            // 
            // ChapXong
            // 
            ChapXong.Image = Board.Properties.Resources.ChapXong;
            ChapXong.Location = new System.Drawing.Point(105, 55);
            ChapXong.Size = new System.Drawing.Size(81, 20);
            // 
            // ThGian_NguoiChoi1
            // 
            ThGian_NguoiChoi1.BackColor = System.Drawing.Color.Transparent;
            ThGian_NguoiChoi1.Top = 9;
            ThGian_NguoiChoi1.Left = 551;
            ThGian_NguoiChoi1.Size = new System.Drawing.Size(300, 100);
            ThGian_NguoiChoi1.Text = "Thời gian còn: " + Convert.ToString(NguoiChoi1_phut) + " : 00";
            ThGian_NguoiChoi1.Location = new System.Drawing.Point(597, 203);
            ThGian_NguoiChoi1.ForeColor = System.Drawing.Color.Maroon;
            ThGian_NguoiChoi1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // ThGian_NguoiChoi2
            // 
            ThGian_NguoiChoi2.BackColor = System.Drawing.Color.Transparent;
            ThGian_NguoiChoi2.Top = 9;
            ThGian_NguoiChoi2.Left = 551;
            ThGian_NguoiChoi2.Size = new System.Drawing.Size(300, 100);
            ThGian_NguoiChoi2.Text = "Thời gian còn: " + Convert.ToString(NguoiChoi2_phut) + " : 00";
            ThGian_NguoiChoi2.Location = new System.Drawing.Point(597, 462);
            ThGian_NguoiChoi2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }        

        public static void PlayNhacNen(bool nhacnen)
        {

            if (nhacnen)
            {
                mciSendString("open \"" + Path_NhacNen + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
                mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);
            }
            else mciSendString("close MediaFile", null, 0, IntPtr.Zero);

        }

        public static void NewGame()
        {
            switch (DangChoi)
            {
                case true:
                    //Xóa các quân cờ trên bàn cờ
                    NguoiChoi[0].qTuong.picQuanCo.Visible = false;
                    NguoiChoi[0].qSy[0].picQuanCo.Visible = false;
                    NguoiChoi[0].qSy[1].picQuanCo.Visible = false;
                    NguoiChoi[0].qTinh[0].picQuanCo.Visible = false;
                    NguoiChoi[0].qTinh[1].picQuanCo.Visible = false;
                    NguoiChoi[0].qXe[0].picQuanCo.Visible = false;
                    NguoiChoi[0].qXe[1].picQuanCo.Visible = false;
                    NguoiChoi[0].qPhao[0].picQuanCo.Visible = false;
                    NguoiChoi[0].qPhao[1].picQuanCo.Visible = false;
                    NguoiChoi[0].qMa[0].picQuanCo.Visible = false;
                    NguoiChoi[0].qMa[1].picQuanCo.Visible = false;
                    NguoiChoi[0].qChot[0].picQuanCo.Visible = false;
                    NguoiChoi[0].qChot[1].picQuanCo.Visible = false;
                    NguoiChoi[0].qChot[2].picQuanCo.Visible = false;
                    NguoiChoi[0].qChot[3].picQuanCo.Visible = false;
                    NguoiChoi[0].qChot[4].picQuanCo.Visible = false;
                    NguoiChoi[1].qTuong.picQuanCo.Visible = false;
                    NguoiChoi[1].qSy[0].picQuanCo.Visible = false;
                    NguoiChoi[1].qSy[1].picQuanCo.Visible = false;
                    NguoiChoi[1].qTinh[0].picQuanCo.Visible = false;
                    NguoiChoi[1].qTinh[1].picQuanCo.Visible = false;
                    NguoiChoi[1].qXe[0].picQuanCo.Visible = false;
                    NguoiChoi[1].qXe[1].picQuanCo.Visible = false;
                    NguoiChoi[1].qPhao[0].picQuanCo.Visible = false;
                    NguoiChoi[1].qPhao[1].picQuanCo.Visible = false;
                    NguoiChoi[1].qMa[0].picQuanCo.Visible = false;
                    NguoiChoi[1].qMa[1].picQuanCo.Visible = false;
                    NguoiChoi[1].qChot[0].picQuanCo.Visible = false;
                    NguoiChoi[1].qChot[1].picQuanCo.Visible = false;
                    NguoiChoi[1].qChot[2].picQuanCo.Visible = false;
                    NguoiChoi[1].qChot[3].picQuanCo.Visible = false;
                    NguoiChoi[1].qChot[4].picQuanCo.Visible = false;

                    //Xóa 2 người chơi 
                    Array.Resize<Board.NguoiChoi>(ref NguoiChoi, 0);

                    //Khởi tạo 2 người chơi mới
                    Array.Resize<Board.NguoiChoi>(ref NguoiChoi, 2);
                    NguoiChoi[0] = new NguoiChoi(0);
                    NguoiChoi[1] = new NguoiChoi(1);

                    //Khởi tạo bàn cờ mới
                    BanCo.ResetBanCo();
                    winner = 2;
                    LuotDi = 0;
                    turn = 0;
                    count_den = 0;
                    count_do = 0;
                    ThongBaoChieuTuong.Visible = false;
                    ChieuBi.Visible = false;
                    P1_Turn.Image = Board.Properties.Resources.Turning;
                    P2_Turn.Image = Board.Properties.Resources.NotTurn;


                    //Đặt các quân cờ của Người Chơi 1
                    NguoiChoi[0].qTuong.draw();
                    NguoiChoi[0].qSy[0].draw();
                    NguoiChoi[0].qSy[1].draw();
                    NguoiChoi[0].qTinh[0].draw();
                    NguoiChoi[0].qTinh[1].draw();
                    NguoiChoi[0].qXe[0].draw();
                    NguoiChoi[0].qXe[1].draw();
                    NguoiChoi[0].qPhao[0].draw();
                    NguoiChoi[0].qPhao[1].draw();
                    NguoiChoi[0].qMa[0].draw();
                    NguoiChoi[0].qMa[1].draw();
                    NguoiChoi[0].qChot[0].draw();
                    NguoiChoi[0].qChot[1].draw();
                    NguoiChoi[0].qChot[2].draw();
                    NguoiChoi[0].qChot[3].draw();
                    NguoiChoi[0].qChot[4].draw();

                    //Đặt các quân cờ của Người Chơi 2
                    NguoiChoi[1].qTuong.draw();
                    NguoiChoi[1].qSy[0].draw();
                    NguoiChoi[1].qSy[1].draw();
                    NguoiChoi[1].qTinh[0].draw();
                    NguoiChoi[1].qTinh[1].draw();
                    NguoiChoi[1].qXe[0].draw();
                    NguoiChoi[1].qXe[1].draw();
                    NguoiChoi[1].qPhao[0].draw();
                    NguoiChoi[1].qPhao[1].draw();
                    NguoiChoi[1].qMa[0].draw();
                    NguoiChoi[1].qMa[1].draw();
                    NguoiChoi[1].qChot[0].draw();
                    NguoiChoi[1].qChot[1].draw();
                    NguoiChoi[1].qChot[2].draw();
                    NguoiChoi[1].qChot[3].draw();
                    NguoiChoi[1].qChot[4].draw();
                    if(VanCo.AmThanh) ClickSound("ready");
                    break;

                case false:
                    //Tạo bàn cờ trống
                    BanCo.ResetBanCo();
                    VanCo.DangChoi = true;

                    //Đặt các quân cờ của Người Chơi 1
                    NguoiChoi[0].qTuong.draw();
                    NguoiChoi[0].qSy[0].draw();
                    NguoiChoi[0].qSy[1].draw();
                    NguoiChoi[0].qTinh[0].draw();
                    NguoiChoi[0].qTinh[1].draw();
                    NguoiChoi[0].qXe[0].draw();
                    NguoiChoi[0].qXe[1].draw();
                    NguoiChoi[0].qPhao[0].draw();
                    NguoiChoi[0].qPhao[1].draw();
                    NguoiChoi[0].qMa[0].draw();
                    NguoiChoi[0].qMa[1].draw();
                    NguoiChoi[0].qChot[0].draw();
                    NguoiChoi[0].qChot[1].draw();
                    NguoiChoi[0].qChot[2].draw();
                    NguoiChoi[0].qChot[3].draw();
                    NguoiChoi[0].qChot[4].draw();

                    //Đặt các quân cờ của Người Chơi 2
                    NguoiChoi[1].qTuong.draw();
                    NguoiChoi[1].qSy[0].draw();
                    NguoiChoi[1].qSy[1].draw();
                    NguoiChoi[1].qTinh[0].draw();
                    NguoiChoi[1].qTinh[1].draw();
                    NguoiChoi[1].qXe[0].draw();
                    NguoiChoi[1].qXe[1].draw();
                    NguoiChoi[1].qPhao[0].draw();
                    NguoiChoi[1].qPhao[1].draw();
                    NguoiChoi[1].qMa[0].draw();
                    NguoiChoi[1].qMa[1].draw();
                    NguoiChoi[1].qChot[0].draw();
                    NguoiChoi[1].qChot[1].draw();
                    NguoiChoi[1].qChot[2].draw();
                    NguoiChoi[1].qChot[3].draw();
                    NguoiChoi[1].qChot[4].draw();
                    P1_Turn.Image = Board.Properties.Resources.Turning;
                    P2_Turn.Image = Board.Properties.Resources.NotTurn;
                    if(VanCo.AmThanh) ClickSound("ready");
                    break;
            }
        }

        public static void DoiLuotDi()
        {
            if (LuotDi == 0) LuotDi = 1;
            else LuotDi = 0;

            if (VanCo.LuotDi == 0)
            {
                VanCo.NguoiChoi[0].qTuong.Khoa = false;
                VanCo.NguoiChoi[0].qSy[0].Khoa = false;
                VanCo.NguoiChoi[0].qSy[1].Khoa = false;
                VanCo.NguoiChoi[0].qTinh[0].Khoa = false;
                VanCo.NguoiChoi[0].qTinh[1].Khoa = false;
                VanCo.NguoiChoi[0].qXe[0].Khoa = false;
                VanCo.NguoiChoi[0].qXe[1].Khoa = false;
                VanCo.NguoiChoi[0].qPhao[0].Khoa = false;
                VanCo.NguoiChoi[0].qPhao[1].Khoa = false;
                VanCo.NguoiChoi[0].qMa[0].Khoa = false;
                VanCo.NguoiChoi[0].qMa[1].Khoa = false;
                VanCo.NguoiChoi[0].qChot[0].Khoa = false;
                VanCo.NguoiChoi[0].qChot[1].Khoa = false;
                VanCo.NguoiChoi[0].qChot[2].Khoa = false;
                VanCo.NguoiChoi[0].qChot[3].Khoa = false;
                VanCo.NguoiChoi[0].qChot[4].Khoa = false;

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

                VanCo.P1_Turn.Image = Board.Properties.Resources.Turning;
                VanCo.P2_Turn.Image = Board.Properties.Resources.NotTurn;

                if (VanCo.TinhThoiGian == true)
                {
                    ChessBoard.Timer_NguoiChoi1.Enabled = true;
                    ChessBoard.Timer_NguoiChoi2.Enabled = false;
                }
            }
            if (VanCo.LuotDi == 1)
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

                VanCo.NguoiChoi[1].qTuong.Khoa = false;
                VanCo.NguoiChoi[1].qSy[0].Khoa = false;
                VanCo.NguoiChoi[1].qSy[1].Khoa = false;
                VanCo.NguoiChoi[1].qTinh[0].Khoa = false;
                VanCo.NguoiChoi[1].qTinh[1].Khoa = false;
                VanCo.NguoiChoi[1].qXe[0].Khoa = false;
                VanCo.NguoiChoi[1].qXe[1].Khoa = false;
                VanCo.NguoiChoi[1].qPhao[0].Khoa = false;
                VanCo.NguoiChoi[1].qPhao[1].Khoa = false;
                VanCo.NguoiChoi[1].qMa[0].Khoa = false;
                VanCo.NguoiChoi[1].qMa[1].Khoa = false;
                VanCo.NguoiChoi[1].qChot[0].Khoa = false;
                VanCo.NguoiChoi[1].qChot[1].Khoa = false;
                VanCo.NguoiChoi[1].qChot[2].Khoa = false;
                VanCo.NguoiChoi[1].qChot[3].Khoa = false;
                VanCo.NguoiChoi[1].qChot[4].Khoa = false;

                VanCo.P2_Turn.Image = Board.Properties.Resources.Turning;
                VanCo.P1_Turn.Image = Board.Properties.Resources.NotTurn;

                if (VanCo.TinhThoiGian == true)
                {
                    ChessBoard.Timer_NguoiChoi2.Enabled = true;
                    ChessBoard.Timer_NguoiChoi1.Enabled = false;
                }

            }
        }

        public static void Undo()
        {
            int t;
            QuanCo temp_d, temp_c;
            temp_d = new QuanCo();
            temp_c = new QuanCo();

            if (!VanCo.Marked)
                if (VanCo.turn > 0)
                {
                    //Kiểm tra -> tham chiếu temp_d đến quân cờ trên bàn cờ
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "tuong") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qTuong;
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "sy")
                    {
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "0") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qSy[0];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "1") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qSy[1];
                    }
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "tinh")
                    {
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "0") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qTinh[0];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "1") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qTinh[1];
                    }
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "xe")
                    {
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "0") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qXe[0];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "1") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qXe[1];
                    }
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "phao")
                    {
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "0") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qPhao[0];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "1") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qPhao[1];
                    }
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "ma")
                    {
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "0") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qMa[0];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "1") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qMa[1];
                    }
                    if (VanCo.GameLog[VanCo.turn - 1].Dau.Ten == "chot")
                    {
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "0") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qChot[0];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "1") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qChot[1];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "2") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qChot[2];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "3") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qChot[3];
                        if (VanCo.GameLog[VanCo.turn - 1].Dau.ThuTu == "4") temp_d = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Dau.Phe].qChot[4];
                    }

                    //Kiểm tra nước đi có phải là nước đi ăn hay không
                    if (VanCo.GameLog[VanCo.turn - 1].Cuoi == null) t = 0;
                    else t = 1;

                    switch (t)
                    {
                        //Nếu là nước đi không ăn quân cờ của đối phương
                        case 0:
                            //Trả lại ô cờ trống cho vị trí vừa đi đến
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].Trong = true;
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].Phe = 2;
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].Ten = "";
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].ThuTu = "";

                            //Đặt quân cờ vừa đi trở lại vị trí cũ
                            temp_d.Hang = VanCo.GameLog[VanCo.turn - 1].Hang_Dau;
                            temp_d.Cot = VanCo.GameLog[VanCo.turn - 1].Cot_Dau;
                            temp_d.picQuanCo.Top = temp_d.Hang * 53 + 80;
                            temp_d.picQuanCo.Left = temp_d.Cot * 53 + 61;

                            //Thiết lập quân cờ tại vị trí cũ vừa đặt ở trên
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].Trong = false;
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].Phe = temp_d.Phe;
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].ThuTu = temp_d.ThuTu;
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].Ten = temp_d.Ten;

                            //Xóa nước đi cuối cùng khỏi GameLog
                            if (VanCo.turn >= 1) VanCo.turn--;
                            Array.Resize<VanCo.NuocDi>(ref VanCo.GameLog, VanCo.turn);

                            //Kiểm tra chiếu tướng
                            if (VanCo.AmThanh) ClickSound("0");
                            VanCo.KiemTraChieuTuong();

                            if (VanCo.ChieuBi.Visible == true) VanCo.ChieuBi.Visible = false;

                            //Trả lại lượt đi                        
                            VanCo.DoiLuotDi();
                            break;

                        //Nếu là nước đi ăn quân cờ của đối phương
                        case 1:
                            //Kiểm tra -> tham chiếu temp_c đến quân cờ trên bàn cờ
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "tuong") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qTuong;
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "sy")
                            {
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "0") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qSy[0];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "1") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qSy[1];
                            }
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "tinh")
                            {
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "0") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qTinh[0];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "1") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qTinh[1];
                            }
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "xe")
                            {
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "0") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qXe[0];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "1") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qXe[1];
                            }
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "phao")
                            {
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "0") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qPhao[0];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "1") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qPhao[1];
                            }
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "ma")
                            {
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "0") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qMa[0];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "1") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qMa[1];
                            }
                            if (VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten == "chot")
                            {
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "0") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qChot[0];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "1") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qChot[1];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "2") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qChot[2];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "3") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qChot[3];
                                if (VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu == "4") temp_c = VanCo.NguoiChoi[VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe].qChot[4];
                            }

                            //Thiết lập lại quân cờ ở vị trí vừa bị ăn trên Bàn Cờ
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].Trong = false;
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].Phe = VanCo.GameLog[VanCo.turn - 1].Cuoi.Phe;
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].Ten = VanCo.GameLog[VanCo.turn - 1].Cuoi.Ten;
                            BanCo.ViTri[VanCo.GameLog[VanCo.turn - 1].Dau.Hang, VanCo.GameLog[VanCo.turn - 1].Dau.Cot].ThuTu = VanCo.GameLog[VanCo.turn - 1].Cuoi.ThuTu;


                            //Đặt quân cờ bị ăn vào vị trí ở trên
                            temp_c.TrangThai = 1;
                            temp_c.picQuanCo.Top = temp_c.Hang * 53 + 80;
                            temp_c.picQuanCo.Left = temp_c.Cot * 53 + 61;
                            temp_c.picQuanCo.Width = 42;
                            temp_c.picQuanCo.Height = 42;
                            temp_c.picQuanCo.Cursor = Cursors.Hand;
                            if (temp_c.Phe == 0)
                            {
                                count_do--;
                                if (temp_c.Ten == "tuong") temp_c.picQuanCo.Image = Board.Properties.Resources._1tuong;
                                if (temp_c.Ten == "sy") temp_c.picQuanCo.Image = Board.Properties.Resources._1sy;
                                if (temp_c.Ten == "tinh") temp_c.picQuanCo.Image = Board.Properties.Resources._1tinh;
                                if (temp_c.Ten == "xe") temp_c.picQuanCo.Image = Board.Properties.Resources._1xe;
                                if (temp_c.Ten == "phao") temp_c.picQuanCo.Image = Board.Properties.Resources._1phao;
                                if (temp_c.Ten == "ma") temp_c.picQuanCo.Image = Board.Properties.Resources._1ma;
                                if (temp_c.Ten == "chot") temp_c.picQuanCo.Image = Board.Properties.Resources._1chot;
                            }
                            if (temp_c.Phe == 1)
                            {
                                count_den--;
                                if (temp_c.Ten == "tuong") temp_c.picQuanCo.Image = Board.Properties.Resources._2tuong;
                                if (temp_c.Ten == "sy") temp_c.picQuanCo.Image = Board.Properties.Resources._2sy;
                                if (temp_c.Ten == "tinh") temp_c.picQuanCo.Image = Board.Properties.Resources._2tinh;
                                if (temp_c.Ten == "xe") temp_c.picQuanCo.Image = Board.Properties.Resources._2xe;
                                if (temp_c.Ten == "phao") temp_c.picQuanCo.Image = Board.Properties.Resources._2phao;
                                if (temp_c.Ten == "ma") temp_c.picQuanCo.Image = Board.Properties.Resources._2ma;
                                if (temp_c.Ten == "chot") temp_c.picQuanCo.Image = Board.Properties.Resources._2chot;
                            }

                            //Đặt quân cờ vừa đi trở lại vị trí cũ
                            temp_d.Hang = VanCo.GameLog[VanCo.turn - 1].Hang_Dau;
                            temp_d.Cot = VanCo.GameLog[VanCo.turn - 1].Cot_Dau;
                            temp_d.picQuanCo.Top = temp_d.Hang * 53 + 80;
                            temp_d.picQuanCo.Left = temp_d.Cot * 53 + 61;

                            //Thiết lập quân cờ tại vị trí vừa đặt ở trên
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].Trong = false;
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].Phe = temp_d.Phe;
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].ThuTu = temp_d.ThuTu;
                            BanCo.ViTri[temp_d.Hang, temp_d.Cot].Ten = temp_d.Ten;

                            //Xóa nước đi cuối cùng khỏi GameLog
                            if (VanCo.turn >= 1) VanCo.turn--;
                            Array.Resize<VanCo.NuocDi>(ref VanCo.GameLog, VanCo.turn);

                            //Trả lại VanCo.winner=2 nếu nước đi ăn Tướng đối phương
                            if (VanCo.winner != 2) VanCo.winner = 2;

                            //Kiểm tra chiếu tướng
                            if (VanCo.AmThanh) ClickSound("0");
                            VanCo.KiemTraChieuTuong();

                            //Trả lại lượt đi                            
                            VanCo.DoiLuotDi();

                            break;
                    }
                }
        }

        public static void Save()
        {
            FileStream saveFile;
            //FileInfo file = new FileInfo(Application.StartupPath + "\\save\\" + TenNguoiChoi1 + "_vs_" + TenNguoiChoi2 + "_" + Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year) + ".ccb");
            //MessageBox.Show(file.Name);
            //if (file.Exists) saveFile = File.Create(Application.StartupPath + "\\save\\" + TenNguoiChoi1 + "_vs_" + TenNguoiChoi2 + "_1.ccb");
            saveFile = File.Create(Application.StartupPath + "\\save\\" + TenNguoiChoi1 + "_vs_" + TenNguoiChoi2 + "__" + Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year) + "__" + Convert.ToString(DateTime.Now.Hour) + "." + Convert.ToString(DateTime.Now.Minute)+ "." + Convert.ToString(DateTime.Now.Second)+ ".ccb");

            StreamWriter fileWriter = new StreamWriter(saveFile);

            //Ghi lượt đi vào dòng đầu tiên
            fileWriter.WriteLine(Convert.ToString(VanCo.LuotDi));
            //Tính thời gian (0 hoặc 1)
            if (TinhThoiGian == true) fileWriter.WriteLine("1");
            else fileWriter.WriteLine("0");
            //Ghi tên 2 người chơi vào 2 dòng tiếp theo
            fileWriter.WriteLine(VanCo.TenNguoiChoi1);
            fileWriter.WriteLine(VanCo.TenNguoiChoi2);
            //Ghi thời gian còn lại
            fileWriter.WriteLine(Convert.ToString(VanCo.NguoiChoi1_phut));
            fileWriter.WriteLine(Convert.ToString(VanCo.NguoiChoi1_giay));
            fileWriter.WriteLine(Convert.ToString(VanCo.NguoiChoi2_phut));
            fileWriter.WriteLine(Convert.ToString(VanCo.NguoiChoi2_giay));


            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if (BanCo.ViTri[i, j].Trong == false)
                    {
                        fileWriter.WriteLine(Convert.ToString(BanCo.ViTri[i, j].Hang) + Convert.ToString(BanCo.ViTri[i, j].Cot) + Convert.ToString(BanCo.ViTri[i, j].Phe) + BanCo.ViTri[i, j].ThuTu + BanCo.ViTri[i, j].Ten);
                    }
                }
            }
            fileWriter.Close();
            saveFile.Close();
            MessageBox.Show("Ván cờ đã được lưu");
        }

        public static void Open(string path)
        {
            VanCo.NguoiChoi[0].qTuong.picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qSy[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qSy[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qTinh[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qTinh[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qXe[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qXe[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qPhao[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qPhao[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qMa[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qMa[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qChot[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qChot[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qChot[2].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qChot[3].picQuanCo.Visible = false;
            VanCo.NguoiChoi[0].qChot[4].picQuanCo.Visible = false;

            VanCo.NguoiChoi[1].qTuong.picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qSy[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qSy[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qTinh[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qTinh[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qXe[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qXe[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qPhao[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qPhao[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qMa[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qMa[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qChot[0].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qChot[1].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qChot[2].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qChot[3].picQuanCo.Visible = false;
            VanCo.NguoiChoi[1].qChot[4].picQuanCo.Visible = false;
            BanCo.ResetBanCo();

            QuanCo temp;
            temp = new QuanCo();
            int hang, cot, phe, luotdi, nc1_phut, nc1_giay, nc2_phut, nc2_giay;
            string ld = "", ten = "", thutu = "", ten1, ten2, thgian;
            StreamReader fileReader = File.OpenText(path);

            //Đọc vào giá trị luotdi
            luotdi = Convert.ToInt32(ld = fileReader.ReadLine());
            //Tính thời gian
            thgian = fileReader.ReadLine();
            //Đọc vào tên 2 người chơi
            ten1 = fileReader.ReadLine();
            ten2 = fileReader.ReadLine();
            //Đọc vào thời gian còn lại
            nc1_phut = Convert.ToInt32(fileReader.ReadLine());
            nc1_giay = Convert.ToInt32(fileReader.ReadLine());
            nc2_phut = Convert.ToInt32(fileReader.ReadLine());
            nc2_giay = Convert.ToInt32(fileReader.ReadLine());

            while (!fileReader.EndOfStream)
            {
                string Line = fileReader.ReadLine();
                hang = Convert.ToInt32(Convert.ToString(Line[0]));
                cot = Convert.ToInt32(Convert.ToString(Line[1]));
                phe = Convert.ToInt32(Convert.ToString(Line[2]));
                thutu = Convert.ToString(Line[3]);
                for (int i = 4; i < Line.Length; i++)
                {
                    ten += Line[i];
                }

                //Kiểm tra quân cờ để tham chiếu
                if (ten == "tuong") temp = VanCo.NguoiChoi[phe].qTuong;
                if (ten == "sy")
                {
                    if (thutu == "0") temp = VanCo.NguoiChoi[phe].qSy[0];
                    if (thutu == "1") temp = VanCo.NguoiChoi[phe].qSy[1];
                }
                if (ten == "tinh")
                {
                    if (thutu == "0") temp = VanCo.NguoiChoi[phe].qTinh[0];
                    if (thutu == "1") temp = VanCo.NguoiChoi[phe].qTinh[1];
                }
                if (ten == "xe")
                {
                    if (thutu == "0") temp = VanCo.NguoiChoi[phe].qXe[0];
                    if (thutu == "1") temp = VanCo.NguoiChoi[phe].qXe[1];
                }
                if (ten == "phao")
                {
                    if (thutu == "0") temp = VanCo.NguoiChoi[phe].qPhao[0];
                    if (thutu == "1") temp = VanCo.NguoiChoi[phe].qPhao[1];
                }
                if (ten == "ma")
                {
                    if (thutu == "0") temp = VanCo.NguoiChoi[phe].qMa[0];
                    if (thutu == "1") temp = VanCo.NguoiChoi[phe].qMa[1];
                }
                if (ten == "chot")
                {
                    if (thutu == "0") temp = VanCo.NguoiChoi[phe].qChot[0];
                    if (thutu == "1") temp = VanCo.NguoiChoi[phe].qChot[1];
                    if (thutu == "2") temp = VanCo.NguoiChoi[phe].qChot[2];
                    if (thutu == "3") temp = VanCo.NguoiChoi[phe].qChot[3];
                    if (thutu == "4") temp = VanCo.NguoiChoi[phe].qChot[4];
                }

                //Thiết lập quân cờ trên bàn cờ
                BanCo.ViTri[hang, cot].Trong = false;
                BanCo.ViTri[hang, cot].Phe = phe;
                BanCo.ViTri[hang, cot].Ten = ten;
                BanCo.ViTri[hang, cot].ThuTu = thutu;

                //Đặt quân cờ vào vị trí
                if (luotdi != phe) temp.Khoa = true;
                else temp.Khoa = false;
                temp.picQuanCo.Visible = true;
                temp.Hang = hang;
                temp.Cot = cot;
                temp.TrangThai = 1;
                temp.draw();

                //Trả lại giá trị null để tiếp tục lấy thông tin quân cờ khác
                ten = "";
                thutu = "";
            }
            //Thiết lập lượt đi, tên người chơi, thời gian
            VanCo.TenNguoiChoi1 = ten1;
            VanCo.TenNguoiChoi2 = ten2;
            if (thgian == "0") VanCo.TinhThoiGian = false;
            if (thgian == "1") VanCo.TinhThoiGian = true;
            VanCo.NguoiChoi1_phut = nc1_phut;
            VanCo.NguoiChoi1_giay = nc1_giay;
            VanCo.NguoiChoi2_phut = nc2_phut;
            VanCo.NguoiChoi2_giay = nc2_giay;
            VanCo.LuotDi = luotdi;
            turn = 0;
            DangChoi = true;
            winner = 2;
            count_den = 0;
            count_do = 0;
            //Kiểm tra chiếu tướng
            VanCo.KiemTraChieuTuong();

            fileReader.Close();
        }

        public static void OCoTrong(int i, int j)
        {
            BanCo.ViTri[i, j].Trong = true;
            BanCo.ViTri[i, j].Ten = "";
            BanCo.ViTri[i, j].ThuTu = "";
            BanCo.ViTri[i, j].Phe = 2;
        }

        public static void DatQuanCo(Object sender, QuanCo q, int i, int j)
        {
            if (sender.GetType() == typeof(Board.ChessBoard))
            {
                q.Hang = i;
                q.Cot = j;
                q.draw();
            }
            if (sender.GetType() == typeof(System.Windows.Forms.PictureBox))
            {
                BanCo.ViTri[i, j].Trong = false;
                BanCo.ViTri[i, j].Phe = VanCo.DanhDau.Phe;
                BanCo.ViTri[i, j].Ten = VanCo.DanhDau.Ten;
                BanCo.ViTri[i, j].ThuTu = VanCo.DanhDau.ThuTu;
                VanCo.DanhDau.Hang = i;
                VanCo.DanhDau.Cot = j;
                VanCo.DanhDau.picQuanCo.Top = i * 53 + 80;
                VanCo.DanhDau.picQuanCo.Left = j * 53 + 61;
            }

        }

        public static void LuuVaoGameLog(Object sender, QuanCo q)
        {
            if (sender.GetType() == typeof(Board.ChessBoard))
            {
                VanCo.turn++;
                Array.Resize<VanCo.NuocDi>(ref VanCo.GameLog, VanCo.turn);
                VanCo.GameLog[VanCo.turn - 1].Dau = VanCo.DanhDau;
                VanCo.GameLog[VanCo.turn - 1].Hang_Dau = q.Hang;
                VanCo.GameLog[VanCo.turn - 1].Cot_Dau = q.Cot;
            }
            if (sender.GetType() == typeof(System.Windows.Forms.PictureBox))
            {
                VanCo.turn++;
                Array.Resize<VanCo.NuocDi>(ref VanCo.GameLog, VanCo.turn);
                VanCo.GameLog[VanCo.turn - 1].Dau = VanCo.DanhDau;
                VanCo.GameLog[VanCo.turn - 1].Hang_Dau = VanCo.DanhDau.Hang;
                VanCo.GameLog[VanCo.turn - 1].Cot_Dau = VanCo.DanhDau.Cot;
                VanCo.GameLog[VanCo.turn - 1].Cuoi = q;
                VanCo.GameLog[VanCo.turn - 1].Hang_Cuoi = q.Hang;
                VanCo.GameLog[VanCo.turn - 1].Cot_Cuoi = q.Cot;
            }
        }

        public static bool ChieuTuong(QuanCo tuong)
        {
            string kt = "";
            bool chieu = false;
            if (tuong.Phe == 0)
            {
                int xe0 = 0, xe1 = 0, phao0 = 0, phao1 = 0, ma0 = 0, ma1 = 0, chot0 = 0, chot1 = 0, chot2 = 0, chot3 = 0, chot4 = 0;

                if (VanCo.NguoiChoi[1].qXe[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qXe[0].KiemTra(tuong.Hang, tuong.Cot) == 1) xe0 = 1;
                if (VanCo.NguoiChoi[1].qXe[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qXe[1].KiemTra(tuong.Hang, tuong.Cot) == 1) xe1 = 1;
                if (VanCo.NguoiChoi[1].qPhao[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qPhao[0].KiemTra(tuong.Hang, tuong.Cot) == 1) phao0 = 1;
                if (VanCo.NguoiChoi[1].qPhao[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qPhao[1].KiemTra(tuong.Hang, tuong.Cot) == 1) phao1 = 1;
                if (VanCo.NguoiChoi[1].qMa[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qMa[0].KiemTra(tuong.Hang, tuong.Cot) == 1) ma0 = 1;
                if (VanCo.NguoiChoi[1].qMa[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qMa[1].KiemTra(tuong.Hang, tuong.Cot) == 1) ma1 = 1;
                if (VanCo.NguoiChoi[1].qChot[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qChot[0].KiemTra(tuong.Hang, tuong.Cot) == 1) chot0 = 1;
                if (VanCo.NguoiChoi[1].qChot[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qChot[1].KiemTra(tuong.Hang, tuong.Cot) == 1) chot1 = 1;
                if (VanCo.NguoiChoi[1].qChot[2].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qChot[2].KiemTra(tuong.Hang, tuong.Cot) == 1) chot2 = 1;
                if (VanCo.NguoiChoi[1].qChot[3].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qChot[3].KiemTra(tuong.Hang, tuong.Cot) == 1) chot3 = 1;
                if (VanCo.NguoiChoi[1].qChot[4].TrangThai == 1)
                    if (VanCo.NguoiChoi[1].qChot[4].KiemTra(tuong.Hang, tuong.Cot) == 1) chot4 = 1;

                if (xe0 == 1) kt += " xe0";
                if (xe0 == 1) kt += " xe1";
                if (phao0 == 1) kt += " phao0";
                if (phao1 == 1) kt += " phao1";
                if (ma0 == 1) kt += " ma0";
                if (ma1 == 1) kt += " ma1";
                if (chot0 == 1) kt += " chot0";
                if (chot1 == 1) kt += " chot1";
                if (chot2 == 1) kt += " chot2";
                if (chot3 == 1) kt += " chot3";
                if (chot4 == 1) kt += " chot4";

                //if(kt!="") MessageBox.Show(kt);

                if (xe0 != 1 &&
                    xe1 != 1 &&
                    phao0 != 1 &&
                    phao1 != 1 &&
                    ma0 != 1 &&
                    ma1 != 1 &&
                    chot0 != 1 &&
                    chot1 != 1 &&
                    chot2 != 1 &&
                    chot3 != 1 &&
                    chot4 != 1)
                    chieu = false;
                else chieu = true;

            }
            if (tuong.Phe == 1)
            {
                int xe0 = 0, xe1 = 0, phao0 = 0, phao1 = 0, ma0 = 0, ma1 = 0, chot0 = 0, chot1 = 0, chot2 = 0, chot3 = 0, chot4 = 0;

                if (VanCo.NguoiChoi[0].qXe[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qXe[0].KiemTra(tuong.Hang, tuong.Cot) == 1) xe0 = 1;
                if (VanCo.NguoiChoi[0].qXe[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qXe[1].KiemTra(tuong.Hang, tuong.Cot) == 1) xe1 = 1;
                if (VanCo.NguoiChoi[0].qPhao[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qPhao[0].KiemTra(tuong.Hang, tuong.Cot) == 1) phao0 = 1;
                if (VanCo.NguoiChoi[0].qPhao[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qPhao[1].KiemTra(tuong.Hang, tuong.Cot) == 1) phao1 = 1;
                if (VanCo.NguoiChoi[0].qMa[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qMa[0].KiemTra(tuong.Hang, tuong.Cot) == 1) ma0 = 1;
                if (VanCo.NguoiChoi[0].qMa[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qMa[1].KiemTra(tuong.Hang, tuong.Cot) == 1) ma1 = 1;
                if (VanCo.NguoiChoi[0].qChot[0].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qChot[0].KiemTra(tuong.Hang, tuong.Cot) == 1) chot0 = 1;
                if (VanCo.NguoiChoi[0].qChot[1].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qChot[1].KiemTra(tuong.Hang, tuong.Cot) == 1) chot1 = 1;
                if (VanCo.NguoiChoi[0].qChot[2].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qChot[2].KiemTra(tuong.Hang, tuong.Cot) == 1) chot2 = 1;
                if (VanCo.NguoiChoi[0].qChot[3].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qChot[3].KiemTra(tuong.Hang, tuong.Cot) == 1) chot3 = 1;
                if (VanCo.NguoiChoi[0].qChot[4].TrangThai == 1)
                    if (VanCo.NguoiChoi[0].qChot[4].KiemTra(tuong.Hang, tuong.Cot) == 1) chot4 = 1;

                if (xe0 == 1) kt += " xe0";
                if (xe0 == 1) kt += " xe1";
                if (phao0 == 1) kt += " phao0";
                if (phao1 == 1) kt += " phao1";
                if (ma0 == 1) kt += " ma0";
                if (ma1 == 1) kt += " ma1";
                if (chot0 == 1) kt += " chot0";
                if (chot1 == 1) kt += " chot1";
                if (chot2 == 1) kt += " chot2";
                if (chot3 == 1) kt += " chot3";
                if (chot4 == 1) kt += " chot4";

                //if (kt != "") MessageBox.Show(kt);

                if (xe0 != 1 &&
                    xe1 != 1 &&
                    phao0 != 1 &&
                    phao1 != 1 &&
                    ma0 != 1 &&
                    ma1 != 1 &&
                    chot0 != 1 &&
                    chot1 != 1 &&
                    chot2 != 1 &&
                    chot3 != 1 &&
                    chot4 != 1)
                    chieu = false;
                else chieu = true;

            }
            return chieu;
        }

        public static void KiemTraChieuTuong()
        {
            int t = 0;
            if (VanCo.ChieuTuong(VanCo.NguoiChoi[1].qTuong) == true && VanCo.ChieuTuong(VanCo.NguoiChoi[0].qTuong) == false) t = 1;
            if (VanCo.ChieuTuong(VanCo.NguoiChoi[1].qTuong) == false && VanCo.ChieuTuong(VanCo.NguoiChoi[0].qTuong) == true) t = 0;
            if (VanCo.ChieuTuong(VanCo.NguoiChoi[1].qTuong) == false && VanCo.ChieuTuong(VanCo.NguoiChoi[0].qTuong) == false) t = 2;
            switch (t)
            {
                case 1:
                    VanCo.NguoiChoi[1].qTuong.picQuanCo.Image = Board.Properties.Resources._2tuong_bichieu;
                    VanCo.NguoiChoi[0].qTuong.picQuanCo.Image = Board.Properties.Resources._1tuong;
                    VanCo.ThongBaoChieuTuong.Visible = true;
                    if (VanCo.AmThanh) ClickSound("chieu");
                    break;
                case 0:
                    VanCo.NguoiChoi[0].qTuong.picQuanCo.Image = Board.Properties.Resources._1tuong_bichieu;
                    VanCo.NguoiChoi[1].qTuong.picQuanCo.Image = Board.Properties.Resources._2tuong;
                    VanCo.ThongBaoChieuTuong.Visible = true;
                    if (VanCo.AmThanh) ClickSound("chieu");
                    break;
                case 2:
                    VanCo.NguoiChoi[0].qTuong.picQuanCo.Image = Board.Properties.Resources._1tuong;
                    VanCo.NguoiChoi[1].qTuong.picQuanCo.Image = Board.Properties.Resources._2tuong;
                    VanCo.ThongBaoChieuTuong.Visible = false;
                    break;
            }
        }

        public static void AnQuanCo(QuanCo q)
        {
            int hang = 0;
            int cot = 0;
            q.TrangThai = 0;
            if (q.Phe == 1)
            {
                if (count_den >= 0 && count_den <= 5)
                {
                    hang = 0;
                    cot = count_den;
                }
                if (count_den >= 6 && count_den <= 11)
                {
                    hang = 1;
                    cot = count_den - 6;
                }
                if (count_den >= 12 && count_den <= 15)
                {
                    hang = 2;
                    cot = count_den - 12;
                }
                count_den++;
                Array.Resize<QuanBiAn>(ref QuanDenBiAn, count_den);
                VanCo.QuanDenBiAn[count_den - 1].Hang = hang;
                VanCo.QuanDenBiAn[count_den - 1].Cot = cot;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo = q.picQuanCo;
                if (q.Ten == "tuong") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2tuong_bian;
                if (q.Ten == "sy") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2sy_bian;
                if (q.Ten == "tinh") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2tinh_bian;
                if (q.Ten == "xe") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2xe_bian;
                if (q.Ten == "phao") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2phao_bian;
                if (q.Ten == "ma") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2ma_bian;
                if (q.Ten == "chot") VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Image = Board.Properties.Resources._2chot_bian;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Width = 31;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Height = 31;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo.BackColor = Color.Transparent;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Cursor = Cursors.Arrow;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Top = VanCo.QuanDenBiAn[count_den - 1].Hang * 33 + 244;
                VanCo.QuanDenBiAn[count_den - 1].picQuanCo.Left = VanCo.QuanDenBiAn[count_den - 1].Cot * 33 + 596;
            }
            if (q.Phe == 0)
            {
                if (count_do >= 0 && count_do <= 5)
                {
                    hang = 0;
                    cot = count_do;
                }
                if (count_do >= 6 && count_do <= 11)
                {
                    hang = 1;
                    cot = count_do - 6;
                }
                if (count_do >= 12 && count_do <= 15)
                {
                    hang = 2;
                    cot = count_do - 12;
                }
                count_do++;
                Array.Resize<QuanBiAn>(ref QuanDoBiAn, count_do);
                VanCo.QuanDoBiAn[count_do - 1].Hang = hang;
                VanCo.QuanDoBiAn[count_do - 1].Cot = cot;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo = q.picQuanCo;
                if (q.Ten == "tuong") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1tuong_bian;
                if (q.Ten == "sy") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1sy_bian;
                if (q.Ten == "tinh") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1tinh_bian;
                if (q.Ten == "xe") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1xe_bian;
                if (q.Ten == "phao") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1phao_bian;
                if (q.Ten == "ma") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1ma_bian;
                if (q.Ten == "chot") VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Image = Board.Properties.Resources._1chot_bian;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Width = 31;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Height = 31;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Cursor = Cursors.Arrow;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo.BackColor = Color.Transparent;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Top = VanCo.QuanDoBiAn[count_do - 1].Hang * 33 + 503;
                VanCo.QuanDoBiAn[count_do - 1].picQuanCo.Left = VanCo.QuanDoBiAn[count_do - 1].Cot * 33 + 596;
            }
        }

        public static void KiemTraChieuBi()
        {
            bool cuu = false;
            int tuong = 0, sy0 = 0, sy1 = 0, tinh0 = 0, tinh1 = 0, xe0 = 0, xe1 = 0, phao0 = 0, phao1 = 0, ma0 = 0, ma1 = 0, chot0 = 0, chot1 = 0, chot2 = 0, chot3 = 0, chot4 = 0;
            if (VanCo.LuotDi == 0)
            {
                if (ChieuTuong(NguoiChoi[0].qTuong) == true)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (NguoiChoi[0].qTuong.TrangThai == 1)
                                if (NguoiChoi[0].qTuong.KiemTra(i, j) == 1 && NguoiChoi[0].qTuong.TuongAnToan(i, j) == 1) tuong = 1;
                            if (NguoiChoi[0].qSy[0].TrangThai == 1)
                                if (NguoiChoi[0].qSy[0].KiemTra(i, j) == 1 && NguoiChoi[0].qSy[0].TuongAnToan(i, j) == 1) sy0 = 1;
                            if (NguoiChoi[0].qSy[1].TrangThai == 1)
                                if (NguoiChoi[0].qSy[1].KiemTra(i, j) == 1 && NguoiChoi[0].qSy[1].TuongAnToan(i, j) == 1) sy1 = 1;
                            if (NguoiChoi[0].qTinh[0].TrangThai == 1)
                                if (NguoiChoi[0].qTinh[0].KiemTra(i, j) == 1 && NguoiChoi[0].qTinh[0].TuongAnToan(i, j) == 1) tinh0 = 1;
                            if (NguoiChoi[0].qTinh[1].TrangThai == 1)
                                if (NguoiChoi[0].qTinh[1].KiemTra(i, j) == 1 && NguoiChoi[0].qTinh[1].TuongAnToan(i, j) == 1) tinh1 = 1;
                            if (NguoiChoi[0].qXe[0].TrangThai == 1)
                                if (NguoiChoi[0].qXe[0].KiemTra(i, j) == 1 && NguoiChoi[0].qXe[0].TuongAnToan(i, j) == 1) xe0 = 1;
                            if (NguoiChoi[0].qXe[1].TrangThai == 1)
                                if (NguoiChoi[0].qXe[1].KiemTra(i, j) == 1 && NguoiChoi[0].qXe[1].TuongAnToan(i, j) == 1) xe1 = 1;
                            if (NguoiChoi[0].qPhao[0].TrangThai == 1)
                                if (NguoiChoi[0].qPhao[0].KiemTra(i, j) == 1 && NguoiChoi[0].qPhao[0].TuongAnToan(i, j) == 1) phao0 = 1;
                            if (NguoiChoi[0].qPhao[1].TrangThai == 1)
                                if (NguoiChoi[0].qPhao[1].KiemTra(i, j) == 1 && NguoiChoi[0].qPhao[1].TuongAnToan(i, j) == 1) phao1 = 1;
                            if (NguoiChoi[0].qMa[0].TrangThai == 1)
                                if (NguoiChoi[0].qMa[0].KiemTra(i, j) == 1 && NguoiChoi[0].qMa[0].TuongAnToan(i, j) == 1) ma0 = 1;
                            if (NguoiChoi[0].qMa[1].TrangThai == 1)
                                if (NguoiChoi[0].qMa[1].KiemTra(i, j) == 1 && NguoiChoi[0].qMa[1].TuongAnToan(i, j) == 1) ma1 = 1;
                            if (NguoiChoi[0].qChot[0].TrangThai == 1)
                                if (NguoiChoi[0].qChot[0].KiemTra(i, j) == 1 && NguoiChoi[0].qChot[0].TuongAnToan(i, j) == 1) chot0 = 1;
                            if (NguoiChoi[0].qChot[1].TrangThai == 1)
                                if (NguoiChoi[0].qChot[1].KiemTra(i, j) == 1 && NguoiChoi[0].qChot[1].TuongAnToan(i, j) == 1) chot1 = 1;
                            if (NguoiChoi[0].qChot[2].TrangThai == 1)
                                if (NguoiChoi[0].qChot[2].KiemTra(i, j) == 1 && NguoiChoi[0].qChot[2].TuongAnToan(i, j) == 1) chot2 = 1;
                            if (NguoiChoi[0].qChot[3].TrangThai == 1)
                                if (NguoiChoi[0].qChot[3].KiemTra(i, j) == 1 && NguoiChoi[0].qChot[3].TuongAnToan(i, j) == 1) chot3 = 1;
                            if (NguoiChoi[0].qChot[4].TrangThai == 1)
                                if (NguoiChoi[0].qChot[4].KiemTra(i, j) == 1 && NguoiChoi[0].qChot[4].TuongAnToan(i, j) == 1) chot4 = 1;

                            if ((tuong == 1) ||
                                (sy0 == 1) ||
                                (sy1 == 1) ||
                                (tinh0 == 1) ||
                                (tinh1 == 1) ||
                                (xe0 == 1) ||
                                (xe1 == 1) ||
                                (phao0 == 1) ||
                                (phao1 == 1) ||
                                (ma0 == 1) ||
                                (ma1 == 1) ||
                                (chot0 == 1) ||
                                (chot1 == 1) ||
                                (chot2 == 1) ||
                                (chot3 == 1) ||
                                (chot4 == 1))
                            {
                                cuu = true;
                                break;
                            }
                        }
                    }
                }
                else cuu = true;
                if (!cuu) winner = 1;
            }
            if (VanCo.LuotDi == 1)
            {
                if (ChieuTuong(NguoiChoi[1].qTuong) == true)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (NguoiChoi[1].qTuong.TrangThai == 1)
                                if (NguoiChoi[1].qTuong.KiemTra(i, j) == 1 && NguoiChoi[1].qTuong.TuongAnToan(i, j) == 1) tuong = 1;
                            if (NguoiChoi[1].qSy[0].TrangThai == 1)
                                if (NguoiChoi[1].qSy[0].KiemTra(i, j) == 1 && NguoiChoi[1].qSy[0].TuongAnToan(i, j) == 1) sy0 = 1;
                            if (NguoiChoi[1].qSy[1].TrangThai == 1)
                                if (NguoiChoi[1].qSy[1].KiemTra(i, j) == 1 && NguoiChoi[1].qSy[1].TuongAnToan(i, j) == 1) sy1 = 1;
                            if (NguoiChoi[1].qTinh[0].TrangThai == 1)
                                if (NguoiChoi[1].qTinh[0].KiemTra(i, j) == 1 && NguoiChoi[1].qTinh[0].TuongAnToan(i, j) == 1) tinh0 = 1;
                            if (NguoiChoi[1].qTinh[1].TrangThai == 1)
                                if (NguoiChoi[1].qTinh[1].KiemTra(i, j) == 1 && NguoiChoi[1].qTinh[1].TuongAnToan(i, j) == 1) tinh1 = 1;
                            if (NguoiChoi[1].qXe[0].TrangThai == 1)
                                if (NguoiChoi[1].qXe[0].KiemTra(i, j) == 1 && NguoiChoi[1].qXe[0].TuongAnToan(i, j) == 1) xe0 = 1;
                            if (NguoiChoi[1].qXe[1].TrangThai == 1)
                                if (NguoiChoi[1].qXe[1].KiemTra(i, j) == 1 && NguoiChoi[1].qXe[1].TuongAnToan(i, j) == 1) xe1 = 1;
                            if (NguoiChoi[1].qPhao[0].TrangThai == 1)
                                if (NguoiChoi[1].qPhao[0].KiemTra(i, j) == 1 && NguoiChoi[1].qPhao[0].TuongAnToan(i, j) == 1) phao0 = 1;
                            if (NguoiChoi[1].qPhao[1].TrangThai == 1)
                                if (NguoiChoi[1].qPhao[1].KiemTra(i, j) == 1 && NguoiChoi[1].qPhao[1].TuongAnToan(i, j) == 1) phao1 = 1;
                            if (NguoiChoi[1].qMa[0].TrangThai == 1)
                                if (NguoiChoi[1].qMa[0].KiemTra(i, j) == 1 && NguoiChoi[1].qMa[0].TuongAnToan(i, j) == 1) ma0 = 1;
                            if (NguoiChoi[1].qMa[1].TrangThai == 1)
                                if (NguoiChoi[1].qMa[1].KiemTra(i, j) == 1 && NguoiChoi[1].qMa[1].TuongAnToan(i, j) == 1) ma1 = 1;
                            if (NguoiChoi[1].qChot[0].TrangThai == 1)
                                if (NguoiChoi[1].qChot[0].KiemTra(i, j) == 1 && NguoiChoi[1].qChot[0].TuongAnToan(i, j) == 1) chot0 = 1;
                            if (NguoiChoi[1].qChot[1].TrangThai == 1)
                                if (NguoiChoi[1].qChot[1].KiemTra(i, j) == 1 && NguoiChoi[1].qChot[1].TuongAnToan(i, j) == 1) chot1 = 1;
                            if (NguoiChoi[1].qChot[2].TrangThai == 1)
                                if (NguoiChoi[1].qChot[2].KiemTra(i, j) == 1 && NguoiChoi[1].qChot[2].TuongAnToan(i, j) == 1) chot2 = 1;
                            if (NguoiChoi[1].qChot[3].TrangThai == 1)
                                if (NguoiChoi[1].qChot[3].KiemTra(i, j) == 1 && NguoiChoi[1].qChot[3].TuongAnToan(i, j) == 1) chot3 = 1;
                            if (NguoiChoi[1].qChot[4].TrangThai == 1)
                                if (NguoiChoi[1].qChot[4].KiemTra(i, j) == 1 && NguoiChoi[1].qChot[4].TuongAnToan(i, j) == 1) chot4 = 1;

                            if ((tuong == 1) ||
                                (sy0 == 1) ||
                                (sy1 == 1) ||
                                (tinh0 == 1) ||
                                (tinh1 == 1) ||
                                (xe0 == 1) ||
                                (xe1 == 1) ||
                                (phao0 == 1) ||
                                (phao1 == 1) ||
                                (ma0 == 1) ||
                                (ma1 == 1) ||
                                (chot0 == 1) ||
                                (chot1 == 1) ||
                                (chot2 == 1) ||
                                (chot3 == 1) ||
                                (chot4 == 1))
                            {
                                cuu = true;
                                break;
                            }
                        }
                    }
                }
                else cuu = true;
                if (!cuu) winner = 0;
            }
        }

        public static void ClickSound(string s)
        {
            if (s == "chieu")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.Chieu);
                sound.Play();
            }
            if (s == "ready")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.Ready);
                sound.Play();
            }
            if (s == "0")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.Mark);
                sound.Play();
            }
            if (s == "tuong")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.TuongAn);
                sound.Play();
            }
            if (s == "sy")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.SyAn);
                sound.Play();
            }
            if (s == "tinh")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.TinhAn);
                sound.Play();
            }
            if (s == "xe")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.XeAn);
                sound.Play();
            }
            if (s == "phao")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.PhaoAn);
                sound.Play();
            }
            if (s == "ma")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.MaAn);
                sound.Play();
            }
            if (s == "chot")
            {
                SoundPlayer sound = new SoundPlayer(Board.Properties.Resources.ChotAn);
                sound.Play();
            }
        }
    }
}
