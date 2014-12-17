using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace myFinances
{
    class LoadDataDB
    {
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
            // Неправильная сортировка
            // listStructure.Sort((x, y) => x.Name.CompareTo(y.Name));
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
                        Comment = dataReader["Name"] == DBNull.Value ? string.Empty : (string) dataReader["Name"],
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

        public static int GetIdBillbyName(string billName)
        {
            // Если запись есть в БД - вернет её Id, иначе вернет -1
            var idBill = -1;
            var connString = "SERVER=" + Globals.ServerName + "; PORT=" + Globals.ServerPort.ToString() + "; DATABASE=" + Globals.DbName +
                             "; UID=" + Globals.DbUserName + "; PWD=" + Globals.DbUserPassword;

            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                var query = "SELECT * FROM nsi_bill WHERE nsi_bill.Name = '" + billName + "'";
                var command = new MySqlCommand(query) { Connection = conn };
                var dataReader = command.ExecuteReader();
                if (dataReader.Read())
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
                    if ((int)dataReader["Bill_Id"] == 0 || (int)dataReader["Bill_Id"] == idBill)
                    {
                        var structure = new StructureDto()
                        {
                            Id = (int)dataReader["Id"],
                            ParentId = (int)dataReader["Parent_Id"],
                            Name = (string)dataReader["Name"],
                            Comment = dataReader["Comment"] == DBNull.Value ? string.Empty : (string)dataReader["Comment"],
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

        public static int GetIdOperationbyName(string nameOperation, int idBill, string typeOperation)
        {
            var nameTable = string.Empty;
            if (typeOperation.Equals("Добавить доход")) nameTable = "nsi_income_structure";
            else if (typeOperation.Equals("Отметить расход")) nameTable = "nsi_expence_structure";
            else return -1;


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
                    if ((int)dataReader["Bill_Id"] == 0 || (int)dataReader["Bill_Id"] == idBill) 
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
    }
}
