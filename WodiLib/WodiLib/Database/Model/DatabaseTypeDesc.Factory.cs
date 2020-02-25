// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeDesc.Factory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================


using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    public partial class DatabaseTypeDesc
    {
        /// <summary>
        /// 内部処理用DatabaseTypeDescFactory
        /// </summary>
        internal static class Factory
        {
            /// <summary>
            /// DBTypeSet用のDatabaseDatabaseTypeDescインスタンスを生成する。
            /// </summary>
            /// <returns>インスタンス</returns>
            public static DatabaseTypeDesc CreateForDBTypeSet()
            {
                return new DatabaseTypeDesc(BaseListType.DBTypeSet);
            }

            /// <summary>
            /// DBTypeSet用のDatabaseDatabaseTypeDescインスタンスを生成する。
            /// </summary>
            /// <param name="itemSettingList">[NotNull] 項目設定リスト</param>
            /// <returns>インスタンス</returns>
            /// <exception cref="ArgumentNullException">dataNameList, itemSettingList が null の場合</exception>
            public static DatabaseTypeDesc CreateForDBTypeSet(DBItemSettingList itemSettingList)
            {
                if (itemSettingList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(itemSettingList)));

                var result = CreateForDBTypeSet();

                result.WritableItemSettingList.Overwrite(0, itemSettingList);

                return result;
            }

            /// <summary>
            /// DBType用のDatabaseDatabaseTypeDescインスタンスを生成する。
            /// </summary>
            /// <returns>インスタンス</returns>
            public static DatabaseTypeDesc CreateForDBType()
            {
                return new DatabaseTypeDesc(BaseListType.DBType);
            }

            /// <summary>
            /// DBType用のDatabaseDatabaseTypeDescインスタンスを生成する。
            /// </summary>
            /// <returns>インスタンス</returns>
            /// <param name="dataDescList">[NotNull] データ情報リスト</param>
            /// <param name="itemDescList">[NotNull] 項目情報リスト</param>
            /// <exception cref="ArgumentNullException">dataDescList, itemDescList が null の場合</exception>
            public static DatabaseTypeDesc CreateForDBType(DatabaseDataDescList dataDescList,
                DatabaseItemDescList itemDescList)
            {
                var result = new DatabaseTypeDesc(BaseListType.DBType);

                if (dataDescList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(dataDescList)));

                if (itemDescList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(itemDescList)));

                result.ItemDescList.Overwrite(0, itemDescList);
                result.DataDescList.Overwrite(0, dataDescList);

                return result;
            }

            /// <summary>
            /// DBData用のDatabaseDatabaseTypeDescインスタンスを生成する。
            /// </summary>
            /// <returns>インスタンス</returns>
            public static DatabaseTypeDesc CreateForDBData()
            {
                return new DatabaseTypeDesc(BaseListType.DBData);
            }

            /// <summary>
            /// DBData用のDatabaseDatabaseTypeDescインスタンスを生成する。
            /// </summary>
            /// <param name="dataDescList">[NotNull] DBデータ情報</param>
            /// <returns>インスタンス</returns>
            /// <exception cref="ArgumentNullException">dataDescList が null の場合</exception>
            public static DatabaseTypeDesc CreateForDBData(DatabaseDataDescList dataDescList)
            {
                if (dataDescList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(dataDescList)));

                var result = new DatabaseTypeDesc(BaseListType.DBData);
                result.DataDescList.Overwrite(0, dataDescList);

                return result;
            }
        }
    }
}