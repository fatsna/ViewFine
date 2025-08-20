using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewFine.Services;

namespace ViewFine.Services
{
    internal class checkDB
    {
        public static async Task<IReadOnlyList<string>> ListTablesAsync(string connStr)
        {
            var result = new List<string>();

            await using var conn = new SqliteConnection(connStr);
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT name
                FROM sqlite_master
                WHERE type='table' AND name NOT LIKE 'sqlite_%'
                ORDER BY name;";

            await using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
                result.Add(r.GetString(0));

            return result;
        }
        

    }
}
