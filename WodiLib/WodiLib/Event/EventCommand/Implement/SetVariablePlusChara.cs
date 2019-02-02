// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusChara.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・変数操作+（キャラ）
    /// </summary>
    public class SetVariablePlusChara : SetVariablePlusBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x05;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>対象イベントID</summary>
        public int EventId { get; set; }

        private NumberPlusCharaInfoType infoType = NumberPlusCharaInfoType.XPositionStandard;

        /// <summary>[NotNull] 取得情報種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberPlusCharaInfoType InfoType
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
        //     Protected Abstract Property
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
            get => EventCommandConstant.SetVariablePlus.ExecCode.Chara;
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
            get => EventId;
            set => EventId = value;
        }

        /// <summary>取得情報コード値</summary>
        protected override int TargetDetailCode
        {
            get => InfoType.Code;
            set => InfoType = NumberPlusCharaInfoType.FromValue(value);
        }
    }
}