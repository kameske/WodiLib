// ========================================
// Project Name : WodiLib
// File Name    : ReadOnlyDBItemValuesList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 項目が読み取り専用のDB項目設定値リストのリスト
    /// </summary>
    public class ReadOnlyDBItemValuesList : RestrictedCapacityCollection<IReadOnlyDBItemValueList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public new Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        public new IReadOnlyDBItemValueList this[int index]
        {
            get => (IReadOnlyDBItemValueList) InnerList[index];
            set
            {
                if (value == null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(value)));

                var max = Count - 1;
                const int min = 0;
                if (index < min || max < index)
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), min, max, index));

                InnerList[index] = (IFixedLengthDBItemValueList) value;
            }
        }

        /// <summary>要素数</summary>
        public override int Count => InnerList?.Count ?? 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private  Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DBItemValuesList InnerList { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ReadOnlyDBItemValuesList(DBItemValuesList target)
        {
            InnerList = target;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => DBItemValuesList.MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => DBItemValuesList.MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データの末尾に新規Valuesインスタンスを追加する。
        /// </summary>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void AddNewValues() => InnerList.AddNewValues();

        /// <summary>
        /// データの末尾に新規Valuesインスタンスを追加する。
        /// </summary>
        /// <param name="count">[Range(0, 10000)] 追加要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">countが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void AddNewValuesRange(int count) => InnerList.AddNewValuesRange(count);

        /// <summary>
        /// 新規Valuesインスタンスを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void InsertNewValues(int index) => InnerList.InsertNewValues(index);

        /// <summary>
        /// 新規Valuesインスタンスを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="count">[Range(0, 10000]) 挿入要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void InsertNewValuesRange(int index, int count) => InnerList.InsertNewValuesRange(index, count);

        /// <summary>
        /// DB値リストのインスタンスを生成する。
        /// 値リスト中の値は全て初期化された状態で生成される。
        /// </summary>
        /// <returns>DB値リストインスタンス</returns>
        public IReadOnlyDBItemValueList CreateValueListInstance() =>
            InnerList != null
                ? (IReadOnlyDBItemValueList) InnerList.CreateValueListInstance()
                : new DBItemValueList();

        /// <summary>
        /// DB値リストのインスタンスを生成する。
        /// 値リスト中の値はvaluesで初期化される。
        /// </summary>
        /// <param name="values">[NotNull] 初期リスト</param>
        /// <returns>DB値リスト</returns>
        /// <exception cref="ArgumentNullException">valuesがnullの場合</exception>
        /// <exception cref="ArgumentException">
        ///     valuesの要素数、
        ///     またはvalues中の値種別が不適切な場合
        /// </exception>
        public IReadOnlyDBItemValueList CreateValueListInstance(IReadOnlyList<DBItemValue> values)
            => (IReadOnlyDBItemValueList) InnerList.CreateValueListInstance(values);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override IReadOnlyDBItemValueList MakeDefaultItem(int index) => CreateValueListInstance();

        /// <inheritdoc />
        /// <summary>
        /// 指定したインデックス位置にある要素を置き換える。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void SetItem(int index, IReadOnlyDBItemValueList item)
        {
            if (Items.Count > 1)
            {
                // 項目リストが2以上ある場合、最初の要素と一致するかチェック
                var result = ValidateListItem(Items[0], item);
                switch (result)
                {
                    case ValidationResult.LengthError:
                        throw new ArgumentException(
                            $"{nameof(item)}の要素数が異なります。");
                    case ValidationResult.ItemError:
                        throw new ArgumentException(
                            $"{nameof(item)}中に種類の異なる項目があります。");
                }
            }

            InnerList[index] = item.ToFixedLengthList();

            // 要素数は Item で管理しているため、Item にも変更を反映させる必要がある
            base.SetItem(index, item);
        }

        /// <inheritdoc />
        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void InsertItem(int index, IReadOnlyDBItemValueList item)
        {
            if (Items.Count > 1)
            {
                // 項目リストが1以上ある場合、最初の要素と一致するかチェック
                var result = ValidateListItem(Items[0], item);
                switch (result)
                {
                    case ValidationResult.LengthError:
                        throw new ArgumentException(
                            $"{nameof(item)}の要素数が異なります。");
                    case ValidationResult.ItemError:
                        throw new ArgumentException(
                            $"{nameof(item)}中に種類の異なる項目があります。");
                }
            }

            // コンストラクタ中にここに来る場合にInnerList = nullとなる。
            // 親のコンストラクタ処理完了後、自身のコンストラクタ処理でInnerListの初期化を行うため、ここでは処理不要
            InnerList?.Insert(index, item.ToFixedLengthList());
            base.InsertItem(index, item);
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">インデックス</param>
        protected override void RemoveItem(int index)
        {
            InnerList.RemoveAt(index);
            base.RemoveItem(index);
        }

        /// <summary>
        /// 要素をすべて除去したあと、
        /// 必要に応じて最小限の要素を新たに設定する。
        /// </summary>
        protected override void ClearItems()
        {
            InnerList.Clear();
            base.ClearItems();

            /*
             * InnerList.Clearで InnerListの 要素が1になった後、
             * base.ClearItems() 内の処理により InsertItem(0, item) が実行されてしまう（InnerList.Count = 2 となる）ので、
             * 後者によって追加されたデータを打ち消す。
             */
            // InnerList.RemoveAt(0);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目チェック
        /// </summary>
        /// <param name="baseList">[NotNull] チェック基準リスト</param>
        /// <param name="checkList">[NotNull] チェック対象リスト</param>
        /// <returns>チェック結果</returns>
        private static ValidationResult ValidateListItem(IReadOnlyDBItemValueList baseList,
            IReadOnlyCollection<DBItemValue> checkList)
        {
            if (baseList.Count != checkList.Count) return ValidationResult.LengthError;

            var searchError = checkList.Where((t, i) => t.Type != baseList[i].Type).Any();
            if (searchError)
            {
                return ValidationResult.ItemError;
            }

            return ValidationResult.OK;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Enum
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        [Serializable]
        private enum ValidationResult
        {
            OK,
            LengthError,
            ItemError
        }
    }
}