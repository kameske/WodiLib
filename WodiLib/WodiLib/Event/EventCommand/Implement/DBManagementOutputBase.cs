// ========================================
// Project Name : WodiLib
// File Name    : DBManagementOutputBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（読み込み処理ベース）
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DBManagementOutputBase : DBManagementBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public sealed override EventCommandCode EventCommandCode => EventCommandCode.DBManagement;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DBNumberAssignmentOperator assignmentOperator = DBNumberAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBNumberAssignmentOperator AssignmentOperator
        {
            get => assignmentOperator;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AssignmentOperator)));
                assignmentOperator = value;
            }
        }

        /// <summary>左辺（代入先）</summary>
        public int LeftSide { get; set; }

        /// <summary>左辺X番の変数呼び出し</summary>
        public bool IsLeftSideReferX { get; set; }

        /// <inheritdoc />
        /// <summary>入出力値または代入先</summary>
        protected sealed override int _NumValue
        {
            get => LeftSide;
            set => LeftSide = value;
        }

        /// <inheritdoc />
        /// <summary>代入文字列またはCSVファイル名</summary>
        protected sealed override string _StrValue
        {
            get => "";
            set { }
        }

        /// <inheritdoc />
        /// <summary>左辺内容コード</summary>
        protected sealed override byte LeftSideCode
        {
            get => (byte) (IsLeftSideReferX ? 1 : 0);
            set => IsLeftSideReferX = value == 0x01;
        }

        /// <inheritdoc />
        /// <summary>読み書きモード</summary>
        protected sealed override byte ioMode
        {
            get => EventCommandConstant.DBManagement.IoMode.Read;
            set { }
        }

        /// <inheritdoc />
        /// <summary>代入演算子コード</summary>
        protected sealed override byte NumberAssignOperationCode
        {
            get => AssignmentOperator.Code;
            set => AssignmentOperator = DBNumberAssignmentOperator.FromByte(value);
        }
    }
}