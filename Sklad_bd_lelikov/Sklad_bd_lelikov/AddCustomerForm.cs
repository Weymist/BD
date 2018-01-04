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
    public partial class AddCustomerForm : Form
    {
        public AddCustomerForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SkladDBEntities context = new SkladDBEntities();
                int ID = context.Customer.Max(o => o.ID_customer);
                string name = textBox1.Text;

                Customer c = new Customer
                {
                    C_Name = name,
                    ID_customer = ID + 1
                };

                context.Customer.Add(c);
                context.SaveChanges();
                DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("Заполните поле \"Name\"");
                //throw;
            }
        }
    }
}
