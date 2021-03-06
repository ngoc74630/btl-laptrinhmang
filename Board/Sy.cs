namespace Board
{
    class Sy:QuanCo
    {
        public override int KiemTra(int i, int j)
        {
            bool turn = false;
            if ((i >= 0 && i <= 2 && j >= 3 && j <= 5) || (i >= 7 && i <= 9 && j >= 3 && j <= 5))
                if ((i == Hang + 1 && j == Cot + 1) || (i == Hang + 1 && j == Cot - 1) || (i == Hang - 1 && j == Cot - 1) || (i == Hang - 1 && j == Cot + 1))
                {
                    if (BanCo.ViTri[i, j].Trong == true) turn = true;
                    if (BanCo.ViTri[i, j].Trong == false)
                        if (BanCo.ViTri[i, j].Phe != this.Phe) turn = true;
                }
            
            if (VanCo.NguoiChoi[0].qTuong.Cot == VanCo.NguoiChoi[1].qTuong.Cot && Cot == VanCo.NguoiChoi[1].qTuong.Cot)
            {
                int ct = 0;
                if (j != Cot)
                {
                    VanCo.OCoTrong(Hang, Cot);
                    for (int t = VanCo.NguoiChoi[0].qTuong.Hang + 1; t < VanCo.NguoiChoi[1].qTuong.Hang; t++)
                    {
                        if (BanCo.ViTri[t, Cot].Trong == false) ct++;
                    }                    
                    if (ct == 0) turn = false;
                    BanCo.ViTri[Hang, Cot].Trong = false;
                    BanCo.ViTri[Hang, Cot].Ten = this.Ten;
                    BanCo.ViTri[Hang, Cot].Phe = this.Phe;
                    BanCo.ViTri[Hang, Cot].ThuTu = this.ThuTu;
                }
            }
            //Trả về kết quả
            if (turn) return 1;
            else return 0;
        }

        public override int TuongAnToan(int i, int j)
        {
            bool turn = true;

            //Kiểm tra tướng an toàn
            if (turn == true)
            {
                int ho, co;
                QuanCo temp;
                temp = new QuanCo();

                ho = this.Hang;
                co = this.Cot;

                if (BanCo.ViTri[i, j].Trong == false)
                {
                    if (BanCo.ViTri[i, j].Ten == "tuong") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qTuong;
                    if (BanCo.ViTri[i, j].Ten == "sy")
                    {
                        if (BanCo.ViTri[i, j].ThuTu == "0") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qSy[0];
                        if (BanCo.ViTri[i, j].ThuTu == "1") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qSy[1];
                    }
                    if (BanCo.ViTri[i, j].Ten == "tinh")
                    {
                        if (BanCo.ViTri[i, j].ThuTu == "0") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qTinh[0];
                        if (BanCo.ViTri[i, j].ThuTu == "1") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qTinh[1];
                    }
                    if (BanCo.ViTri[i, j].Ten == "xe")
                    {
                        if (BanCo.ViTri[i, j].ThuTu == "0") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qXe[0];
                        if (BanCo.ViTri[i, j].ThuTu == "1") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qXe[1];
                    }
                    if (BanCo.ViTri[i, j].Ten == "phao")
                    {
                        if (BanCo.ViTri[i, j].ThuTu == "0") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qPhao[0];
                        if (BanCo.ViTri[i, j].ThuTu == "1") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qPhao[1];
                    }
                    if (BanCo.ViTri[i, j].Ten == "ma")
                    {
                        if (BanCo.ViTri[i, j].ThuTu == "0") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qMa[0];
                        if (BanCo.ViTri[i, j].ThuTu == "1") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qMa[1];
                    }
                    if (BanCo.ViTri[i, j].Ten == "chot")
                    {
                        if (BanCo.ViTri[i, j].ThuTu == "0") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qChot[0];
                        if (BanCo.ViTri[i, j].ThuTu == "1") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qChot[1];
                        if (BanCo.ViTri[i, j].ThuTu == "2") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qChot[2];
                        if (BanCo.ViTri[i, j].ThuTu == "3") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qChot[3];
                        if (BanCo.ViTri[i, j].ThuTu == "4") temp = VanCo.NguoiChoi[BanCo.ViTri[i, j].Phe].qChot[4];
                    }
                }
                //Giả sử quân cờ được đi để kiểm tra Tướng mình có bị chiếu ko
                VanCo.OCoTrong(Hang, Cot);
                BanCo.ViTri[i, j].Trong = false;
                BanCo.ViTri[i, j].Phe = Phe;
                BanCo.ViTri[i, j].Ten = Ten;
                BanCo.ViTri[i, j].ThuTu = ThuTu;
                this.Hang = i;
                this.Cot = j;
                if (temp.Phe != 2)
                {
                    temp.TrangThai = 0;
                    temp.picQuanCo.Visible = false;
                }

                //Kiểm tra
                if (VanCo.ChieuTuong(VanCo.NguoiChoi[Phe].qTuong) == true) turn = false;

                //Trả lại những gì đã giả sử ở trên ^^!
                this.Hang = ho;
                this.Cot = co;
                VanCo.OCoTrong(i, j);
                BanCo.ViTri[ho, co].Trong = false;
                BanCo.ViTri[ho, co].Phe = Phe;
                BanCo.ViTri[ho, co].Ten = Ten;
                BanCo.ViTri[ho, co].ThuTu = ThuTu;
                if (temp.Phe != 2)
                {
                    temp.TrangThai = 1;
                    temp.picQuanCo.Visible = true;
                    BanCo.ViTri[i, j].Trong = false;
                    BanCo.ViTri[i, j].Ten = temp.Ten;
                    BanCo.ViTri[i, j].Phe = temp.Phe;
                    BanCo.ViTri[i, j].ThuTu = temp.ThuTu;
                }
            } 
            //Trả về kết quả
            if (turn) return 1;
            else return 0;
        }
    }
}
