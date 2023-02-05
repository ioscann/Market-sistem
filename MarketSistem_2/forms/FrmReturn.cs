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
    public partial class FrmReturn : Form
    {
        public FrmReturn()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        void ComboboxList()
        {
            comboBox1.DataSource = (from x in db.TBLURUN
                                    select new
                                    {
                                        x.URUNAD
                                    }).ToList();

            comboBox1.Text = "";
            comboBox1.DisplayMember = "URUNAD";
        }

        void clear()
        {
            textBox1.Text = "";
            comboBox1.Text = "";
        }

        private void FrmReturn_Load(object sender, EventArgs e)
        {
            ComboboxList();
        }

        private void button1_Click(object sender, EventArgs e) // toplam satış kısmını kodladıktan sonra iade edilen ürün kadar satıştan düşmeyi kodla
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "1";
            }

            if (comboBox1.Text != "")
            {
                TBLURUN t = new TBLURUN();
                int id = db.TBLURUN.Where(x => x.URUNAD == comboBox1.Text).Select(y => y.URUNID).FirstOrDefault();
                var z = db.TBLURUN.Find(id);
                z.STOK += int.Parse(textBox1.Text);
                db.SaveChanges();
                clear();
                MessageBox.Show("İade işlemi başarılı ! Lütfen değişiklikleri görmek için ana ekranı yenilemeyi unutmayın !", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Önce iadesi yapılacak ürünü seçiniz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
