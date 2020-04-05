// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusPicture.cs
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
    /// イベントコマンド・変数操作+（ピクチャ番号）
    /// </summary>
    [Serializable]
    public class SetVariablePlusPicture : SetVariablePlusBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ﾋﾟｸﾁｬ:{0} の {1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x05;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int pictureNumber;

        /// <summary>ピクチャ番号</summary>
        public int PictureNumber
        {
            get => pictureNumber;
            set
            {
                pictureNumber = value;
                NotifyPropertyChanged();
            }
        }

        private NumberPlusPictureInfoType infoType = NumberPlusPictureInfoType.PositionX;

        /// <summary>[NotNull] 取得情報種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberPlusPictureInfoType InfoType
        {
            get => infoType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(InfoType)));
                infoType = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
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
            get => EventCommandConstant.SetVariablePlus.ExecCode.Picture;
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
            get => PictureNumber;
            set => PictureNumber = value;
        }

        /// <summary>取得情報コード値</summary>
        protected override int TargetDetailCode
        {
            get => InfoType.Code;
            set => InfoType = NumberPlusPictureInfoType.FromValue(value);
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
            var picNumStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PictureNumber, type, desc);

            return string.Format(EventCommandSentenceFormat,
                picNumStr, InfoType.EventCommandSentence);
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
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_00();
            }

            if (VersionConfig.IsGreaterVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_GreaterVer2_00();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.00未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_00()
        {
            if (TargetDetailCode == NumberPlusPictureInfoType.ZoomWidth.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.ZoomWidth}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.ZoomWidth.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.ZoomHeight}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeLeftUpX.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeLeftUpX}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeLeftUpY.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeLeftUpY}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeLeftDownX.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeLeftDownX}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeLeftDownY.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeLeftDownY}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeRightDownX.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeRightDownX}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeRightDownY.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeRightDownY}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeRightDownX.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeRightDownX}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (TargetDetailCode == NumberPlusPictureInfoType.FreeModeRightDownY.Code)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.FreeModeRightDownY}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }
        }

        private void OutputVersionWarningLogIfNeed_GreaterVer2_00()
        {
            if (TargetDetailCode == NumberPlusPictureInfoType.Zoom.Code)
            {
                Logger.Warning(VersionWarningMessage.NotGreaterInCommandSetting(
                    $"{nameof(SetVariablePlusPicture)}.{nameof(TargetDetailCode)}",
                    $"{NumberPlusPictureInfoType.Zoom}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }
        }
    }
}