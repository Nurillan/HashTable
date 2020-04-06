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
        List<CarRecord> records;

        bool ListIsChanged;
        bool ListIsExist;
        
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
            if (ListIsExist)
            {
                if (!DeleteList())
                    return;
            }
            CreateNewList();

            CarRecord toy1 = new CarRecord(CarBrand.Daihatsu, new Person("aa", "aa", "aa"), new CarNumber("aaa", "111", "111"));
            records.Add(toy1);
            toy1 = new CarRecord(CarBrand.Daihatsu, new Person("bb", "bb", "bb"), new CarNumber("vvv", "222", "22"));
            records.Add(toy1);
            toy1 = new CarRecord(CarBrand.Daihatsu, new Person("vv", "vv", "vv"), new CarNumber("xxx", "333", "33"));
            records.Add(toy1);
            toy1 = new CarRecord(CarBrand.Daihatsu, new Person("ss", "ss", "ss"), new CarNumber("ccc", "444", "44"));
            records.Add(toy1);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
                if (!GetSaveDialog())
                    return;
            SaveList();
            ListIsChanged = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GetSaveDialog())
                return;
            SaveList();
            ListIsChanged = false;
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
            IToyListService saveList = ServiceFactory.getService(currentFile.Extension);
            saveList.Save(records, currentFile.FullName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            if (ListIsExist)
            {
                if (!DeleteList())
                    return;
            }
            CreateNewList(openFileDialog.FileName);

            IToyListService loadList = ServiceFactory.getService(currentFile.Extension);
            records = loadList.Load(currentFile.FullName);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            records.Clear();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteList();
        }

        //buttons on form

        private void btnAdd_Click(object sender, EventArgs e)
        {           
            FormRecordMake formToy = new FormRecordMake();
            if (formToy.ShowDialog() == DialogResult.OK)
            {
                records.Add(formToy.record);
                ListIsChanged = true;
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            int i = GetSelectedToyIndex();
            if (i == -1)
                return;
            
            FormRecordMake formToy = new FormRecordMake(records[i]);
            if (formToy.ShowDialog() == DialogResult.OK)
            {
                records[i] = formToy.record;
                ListIsChanged = true;
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = GetSelectedToyIndex();
            if (i != -1)
            {
                records.RemoveAt(i);
                ListIsChanged = true;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = records;
        }

        //main task

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
           /* SelectForm selectForm = new SelectForm(records);
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                List<CarRecord> selectedToys = selectForm.resultToys;
                ToysListHelper toys = new ToysListHelper(this.records);
                unselectedToys = toys.ListDif(selectedToys);
                this.records = selectedToys;

                selectToolStripMenuItem.Click -= selectToolStripMenuItem_Click;
                selectToolStripMenuItem.Click += returnAllToyToGrid;
                selectToolStripMenuItem.Text = "Return";

            }*/
        }

        //auxiliary

        int GetSelectedToyIndex()
        {
            if (dataGridView.SelectedRows.Count != 1)
                throw new Exception("Only one field have to be selected");

            CarRecord toy = (CarRecord)dataGridView.SelectedRows[0].DataBoundItem;
            return records.IndexOf(toy);
        }

        void CreateNewList(string fileName = null)
        {
            InitFields();
            if (fileName != null)
                SetNewFile(fileName);
            ButtonsToggle(true);
        }

        bool DeleteList()
        {
            if (ListIsChanged)
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
            records = new List<CarRecord>();
            ListIsChanged = false;
            ListIsExist = true;
        }

        private void ClearFields()
        {
            records = null;
            ListIsExist = false;
            currentFile = null;
        }

        private void ButtonsToggle(bool flag)
        {
            btnAdd.Enabled = flag;
            btnChange.Enabled = flag;
            btnDelete.Enabled = flag;
            btnRefresh.Enabled = flag;
            selectToolStripMenuItem.Enabled = flag;
            saveToolStripMenuItem.Enabled = flag;
            saveAsToolStripMenuItem.Enabled = flag;
            clearToolStripMenuItem.Enabled = flag;
            closeToolStripMenuItem.Enabled = flag;
        }

    }
}
