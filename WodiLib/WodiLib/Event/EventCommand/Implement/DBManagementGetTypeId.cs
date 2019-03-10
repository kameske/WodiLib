// ========================================
// Project Name : WodiLib
// File Name    : DBManagementGetTypeId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（タイプ番号取得）
    /// </summary>
    public class DBManagementGetTypeId : DBManagementOutputBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[NotNull] DB種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBKind DBKind
        {
            get => _DBKind;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBKind)));
                _DBKind = value;
            }
        }

        /// <summary>
        /// [NotNull] タイプ名
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string DBTypeName
        {
            get => _DBTypeId.ToStr();
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeName)));
                _DBTypeId.Merge(value);
            }
        }

        /// <inheritdoc />
        /// <summary>データID</summary>
        protected override IntOrStr _DBDataId
        {
            get => EventCommandConstant.DBManagement.IdSet.GetTypeX.DataId;
            set { }
        }

        /// <inheritdoc />
        /// <summary>項目ID</summary>
        protected override IntOrStr _DBItemId
        {
            get => EventCommandConstant.DBManagement.IdSet.GetTypeX.ItemId;
            set { }
        }

        /// <inheritdoc />
        /// <summary>タイプID文字列指定フラグ</summary>
        protected override bool _IsTypeIdUseStr
        {
            get => true;
            set { }
        }

        /// <summary>データID文字列指定フラグ</summary>
        protected override bool _IsDataIdUseStr
        {
            get => false;
            set { }
        }

        /// <inheritdoc />
        /// <summary>項目ID文字列指定フラグ</summary>
        protected override bool _IsItemIdUseStr
        {
            get => false;
            set { }
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
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(DBManagementGetTypeId)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_10));
        }
    }
}