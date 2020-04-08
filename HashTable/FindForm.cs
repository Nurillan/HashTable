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
    public partial class FindForm : Form
    {
        public CarNumber resultNumber;

        public FindForm()
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
            if (!checkMtb())
            {
                MessageBox.Show("Fill the gaps", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            resultNumber = new CarNumber(mtbCarNumber.Text);
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
            for (int i = 0; i < text.Length && flag == true; i++)
                flag = text[i] != ' ';
            mtbCarNumber.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            return flag;
        }
    }
}
