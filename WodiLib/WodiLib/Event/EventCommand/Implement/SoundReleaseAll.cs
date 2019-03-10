// ========================================
// Project Name : WodiLib
// File Name    : SoundReleaseAll.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・サウンド（メモリ全解放）
    /// </summary>
    public class SoundReleaseAll : SoundBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.SoundRelease;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>処理内容コード値</summary>
        protected override ExecType ExecCode => ExecType.ReleaseAll;

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
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_20))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_20();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.20未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_20()
        {
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(SoundReleaseAll)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_20));
        }
    }
}