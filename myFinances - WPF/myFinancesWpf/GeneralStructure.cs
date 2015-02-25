using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myFinancesWpf
{
    public class GlobalConnections
    {
        public static string ServerName { get; set; }
        public static int ServerPort { get; set; }
        public static string DbName { get; set; }
        public static string DbUserName { get; set; }
        public static string DbUserPassword { get; set; }
    }

    public class GlobalDefaults
    {
        public static int IdBill { get; set; }
        public static List<SettingDto> Bill { get; set; }
    }

    public class SettingDto
    {
        public int IdBill { get; set; }
        public int IdIncomeOperation { get; set; }
        public int IdExpenseOperation { get; set; }
    }

    public class BillDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }

    public class StructureDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int IdBill { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }

    public class OperationDto
    {
        public int OperationId { get; set; }
        public int OperationStructureId { get; set; }
        public long Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; }
    }

    public class ShowedDataDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; }
    }
}
