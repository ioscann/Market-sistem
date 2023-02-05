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
    public partial class FrmNotification : Form
    {
        public FrmNotification()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-2IVHC50\\SQLEXPRESS;Initial Catalog=DBMarketSistem;Integrated Security=True");

        void notificationList()
        {
            dataGridView1.DataSource = (from x in db.TBLDUYURU
                                        select new
                                        {
                                            x.YAYINLAYAN,
                                            x.ICERIK,
                                            x.OKUNDUBILGISI
                                        }).ToList();
        }

        private void FrmNotification_Load(object sender, EventArgs e)
        {
            notificationList();
            dataGridView1.RowHeadersVisible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (TxtNotification.Text != "")
            {
                TBLDUYURU t = new TBLDUYURU();
                int id = db.TBLDUYURU.Where(x => x.ICERIK == TxtNotification.Text).Select(y => y.ID).FirstOrDefault();
                var z = db.TBLDUYURU.Find(id);
                z.OKUNDUBILGISI = true;
                db.SaveChanges();
                FrmMainScreen.notificationCount -= 1;
                notificationList();
            }
            else
            {
                MessageBox.Show("Önce duyuruya tıklayınız !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int chosen = dataGridView1.SelectedCells[0].RowIndex;
            TxtNotification.Text = dataGridView1.Rows[chosen].Cells[1].Value.ToString();
        }
    }
}
