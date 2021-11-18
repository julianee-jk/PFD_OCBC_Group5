﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using PFD_OCBC_Group5.Models;
using System.Data;
using System.Diagnostics;

namespace PFD_OCBC_Group5.DAL
{
    public class JointAccountDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public JointAccountDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "FormConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public int Add(JointAccountModel jointAcc)
        {
            Debug.WriteLine(jointAcc.JointNRIC);
            Debug.WriteLine(jointAcc.RelationshipToOwner);
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO JointAccountInfo (OwnerNRIC, JointNRIC, RelationshipToOwner)
                                OUTPUT INSERTED.AccountNumber
                                VALUES (@ownerNric, @jointNric, @relationship)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ownerNric", jointAcc.OwnerNRIC);
            cmd.Parameters.AddWithValue("@jointNric", jointAcc.JointNRIC);
            cmd.Parameters.AddWithValue("@relationship", jointAcc.RelationshipToOwner);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            jointAcc.AccountNumber = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return jointAcc.AccountNumber;
        }

        public int UpdateCreateAccount(string nric)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE AccountHolder SET AccountCreated=@accountCreated WHERE NRIC=@nric";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.

            cmd.Parameters.AddWithValue("@nric", nric);
            cmd.Parameters.AddWithValue("@accountCreated", "Y");


            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }
    }
}
