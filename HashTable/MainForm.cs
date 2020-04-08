using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HashTable
{
    public partial class MainForm : Form
    {
        static string initPath = Application.StartupPath + "\\Files";

        FileInfo currentFile;
        HashTable table;

        bool TableIsChanged;
        bool TableIsExist;
        
        public MainForm()
        {
            InitializeComponent();
            CarBrandClass.InitDict();
            dataGridView.AutoGenerateColumns = true;
            dataGridView.MultiSelect = false;
            saveFileDialog.InitialDirectory = initPath;
            openFileDialog.InitialDirectory = initPath;
        }
        
        //buttons in menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableIsExist)
            {
                if (!DeleteTable())
                    return;
            }
            CreateNewTable();
            GenerateTestData();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
                if (!GetSaveDialog())
                    return;
            SaveList();
            TableIsChanged = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GetSaveDialog())
                return;
            SaveList();
            TableIsChanged = false;
        }

        private bool GetSaveDialog()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return false;
            SetNewFile(saveFileDialog.FileName);
            return true;
        }

        private void SaveList()
        {
            IHashTableService saveList = ServiceFactory.getService(currentFile.Extension);
            table.SaveToFile(currentFile);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            if (TableIsExist)
            {
                if (!DeleteTable())
                    return;
            }
            CreateNewTable(openFileDialog.FileName);

            IHashTableService loadList = ServiceFactory.getService(currentFile.Extension);
            table.LoadFromFile(currentFile);
            GridRefresh();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table.Clear();
            GridRefresh();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteTable();
            GridRefresh();
        }

        //buttons on form

        private void btnAdd_Click(object sender, EventArgs e)
        {           
            FormRecordMake formRecordMake = new FormRecordMake();
            if (formRecordMake.ShowDialog() == DialogResult.OK)
            {
                table.Add(formRecordMake.record);
                TableIsChanged = true;
                GridRefresh();
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            CarRecord record = GetSelectedRecord();
            if (record is null)
                return;
            
            FormRecordMake formRecordMake = new FormRecordMake(record);
            if (formRecordMake.ShowDialog() == DialogResult.OK)
            {
                table.Delete(table.GetKey(record));
                record = formRecordMake.record;
                table.Add(record);
                TableIsChanged = true;
                GridRefresh();
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            CarRecord record = GetSelectedRecord();
            if (record is null)
                return;
            table.Delete(table.GetKey(record));
            TableIsChanged = true;
            GridRefresh();
        }

        //main task

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindForm findForm = new FindForm();
            if (findForm.ShowDialog() == DialogResult.Cancel)
                return;
            CarNumber number = findForm.resultNumber;
            CarRecord record = table.Find(number);
            string mes = record == null ? "The record hadn't been found" : record.ToString();
            MessageBox.Show(mes, "Record search", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //auxiliary

        private void GenerateTestData()
        {
            CarRecord record = new CarRecord(CarBrand.Daihatsu, new Person("aa", "aa", "aa"), new CarNumber("aaa", "111", "111"));
            table.Add(record);
            record = new CarRecord(CarBrand.Daihatsu, new Person("bb", "bb", "bb"), new CarNumber("vvv", "222", "222"));
            table.Add(record);
            record = new CarRecord(CarBrand.Daihatsu, new Person("vv", "vv", "vv"), new CarNumber("xxx", "333", "333"));
            table.Add(record);
            record = new CarRecord(CarBrand.Daihatsu, new Person("ss", "ss", "ss"), new CarNumber("ccc", "444", "444"));
            table.Add(record);
            GridRefresh();
        }

        CarRecord GetSelectedRecord()
        {
            return (CarRecord)dataGridView.SelectedRows[0].DataBoundItem;
        }

        void CreateNewTable(string fileName = null)
        {
            InitFields();
            if (fileName != null)
                SetNewFile(fileName);
            ButtonsToggle(true);
        }

        bool DeleteTable()
        {
            if (TableIsChanged)
                if (!ApproveDeletingModified())
                    return false;
            
            ClearFields();
            ButtonsToggle(false);
            return true;
        }

        private bool ApproveDeletingModified()
        {
            DialogResult result = MessageBox.Show("The file has been modified. Save changes?", "Deleting",
                                                  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                                                  MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.Yes)
                saveToolStripMenuItem.PerformClick();

            return true;
        }

        private void SetNewFile(string fileName)
        {
            currentFile = new FileInfo(fileName);
            saveFileDialog.InitialDirectory = currentFile.DirectoryName;
            openFileDialog.InitialDirectory = currentFile.DirectoryName;
        }

        private void InitFields()
        {
            table = new HashTable();
            TableIsChanged = false;
            TableIsExist = true;
        }

        private void ClearFields()
        {
            table = null;
            TableIsExist = false;
            currentFile = null;
        }

        private void ButtonsToggle(bool flag)
        {
            btnAdd.Enabled = flag;
            btnChange.Enabled = flag;
            btnDelete.Enabled = flag;
            findToolStripMenuItem.Enabled = flag;
            saveToolStripMenuItem.Enabled = flag;
            saveAsToolStripMenuItem.Enabled = flag;
            clearToolStripMenuItem.Enabled = flag;
            closeToolStripMenuItem.Enabled = flag;
        }

        private void GridRefresh()
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = table.GetRecordsToList();
        }

    }
}
