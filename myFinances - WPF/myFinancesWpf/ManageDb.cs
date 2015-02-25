using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myFinancesWpf
{
    class ManageDb
    {
        public static bool CheckConnectionDb()
        {
            // Изначально считаем что все настройки указаны корректно
            var flag = true;

            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;
            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM nsi_bill ";
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                flag = true;
                dataReader.Close();
                conn.Close();
            }
            catch (MySqlException)
            {
                flag = false;
            }

            return flag;
        }

        public static bool IsBillExist(int idBill)
        {
            var result = false;
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;
            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM  nsi_bill  WHERE Id = '" + idBill + "'";
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                if (dataReader.Read()) result = true;
                dataReader.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Произошла ошибка при подключении к nsi_bill : " + ex.ToString());
            }
            return result;
        }
    }
}
