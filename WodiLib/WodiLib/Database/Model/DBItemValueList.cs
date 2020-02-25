// ========================================
// Project Name : WodiLib
// File Name    : DBItemValueList.cs
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
    /// DBデータ設定値リスト
    /// </summary>
    [Serializable]
    public class DBItemValueList : RestrictedCapacityCollection<DBItemValue>,
        IFixedLengthDBItemValueList, IReadOnlyDBItemValueList, IEquatable<DBItemValueList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => 100;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        [NonSerialized] private DBItemValuesList outer;

        /// <summary>外部クラス</summary>
        internal DBItemValuesList Outer
        {
            get => outer;
            private set => outer = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemValueList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">[NotNull] 初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        public DBItemValueList(IReadOnlyCollection<DBItemValue> list) : base(list)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outer">[NotNull] 外部クラス</param>
        /// <exception cref="ArgumentNullException">outerがnullの場合</exception>
        internal DBItemValueList(DBItemValuesList outer)
        {
            if (outer is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outer)));

            Outer = outer;

            // イニシャライズでItems.Count = 0 の状態で到達する可能性がある
            if (outer.Count == 0) return;

            var defaultValues = outer[0]
                .Select(x => x.Type.DBItemDefaultValue)
                .ToList();
            Items.AddRange(defaultValues);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outer">[NotNull] 外部クラス</param>
        /// <param name="list">[NotNull] 値リスト</param>
        /// <exception cref="ArgumentNullException">outer, listがnullの場合</exception>
        /// <exception cref="ArgumentException">
        ///     listの要素数、
        ///     またはlist中の値種別が不適切な場合
        /// </exception>
        internal DBItemValueList(DBItemValuesList outer,
            IReadOnlyCollection<DBItemValue> list)
        {
            if (outer is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outer)));
            if (list is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(list)));

            Outer = outer;

            // validationのためにここで追加する。validation後には追加しない。
            Items.AddRange(list);

            var validateResult = outer.ValidateListItem(this);
            switch (validateResult)
            {
                case DBItemValuesList.ValidationResult.LengthError:
                    throw new ArgumentException(
                        $"{nameof(list)}の要素数が異なります。");
                case DBItemValuesList.ValidationResult.ItemError:
                    throw new ArgumentException(
                        $"{nameof(list)}中に種類の異なる項目があります。");
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// 長さ固定リストに変換する。
        /// </summary>
        /// <returns>インスタンス</returns>
        public IFixedLengthDBItemValueList ToFixedLengthList() => this;

        /// <summary>
        /// 容量変更可能なDBデータ設定値リストに変換する。
        /// </summary>
        /// <returns>DBデータ設定値リスト</returns>
        public DBItemValueList ToLengthChangeableItemValueList()
        {
            // Outerを解除した、自身と同じ項目を持つ別インスタンスを返す
            var result = new DBItemValueList(Items);
            return result;
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

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void Add(DBItemValue item)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.Add(item);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void AddRange(IReadOnlyCollection<DBItemValue> items)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.AddRange(items);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="item">[NotNull] 挿入する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void Insert(int index, DBItemValue item)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.Insert(index, item);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void InsertRange(int index, IReadOnlyCollection<DBItemValue> items)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.InsertRange(index, items);
        }

        /// <summary>
        /// 特定のオブジェクトを要素として持つとき、最初に出現したものを削除する。
        /// </summary>
        /// <param name="item">[Nullable] 削除する要素</param>
        /// <returns>削除成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または削除した結果要素数がMinValue未満になる場合
        /// </exception>
        public new bool Remove(DBItemValue item)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            return base.Remove(item);
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または削除した結果要素数がMinValue未満になる場合
        /// </exception>
        public new void RemoveAt(int index)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.RemoveAt(index);
        }

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     削除した結果要素数がMinValue未満になる場合
        /// </exception>
        public new void RemoveRange(int index, int count)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.RemoveRange(index, count);
        }

        /// <summary>
        /// 要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(GetMinCapacity(), GetMaxCapacity())] 調整する要素数</param>
        /// <exception cref="InvalidOperationException">lengthが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">要素を追加した際にnullがセットされた場合</exception>
        /// <exception cref="InvalidOperationException">紐付けされているDBItemValuesListが存在する場合</exception>
        public new void AdjustLength(int length)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.AdjustLength(length);
        }

        /// <summary>
        /// すべての要素を削除し、最小の要素数だけ初期化する。
        /// </summary>
        /// <exception cref="InvalidOperationException">紐付けされているDBItemValuesListが存在する場合</exception>
        public new void Clear()
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.Clear();
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemValueList other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals((RestrictedCapacityCollection<DBItemValue>) other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IFixedLengthDBItemValueList other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is RestrictedCapacityCollection<DBItemValue> casted)) return false;
            return Equals(casted);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBItemValuesList に紐づけする。
        /// </summary>
        /// <param name="outer">[NotNull] 紐付け対象クラス</param>
        /// <exception cref="InvalidOperationException">既に紐付けされている場合</exception>
        /// <exception cref="ArgumentNullException">outerがnullの場合</exception>
        internal void Attach(DBItemValuesList outer)
        {
            if (!(Outer is null))
                throw new InvalidOperationException(
                    "既に紐付けされているため、処理できません。");
            if (outer is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outer)));

            outer.AddNewRelationshipInstance(this);

            Outer = outer;
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        internal void AddForValuesListInstanceManager(DBItemValue item)
        {
            base.Add(item);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        internal void AddRangeForValuesListInstanceManager(IReadOnlyCollection<DBItemValue> items)
        {
            base.AddRange(items);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="item">[NotNull] 挿入する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        internal void InsertForValuesListInstanceManager(int index, DBItemValue item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">[NotNull] 追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        internal void InsertRangeForValuesListInstanceManager(int index,
            IReadOnlyCollection<DBItemValue> items)
        {
            base.InsertRange(index, items);
        }

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または削除した結果要素数がMinValue未満になる場合
        /// </exception>
        internal void RemoveAtForValuesListInstanceManager(int index)
        {
            base.RemoveAt(index);
        }

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     削除した結果要素数がMinValue未満になる場合
        /// </exception>
        internal void RemoveRangeForValuesListInstanceManager(int index, int count)
        {
            base.RemoveRange(index, count);
        }

        /// <summary>
        /// すべての要素を削除し、最小の要素数だけ初期化する。
        /// </summary>
        /// <exception cref="InvalidOperationException">紐付けされているDBItemValuesListが存在する場合</exception>
        internal void ClearForValuesListInstanceManager()
        {
            base.Clear();
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
        protected override DBItemValue MakeDefaultItem(int index) => new DBItemValue(0);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // 数値項目
            Items.Where(x => x.Type == DBItemType.Int).ToList()
                .ForEach(x => result.AddRange(x.ToBinary()));

            // 文字列項目
            Items.Where(x => x.Type == DBItemType.String).ToList()
                .ForEach(x => result.AddRange(x.ToBinary()));

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
        protected DBItemValueList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}