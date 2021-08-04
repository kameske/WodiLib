// ========================================
// Project Name : WodiLib
// File Name    : WodiLibContainerKeyName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="WodiLibContainer"/> のキー名
    /// </summary>
    public record WodiLibContainerKeyName : SingleStringValueRecord<WodiLibContainerKeyName>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        protected override bool IsAllowEmpty => false;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="value">文字列値</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="value"/> が null の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="value"/> が空文字の場合。
        /// </exception>
        /// <exception cref="ArgumentNewLineException">
        ///     <paramref name="value"/> に改行コードが含まれる場合
        /// </exception>
        public WodiLibContainerKeyName(string value) : base(value)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     <see cref="string"/> -> <see cref="WodiLibContainerKeyName"/> への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator WodiLibContainerKeyName(string src)
            => new(src);

        /// <summary>
        ///     <see cref="WodiLibContainerKeyName"/> -> <see cref="string"/> への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator string(WodiLibContainerKeyName src)
            => src.Value;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     <see cref="WodiLibContainerKeyName"/> + <see cref="string"/>
        /// </summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>
        ///     左項文字列と右項文字列を結合した <see cref="WodiLibContainerKeyName"/> インスタンス。<br/>
        ///     左項および右項が共に <see langword="null"/> の場合、<see langword="null"/> を返却する。<br/>
        ///     左項のみ <see langword="null"/> の場合、右項文字列を <see cref="WodiLibContainerKeyName"/> インスタンスにして返却する。<br/>
        ///     右項のみ <see langword="null"/> の場合、<paramref name="left"/> を返却する。
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     結合結果文字列が <see cref="WodiLibContainerKeyName"/> として不適切な場合
        /// </exception>
        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static WodiLibContainerKeyName? operator +(WodiLibContainerKeyName? left, string? right)
        {
            switch (left?.Value, right)
            {
                case (null, null): return null;
                case (_, null): return left;
                case (null, _):
                    try
                    {
                        return new WodiLibContainerKeyName(right!);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"{right} を {nameof(WodiLibContainerKeyName)} に変換できません。"
                            + "詳細は InnerException 参照。", ex);
                    }
                default:
                    try
                    {
                        return new WodiLibContainerKeyName(left!.Value + right);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"{left} + {right} の結果を {nameof(WodiLibContainerKeyName)} に変換できません。"
                            + "詳細は InnerException 参照。", ex);
                    }
            }
        }

        /// <summary>
        ///     <see cref="WodiLibContainerKeyName"/> + <see cref="WodiLibContainerKeyName"/>
        /// </summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>
        ///     左項文字列と右項文字列を結合した <see cref="WodiLibContainerKeyName"/> インスタンス。<br/>
        ///     左項および右項が共に <see langword="null"/> の場合、<see langword="null"/> を返却する。<br/>
        ///     左項のみ <see langword="null"/> の場合、右項文字列を <see cref="WodiLibContainerKeyName"/> インスタンスにして返却する。<br/>
        ///     右項のみ <see langword="null"/> の場合、<paramref name="left"/> を返却する。
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     結合結果文字列が <see cref="WodiLibContainerKeyName"/> として不適切な場合
        /// </exception>
        [return: NotNullIfNotNull("left")]
        [return: NotNullIfNotNull("right")]
        public static WodiLibContainerKeyName? operator +(WodiLibContainerKeyName? left, WodiLibContainerKeyName? right)
        {
            switch (left?.Value, right?.Value)
            {
                case (null, null): return null;
                case (_, null): return left;
                case (null, _): return right;
                default:
                    try
                    {
                        return new WodiLibContainerKeyName(left!.Value + right!.Value);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"{left} + ${right} の結果を {nameof(WodiLibContainerKeyName)} に変換できません。"
                            + "詳細は InnerException 参照。", ex);
                    }
            }
        }
    }
}
