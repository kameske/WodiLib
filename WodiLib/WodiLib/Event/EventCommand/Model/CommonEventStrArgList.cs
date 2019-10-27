// ========================================
// Project Name : WodiLib
// File Name    : CommonEventStrArgList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// コモンイベント・文字列引数リストオブジェクト
    /// 要素4は必ず数値形式で使用する必要がある。
    /// </summary>
    public class CommonEventStrArgList : RestrictedCapacityCollection<IntOrStr>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 最大容量
        /// </summary>
        public static readonly int MaxCapacity = 5;

        /// <summary>
        /// 最小容量
        /// </summary>
        public static readonly int MinCapacity = 5;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static readonly byte Arg1ReferFlg = 0x10;
        private static readonly byte Arg2ReferFlg = 0x20;
        private static readonly byte Arg3ReferFlg = 0x40;
        private static readonly byte Arg4ReferFlg = 0x80;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 参照フラグ
        /// </summary>
        public byte ReferenceFlg
        {
            get
            {
                byte result = 0x00;
                if (this[0].InstanceIntOrStrType == IntOrStrType.Str) result += Arg1ReferFlg;
                if (this[1].InstanceIntOrStrType == IntOrStrType.Str) result += Arg2ReferFlg;
                if (this[2].InstanceIntOrStrType == IntOrStrType.Str) result += Arg3ReferFlg;
                if (this[3].InstanceIntOrStrType == IntOrStrType.Str) result += Arg4ReferFlg;
                return result;
            }
            set
            {
                if ((value & Arg1ReferFlg) == 0) this[0] = 0;
                else this[0] = "";
                if ((value & Arg2ReferFlg) == 0) this[1] = 0;
                else this[1] = "";
                if ((value & Arg3ReferFlg) == 0) this[2] = 0;
                else this[2] = "";
                if ((value & Arg4ReferFlg) == 0) this[3] = 0;
                else this[3] = "";
            }
        }


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal CommonEventStrArgList()
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
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定した要素が文字列かどうかを返す。
        /// </summary>
        /// <param name="index">[Range(0, 4)] 要素</param>
        /// <returns>文字列の場合、true</returns>
        public bool HasStringParam(int index)
        {
            return this[index].HasStr;
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
        protected override IntOrStr MakeDefaultItem(int index) => new IntOrStr();
    }
}