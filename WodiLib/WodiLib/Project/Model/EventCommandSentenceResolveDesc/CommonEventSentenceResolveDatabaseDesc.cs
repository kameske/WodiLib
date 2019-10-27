// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSentenceResolveDatabaseDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Database;

namespace WodiLib.Project
{
    /// <summary>
    /// DBイベントコマンド文字列解決情報クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CommonEventSentenceResolveDatabaseDesc
    {
        /// <summary>DB種別</summary>
        public DBKind DbKind { get; private set; }

        /// <summary>タイプID</summary>
        public int? TypeId { get; private set; }

        /// <summary>タイプ名</summary>
        public TypeName TypeName { get; private set; }

        /// <summary>データID</summary>
        public int? DataId { get; private set; }

        /// <summary>データ名</summary>
        public DataName DataName { get; private set; }

        /// <summary>項目ID</summary>
        public int? ItemId { get; private set; }

        /// <summary>項目名</summary>
        public ItemName ItemName { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DB種別を設定する。
        /// </summary>
        /// <param name="kind">[Nullable] DB種別</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetDbKind(DBKind kind)
        {
            DbKind = kind;
            return this;
        }

        /// <summary>
        /// タイプIDを設定する。タイプ名にnullが設定される。
        /// </summary>
        /// <param name="id">タイプID</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetTypeId(int id)
        {
            TypeId = id;
            TypeName = null;
            return this;
        }

        /// <summary>
        /// タイプ名を設定する。タイプIDにnullが設定される。
        /// </summary>
        /// <param name="name">[Nullable] タイプ名</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetTypeName(TypeName name)
        {
            TypeId = null;
            TypeName = name;
            return this;
        }

        /// <summary>
        /// タイプ名、タイプIDにnullを設定する。
        /// </summary>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc ClearType()
        {
            TypeId = null;
            TypeName = null;
            return this;
        }

        /// <summary>
        /// データIDを設定する。データ名にnullが設定される。
        /// </summary>
        /// <param name="id">データID</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetDataId(int id)
        {
            DataId = id;
            DataName = null;
            return this;
        }

        /// <summary>
        /// データ名を設定する。データIDにnullが設定される。
        /// </summary>
        /// <param name="name">[Nullable] データ名</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetDataName(DataName name)
        {
            DataId = null;
            DataName = name;
            return this;
        }

        /// <summary>
        /// データ名、データIDにnullを設定する。
        /// </summary>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc ClearData()
        {
            DataId = null;
            DataName = null;
            return this;
        }

        /// <summary>
        /// 項目IDを設定する。項目名にnullが設定される。
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetItemId(int id)
        {
            ItemId = id;
            ItemName = null;
            return this;
        }

        /// <summary>
        /// 項目名を設定する。項目IDにnullが設定される。
        /// </summary>
        /// <param name="name">[Nullable] 項目名</param>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc SetItemName(ItemName name)
        {
            ItemId = null;
            ItemName = name;
            return this;
        }

        /// <summary>
        /// 項目名、項目にnullを設定する。
        /// </summary>
        /// <returns>自分自身</returns>
        public CommonEventSentenceResolveDatabaseDesc ClearItem()
        {
            ItemId = null;
            ItemName = null;
            return this;
        }
    }
}