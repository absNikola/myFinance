using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace myFinances
{
    class SaveDataDB
    {
        public static string SaveBilltoDb(BillDto bill)
        {
            var result = "Success";
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO nsi_bill (Name, Comment) VALUES(?name, ?comment)";
                command.Parameters.Add("?name", MySqlDbType.VarChar).Value = bill.Name;
                command.Parameters.Add("?comment", MySqlDbType.VarChar).Value = bill.Comment;
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                result = "Произошла ошибка при подключении к nsi_bill : " + ex.ToString();
            }

            return result;
        }

        public static string SaveOperationtoDb(OperationDto operation, string nameOperation)
        {
            var nameTable = string.Empty;
            if (nameOperation.Equals("Добавить доход")) nameTable = "income";
            else if (nameOperation.Equals("Отметить расход")) nameTable = "expence";
                 else return "Не определен тип операции";

            var result = "Success";
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO " + nameTable + " (Bill_Id, Amount, Date, Comment) VALUES(?billId, ?amount, ?date, ?comment)";
                command.Parameters.Add("?billId", MySqlDbType.Int32).Value = operation.IdBill;
                command.Parameters.Add("?amount", MySqlDbType.Int64).Value = operation.Amount;
                command.Parameters.Add("?date", MySqlDbType.DateTime).Value = operation.Date;
                command.Parameters.Add("?comment", MySqlDbType.VarChar).Value = operation.Comment;
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                result = "Произошла ошибка при подключении к income : " + ex.ToString();
            }

            return result;
        }
    }
}
