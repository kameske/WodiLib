// ========================================
// Project Name : WodiLib
// File Name    : SetStringStringVar.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・文字列操作（文字列変数）
    /// </summary>
    [Serializable]
    public class SetStringStringVar : SetStringBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "{0}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x04;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int rightSide;

        /// <summary>右辺（文字列変数アドレス）</summary>
        public int RightSide
        {
            get => rightSide;
            set
            {
                rightSide = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>右辺設定コード値</summary>
        protected override byte RightSidePropertyCode
            => EventCommandConstant.SetString.RightSidePropertyCode.StringVar;

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
            get => RightSide;
            set => RightSide = value;
        }

        /// <inheritdoc />
        /// <summary>右辺文字列</summary>
        protected override string RightSideString
        {
            get => "";
            set { }
        }

        /// <inheritdoc />
        /// <summary>右辺置換文字列</summary>
        protected override string RightSideReplaceString
        {
            get => "";
            set { }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var rightSideStr = resolver.GetStringVariableAddressString(RightSide, type, desc);


            return string.Format(EventCommandSentenceFormat, rightSideStr);
        }
    }
}