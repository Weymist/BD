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
    public partial class AddInfoForm : Form
    {
        string addr = "";
        int currentProd = 0;
        int count = 0;
        public AddInfoForm(int id)
        {
            InitializeComponent();
            currentProd = id;
            button3.Enabled = false;
        }

        public AddInfoForm(int id, string adress, int count)
        {
            InitializeComponent();
            button1.Enabled = false;
            currentProd = id;
            addr = adress;
            this.count = count;
            textBox1.Text = addr;
            textBox2.Text = count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt32(textBox2.Text);
                string adr = textBox1.Text;

                if (count < 0)
                {
                    MessageBox.Show("Введите корректное число в поле \"Count\"");
                    return;
                }
                SkladDBEntities context = new SkladDBEntities();

                List<Storage_place> p = context.Storage_place
                    .Where(o => o.Adress == adr).ToList();
                if (p.Count == 0)
                {
                    int idStor = context.Storage_place.Count() != 0 ?
                        context.Storage_place.Max(o => o.ID_storage_place) + 1 : 1;

                    Storage_place s = new Storage_place
                    {
                        ID_storage_place = idStor,
                        Adress = adr
                    };
                    Product_Storage_place n = new Product_Storage_place
                    {
                        ID_storage_place = s.ID_storage_place,
                        ID_product = currentProd,
                        C_Count = count
                    };
                    context.Storage_place.Add(s);
                    context.Product_Storage_place.Add(n);
                }
                else
                {
                    Product_Storage_place n = new Product_Storage_place
                    {
                        ID_storage_place = p[0].ID_storage_place,
                        ID_product = currentProd,
                        C_Count = count
                    };
                    context.Product_Storage_place.Add(n);
                }

                context.SaveChanges();
                DialogResult = DialogResult.OK;
            }
            catch (FormatException) { MessageBox.Show("Заполните поля"); }
            catch (Exception) { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int nCount = Convert.ToInt32(textBox2.Text);
                string adr = textBox1.Text;

                if (nCount < 0)
                {
                    MessageBox.Show("Введите корректное число в поле \"Count\"");
                    return;
                }
                SkladDBEntities context = new SkladDBEntities();
                Storage_place stor = context.Storage_place
                    .Where(o => o.Adress == addr)
                    .First();

                Product_Storage_place p_s = context.Product_Storage_place
                    .Where(o => o.ID_product == currentProd &&
                                o.ID_storage_place == stor.ID_storage_place).First();
                    //.Find(currentProd, stor.ID_storage_place);

                stor.Adress = adr;
                p_s.C_Count = nCount;

                context.Storage_place.Attach(stor);
                context.Entry(stor).Property(o => o.Adress).IsModified = true;
                context.Product_Storage_place.Attach(p_s);
                context.Entry(p_s).Property(o => o.C_Count).IsModified = true;
                context.SaveChanges();
                DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show("Заполните поля");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
