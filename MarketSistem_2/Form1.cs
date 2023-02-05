using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketSistem_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        private void BtnExit_Click(object sender, EventArgs e) // x butonu
        {
            Application.Exit();
        }

        public static string userName; 

        private void BtnLogin_Click(object sender, EventArgs e) // login tuşuna basılınca
        {
            userName = TxtUser.Text;

            if (TxtUser.Text != "" && TxtPass.Text != "") // eğer hiçbir alan boş bırakılmazsa 
            {
                var query = from x in db.TBLPERSONEL where x.KADI == TxtUser.Text & x.SIFRE == TxtPass.Text select x; // veritabanından kullanıcı adı ve şifre sorgusu yapılır

                if (query.Any()) // eğer kullanıcı adı ve şifre eşleşirse
                {
                    forms.FrmMainScreen frm = new forms.FrmMainScreen(); // ana ekran açılır
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Hatalı giriş !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); // eşleşmezse uyarı verir
                }
            }
            else // eğer kullanıcı adı veya şifre kısmı boş bırakılırsa uyarı verir
            {
                MessageBox.Show("Hiçbir alan boş bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
