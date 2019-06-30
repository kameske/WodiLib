// ========================================
// Project Name : WodiLib
// File Name    : MapTreeData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップツリーデータ
    /// </summary>
    public class MapTreeData
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        public static readonly byte[] Header =
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90
        };

        /// <summary>
        /// フッタ
        /// </summary>
        public static readonly byte[] Footer =
        {
            0xBC
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapTreeNodeList treeNodeList = new MapTreeNodeList();

        /// <summary>
        /// [NotNull] マップツリーノードリスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapTreeNodeList TreeNodeList
        {
            get => treeNodeList;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TreeNodeList)));
                treeNodeList = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(Header);

            // ツリーノード
            result.AddRange(TreeNodeList.ToBinary());

            // フッタ
            result.AddRange(Footer);

            return result.ToArray();
        }
    }
}