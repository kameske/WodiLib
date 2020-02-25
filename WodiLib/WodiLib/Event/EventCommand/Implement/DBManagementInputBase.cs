// ========================================
// Project Name : WodiLib
// File Name    : DBManagementInputBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・DB操作（書き込み処理ベース）
    /// </summary>
    public abstract class DBManagementInputBase : DBManagementBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■{0}DB書込：DB[ {1} : {3} : {5} ]  ({2} : {4} : {6}) {7}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.DBManagement;

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

        /// <summary>[NotNull] 項目ID</summary>
        /// <exception cref="PropertyNullException">nullまたはStrOrInt.Noneをセットした場合</exception>
        public IntOrStr DBItemId
        {
            get => _IsItemIdUseStr ? (IntOrStr) _DBItemId.ToStr() : _DBItemId.ToInt();
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBItemId)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DBItemId)));
                _DBItemId.Merge(value);
            }
        }

        /// <summary>右辺X番の変数呼び出し</summary>
        public bool IsRightSideReferX { get; set; }

        /// <inheritdoc />
        /// <summary>読み書きモード</summary>
        protected sealed override byte ioMode
        {
            get => 0x00;
            set { }
        }

        /// <summary>タイプID文字列指定フラグ</summary>
        public bool IsTypeIdUseStr
        {
            get => _IsTypeIdUseStr;
            set => _IsTypeIdUseStr = value;
        }

        /// <summary>データID文字列指定フラグ</summary>
        public bool IsDataIdUseStr
        {
            get => _IsDataIdUseStr;
            set => _IsDataIdUseStr = value;
        }

        /// <summary>項目ID文字列指定フラグ</summary>
        public bool IsItemIdUseStr
        {
            get => _IsItemIdUseStr;
            set => _IsItemIdUseStr = value;
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
            string paramType;
            string targetType;
            int? typeId;
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

            // タイプIDが変数アドレス値の場合、データ名に"×NoData"を表示させるための措置
            var correctTypeId = typeId is null
                ? null
                : typeId.Value.IsVariableAddressSimpleCheck()
                    ? TypeId.MaxValue + 1
                    : typeId;
            string paramData;
            string targetData;
            if (IsDataIdUseStr)
            {
                var dataName = DBDataId.ToStr();
                paramData = dataName;
                targetData = resolver.GetDatabaseDataId(_DBKind, correctTypeId, dataName).Item2;
            }
            else
            {
                var dataId = DBDataId.ToInt();
                paramData = resolver.GetNumericVariableAddressStringIfVariableAddress(dataId, type, desc);
                targetData = resolver.GetDatabaseDataName(_DBKind, correctTypeId, dataId).Item2;
            }

            string paramItem;
            string targetItem;
            if (IsItemIdUseStr)
            {
                var itemName = DBItemId.ToStr();
                paramItem = itemName;
                targetItem = resolver.GetDatabaseItemIdForInputDatabase(_DBKind, typeId, itemName).Item2;
            }
            else
            {
                var itemId = DBItemId.ToInt();
                paramItem = resolver.GetNumericVariableAddressStringIfVariableAddress(itemId, type, desc);
                targetItem = resolver.GetDatabaseItemName(_DBKind, typeId, itemId).Item2;
            }

            var rightSide = MakeEventCommandRightSideSentence(resolver, type, desc);

            return string.Format(EventCommandSentenceFormat,
                _DBKind.EventCommandSentence, paramType, targetType,
                paramData, targetData, paramItem, targetItem, rightSide);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Abstract Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文字列の右辺部分を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列のメイン部分</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);
    }
}