// ========================================
// Project Name : WodiLib
// File Name    : DBManagementGetDataId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（データ番号取得）
    /// </summary>
    public class DBManagementGetDataId : DBManagementOutputBase
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

        /// <summary>[NotNull] タイプID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBTypeId
        {
            get => _IsTypeIdUseStr ? (IntOrStr) _DBTypeId.ToStr() : _DBTypeId.ToInt();
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBTypeId)));
                _DBTypeId.Merge(value);
            }
        }

        /// <inheritdoc />
        /// <summary>項目ID</summary>
        protected override IntOrStr _DBItemId
        {
            get => EventCommandConstant.DBManagement.IdSet.GetDataX.ItemId;
            set { }
        }

        /// <summary>[NotNull] データ名</summary>
        public string DBDataName
        {
            get => _DBDataId.ToStr();
            set => _DBDataId.Merge(value);
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
            get => true;
            set { }
        }

        /// <inheritdoc />
        /// <summary>項目ID文字列指定フラグ</summary>
        protected override bool _IsItemIdUseStr
        {
            get => false;
            set { }
        }
    }
}