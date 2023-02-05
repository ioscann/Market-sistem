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
    public partial class FrmStats : Form
    {
        public FrmStats()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        private void FrmStats_Load(object sender, EventArgs e)
        {          
            label2.Text = db.TBLPERSONEL.OrderByDescending(x => x.SATISSAYISI).Select(y => y.PERSONELAD).FirstOrDefault().ToString() + " " + db.TBLPERSONEL.OrderByDescending(z => z.SATISSAYISI).Select(a => a.PERSONELSOYAD).FirstOrDefault().ToString();
            label4.Text = db.TBLPERSONEL.Count().ToString();
            label6.Text = db.TBLURUN.Count().ToString();
            label8.Text = db.TBLURUN.OrderByDescending(x => x.SATISFIYAT).Select(y => y.URUNAD).FirstOrDefault().ToString();
            label10.Text = db.TBLSATISLAR.Count().ToString();
            label12.Text = db.TBLSATISLAR.Sum(x => x.TUTAR).ToString();
        }
    }
}
