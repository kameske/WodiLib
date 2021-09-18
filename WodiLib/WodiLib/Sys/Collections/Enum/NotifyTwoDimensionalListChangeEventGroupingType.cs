// ========================================
// Project Name : WodiLib
// File Name    : NotifyTwoDimensionalListChangeEventGroupingType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト変更通知時の要素グルーピング種別
    /// </summary>
    public class
        NotifyTwoDimensionalListChangeEventGroupingType : TypeSafeEnum<NotifyTwoDimensionalListChangeEventGroupingType>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Types
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>個別</summary>
        /// <remarks>
        ///     この値が設定されている場合、1要素ごとに1つのイベントが発生する。
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventGroupingType None;

        /// <summary>行</summary>
        /// <remarks>
        ///     この値が設定されている場合、操作した行ごとにイベントが発生する。
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventGroupingType Row;

        /// <summary>列</summary>
        /// <remarks>
        ///     この値が設定されている場合、操作した列ごとにイベントが発生する。
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventGroupingType Column;

        /// <summary>操作方向依存</summary>
        /// <remarks>
        ///     この値が設定されている場合、操作した行または列ごとにイベントが発生する。
        ///     どちらのパターンで発生するかは行った操作に依存する。
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventGroupingType Direct;

        /// <summary>一括</summary>
        /// <remarks>
        ///     この値が設定されている場合、すべての要素が含まれたイベントが1度だけ発生する。
        /// </remarks>
        public static readonly NotifyTwoDimensionalListChangeEventGroupingType All;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Static Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static NotifyTwoDimensionalListChangeEventGroupingType()
        {
            None = new NotifyTwoDimensionalListChangeEventGroupingType("None", null);
            Row = new NotifyTwoDimensionalListChangeEventGroupingType("Row", Direction.Row);
            Column = new NotifyTwoDimensionalListChangeEventGroupingType("Column", Direction.Column);
            Direct = new NotifyTwoDimensionalListChangeEventGroupingType("Direct", null);
            All = new NotifyTwoDimensionalListChangeEventGroupingType("All", Direction.None);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     通知方向
        /// </summary>
        /// <remarks>
        ///     通知方向が処理依存の場合 <see langword="null"/> が格納される。
        ///     一つの通知引数に単一要素を格納する場合、 <see cref="WodiLib.Sys.Collections.Direction.None"/> が格納される。
        /// </remarks>
        public Direction? Direction { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private NotifyTwoDimensionalListChangeEventGroupingType(string id, Direction? direction) : base(id)
        {
            Direction = direction;
        }
    }
}
