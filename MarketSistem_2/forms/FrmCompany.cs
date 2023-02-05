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
    public partial class FrmCompany : Form
    {
        public FrmCompany()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        void CompanyList()
        {
            dataGridView1.DataSource = (from x in db.TBLFIRMA
                                        select new
                                        {
                                            x.FIRMAID,
                                            x.AD,
                                            x.YETKILI,
                                            x.YETKILITEL,
                                            x.FIRMATEL,
                                            x.IL,
                                            x.ILCE
                                        }).ToList();
        }

        void ComboboxList()
        {
            comboBox1.DataSource = (from x in db.TBLILLER
                                    select new
                                    {
                                        x.ID,
                                        x.SEHIR
                                    }).ToList();

            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "SEHIR";
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
        }

        void clear()
        {
            TxtID.Text = "";
            TxtCompanyName.Text = "";
            TxtAuthorizedName.Text = "";
            TxtAuthorizedNum.Text = "";
            TxtCompanyNum.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        new void Refresh()
        {
            CompanyList();
            ComboboxList();
            clear();
        }

        void Remove()
        {
            if (TxtID.Text != "")
            {
                TBLFIRMA t = new TBLFIRMA();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLFIRMA.Find(id);
                db.TBLFIRMA.Remove(x);
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce silmek istediğiniz firmaya tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmCompany_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int chosen = dataGridView1.SelectedCells[0].RowIndex;
            TxtID.Text = dataGridView1.Rows[chosen].Cells[0].Value.ToString();
            TxtCompanyName.Text = dataGridView1.Rows[chosen].Cells[1].Value.ToString();
            TxtAuthorizedName.Text = dataGridView1.Rows[chosen].Cells[2].Value.ToString();
            TxtAuthorizedNum.Text = dataGridView1.Rows[chosen].Cells[3].Value.ToString();
            TxtCompanyNum.Text = dataGridView1.Rows[chosen].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[chosen].Cells[5].Value.ToString();
            comboBox2.Text = dataGridView1.Rows[chosen].Cells[6].Value.ToString();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (TxtCompanyName.Text != "" && TxtAuthorizedName.Text != "" && TxtAuthorizedNum.Text != "" && TxtCompanyNum.Text != "")
            {
                TBLFIRMA t = new TBLFIRMA();
                t.AD = TxtCompanyName.Text;
                t.YETKILI = TxtAuthorizedName.Text;
                t.YETKILITEL = TxtAuthorizedNum.Text;
                t.FIRMATEL = TxtCompanyNum.Text;
                t.IL = comboBox1.Text;
                t.ILCE = comboBox2.Text;
                db.TBLFIRMA.Add(t);
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Firma eklenirken boş alan bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TxtID.Text != "")
            {
                TBLFIRMA t = new TBLFIRMA();
                int id = int.Parse(TxtID.Text);
                var x = db.TBLFIRMA.Find(id);
                x.AD = TxtCompanyName.Text;
                x.YETKILI = TxtAuthorizedName.Text;
                x.YETKILITEL = TxtAuthorizedNum.Text;
                x.FIRMATEL = TxtCompanyNum.Text;
                x.IL = comboBox1.Text;
                x.ILCE = comboBox2.Text;
                db.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show("Önce güncellemek istediğiniz firmaya tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int chosen = int.Parse(comboBox1.SelectedValue.ToString());

            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "ILCE";

            comboBox2.DataSource = (from x in db.TBLILCELER
                                    select new
                                    {
                                        x.ID,
                                        x.ILCE,
                                        x.SEHIR
                                    }).Where(y => y.SEHIR == chosen).ToList();
        }
    }
}
