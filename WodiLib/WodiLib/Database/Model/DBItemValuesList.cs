// ========================================
// Project Name : WodiLib
// File Name    : DBItemValuesList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
#pragma warning disable 618
        private readonly SetItemHandlerList<DBItemValue> setFieldHandlerList = new SetItemHandlerList<DBItemValue>();
#pragma warning restore 618

        /// <summary>
        /// SetItemイベントハンドラリスト
        /// </summary>
        [Obsolete("要素変更通知は各要素のCollectionChangedイベントを利用して取得してください。 Ver1.3 で削除します。")]
        public SetItemHandlerList<DBItemValue> SetFieldHandlerList => setFieldHandlerList;

#pragma warning disable 618
        [NonSerialized] private readonly InsertItemHandlerList<DBItemValue> insertFieldHandlerList =
            new InsertItemHandlerList<DBItemValue>();
#pragma warning restore 618

        /// <summary>
        /// InsertItemイベントハンドラリスト
        /// </summary>
        [Obsolete("要素変更通知は各要素のCollectionChangedイベントを利用して取得してください。 Ver1.3 で削除します。")]
        public InsertItemHandlerList<DBItemValue> InsertFieldHandlerList => insertFieldHandlerList;

#pragma warning disable 618
        [NonSerialized] private readonly RemoveItemHandlerList<DBItemValue> removeFieldHandlerList =
            new RemoveItemHandlerList<DBItemValue>();
#pragma warning restore 618

        /// <summary>
        /// RemoveItemイベントハンドラリスト
        /// </summary>
        [Obsolete("要素変更通知は各要素のCollectionChangedイベントを利用して取得してください。 Ver1.3 で削除します。")]
        public RemoveItemHandlerList<DBItemValue> RemoveFieldHandlerList => removeFieldHandlerList;

#pragma warning disable 618
        [NonSerialized] private readonly ClearItemHandlerList<DBItemValue> clearFieldHandlerList =
            new ClearItemHandlerList<DBItemValue>();
#pragma warning restore 618

        /// <summary>
        /// ClearItemイベントハンドラリスト
        /// </summary>
        [Obsolete("要素変更通知は各要素のCollectionChangedイベントを利用して取得してください。 Ver1.3 で削除します。")]
        public ClearItemHandlerList<DBItemValue> ClearFieldHandlerList => clearFieldHandlerList;


        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized]
        private event NotifyCollectionChangedEventHandler _fieldCollectionChanged
            = delegate { };

        [field: NonSerialized]
        private event PropertyChangedEventHandler _fieldPropertyChanged
            = delegate { };

        /// <summary>
        /// 要素変更通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        public event NotifyCollectionChangedEventHandler FieldCollectionChanged
        {
            add
            {
                if (_fieldCollectionChanged != null
                    && _fieldCollectionChanged.GetInvocationList().Contains(value)) return;
                _fieldCollectionChanged += value;
            }
            remove => _fieldCollectionChanged -= value;
        }

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        public event PropertyChangedEventHandler FieldPropertyChanged
        {
            add
            {
                if (_fieldPropertyChanged != null
                    && _fieldPropertyChanged.GetInvocationList().Contains(value)) return;
                _fieldPropertyChanged += value;
            }
            remove => _fieldPropertyChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private  Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        [NonSerialized] private readonly MadeInstanceManager madeInstances = new MadeInstanceManager();

        /// <summary>生成した値リストインスタンスの弱参照管理インスタンス</summary>
        private MadeInstanceManager MadeInstances => madeInstances;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 子要素プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnFieldCollectionPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            _fieldPropertyChanged.Invoke(this, args);
        }

        /// <summary>
        /// 子要素アイテム変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnFieldCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            _fieldCollectionChanged.Invoke(this, args);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemValuesList()
        {
            AttachFiledCollectionNotification(this[0]);
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="initItems">[NotNull] 初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">initItemsの要素数が不適切な場合</exception>
        /// <exception cref="ArgumentException">
        ///     initItems中に要素数の異なるリスト、
        ///     または要素の種類が異なるリストがある場合
        /// </exception>
        public DBItemValuesList(IEnumerable<IEnumerable<DBItemValue>> initItems)
        {
            if (initItems is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(initItems)));

            var initItemArr = initItems.ToArray();

            if (initItemArr.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(initItems)));
            if (initItemArr.Any(x => x.HasNullItem()))
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList($"{nameof(initItems)}の要素"));

            var cnt = initItemArr.Length;
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
            if (initItemArr.Length == 0)
            {
                // 項目なし
                Items.Add(CreateValueListInstance());
                AttachFiledCollectionNotification(this[0]);
                return;
            }

            // 入力データの1行目を基準データにする
            Items.Add(CreateValueListInstance(initItemArr[0]));

            if (initItemArr.Length == 1)
            {
                AttachFiledCollectionNotification(this[0]);
                return;
            }

            // データが1件以上の場合、2件目以降のデータの並びが1件目と同様であるかチェック
            for (var i = 1; i < initItemArr.Length; i++)
            {
                var result = ValidateListItem(initItemArr[0], initItemArr[i]);
                switch (result)
                {
                    case ValidationResult.LengthError:
                        throw new ArgumentException(
                            $"{nameof(initItems)}[{i}の要素数が異なります。");
                    case ValidationResult.ItemError:
                        throw new ArgumentException(
                            $"{nameof(initItems)}[{i}]中に種類の異なる項目があります。");
                }

                Items.Add(CreateValueListInstance(initItemArr[i]));
            }

            AttachFiledCollectionNotification(this[0]);
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
            IEnumerable<DBItemValue> values)
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
#pragma warning disable 618
            SetFieldHandlerList.Execute(itemId, type.DBItemDefaultValue);
#pragma warning restore 618
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
#pragma warning disable 618
            SetFieldHandlerList.Execute(itemId, value);
#pragma warning restore 618
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
#pragma warning disable 618
            InsertFieldHandlerList.Execute(index, type.DBItemDefaultValue);
#pragma warning restore 618
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
#pragma warning disable 618
            InsertFieldHandlerList.Execute(index, value);
#pragma warning restore 618
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、項目を追加する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="types">[NotNull] 値種別リスト</param>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        /// <exception cref="InvalidOperationException">項目数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void AddFieldRange(IEnumerable<DBItemType> types)
        {
            if (types is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(types)));

            var typeArr = types.ToArray();

            var addedLength = Items[0].Count + typeArr.Length;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            var index = Items[0].Count;
            MadeInstances.ReflectAddValueTypeRange(typeArr);
            foreach (var type in typeArr)
            {
#pragma warning disable 618
                InsertFieldHandlerList.Execute(index, type.DBItemDefaultValue);
#pragma warning restore 618
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
        public void AddFieldRange(IEnumerable<DBItemValue> values)
        {
            if (values is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(values)));

            var valueArr = values.ToArray();

            var addedLength = Items[0].Count + valueArr.Length;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            var index = Items[0].Count;
            MadeInstances.ReflectAddValueRange(valueArr);
#pragma warning disable 618
            foreach (var value in valueArr)
            {
                InsertFieldHandlerList.Execute(index, value);
                index++;
            }
#pragma warning restore 618
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
#pragma warning disable 618
            InsertFieldHandlerList.Execute(itemId, type.DBItemDefaultValue);
#pragma warning restore 618
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
#pragma warning disable 618
            InsertFieldHandlerList.Execute(itemId, value);
#pragma warning restore 618
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
        public void InsertFieldRange(int index, IEnumerable<DBItemType> types)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (types is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(types)));

            var typeArr = types.ToArray();

            if (typeArr.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(types)));

            var addedLength = Items[0].Count + typeArr.Length;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            MadeInstances.ReflectInsertValueTypeRange(index, typeArr);
            var handlerIndex = index;
#pragma warning disable 618
            foreach (var type in typeArr)
            {
                InsertFieldHandlerList.Execute(handlerIndex, type.DBItemDefaultValue);
                handlerIndex++;
            }
#pragma warning restore 618
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
        public void InsertFieldRange(int index, IEnumerable<DBItemValue> values)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (values is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(values)));

            var valueArr = values.ToArray();

            if (valueArr.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(values)));

            var addedLength = Items[0].Count + valueArr.Length;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            MadeInstances.ReflectInsertValueRange(index, valueArr);
            var handlerIndex = index;
#pragma warning disable 618
            foreach (var value in valueArr)
            {
                InsertFieldHandlerList.Execute(handlerIndex, value);
                handlerIndex++;
            }
#pragma warning restore 618
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
#pragma warning disable 618
            RemoveFieldHandlerList.Execute(itemId);
#pragma warning restore 618
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
#pragma warning disable 618
            for (var i = 0; i < count; i++)
            {
                RemoveFieldHandlerList.Execute(index);
            }
#pragma warning restore 618
        }

        /// <summary>
        /// 自身が生成したすべての値リストインスタンスに対し、
        /// すべての要素を削除する。
        /// </summary>
        public void ClearField()
        {
            MadeInstances.ReflectClear();
#pragma warning disable 618
            ClearFieldHandlerList.Execute();
#pragma warning restore 618
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

            ReattachFiledCollectionNotificationIfNeed_Set(index, item);
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

            ReattachFiledCollectionNotificationIfNeed_Insert(index, item);
            base.InsertItem(index, item);
            RefreshMadeInstanceReference();
        }

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス</param>
        /// <param name="newIndex">移動先のインデックス</param>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            ReattachFiledCollectionNotificationIfNeed_Move(oldIndex, newIndex);
            base.MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">インデックス</param>
        protected override void RemoveItem(int index)
        {
            ReattachFiledCollectionNotificationIfNeed_Remove(index);
            base.RemoveItem(index);
        }

        /// <summary>
        /// 要素をすべて除去する。
        /// </summary>
        protected override void ClearItems()
        {
            ReattachFiledCollectionNotificationIfNeed_Clear();
            base.ClearItems();
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

        /// <summary>
        /// 項目チェック
        /// </summary>
        /// <param name="baseItems">[NotNull] チェック基準要素</param>
        /// <param name="checkItems">[NotNull] チェック対象要素</param>
        /// <returns>チェック結果</returns>
        private static ValidationResult ValidateListItem(IEnumerable<DBItemValue> baseItems,
            IEnumerable<DBItemValue> checkItems)
        {
            var baseArr = baseItems.ToArray();
            var checkArr = checkItems.ToArray();

            if (baseArr.Length != checkArr.Length) return ValidationResult.LengthError;

            var searchError = checkArr.Where((t, i) => t.Type != baseArr[i].Type).Any();
            if (searchError)
            {
                return ValidationResult.ItemError;
            }

            return ValidationResult.OK;
        }

        /// <summary>
        /// 必要に応じて子要素通知を付け替える。<br/>
        /// Set処理用。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        private void ReattachFiledCollectionNotificationIfNeed_Set(
            int index, IFixedLengthDBItemValueList item)
        {
            if (index != 0) return;

            var oldItem = this[0];
            ReattachFiledCollectionNotification(oldItem, item);
        }

        /// <summary>
        /// 必要に応じて子要素通知を付け替える。<br/>
        /// Insertイベント用。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        private void ReattachFiledCollectionNotificationIfNeed_Insert(
            int index, IFixedLengthDBItemValueList item)
        {
            if (index != 0) return;

            // Clear() 実行時に `Count == 0` の状態でこのメソッドが呼ばれる
            var oldItem = Count > 0
                ? this[0]
                : null;

            ReattachFiledCollectionNotification(oldItem, item);
        }

        /// <summary>
        /// 必要に応じて子要素通知を付け替える。<br/>
        /// Moveイベント用。
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス</param>
        /// <param name="newIndex">移動先のインデックス</param>
        private void ReattachFiledCollectionNotificationIfNeed_Move(
            int oldIndex, int newIndex)
        {
            /*
             * oldIndex, newIndex がどちらも0ではない場合付け替え不要。
             * どちらも0の場合、実質移動が起こらないため付替不要。
             */

            if ((oldIndex == 0) == (newIndex == 0))
            {
                return;
            }

            var oldItem = this[0];
            var newItem = newIndex == 0
                ? this[oldIndex]
                : this[1];

            ReattachFiledCollectionNotification(oldItem, newItem);
        }

        /// <summary>
        /// 必要に応じて子要素通知を付け替える。<br/>
        /// Removeイベント用。
        /// </summary>
        /// <param name="index">インデックス</param>
        private void ReattachFiledCollectionNotificationIfNeed_Remove(int index)
        {
            if (index != 0) return;
            var oldItem = this[0];
            var newItem = this[1];
            ReattachFiledCollectionNotification(oldItem, newItem);
        }

        /// <summary>
        /// 必要に応じて子要素通知を付け替える。<br/>
        /// Clear処理用。
        /// </summary>
        private void ReattachFiledCollectionNotificationIfNeed_Clear()
        {
            // 付け替え先はここでは取得できないのでInsert処理に任せる
            var oldItem = this[0];
            ReattachFiledCollectionNotification(oldItem, null);
        }

        /// <summary>
        /// 子要素通知付替。
        /// </summary>
        /// <remarks>
        /// oldItem から変更通知を削除し、<br/>
        /// newItem に変更通知を付与する。
        /// </remarks>
        /// <param name="oldItem">以前0番目だった要素</param>
        /// <param name="newItem">新規に0番目となる要素</param>
        private void ReattachFiledCollectionNotification(IFixedLengthDBItemValueList oldItem,
            IFixedLengthDBItemValueList newItem)
        {
            DetachFiledCollectionNotification(oldItem);
            AttachFiledCollectionNotification(newItem);
        }

        /// <summary>
        /// 子要素通知イベントを付与する。
        /// </summary>
        /// <param name="target">対象</param>
        private void AttachFiledCollectionNotification(IFixedLengthDBItemValueList target)
        {
            if (target == null) return;

            target.PropertyChanged += OnFieldCollectionPropertyChanged;
            target.CollectionChanged += OnFieldCollectionCollectionChanged;
        }

        /// <summary>
        /// 子要素通知イベントを除去する。
        /// </summary>
        /// <param name="target">対象</param>
        private void DetachFiledCollectionNotification(IFixedLengthDBItemValueList target)
        {
            if (target == null) return;

            target.PropertyChanged -= OnFieldCollectionPropertyChanged;
            target.CollectionChanged -= OnFieldCollectionCollectionChanged;
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