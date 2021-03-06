namespace Board
{
    class NguoiChoi
    {
        //public string TenNguoiChoi;
        public int Phe;
        public Tuong qTuong = new Tuong();
        public Sy[] qSy = new Sy[2];
        public Tinh[] qTinh = new Tinh[2];
        public Xe[] qXe = new Xe[2];
        public Phao[] qPhao = new Phao[2];
        public Ma[] qMa = new Ma[2];
        public Chot[] qChot = new Chot[5];
               
        public NguoiChoi(int x) //Khoi tao cac quan co cho NguoiChoi          NguoiChoi NguoiChoi1 = new NguoiChoi(1);
        {
           
            if (x == 0)
            {
                qSy[0] = new Sy();
                qSy[1] = new Sy();
                qTinh[0] = new Tinh();
                qTinh[1] = new Tinh();
                qXe[0] = new Xe();
                qXe[1] = new Xe();
                qPhao[0] = new Phao();
                qPhao[1] = new Phao();
                qMa[0] = new Ma();
                qMa[1] = new Ma();
                qChot[0] = new Chot();
                qChot[1] = new Chot();
                qChot[2] = new Chot();
                qChot[3] = new Chot();
                qChot[4] = new Chot();

                Phe = 0;
                qTuong.KhoiTao(0, "tuong", "0", 1, false, 0, 4);
                qSy[0].KhoiTao(0, "sy", "0", 1, false, 0, 3);
                qSy[1].KhoiTao(0, "sy", "1", 1, false, 0, 5);
                qTinh[0].KhoiTao(0, "tinh", "0", 1, false, 0, 2);
                qTinh[1].KhoiTao(0, "tinh", "1", 1, false, 0, 6);
                qXe[0].KhoiTao(0, "xe", "0", 1, false, 0, 0);
                qXe[1].KhoiTao(0, "xe", "1", 1, false, 0, 8);
                qPhao[0].KhoiTao(0, "phao", "0", 1, false, 2, 1);
                qPhao[1].KhoiTao(0, "phao", "1", 1, false, 2, 7);
                qMa[0].KhoiTao(0, "ma", "0", 1, false, 0, 1);
                qMa[1].KhoiTao(0, "ma", "1", 1, false, 0, 7);
                qChot[0].KhoiTao(0, "chot", "0", 1, false, 3, 0);
                qChot[1].KhoiTao(0, "chot", "1", 1, false, 3, 2);
                qChot[2].KhoiTao(0, "chot", "2", 1, false, 3, 4);
                qChot[3].KhoiTao(0, "chot", "3", 1, false, 3, 6);
                qChot[4].KhoiTao(0, "chot", "4", 1, false, 3, 8);
            }
            else
            {
                qSy[0] = new Sy();
                qSy[1] = new Sy();
                qTinh[0] = new Tinh();
                qTinh[1] = new Tinh();
                qXe[0] = new Xe();
                qXe[1] = new Xe();
                qPhao[0] = new Phao();
                qPhao[1] = new Phao();
                qMa[0] = new Ma();
                qMa[1] = new Ma();
                qChot[0] = new Chot();
                qChot[1] = new Chot();
                qChot[2] = new Chot();
                qChot[3] = new Chot();
                qChot[4] = new Chot();

                Phe = 1;
                qTuong.KhoiTao(1, "tuong", "0", 1, true, 9, 4);
                qSy[0].KhoiTao(1, "sy", "0", 1, true, 9, 3);
                qSy[1].KhoiTao(1, "sy", "1", 1, true, 9, 5);
                qTinh[0].KhoiTao(1, "tinh", "0", 1, true, 9, 2);
                qTinh[1].KhoiTao(1, "tinh", "1", 1, true, 9, 6);
                qXe[0].KhoiTao(1, "xe", "0", 1, true, 9, 0);
                qXe[1].KhoiTao(1, "xe", "1", 1, true, 9, 8);
                qPhao[0].KhoiTao(1, "phao", "0", 1, true, 7, 1);
                qPhao[1].KhoiTao(1, "phao", "1", 1, true, 7, 7);
                qMa[0].KhoiTao(1, "ma", "0", 1, true, 9, 1);
                qMa[1].KhoiTao(1, "ma", "1", 1, true, 9, 7);
                qChot[0].KhoiTao(1, "chot", "0", 1, true, 6, 0);
                qChot[1].KhoiTao(1, "chot", "1", 1, true, 6, 2);
                qChot[2].KhoiTao(1, "chot", "2", 1, true, 6, 4);
                qChot[3].KhoiTao(1, "chot", "3", 1, true, 6, 6);
                qChot[4].KhoiTao(1, "chot", "4", 1, true, 6, 8);
            }
        }       
    }
}
