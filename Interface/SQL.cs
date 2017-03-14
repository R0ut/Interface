using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    class SQL
    {

        // metoda na dodanie do obu tabel samochodu id,marka,model,rok,wielkosc,naped,obrazek
        public void insertSQL(DataGridView dgCars, DataGridView dgCarsDetail, TextBox id, TextBox brand, TextBox model, TextBox year, string vehicleSize, string wheels, byte[] img)
        {
            string query1 = "INSERT INTO Cars (id,Brand,Model) VALUES(@id, @Brand, @Model)";
            string query2 = "INSERT INTO CarsDetail (YearMade, VehicleSize, DrivenWheels, Picture, eid) VALUES(@YearMade, @VehicleSize, @DrivenWheels, @Picture, @eid)";


            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"))
                {
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@id", Int32.Parse(id.Text));
                    command.Parameters.AddWithValue("@Brand", brand.Text);
                    command.Parameters.AddWithValue("@Model", model.Text);
                    connection.Open();
                    command.ExecuteNonQuery();


                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@eid", Int32.Parse(id.Text));
                    command2.Parameters.AddWithValue("@YearMade", year.Text);
                    command2.Parameters.AddWithValue("@VehicleSize", vehicleSize);
                    command2.Parameters.AddWithValue("@DrivenWheels", wheels);
                    command2.Parameters.AddWithValue("@Picture", img);
                    command2.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Data added correctly");
                refresh(dgCars);
                refresh(dgCarsDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with database, Id must be unique");
                MessageBox.Show(ex.ToString());
            }
        }
        //metoda do wyswietlenia obrazka aktualnie wybranego auta
        public void ShowIMG(PictureBox pic, DataGridView dg)
        {
            string query = "SELECT a.Picture from CarsDetail a " +
                "WHERE a.eid = @parm";
                
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"))
                {
                    
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@parm", dg.CurrentRow.Cells[0].Value);
                    connection.Open();
                    byte[] imgg = (byte[])cmd.ExecuteScalar();
                    MemoryStream strr = new MemoryStream();
                    strr.Write(imgg, 0, imgg.Length);
                    Bitmap bitt = new Bitmap(strr);
                    pic.Image = bitt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with database");
                MessageBox.Show(ex.ToString());
            }
        }
        //metoda updatuje rekord ktory jest zaznacznoy
        public void updateSQL(DataGridView dgCars, DataGridView dgCarsDetail, TextBox brand, TextBox model, TextBox year, string vehicleSize, string wheels, byte[] img)
        {


            string query1 = "UPDATE Cars SET Brand = @Brand, Model = @Model WHERE id = @id";
            string query2 = "UPDATE CarsDetail SET YearMade = @YearMade, VehicleSize = @VehicleSize,  DrivenWheels = @DrivenWheels,  Picture = @Picture WHERE eid = @idd";


            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"))
                {
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@id", dgCars.CurrentRow.Cells[0].Value);
                    command.Parameters.AddWithValue("@Brand", brand.Text);
                    command.Parameters.AddWithValue("@Model", model.Text);
                    connection.Open();
                    command.ExecuteNonQuery();


                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@idd", dgCars.CurrentRow.Cells[0].Value);
                    command2.Parameters.AddWithValue("@YearMade", year.Text);
                    command2.Parameters.AddWithValue("@VehicleSize", vehicleSize);
                    command2.Parameters.AddWithValue("@DrivenWheels", wheels);
                    command2.Parameters.AddWithValue("@Picture", img);
                    command2.ExecuteNonQuery();
                    connection.Close();
                }

               
                MessageBox.Show("Data updated correctly");
                refresh(dgCars);
                refresh(dgCarsDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with database, Id must be unique");
                MessageBox.Show(ex.ToString());
            }
        }

        public void selectCarsDetail(DataGridView dgDetail, DataGridView dgCars)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"); // Your Connection String here


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = con;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@id", dgCars.CurrentRow.Cells[0].Value);
            sqlCmd.CommandText = "SELECT * FROM CarsDetail WHERE eid = @id";
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);
            DataTable dtRecord = new DataTable();
            sqlDataAdap.Fill(dtRecord);
            dgDetail.DataSource = dtRecord;
        }
        //option 1 for dgv Cars option 2 for dgv CarsDetail
        public void deleteSQL(DataGridView dgCars, DataGridView dgCarsDetail)
        {

            string query1 = "DELETE From Cars WHERE id = @id";
            string query2 = "DELETE From CarsDetail WHERE eid = @id";


            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"))
                {
                    SqlCommand command = new SqlCommand(query2, connection);
                    command.Parameters.AddWithValue("@id", dgCars.CurrentRow.Cells[0].Value);
                    connection.Open();
                    command.ExecuteNonQuery();


                    SqlCommand command2 = new SqlCommand(query1, connection);
                    command2.Parameters.AddWithValue("@id", dgCars.CurrentRow.Cells[0].Value);
                    command2.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Data deleted correctly");
                refresh(dgCars);
                refresh(dgCarsDetail);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with database, Id must be unique");
                MessageBox.Show(ex.ToString());
            }
        }

        public void refresh(DataGridView dgvCars)
        {
            string query1 = "Select * From Cars";
            


            try
            {
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"); // Your Connection String here


                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = query1;
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);
                DataTable dtRecord = new DataTable();
                sqlDataAdap.Fill(dtRecord);
                dgvCars.DataSource = dtRecord;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with database, Id must be unique");
                MessageBox.Show(ex.ToString());
            }
        }

        public void searchSQL(DataGridView dg, TextBox txt)
        {
         
           
            string query2 = "SELECT * FROM Cars WHERE Brand like('%" + txt.Text +"%') OR Model like('%" + txt.Text + "%')";


            try
            {
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\CarsDB.mdf; Integrated Security = True"); // Your Connection String here


                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = query2;
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);
                DataTable dtRecord = new DataTable();
                sqlDataAdap.Fill(dtRecord);
                dg.DataSource = dtRecord;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with database, Id must be unique");
                MessageBox.Show(ex.ToString());
            }
        }

       
    





    }
}
