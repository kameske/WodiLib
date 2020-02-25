// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.Database.Basic.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス・DB（通常）
    /// </summary>
    internal class EventCommandSentenceResolver_Database_Basic
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal static readonly string DatabaseDataNotFound = "×NoData";

        internal static readonly string TargetIdNotFound = " ? ";
        internal static readonly string TargetIdVariableAddress = "-";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>利用元</summary>
        public EventCommandSentenceResolver Master { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="master">利用元</param>
        public EventCommandSentenceResolver_Database_Basic(
            EventCommandSentenceResolver master)
        {
            Master = master;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイプIDを指定してDBのタイプ名を取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="id">タイプID</param>
        /// <returns>
        ///     存在フラグとタイプ名。
        ///     存在しない場合、nullであれば専用の文字列。
        ///     id が変数アドレス値の場合、専用の文字列。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (bool, string) GetDatabaseTypeName(DBKind dbKind, int id)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));

            if (id.IsVariableAddressSimpleCheck()) return (true, TargetIdVariableAddress);

            var refer = GetDatabaseRefer(dbKind);

            var typeDescList = refer.TypeDescList;
            if (id < 0 || typeDescList.Count <= id) return (false, DatabaseDataNotFound);

            return (true, typeDescList[id].TypeName);
        }

        /// <summary>
        /// タイプID、データIDを指定して可変DBのデータ名を取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="dataId">データID</param>
        /// <returns>
        ///     存在フラグとデータ名。
        ///     存在しない場合、専用の文字列。
        ///     dataId が変数アドレス値の場合、専用の文字列。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (bool, string) GetDatabaseDataName(DBKind dbKind, int? typeId,
            int dataId)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));

            if (typeId is null) return (false, DatabaseDataNotFound);

            if (dataId.IsVariableAddressSimpleCheck()) return (true, TargetIdVariableAddress);

            var refer = GetDatabaseRefer(dbKind);

            var typeDescList = refer.TypeDescList;
            if (typeId < 0 || typeDescList.Count <= typeId) return (false, DatabaseDataNotFound);
            var typeDesc = typeDescList[typeId.Value];

            var dataDescList = typeDesc.DataDescList;
            if (dataId < 0 || dataDescList.Count <= dataId) return (false, DatabaseDataNotFound);

            var dataSettingType = typeDesc.DataSettingType;

            if (dataSettingType == DBDataSettingType.Manual)
            {
                return (true, dataDescList[dataId].DataName);
            }

            if (dataSettingType == DBDataSettingType.EqualBefore)
            {
                if (typeId == 0)
                {
                    return (false, DatabaseDataNotFound);
                }

                return GetDatabaseDataName(dbKind, typeId - 1, dataId);
            }

            if (dataSettingType == DBDataSettingType.FirstStringData)
            {
                var targetItemIndex = typeDesc.DBItemSettingList
                    .FindIndex(x => x.ItemType == DBItemType.String);

                if (targetItemIndex == -1) return (true, string.Empty);
                return (true, typeDesc.DataDescList[dataId].ItemValueList[targetItemIndex].StringValue);
            }

            if (dataSettingType == DBDataSettingType.DesignatedType)
            {
                return GetDatabaseDataName(typeDesc.DBKind, typeDesc.TypeId, dataId);
            }

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }

        /// <summary>
        /// タイプID、項目IDを指定してDBの項目名を取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="itemId">項目ID</param>
        /// <returns>
        ///     存在フラグと項目名。
        ///     存在しない場合、専用の文字列。
        ///     itemId が変数アドレス値の場合、専用の文字列。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (bool, string) GetDatabaseItemName(DBKind dbKind, int? typeId,
            int itemId)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));

            if (typeId is null) return (false, DatabaseDataNotFound);

            if (itemId.IsVariableAddressSimpleCheck()) return (true, TargetIdVariableAddress);

            var refer = GetDatabaseRefer(dbKind);

            var typeDescList = refer.TypeDescList;
            if (typeId.Value < 0 || typeDescList.Count <= typeId.Value) return (false, DatabaseDataNotFound);
            var typeDesc = typeDescList[typeId.Value];

            var itemDescList = typeDesc.ItemDescList;
            if (itemId < 0 || itemDescList.Count <= itemId) return (false, DatabaseDataNotFound);

            return (true, itemDescList[itemId].ItemName);
        }

        /// <summary>
        /// タイプ名を指定してタイプIDを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="name">[NotNull] タイプ名</param>
        /// <returns>タイプID。見つからない場合null。</returns>
        /// <exception cref="ArgumentNullException">dbKind または name が null の場合</exception>
        public (int?, string) GetDatabaseTypeId(DBKind dbKind, string name)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));
            if (name is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(name)));

            var refer = GetDatabaseRefer(dbKind);

            var searchTypeId = -1;
            for (var i = 0; i < refer.TypeDescList.Count; i++)
            {
                if (refer.TypeDescList[i].TypeName.Equals(name))
                {
                    searchTypeId = i;
                    break;
                }
            }

            if (searchTypeId == -1) return (null, TargetIdNotFound);

            return (searchTypeId, searchTypeId.ToString());
        }

        /// <summary>
        /// タイプID、データ名を指定してデータIDを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="dataName">[NotNull] データ名</param>
        /// <returns>
        ///     データID。
        ///     データが見つからない場合、null。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKind または dataName が null の場合</exception>
        public (int?, string) GetDatabaseDataId(DBKind dbKind, int? typeId,
            string dataName)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));
            if (dataName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataName)));

            if (typeId is null) return (null, DatabaseDataNotFound);

            if (typeId.Value.IsVariableAddressSimpleCheck()) return (null, TargetIdNotFound);

            var refer = GetDatabaseRefer(dbKind);

            var typeDescList = refer.TypeDescList;
            if (typeId.Value < 0 || typeDescList.Count <= typeId.Value) return (null, DatabaseDataNotFound);
            var targetTypeDesc = typeDescList[typeId.Value];

            var targetDataDesc = targetTypeDesc.DataDescList
                .Select((x, idx) => (idx, x.DataName))
                .FirstOrDefault(y => y.DataName.Equals(dataName));

            if (targetDataDesc == default((int, DataName))) return (null, TargetIdNotFound);

            return (targetDataDesc.idx, targetDataDesc.idx.ToString());
        }

        /// <summary>
        /// タイプID、項目名を指定して項目IDを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="itemName">[NotNull] 項目名</param>
        /// <returns>
        ///     項目ID。
        ///     項目が見つからない場合、null。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (int?, string) GetDatabaseItemId(DBKind dbKind, int? typeId,
            string itemName)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));
            if (itemName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(itemName)));

            if (typeId is null) return (null, DatabaseDataNotFound);

            if (typeId.Value.IsVariableAddressSimpleCheck()) return (null, DatabaseDataNotFound);

            var refer = GetDatabaseRefer(dbKind);

            if (typeId.Value < 0 || refer.TypeDescList.Count <= typeId.Value) return (null, DatabaseDataNotFound);
            var targetTypeDesc = refer.TypeDescList[typeId.Value];

            var targetItemDesc = targetTypeDesc.ItemDescList
                .Select((x, idx) => (idx, x.ItemName))
                .FirstOrDefault(y => y.ItemName.Equals(itemName));

            if (targetItemDesc == default((int, DataName))) return (null, TargetIdNotFound);

            return (targetItemDesc.idx, targetItemDesc.idx.ToString());
        }

        /// <summary>
        /// タイプID、項目名を指定して項目IDを取得する。
        /// イベントコマンド「DB書込」専用。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <param name="itemName">[NotNull] 項目名</param>
        /// <returns>
        ///     項目ID。
        ///     項目が見つからない場合、null。
        /// </returns>
        /// <exception cref="ArgumentNullException">dbKindがnullの場合</exception>
        public (int?, string) GetDatabaseItemIdForInputDatabase(DBKind dbKind, int? typeId,
            string itemName)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));
            if (itemName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(itemName)));

            if (typeId is null) return (null, DatabaseDataNotFound);

            if (typeId.Value.IsVariableAddressSimpleCheck()) return (null, TargetIdNotFound);

            var refer = GetDatabaseRefer(dbKind);

            if (typeId.Value < 0 || refer.TypeDescList.Count <= typeId.Value) return (null, DatabaseDataNotFound);
            var targetTypeDesc = refer.TypeDescList[typeId.Value];

            var targetItemDesc = targetTypeDesc.ItemDescList
                .Select((x, idx) => (idx, x.ItemName))
                .FirstOrDefault(y => y.ItemName.Equals(itemName));

            if (targetItemDesc == default((int, DataName))) return (null, TargetIdNotFound);

            return (targetItemDesc.idx, targetItemDesc.idx.ToString());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBマージデータの参照を取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <returns>DBマージデータ</returns>
        /// <exception cref="InvalidOperationException">意図しない処理の場合</exception>
        private DatabaseMergedData GetDatabaseRefer(DBKind dbKind)
        {
            if (dbKind == DBKind.Changeable)
            {
                return Master.ChangeableDatabase;
            }

            if (dbKind == DBKind.User)
            {
                return Master.UserDatabase;
            }

            if (dbKind == DBKind.System)
            {
                return Master.SystemDatabase;
            }

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }
    }
}