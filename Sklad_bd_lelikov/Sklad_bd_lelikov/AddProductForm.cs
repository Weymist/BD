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
    public partial class AddProductForm : Form
    {
        public AddProductForm()
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
                int id = context.Product.Max(o => o.ID_product);
                string name = textBox1.Text;
                string manuf = textBox2.Text;

                List<Manufacturer> m = context.Manufacturer
                    .Where(o => o.C_Name == name).ToList();

                if (m.Count == 0)
                {
                    int idM = context.Manufacturer.Max(o => o.ID_manufacturer);
                    Manufacturer man = new Manufacturer
                    {
                        ID_manufacturer = idM + 1,
                        C_Name = manuf
                    };
                    Product p = new Product
                    {
                        C_Name = name,
                        ID_product = id + 1,
                        C_Count = 0,
                        ID_manufacturer = man.ID_manufacturer
                    };

                    context.Manufacturer.Add(man);
                    context.Product.Add(p);
                }
                else
                {
                    Product p = new Product
                    {
                        C_Name = name,
                        ID_product = id + 1,
                        C_Count = 0,
                        ID_manufacturer = m[0].ID_manufacturer
                    };

                    context.Product.Add(p);
                }

                context.SaveChanges();
                DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("Заполните пустые поля");
            }
        }
    }
}
