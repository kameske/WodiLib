// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・変数操作+（位置）
    /// </summary>
    [Serializable]
    public class SetVariablePlusPosition : SetVariablePlusBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x05;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>X座標</summary>
        public int PositionX { get; set; }

        /// <summary>Y座標</summary>
        public int PositionY { get; set; }

        /// <summary>精密座標フラグ</summary>
        public bool IsPrecise { get; set; }

        private NumberPlusPositionInfoType infoType = NumberPlusPositionInfoType.EventId;

        /// <summary>[NotNull] 取得情報種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberPlusPositionInfoType InfoType
        {
            get => infoType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(InfoType)));
                infoType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>取得情報オプションフラグ</summary>
        protected override byte InfoOptionFlag
        {
            get => (byte) (IsPrecise ? FlgPrecise : 0x00);
            set => IsPrecise = (value & FlgPrecise) != 0;
        }

        /// <summary>取得項目コード値</summary>
        protected override byte ExecCode
        {
            get => EventCommandConstant.SetVariablePlus.ExecCode.Position;
            set { }
        }

        /// <summary>取得情報種別コード値</summary>
        protected override byte InfoTypeCode
        {
            get => InfoType.Code;
            set => InfoType = NumberPlusPositionInfoType.FromByte(value);
        }

        /// <summary>取得項目コード値</summary>
        protected override int TargetCode
        {
            get => PositionX;
            set => PositionX = value;
        }

        /// <summary>取得情報コード値</summary>
        protected override int TargetDetailCode
        {
            get => PositionY;
            set => PositionY = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgPrecise = 0x20;

        private const string EventCommandSentenceFormat = "X:{0} Y:{1}の{2}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var xStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PositionX, type, desc);
            var yStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PositionY, type, desc);

            return string.Format(EventCommandSentenceFormat,
                xStr, yStr, InfoType.EventCommandSentence);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     VersionCheck
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public override void OutputVersionWarningLogIfNeed()
        {
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver1_30))
            {
                OutputVersionWarningLogIfNeed_UnderVer1_30();
            }
        }

        /// <summary>
        /// 設定バージョン = 1.30未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer1_30()
        {
            if (InfoTypeCode == NumberPlusPositionInfoType.Layer1ChipNumber.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPosition)}.{nameof(InfoTypeCode)}",
                    $"{nameof(NumberPlusPositionInfoType.Layer1ChipNumber)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver1_30));
            }

            if (InfoTypeCode == NumberPlusPositionInfoType.Layer2ChipNumber.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPosition)}.{nameof(InfoTypeCode)}",
                    $"{nameof(NumberPlusPositionInfoType.Layer2ChipNumber)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver1_30));
            }

            if (InfoTypeCode == NumberPlusPositionInfoType.Layer3ChipNumber.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPosition)}.{nameof(InfoTypeCode)}",
                    $"{nameof(NumberPlusPositionInfoType.Layer3ChipNumber)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver1_30));
            }

            if (InfoTypeCode == NumberPlusPositionInfoType.Layer1TileTag.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPosition)}.{nameof(InfoTypeCode)}",
                    $"{nameof(NumberPlusPositionInfoType.Layer1TileTag)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver1_30));
            }

            if (InfoTypeCode == NumberPlusPositionInfoType.Layer2TileTag.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPosition)}.{nameof(InfoTypeCode)}",
                    $"{nameof(NumberPlusPositionInfoType.Layer2TileTag)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver1_30));
            }

            if (InfoTypeCode == NumberPlusPositionInfoType.Layer3TileTag.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPosition)}.{nameof(InfoTypeCode)}",
                    $"{nameof(NumberPlusPositionInfoType.Layer3TileTag)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver1_30));
            }
        }
    }
}