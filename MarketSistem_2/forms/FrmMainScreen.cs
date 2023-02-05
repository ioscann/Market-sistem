using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Entity;

namespace MarketSistem_2.forms
{
    public partial class FrmMainScreen : Form
    {
        public FrmMainScreen()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-2IVHC50\\SQLEXPRESS;Initial Catalog=DBMarketSistem;Integrated Security=True");

        public static decimal LastPrice; // fiş ekranına toplam tutarı gönderen değişken
        string MainScreen; // bütün girilen rakam ve operatörlerin tutulduğu string
        string Display; // ekranda görünen rakamların tutulduğuı değişken
        int DotCount = 0; // nokta sayacı
        int OperatorCount = 0; // + - x / işaretlerinin sayacı
        int EqualCount = 0; // = butonuna basılma sayacı
        decimal amount_sell; // toplam tutar
        int stock; // veritabanından ürünün stok bilgisinin çekildiği değişken
        int ID; // veritabanından ürünün ID bilgisinin çekildiği değişken
        int mainStock; // veritabanından ürünün stoğu arttırılıp - azaltıldıktan sonraki yeni stok değeri
        int personelID; // veritabanından personel ID sinin çekildiği değişken
        int selledID; // veritabanında satılan ürün ID sinin çekildiği değişken
        decimal selledPrice; // veritabanında satılan ürünün tutarının çekildiği değişken
        int selledlNumber; // veritabanında satılan ürün adetinin çekildiği değişken
        decimal unit; // veritabanında satılan ürün birim fiuayının çekildiği değişken
        int number; // veritabanından seçilen ürünün adetinin çekildiği değişken
        public static int notificationCount = 0; // okunmamış bildirim sayacı
        string expiredProducts; // tarihi geçen ürünlerin çekildiği değişken
        int expirationCount = 0; // tarihi geçmiş ürün sayacı
        string name;
        decimal price;

        void ProductList() // ürün listesini listeleme fonksiyonu
        {
            dataGridView1.DataSource = (from x in db.TBLURUN
                                        select new
                                        {
                                            x.URUNID,
                                            x.BARKOD,
                                            x.URUNAD,
                                            x.SATISFIYAT,
                                            x.STOK
                                        }).ToList();
        }

        void SellList() // satış listesini listeleme fonksiyonu
        {
            dataGridView2.DataSource = (from x in db.TBLSATIS
                                        select new
                                        {
                                            x.SATISID,
                                            x.URUN,
                                            x.ADET,
                                            x.TUTAR
                                        }).ToList();

        }

        void clear() // textbox verilerini temizleme fonksiyonu
        {
            TxtID.Text = "";
            TxtProduct.Text = "";
            TxtStock.Text = "";
            TxtNumber.Text = "";
            TxtPrice.Text = "";
            TxtBarcode2.Text = "";
            TxtNumber2.Text = "";
            TxtSellID.Text = "";
        }

        void displayAndCountClear() // görüntüdeki verileri ve sayaçları temizleyen fonksiyon
        {
            screen.Text = "";
            MainScreen = "";
            Display = "";
            DotCount = 0;
            OperatorCount = 0;
            EqualCount = 0;
        }

        public void Notification()
        {
            con.Open();

            SqlCommand command = new SqlCommand("select * from TBLDUYURU where OKUNDUBILGISI = 'False'", con);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                notificationCount++;
            }

            

            if (notificationCount > 0)
            {
                bildirimlerToolStripMenuItem.BackColor = Color.Red;
                bildirimlerToolStripMenuItem.ForeColor = Color.Black;
            }
            else
            {
                bildirimlerToolStripMenuItem.BackColor = Color.Transparent;
                bildirimlerToolStripMenuItem.ForeColor = Color.Crimson;
            }

            con.Close();
        }

        void superUser()
        {
            string superUser = db.TBLPERSONEL.Where(x => x.KADI == label5.Text).Select(y => y.SUPERUSER).FirstOrDefault().ToString();

            if (superUser == "False")
            {
                personelİşlemleriToolStripMenuItem.Visible = false;
                istatistiklerToolStripMenuItem.Visible = false;
                raporlarToolStripMenuItem.Visible = false;
                araçlarToolStripMenuItem.Visible = false;
            }
        }

        void expiration()
        {
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            string newDate = year.ToString() + "." + month.ToString() + "." + day.ToString();

            con.Open();

            SqlCommand command = new SqlCommand("select URUNAD from TBLURUN where SKT < '@p1'", con);
            command.Parameters.AddWithValue("@p1", newDate);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                expiredProducts += dr[0].ToString() + ",";
                expirationCount++;
            }

            con.Close();

            if (expirationCount > 0)
            {
                MessageBox.Show("Son kullanma tarihi geçmiş ürün/ürünler var bunlar : " + expiredProducts, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                expiredProducts = "";
                expirationCount = 0;
            }
        }

        new void Refresh()
        {
            SellList();
            ProductList();
            Notification();
            superUser();
        }

        private void FrmMainScreen_Load(object sender, EventArgs e)
        {          
            expiration();
            label5.Text = Form1.userName;
            Refresh();
            dataGridView1.Columns["URUNID"].Visible = false;
            DatagridviewSetting(dataGridView1);
            DatagridviewSetting(dataGridView2);
            timer1.Start(); // güncel saat bilgisine sahip olmak için datetime fonksiyonu timer'a bağlanıyor           
            amount_sell = 0;
            MainScreen = " ";
            Display = "";
        }

        public void DatagridviewSetting(DataGridView datagridview) // datagridview
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;
            datagridview.BorderStyle = BorderStyle.None;
            datagridview.BackgroundColor = Color.FromArgb(50, 50, 50);
            datagridview.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            datagridview.DefaultCellStyle.ForeColor = Color.White;
            datagridview.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            datagridview.DefaultCellStyle.SelectionForeColor = Color.White;
            datagridview.EnableHeadersVisualStyles = false;
            datagridview.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            datagridview.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            datagridview.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            datagridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            date_time.Text = DateTime.Now.ToLongDateString();
            clock.Text = DateTime.Now.ToLongTimeString();
        }

        private void TxtBarcode_TextChanged(object sender, EventArgs e) // girilen barkod veya ada göre ürünleri gösterir
        {
            con.Open();

            SqlCommand command = new SqlCommand("select URUNID,BARKOD,URUNAD,SATISFIYAT,STOK from TBLURUN where URUNAD like '%" + TxtBarcode.Text + "%'", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            con.Close();
        }

        private void screen_TextChanged(object sender, EventArgs e)
        {
            TxtNumber.Text = screen.Text; // satılacak ürün adetinin belirtildiği yer
        }

        // rakamların ekrana yazılmasını sağlayan kodlar

        private void button16_Click(object sender, EventArgs e) // 0 butonu
        {
            screen.Text += "0";
            OperatorCount = 0;
        }

        private void button9_Click(object sender, EventArgs e) // 1 butonu
        {
            screen.Text += "1";
            OperatorCount = 0;
        }

        private void button10_Click(object sender, EventArgs e) // 2 butonu
        {
            screen.Text += "2";
            OperatorCount = 0;
        }

        private void button11_Click(object sender, EventArgs e) // 3 butonu
        {
            screen.Text += "3";
            OperatorCount = 0;
        }

        private void button5_Click(object sender, EventArgs e) // 4 butonu
        {
            screen.Text += "4";
            OperatorCount = 0;
        }

        private void button6_Click(object sender, EventArgs e) // 5 butonu
        {
            screen.Text += "5";
            OperatorCount = 0;
        }

        private void button7_Click(object sender, EventArgs e) // 6 butonu
        {
            screen.Text += "6";
            OperatorCount = 0;
        }

        private void button1_Click(object sender, EventArgs e) // 7 butonu
        {
            screen.Text += "7";
            OperatorCount = 0;
        }

        private void button2_Click(object sender, EventArgs e) // 8 butonu
        {
            screen.Text += "8";
            OperatorCount = 0;
        }

        private void button3_Click(object sender, EventArgs e) // 9 butonu
        {
            screen.Text += "9";
            OperatorCount = 0;
        }

        // işlem operatör tuşlarına tıklanınca gerçekleşecek işlemler

        private void button4_Click(object sender, EventArgs e)  // + tuşu
        {
            if (screen.Text != "")
            {
                if (OperatorCount == 0)
                {
                    Display = screen.Text + "+";
                    screen.Text = "";
                    MainScreen += Display;
                    Display = "";
                    DotCount = 0;
                    OperatorCount += 1;
                    EqualCount = 0;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)  // - tuşu
        {
            if (screen.Text != "")
            {
                if (OperatorCount == 0)
                {
                    Display = screen.Text + "-";
                    screen.Text = "";
                    MainScreen += Display;
                    Display = "";
                    DotCount = 0;
                    OperatorCount += 1;
                    EqualCount = 0;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)  // ÷ tuşu
        {
            if (screen.Text != "")
            {
                if (OperatorCount == 0)
                {
                    Display = screen.Text + "/";
                    screen.Text = "";
                    MainScreen += Display;
                    Display = "";
                    DotCount = 0;
                    OperatorCount += 1;
                    EqualCount = 0;
                }
            }
        }

        private void button18_Click(object sender, EventArgs e) // x tuşu
        {
            if (screen.Text != "")
            {
                if (OperatorCount == 0)
                {
                    Display = screen.Text + "*";
                    screen.Text = "";
                    MainScreen += Display;
                    Display = "";
                    DotCount = 0;
                    OperatorCount += 1;
                    EqualCount = 0;
                }
            }
        }

        private void button15_Click(object sender, EventArgs e) // . tuşu
        {
            if (screen.Text != "")
            {
                if (DotCount == 0)
                {
                    screen.Text += ".";
                    DotCount += 1;
                }
            }
            else
            {
                screen.Text = "0.";
            }
        }

        private void button14_Click(object sender, EventArgs e) // sil tuşu
        {
            displayAndCountClear();
        }

        private void button17_Click(object sender, EventArgs e) //  ekrandan karakter silme işlemi
        {
            string article = screen.Text;

            if (screen.Text != "")
            {
                string LastChar = article.Substring(article.Length - 1, 1);

                if (LastChar == ".") // eğer . silinirse nokta sayacı sıfırlanır ve sistem tekrar nokta kullanımına izin verir
                {
                    article = article.Remove(article.Length - 1, 1);
                    DotCount = 0;
                }
                else
                {
                    article = article.Remove(article.Length - 1, 1); // eğer nokta değilse düz silme işlemi gerçekleşir
                }
            }
            screen.Text = article;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string last = screen.Text; // ='e basıldığında ekranda yazan son karakterler MainScreen değişkenine eklenir
            MainScreen += last;
            string LastChar = MainScreen.Substring(MainScreen.Length - 1, 1); // işlemin son karakteri bulunur

            if (LastChar != "+" && LastChar != "-" && LastChar != "*" && LastChar != "/") // eğer son karakter bir operatör değilse
            {
                if (MainScreen.Length > 0)
                {
                    if (EqualCount == 0)
                    {
                        EqualCount += 1;

                        DataTable dt = new DataTable();
                        var v = dt.Compute(MainScreen, ""); //data table yardımıyla string üzerinden sayısal işlem yapılıp sonuç hesaplanır
                        double conculusion = Convert.ToDouble(v);
                        screen.Text = conculusion.ToString(); // ve ekrana yazdırılır
                        MainScreen = ""; // ana ekran değişkeni ve sayaçlar sıfırlanır
                        DotCount = 0;
                        OperatorCount = 0;
                        EqualCount = 0;
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e) // satış listesine ürün ekleme butonu
        {
            if (TxtNumber.Text == "")
            {
                TxtNumber.Text = "1";
            }

            mainStock = int.Parse(TxtStock.Text);
            int sellnumber = int.Parse(TxtNumber.Text);

            if (mainStock >= sellnumber)
            {
                if (TxtProduct.Text != "" && TxtNumber.Text != "" && TxtPrice.Text != "")
                { 
                    con.Open(); // eğer eklenecek ürün daha önce satış ekranına eklendiyse aşağıdaki işlemleri yapar

                    SqlCommand command = new SqlCommand("select SATISID,ADET,TUTAR from TBLSATIS where URUN = @p1", con);
                    command.Parameters.AddWithValue("@p1", TxtProduct.Text);
                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        selledID = int.Parse(dr[0].ToString());
                        selledlNumber = int.Parse(dr[1].ToString());
                        selledPrice = decimal.Parse(dr[2].ToString());
                        unit = selledPrice / selledlNumber;

                        var z = db.TBLSATIS.Find(selledID);
                        z.BARKOD = TxtBarcode2.Text;
                        z.ADET += int.Parse(TxtNumber.Text);
                        z.TUTAR = z.ADET * unit;
                        db.SaveChanges();
                        SellList();
                    }
                    else
                    {
                        DBMarketSistemEntities db = new DBMarketSistemEntities();
                        TBLSATIS t = new TBLSATIS();
                        t.URUN = TxtProduct.Text;
                        t.BARKOD = TxtBarcode2.Text;
                        t.ADET = int.Parse(TxtNumber.Text);
                        t.TUTAR = decimal.Parse(TxtPrice.Text) * int.Parse(TxtNumber.Text);
                        db.TBLSATIS.Add(t);
                        db.SaveChanges();
                    }

                    con.Close();

                    TBLURUN t2 = new TBLURUN(); // ekleme yapıldıktan sonra stoktan düşme işlemleri
                    int id = db.TBLURUN.Where(x => x.URUNAD == TxtProduct.Text).Select(y => y.URUNID).FirstOrDefault();
                    var q= db.TBLURUN.Find(id);
                    int stock = int.Parse(TxtStock.Text);
                    q.STOK = stock - int.Parse(TxtNumber.Text);
                    db.SaveChanges();
                    ProductList();
                    SellList();
                    clear();

                    con.Open();

                    SqlCommand com = new SqlCommand("select TUTAR from TBLSATIS", con); // satış ekranındaki ürünlerin toplam tutarlarının hesaplanması
                    SqlDataReader drr = com.ExecuteReader();

                    while (drr.Read())
                    {
                        amount_sell += decimal.Parse(drr[0].ToString());
                    }

                    con.Close();

                    LblPrice.Text = amount_sell.ToString(); // toplam tutarın label'a yazdırılması

                    amount_sell = 0;

                    screen.Text = "";                    
                }
            }
            else
            {
                MessageBox.Show("Yeterli stok yok", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSell_Click(object sender, EventArgs e) // satış yap butonu işlemleri
        {
             int count = db.TBLSATIS.Count();

            LastPrice = decimal.Parse(LblPrice.Text);

            if (count > 0)
            {
                con.Open();

                SqlCommand com = new SqlCommand("select URUN,ADET,TUTAR from TBLSATIS", con); // satış ekranındaki veriler çekilir ve değişkenlere atanıp satışlar ve fiş tablosuna eklenir
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    name = dr[0].ToString();
                    number = int.Parse(dr[1].ToString());
                    price = decimal.Parse(dr[2].ToString());
                    string date = DateTime.Now.ToShortDateString();

                    TBLSATISLAR t = new TBLSATISLAR();
                    t.URUN = name;
                    t.ADET = number;
                    t.TUTAR = price;
                    t.TARIH = date;
                    db.TBLSATISLAR.Add(t);
                    db.SaveChanges();

                    DBMarketSistemEntities db2 = new DBMarketSistemEntities(); // iki defa aynı işlem yapıldığında aynı db kullanılınca hata verdiği için farklı db değişkeni tanımladım
                    TBLFIS t2 = new TBLFIS();
                    t2.URUN = name;
                    t2.ADET = number;
                    t2.FIYAT = price / number;
                    t2.TUTAR = price;
                    db2.TBLFIS.Add(t2);
                    db2.SaveChanges();
                }
                con.Close();

                personelID = db.TBLPERSONEL.Where(x => x.KADI == label5.Text).Select(y => y.PERSONELID).FirstOrDefault();

                DBMarketSistemEntities db3 = new DBMarketSistemEntities(); // aynı şekilde farklı bir db daha
                TBLPERSONEL t3 = new TBLPERSONEL(); // personelin yaptığı satış sayısı 1 arttırılır
                var z = db.TBLPERSONEL.Find(personelID);
                z.SATISSAYISI += 1;
                db3.SaveChanges();

                con.Open();

                SqlCommand com2 = new SqlCommand("truncate table TBLSATIS", con); // ve satış tablosu sıfırlanır
                com2.ExecuteNonQuery();
                SellList();

                con.Close();

                LblPrice.Text = 0.ToString();
            }
        }

        private void BtnReturn_Click(object sender, EventArgs e) // iade işlemleri
        {
            int count = int.Parse(db.TBLSATIS.Count().ToString()); // satış ekranındaki ürün sayısı

            if (count > 0)
            {
                int number;
                decimal price;
                int subtract;

                TBLSATIS t = new TBLSATIS();
                int id = int.Parse(TxtSellID.Text);
                var x = db.TBLSATIS.Find(id);
                db.TBLSATIS.Remove(x); // iade edilen ürün satış listesinden silinir
                db.SaveChanges();
                SellList();

                number = int.Parse(TxtNumber2.Text); // ürün adeti bilgisi
                price = decimal.Parse(TxtPrice.Text); // ürün fiyatı bilgisi

                con.Open();

                SqlCommand com = new SqlCommand("select URUNID,STOK from TBLURUN where URUNAD = @p1", con); // veritabanından ürüne ait ID ve STOK sayısı çekiliyor
                com.Parameters.AddWithValue("@p1", TxtProduct.Text);
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    ID = int.Parse(dr[0].ToString());
                    stock = int.Parse(dr[1].ToString());
                }

                con.Close();

                TBLURUN t2 = new TBLURUN(); // iade sonrası ürün stok sayısı güncellleniyor
                var y = db.TBLURUN.Find(ID);
                subtract = int.Parse(TxtNumber2.Text);
                int newStock = stock + subtract;
                y.STOK = newStock;
                db.SaveChanges();
                ProductList();

                decimal sum = decimal.Parse(LblPrice.Text); // iade sonrasi yeni toplam tutar belirleniyor
                decimal part = decimal.Parse(TxtPrice.Text);
                decimal newAmount = sum - part;

                LblPrice.Text = newAmount.ToString(); // yeni toplam tutar label a yazdırılıyor
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // datagridview hücrelerine tıklanınca verilerin textboxlara gönderilmesi
        {
            int chosen = dataGridView1.SelectedCells[0].RowIndex;
            TxtID.Text = dataGridView1.Rows[chosen].Cells[0].Value.ToString();
            TxtBarcode2.Text = dataGridView1.Rows[chosen].Cells[1].Value.ToString();
            TxtProduct.Text = dataGridView1.Rows[chosen].Cells[2].Value.ToString();
            TxtPrice.Text = dataGridView1.Rows[chosen].Cells[3].Value.ToString();
            TxtStock.Text = dataGridView1.Rows[chosen].Cells[4].Value.ToString();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int chosen = dataGridView2.SelectedCells[0].RowIndex;
            TxtSellID.Text = dataGridView2.Rows[chosen].Cells[0].Value.ToString();
            TxtProduct.Text = dataGridView2.Rows[chosen].Cells[1].Value.ToString();
            TxtNumber2.Text = dataGridView2.Rows[chosen].Cells[2].Value.ToString();
            TxtPrice.Text = dataGridView2.Rows[chosen].Cells[3].Value.ToString();
        }

        private void BtnPill_Click(object sender, EventArgs e) // fiş ekranının açılması eğer satılmış ürün yoksa fiş ekranı açılmaz
        {
            int sellCount = int.Parse(db.TBLFIS.Count().ToString());

            if (sellCount > 0)
            {
                forms.FrmPill frm = new FrmPill();
                frm.Show();
            }
            else
            {
                MessageBox.Show("Önce satış yapın !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ürünİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmProduct frm = new FrmProduct();
            frm.Show();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void kategoriİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmCategory frm = new FrmCategory();
            frm.Show();
        }

        private void personelBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmPersonel frm = new FrmPersonel();
            frm.Show();
        }

        private void firmalarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmCompany frm = new FrmCompany();
            frm.Show();
        }

        private void iadeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmReturn frm = new FrmReturn();
            frm.Show();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Gerçekten çıkmak istiyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

            if (DialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ürünSatışPersonelİstatistikleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmStats frm = new FrmStats();
            frm.Show();
        }

        private void duyuruYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmAnnouncement frm = new FrmAnnouncement();
            frm.Show();
        }

        private void bildirimlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmNotification frm = new FrmNotification();
            frm.Show();
        }

        private void raporlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmReports frm = new FrmReports();
            frm.Show();
        }

        private void hesapMakinesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Calc.exe");
        }

        private void kasaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmCase frm = new FrmCase();
            frm.Show();
        }

        private void yardımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Herhangi bir arıza durumunda test@outlook.com adresinden veya 444 33 22 numarsından bize ulaşabilirsiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void kullanıcıAdıDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmUserNameChange frm = new FrmUserNameChange();
            frm.Show();
        }

        private void çıkışYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Oturumu kapatmak istediğinize emin misiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (DialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void şifreDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.FrmPasswordChange frm = new FrmPasswordChange();
            frm.Show();
        }
    }
}
