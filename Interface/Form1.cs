using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{

  
    public partial class Form1 : Form
    {
        SQL sql;
        byte[] img;
        bool flag = false;
        public Form1()
        {
            InitializeComponent();
            sql = new SQL();
            clear();
            carsDetailDataGridView.ScrollBars = ScrollBars.None;
            carsDataGridView.ScrollBars = ScrollBars.Vertical;

        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            clear();
            groupBox1.Visible = true;

            txtId.Text = carsDataGridView.RowCount.ToString();
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Pliki JPG(*.jpg)|*.jpg";
            openFileDialog.FileName = "*.*";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = false;
            DialogResult resoult = openFileDialog.ShowDialog();

            if (resoult == DialogResult.OK)
            {
                try
                {
                    string a = openFileDialog.FileName;
                    img = File.ReadAllBytes(a);
                    picImageStatus.Image = Properties.Resources.yes;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading picture\n\n" + ex.ToString());
                }

            }
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'carsDBDataSet.CarsDetail' table. You can move, or remove it, as needed.
            this.carsDetailTableAdapter.Fill(this.carsDBDataSet.CarsDetail);

            // TODO: This line of code loads data into the 'carsDBDataSet.Cars' table. You can move, or remove it, as needed.
            this.carsTableAdapter.Fill(this.carsDBDataSet.Cars);

        }

       
       

        private void carsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sql.ShowIMG(picCar, carsDataGridView);
            sql.selectCarsDetail(carsDetailDataGridView, carsDataGridView);

            
            
               // MessageBox.Show(carsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
           // MessageBox.Show(carsDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            txtId.Text = carsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txtBrand.Text = carsDataGridView.CurrentRow.Cells[1].Value.ToString();
            txtModel.Text = carsDataGridView.CurrentRow.Cells[2].Value.ToString();
            txtYear.Text = carsDetailDataGridView.CurrentRow.Cells[0].Value.ToString();
            cboxSize.Text = carsDetailDataGridView.CurrentRow.Cells[1].Value.ToString();
            cBoxWheel.Text = carsDetailDataGridView.CurrentRow.Cells[2].Value.ToString();
            flag = true;
        }

       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            sql.deleteSQL(carsDataGridView, carsDetailDataGridView);
            picCar.Image = null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            sql.refresh(carsDataGridView);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "") sql.refresh(carsDataGridView);
            else sql.searchSQL(carsDataGridView, txtSearch);
        }

        private void clear()
        {
            txtBrand.Text = "";
            txtId.Text = "";
            txtModel.Text = "";
            txtYear.Text = "";
            cboxSize.Text = "Please, select any value";
            cBoxWheel.Text = "Please, select any value";
            picImageStatus.Image = Properties.Resources.no;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (flag == false)
                sql.insertSQL(carsDataGridView, carsDetailDataGridView, txtId, txtBrand, txtModel, txtYear, cboxSize.Text, cBoxWheel.Text, img);
            else
                sql.updateSQL(carsDataGridView, carsDetailDataGridView, txtBrand, txtModel, txtYear, cboxSize.Text, cBoxWheel.Text, img);
            groupBox1.Visible = false;
            

        }
    }
}
