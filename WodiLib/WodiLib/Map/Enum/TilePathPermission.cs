// ========================================
// Project Name : WodiLib
// File Name    : TilePathPermission.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Map
{
    /// <summary>
    ///     タイル通行許可
    /// </summary>
    public class TilePathPermission : TypeSafeEnum<TilePathPermission>
    {
        /// <summary>通行可</summary>
        public static readonly TilePathPermission Allow;

        /// <summary>通行不可</summary>
        public static readonly TilePathPermission Deny;

        /// <summary>部分的に通行不可</summary>
        public static readonly TilePathPermission PartialDeny;

        /// <summary>下レイヤーに依存</summary>
        public static readonly TilePathPermission Dependent;

        static TilePathPermission()
        {
            Allow = new TilePathPermission(nameof(Allow), 0x00, null, InnerFlagGroup.CannotPassing);
            PartialDeny = new TilePathPermission(nameof(PartialDeny), 0x20, null, InnerFlagGroup.Impassible);
            Dependent = new TilePathPermission(nameof(Dependent), 0x02_00, WoditorVersion.Ver2_00,
                InnerFlagGroup.CannotPassing);
            // 列挙時に他の要素を先に検索させるため、Deny の初期化は意図的に最後に回す
            Deny = new TilePathPermission(nameof(Deny), 0x0F, null, InnerFlagGroup.Not);
        }

        private TilePathPermission(string id, int code, WoditorVersion supportVersion, InnerFlagGroup groupCode) :
            base(id)
        {
            Code = code;
            SupportVersion = supportVersion;
            GroupCode = groupCode;
        }

        /// <summary>
        /// ロガー
        /// </summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>コード値</summary>
        public int Code { get; }

        /// <summary>サポート最小バージョン（nullの場合制限なし）</summary>
        private WoditorVersion SupportVersion { get; }

        /// <summary>内部処理用グループコード</summary>
        internal InnerFlagGroup GroupCode { get; }

        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public void OutputVersionWarningLogIfNeed()
        {
            if (SupportVersion is null) return;

            if (VersionConfig.IsUnderVersion(SupportVersion))
            {
                Logger.Warning(VersionWarningMessage.NotUnderInSetting(
                    $"{nameof(TilePathPermission)}.{nameof(Dependent)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    SupportVersion));
            }
        }

        /// <summary>
        /// コード値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static TilePathPermission FromCode(int code)
        {
            var searchedWithoutCodeZero =
                _FindAll().Where(x => x.Code != 0).FirstOrDefault(x => (x.Code & code) == x.Code);
            if (!(searchedWithoutCodeZero is null)) return searchedWithoutCodeZero;

            return _FindAll().First(x => x.Code == 0);
        }

        internal enum InnerFlagGroup
        {
            Impassible,
            CannotPassing,
            Not,
        }
    }
}