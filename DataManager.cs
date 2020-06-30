using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Text.Json;
using System.IO;

namespace WebForms
{
    public static class DataManager
    {
        static string InstanceName = "TESTSQLINSTANCE";
        static string DbName = "Contacts";
        static DataManager()
        {
            if (File.Exists(@"wwwroot\dbsettings.txt"))
            {

                string FileContent = File.ReadAllText(@"wwwroot\dbsettings.txt");
                try
                {
                    InstanceName = FileContent.Split(',')[0];
                    DbName = FileContent.Split(',')[1];
                }
                catch { }
            }
            EnsureTableExists();
        }
        private static SqlConnection NewConnection()
        {
            string connectionString = @$"Data Source=.\{InstanceName};Initial Catalog={DbName};Integrated Security=True";
            return new SqlConnection(connectionString);
        }

        public static string[] ContactMainFields = { "id", "FirstName", "LastName", "PhoneNumber1" };

        public static string[] ContactShowableFields = { "id", "FirstName", "LastName", "Company", "PhoneNumber1", "PhoneNumber2", "PhoneNumber3", "Commentary" };

        public static string ParameterStringToCreateVUE()
        {
            string res = "";
            bool First = true;
            foreach (string Field in ContactShowableFields)
            {
                res = res + (First ? "" : ",\n") + Field + ": \' \'";
                First = false;
            }
            return res;

        }

        public static void EnlistNewContact(Dictionary<string, object> Data)
        {
            var FilteredData = Data.Where(elem => ContactShowableFields.Contains(elem.Key) && elem.Key != "id");

            string QueryText = "insert into ContactsInfo (" +
                String.Join(", ", FilteredData.Select(elem => elem.Key))
                + ")" + "values (" +
                String.Join(", ", FilteredData.Select(elem => QuoteIfString(elem.Value))) + ")";
            //" where id = " + Data["id"].ToString();

            var conn = NewConnection();
            conn.Open();

            new SqlCommand(QueryText, conn).ExecuteNonQuery();
        }

        public static void EnsureTableExists()
        {
            var conn = NewConnection();
            var TableCreatingExpression = "Create table ContactsInfo (id int Primary key Not Null Identity(1,1), FirstName text, " +
                "LastName text, Company text, PhoneNumber1 text, PhoneNumber2 text, PhoneNumber3 text, Commentary text)";
            try
            {
                var command = new SqlCommand(TableCreatingExpression, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            { conn.Close(); }

        }

        private static string QuoteIfString(object PossibleString)
        {
            if (((JsonElement)PossibleString).ValueKind is JsonValueKind.String)
            {
                return "'" + PossibleString.ToString() + "'";
            }
            else { return PossibleString.ToString(); }
        }

        public static void UpdateContactInfo(Dictionary<string, object> Data)
        {
            var FilteredData = Data.Where(elem => ContactShowableFields.Contains(elem.Key) && elem.Key != "id");

            string QueryText = "update ContactsInfo set " + String.Join(", ",
                FilteredData.Select(elem => elem.Key + " = " + QuoteIfString(elem.Value))) +
                " where id = " + Data["id"].ToString();

            var conn = NewConnection();
            conn.Open();

            new SqlCommand(QueryText, conn).ExecuteNonQuery();
        }

        //public static void 

        public static IEnumerable<Dictionary<string, object>> AllContacts()
        {
            var conn = NewConnection();
            string GetterCommand = "select " + String.Join(", ", ContactMainFields) + " from ContactsInfo";
            var command = new SqlCommand(GetterCommand, conn);
            conn.Open();
            var Selection = command.ExecuteReader();

            var result = new List<Dictionary<string, object>>();

            while (Selection.Read())
            {
                var Dict = new Dictionary<string, object>();
                foreach (string Field in ContactMainFields)
                {
                    Dict.Add(Field, Selection[Field]);
                }
                result.Add(Dict);
            }
            return result;
        }

        public static object ContactInfo(int id)
        {
            var conn = NewConnection();
            string GetterQueryText = "select * from ContactsInfo where id = " + id.ToString();

            var command = new SqlCommand(GetterQueryText, conn);
            conn.Open();
            var Selection = command.ExecuteReader();

            object res;

            if (Selection.Read())
            {
                res = Selection.ToDictionary();
            }
            else
            {
                res = null;
            }
            conn.Close();
            return res;

        }

        public static void DeleteContact(int id)
        {
            string QueryText = "Delete from ContactsInfo where id = " + id.ToString();
            var conn = NewConnection();
            conn.Open();
            new SqlCommand(QueryText, conn).ExecuteNonQuery();
            conn.Close();
        }
    }

    public static class SelectionExtension
    {
        public static Dictionary<string, object> ToDictionary(this SqlDataReader Selection)
        {
            var Result = new Dictionary<string, object>();
            for (int i = 0; i < Selection.VisibleFieldCount; i++)
            {
                Result.Add(Selection.GetName(i), Selection.GetValue(i));
            }
            return Result;
        }
    }
}
