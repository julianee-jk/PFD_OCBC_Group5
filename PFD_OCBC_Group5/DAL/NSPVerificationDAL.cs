using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PFD_OCBC_Group5.DAL
{
    public class NSPVerificationDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public NSPVerificationDAL()
        {
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJPConnectionString");

            conn = new SqlConnection(strConn);
        }

        //public int Add(NSPVerificationDAL verification)
        //{
        //    SqlCommand cmd = conn.CreateCommand();

        //    cmd.CommandText = @"INSERT INTO CompetitionSubmission (CompetitionID, CompetitorID, FileSubmitted, DateTimeFileUpload, Appeal, VoteCount, Ranking)
        //                    OUTPUT INSERTED.CompetitionID
        //                    VALUES(@competitionid, @competitorid, @filesubmitted, @datetimefileupload, @appeal, @votecount, @ranking)";

        //    cmd.Parameters.AddWithValue("@competitionid", submission.CompetitionID);
        //    cmd.Parameters.AddWithValue("@competitorid", submission.CompetitorID);
        //    cmd.Parameters.AddWithValue("@filesubmitted", DBNull.Value);
        //    cmd.Parameters.AddWithValue("@datetimefileupload", DBNull.Value);
        //    cmd.Parameters.AddWithValue("@appeal", DBNull.Value);
        //    cmd.Parameters.AddWithValue("@votecount", submission.VoteCount);
        //    cmd.Parameters.AddWithValue("@ranking", DBNull.Value);

        //    conn.Open();

        //    submission.CompetitionID = (int)cmd.ExecuteScalar();

        //    conn.Close();

        //    return submission.CompetitionID;
        //}
    }
}
