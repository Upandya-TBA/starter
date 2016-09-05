using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateReader
{
    public partial class Form1 : Form
    {
        public static string ENVIRONMENT;
        DB_Hepler db = new DB_Hepler();

        DataTable productDataTable = null;

        public Form1()
        {
            InitializeComponent();
            txtObjectID.ReadOnly = true;
            txtFilePath.ReadOnly = true;

            cmbEnvironment.SelectedIndex = 0;
        }

        private void cmbEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEnvironment.SelectedIndex > 0)
            {
                ENVIRONMENT = cmbEnvironment.Text;
                productDataTable = db.GetAllProduct(ENVIRONMENT.ToUpper());

                cmbProduct.DataSource = productDataTable;
                cmbProduct.DisplayMember = "Object_Name";
                cmbProduct.ValueMember = "OBjectID";
            }
        }   // end of cmbEnvironment_SelectedIndexChanged(object sender, EventArgs e)

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            String s = cmbProduct.SelectedValue.ToString();
            String t = cmbProduct.Text;
            txtObjectID.Text = s;
        }//end of cmbProduct_SelectedIndexChanged(object sender, EventArgs e)

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                txtFilePath.Text = folderPath;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            dataGridView.SelectedRows.Cast<DataGridViewRow>().ToList<DataGridViewRow>().ForEach(listRecords =>
            {
                string query = "Exec DeleteObjectStorage200 '" + listRecords.Cells["ObjectId"].Value.ToString() + "', " + listRecords.Cells["Version"].Value.ToString();
                try
                {
                    dataGridView.Rows.RemoveAt(listRecords.Index);
                    dataGridView.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message, "Auto Update Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
    }
}
