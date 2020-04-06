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
                cbCarBrand.SelectedIndex = (int)record.Brand - 1;
                mtbCarNumber.Text = record.Number.ToString();
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
            
            if (!checkMtb() || (tbName.Text.Trim() == "") || 
                (tbSurname.Text.Trim() == "") || (tbPatronymic.Text.Trim() == ""))
            {
                MessageBox.Show("Fill the gaps", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedBrand = cbCarBrand.SelectedIndex + 1;
            CarBrand brand = CarBrandClass.CarBrandDict[selectedBrand];
            Person person = new Person(tbName.Text.Trim(), tbSurname.Text.Trim(), tbPatronymic.Text.Trim());
            CarNumber number = new CarNumber(mtbCarNumber.Text);

            this.record = new CarRecord(brand, person, number);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkMtb()
        {
            mtbCarNumber.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string text = mtbCarNumber.Text;
            if (text.Length < 9)
                return false;
            bool flag = true;
            for(int i = 0; i < text.Length && flag == true; i ++)
                flag = text[i] != ' ';
            return flag;
        }
    }
}
