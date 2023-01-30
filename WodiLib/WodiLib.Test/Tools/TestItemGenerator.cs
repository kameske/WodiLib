// ========================================
// Project Name : WodiLib.Test
// File Name    : TestItemGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    public static class TestItemGenerator
    {
        public enum GenerateArrayType
        {
            NotNull,
            EvenNull,
            AllNull
        }

        /// <summary>
        ///     テスト用の列挙データを生成する。
        /// </summary>
        /// <param name="length">要素数。0未満の場合 null を返却する。</param>
        /// <param name="type">列挙タイプ</param>
        /// <param name="generator">要素生成処理</param>
        /// <typeparam name="T">生成データ型</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GenerateArray<T>(int length, GenerateArrayType type, Func<int, T> generator)
        {
            if (length < 0)
            {
                return null!;
            }

            return length.Range()
                .Select(
                    idx =>
                    {
                        return type switch
                        {
                            GenerateArrayType.NotNull => generator(idx),
                            GenerateArrayType.EvenNull => idx % 2 == 0
                                ? default!
                                : generator(idx),
                            GenerateArrayType.AllNull => default!,
                            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                        };
                    }
                );
        }

        public enum GenerateTwoDimArrayType
        {
            Null,
            Dig,
            HasNull,
            NotEqualColumnLength,
            Fill
        }

        /// <summary>
        ///     テスト用の二次元列挙データを生成する。
        /// </summary>
        /// <param name="type">列挙タイプ</param>
        /// <param name="rowLength">行要素数。0未満の場合 null を返却する。</param>
        /// <param name="columnLength">列要素数。0未満の場合すべての列要素を null として返却する。</param>
        /// <param name="generator">要素生成処理</param>
        /// <typeparam name="T">生成データ型</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<IEnumerable<T>> GenerateTwoDimArray<T>(
            GenerateTwoDimArrayType type,
            int rowLength,
            int columnLength,
            Func<int, int, T> generator
        )
        {
            if (rowLength < 0)
            {
                return null!;
            }

            if (columnLength < 0)
            {
                return rowLength.Iterate(_ => (IEnumerable<T>)null!);
            }

            return type switch
            {
                GenerateTwoDimArrayType.Null => default!,
                GenerateTwoDimArrayType.Dig => rowLength.Iterate(
                    i => (columnLength + i % 2).Iterate(j => generator(i, j))
                ),
                GenerateTwoDimArrayType.HasNull => rowLength.Iterate(
                    i => columnLength.Iterate(
                        j => j % 2 == 0
                            ? generator(i, j)
                            : default!
                    )
                ),
                GenerateTwoDimArrayType.NotEqualColumnLength => rowLength.Iterate(
                    i => (columnLength + 1).Iterate(j => generator(i, j))
                ),
                GenerateTwoDimArrayType.Fill => rowLength.Iterate(
                    i => columnLength.Iterate(j => generator(i, j))
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        /// <summary>
        ///     テスト用の二次元列挙データを生成する。
        /// </summary>
        /// <param name="rowsInfo">データ生成情報</param>
        /// <param name="generator">要素生成処理</param>
        /// <typeparam name="T">生成データ型</typeparam>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GenerateTwoDimArray<T>(
            RowsInfo rowsInfo,
            Func<int, int, T> generator
        )
            => GenerateTwoDimArray(rowsInfo.Type, rowsInfo.RowLength, rowsInfo.ColumnLength, generator);

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1>(
            IEnumerable<T1> @params,
            Func<T1, bool> expectedError
        )
            => GenerateTestCaseSource(@params, null, expectedError);

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="filter">生成した組み合わせフィルタ処理</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1>(
            IEnumerable<T1> @params,
            Func<T1, bool>? filter,
            Func<T1, bool> expectedError
        )
        {
            var filtered = filter is not null
                ? @params.Where(filter)
                : @params;
            return filtered.Select(product => new object[] { product!, expectedError(product) } as object).ToArray();
        }

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <typeparam name="T2">パラメータ2型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1, T2>(
            (IEnumerable<T1>, IEnumerable<T2>) @params,
            Func<T1, T2, bool> expectedError
        )
            => GenerateTestCaseSource(@params, null, expectedError);

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="filter">生成した組み合わせフィルタ処理</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <typeparam name="T2">パラメータ2型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1, T2>(
            (IEnumerable<T1>, IEnumerable<T2>) @params,
            Func<T1, T2, bool>? filter,
            Func<T1, T2, bool> expectedError
        )
        {
            var products = GenerateProduct(@params.Item1, @params.Item2);
            var filtered = filter is not null
                ? products.Where(product => filter(product.Item1, product.Item2))
                : products;
            return
                filtered.Select(
                        product => new object[]
                            { product.Item1!, product.Item2!, expectedError(product.Item1, product.Item2) } as object
                    )
                    .ToArray();
        }

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <typeparam name="T2">パラメータ2型</typeparam>
        /// <typeparam name="T3">パラメータ3型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1, T2, T3>(
            (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) @params,
            Func<T1, T2, T3, bool> expectedError
        )
            => GenerateTestCaseSource(@params, null, expectedError);

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="filter">生成した組み合わせフィルタ処理</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <typeparam name="T2">パラメータ2型</typeparam>
        /// <typeparam name="T3">パラメータ3型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1, T2, T3>(
            (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) @params,
            Func<T1, T2, T3, bool>? filter,
            Func<T1, T2, T3, bool> expectedError
        )
        {
            var products = GenerateProduct(@params.Item1, @params.Item2, @params.Item3);
            var filtered = filter is not null
                ? products.Where(product => filter(product.Item1, product.Item2, product.Item3))
                : products;
            return
                filtered.Select(
                        product => new object[]
                        {
                            product.Item1!, product.Item2!, product.Item3!,
                            expectedError(product.Item1, product.Item2, product.Item3)
                        } as object
                    )
                    .ToArray();
        }

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <typeparam name="T2">パラメータ2型</typeparam>
        /// <typeparam name="T3">パラメータ3型</typeparam>
        /// <typeparam name="T4">パラメータ4型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1, T2, T3, T4>(
            (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>) @params,
            Func<T1, T2, T3, T4, bool> expectedError
        )
            => GenerateTestCaseSource(@params, null, expectedError);

        /// <summary>
        ///     TestCaseSource を生成する。
        /// </summary>
        /// <param name="params">引数パターン</param>
        /// <param name="filter">生成した組み合わせフィルタ処理</param>
        /// <param name="expectedError">引数パターン別 実行エラー有無期待値</param>
        /// <typeparam name="T1">パラメータ1型</typeparam>
        /// <typeparam name="T2">パラメータ2型</typeparam>
        /// <typeparam name="T3">パラメータ3型</typeparam>
        /// <typeparam name="T4">パラメータ4型</typeparam>
        /// <returns></returns>
        public static object[] GenerateTestCaseSource<T1, T2, T3, T4>(
            (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>) @params,
            Func<T1, T2, T3, T4, bool>? filter,
            Func<T1, T2, T3, T4, bool> expectedError
        )
        {
            var products = GenerateProduct(@params.Item1, @params.Item2, @params.Item3, @params.Item4);
            var filtered = filter is not null
                ? products.Where(product => filter(product.Item1, product.Item2, product.Item3, product.Item4))
                : products;
            return
                filtered.Select(
                        product => new object[]
                        {
                            product.Item1!, product.Item2!, product.Item3!, product.Item4!,
                            expectedError(product.Item1, product.Item2, product.Item3, product.Item4)
                        } as object
                    )
                    .ToArray();
        }

        /// <summary>
        ///     2つの集合の積を生成する。
        /// </summary>
        /// <param name="x">集合X</param>
        /// <param name="y">集合Y</param>
        /// <typeparam name="T1">集合Xの型</typeparam>
        /// <typeparam name="T2">集合Yの型</typeparam>
        /// <returns></returns>
        private static IEnumerable<(T1, T2)> GenerateProduct<T1, T2>(IEnumerable<T1> x, IEnumerable<T2> y)
            => x.SelectMany(xItem => y.Select(yItem => (xItem, yItem)));

        /// <summary>
        ///     3つの集合の積を生成する。
        /// </summary>
        /// <param name="x">集合X</param>
        /// <param name="y">集合Y</param>
        /// <param name="z">集合Z</param>
        /// <typeparam name="T1">集合Xの型</typeparam>
        /// <typeparam name="T2">集合Yの型</typeparam>
        /// <typeparam name="T3">集合Zの型</typeparam>
        /// <returns></returns>
        private static IEnumerable<(T1, T2, T3)> GenerateProduct<T1, T2, T3>(
            IEnumerable<T1> x,
            IEnumerable<T2> y,
            IEnumerable<T3> z
        )
            => GenerateProduct(x, GenerateProduct(y, z)).Select(a => (a.Item1, a.Item2.Item1, a.Item2.Item2));

        /// <summary>
        ///     4つの集合の積を生成する。
        /// </summary>
        /// <param name="a">集合A</param>
        /// <param name="b">集合B</param>
        /// <param name="c">集合C</param>
        /// <param name="d">集合D</param>
        /// <typeparam name="T1">集合aの型</typeparam>
        /// <typeparam name="T2">集合bの型</typeparam>
        /// <typeparam name="T3">集合cの型</typeparam>
        /// <typeparam name="T4">集合dの型</typeparam>
        /// <returns></returns>
        private static IEnumerable<(T1, T2, T3, T4)> GenerateProduct<T1, T2, T3, T4>(
            IEnumerable<T1> a,
            IEnumerable<T2> b,
            IEnumerable<T3> c,
            IEnumerable<T4> d
        )
            => GenerateProduct(a, GenerateProduct(b, c, d))
                .Select(x => (x.Item1, x.Item2.Item1, x.Item2.Item2, x.Item2.Item3));

        /// <summary>
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="RowLength"></param>
        /// <param name="ColumnLength"></param>
        public record RowsInfo(
            GenerateTwoDimArrayType Type,
            int RowLength,
            int ColumnLength
        );
    }
}
