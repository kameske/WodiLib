// ========================================
// Project Name : WodiLib
// File Name    : DBItemValueList.cs
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
    /// DBデータ設定値リスト
    /// </summary>
    [Serializable]
    public class DBItemValueList : RestrictedCapacityList<DBItemValue, DBItemValueList>,
        IFixedLengthDBItemValueList, IReadOnlyDBItemValueList, IEquatable<DBItemValueList>,
        IChildDBItemValueList
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

        /// <summary>DBItemValuesListクラスのインスタンスとの関連性有無</summary>
        [field: NonSerialized]
        internal bool HasRelationship { get; set; }

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
        /// <param name="items">初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        public DBItemValueList(IEnumerable<DBItemValue> items) : base(items)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outer">外部クラス</param>
        /// <exception cref="ArgumentNullException">outerがnullの場合</exception>
        internal DBItemValueList(DBItemValuesList outer)
        {
            if (outer is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outer)));

            // イニシャライズでItems.Count = 0 の状態で到達する可能性がある
            if (outer.Count == 0) return;

            var defaultValues = outer[0]
                .Select(x => x.Type.DBItemDefaultValue)
                .ToList();
            AddRange(defaultValues);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outer">外部クラス</param>
        /// <param name="items">値列挙</param>
        /// <exception cref="ArgumentNullException">outer, itemsがnullの場合</exception>
        /// <exception cref="ArgumentException">
        ///     itemsの要素数、
        ///     またはitems中の値種別が不適切な場合
        /// </exception>
        internal DBItemValueList(DBItemValuesList outer,
            IEnumerable<DBItemValue> items)
        {
            if (outer is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outer)));
            if (items is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(items)));

            // validationのためにここで追加する。validation後には追加しない。
            this.AddRange(items);

            DBItemValuesListValidationHelper.ValidateListItem(outer, this);
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
        /// 自身と同じ値情報を持つ、DBItemValuesList に紐付けられていないインスタンスに変換する。
        /// </summary>
        /// <returns>DBデータ設定値リスト</returns>
        public DBItemValueList ToLengthChangeableItemValueList()
        {
            // Outerを解除した、自身と同じ項目を持つ別インスタンスを返す
            var result = new DBItemValueList(this);
            return result;
        }

        /// <summary>
        /// 容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public int GetCapacity()
        {
            /*
             * IFixedLengthDBItemValueListとして扱う際に必要になる
             */
            return Count;
        }

        IFixedLengthList<DBItemValue> IFixedLengthList<DBItemValue>.DeepCloneWith(
            IEnumerable<KeyValuePair<int, DBItemValue>>? values)
        {
            throw new NotImplementedException();
        }

        IFixedLengthList<DBItemValue> IFixedLengthList<DBItemValue>.DeepCloneWith(int? length,
            IEnumerable<KeyValuePair<int, DBItemValue>>? values)
        {
            throw new NotImplementedException();
        }

        IReadOnlyFixedLengthList<DBItemValue> IReadOnlyFixedLengthList<DBItemValue>.DeepCloneWith(
            IEnumerable<KeyValuePair<int, DBItemValue>>? values)
        {
            throw new NotImplementedException();
        }

        IReadOnlyFixedLengthList<DBItemValue> IReadOnlyFixedLengthList<DBItemValue>.DeepCloneWith(int? length,
            IEnumerable<KeyValuePair<int, DBItemValue>>? values)
        {
            throw new NotImplementedException();
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
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void Add(DBItemValue item)
        {
            if (HasRelationship)
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.Add(item);
        }

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void AddRange(IEnumerable<DBItemValue> items)
        {
            if (HasRelationship)
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.AddRange(items);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="item">挿入する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void Insert(int index, DBItemValue item)
        {
            if (HasRelationship)
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.Insert(index, item);
        }

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または要素数がMaxCapacityを超える場合
        /// </exception>
        public new void InsertRange(int index, IEnumerable<DBItemValue> items)
        {
            if (HasRelationship)
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.InsertRange(index, items);
        }

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, Count - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex が指定範囲外の場合
        /// </exception>
        public new void Move(int oldIndex, int newIndex)
        {
            if (HasRelationship)
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.Move(oldIndex, newIndex);
        }


        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, Count - count)] 移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex, count が指定範囲外の場合
        /// </exception>
        public new void MoveRange(int oldIndex, int newIndex, int count)
        {
            if (HasRelationship)
                throw new InvalidOperationException(
                    $"{nameof(DBItemValuesList)}に紐付けされているため、個別の操作はできません。" +
                    $"紐付けされている{nameof(DBItemValuesList)}を通じて操作してください。");

            base.MoveRange(oldIndex, newIndex, count);
        }

        /// <summary>
        /// 特定のオブジェクトを要素として持つとき、最初に出現したものを削除する。
        /// </summary>
        /// <param name="item">削除する要素</param>
        /// <returns>削除成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     紐付けされているDBItemValuesListが存在する場合、
        ///     または削除した結果要素数がMinValue未満になる場合
        /// </exception>
        public new bool Remove(DBItemValue? item)
        {
            if (HasRelationship)
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
            if (HasRelationship)
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
            if (HasRelationship)
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
            if (HasRelationship)
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
            if (HasRelationship)
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
        public bool Equals(DBItemValueList? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ItemEquals(other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(IFixedLengthDBItemValueList? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals((IReadOnlyFixedLengthList<DBItemValue>) other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
#pragma warning disable 618 // TODO Ver 2.6 まで
        public bool ItemEquals(IReadOnlyFixedLengthList<DBItemValue>? other)
#pragma warning restore 618
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public bool ItemEquals(IFixedLengthList<DBItemValue>? other)
            => Equals((IEnumerable<DBItemValue>?) other);

        /// <summary>
        /// 自身と同じ型情報を持ち、すべての項目がデフォルト値で初期化された新規インスタンスを生成する。
        /// </summary>
        /// <returns>DBItemValueList インスタンス</returns>
        public DBItemValueList CreateDefaultValueListInstance()
            => new DBItemValueList(
                this.Select(x => x.GetDefaultValue())
            );

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region IChildDBItemValueList

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        void IChildDBItemValueList.Add(DBItemValue item)
            => base.Add(item);

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        void IChildDBItemValueList.AddRange(IEnumerable<DBItemValue> items)
            => base.AddRange(items);

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="item">挿入する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        void IChildDBItemValueList.Insert(int index, DBItemValue item)
            => base.Insert(index, item);

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">要素数がMaxCapacityを超える場合</exception>
        void IChildDBItemValueList.InsertRange(int index, IEnumerable<DBItemValue> items)
            => base.InsertRange(index, items);

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, Count - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex が指定範囲外の場合
        /// </exception>
        void IChildDBItemValueList.Move(int oldIndex, int newIndex)
            => base.Move(oldIndex, newIndex);

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, Count - count)] 移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex, count が指定範囲外の場合
        /// </exception>
        void IChildDBItemValueList.MoveRange(int oldIndex, int newIndex, int count)
            => base.MoveRange(oldIndex, newIndex, count);

        /// <summary>
        /// 指定したインデックスにある要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinCapacity未満になる場合</exception>
        void IChildDBItemValueList.RemoveAt(int index)
            => base.RemoveAt(index);

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinCapacity未満になる場合</exception>
        void IChildDBItemValueList.RemoveRange(int index, int count)
            => base.RemoveRange(index, count);

        /// <summary>
        /// すべての要素を削除し、最小の要素数だけ初期化する。
        /// </summary>
        void IChildDBItemValueList.Clear()
            => base.Clear();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override DBItemValue MakeDefaultItem(int index) =>
            Count > 0
                ? this[index].Type.DBItemDefaultValue
                : DBItemType.Int.DBItemDefaultValue;

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
            this.Where(x => x.Type == DBItemType.Int).ToList()
                .ForEach(x => result.AddRange(x.ToBinary()));

            // 文字列項目
            this.Where(x => x.Type == DBItemType.String).ToList()
                .ForEach(x => result.AddRange(x.ToBinary()));

            return result.ToArray();
        }

        IReadOnlyFixedLengthList<DBItemValue> IDeepCloneable<IReadOnlyFixedLengthList<DBItemValue>>.DeepClone()
        {
            throw new NotImplementedException();
        }

        IFixedLengthList<DBItemValue> IDeepCloneable<IFixedLengthList<DBItemValue>>.DeepClone()
        {
            throw new NotImplementedException();
        }
    }
}
