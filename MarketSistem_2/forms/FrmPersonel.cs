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
    public partial class FrmPersonel : Form
    {
        public FrmPersonel()
        {
            InitializeComponent();
        }

        bool superUser;

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        void PersonelList()
        {
            dataGridView1.DataSource = (from x in db.TBLPERSONEL
                                        select new
                                        {
                                            x.PERSONELID,
                                            x.PERSONELAD,
                                            x.PERSONELSOYAD,
                                            x.KADI,
                                            x.SUPERUSER,
                                            x.SATISSAYISI
                                        }).ToList();
        }

        void clear()
        {
            TxtID.Text = "";
            TxtName.Text = "";
            TxtSurname.Text = "";
            TxtUserName.Text = "";
            TxtPass.Text = "";
            checkBox1.Checked = false;
        }

        new void Refresh()
        {
            PersonelList();
            clear();
        }

        void Remove()
        {
            if (TxtID.Text != "")
            {
                TBLPERSONEL t = new TBLPERSONEL();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLPERSONEL.Find(id);
                db.TBLPERSONEL.Remove(x);
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce silmek istediğiniz personele tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmPersonel_Load(object sender, EventArgs e)
        {
            TxtSellCount.Text = "0";
            PersonelList();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (TxtName.Text != "" && TxtSurname.Text != "" && TxtUserName.Text != "" && TxtPass.Text != "")
            {
                TBLPERSONEL t = new TBLPERSONEL();
                t.PERSONELAD = TxtName.Text;
                t.PERSONELSOYAD = TxtSurname.Text;
                t.KADI = TxtUserName.Text;
                t.SIFRE = TxtPass.Text;
                t.SUPERUSER = superUser;
                t.SATISSAYISI = int.Parse(TxtSellCount.Text);
                db.TBLPERSONEL.Add(t);
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Personel eklenirken hiçbir alan boş bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TxtID.Text != "")
            {
                TBLPERSONEL t = new TBLPERSONEL();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLPERSONEL.Find(id);
                x.PERSONELAD = TxtName.Text;
                x.PERSONELSOYAD = TxtSurname.Text;
                x.KADI = TxtUserName.Text;
                x.SUPERUSER = superUser;
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce güncellemek istediğiniz personele tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int chosen = dataGridView1.SelectedCells[0].RowIndex;
            TxtID.Text = dataGridView1.Rows[chosen].Cells[0].Value.ToString();
            TxtName.Text = dataGridView1.Rows[chosen].Cells[1].Value.ToString();
            TxtSurname.Text = dataGridView1.Rows[chosen].Cells[2].Value.ToString();
            TxtUserName.Text = dataGridView1.Rows[chosen].Cells[3].Value.ToString();
            label7.Text = dataGridView1.Rows[chosen].Cells[4].Value.ToString();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void label7_TextChanged_1(object sender, EventArgs e)
        {
            if (label7.Text == "True")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                superUser = true;
                checkBox1.Text = "Sisteme tam erişim";
            }
            else
            {
                superUser = false;
                checkBox1.Text = "Sisteme sınırlı erişim";
            }
        }
    }
}
