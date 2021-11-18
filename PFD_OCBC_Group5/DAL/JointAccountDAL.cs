using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using PFD_OCBC_Group5.Models;
using System.Data;

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

        public string Add(JointAccountModel jointAcc)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO JointAccountInfo (OwnerNRIC, JointNRIC, Relationship)
                                OUTPUT INSERTED.AccountNumber
                                VALUES (@ownerNric, @jointNric, @relationship)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ownerNric", jointAcc.OwnerNRIC);
            cmd.Parameters.AddWithValue("@nric", jointAcc.JointNRIC);
            cmd.Parameters.AddWithValue("@occupation", jointAcc.RelationshipToOwner);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            jointAcc.AccountNumber = (string)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return jointAcc.AccountNumber;
        }
    }
}
