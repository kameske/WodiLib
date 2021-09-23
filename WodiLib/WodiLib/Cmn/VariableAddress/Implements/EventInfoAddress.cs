// ========================================
// Project Name : WodiLib
// File Name    : EventInfoAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Map;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(9100000, 9179999)] イベント情報アドレス値
    /// </summary>
    [VariableAddress(MinValue = 9100000, MaxValue = 9179999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(EventInfoAddress), typeof(VariableAddress) }
    )]
    public partial class EventInfoAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>取得情報</summary>
        public InfoAddressInfoType InfoType => InfoAddressInfoType.FromCode(RawValue.SubInt(0, 1));

        /// <summary>マップイベントID</summary>
        public MapEventId MapEventId => RawValue.SubInt(1, 4);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        partial void DoConstructorExpansion(int value)
        {
            // 未対応チェック 未対応の場合警告ログ出力
            VersionCheck(value);
        }

        /// <summary>
        ///     バージョンによる定義チェックを行い、警告メッセージを出力する
        /// </summary>
        /// <param name="value">変数アドレス値</param>
        private static void VersionCheck(int value)
        {
            var infoCode = value % 10;

            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_01))
            {
                // 「イベントの座標」のうち、10の位が5または6のアドレスは
                // ウディタVer2.01未満では非対応
                if (infoCode is 5 or 6)
                {
                    WodiLibLogger.GetInstance().Warning(
                        VersionWarningMessage.NotUnderInVariableAddress(
                            value,
                            VersionConfig.GetConfigWoditorVersion(),
                            WoditorVersion.Ver2_01));
                }
            }

            if (infoCode is 7 or 8)
            {
                WodiLibLogger.GetInstance().Warning(
                    VersionWarningMessage.NotUsingVariableAddress(value));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     イベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="type">イベントコマンド種別</param>
        /// <param name="desc">付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string ResolveEventCommandString(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc? desc)
            => InfoType.MakeEventCommandSentenceForMapEvent(MapEventId);
    }
}
