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
    public class DBItemValuesList : RestrictedCapacityCollection<IFixedLengthDBItemValueList>,
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
                if (_fieldCollectionChanged.GetInvocationList().Contains(value)) return;
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
                if (_fieldPropertyChanged.GetInvocationList().Contains(value)) return;
                _fieldPropertyChanged += value;
            }
            remove => _fieldPropertyChanged -= value;
        }

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
        /// <param name="initItems">初期要素</param>
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
            var max = GetMaxCapacity();
            var addedLength = Count + 1;
            if (addedLength > max)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(max));

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
            if (addedLength > max)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(max));

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

            var maxCapacity = GetMaxCapacity();
            var addedLength = Count + 1;
            if (addedLength > maxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(maxCapacity));

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
            const int indexMin = 0;
            if (index < indexMin || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), indexMin, indexMax, index));

            var countMax = GetMaxCapacity();
            const int countMin = 0;
            if (count < countMin || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), countMin, countMax, count));

            var lengthMax = GetMaxCapacity();
            var addedLength = Count + count;
            if (addedLength > lengthMax)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(lengthMax));

            var valuesList = new List<IFixedLengthDBItemValueList>();
            for (var i = 0; i < count; i++)
            {
                valuesList.Add(CreateValueListInstance());
            }

            InsertRange(index, valuesList);
        }

        /// <summary>
        /// 自身と同じDB値型配列を持つDB値リストのインスタンスを生成する。
        /// 値リスト中の値は全て初期化された状態で生成される。
        /// </summary>
        /// <returns>DB値リストインスタンス</returns>
        public DBItemValueList CreateValueListInstance()
        {
            var instance = new DBItemValueList(this);
            return instance;
        }

        /// <summary>
        /// 自身と同じDB値型配列を持つDB値リストのインスタンスを生成する。
        /// 値リスト中の値はvaluesで初期化される。
        /// </summary>
        /// <param name="values">初期リスト</param>
        /// <returns>DB値リストインスタンス</returns>
        /// <exception cref="ArgumentNullException">valuesがnullの場合</exception>
        /// <exception cref="ArgumentException">
        ///     valuesの要素数、
        ///     またはvalues中の値種別が不適切な場合
        /// </exception>
        public DBItemValueList CreateValueListInstance(
            IEnumerable<DBItemValue> values)
        {
            if (values == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(values)));

            var instance = new DBItemValueList(values);

            var validateResult = ValidateListItem(instance);
            switch (validateResult)
            {
                case ValidationResult.LengthError:
                    throw new ArgumentException(
                        $"{nameof(values)}の要素数が異なります。");
                case ValidationResult.ItemError:
                    throw new ArgumentException(
                        $"{nameof(values)}中に種類の異なる項目があります。");
            }

            return instance;
        }

        /// <summary>
        /// 指定した項目の値型を更新する。
        /// 更新された項目は、
        /// <list type="bullet">
        ///     <item><term>値種別が変化した場合はデフォルト値で初期化される。</term></item>
        ///     <item><term>値種別が変化しなかった場合は値の変更は起こらない。ただしPropertyChangedイベントは発火する。</term></item>
        /// </list>
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1] 項目ID</param>
        /// <param name="type">値種別</param>
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

            ReflectItemType(itemId, type);
        }

        /// <summary>
        /// 指定した項目IDに値型を反映する。
        /// <paramref name="itemId"/> および <paramref name="type"/> は適切であることが前提。
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="type">値型</param>
        private void ReflectItemType(ItemId itemId, DBItemType type)
        {
            // タイプ変化チェックメソッド 後のロジックのため匿名関数にする
            Func<IFixedLengthDBItemValueList, bool>? typeCheckFunc = target => target[itemId].Type == type;
            var isSameType = false;

            int idx = itemId;
            var value = type.DBItemDefaultValue;
            this.ForEach(target =>
            {
                // タイプ変化チェックは最初の1回だけ行えばいい
                //   要素変更イベントを発火するため、タイプ変化していない場合でも要素を上書きする。
                if (typeCheckFunc != null)
                {
                    isSameType = typeCheckFunc.Invoke(target);
                    typeCheckFunc = null;
                }

                target[idx] = isSameType
                    ? target[idx]
                    : value;
            });
        }

        /// <summary>
        /// 項目IDについて、指定した値で更新する。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1] 項目ID</param>
        /// <param name="value">値</param>
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

            this.ForEach(target => target[itemId] = value);
        }

        /// <summary>
        /// 指定した項目の値型を更新する。
        /// 更新された項目は、
        /// <list type="bullet"></list>
        ///     <item><term>値種別が変化した場合はデフォルト値で初期化される。</term></item>
        ///     <item><term>値種別が変化しなかった場合は値の変更は起こらない。ただしPropertyChangedイベントは発火する。</term></item>
        /// </summary>
        /// <param name="funcGetType">[NotNull] 更新型生成関数</param>
        /// <exception cref="ArgumentNullException">funcGetTypeがnullの場合</exception>
        public void SetField(Func<ItemId, DBItemType> funcGetType)
        {
            if (funcGetType is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetType)));

            var fieldLength = this[0].Count;

            for (var i = 0; i < fieldLength; i++)
            {
                var itemId = (ItemId) i;
                var type = funcGetType(itemId);
                ReflectItemType(itemId, type);
            }
        }

        /// <summary>
        /// すべてのデータの指定した項目IDについて、値を更新する。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1] 項目ID</param>
        /// <param name="funcGetValue">[NotNull] 更新値生成関数</param>
        /// <exception cref="ArgumentOutOfRangeException">itemIdが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">funcGetValueがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     項目数がDBItemValueList.MaxCapacityを超える場合、
        ///     またはfuncGetValueがnullを返却した場合、
        ///     またはfuncGetValueでdataId>=1のときに返却した値型がdataId=0のときに返却した値型と異なる場合
        /// </exception>
        public void SetField(ItemId itemId, Func<DataId, DBItemValue> funcGetValue)
        {
            var max = Items[0].Count - 1;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (funcGetValue is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetValue)));

            DBItemType? firstType = null;
            this.ForEach((target, dataId) =>
            {
                var value = funcGetValue(dataId);
                if (value is null)
                    throw new InvalidOperationException(ErrorMessage.NotNull($"{nameof(funcGetValue)}の返却値"));

                if (dataId == 0)
                {
                    firstType = value.Type;
                }
                else if (firstType != value.Type)
                    throw new InvalidOperationException(
                        ErrorMessage.NotExecute("データ0の値とは異なるデータ型の値を設定しようとしたため"));

                target[itemId] = funcGetValue(dataId);
            });
        }

        /// <summary>
        /// 項目を追加する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="type">値種別</param>
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

            this.ForEach(target => ((IChildDBItemValueList) target).Add(type.DBItemDefaultValue));
        }

        /// <summary>
        /// 項目を追加する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="value">値</param>
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

            this.ForEach(target => ((IChildDBItemValueList) target).Add(value));
        }

        /// <summary>
        /// 項目を追加する。
        /// 追加された項目はfuncGetValueが返却した値によって初期化される。
        /// </summary>
        /// <param name="funcGetValue">項目初期化値取得関数</param>
        /// <exception cref="ArgumentNullException">funcGetValueがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     項目数がDBItemValueList.MaxCapacityを超える場合、
        ///     またはfuncGetValueがnullを返却した場合、
        ///     またはfuncGetValueでdataId>=1のときに返却した値型がdataId=0のときに返却した値型と異なる場合
        /// </exception>
        public void AddField(Func<DataId, DBItemValue> funcGetValue)
        {
            if (funcGetValue == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetValue)));

            var addedLength = Items[0].Count + 1;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            DBItemType? firstType = null;
            this.ForEach((target, dataId) =>
            {
                var value = funcGetValue(dataId);
                if (value is null)
                    throw new InvalidOperationException(ErrorMessage.NotNull($"{nameof(funcGetValue)}の返却値"));

                if (dataId == 0)
                {
                    firstType = value.Type;
                }
                else if (firstType != value.Type)
                    throw new InvalidOperationException(
                        ErrorMessage.NotExecute("データ0の値とは異なるデータ型の値を設定しようとしたため"));

                ((IChildDBItemValueList) target).Add(value);
            });
        }

        /// <summary>
        /// 項目を追加する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="types">値種別リスト</param>
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

            var values = typeArr.Select(type => type.DBItemDefaultValue);
            this.ForEach(target => ((IChildDBItemValueList) target).AddRange(values));
        }

        /// <summary>
        /// 項目を追加する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="values">値</param>
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

            this.ForEach(target => ((IChildDBItemValueList) target).AddRange(valueArr));
        }

        /// <summary>
        /// 項目を追加する。
        /// 追加された項目はfuncGetValuesが返却した値で初期化される。
        /// </summary>
        /// <param name="funcGetValues">項目初期化値取得関数</param>
        /// <exception cref="ArgumentNullException">funcGetValuesがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     項目数がDBItemValueList.MaxCapacityを超える場合、
        ///     またはfuncGetValuesがnullを返却した場合、
        ///     またはまたはfuncGetValuesの返却値にnullが含まれる場合、
        ///     またはfuncGetValuesでdataId>=1のときに返却した値型がdataId=0のときに返却した値型と異なる場合
        /// </exception>
        public void AddFieldRange(Func<DataId, IEnumerable<DBItemValue>> funcGetValues)
        {
            if (funcGetValues == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetValues)));

            var firstTypes = new List<DBItemType>();
            this.ForEach((target, dataId) =>
            {
                var retValue = funcGetValues(dataId);
                if (retValue is null)
                    throw new InvalidOperationException(ErrorMessage.NotNull($"{nameof(funcGetValues)}の返却値"));
                var values = retValue.ToList();
                if (values.HasNullItem())
                    throw new InvalidOperationException(ErrorMessage.NotNullInList($"{nameof(funcGetValues)}の返却値"));

                if (dataId == 0)
                {
                    firstTypes = values.Select(x => x.Type).ToList();

                    var addedLength = Items[0].Count + firstTypes.Count;
                    if (addedLength > DBItemValueList.MaxCapacity)
                        throw new InvalidOperationException(
                            ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));
                }
                else if (!firstTypes.SequenceEqual(values.Select(x => x.Type).ToList()))
                    throw new InvalidOperationException(
                        ErrorMessage.NotExecute("データ0の値とは異なるデータ型の値を設定しようとしたため"));

                ((IChildDBItemValueList) target).AddRange(values);
            });
        }

        /// <summary>
        /// 項目を挿入する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count)] 項目ID</param>
        /// <param name="type">値種別</param>
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

            this.ForEach(target => ((IChildDBItemValueList) target).Insert(itemId, type.DBItemDefaultValue));
        }

        /// <summary>
        /// 項目を挿入する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count)] インデックス</param>
        /// <param name="value">値</param>
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

            this.ForEach(target => ((IChildDBItemValueList) target).Insert(itemId, value));
        }

        /// <summary>
        /// 項目を挿入する。
        /// 挿入された項目はfuncGetValueが返却した値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count)] インデックス</param>
        /// <param name="funcGetValue">項目初期化値取得関数</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">funcGetValuesがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     項目数がDBItemValueList.MaxCapacityを超える場合、
        ///     またはfuncGetValuesがnullを返却した場合、
        ///     またはfuncGetValuesでdataId>=1のときに返却した値型がdataId=0のときに返却した値型と異なる場合
        /// </exception>
        public void InsertField(ItemId itemId, Func<DataId, DBItemValue> funcGetValue)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (funcGetValue == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetValue)));

            DBItemType firstType = default!;
            this.ForEach((target, dataId) =>
            {
                var value = funcGetValue(dataId);
                if (value is null)
                    throw new InvalidOperationException(ErrorMessage.NotNull($"{nameof(funcGetValue)}の返却値"));

                if (dataId == 0)
                {
                    firstType = value.Type;
                }
                else if (firstType != value.Type)
                    throw new InvalidOperationException(
                        ErrorMessage.NotExecute("データ0の値とは異なるデータ型の値を設定しようとしたため"));

                ((IChildDBItemValueList) target).Insert(itemId, value);
            });
        }

        /// <summary>
        /// 項目を挿入する。
        /// 追加された項目はデフォルト値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count ～ ItemId.MaxValue)] 項目ID</param>
        /// <param name="types">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">itemIdが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     typesがnullの場合、
        ///     またはtypesにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void InsertFieldRange(ItemId itemId, IEnumerable<DBItemType> types)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (types is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(types)));

            var typeArr = types.ToArray();

            if (typeArr.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(types)));

            var addedLength = Items[0].Count + typeArr.Length;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            this.ForEach(target =>
                ((IChildDBItemValueList) target).InsertRange(itemId, typeArr.Select(type => type.DBItemDefaultValue)));
        }

        /// <summary>
        /// 項目を挿入する。
        /// 追加された項目は引数で指定された値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count ～ ItemId.MaxValue)] 項目ID</param>
        /// <param name="values">追加する値</param>
        /// <exception cref="ArgumentOutOfRangeException">itemIdが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     typesがnullの場合、
        ///     またはtypesにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がDBItemValueList.MaxCapacityを超える場合</exception>
        public void InsertFieldRange(ItemId itemId, IEnumerable<DBItemValue> values)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (values is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(values)));

            var valueArr = values.ToArray();

            if (valueArr.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(values)));

            var addedLength = Items[0].Count + valueArr.Length;
            if (addedLength > DBItemValueList.MaxCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));

            this.ForEach(target => ((IChildDBItemValueList) target).InsertRange(itemId, valueArr));
        }

        /// <summary>
        /// 項目を挿入する。
        /// 追加された項目はfuncGetValuesが返却した値で初期化される。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count ～ ItemId.MaxValue)] インデックス</param>
        /// <param name="funcGetValues">項目初期化値取得関数</param>
        /// <exception cref="ArgumentNullException">funcGetValuesがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     項目数がDBItemValueList.MaxCapacityを超える場合、
        ///     またはfuncGetValuesがnullを返却した場合、
        ///     またはまたはfuncGetValuesの返却値にnullが含まれる場合、
        ///     またはfuncGetValuesでdataId>=1のときに返却した値型がdataId=0のときに返却した値型と異なる場合
        /// </exception>
        public void InsertFieldRange(ItemId itemId, Func<DataId, IEnumerable<DBItemValue>> funcGetValues)
        {
            var max = Items[0].Count;
            const int min = 0;
            if (itemId < min || max < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, max, itemId));

            if (funcGetValues == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetValues)));

            var firstTypes = new List<DBItemType>();
            this.ForEach((target, dataId) =>
            {
                var values = funcGetValues(dataId).ToList();
                if (values.HasNullItem())
                    throw new InvalidOperationException(ErrorMessage.NotNullInList($"{nameof(funcGetValues)}の返却値"));

                if (dataId == 0)
                {
                    firstTypes = values.Select(x => x.Type).ToList();

                    var addedLength = Items[0].Count + firstTypes.Count;
                    if (addedLength > DBItemValueList.MaxCapacity)
                        throw new InvalidOperationException(
                            ErrorMessage.OverListLength(DBItemValueList.MaxCapacity));
                }
                else if (!firstTypes.SequenceEqual(values.Select(x => x.Type).ToList()))
                    throw new InvalidOperationException(
                        ErrorMessage.NotExecute("データ0の値とは異なるデータ型の値を設定しようとしたため"));

                ((IChildDBItemValueList) target).InsertRange(itemId, values);
            });
        }

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldItemId">[Range(0, Items[0].Count - 1)] 移動する項目ID</param>
        /// <param name="newItemId">[Range(0, Items[0].Count - 1)] 移動先の項目ID</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldItemId, newItemId が指定範囲外の場合
        /// </exception>
        public void MoveField(ItemId oldItemId, ItemId newItemId)
        {
            if (Items[0].Count == 0)
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("リストの要素が0個のため"));

            var max = Items[0].Count - 1;
            const int min = 0;
            if (oldItemId < min || max < oldItemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(oldItemId), min, max, oldItemId));
            if (newItemId < min || max < newItemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(newItemId), min, max, newItemId));

            this.ForEach(target => ((IChildDBItemValueList) target).Move(oldItemId, newItemId));
        }

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldItemId">[Range(0, Items[0].Count - 1)] 移動する項目ID開始位置</param>
        /// <param name="newItemId">[Range(0, Items[0].Count - count)] 移動先の項目ID開始位置</param>
        /// <param name="count">[Range(0, Items[0].Count - oldItemId)] 移動させる項目数</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldItemId, newItemId, count が指定範囲外の場合
        /// </exception>
        public void MoveFieldRange(ItemId oldItemId, ItemId newItemId, int count)
        {
            if (Items[0].Count == 0)
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("リストの要素が0個のため"));

            const int min = 0;
            var oldIndexMax = Items[0].Count - 1;
            if (oldItemId < min || oldIndexMax < oldItemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(oldItemId), min, oldIndexMax, oldItemId));
            var lengthMax = Items[0].Count - oldItemId;
            if (count < min || lengthMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(newItemId), min, lengthMax, newItemId));
            var newIndexMax = Items[0].Count - count;
            if (newItemId < min || newIndexMax < newItemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(newItemId), min, newIndexMax, newItemId));

            this.ForEach(target => ((IChildDBItemValueList) target).MoveRange(oldItemId, newItemId, count));
        }

        /// <summary>
        /// 指定した項目を削除する。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">itemIdが指定範囲外の場合</exception>
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

            this.ForEach(target => ((IChildDBItemValueList) target).RemoveAt(itemId));
        }

        /// <summary>
        /// 指定したIDを起点として複数の項目を削除する。
        /// </summary>
        /// <param name="itemId">[Range(0, Items[0].Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Items[0].Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">itemId, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がDBItemValueList.MinValue未満になる場合</exception>
        public void RemoveFieldRange(ItemId itemId, int count)
        {
            var indexMax = Items[0].Count - 1;
            const int min = 0;

            if (itemId < min || indexMax < itemId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(itemId), min, indexMax, itemId));

            var countMax = Items[0].Count;

            if (count < min || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, countMax, count));

            if (Items[0].Count - itemId < count)
                throw new ArgumentException(
                    $"{nameof(itemId)}および{nameof(count)}が有効な範囲を示していません。");

            var removedLength = Items[0].Count - count;
            if (removedLength < DBItemValueList.MinCapacity)
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(GetMinCapacity()));

            this.ForEach(target => ((IChildDBItemValueList) target).RemoveRange(itemId, count));
        }

        /// <summary>
        /// すべての項目を削除する。
        /// </summary>
        public void ClearField()
        {
            this.ForEach(target => ((IChildDBItemValueList) target).Clear());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目チェック
        /// </summary>
        /// <param name="checkList">チェック対象リスト</param>
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
            if (writableItem.HasRelationship)
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable(nameof(item),
                        $"既に他の{nameof(DBItemValuesList)}に紐付けられているためセットできません。"));
            }

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

            writableItem.HasRelationship = true;

            ReattachFiledCollectionNotificationIfNeed_Set(index, item);
            base.SetItem(index, item);
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
            if (writableItem.HasRelationship)
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable(nameof(item),
                        $"既に他の{nameof(DBItemValuesList)}に紐付けられているため追加できません。"));
            }

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

            writableItem.HasRelationship = true;

            ReattachFiledCollectionNotificationIfNeed_Insert(index, item);
            base.InsertItem(index, item);
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
            var writableItem = (DBItemValueList) this[index];
            writableItem.HasRelationship = false;
            ReattachFiledCollectionNotificationIfNeed_Remove(index);
            base.RemoveItem(index);
        }

        /// <summary>
        /// 要素をすべて除去する。
        /// </summary>
        protected override void ClearItems()
        {
            this.ForEach(item =>
            {
                var writableItem = (DBItemValueList) item;
                writableItem.HasRelationship = false;
            });
            ReattachFiledCollectionNotificationIfNeed_Clear();
            base.ClearItems();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目チェック
        /// </summary>
        /// <param name="baseItems">チェック基準要素</param>
        /// <param name="checkItems">チェック対象要素</param>
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
        private void ReattachFiledCollectionNotification(IFixedLengthDBItemValueList? oldItem,
            IFixedLengthDBItemValueList? newItem)
        {
            DetachFiledCollectionNotification(oldItem);
            AttachFiledCollectionNotification(newItem);
        }

        /// <summary>
        /// 子要素通知イベントを付与する。
        /// </summary>
        /// <param name="target">対象</param>
        private void AttachFiledCollectionNotification(IFixedLengthDBItemValueList? target)
        {
            if (target == null) return;

            target.PropertyChanged += OnFieldCollectionPropertyChanged;
            target.CollectionChanged += OnFieldCollectionCollectionChanged;
        }

        /// <summary>
        /// 子要素通知イベントを除去する。
        /// </summary>
        /// <param name="target">対象</param>
        private void DetachFiledCollectionNotification(IFixedLengthDBItemValueList? target)
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
