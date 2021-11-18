using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFD_OCBC_Group5.Models;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PFD_OCBC_Group5.DAL
{
    public class NSPVerificationDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public NSPVerificationDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJPConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public string Add(NSPVerification verification)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO AccountVerification (NRIC, ImageFile, VerificationCode, DateUploaded)
                            OUTPUT INSERTED.NRIC
                            VALUES(@nric, @imagefile, @verificationcode, @dateuploaded)";

            cmd.Parameters.AddWithValue("@nric", verification.NRIC);
            cmd.Parameters.AddWithValue("@imagefile", verification.VerificationImage);
            cmd.Parameters.AddWithValue("@verificationcode", verification.VerificationCode);
            cmd.Parameters.AddWithValue("@dateuploaded", verification.VerificationDate);

            conn.Open();

            verification.NRIC = (string)cmd.ExecuteScalar();

            conn.Close();

            return verification.NRIC;
        }

        public List<NSPVerification> GetAllVerificationDetails(string NRIC)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"";

            cmd.Parameters.AddWithValue("@nric", NRIC);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<NSPVerification> verificationList = new List<NSPVerification>();

            while (reader.Read())
            {
                verificationList.Add(
                    new NSPVerification
                    {
                        NRIC = reader.GetString(0),
                        VerificationImage = reader.GetString(1),
                        VerificationCode = reader.GetString(2),
                        VerificationDate = reader.GetDateTime(3)
                    }

                );
            }

            // Close DataReader
            reader.Close();

            conn.Close();

            return verificationList;

        }
    }
}
