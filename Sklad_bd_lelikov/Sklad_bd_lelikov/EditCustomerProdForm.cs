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
    public partial class EditCustomerProdForm : Form
    {
        int currentCust = 0;
        int currentProd = 0;
        public EditCustomerProdForm(int custId, int prodId)
        {
            InitializeComponent();
            currentCust = custId;
            currentProd = prodId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string date = textBox1.Text;
                SkladDBEntities context = new SkladDBEntities();
                Product_customer p_c = context.Product_customer
                    .Where(o => o.ID_customer == currentCust &&
                                o.ID_product == currentProd).First();
                if (date == "")
                {
                    MessageBox.Show("Заполните поле дата");
                    return;
                }
                p_c.Order_date = date;
                context.Product_customer.Attach(p_c);
                context.Entry(p_c).Property(o => o.Order_date).IsModified = true;
                context.SaveChanges();

                DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show("Заполните поле дата");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void EditCustomerProdForm_Load(object sender, EventArgs e)
        {
            SkladDBEntities context = new SkladDBEntities();

            var trans = from prod in context.Product
                        join prod_cust in context.Product_customer on prod.ID_product equals prod_cust.ID_product
                        where prod_cust.ID_customer == currentCust
                        select new
                        {
                            Product = prod.C_Name,
                            Order_date = prod_cust.Order_date
                        };
            textBox1.Text = trans.ToList()[0].Order_date;
            dataGridView1.DataSource = trans.ToList();
        }
    }
}
