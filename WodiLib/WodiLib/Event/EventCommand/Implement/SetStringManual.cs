// ========================================
// Project Name : WodiLib
// File Name    : SetStringManual.cs
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
    /// イベントコマンド・文字列操作（手動入力）
    /// </summary>
    [Serializable]
    public class SetStringManual : SetStringBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "\"{0}\"";

        private const string EventCommandSentenceFormat_Replace = "\"{0}\" → \"{1}\"";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount =>
            (byte) (AssignmentOperator == StringAssignmentOperator.Replace
                ? 0x02
                : 0x01);

        /// <inheritdoc />
        /// <summary>文字列変数最小個数</summary>
        public override byte StringVariableCountMin => 0x01;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private string rightSide = "";

        /// <summary>[NotNull] 右辺（文字列）</summary>
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

        /// <summary>[NotNull] 右辺置換先文字列</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public string RightSideReplaceNewStr
        {
            get
            {
                if (AssignmentOperator == StringAssignmentOperator.Replace) return RightSideReplaceString;
                return "";
            }
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(RightSideReplaceNewStr));
                RightSideReplaceString = value;
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (AssignmentOperator == StringAssignmentOperator.Replace)
            {
                return string.Format(EventCommandSentenceFormat_Replace,
                    RightSideString, RightSideReplaceNewStr);
            }

            return string.Format(EventCommandSentenceFormat, RightSideString);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>右辺設定コード値</summary>
        protected override byte RightSidePropertyCode
            => EventCommandConstant.SetString.RightSidePropertyCode.Manual;

        /// <inheritdoc />
        /// <summary>右辺特殊設定コード値</summary>
        protected override byte RightSideSpecialSettingsCode
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        /// <summary>右辺オプション</summary>
        protected override int RightSideOption
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        /// <summary>右辺文字列</summary>
        protected override string RightSideString
        {
            get => RightSide;
            set => RightSide = value;
        }

        /// <inheritdoc />
        /// <summary>右辺置換文字列</summary>
        protected override string RightSideReplaceString { get; set; } = "";
    }
}