// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingDescFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// コモンイベント引数特殊指定情報内部クラスファクトリ
    /// </summary>
    internal static class DBItemSettingDescFactory
    {
        /// <summary>
        /// 特殊引数タイプと引数から情報内部クラスのインスタンスを取得する。
        /// </summary>
        /// <param name="type">[NotNull] 特殊引数タイプ</param>
        /// <param name="argCaseList">[Nullable] 選択肢と文字列リスト</param>
        /// <returns>内部情報クラスインスタンス</returns>
        /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
        /// <exception cref="ArgumentException">typeに対応したファクトリメソッドが定義されていない場合</exception>
        public static IDBItemSettingDesc Create(
            DBItemSpecialSettingType type, DatabaseValueCaseList argCaseList)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(argCaseList)));

            if (type == DBItemSpecialSettingType.Normal)
            {
                return CreateNormal();
            }

            if (type == DBItemSpecialSettingType.LoadFile)
            {
                return CreateLoadFile(argCaseList);
            }

            if (type == DBItemSpecialSettingType.ReferDatabase)
            {
                return CreateReferDatabase(argCaseList);
            }

            if (type == DBItemSpecialSettingType.Manual)
            {
                return CreateManual(argCaseList);
            }

            throw new ArgumentException($"{nameof(type)}のファクトリメソッドが未実装です。");
        }

        /// <summary>
        /// 「特殊な設定方法を使用しない」情報内部クラスのインスタンスを生成する。
        /// </summary>
        /// <returns>内部情報クラスインスタンス</returns>
        public static IDBItemSettingDesc CreateNormal()
        {
            var result = new DBItemSettingDescNormal();
            return result;
        }

        /// <summary>
        /// 「ファイル読み込み」情報内部クラスのインスタンスを生成する。
        /// </summary>
        /// <returns>[Nullable][ItemLength(1)] 内部情報クラスインスタンス</returns>
        /// <exception cref="ArgumentException">argCaseList.Countが1以外の場合</exception>
        public static IDBItemSettingDesc CreateLoadFile(DatabaseValueCaseList argCaseList)
        {
            if (argCaseList is null)
            {
                return new DBItemSettingDescLoadFile();
            }

            if (argCaseList.Count != 1)
                throw new ArgumentException(
                    ErrorMessage.LengthRange(nameof(argCaseList), 1,
                        1, argCaseList.Count));

            var result = new DBItemSettingDescLoadFile(argCaseList[0]);
            return result;
        }

        /// <summary>
        /// 「データベース参照」内部情報クラスのインスタンスを生成する。
        /// </summary>
        /// <param name="argCaseList">[Nullable] 引数と文字列リスト</param>
        /// <returns>内部情報クラスインスタンス</returns>
        public static IDBItemSettingDesc CreateReferDatabase(
            DatabaseValueCaseList argCaseList)
        {
            var argCaseMinus1 = "";
            var argCaseMinus2 = "";
            var argCaseMinus3 = "";
            if (!(argCaseList is null))
            {
                argCaseMinus1 = argCaseList.GetDescriptionForCaseNumber(-1) ?? "";
                argCaseMinus2 = argCaseList.GetDescriptionForCaseNumber(-2) ?? "";
                argCaseMinus3 = argCaseList.GetDescriptionForCaseNumber(-3) ?? "";
            }

            var result = new DBItemSettingDescDatabase();
            result.UpdateDatabaseSpecialCase(-1, argCaseMinus1);
            result.UpdateDatabaseSpecialCase(-2, argCaseMinus2);
            result.UpdateDatabaseSpecialCase(-3, argCaseMinus3);

            return result;
        }

        /// <summary>
        /// 「選択肢手動生成」内部情報クラスのインスタンスを生成する。
        /// </summary>
        /// <param name="argCaseList">[Nullable] 引数と文字列リスト</param>
        /// <returns>内部情報クラスインスタンス</returns>
        public static IDBItemSettingDesc CreateManual(
            DatabaseValueCaseList argCaseList)
        {
            return new DBItemSettingDescManual(argCaseList);
        }
    }
}