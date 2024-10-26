﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospodaUBobra
{
    public partial class RegisterForm : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }

            if (UsernameExists(username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return;
            }

            if (RegisterUser(username, password, UserRole.User.ToString()))
            {
                MessageBox.Show("Registration successful!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again.");
            }
        }

        private bool UsernameExists(string username)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE username = :username";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;

                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        MessageBox.Show($"Debug: User count for username '{username}' is {userCount}");

                        return userCount > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("General error: " + ex.Message);
                return false;
            }
        }


        private bool RegisterUser(string username, string password, string role)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(password, salt);

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Users (username, password, salt, role) VALUES (:username, :password, :salt, :role)";

                    int rowsAffected = 0;

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;
                        cmd.Parameters.Add(new OracleParameter("password", OracleDbType.Varchar2)).Value = hashedPassword;
                        cmd.Parameters.Add(new OracleParameter("salt", OracleDbType.Varchar2)).Value = salt;
                        cmd.Parameters.Add(new OracleParameter("role", OracleDbType.Varchar2)).Value = role;

                        cmd.ExecuteNonQuery();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                    MessageBox.Show("Registration successful!");
                    return rowsAffected > 0;
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("General error: " + ex.Message);
                return false;    
            }
        }

    }
}