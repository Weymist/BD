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
    public partial class Form1 : Form
    {
        int currentProduct = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshProduct();
            RefreshCustomer();

        }

        void RefreshProduct()
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
        void RefreshCustomer()
        {
            SkladDBEntities context = new SkladDBEntities();
            var trans = from cust in context.Customer                        
                        select new
                        {
                            name = cust.C_Name,
                            custID = cust.ID_customer                                                       
                        };
            dataGridView2.DataSource = trans.ToList();
            dataGridView2.Columns[1].Visible = false;
        }
        void RefreshCustProducts()
        {
            SkladDBEntities context = new SkladDBEntities();
            int custID = (int)dataGridView2.CurrentRow.Cells[1].Value;

            var trans = from prod_cust in context.Product_customer
                        join prod in context.Product on prod_cust.ID_product equals prod.ID_product
                        where prod_cust.ID_customer == custID
                        select new
                        {
                            Product = prod.C_Name,
                            Order_Date = prod_cust.Order_date,
                            prodId = prod_cust.ID_product
                        };
            dataGridView3.DataSource = trans.ToList();
            dataGridView3.Columns[2].Visible = false;
        }
        void RefreshProdInfo()
        {
            SkladDBEntities context = new SkladDBEntities();
            int prodID = (int)dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value;
            currentProduct = prodID;

            var trans = from prod_stor in context.Product_Storage_place
                        join stor in context.Storage_place on prod_stor.ID_storage_place equals stor.ID_storage_place
                        where prod_stor.ID_product == prodID
                        select new
                        {
                            Adress = stor.Adress,
                            Count = prod_stor.C_Count
                        };
            dataGridView4.DataSource = trans.ToList();
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            RefreshCustProducts();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            RefreshProdInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddCustomerForm f = new AddCustomerForm();

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                RefreshCustomer();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddProductForm f = new AddProductForm();

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                RefreshProduct();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SkladDBEntities context = new SkladDBEntities();
            int currentProduct = Convert.ToInt32(
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value);

            Product c = context.Product.Find(currentProduct);

            List<Product_Storage_place> invoidce = context.Product_Storage_place
                .Where(o => o.ID_product == currentProduct).ToList();

            List<Product_customer> pc = context.Product_customer
                .Where(o => o.ID_product == currentProduct).ToList();

            for (int i = 0; i < invoidce.Count; i++)
            {
                context.Product_Storage_place.Remove(invoidce[i]);
            }
            for (int i = 0; i < pc.Count; i++)
            {
                context.Product_customer.Remove(pc[i]);
            }
            context.Product.Remove(c);
            context.SaveChanges();

            //RefreshCustomer();
            RefreshCustProducts();
            RefreshProduct();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SkladDBEntities context = new SkladDBEntities();
            int currentCustomer = Convert.ToInt32(
                dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[1].Value);

            Customer c = context.Customer.Find(currentCustomer);

            List<Invoice> invoidce = context.Invoice
                .Where(o => o.ID_customer == currentCustomer).ToList();

            List<Product_customer> pc = context.Product_customer
                .Where(o =>  o.ID_customer == currentCustomer).ToList();

            for (int i = 0; i < invoidce.Count; i++)
            {
                context.Invoice.Remove(invoidce[i]);
            }
            for (int i = 0; i < pc.Count; i++)
            {
                context.Product_customer.Remove(pc[i]);
            }
            context.Customer.Remove(c);
            context.SaveChanges();

            RefreshCustomer();
            RefreshCustProducts();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddInfoForm f = new AddInfoForm(currentProduct);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                RefreshProdInfo();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dataGridView4.Rows[dataGridView4.CurrentRow.Index];
            AddInfoForm f = new AddInfoForm(currentProduct, 
                                            currentRow.Cells[0].Value.ToString(),
                                            (int)currentRow.Cells[1].Value);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                RefreshProdInfo();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string currentRow = dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value.ToString();
            SkladDBEntities context = new SkladDBEntities();
            Storage_place stor = context.Storage_place
                .Where(o => o.Adress == currentRow).First();

            Product_Storage_place p_s = context.Product_Storage_place
                .Where(o => o.ID_product == currentProduct &&
                            o.ID_storage_place == stor.ID_storage_place).First();

            context.Product_Storage_place.Remove(p_s);
            context.SaveChanges();
            RefreshProdInfo();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int currentCust = (int)dataGridView2.CurrentRow.Cells[1].Value;
            AddCustomerProdForm f = new AddCustomerProdForm(currentCust);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                RefreshCustProducts();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int currentProd = (int)dataGridView3.CurrentRow.Cells[2].Value;
            int currentCust = (int)dataGridView2.CurrentRow.Cells[1].Value;
            SkladDBEntities context = new SkladDBEntities();

            Product_customer p_c = context.Product_customer
                .Where(o => o.ID_customer == currentCust &&
                            o.ID_product == currentProd).First();

            context.Product_customer.Remove(p_c);
            context.SaveChanges();
            RefreshCustProducts();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int currentProd = (int)dataGridView3.CurrentRow.Cells[2].Value;
            int currentCust = (int)dataGridView2.CurrentRow.Cells[1].Value;
            EditCustomerProdForm f = new EditCustomerProdForm(currentCust, currentProd);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                RefreshCustProducts();
            }
        }
    }
}
