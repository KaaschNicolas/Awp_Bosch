using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs
{
    [Keyless]
    public class EvaluationDTO
    {
        public List<Dictionary<string, object>> GenerateDTO(string connectionString, string sqlQuery)
        {
            List<Dictionary<string, object>> dto = new List<Dictionary<string, object>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Spaltennamen und -typen abrufen
                        DataTable schemaTable = reader.GetSchemaTable();
                        List<string> columnNames = new List<string>();
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string columnName = row["ColumnName"].ToString();
                            columnNames.Add(columnName);
                        }

                        // Daten in das DTO übertragen
                        while (reader.Read())
                        {
                            Dictionary<string, object> data = new Dictionary<string, object>();
                            foreach (string columnName in columnNames)
                            {
                                object value = reader[columnName];
                                data[columnName] = value;
                            }
                            dto.Add(data);
                        }
                    }
                }
            }

            return dto;
        }
    }

}
