// ========================================
// Project Name : WodiLib
// File Name    : DBManagementGetItemId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（項目番号取得）
    /// </summary>
    [Serializable]
    public class DBManagementGetItemId : DBManagementOutputBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "{0}DB[ﾀｲﾌﾟ{1}({2}) 項目{3}({4})の項目番号]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[NotNull] DB種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBKind DBKind
        {
            get => _DBKind;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBKind)));
                _DBKind = value;
            }
        }

        /// <summary>[NotNull] タイプID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBTypeId
        {
            get => _IsTypeIdUseStr ? (IntOrStr) _DBTypeId.ToStr() : _DBTypeId.ToInt();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                _DBTypeId.Merge(value);
            }
        }

        /// <summary>[NotNull] データID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBDataId
        {
            get => _IsDataIdUseStr ? (IntOrStr) _DBDataId.ToStr() : _DBDataId.ToInt();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBDataId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBDataId)));
                _DBDataId.Merge(value);
            }
        }

        /// <inheritdoc />
        /// <summary>データID</summary>
        protected override IntOrStr _DBDataId
        {
            get => EventCommandConstant.DBManagement.IdSet.GetItemX.DataId;
            set { }
        }

        /// <summary>[NotNull] 項目名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string DBItemName
        {
            get => _DBItemId.ToStr();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBItemName)));
                _DBItemId.Merge(value);
            }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        public bool IsTypeIdUseStr
        {
            get => _IsTypeIdUseStr;
            set => _IsTypeIdUseStr = value;
        }

        /// <inheritdoc />
        /// <summary>[NotNull] データID文字列指定フラグ</summary>
        protected override bool _IsDataIdUseStr
        {
            get => false;
            set { }
        }

        /// <inheritdoc />
        /// <summary>[NotNull] 項目ID文字列指定フラグ</summary>
        protected override bool _IsItemIdUseStr
        {
            get => true;
            set { }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            string paramType;
            string targetType;
            TypeId? typeId;
            if (IsTypeIdUseStr)
            {
                paramType = DBTypeId.ToStr();
                var (typeIdNum, typeIdStr) = resolver.GetDatabaseTypeId(_DBKind, DBTypeId.ToStr());
                typeId = typeIdNum;
                targetType = typeIdStr;
            }
            else
            {
                paramType = resolver.GetNumericVariableAddressStringIfVariableAddress(
                    DBTypeId.ToInt(), type, desc);
                typeId = DBTypeId.ToInt();
                targetType = resolver.GetDatabaseTypeName(_DBKind, DBTypeId.ToInt()).Item2;
            }

            var paramData = DBItemName;
            var targetData = resolver.GetDatabaseItemId(DBKind, typeId, DBItemName).Item2;

            return string.Format(EventCommandSentenceFormat,
                _DBKind.EventCommandSentence, paramType, targetType,
                paramData, targetData);
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
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(DBManagementGetItemId)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_10));
        }
    }
}