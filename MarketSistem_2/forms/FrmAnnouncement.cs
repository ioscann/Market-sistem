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
    public partial class FrmAnnouncement : Form
    {
        public FrmAnnouncement()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        private void BtnPublish_Click(object sender, EventArgs e)
        {
            if (TxtContent.Text != "")
            {
                TBLDUYURU t = new TBLDUYURU();
                t.YAYINLAYAN = Form1.userName;
                t.ICERIK = TxtContent.Text;
                t.OKUNDUBILGISI = false;
                db.TBLDUYURU.Add(t);
                db.SaveChanges();
                TxtContent.Text = "";
                
                MessageBox.Show("Duyuru başarıyla yayınlandı !", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);              
            }
            else
            {
                MessageBox.Show("İçerik kısmı boş bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
