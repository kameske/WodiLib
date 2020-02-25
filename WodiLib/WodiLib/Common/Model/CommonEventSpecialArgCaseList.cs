// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgCaseList.cs
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

namespace WodiLib.Common
{
    /// <summary>
    /// 選択肢情報リスト
    /// </summary>
    [Serializable]
    internal class CommonEventSpecialArgCaseList : RestrictedCapacityCollection<CommonEventSpecialArgCase>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static int MaxCapacity => int.MaxValue;

        /// <summary>リスト最小数</summary>
        public static int MinCapacity => 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventSpecialArgCaseList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="list">[NotNull] 初期リスト</param>
        /// <exception cref="ArgumentNullException">listがnullの場合</exception>
        /// <exception cref="InvalidOperationException">listの要素数がMaxLengthを超える場合</exception>
        public CommonEventSpecialArgCaseList(
            IReadOnlyCollection<CommonEventSpecialArgCase> list) : base(list)
        {
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
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override CommonEventSpecialArgCase MakeDefaultItem(int index)
            => new CommonEventSpecialArgCase(0, "");

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢番号から選択肢文字列を取得する。
        /// </summary>
        /// <param name="caseNumber">選択肢番号</param>
        /// <returns>選択肢文字列。番号に対応した情報が存在しない場合null。</returns>
        public string GetDescriptionForCaseNumber(int caseNumber)
        {
            var info = GetForCaseNumber(caseNumber);
            if (info is null) return null;
            return GetForCaseNumber(caseNumber).Description;
        }

        /// <summary>
        /// 選択肢番号から選択肢情報を取得する。
        /// </summary>
        /// <param name="caseNumber">選択肢番号</param>
        /// <returns>選択肢情報。情報が存在しない場合CommonEventSpecialArgCase.Empty</returns>
        public CommonEventSpecialArgCase GetForCaseNumber(int caseNumber)
        {
            return Items.FirstOrDefault(x => x.CaseNumber == caseNumber);
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
        protected CommonEventSpecialArgCaseList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}