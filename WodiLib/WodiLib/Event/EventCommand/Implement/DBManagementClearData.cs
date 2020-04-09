// ========================================
// Project Name : WodiLib
// File Name    : DBManagementClearData.cs
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
    /// イベントコマンド・DB操作（全データ初期化）
    /// </summary>
    [Serializable]
    public class DBManagementClearData : DBManagementClearBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■{0}DB書込：[ﾀｲﾌﾟ {1}({2}) を初期化]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>DB種別</summary>
        protected override DBKind _DBKind
        {
            get => DBKind.Changeable;
            set { }
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
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>データID</summary>
        protected override IntOrStr _DBDataId
        {
            get => EventCommandConstant.DBManagement.IdSet.ClearData.DataId;
            set { }
        }

        /// <inheritdoc />
        /// <summary>項目ID</summary>
        protected override IntOrStr _DBItemId
        {
            get => EventCommandConstant.DBManagement.IdSet.ClearData.ItemId;
            set { }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        public bool IsTypeIdUseStr
        {
            get => _IsTypeIdUseStr;
            set
            {
                _IsTypeIdUseStr = value;
                NotifyPropertyChanged();
            }
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
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var paramType = IsTypeIdUseStr
                ? DBTypeId.ToValueString()
                : resolver.GetNumericVariableAddressStringIfVariableAddress(DBTypeId.ToInt(), type, desc);

            var targetType = IsTypeIdUseStr
                ? resolver.GetDatabaseTypeId(_DBKind, DBTypeId.ToStr()).Item2
                : resolver.GetDatabaseTypeName(_DBKind, DBTypeId.ToInt()).Item2;

            return string.Format(EventCommandSentenceFormat,
                _DBKind.EventCommandSentence, paramType, targetType);
        }
    }
}