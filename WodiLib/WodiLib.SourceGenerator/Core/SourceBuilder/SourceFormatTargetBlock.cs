// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceFormatTargetBlock.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     ソースコード文字列整形対象ブロック
    /// </summary>
    internal class SourceFormatTargetBlock : IEnumerable<SourceFormatTarget>
    {
        /// <summary>空ブロック</summary>
        public static SourceFormatTargetBlock NoLine { get; } = new(Array.Empty<SourceFormatTarget>());

        /// <summary>空行ブロック</summary>
        public static SourceFormatTargetBlock Empty { get; } = new(SourceFormatTarget.Empty);

        /// <summary>
        ///     複数の <see cref="SourceFormatTargetBlock"/> を一つの <see cref="SourceFormatTargetBlock"/> にまとめる。
        /// </summary>
        /// <param name="targets">マージ対象</param>
        /// <returns>処理結果</returns>
        public static SourceFormatTargetBlock Merge(params SourceFormatTargetBlock[] targets)
            => new(
                targets.SelectMany(block => block)
            );

        /*
         * この中には IsAppend == true の SourceFormatTarget のみ格納していく。
         */

        /// <summary>
        ///     空フラグ
        /// </summary>
        public bool IsEmpty => BlockImpl.Count == 0;

        /// <summary>
        ///     最後の行 = 空行フラグ
        /// </summary>
        /// <remarks>
        ///     <see cref="IsEmpty"/> == <see langword="true"/> の場合は <see langword="false"/>
        /// </remarks>
        public bool IsEmptyLastLine
            => !IsEmpty && BlockImpl[BlockImpl.Count - 1].IsEmpty;

        /// <summary>
        ///     行数
        /// </summary>
        public int Length => BlockImpl.Count;

        /// <summary>
        ///     ソースコード情報リスト
        /// </summary>
        private List<SourceFormatTarget> BlockImpl { get; }

        public SourceFormatTargetBlock(params string[] targets) : this(
            targets.Select(
                s =>
                    new SourceFormatTarget(s)
            )
        )
        {
        }

        public SourceFormatTargetBlock(params SourceFormatTarget[] targets)
        {
            var filtered = targets.Where(item => item.IsAppend);
            BlockImpl = new List<SourceFormatTarget>(filtered);
        }

        public SourceFormatTargetBlock(IEnumerable<SourceFormatTarget> targets) : this(targets.ToArray())
        {
        }

        public IEnumerator<SourceFormatTarget> GetEnumerator()
            => BlockImpl.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => BlockImpl.GetEnumerator();

        /// <summary>
        ///     空行ではない最終行のインデックスを取得する。
        /// </summary>
        public int LastIndexOfNotEmptyLine()
        {
            if (IsEmpty) return -1;
            return BlockImpl.FindLastIndex(item => !item.IsEmpty);
        }

        public SourceFormatTargetBlock Add(params SourceFormatTarget[] targets)
        {
            var filtered = targets.Where(item => item.IsAppend);
            BlockImpl.AddRange(filtered);
            return this;
        }

        /// <summary>
        ///     最終行が空行ではない場合、空行を追加する。
        /// </summary>
        public SourceFormatTargetBlock AddNewLine()
        {
            if (IsEmpty) return this;
            if (IsEmptyLastLine) return this;
            Add(SourceFormatTarget.Empty);
            return this;
        }

        /// <summary>
        ///     すべての行にprefixを付与する。
        /// </summary>
        /// <param name="prefix">結合する文字列</param>
        public SourceFormatTargetBlock AppendPrefixAllLine(string prefix)
        {
            if (IsEmpty) return this;

            for (var i = 0; i < Length; i++)
            {
                BlockImpl[i] = BlockImpl[i].AppendPrefix(prefix);
            }

            return this;
        }

        /// <summary>
        ///     空行ではない最終行にsuffixを付与する。
        /// </summary>
        /// <param name="suffix">付与するsuffix</param>
        public SourceFormatTargetBlock AppendSuffixLastLine(string suffix)
        {
            if (IsEmpty) return this;

            var lastIndex = LastIndexOfNotEmptyLine();
            BlockImpl[lastIndex] = BlockImpl[lastIndex].AppendSuffix(suffix);
            return this;
        }

        /// <summary>
        ///     最終行が空行の場合に除去する。空行が複数行存在する場合はすべて除去する。
        /// </summary>
        public SourceFormatTargetBlock TrimLastEmptyLine()
        {
            if (IsEmpty) return this;

            var lastIndex = Length - 1;
            if (!BlockImpl[lastIndex].IsEmpty) return this;

            BlockImpl.RemoveAt(lastIndex);

            return TrimLastEmptyLine();
        }

        public static implicit operator SourceFormatTargetBlock(SourceFormatTarget[] src)
            => new(src);

        public static implicit operator SourceFormatTargetBlock(string[] src)
            => new(src);

        public static implicit operator SourceFormatTargetBlock(List<SourceFormatTarget> src)
            => new(src);

        public static implicit operator SourceFormatTargetBlock(string src)
            => new(src);
    }
}
