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

namespace MarketSistem_2.forms
{
    public partial class FrmProduct : Form
    {
        public FrmProduct()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-2IVHC50\\SQLEXPRESS;Initial Catalog=DBMarketSistem;Integrated Security=True");

        void ProductList() // veritabanındaki ürünleri listeleyen fonksiyon
        {
            dataGridView1.DataSource = (from x in db.TBLURUN
                                        select new
                                        {
                                            x.URUNID,
                                            x.BARKOD,
                                            x.URUNAD,
                                            x.TBLKATEGORI.KATEGORIAD,
                                            FIRMA = x.TBLFIRMA.AD,
                                            x.ALISFIYAT,
                                            x.SATISFIYAT,
                                            x.STOK,
                                            x.GELISTARIH,
                                            x.SKT
                                        }).ToList();
        }

        void comboBoxList() // combobox'lara veritabanından veri çeken ve Value-Display değerlerini belirleyen fonksiyon
        {
            comboBox1.DataSource = (from x in db.TBLKATEGORI
                                    select new
                                    {
                                        x.ID,
                                        x.KATEGORIAD
                                    }).ToList();

            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "KATEGORIAD";
            comboBox1.Text = "";

            comboBox2.DataSource = (from x in db.TBLFIRMA
                                    select new
                                    {
                                        x.FIRMAID,
                                        x.AD
                                    }).ToList();

            comboBox2.ValueMember = "FIRMAID";
            comboBox2.DisplayMember = "AD";
            comboBox2.Text = "";
        }

        void Remove() // ürün silme fonksiyonu
        {
            if (TxtID.Text != "")
            {
                TBLURUN t = new TBLURUN();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLURUN.Find(id);
                db.TBLURUN.Remove(x);
                db.SaveChanges();
                ProductList();
                TxtID.Text = "";
            }
            else
            {
                MessageBox.Show("Önce silmek istediğiniz ürüne tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        new void Refresh() // sayfa yenileme fonksiyonu
        {
            ProductList();
            comboBoxList();
        }

        void clear() // textbox'ları temizleme fonksiyonu
        {
            TxtID.Text = "";
            TxtBarcode.Text = "";
            TxtProduct.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            TxtUnit.Text = "";
            TxtPrice.Text = "";
            TxtStock.Text = "";
            maskedTextBox1.Text = "";
            maskedTextBox2.Text = "";
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            Refresh();
            dataGridView1.Columns["URUNID"].Visible = false;
        }

        private void BtnAdd_Click(object sender, EventArgs e) // ürün ekle butonu
        {
            try
            {
                TBLURUN t = new TBLURUN();
                t.BARKOD = TxtBarcode.Text;
                t.URUNAD = TxtProduct.Text;
                t.KATEGORI = int.Parse(comboBox1.SelectedValue.ToString());
                t.FIRMA = int.Parse(comboBox2.SelectedValue.ToString());
                t.ALISFIYAT = decimal.Parse(TxtUnit.Text);
                t.SATISFIYAT = decimal.Parse(TxtPrice.Text);
                t.STOK = int.Parse(TxtStock.Text);
                t.GELISTARIH = maskedTextBox1.Text;
                t.SKT = maskedTextBox2.Text;
                db.TBLURUN.Add(t);
                db.SaveChanges();

                if (checkBox1.Checked == true)
                {
                    int id = int.Parse(TxtID.Text);
                    var x = db.TBLURUN.Find(id);
                    x.SKT = label13.Text;
                    db.SaveChanges();
                }

                ProductList();
                clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Ürün eklenirken hiçbir alan boş bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
                  
        private void BtnDelete_Click(object sender, EventArgs e) // ürün sil butonu
        {
            Remove();
        }

        private void saToolStripMenuItem_Click(object sender, EventArgs e) // context menu stript sil butonu
        {
            Remove();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e) // context menu stript yenile butonu
        {
            Refresh();
        }

        private void BtnUpdate_Click(object sender, EventArgs e) // ürün bilgisi güncelle butonu
        {
            if (TxtID.Text != "")
            {
                TBLURUN t = new TBLURUN();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLURUN.Find(id);
                x.BARKOD = TxtBarcode.Text;
                x.URUNAD = TxtProduct.Text;
                x.KATEGORI = int.Parse(comboBox1.SelectedValue.ToString());
                x.FIRMA = int.Parse(comboBox2.SelectedValue.ToString());
                x.ALISFIYAT = decimal.Parse(TxtUnit.Text);
                x.SATISFIYAT = decimal.Parse(TxtPrice.Text);
                x.STOK = int.Parse(TxtStock.Text);
                x.GELISTARIH = maskedTextBox1.Text;

                if (checkBox1.Checked == true)
                {
                    x.SKT = label13.Text;
                }
                else
                {
                    x.SKT = maskedTextBox2.Text;
                }
                
                db.SaveChanges();
                ProductList();
                clear();
            }
            else
            {
                MessageBox.Show("Önce bilgisini güncellemek istediğin ürüne tıklamalısın !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e) // veritabanındaki bilgileri textbox'lara ceken kod parçaşı
        {
            int chosen = dataGridView1.SelectedCells[0].RowIndex;
            TxtID.Text = dataGridView1.Rows[chosen].Cells[0].Value.ToString();
            TxtBarcode.Text = dataGridView1.Rows[chosen].Cells[1].Value.ToString();
            TxtProduct.Text = dataGridView1.Rows[chosen].Cells[2].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[chosen].Cells[3].Value.ToString();
            comboBox2.Text = dataGridView1.Rows[chosen].Cells[4].Value.ToString();
            TxtUnit.Text = dataGridView1.Rows[chosen].Cells[5].Value.ToString();
            TxtPrice.Text = dataGridView1.Rows[chosen].Cells[6].Value.ToString();
            TxtStock.Text = dataGridView1.Rows[chosen].Cells[7].Value.ToString();
            maskedTextBox1.Text = dataGridView1.Rows[chosen].Cells[8].Value.ToString();
            maskedTextBox2.Text = dataGridView1.Rows[chosen].Cells[9].Value.ToString();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e) // ürün - barkod arama 
        {
            con.Open();

            SqlCommand command = new SqlCommand("select * from TBLURUN where URUNAD like '%" + TxtSearch.Text + "%'", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            con.Close();

            if (TxtSearch.Text == "")
            {
                ProductList();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                maskedTextBox2.Enabled = false;
                label13.Text = "YOK";
            }
            else
            {
                maskedTextBox2.Enabled = true;
                label13.Text = "VAR";
            }
        }
    }
}
