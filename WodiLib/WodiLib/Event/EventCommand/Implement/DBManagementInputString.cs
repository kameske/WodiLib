// ========================================
// Project Name : WodiLib
// File Name    : DBManagementInputString.cs
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
    /// イベントコマンド・DB操作（文字入力）
    /// </summary>
    [Serializable]
    public class DBManagementInputString : DBManagementInputBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "{0}  {1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBStringAssignmentOperator AssignmentOperator
        {
            get => DBStringAssignmentOperator.FromByte((byte) (NumberAssignOperationCode + LeftSideCode));
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBStringAssignmentOperator)));
                NumberAssignOperationCode = (byte) (value.Code & 0xF0);
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(RightSide)));
                rightSide = value;
            }
        }

        /// <inheritdoc />
        /// <summary>入出力値または代入先</summary>
        protected sealed override int _NumValue
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        /// <summary>代入文字列またはCSVファイル名</summary>
        protected sealed override string _StrValue { get; set; }

        /// <inheritdoc />
        /// <summary>代入演算子コード</summary>
        protected sealed override byte NumberAssignOperationCode { get; set; }

        /// <inheritdoc />
        /// <summary>右辺内容コード</summary>
        protected sealed override byte LeftSideCode
        {
            get => 2;
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
            return string.Format(EventCommandSentenceFormat,
                AssignmentOperator.EventCommandSentence, StrValue);
        }
    }
}