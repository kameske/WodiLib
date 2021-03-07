// ========================================
// Project Name : WodiLib
// File Name    : CommonEventIntArgList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンド数値引数拡張リスト
    /// </summary>
    /// <remarks>
    /// ウディタの通常の動作で使用する引数はここには含まれない。
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class EventCommandExpansionNumberArgList : RestrictedCapacityList<int, EventCommandExpansionNumberArgList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 最大容量
        /// </summary>
        public static readonly int MaxCapacity = EventCommandBase.NumberArgsLengthMax;

        /// <summary>
        /// 最小容量
        /// </summary>
        public static readonly int MinCapacity = EventCommandBase.NumberArgsLengthMin;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal EventCommandExpansionNumberArgList()
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
        protected override int MakeDefaultItem(int index) => 0;
    }
}
