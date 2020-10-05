// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataDescList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBデータ情報リスト
    /// </summary>
    [Serializable]
    public class DatabaseDataDescList : RestrictedCapacityCollection<DatabaseDataDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => DBItemValuesList.MaxCapacity;

        /// <summary>最小容量</summary>
        public static int MinCapacity => DBItemValuesList.MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseDataDescList()
        {
            StartObserveListEvent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期DB項目設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が不適切な場合</exception>
        public DatabaseDataDescList(IEnumerable<DatabaseDataDesc> items) : base(items)
        {
            StartObserveListEvent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNameList">データ名リスト</param>
        /// <param name="valuesList">値リスト</param>
        /// <exception cref="ArgumentNullException">dataNameList, valuesList が null の場合</exception>
        /// <exception cref="ArgumentException">dataNameListとvaluesListの要素数が異なる場合</exception>
        internal DatabaseDataDescList(DataNameList dataNameList,
            DBItemValuesList valuesList)
            : this(DatabaseDataDescCreator.CreateEnumerableDatabaseDataDesc(dataNameList, valuesList))
        {
        }

        /// <summary>
        /// 独自リストのイベント購読を開始する。コンストラクタ用。
        /// </summary>
        private void StartObserveListEvent()
        {
            CollectionChanging += OnCollectionChanging;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自分自身の型情報に適合するDatabaseDataDescインスタンスを生成する。
        /// </summary>
        /// <returns></returns>
        public DatabaseDataDesc CreateMatchItemInstance()
            => new DatabaseDataDesc
            {
                ItemValueList = this[0].ItemValueList.CreateDefaultValueListInstance()
            };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override DatabaseDataDesc MakeDefaultItem(int index)
            => Count > 0
                ? CreateMatchItemInstance()
                : new DatabaseDataDesc();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Event Handler
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CollectionChanging

        private void OnCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ExecuteByAction<DatabaseDataDesc>(
                replaceAction: OnPreSetItem,
                addAction: OnPreInsertItem);
        }

        /// <summary>
        /// 要素を更新する直前の処理
        /// </summary>
        /// <param name="index">更新する要素の先頭インデックス</param>
        /// <param name="oldItems">更新前要素</param>
        /// <param name="newItems">更新後要素</param>
        /// <exception cref="InvalidOperationException">値型情報が一致しない場合場合</exception>
        private void OnPreSetItem(int index, IEnumerable<DatabaseDataDesc> oldItems,
            IEnumerable<DatabaseDataDesc> newItems)
        {
            if (Count <= 1) return;
            newItems.ForEach(item => DBItemValueListValidationHelper.ItemTypeIsSame(this[0], item));
        }

        /// <summary>
        /// 要素を挿入する直前の処理
        /// </summary>
        /// <param name="index">追加するインデックス</param>
        /// <param name="items">追加要素</param>
        private void OnPreInsertItem(int index, IEnumerable<DatabaseDataDesc> items)
        {
            if (Count <= 1) return;

            items.ForEach(item => DBItemValueListValidationHelper.ItemTypeIsSame(this[0], item));
        }

        #endregion


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DatabaseDataDescList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
