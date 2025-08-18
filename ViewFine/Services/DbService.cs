using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ViewFine.Core;

namespace ViewFine.Services
{
    public sealed class DbService : IDbService, IAsyncDisposable
    {
        private readonly string _dbPath;
        private readonly SqliteConnection _conn;

        public DbService(string? dbPath = null)
        {
            _dbPath = dbPath ?? Path.Combine(AppContext.BaseDirectory, "Data", "law_portable.db");
            _conn = new SqliteConnection($"Data Source={_dbPath};Cache=Shared;Pooling=True;Mode=ReadWrite");
        }

        public async Task OpenAsync()
        {
            if (_conn.State != ConnectionState.Open)
            {
                await _conn.OpenAsync();
                using var cmd = _conn.CreateCommand();
                cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL; PRAGMA foreign_keys=ON;";
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<(int cancel, int suspend)> GetCountsAsync()
        {
            await OpenAsync();
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"SELECT (SELECT COUNT(*) FROM byeol28_cancel),
                                       (SELECT COUNT(*) FROM byeol28_suspendA);";
            using var rd = await cmd.ExecuteReaderAsync();
            rd.Read();
            return (rd.GetInt32(0), rd.GetInt32(1));
        }

        public async Task<DataTable> GetCancelAsync()
        {
            await OpenAsync();
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                SELECT item_no, item_label_full, law_ref_short, notes, revised_date
                FROM byeol28_cancel
                ORDER BY CAST(REPLACE(item_no,'의','.') AS REAL), item_no;";
            using var rd = await cmd.ExecuteReaderAsync();
            var dt = new DataTable(); dt.Load(rd); return dt;
        }

        public async Task<DataTable> GetSuspendAAsync()
        {
            await OpenAsync();
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                SELECT item_no, item_label_full, law_ref_short, points, revised_date
                FROM byeol28_suspendA
                ORDER BY CAST(REPLACE(item_no,'의','.') AS REAL), item_no;";
            using var rd = await cmd.ExecuteReaderAsync();
            var dt = new DataTable(); dt.Load(rd); return dt;
        }

        public async Task<List<PenaltyRow>> GetMergedPenaltiesAsync(string? search = null)
        {
            await OpenAsync();
            var list = new List<PenaltyRow>();

            // 취소(벌점 없음)
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT '취소' AS category, item_no, item_label_full, law_ref_short, NULL as points, revised_date
                    FROM byeol28_cancel";
                if (!string.IsNullOrWhiteSpace(search))
                    cmd.CommandText += " WHERE item_no LIKE @q OR item_label_full LIKE @q OR law_ref_short LIKE @q OR notes LIKE @q";
                cmd.CommandText += " ORDER BY CAST(REPLACE(item_no,'의','.') AS REAL), item_no;";
                if (!string.IsNullOrWhiteSpace(search))
                    cmd.Parameters.AddWithValue("@q", $"%{search}%");

                using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    list.Add(new PenaltyRow(
                        category: rd.GetString(0),
                        item_no: rd.GetString(1),
                        label: rd.GetString(2),
                        law: rd.IsDBNull(3) ? "" : rd.GetString(3),
                        points: null,
                        revised_date: rd.GetString(5)
                    ));
                }
            }

            // 정지(가)
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT '정지(가)' AS category, item_no, item_label_full, law_ref_short, points, revised_date
                    FROM byeol28_suspendA";
                if (!string.IsNullOrWhiteSpace(search))
                    cmd.CommandText += " WHERE item_no LIKE @q OR item_label_full LIKE @q OR law_ref_short LIKE @q";
                cmd.CommandText += " ORDER BY CAST(REPLACE(item_no,'의','.') AS REAL), item_no;";
                if (!string.IsNullOrWhiteSpace(search))
                    cmd.Parameters.AddWithValue("@q", $"%{search}%");

                using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    list.Add(new PenaltyRow(
                        category: rd.GetString(0),
                        item_no: rd.GetString(1),
                        label: rd.GetString(2),
                        law: rd.IsDBNull(3) ? "" : rd.GetString(3),
                        points: rd.IsDBNull(4) ? null : rd.GetInt32(4),
                        revised_date: rd.GetString(5)
                    ));
                }
            }

            return list;
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Yield();
            _conn.Dispose();
        }
    }
}
