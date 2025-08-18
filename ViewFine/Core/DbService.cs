
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace ViewFine.Core
{
    public sealed class DbService : IAsyncDisposable
    {
        private readonly string _dbPath;
        private readonly SqliteConnection _conn;

        public DbService(string? dbPath = null)
        {
            // Default: ./Data/law_portable.db next to the exe
            _dbPath = dbPath ?? Path.Combine(AppContext.BaseDirectory, "Data", "law_portable.db");
            _conn = new SqliteConnection($"Data Source={_dbPath};Cache=Shared;Pooling=True;Mode=ReadWrite");
        }

        public async Task OpenAsync()
        {
            if (_conn.State != ConnectionState.Open)
            {
                await _conn.OpenAsync();
                // Recommended pragmas for desktop apps
                using var cmd = _conn.CreateCommand();
                cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL; PRAGMA foreign_keys=ON;";
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<(int cancel, int suspend)> GetCountsAsync()
        {
            await OpenAsync();
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                SELECT
                  (SELECT COUNT(*) FROM byeol28_cancel),
                  (SELECT COUNT(*) FROM byeol28_suspendA);
            ";
            using var rd = await cmd.ExecuteReaderAsync();
            rd.Read();
            int cancel = rd.GetInt32(0);
            int suspend = rd.GetInt32(1);
            return (cancel, suspend);
        }

        public async Task<DataTable> GetCancelAsync()
        {
            await OpenAsync();
            using var cmd = _conn.CreateCommand();
            // '3의2' -> 3.2 로 변환해 정렬
            cmd.CommandText = @"
                SELECT item_no, item_label_full, law_ref_short, notes, revised_date
                FROM byeol28_cancel
                ORDER BY CAST(REPLACE(item_no,'의','.') AS REAL), item_no;
            ";
            using var rd = await cmd.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(rd);
            return dt;
        }

        public async Task<DataTable> GetSuspendAAsync()
        {
            await OpenAsync();
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                SELECT item_no, item_label_full, law_ref_short, points, revised_date
                FROM byeol28_suspendA
                ORDER BY CAST(REPLACE(item_no,'의','.') AS REAL), item_no;
            ";
            using var rd = await cmd.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(rd);
            return dt;
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Yield();
            _conn.Dispose();
        }
    }
}
