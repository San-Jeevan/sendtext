
using System.IO;

using Common.SQL;
using Common.SQL.Models;
using SQLite;

namespace Common.SQL
{


    public static class SQLCore
    {
    

        private static string CreateDatabase(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                {
                    connection.CreateTableAsync<Contact>();
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }


        private static string InsertUpdateData(Contact data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                if (db.InsertAsync(data).Result != 0)
                    db.UpdateAsync(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }


        private static int FindNumberRecords(string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalarAsync<int>("SELECT Count(*) FROM Person");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return count.Result;
            }
            catch (SQLiteException ex)
            {
                return -1;
            }
        }



    }
    }

