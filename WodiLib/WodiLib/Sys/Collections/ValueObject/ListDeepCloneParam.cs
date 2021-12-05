// ========================================
// Project Name : WodiLib
// File Name    : ListDeepCloneParam.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     DeepCloneパラメータ
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    public record ListDeepCloneParam<TItem>
    {
        private readonly IReadOnlyDictionary<int, TItem>? values;
        private readonly int? length;

        /// <summary>
        ///     ディープコピー時の上書きインデックスと値のペア列挙子
        /// </summary>
        /// <remarks>
        ///     ディープクローン時、インデックスの要素を上書きする。<br/>
        ///     返却する要素数を上回るインデックスが指定されている場合、その要素は無視される。
        /// </remarks>
        /// <exception cref="PropertyNullException">
        ///     セットした値に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public IReadOnlyDictionary<int, TItem>? Values
        {
            get => values;
            init
            {
                value?.ForEach(pair =>
                {
                    ThrowHelper.ValidatePropertyNotNull(pair.Value is null,
                        $"{nameof(Values)}（key={pair.Key}）");
                });
                values = value;
            }
        }

        /// <summary>
        ///     ディープコピー後の要素数
        /// </summary>
        /// <remarks>
        ///     ディープコピー後の要素数を指定された数に変更する。<br/>
        ///     コピー元の要素数より小さな値を指定した場合、超過する要素は切り捨てられる。<br/>
        ///     コピー元の要素数より大きな値を指定した場合、不足する要素は内包型ごとに定められたデフォルト値
        ///     （<see langword="null"/>ではない）が格納される。
        /// </remarks>
        public int? Length
        {
            get => length;
            init
            {
                if (value is not null)
                {
                    const int min = 0;
                    ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                        value.Value < min,
                        nameof(Length),
                        min,
                        value.Value
                    );
                }

                length = value;
            }
        }
    }
}
