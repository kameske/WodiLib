// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusChara.cs
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
    /// イベントコマンド・変数操作+（キャラ）
    /// </summary>
    [Serializable]
    public class SetVariablePlusChara : SetVariablePlusBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "{0} の {1}";

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
                if (value is null)
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var charaStr = resolver.GetMapEventIdStr(EventId, type, desc).Item2;

            return string.Format(EventCommandSentenceFormat,
                charaStr, InfoType.EventCommandSentence);
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
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_10))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_10();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.10未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_10()
        {
            if (InfoType == NumberPlusCharaInfoType.AnimationPattern)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusChara)}.{nameof(InfoType)}",
                    $"{NumberPlusCharaInfoType.AnimationPattern}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_10));
            }

            if (InfoType == NumberPlusCharaInfoType.IsMoving)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetVariablePlusChara)}.{nameof(InfoType)}",
                    $"{NumberPlusCharaInfoType.IsMoving}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_10));
            }
        }
    }
}