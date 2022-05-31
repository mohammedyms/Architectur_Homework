using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Architectur_Homework
{
    public partial class FrmOrder : Form
    {
        private const string connString = "data source = ABOOD\\MSSQLSERVER01;initial catalog = POS; persist security info=True; Integrated Security = SSPI;";
        private static SqlConnection conn = new SqlConnection(connString);
        private static SqlCommand cmd = new SqlCommand();
        private static SqlDataReader reader = null;
        private static SqlDataAdapter adapter = null;
        public FrmOrder()
        {
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text == "")
            {
                MessageBox.Show("قم بتعبئة سعر الصنف");
            }          
            if (checkBox_Apple.Checked)
            {
                txtCheckOut.Text += checkBox_Apple.Text + "\n";
                lstQty.Items.Add(comboBox_Quantity.Text);
                lstPrice.Items.Add(txtPrice.Text);
            }
            if (checkBox_Panana.Checked)
            {
                txtCheckOut.Text += checkBox_Panana.Text + "\n";
                lstQty.Items.Add(comboBox_Quantity.Text);
                lstPrice.Items.Add(txtPrice.Text);
            }           
            if (checkBox_Orange.Checked)
            {
                txtCheckOut.Text += ((checkBox_Orange.Checked) ? checkBox_Orange.Text : "") + "\n";
                lstQty.Items.Add(comboBox_Quantity.Text);
                lstPrice.Items.Add(txtPrice.Text);
            }
            else
            {
                MessageBox.Show("قم باختيار الصنف");
            }
            FindTotals();


        }
        private void FindTotals()
        {
            lstSuTotal.Items.Clear();
            double total = 0.0;
            double Total, qty, price;

            for(int row=0; row<lstPrice.Items.Count; row++)
            {
                price =double.Parse(lstPrice.Items[row].ToString());
                qty = double.Parse(lstQty.Items[row].ToString());
                Total = price * qty;
                lstSuTotal.Items.Add(Total.ToString());
                total += Total;
            }
            txtTotal.Text = total.ToString();
            txtTax.Text = (total * 0.16).ToString();
            txtGrandTotal.Text = (total + (total * 0.16)).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int index=1;index<=50;index++)
            {
                 comboBox_Quantity.Items.Add(index.ToString());
            }
        }
        private void checkBox_Arabic_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Arabic.Checked) this.RightToLeft = RightToLeft.Yes;
            else this.RightToLeft = RightToLeft.No;
            ChangeControlLayout(checkBox_Arabic.Checked);
        }
        private void ChangeControlLayout(bool islanguage)
        {
            if (islanguage)
            {
                checkBox_Apple.Text = "تفاح";
                checkBox_Arabic.Text = "عربي";
                checkBox_Orange.Text = "برتقال";
                checkBox_Panana.Text = "موز";                
                label1.Text = "الكمية";
                label2.Text = "اسم الصنف";
                label4.Text = "الكمية";
                label3.Text = "السعر"; 
                label6.Text = "الصافي";
                label7.Text = "المجموع";
                label8.Text = "الضريبة";
                label5.Text = "السعر";
                label10.Text = "الصافي";
                groupBox1.Text = "الاصناف";
                btn_Add.Text = "اضافة";
                btnRemove.Text = "حذف";
                btn_Save.Text = "حفظ";

            }
            else
            {
                checkBox_Arabic.Text = "Arabic";
                btn_Add.Text = "Add";
                checkBox_Apple.Text = "Apple";
                checkBox_Orange.Text = "Orange";
                checkBox_Panana.Text = "Panana";
                label1.Text = "Quantity";
                label2.Text = "Item Name";
                label4.Text = "Quantity";
                label3.Text = "Price";
                label6.Text = "Grand Total";
                label7.Text = "Total";
                label8.Text = "Tax";
                label5.Text = "Price";
                label10.Text = "Sub Total";
                groupBox1.Text = "The Items";
                btnRemove.Text = "Delete";
                btn_Save.Text = "Save";
            }
        }

        private void lstQty_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstPrice.SelectedIndex = lstSuTotal.SelectedIndex = lstQty.SelectedIndex;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int row=lstQty.SelectedIndex;
            if (row >= 0)
            {           
                txtCheckOut.Text.Remove(row); 
                lstQty.Items.RemoveAt(row);
                lstPrice.Items.RemoveAt(row);
                lstSuTotal.Items.RemoveAt(row);
                FindTotals();
            }
            else
            {
                MessageBox.Show("قم باختيار الصنف الذي تريد حذفة");
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {                 
            conn.Open();           
            string Sql = "";
            Sql += "INSERT INTO [dbo].[Additems] ([itemName],[quantity],[price],[subTotal],[tax],[grandToal]) VALUES (@itemName,@quantity,@price,@subTotal,@tax,@grandToal)";            
            SqlCommand cmd = new SqlCommand(Sql, conn);
            cmd.CommandType = CommandType.Text;//تستخدم حتى اضيف متغيرات مثل التحت كذالك الامر يحدث في الداتا بيس
            cmd.Parameters.AddWithValue("@itemName", txtCheckOut.Text);
            cmd.Parameters.AddWithValue("@quantity", comboBox_Quantity.Text);
            cmd.Parameters.AddWithValue("@price", txtPrice.Text);
            cmd.Parameters.AddWithValue("@subTotal", txtPrice.Text);
            cmd.Parameters.AddWithValue("@tax", txtTax.Text);
            cmd.Parameters.AddWithValue("@grandToal", txtGrandTotal.Text);
            cmd.CommandText = Sql;
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("تمت الحفظ بنجاح");
        }
    }
}
