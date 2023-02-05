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
    public partial class FrmPasswordChange : Form
    {
        public FrmPasswordChange()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        string oldPass;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                textBox1.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                textBox3.UseSystemPasswordChar = false;
            }
            else
            {
                textBox3.UseSystemPasswordChar = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                if (textBox1.Text == oldPass)
                {
                    if (textBox2.Text != oldPass || textBox3.Text != oldPass)
                    {
                        if (textBox2.Text == textBox3.Text)
                        {
                            TBLPERSONEL t = new TBLPERSONEL();
                            int id = db.TBLPERSONEL.Where(x => x.KADI == Form1.userName).Select(y => y.PERSONELID).FirstOrDefault();
                            var z = db.TBLPERSONEL.Find(id);
                            z.SIFRE = textBox2.Text;
                            db.SaveChanges();

                            MessageBox.Show("Şifreniz başarıyla değiştirildi. Lütfen yeni şifrenizle sisteme tekrar giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Restart();
                        }
                        else
                        {
                            MessageBox.Show("Yeni şifreniz ile yeni şifre tekrarınız uyuşmuyor !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Yeni şifreniz eski şifreniz ile aynı olamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                  
                }
                else
                {
                    MessageBox.Show("Eski şifreniz yanlış !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }               
            }
            else
            {
                MessageBox.Show("Hiçbir alan boş bırakılamaz !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmPasswordChange_Load(object sender, EventArgs e)
        {
            oldPass = db.TBLPERSONEL.Where(x => x.KADI == Form1.userName).Select(y => y.SIFRE).FirstOrDefault();
        }
    }
}
