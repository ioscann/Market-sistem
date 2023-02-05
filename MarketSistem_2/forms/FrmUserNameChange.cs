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
    public partial class FrmUserNameChange : Form
    {
        public FrmUserNameChange()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (TxtUserName.Text != "" && TxtUserName.Text != label2.Text)
            {
                TBLPERSONEL t = new TBLPERSONEL();
                int id = db.TBLPERSONEL.Where(x => x.KADI == label2.Text).Select(y => y.PERSONELID).FirstOrDefault();
                var z = db.TBLPERSONEL.Find(id);
                z.KADI = TxtUserName.Text;
                db.SaveChanges();

                MessageBox.Show("Kullanıcı adınız başarıyla değiştirildi. Lütfen yeni kullanıcı adınızla tekrar sisteme giriş yapın .", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Yeni kullanıcı adı kısmı boş bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmUserNameChange_Load(object sender, EventArgs e)
        {
            label2.Text = Form1.userName;
        }
    }
}
