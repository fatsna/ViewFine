using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewFine.Core;

namespace ViewFine.ViewModels
{
    // UI 전용 래퍼 (행 단위)
    public partial class PenaltyItem : ObservableObject
    {
        public PenaltyRow Row { get; }
        public PenaltyItem(PenaltyRow row) => Row = row;

        public string ItemNo => Row.item_no;
        public string Label => Row.label;
        public string Law => Row.law;
        public string Category => Row.category;
        public int? Points => Row.points;

        [ObservableProperty] private bool isExpanded; // ← 펼침/접힘(UI 상태)
    }

}
