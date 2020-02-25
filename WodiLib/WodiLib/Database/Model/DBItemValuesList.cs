// ========================================
// Project Name : WodiLib
// File Name    : DBItemValuesList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目設定値リスト
    /// </summary>
    [Serializable]
    public partial class DBItemValuesList : RestrictedCapacityCollection<IFixedLengthDBItemValueList>,
        IReadOnlyDBItemValuesList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => 10000;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public  Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        [NonSerialized]
        private readonly SetItemHandlerList<DBItemValue> setFieldHandlerList = new SetItemHandlerList<DBItemValue>();

        /// <summary>
        /// SetItemイベントハンドラリスト
        /// </summary>
        public SetItemHandlerList<DBItemValue> SetFieldHandlerList => setFieldHandlerList;

        [NonSerialized] private readonly InsertItemHandlerList<DBItemValue> insertFieldHandlerList =
            new InsertItemHandlerList<DBItemValue>();

        /// <summary>
        /// InsertItemイベントハンドラリスト
        /// </summary>
        public InsertItemHandlerList<DBItemValue> InsertFieldHandlerList => insertFieldHandlerList;

        [NonSerialized] private readonly RemoveItemHandlerList<DBItemValue> removeFieldHandlerList =
            new RemoveItemHandlerList<DBItemValue>();

        /// <summary>
        /// RemoveItemイベントハンドラリスト
        /// </summary>
        public RemoveItemHandlerList<DBItemValue> RemoveFieldHandlerList => removeFieldHandlerList;

        [NonSerialized] private readonly ClearItemHandlerList<DBItemValue> clearFieldHandlerList =
            new ClearItemHandlerList<DBItemValue>();

        /// <summary>
        /// ClearItemイベントハンドラリスト
        /// </summary>
        public ClearItemHandlerList<DBItemValue> ClearFieldHandlerList => clearFieldHandlerList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private  Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        [NonSerialized]
        private readonly MadeInstanceManager madeInstances = new MadeInstanceManager();

        /// <summary>生成した値リストインスタンスの弱参照管理インスタンス</summary>
        private MadeInstanceManager MadeInstances => madeInstances;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemValuesList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="list">[NotNull] 初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        /// <exception cref="ArgumentException">
        ///     list中に要素数の異なるリスト、
        ///     または要素の種類が異なるリストがある場合
        /// </exception>
        public DBItemValuesList(IReadOnlyList<IReadOnlyList<DBItemValue>> list)
        {
            try
            {
                ValidateCapacity();
                ValidateDefaultItem();
            }
            catch
            {
                // ここに来てはいけない
                throw new InvalidOperationException();
            }

            if (list is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(list)));

            if (list.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(list)));
            if (list.Any(x => x.HasNullItem()))
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList($"{nameof(list)}の要素"));

            var cnt = list.Count;
            if (cnt < GetMinCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));
            if (cnt > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            // 親の影響を受けないよう要素を削除
            Items.Clear();
            MadeInstances.Clear();

            // 基準になるデータを生成
            if (list.Count == 0)
            {
                // 項目なし
                Items.Add(CreateValueListInstance());
                return;
            }

            // 入力データの1行目を基準データにする
            Items.Add(CreateValueListInstance(list[0]));

            if (list.Count == 1) return;

            // データが1件以上の場合、2件目以降のデータの並びが1件目と同様であるかチェック
            for (var i = 1; i < list.Count; i++)
            {
                var result = ValidateListItem(list[0], list[i]);
                switch (result)
                {
                    case ValidationResult.LengthError:
                        throw new ArgumentException(
                            $"{nameof(list)}[{i}の要素数が異なります。");
                    case ValidationResult.ItemError:
                        throw new ArgumentException(
                            $"{nameof(list)}[{i}]中に種類の異なる項目があります。");
                }

                Items.Add(CreateValueListInstance(list[i]));
            }
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
        /// データの末尾に新規Valuesインスタンスを追加する。
        /// </summary>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void AddNewValues()
        {
            var addedLength = Count + 1;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var values = CreateValueListInstance();
            Add(values);
        }

        /// <summary>
        /// データの末尾に新規Valuesインスタンスを追加する。
        /// </summary>
        /// <param name="count">[Range(0, 10000)] 追加要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">countが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void AddNewValuesRange(int count)
        {
            var max = GetMaxCapacity();
            const int min = 0;
            if (count < min || max < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, max, count));

            var addedLength = Count + count;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var valuesList = new List<IFixedLengthDBItemValueList>();
            for (var i = 0; i < count; i++)
            {
                valuesList.Add(CreateValueListInstance());
            }

            AddRange(valuesList);
        }

        /// <summary>
        /// 新規Valuesインスタンスを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void InsertNewValues(int index)
        {
            var max = Count;
            var min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            var addedLength = Count + 1;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var values = CreateValueListInstance();
            Insert(index, values);
        }

        /// <summary>
        /// 新規Valuesインスタンスを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="count">[Range(0, 10000]) 挿入要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">itemId, countが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">データ数がMaxCapacityを超える場合</exception>
        public void InsertNewValuesRange(int index, int count)
        {
            var indexMax = Count;
            var indexMin = 0;
            if (index < indexMin || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), indexMin, indexMax, index));

            var countMax = GetMaxCapacity();
            const int countMin = 0;
            if (count < countMin || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), countMin, countMax, count));

            var addedLength = Count + count;
            if (addedLength > GetMaxCapacity())
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(GetMaxCapacity()));

            var valuesList = new List<IFixedLengthDBItemValueList>();
            for (var i = 0; i < count; i++)
            {
                valuesList.Add(CreateValueListInstance());
            }

            InsertRange(index, valuesList);
        }

        /// <summary>
        /// DB値リストのインスタンスを生成する。
        /// 値リスト中の値は全て初期化された状態で生成される。
        /// </summary>
        /// <returns>DB値リストインスタンス</returns>
        public IFixedLengthDBItemValueList CreateValueListInstance()
        {
            var instance = new DBItemValueList(this);
            RefreshMadeInstanceReference();
            MadeInstances.Add(new WeakReference<DBItemValueList>(instance));
            return instance;
        }

        /// <summary>
        /// DB値リストのインスタンスを生成する。
        /// 値リスト中の値はvaluesで初期化される。
        /// </summary>
        /// <param name="values">[NotNull] 初期リスト</param>
        /// <returns>DB値リストインスタンス</returns>
        /// <exception cref="ArgumentNullException">valuesがnullの場合</exception>
        /// <exception cref="ArgumentException">
        ///     valuesの要素数、
        ///     またはvalues中の値種別が不適切な場合
        /// </exception>
        public IFixedLengthDBItemValueList CreateValueListInstance(
            IReadOnlyList<DBItemValue> values)
        {
            var instance = new DBItemValueList(this, values);
            RefreshMadeInstanceReference();
            MadeInstances.Add(new WeakReference<DBItemValueList>(instance));
            return instance;
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を更新する。
        /// 更新された項目は、
        ///     値種別が変化した場合はデフォルト値で初期化され、
        ///     値種別が変化しなかった場合は値の変更は起こらない。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1] 項目ID</param>
        /// <param name="type">[NotNull] 値種別</param>
        /// <exception cref="ArgumentOutOfRangeException">itemIdが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public void SetField(ItemId itemId, DBItemType type)
        {
            var max = Items[0].Count - 1;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            MadeInstances.ReflectChangedValueType(itemId, type);
            SetFieldHandlerList.Execute(itemId, type.DBItemDefaultValue);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を更新する。
        /// 更新された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1] 項目ID</param>
        /// <param name="value">[NotNull] 値</param>
        /// <exception cref="ArgumentOutOfRangeException">itemIdが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public void SetField(ItemId itemId, DBItemValue value)
        {
            var max = Items[0].Count - 1;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (value is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(value)));

            MadeInstances.ReflectChangedValue(itemId, value);
            SetFieldHandlerList.Execute(itemId, value);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を追加する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="type">[NotNull] 値種別</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        /// <exception cref="InvalidOperationException">項目数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void AddField(DBItemType type)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            var addedLength = Items[0].Count + 1;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            var index = Items[0].Count;
            MadeInstances.ReflectAddValueType(type);
            InsertFieldHandlerList.Execute(index, type.DBItemDefaultValue);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を追加する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="value">[NotNull] 値</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        /// <exception cref="InvalidOperationException">項目数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void AddField(DBItemValue value)
        {
            if (value is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(value)));

            var addedLength = Items[0].Count + 1;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            var index = Items[0].Count;
            MadeInstances.ReflectAddValue(value);
            InsertFieldHandlerList.Execute(index, value);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を追加する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="types">[NotNull] 値種別リスト</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        /// <exception cref="InvalidOperationException">項目数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void AddFieldRange(IReadOnlyCollection<DBItemType> types)
        {
            if (types is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(types)));

            var addedLength = Items[0].Count + types.Count;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            var index = Items[0].Count;
            MadeInstances.ReflectAddValueTypeRange(types);
            foreach (var type in types)
            {
                InsertFieldHandlerList.Execute(index, type.DBItemDefaultValue);
                index++;
            }
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を追加する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="values">[NotNull] 値</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        /// <exception cref="InvalidOperationException">項目数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void AddFieldRange(IReadOnlyCollection<DBItemValue> values)
        {
            if (values is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(values)));

            var addedLength = Items[0].Count + values.Count;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            var index = Items[0].Count;
            MadeInstances.ReflectAddValueRange(values);
            foreach (var value in values)
            {
                InsertFieldHandlerList.Execute(index, value);
                index++;
            }
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を挿入する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count)] 項目ID</param>
        /// <param name="type">[NotNull] 値種別</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void InsertField(ItemId itemId, DBItemType type)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            var insertedLength = Items[0].Count + 1;
            if (insertedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            MadeInstances.ReflectInsertValueType(itemId, type);
            InsertFieldHandlerList.Execute(itemId, type.DBItemDefaultValue);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を挿入する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count)] インデックス</param>
        /// <param name="value">[NotNull] 値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void InsertField(ItemId itemId, DBItemValue value)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (value is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(value)));

            var insertedLength = Items[0].Count + 1;
            if (insertedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            MadeInstances.ReflectInsertValue(itemId, value);
            InsertFieldHandlerList.Execute(itemId, value);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を挿入する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="index">[Range(0, Items[0].Count)] インデックス</param>
        /// <param name="types">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     typesがnullの場合、
        ///     またはtypesにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void InsertFieldRange(int index, IReadOnlyCollection<DBItemType> types)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (types is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(types)));

            if (types.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(types)));

            var addedLength = Items[0].Count + types.Count;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            MadeInstances.ReflectInsertValueTypeRange(index, types);
            var handlerIndex = index;
            foreach (var type in types)
            {
                InsertFieldHandlerList.Execute(handlerIndex, type.DBItemDefaultValue);
                handlerIndex++;
            }
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を挿入する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="index">[Range(0, Items[0].Count)] インデックス</param>
        /// <param name="values">[NotNull] 追加する値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     typesがnullの場合、
        ///     またはtypesにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void InsertFieldRange(int index, IReadOnlyCollection<DBItemValue> values)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (values is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(values)));

            if (values.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(values)));

            var addedLength = Items[0].Count + values.Count;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            MadeInstances.ReflectInsertValueRange(index, values);
            var handlerIndex = index;
            foreach (var value in values)
            {
                InsertFieldHandlerList.Execute(handlerIndex, value);
                handlerIndex++;
            }
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、指定したインデックスにある項目を削除する。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がDBItemValueList.MinValue未満になる場合</exception>
        public void RemoveFieldAt(ItemId itemId)
        {
            var max = Items[0].Count - 1;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            var removedLength = Items[0].Count - 1;
            if (removedLength < DBItemValueList.MinCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(DBItemValueList.MinCapacity));

            MadeInstances.ReflectRemoveAt(itemId);
            RemoveFieldHandlerList.Execute(itemId);
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Items[0].Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Items[0].Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMDBItemValueList.inValue未満になる場合</exception>
        public void RemoveFieldRange(int index, int count)
        {
            var indexMax = Items[0].Count - 1;
            const int min = 0;

            if (index < min || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, indexMax, index));

            var countMax = Items[0].Count;

            if (count < min || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, countMax, count));

            if (Items[0].Count - index < count)
                throw new ArgumentException(
                    $"{nameof(index)}および{nameof(count)}が有効な範囲を示していません。");

            var removedLength = Items[0].Count - count;
            if (removedLength < DBItemValueList.MinCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));

            MadeInstances.ReflectRemoveRange(index, count);
            for (var i = 0; i < count; i++)
            {
                RemoveFieldHandlerList.Execute(index);
            }
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、
        /// すべての要素を削除する。
        /// </summary>
        public void ClearField()
        {
            MadeInstances.ReflectClear();
            ClearFieldHandlerList.Execute();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目チェック
        /// </summary>
        /// <param name="checkList">[NotNull] チェック対象リスト</param>
        /// <returns>チェック結果</returns>
        internal ValidationResult ValidateListItem(DBItemValueList checkList)
        {
            // 基準データがない場合はcheckList自身が基準となるためチェックしない
            if (Items.Count == 0) return ValidationResult.OK;

            var baseList = Items[0];

            if (baseList.Count != checkList.Count) return ValidationResult.LengthError;

            var searchError = checkList.Where((t, i) => t.Type != baseList[i].Type).Any();
            if (searchError)
            {
                return ValidationResult.ItemError;
            }

            return ValidationResult.OK;
        }

        /// <summary>
        /// いずれにも紐付いていないDBItemValueListを自身に紐付ける。
        /// </summary>
        /// <param name="item">[NotNull] 紐付け対象</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">itemが既に別のDBItemValuesListに紐付けられている場合</exception>
        internal void AddNewRelationshipInstance(DBItemValueList item)
        {
            if (item is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));

            var isReferenceEqual = ReferenceEquals(item.Outer, this);
            if (!(item.Outer is null) && !isReferenceEqual)
            {
                throw new InvalidOperationException(
                    $"既に異なる{nameof(DBItemValuesList)}に紐付けされているため、" +
                    $"紐付けできません。");
            }

            if (isReferenceEqual)
            {
                // 既に紐付けされているため、何もしない
                return;
            }

            MadeInstances.Add(new WeakReference<DBItemValueList>(item));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override IFixedLengthDBItemValueList MakeDefaultItem(int index) => CreateValueListInstance();

        /// <inheritdoc />
        /// <summary>
        /// 指定したインデックス位置にある要素を置き換える。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void SetItem(int index, IFixedLengthDBItemValueList item)
        {
            var writableItem = (DBItemValueList) item;
            if (!(writableItem.Outer is null)
                && !writableItem.Outer.Equals(this))
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable(nameof(item),
                        "セットしようとしたDBItemValuesListによって生成されたインスタンスではないため、" +
                        "更新できません。"));
            }

            if (writableItem.Outer is null)
            {
                var validateResult = ValidateListItem(writableItem);
                switch (validateResult)
                {
                    case ValidationResult.LengthError:
                        throw new ArgumentException(
                            $"{nameof(item)}の要素数が異なります。");
                    case ValidationResult.ItemError:
                        throw new ArgumentException(
                            $"{nameof(item)}中に種類の異なる項目があります。");
                }

                writableItem.Attach(this);
            }

            base.SetItem(index, item);
            RefreshMadeInstanceReference();
        }

        /// <inheritdoc />
        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void InsertItem(int index, IFixedLengthDBItemValueList item)
        {
            var writableItem = (DBItemValueList) item;
            if (!(writableItem.Outer is null)
                && !writableItem.Outer.Equals(this))
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable(nameof(item),
                        "セットしようとしたDBItemValuesListによって生成されたインスタンスではないため、" +
                        "追加できません。"));
            }

            if (writableItem.Outer is null)
            {
                var validateResult = ValidateListItem(writableItem);
                switch (validateResult)
                {
                    case ValidationResult.LengthError:
                        throw new ArgumentException(
                            $"{nameof(item)}の要素数が異なります。");
                    case ValidationResult.ItemError:
                        throw new ArgumentException(
                            $"{nameof(item)}中に種類の異なる項目があります。");
                }

                writableItem.Attach(this);
            }

            base.InsertItem(index, item);
            RefreshMadeInstanceReference();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 生成した値リストインスタンスの弱参照から、参照できなくなったインスタンスの参照を解除する。
        /// </summary>
        private void RefreshMadeInstanceReference()
        {
            MadeInstances.Refresh();
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
        private static ValidationResult ValidateListItem(IReadOnlyList<DBItemValue> baseList,
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
        //     Internal Enum
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal enum ValidationResult
        {
            OK,
            LengthError,
            ItemError
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目数 + 設定種別 &amp; 種別順列 をバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinarySettingList()
        {
            var result = new List<byte>();

            // 要素数
            result.AddRange(Items[0].Count.ToBytes(Endian.Woditor));

            // 要素
            var cntDict = new Dictionary<DBItemType, int>
            {
                {DBItemType.Int, 0},
                {DBItemType.String, 0}
            };

            foreach (var itemType in Items[0].Select(x => x.Type))
            {
                var addValue = itemType.TypeOrderStart + cntDict[itemType];
                result.AddRange(addValue.ToBytes(Endian.Woditor));

                cntDict[itemType] += 1;
            }

            return result.ToArray();
        }

        /// <summary>
        /// データ数 &amp; データ設定値をバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryDataValues()
        {
            var result = new List<byte>();

            // 要素数
            result.AddRange(Count.ToBytes(Endian.Woditor));

            // DB項目設定値リスト
            foreach (var item in Items.Select(x => (DBItemValueList) x))
            {
                result.AddRange(item.ToBinary());
            }

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DBItemValuesList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}