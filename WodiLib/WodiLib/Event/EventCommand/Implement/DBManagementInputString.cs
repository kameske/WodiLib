// ========================================
// Project Name : WodiLib
// File Name    : DBManagementInputString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（文字入力）
    /// </summary>
    public class DBManagementInputString : DBManagementBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.DBManagement;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>DB種別</summary>
        protected override DBKind _DBKind
        {
            get => DBKind.Changeable;
            set { }
        }

        /// <summary>[NotNull] タイプID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBTypeId
        {
            get => _IsTypeIdUseStr ? (IntOrStr) _DBTypeId.ToStr() : _DBTypeId.ToInt();
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                _DBTypeId.Merge(value);
            }
        }

        /// <summary>[NotNull] データID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBDataId
        {
            get => _IsDataIdUseStr ? (IntOrStr) _DBDataId.ToStr() : _DBDataId.ToInt();
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBDataId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBDataId)));
                _DBDataId.Merge(value);
            }
        }

        /// <summary>[NotNull] 項目ID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBItemId
        {
            get => _IsItemIdUseStr ? (IntOrStr) _DBItemId.ToStr() : _DBItemId.ToInt();
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBItemId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBItemId)));
                _DBItemId.Merge(value);
            }
        }

        private DBStringAssignmentOperator assignmentOperator = DBStringAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBStringAssignmentOperator AssignmentOperator
        {
            get => assignmentOperator;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBStringAssignmentOperator)));
                assignmentOperator = value;
            }
        }

        private string rightSide = "";

        /// <summary>[NotNull] 右辺</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string RightSide
        {
            get => rightSide;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(RightSide)));
                rightSide = value;
            }
        }

        /// <inheritdoc />
        /// <summary>入出力値または代入先</summary>
        protected sealed override int NumValue
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        /// <summary>代入文字列またはCSVファイル名</summary>
        protected sealed override string StrValue { get; set; }

        /// <inheritdoc />
        /// <summary>右辺内容コード</summary>
        protected sealed override byte RightSideCode
        {
            get => AssignmentOperator.Code;
            set => AssignmentOperator = DBStringAssignmentOperator.FromByte(value);
        }

        /// <inheritdoc />
        /// <summary>読み書きモード</summary>
        protected sealed override byte ioMode
        {
            get => 0x00;
            set { }
        }

        /// <inheritdoc />
        /// <summary>代入演算子コード</summary>
        protected sealed override byte NumberAssignOperationCode
        {
            get => 0;
            set { }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        public bool IsTypeIdUseStr
        {
            get => _IsTypeIdUseStr;
            set => _IsTypeIdUseStr = value;
        }

        /// <summary>データID文字列指定フラグ</summary>
        public bool IsDataIdUseStr
        {
            get => _IsDataIdUseStr;
            set => _IsDataIdUseStr = value;
        }

        /// <summary>項目ID文字列指定フラグ</summary>
        public bool IsItemIdUseStr
        {
            get => _IsItemIdUseStr;
            set => _IsItemIdUseStr = value;
        }
    }
}