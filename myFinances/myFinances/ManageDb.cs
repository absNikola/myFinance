using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace myFinances
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

        #region Save Data
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
        #endregion

        #region Load Data - Get full Lists
        public static List<StructureDto> FindChildrenAtListOperation(StructureDto parent, List<StructureDto> listStructure, string prefix)
        {
            var listOfChildren = new List<StructureDto>();
            foreach (var element in listStructure)
                if (element.ParentId.Equals(parent.Id) && 
                    !element.ParentId.Equals(element.Id))
                {
                    element.Name = prefix + element.Name;
                    listOfChildren.Add(element);
                    listOfChildren.AddRange(FindChildrenAtListOperation(element, listStructure, prefix + "   "));
                }
            return listOfChildren;
        }

        public static List<StructureDto> SortingAtListOperation(List<StructureDto> listIn)
        {
            // Неправильная сортировка, поскольку нужно не по алфавиту, а по структуре
            // listStructure.Sort((x, y) => x.Name.CompareTo(y.Name));
            // А теперь правильная:
            var listOut = new List<StructureDto>();
            foreach (var element in listIn)
                if (element.Id.Equals(element.ParentId))
                {
                    // для данного элемента должны найти всех его детей
                    listOut.Add(element);
                    listOut.AddRange(FindChildrenAtListOperation(element, listIn, "   "));
                }

            return listOut;
        }

        public static List<StructureDto> GetListOperation(int idBill, string typeOperation)
        {
            var nameTable = string.Empty;
            if (typeOperation.Equals("Добавить доход")) nameTable = "nsi_income_structure";
            else if (typeOperation.Equals("Отметить расход")) nameTable = "nsi_expence_structure";
            else return null;

            var listStructure = new List<StructureDto>();
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM " + nameTable;
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    if ((int) dataReader["Bill_Id"] == 0 || (int) dataReader["Bill_Id"] == idBill || idBill == -1)
                    {
                        var structure = new StructureDto()
                        {
                            Id = (int) dataReader["Id"],
                            ParentId = (int) dataReader["Parent_Id"],
                            Name = (string) dataReader["Name"],
                            Comment =
                                dataReader["Comment"] == DBNull.Value ? string.Empty : (string) dataReader["Comment"],
                        };
                        listStructure.Add(structure);
                    }
                }
                dataReader.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Произошла ошибка при подключении к " + nameTable + " : " + ex.ToString());
            }

            // и еще необходимо отсортировать по правильным вложенным подкатегориям
            listStructure = SortingAtListOperation(listStructure);

            // сказали ответ
            return listStructure;
        }

        public static List<BillDto> GetListBill()
        {
            var listBill = new List<BillDto>();
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM nsi_bill ";
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var bill = new BillDto()
                    {
                        Id = (int)dataReader["Id"],
                        Name = (string)dataReader["Name"],
                        Comment = dataReader["Name"] == DBNull.Value ? string.Empty : (string)dataReader["Name"],
                    };
                    listBill.Add(bill);
                }
                dataReader.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Произошла ошибка при подключении к nsi_bill : " + ex.ToString());
            }

            return listBill;
        }
        #endregion

        #region Load Data - Find record by Parameters
        public static int GetIdBillbyName(string nameBill)
        {
            // Если запись есть в БД - вернет её Id, иначе вернет -1
            var idBill = -1;
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM  nsi_bill  WHERE Name = '" + nameBill + "'";
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    idBill = (int)dataReader["Id"];
                }
                dataReader.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Произошла ошибка при подключении к nsi_bill : " + ex.ToString());
            }

            return idBill;
        }

        public static int GetIdOperationbyName(string nameOperation, string typeOperation)
        {
            var nameTable = string.Empty;
            if (typeOperation.Equals("Добавить доход")) nameTable = "nsi_income_structure";
            else if (typeOperation.Equals("Отметить расход")) nameTable = "nsi_expence_structure";
            else return -1;


            while (nameOperation[0] == ' ')
            nameOperation = nameOperation.Substring(1, nameOperation.Length - 1);
            // Если запись есть в БД - вернет её Id, иначе вернет -1
            var idOperation = -1;
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM " + nameTable + " WHERE " + nameTable + ".Name = '" + nameOperation + "'";
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idOperation = (int)dataReader["Id"];
                }
                dataReader.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Произошла ошибка при подключении к " + nameTable + " : " + ex.ToString());
            }

            return idOperation;
        }
        #endregion
    }
}
