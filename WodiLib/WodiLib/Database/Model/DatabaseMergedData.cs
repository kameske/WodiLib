// ========================================
// Project Name : WodiLib
// File Name    : DatabaseMergedData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// datファイルとprojectファイルの内容をマージしたデータ
    /// </summary>
    [Serializable]
    public class DatabaseMergedData : IEquatable<DatabaseMergedData>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBタイプ情報リスト
        /// </summary>
        public DatabaseTypeDescList TypeDescList { get; } = new DatabaseTypeDescList();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseMergedData()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="typeSettingList">[NotNull] タイプ設定リスト</param>
        /// <param name="dataSettingList">[NotNull] データ設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     typeSettingList, dataSettingList が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     typeSettingListとdataSettingListの要素数が異なる場合。
        /// </exception>
        public DatabaseMergedData(DBTypeSettingList typeSettingList,
            DBDataSettingList dataSettingList)
        {
            InitializeTypeDescList(typeSettingList, dataSettingList);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dat">[NotNull] DBデータ</param>
        /// <param name="project">[NotNull] DBプロジェクト</param>
        /// <exception cref="ArgumentNullException">
        ///     dat, project が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///    dat.TypeSettingListとproject.SettingListの要素数が異なる場合
        /// </exception>
        public DatabaseMergedData(DatabaseDat dat,
            DatabaseProject project)
        {
            if (dat is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dat)));
            if (project is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(project)));

            InitializeTypeDescList(project.TypeSettingList, dat.SettingList);
        }

        /// <summary>
        /// TypeDescListを初期化する。
        /// </summary>
        /// <param name="typeSettingList">[NotNull] タイプ設定リスト</param>
        /// <param name="dataSettingList">[NotNull] データ設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     typeSettingList, dataSettingList が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///    typeSettingListとdataSettingListの要素数が異なる場合
        /// </exception>
        private void InitializeTypeDescList(DBTypeSettingList typeSettingList,
            DBDataSettingList dataSettingList)
        {
            if (typeSettingList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(typeSettingList)));
            if (dataSettingList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataSettingList)));

            if (typeSettingList.Count != dataSettingList.Count)
                throw new ArgumentException(
                    ErrorMessage.NotEqual($"{nameof(typeSettingList)}の要素数",
                        $"{nameof(dataSettingList)}の要素数"));

            for (var i = 0; i < typeSettingList.Count; i++)
            {
                TypeDescList.Add(new DatabaseTypeDesc(typeSettingList[i], dataSettingList[i]));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したタイプIDのデータ名リストを取得する。
        /// </summary>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <returns>DBデータ情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeId が指定範囲外の場合</exception>
        public IReadOnlyDataNameList GetDataNameList(TypeId typeId)
            => TypeDescList[typeId].DataNameList;

        /// <summary>
        /// 指定したタイプIDのデータ情報リストを取得する。
        /// </summary>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <returns>DBデータ情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeId が指定範囲外の場合</exception>
        public DatabaseDataDescList GetDataDescList(TypeId typeId)
            => TypeDescList[typeId].DataDescList;

        /// <summary>
        /// 指定したタイプIDのデータ情報リストを取得する。
        /// </summary>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <returns>DBデータ情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeId, dataId が指定範囲外の場合</exception>
        public DatabaseDataDesc GetDataDesc(TypeId typeId, DataId dataId)
            => TypeDescList[typeId].DataDescList[dataId];

        /// <summary>
        /// 指定したタイプID、データIDの項目値リストを取得する。
        /// </summary>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <returns>DB項目値リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeId, dataId が指定範囲外の場合</exception>
        public DBItemValueList GetItemValueList(TypeId typeId, DataId dataId)
            => TypeDescList[typeId].GetItemValueList(dataId);

        /// <summary>
        /// 指定したタイプID、データID、項目IDの項目値を取得する。
        /// </summary>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <param name="itemId">[Range(0, {対象DB・タイプ・データの項目数} - 1)] データID</param>
        /// <returns>DB項目値リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeId, dataId, itemId が指定範囲外の場合</exception>
        public DBItemValue GetItemValue(TypeId typeId, DataId dataId, ItemId itemId)
            => TypeDescList[typeId].GetItemValue(dataId, itemId);

        /// <summary>
        /// 自身が持つ情報からDatabaseDatインスタンスを生成する。
        /// </summary>
        /// <param name="dbKind">[Nullable] DB種別</param>
        /// <returns>DatabaseDatインスタンス</returns>
        public DatabaseDat GenerateDatabaseDat(DBKind dbKind = null)
        {
            return new DatabaseDat
            {
                DBKind = dbKind,
                SettingList = SelectDataSettingList()
            };
        }

        /// <summary>
        /// 自身が持つ情報からDatabaseProjectインスタンスを生成する。
        /// </summary>
        /// <param name="dbKind">[Nullable] DB種別</param>
        /// <returns>DatabaseProjectインスタンス</returns>
        public DatabaseProject GenerateDatabaseProject(DBKind dbKind = null)
        {
            var result = new DatabaseProject
            {
                DBKind = dbKind
            };
            result.TypeSettingList.AddRange(SelectTypeSettingList());
            return result;
        }

        /// <summary>
        /// 自身の情報を元にDBTypeSetインスタンスを生成する。
        /// </summary>
        /// <param name="typeId">[Range(0, TypeDescList.Count - 1)] タイプID</param>
        /// <returns>DBTypeSetインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeIdが指定範囲外の場合</exception>
        public DBTypeSet GenerateDBTypeSet(TypeId typeId)
        {
            var max = TypeDescList.Count - 1;
            const int min = 0;
            if (typeId > max)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(typeId), min, max, typeId));

            return TypeDescList[typeId].GenerateDBTypeSet();
        }

        /// <summary>
        /// 自身の情報を元にDBTypeインスタンスを生成する。
        /// </summary>
        /// <param name="typeId">[Range(0, TypeDescList.Count - 1)] タイプID</param>
        /// <returns>DBTypeインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeIdが指定範囲外の場合</exception>
        public DBType GenerateDBType(TypeId typeId)
        {
            var max = TypeDescList.Count - 1;
            const int min = 0;
            if (typeId > max)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(typeId), min, max, typeId));

            return TypeDescList[typeId].GenerateDBType();
        }

        /// <summary>
        /// 自身の情報を元にDBDataインスタンスを生成する。
        /// </summary>
        /// <param name="typeId">[Range(0, TypeDescList.Count - 1)] タイプID</param>
        /// <param name="start">[Range(0, TypeDescList[typeId].DataDescList.Count - 1)] 始点データID</param>
        /// <param name="count">[Range(0, TypeDescList[typeId].DataDescList.Count)] 出力データ数</param>
        /// <returns>DBDataインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">typeId, start, countが指定範囲外の場合</exception>
        public DBData GenerateDBData(TypeId typeId, DataId start, int count)
        {
            var typeIdMax = TypeDescList.Count - 1;
            const int typeIdMin = 0;
            if (typeId > typeIdMax)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(typeId), typeIdMin, typeIdMax, typeId));

            var dataDescListCount = TypeDescList[typeId].DataDescList.Count;
            var startMax = dataDescListCount - 1;
            const int startMin = 0;
            if (start > startMax)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(start), startMin, startMax, start));

            var countMax = dataDescListCount;
            const int countMin = 0;
            if (count < countMin || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), countMin, countMax, count));

            if (dataDescListCount - start < count)
                throw new ArgumentException(
                    $"{nameof(start)}および{nameof(count)}が有効な範囲を示していません。");

            return TypeDescList[typeId].GenerateDBData(start, count);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DatabaseMergedData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeDescList.Equals(other.TypeDescList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DBDataSettingList SelectDataSettingList()
        {
            return new DBDataSettingList(
                TypeDescList.Select(x => x.DataSetting).ToList());
        }

        private DBTypeSettingList SelectTypeSettingList()
        {
            return new DBTypeSettingList(
                TypeDescList.Select(x => x.TypeSetting).ToList());
        }
    }
}