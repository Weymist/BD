using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sklad_bd_lelikov
{
    public partial class AddCustomerProdForm : Form
    {
        int currentCust = 0;
        public AddCustomerProdForm(int id)
        {
            InitializeComponent();
            currentCust = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void AddCustomerProdForm_Load(object sender, EventArgs e)
        {
            SkladDBEntities context = new SkladDBEntities();
            var trans = from prod in context.Product
                        join manuf in context.Manufacturer on prod.ID_manufacturer equals manuf.ID_manufacturer
                        select new
                        {
                            name = prod.C_Name,
                            manufacturer = manuf.C_Name,
                            prodID = prod.ID_product
                        };
            dataGridView1.DataSource = trans.ToList();
            dataGridView1.Columns[2].Visible = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                SkladDBEntities context = new SkladDBEntities();
                int currentProd = (int)dataGridView1.CurrentRow.Cells[2].Value;

                Product_customer p_c = new Product_customer
                {
                    ID_product = currentProd,
                    ID_customer = currentCust,
                    Order_date = DateTime.Now.Date.ToString()
                };

                context.Product_customer.Add(p_c);
                context.SaveChanges();
                MessageBox.Show("Товар добавлен");

            }
            catch (Exception)
            {
                MessageBox.Show("Товар уже добавлен");
            }
        }

        private void AddCustomerProdForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
