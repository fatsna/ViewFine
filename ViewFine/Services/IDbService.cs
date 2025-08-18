using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using ViewFine.Core;

namespace ViewFine.Services
{
    public interface IDbService
    {
        Task OpenAsync();
        Task<(int cancel, int suspend)> GetCountsAsync();
        Task<DataTable> GetCancelAsync();
        Task<DataTable> GetSuspendAAsync();

        // 목록 화면에서 사용할 통합 데이터(취소 + 정지(가))
        Task<List<PenaltyRow>> GetMergedPenaltiesAsync(string? search = null);
    }
}
