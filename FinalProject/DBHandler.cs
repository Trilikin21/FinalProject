﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public sealed class DBHandler
    {
        private DBHandler()
        { }

        private static readonly DBHandler instance = new DBHandler();

        public static DBHandler Instance
        {
            get { return instance; }
        }

        private string ConnString = ConfigurationManager.ConnectionStrings["ContactConn"].ConnectionString;

        public List<Person> ReadAllPersons()
        {
            List<Person> personList = new List<Person>();

            using (SqlConnection conn = new SqlConnection(ConnString))

            {
                conn.Open();
                SqlCommand cm = new SqlCommand("select * from Person", conn);

                using (SqlDataReader sqlDataReader = cm.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        Person person = new Person();

                        if (Int32.TryParse(sqlDataReader["Id"].ToString(), out int id))
                        {
                            person.Id = id;
                        }
                        person.FirstName = sqlDataReader["FirstName"].ToString();
                        person.LastName = sqlDataReader["LastName"].ToString();
                        person.Email = sqlDataReader["Email"].ToString();

                        if (Int32.TryParse(sqlDataReader["Age"].ToString(), out int age))
                        {
                            person.Age = age;
                        }

                        person.Email = sqlDataReader["Email"].ToString();

                        person.PhoneNumber = sqlDataReader["Phone_Number"].ToString();

                        personList.Add(person);
                    }
                }
            }

            return personList;
        }

        public void InsertingRecord(string FirstName, string LastName, int Age, string Email, string Phone)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string query = "insert into Person (FirstName, LastName, Age, Email, Phone_Number) values(@FN,@LN,@A,@E,@PN)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@FN", FirstName);
                    cmd.Parameters.AddWithValue("@LN", LastName);
                    cmd.Parameters.AddWithValue("@A", Age);
                    cmd.Parameters.AddWithValue("@E", Email);
                    cmd.Parameters.AddWithValue("@PN", Phone);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateRecord(string FirstName, string LastName, int Age, string Email, string Phone, int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string query = "update Person set FirstName = @FN, LastName = @LN, Age = @A, Email = @E, Phone_Number = @PN where Id = @rID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@FN", FirstName);
                    cmd.Parameters.AddWithValue("@LN", LastName);
                    cmd.Parameters.AddWithValue("@A", Age);
                    cmd.Parameters.AddWithValue("@E", Email);
                    cmd.Parameters.AddWithValue("@PN", Phone);
                    cmd.Parameters.AddWithValue("@rID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRecord(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("delete from Person where Id = @rID", conn, transaction);
                cmd.Parameters.AddWithValue("@rID", id);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                conn.Close();
            }
        }

        public void DeleteAllRecord()
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("delete from Person", conn, transaction);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                conn.Close();
            }
        }
    }
}