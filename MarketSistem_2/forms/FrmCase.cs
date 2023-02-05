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
    public partial class FrmCase : Form
    {
        public FrmCase()
        {
            InitializeComponent();
        }

        DBMarketSistemEntities db = new DBMarketSistemEntities();

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-2IVHC50\\SQLEXPRESS;Initial Catalog=DBMarketSistem;Integrated Security=True");

        decimal spending=0;
        decimal selling=0;

        void ProductList()
        {
            dataGridView1.DataSource = (from x in db.TBLURUN
                                        select new
                                        {
                                            x.URUNAD,
                                            KATEGORI = x.TBLKATEGORI.KATEGORIAD,
                                            x.ALISFIYAT,
                                            ADET = x.STOK
                                        }).ToList();
        }

        void SellList()
        {
            dataGridView2.DataSource = (from x in db.TBLSATISLAR
                                        select new
                                        {
                                            x.URUN,
                                            x.ADET,
                                            x.TUTAR
                                        }).ToList();
        }

        void spendingCalc()
        {
            con.Open();

            SqlCommand command = new SqlCommand("select ALISFIYAT,STOK from TBLURUN", con);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                decimal price = decimal.Parse(dr[0].ToString());
                int stock = int.Parse(dr[1].ToString());

                spending += price * stock;
            }

            label6.Text = spending.ToString();

            con.Close();
        }

        void sellingCalc()
        {
            con.Open();

            SqlCommand command = new SqlCommand("select TUTAR from TBLSATISLAR", con);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            { 
                selling += decimal.Parse(dr[0].ToString());
            }

            label7.Text = selling.ToString();

            con.Close();
        }

        void earningCalc()
        {
            label8.Text = (selling - spending).ToString();
        }

        void allOfThem()
        {
            ProductList();
            SellList();
            spendingCalc();
            sellingCalc();
            earningCalc();
        }

        private void FrmCase_Load(object sender, EventArgs e)
        {
            allOfThem();
        }
    }
}
