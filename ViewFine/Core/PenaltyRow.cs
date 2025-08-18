namespace ViewFine.Core
{
    public record PenaltyRow(
        string category,     // 취소 / 정지(가)
        string item_no,
        string label,
        string law,
        int? points,
        string revised_date
    );
}
