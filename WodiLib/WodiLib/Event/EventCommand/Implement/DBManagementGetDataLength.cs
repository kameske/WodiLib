// ========================================
// Project Name : WodiLib
// File Name    : DBManagementGetDataLength.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（データ数取得）
    /// </summary>
    [Serializable]
    public class DBManagementGetDataLength : DBManagementOutputBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "{0}DB[ﾀｲﾌﾟ{1}({2})のデータ数]";

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

        /// <inheritdoc />
        /// <summary>データID</summary>
        protected override IntOrStr _DBDataId
        {
            get => EventCommandConstant.DBManagement.IdSet.GetDataLength.DataId;
            set { }
        }

        /// <inheritdoc />
        /// <summary>項目ID</summary>
        protected override IntOrStr _DBItemId
        {
            get => EventCommandConstant.DBManagement.IdSet.GetDataLength.ItemId;
            set { }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        public bool IsTypeIdUseStr
        {
            get => _IsTypeIdUseStr;
            set => _IsTypeIdUseStr = value;
        }

        /// <inheritdoc />
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
            if (IsTypeIdUseStr)
            {
                paramType = DBTypeId.ToStr();
                targetType = resolver.GetDatabaseTypeId(_DBKind, DBTypeId.ToStr())
                    .Item2;
            }
            else
            {
                paramType = resolver.GetNumericVariableAddressStringIfVariableAddress(
                    DBTypeId.ToInt(), type, desc);
                targetType = resolver.GetDatabaseTypeName(_DBKind, DBTypeId.ToInt()).Item2;
            }

            return string.Format(EventCommandSentenceFormat,
                _DBKind.EventCommandSentence, paramType, targetType);
        }
    }
}