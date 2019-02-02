// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusEtc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・変数操作+（その他）
    /// </summary>
    public class SetVariablePlusEtc : SetVariablePlusBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x04;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private NumberPlusEtcInfoType infoType = NumberPlusEtcInfoType.CurrentMapId;

        /// <summary>[NotNull] 取得情報種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberPlusEtcInfoType InfoType
        {
            get => infoType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(InfoType)));
                infoType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>取得情報オプションフラグ</summary>
        protected override byte InfoOptionFlag
        {
            get => 0x00;
            set { }
        }

        /// <summary>取得項目コード値</summary>
        protected override byte ExecCode
        {
            get => EventCommandConstant.SetVariablePlus.ExecCode.Etc;
            set { }
        }

        /// <summary>取得情報種別コード値</summary>
        protected override byte InfoTypeCode
        {
            get => 0x00;
            set { }
        }

        /// <summary>取得項目コード値</summary>
        protected override int TargetCode
        {
            get => InfoType.Code;
            set => InfoType = NumberPlusEtcInfoType.FromValue(value);
        }

        /// <summary>取得情報コード値</summary>
        protected override int TargetDetailCode
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}