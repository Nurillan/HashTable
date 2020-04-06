using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HashTable
{
    public partial class SelectForm : Form
    {
        public List<CarRecord> resultToys;

        public SelectForm(List<CarRecord> toys)
        {          
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (!checkFields())
            {
                MessageBox.Show("Price must be positive number\nAge must be between 0 and 100", "Inappropriate data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte minAge = byte.Parse(tbAge.Text);
            int maxPrice = int.Parse(tbPrice.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkFields()
        {
            try
            {                
                byte age = byte.Parse(tbAge.Text);
                double price = double.Parse(tbPrice.Text);
                return (age >= 0 && age <= 100 && price >= 0);
            }
            catch
            {
                return false;
            }
        }
    }
}
