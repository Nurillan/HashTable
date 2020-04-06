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
    public partial class FormRecordMake : Form
    {        
        public CarRecord record { get; private set; }

        public FormRecordMake(CarRecord record = null)
        {
            InitializeComponent();
            this.record = record;
            InitFields();
            
        }

        private void InitFields()
        {
            cbCarBrand.DataSource = new BindingSource(CarBrandClass.CarBrandDict, null);
            cbCarBrand.DisplayMember = "Value";
            cbCarBrand.ValueMember = "Key";

            if (record != null)
            {
                cbCarBrand.SelectedIndex = (int)record.Brand;
                tbCarNumber.Text = record.Number.ToString();
                tbName.Text = record.Person.Name;
                tbSurname.Text = record.Person.Surname;
                tbPatronymic.Text = record.Person.Patronymic;
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            int selectedBrand = cbCarBrand.SelectedIndex + 1;
            CarBrand brand = CarBrandClass.CarBrandDict[selectedBrand];
            Person person = new Person(tbName.Text.Trim(), tbSurname.Text.Trim(), tbPatronymic.Text.Trim());
            CarNumber number = MakeCarNumber();

            this.record = new CarRecord(brand, person, number);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private CarNumber MakeCarNumber()
        {
            string text = tbCarNumber.Text;
            string letters = text[0].ToString() + text[4] + text[5];
            string num = text.Substring(1, 3);
            string region = text.Substring(text.IndexOf('(')).Remove(text.Length - 1);
            return new CarNumber(letters, num, region);
        }
    }
}
