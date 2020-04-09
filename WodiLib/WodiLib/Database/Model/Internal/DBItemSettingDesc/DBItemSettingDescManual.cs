// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingDescManual.cs
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
    /// データベース設定値特殊指定・選択肢手動生成
    /// </summary>
    [Serializable]
    internal class DBItemSettingDescManual : DBItemSettingDescBase, IEquatable<DBItemSettingDescManual>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 値特殊指定タイプ
        /// </summary>
        public override DBItemSpecialSettingType SettingType => DBItemSpecialSettingType.Manual;

        /// <summary>
        /// 選択肢リスト
        /// </summary>
        public override IReadOnlyDatabaseValueCaseList ArgCaseList => argCaseList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>選択肢リスト</summary>
        private DatabaseValueCaseList argCaseList = new DatabaseValueCaseList();

        /// <summary>
        /// デフォルト設定値種別
        /// </summary>
        public override DBItemType DefaultType => DBItemType.Int;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argCaseList">[Nullable] 選択肢とその文字列リスト</param>
        public DBItemSettingDescManual(DatabaseValueCaseList argCaseList = null)
        {
            if (!(argCaseList is null))
            {
                this.argCaseList = argCaseList;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 引数種別によらずすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCase> GetAllSpecialCase()
        {
            return argCaseList.ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢番号を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCaseNumber> GetAllSpecialCaseNumber()
        {
            return GetAllManualCase().Select(x => x.CaseNumber).ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// すべての選択肢文字列を取得する。
        /// </summary>
        /// <returns>すべての選択肢リスト</returns>
        public override List<DatabaseValueCaseDescription> GetAllSpecialCaseDescription()
        {
            return GetAllManualCase().Select(x => x.Description).ToList();
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCase">[NotNull] 選択肢情報</param>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        public override void AddSpecialCase(DatabaseValueCase argCase)
        {
            if (argCase is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCase)));

            argCaseList.Add(argCase);
        }

        /// <summary>
        /// 選択肢を追加する。
        /// </summary>
        /// <param name="argCases">[NotNull] 選択肢</param>
        /// <exception cref="ArgumentNullException">argCaseがnullの場合</exception>
        public override void AddRangeSpecialCase(IEnumerable<DatabaseValueCase> argCases)
        {
            if (argCases is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCases)));

            argCaseList.AddRange(argCases);
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
        /// <param name="argCase">[NotNull] 選択肢情報</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public override void InsertSpecialCase(int index, DatabaseValueCase argCase)
        {
            var max = argCaseList.Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (argCase is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCase)));

            argCaseList.Insert(index, argCase);
        }

        /// <summary>
        /// 選択肢を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength)] インデックス</param>
        /// <param name="argCases">[NotNull] 選択肢</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public override void InsertRangeSpecialCase(int index, IEnumerable<DatabaseValueCase> argCases)
        {
            var max = argCaseList.Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (argCases is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCases)));

            argCaseList.InsertRange(index, argCases);
        }

        /// <summary>
        /// 選択肢を更新する。
        /// </summary>
        /// <param name="index">[Range(0, 選択肢数-1)] 更新する選択肢</param>
        /// <param name="argCase">[NotNull] 更新する選択肢内容</param>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">argCasesがnullの場合</exception>
        public override void UpdateManualSpecialCase(int index, DatabaseValueCase argCase)
        {
            var max = argCaseList.Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));
            if (argCase is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotEmpty(nameof(argCase)));

            argCaseList[index] = argCase;
        }

        /// <summary>
        /// 選択肢を削除する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public override void RemoveSpecialCaseAt(int index)
        {
            var max = argCaseList.Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            argCaseList.RemoveAt(index);
        }

        /// <summary>
        /// 選択肢を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, ManualCaseLength - 1)] インデックス</param>
        /// <param name="count">[Range(0, ManualCaseLength)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index、またはcountが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">リストの範囲を超えて削除しようとする場合</exception>
        public override void RemoveSpecialCaseRange(int index, int count)
        {
            var allLength = argCaseList.Count;

            var indexMax = allLength - 1;
            const int min = 0;
            if (index < min || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, indexMax, index));
            var countMax = allLength;
            if (count < min || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, countMax, count));

            const int listLengthMin = 0;
            if (allLength - index < count + listLengthMin)
                throw new ArgumentException(
                    $"リストの範囲を超えて削除しようとしています。" +
                    $"{nameof(index)}:{index}, {nameof(count)}:{count}");

            argCaseList.RemoveRange(index, count);
        }

        /// <summary>
        /// 選択肢をクリアする。
        /// </summary>
        /// <exception cref="InvalidOperationException">特殊指定が「手動生成」以外の場合</exception>
        public override void ClearSpecialCase()
        {
            argCaseList.Clear();
        }

        /// <summary>
        /// 指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">[NotNull] 値種別</param>
        /// <returns>設定可能な場合true</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        public override bool CanSetItemType(DBItemType type)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            return type == DBItemType.Int;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(IDBItemSettingDesc other)
        {
            if (ReferenceEquals(null, this)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is DBItemSettingDescManual casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DBItemSettingDescBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(other is DBItemSettingDescManual casted)) return false;

            return Equals(casted);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemSettingDescManual other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return argCaseList.Equals(other.argCaseList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢手動生成時のすべての選択肢を取得する。
        /// </summary>
        /// <returns>すべての選択肢情報</returns>
        /// <exception cref="InvalidOperationException">特殊指定が「選択肢手動生成」以外の場合</exception>
        private IEnumerable<DatabaseValueCase> GetAllManualCase()
        {
            return argCaseList.ToList();
        }
    }
}