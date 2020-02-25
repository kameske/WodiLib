// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusEtc.cs
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
    /// イベントコマンド・変数操作+（その他）
    /// </summary>
    [Serializable]
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
                if (value is null)
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            return InfoType.EventCommandSentence;
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
        }

        /// <summary>
        /// 設定バージョン = 2.00未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_00()
        {
            if (InfoType == NumberPlusEtcInfoType.ActiveEventId)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusEtc)}.{nameof(InfoType)}",
                    $"{NumberPlusEtcInfoType.ActiveEventId}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }

            if (InfoType == NumberPlusEtcInfoType.ActiveEventLine)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusEtc)}.{nameof(InfoType)}",
                    $"{NumberPlusEtcInfoType.ActiveEventLine}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }
        }
    }
}