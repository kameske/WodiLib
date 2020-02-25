// ========================================
// Project Name : WodiLib
// File Name    : DBManagementInputNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（数値入力）
    /// </summary>
    [Serializable]
    public class DBManagementInputNumber : DBManagementInputBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "{0}  {1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>右辺</summary>
        public int RightSide { get; set; }

        private DBNumberAssignmentOperator assignmentOperator = DBNumberAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBNumberAssignmentOperator AssignmentOperator
        {
            get => assignmentOperator;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AssignmentOperator)));
                assignmentOperator = value;
            }
        }

        /// <inheritdoc />
        /// <summary>代入演算子コード</summary>
        protected sealed override byte NumberAssignOperationCode
        {
            get => AssignmentOperator.Code;
            set => AssignmentOperator = DBNumberAssignmentOperator.FromByte(value);
        }

        /// <inheritdoc />
        /// <summary>右辺内容コード</summary>
        protected sealed override byte LeftSideCode
        {
            get => (byte) (IsRightSideReferX ? 1 : 0);
            set => IsRightSideReferX = value == 0x01;
        }

        /// <inheritdoc />
        /// <summary>入出力値または代入先</summary>
        protected sealed override int _NumValue
        {
            get => RightSide;
            set => RightSide = value;
        }

        /// <inheritdoc />
        /// <summary>代入文字列またはCSVファイル名</summary>
        protected sealed override string _StrValue
        {
            get => "";
            set { }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var varName = resolver.GetVariableAddressStringIfVariableAddress(RightSide, type, desc);
            if (IsRightSideReferX) varName = $"V[{varName}]";
            return string.Format(EventCommandSentenceFormat,
                AssignmentOperator.EventCommandSentence, varName);
        }
    }
}