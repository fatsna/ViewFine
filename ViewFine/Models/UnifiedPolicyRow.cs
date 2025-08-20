using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewFine.Models
{
    public record UnifiedPolicyRow(
    string Title,          // 공통: 항목명 (필수)
    string? Law,           // 공통: 법조
    int? Points,           // 정지(A)만 사용
    int? AmountWon,        // 과태료/범칙금 테이블에서 사용
    string? AmountKind,    // ticket/fine 등
    string? VehicleCode,   // 차량구분
    string? Zone,          // 별표7 등 구역
    string? Notes,         // 비고/메모
    string? RevisedDate,   // 개정일
    string Source          // 어느 테이블에서 왔는지 표시용(필터/디버그)
    );

}
