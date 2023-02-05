using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketSistem_2.forms
{
    public partial class FrmCategory : Form
    {
        public FrmCategory()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        void CategoryList() // kategorileri listeleyen fonksiyon
        {
            dataGridView1.DataSource = (from x in db.TBLKATEGORI
                                        select new
                                        {
                                            x.ID,
                                            x.KATEGORIAD
                                        }).ToList();                                      
        }

        void clear() // textboxları temizleyen fonksiyon
        {
            TxtID.Text = "";
            TxtName.Text = "";
        }

        void Remove() // kategori silme işlemini gerçekleştiren fonksiyon
        {
            if (TxtID.Text != "")
            {
                TBLKATEGORI t = new TBLKATEGORI();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLKATEGORI.Find(id);
                db.TBLKATEGORI.Remove(x);
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce silmek istedğiğniz veriye tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        new void Refresh() // yenile fonksiyonu
        {
            CategoryList();
            clear();
        }

        private void FrmCategory_Load(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // veritabanındaki bilgileri textbox'lara çeken kod parçası
        {
            int chosen = dataGridView1.SelectedCells[0].RowIndex;
            TxtID.Text = dataGridView1.Rows[chosen].Cells[0].Value.ToString();
            TxtName.Text = dataGridView1.Rows[chosen].Cells[1].Value.ToString();
        }

        private void BtnAdd_Click(object sender, EventArgs e) // ekle bütonu
        {
            if (TxtName.Text != "")
            {
                TBLKATEGORI t = new TBLKATEGORI();
                t.KATEGORIAD = TxtName.Text;
                db.TBLKATEGORI.Add(t);
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce kategori adı kısmını doldurmalısınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e) // güncelle butonu 
        {
            if (TxtID.Text != "")
            {
                TBLKATEGORI t = new TBLKATEGORI();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLKATEGORI.Find(id);
                x.KATEGORIAD = TxtName.Text;
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce güncellemek istediğiniz veriye tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove();
        }
    }
}
