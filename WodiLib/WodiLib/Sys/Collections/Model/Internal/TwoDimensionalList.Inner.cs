// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<T>
    {
        /// <summary>
        ///     <see cref="TwoDimensionalList{T}"/> の内側要素実装クラス
        /// </summary>
        private class Inner : ExtendedList<T>
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Public Delegate
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public delegate IEnumerable<T> MakeDefaultItem(Inner self, int column, int count);

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Constructors
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            private Inner(IEnumerable<T> values, MakeDefaultItem funcMakeDefaultItem) : base(values)
            {
                FuncMakeItems = (idx, cnt) => funcMakeDefaultItem(this, idx, cnt);
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Public Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            // ______________________ Get ______________________

            public int GetCapacity() => Count;

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Classes
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public class Factory
            {
                private MakeDefaultItem FuncMakeDefaultItem { get; }

                public Factory(MakeDefaultItem funcMakeDefaultItem)
                {
                    FuncMakeDefaultItem = funcMakeDefaultItem;
                }

                public Inner Create(IEnumerable<T> values)
                    => new(values, FuncMakeDefaultItem);
            }
        }
    }
}
