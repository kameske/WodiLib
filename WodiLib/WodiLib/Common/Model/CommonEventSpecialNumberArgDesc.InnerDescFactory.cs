// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialNumberArgDesc.InnerDescFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Common
{
    public partial class CommonEventSpecialNumberArgDesc
    {
        /// <summary>
        /// コモンイベント引数特殊指定情報内部クラスファクトリ
        /// </summary>
        [Serializable]
        internal static class InnerDescFactory
        {
            /// <summary>
            /// 特殊引数タイプと引数から情報内部クラスのインスタンスを取得する。
            /// </summary>
            /// <param name="type">[NotNull] 特殊引数タイプ</param>
            /// <param name="argCaseList">[Nullable] 選択肢と文字列リスト</param>
            /// <returns>内部情報クラスインスタンス</returns>
            /// <exception cref="ArgumentNullException">typeがnullの場合</exception>
            /// <exception cref="ArgumentException">typeに対応したファクトリメソッドが定義されていない場合</exception>
            public static IInnerDesc Create(
                CommonEventArgType type, CommonEventSpecialArgCaseList argCaseList)
            {
                if (type is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(argCaseList)));

                if (type == CommonEventArgType.Normal)
                {
                    return CreateNormal();
                }

                if (type == CommonEventArgType.ReferDatabase)
                {
                    return CreateReferDatabase(argCaseList);
                }

                if (type == CommonEventArgType.Manual)
                {
                    return CreateManual(argCaseList);
                }

                throw new ArgumentException($"{nameof(type)}のファクトリメソッドが未実装です。");
            }

            /// <summary>
            /// 「特殊な設定方法を使用しない」情報内部クラスのインスタンスを生成する。
            /// </summary>
            /// <returns>内部情報クラスインスタンス</returns>
            public static IInnerDesc CreateNormal()
            {
                var result = new InnerDescNormal();
                return result;
            }

            /// <summary>
            /// 「データベース参照」内部情報クラスのインスタンスを生成する。
            /// </summary>
            /// <param name="argCaseList">[Nullable] 引数と文字列リスト</param>
            /// <returns>内部情報クラスインスタンス</returns>
            public static IInnerDesc CreateReferDatabase(
                CommonEventSpecialArgCaseList argCaseList)
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

                var result = new InnerDescDatabase();
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
            public static IInnerDesc CreateManual(
                CommonEventSpecialArgCaseList argCaseList)
            {
                return new InnerDescManual(argCaseList);
            }
        }
    }
}