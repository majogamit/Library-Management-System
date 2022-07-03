﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    public partial class ViewTab : Form
    {
        public ViewTab()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home newTab = new Home();
            newTab.ShowDialog();
            this.Close();
        }

        // Loads the database to dataGrid when ViewTab form is launched
        private void ViewTab_Load(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-9MBNT14\\SQLEXPRESS;Initial Catalog=libraryData;Integrated Security=True");
            con.Open();

            cmd = new SqlCommand("SELECT * FROM booksData", con);
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgvBooks.DataSource = dt;
            con.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {            
            // checks if txtSearch is empty
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                ViewTab_Load(sender, e);
            }
            else
            {
                con = new SqlConnection("Data Source=DESKTOP-9MBNT14\\SQLEXPRESS;Initial Catalog=libraryData;Integrated Security=True");
                con.Open();
                string searchText = txtSearch.Text;

                // checks if input in txtSearch is an integer value
                if (int.TryParse(txtSearch.Text, out int id))
                {
                    string cmdTextInt = "SELECT * FROM booksData WHERE BookID = '" + searchText + "'";
                    cmd = new SqlCommand(cmdTextInt, con);
                }
                else
                {
                    string cmdTextStr = "SELECT * FROM booksData WHERE Title = '" + searchText + "' OR Author = '" + searchText + "' OR Genre = '" + searchText + "' OR Edition = '" + searchText + "' OR Publication = '" + searchText + "'";
                    cmd = new SqlCommand(cmdTextStr, con);
                }

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dgvBooks.DataSource = dt;
                con.Close();
            }            
        }

        // Checks if the user pressed enter in txtSearch
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {         
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }            
        }

        // Sort feature incomplete. Currently in testing.
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSort.SelectedIndex == 0)
                MessageBox.Show("You chose Alphabetical");
            if (cmbSort.SelectedIndex == 1)
                MessageBox.Show("You chose Book ID");
            if (cmbSort.SelectedIndex == 2)
                MessageBox.Show("You chose Genre");
        }
    }
}
