// ========================================
// Project Name : WodiLib
// File Name    : DBManagementClearBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（初期化系処理ベース）
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DBManagementClearBase : DBManagementBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public sealed override EventCommandCode EventCommandCode => EventCommandCode.DBManagement;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>入出力値または代入先</summary>
        protected sealed override int NumValue
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        /// <summary>代入文字列またはCSVファイル名</summary>
        protected sealed override string StrValue
        {
            get => "";
            set { }
        }

        /// <inheritdoc />
        /// <summary>右辺内容コード</summary>
        protected sealed override byte RightSideCode { get; set; }

        /// <inheritdoc />
        /// <summary>読み書きモード</summary>
        protected sealed override byte ioMode { get; set; }
            = EventCommandConstant.DBManagement.IoMode.Write;

        /// <inheritdoc />
        /// <summary>代入演算子コード</summary>
        protected sealed override byte NumberAssignOperationCode { get; set; }
    }
}