// ========================================
// Project Name : WodiLib
// File Name    : DBManagementGetTypeId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

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
    }
}