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
    public class AccountDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        public AccountDAL()
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

        public string Add(AccountFormModel account)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO AccountHolder (NRIC, Name, Nationality, DOB, Occupation, PR, Gender, SelfEmployed, NatureOfBusiness, HomeAddress, PostalCode, MailingAddress, MailingPostalCode, Email, MobileNumber, HomeNumber, AccountCreated)
OUTPUT INSERTED.NRIC
VALUES(@nric, @name, @nationality, @dob, @occupation, @pr, @gender, @selfEmployed, @natureOfBusiness, @homeAddress, @postalCode, @mailingAddress, @mailingPostalCode, @email, @mobileNumber, @homeNumber, @accountCreated)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            
            cmd.Parameters.AddWithValue("@nric", account.NRIC);
            cmd.Parameters.AddWithValue("@name", account.Name);
            cmd.Parameters.AddWithValue("@nationality", account.Nationality);
            cmd.Parameters.AddWithValue("@dob", account.DOB);
            cmd.Parameters.AddWithValue("@occupation", account.Occupation ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pr", account.PR ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@gender", account.Gender ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@selfEmployed", account.SelfEmployed ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@natureOfBusiness", account.NatureOfBusiness ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@homeAddress", account.HomeAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@postalCode", account.PostalCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@mailingAddress", account.MailingAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@mailingPostalCode", account.MailingPostalCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", account.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@mobileNumber", account.MobileNumber);
            cmd.Parameters.AddWithValue("@homeNumber", account.HomeNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@accountCreated", account.AccountCreated);
            


           

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return account.NRIC;
        }

        public int Update(AccountFormModel account)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE AccountHolder SET Name=@name, Nationality=@nationality, DOB=@dob, Occupation=@occupation, PR=@pr, Gender=@gender, SelfEmployed=@selfEmployed, NatureOfBusiness=@natureOfBusiness, HomeAddress=@homeAddress, PostalCode=@postalCode, MailingAddress=@mailingAddress, MailingPostalCode=@mailingPostalCode, Email=@email, HomeNumber=@homeNumber, AccountCreated=@accountCreated WHERE NRIC=@nric";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.

            
            

            cmd.Parameters.AddWithValue("@nric", account.NRIC);
            cmd.Parameters.AddWithValue("@name", account.Name);
            cmd.Parameters.AddWithValue("@nationality", account.Nationality);
            cmd.Parameters.AddWithValue("@dob", account.DOB);
            cmd.Parameters.AddWithValue("@occupation", account.Occupation ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pr", account.PR ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@gender", account.Gender ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@selfEmployed", account.SelfEmployed ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@natureOfBusiness", account.NatureOfBusiness ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@homeAddress", account.HomeAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@postalCode", account.PostalCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@mailingAddress", account.MailingAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@mailingPostalCode", account.MailingPostalCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", account.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@mobileNumber", account.MobileNumber);
            cmd.Parameters.AddWithValue("@homeNumber", account.HomeNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@accountCreated", account.AccountCreated);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public bool AccountExists(string nric)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM AccountHolder WHERE NRIC = @nric";
            cmd.Parameters.AddWithValue("@nric", nric);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            bool exists = reader.HasRows;

            reader.Close();
            conn.Close();

            return exists;
        }

    }


}
