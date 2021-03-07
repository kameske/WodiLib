using System;
using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント文字列引数特殊指定情報リスト
    /// </summary>
    public class CommonEventSpecialStringArgDescList : FixedLengthList<CommonEventSpecialStringArgDesc, CommonEventSpecialStringArgDescList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト数</summary>
        public static int Capacity => 5;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventSpecialStringArgDescList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期リスト</param>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が5以外の場合</exception>
        public CommonEventSpecialStringArgDescList(
            IEnumerable<CommonEventSpecialStringArgDesc> items)
            : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public override int GetCapacity() => Capacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override CommonEventSpecialStringArgDesc MakeDefaultItem(int index) =>
            new CommonEventSpecialStringArgDesc();
    }
}
