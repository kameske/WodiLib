// ========================================
// Project Name : WodiLib.Test
// File Name    : TestTemplate.ts.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    /// テスト用テンプレート処理
    /// </summary>
    internal static class TestTemplate
    {
        #region Constructor

        /// <summary>
        /// コンストラクタテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>コンストラクタ実行によりインスタンスを生成する。</li>
        /// <li>インスタンス生成によるエラーの有無をテスト</li>
        /// <li>生成されたインスタンスまたはデフォルト値を返却</li>
        /// </ul>
        /// </remarks>
        /// <param name="factory">コンストラクタ実行処理</param>
        /// <param name="expectedThrowCreateNewInstance">コンストラクタエラー有無</param>
        /// <param name="logger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Constructor<T>(
            Func<T> factory,
            bool expectedThrowCreateNewInstance,
            Logger logger
        )
        {
            var errorOccured = false;
            try
            {
                var instance = factory();
                return instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
                return default!;
            }
            finally
            {
                // エラー有無が意図した結果であること
                Assert.AreEqual(expectedThrowCreateNewInstance, errorOccured);
            }
        }

        #endregion

        #region Property

        /// <summary>
        /// プロパティ値編集 &amp; 取得のテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>プロパティに対して値の編集を試みる</li>
        /// <li>値編集によるエラーの有無をテスト</li>
        /// <li>プロパティ値編集によりエラーが発生した場合処理を終了</li>
        /// <li>既値と異なる値が編集された場合、プロパティ変更通知が行われていることをテスト</li>
        /// <li>既値と同一値が編集された場合、プロパティ変更通知が行われていないことをテスト</li>
        /// <li>プロパティ値の取得を試みる</li>
        /// <li>プロパティ値取得によるエラーの有無をテスト</li>
        /// <li>プロパティ変更通知が行われていないことをテスト</li>
        /// <li>プロパティ値取得によりエラーが発生した場合処理を終了</li>
        /// <li>編集したプロパティ値と取得したプロパティ値が同値であることをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="propertyName">テスト対象のプロパティ名</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="isEqualSetItemBeforePropertyValue">編集する値と編集前の値が同値であるか</param>
        /// <param name="propertySetter">プロパティ編集処理</param>
        /// <param name="expectedThrowActPropertySet">プロパティ編集時例外有無期待値</param>
        /// <param name="propertyGetter">プロパティ取得処理</param>
        /// <param name="expectedThrowActPropertyGet">プロパティ取得処理</param>
        /// <param name="itemEqualityComparer">設定値と取得値の比較処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public static void PropertyGetAndSet<TTarget, TItem>(
            TTarget instance,
            string propertyName,
            TItem setItem,
            bool isEqualSetItemBeforePropertyValue,
            Action<TTarget, TItem> propertySetter,
            bool expectedThrowActPropertySet,
            Func<TTarget, TItem> propertyGetter,
            bool expectedThrowActPropertyGet,
            Func<TItem, TItem, bool> itemEqualityComparer,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged
        {
            PropertySet(
                instance,
                propertyName,
                setItem,
                isEqualSetItemBeforePropertyValue,
                propertySetter,
                expectedThrowActPropertySet,
                logger
            );

            var isExpectedItem = new Func<TItem, bool>(getItem => itemEqualityComparer(setItem, getItem));
            PropertyGet(instance, propertyGetter, expectedThrowActPropertyGet, isExpectedItem, logger);
        }

        /// <summary>
        /// プロパティ値取得のテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>プロパティ値の取得を試みる</li>
        /// <li>プロパティ値取得によりエラーが発生しないことをテスト</li>
        /// <li>プロパティ値取得によるエラーの有無をテスト</li>
        /// <li>プロパティ変更通知が行われていないことをテスト</li>
        /// <li>プロパティ値取得によりエラーが発生した場合処理を終了</li>
        /// <li>取得したプロパティ値が同値であることをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="propertyGetter">プロパティ取得処理</param>
        /// <param name="expectedThrowActPropertyGet">プロパティ取得処理</param>
        /// <param name="isExpectedItem">取得した要素が意図した値であることを検証する処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        public static void PropertyGet<TTarget, TItem>(
            TTarget instance,
            Func<TTarget, TItem> propertyGetter,
            bool expectedThrowActPropertyGet,
            Func<TItem, bool> isExpectedItem,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged
        {
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };

            TItem getResult = default!;
            var errorOccured = false;
            try
            {
                getResult = propertyGetter(instance);
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowActPropertyGet, errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);

            if (errorOccured)
            {
                return;
            }

            // 取得した要素が期待した要素と一致すること
            Assert.IsTrue(isExpectedItem(getResult));
        }

        /// <summary>
        /// プロパティ値編集のテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>プロパティに対して値の編集を試みる</li>
        /// <li>値編集によるエラーの有無をテスト</li>
        /// <li>既値と異なる値が編集された場合、プロパティ変更通知が行われていることをテスト</li>
        /// <li>既値と同一値が編集された場合、プロパティ変更通知が行われていないことをテスト</li>
        /// <li>プロパティに対して同じ値の編集を試みる</li>
        /// <li>プロパティ変更通知が行われていないことをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="propertyName">テスト対象のプロパティ名</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="isEqualSetItemBeforePropertyValue">編集する値と編集前の値が同値であるか</param>
        /// <param name="propertySetter">プロパティ編集処理</param>
        /// <param name="expectedThrowActPropertySet">プロパティ編集時例外有無期待値</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public static void PropertySet<TTarget, TItem>(
            TTarget instance,
            string propertyName,
            TItem setItem,
            bool isEqualSetItemBeforePropertyValue,
            Action<TTarget, TItem> propertySetter,
            bool expectedThrowActPropertySet,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged
        {
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                propertySetter(instance, setItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowActPropertySet, errorOccured);

            if (errorOccured)
            {
                // プロパティ変更通知が発火していないこと
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                if (isEqualSetItemBeforePropertyValue)
                {
                    // プロパティ変更通知が発火していないこと
                    Assert.AreEqual(changedPropertyList.Count, 0);
                }
                else
                {
                    // プロパティ変更通知が発火していること
                    Assert.AreEqual(changedPropertyList.Count, 1);
                    Assert.AreEqual(changedPropertyList[0], propertyName);
                }
            }

            {
                // 同じ値をセットしてプロパティ変更通知が発火していないことを確認

                changedPropertyList.Clear();

                propertySetter(instance, setItem);

                // プロパティ変更通知が発火していないこと
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// 純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>メソッドを実行する</li>
        /// <li>メソッド実行によるエラーの有無をテスト</li>
        /// <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        /// <li>メソッド実行前後で状態が変化していないことをテスト</li>
        /// <li>メソッド実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="isExpectedItem">取得した要素が意図した値であることを検証する処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        /// <returns></returns>
        public static MethodTestResult<TResult> PureMethod<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Func<TResult, bool> isExpectedItem,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged, IEqualityComparable<TTarget>
        {
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };

            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);

            // 取得した値が意図した値であること
            if (!errorOccured)
            {
                Assert.IsTrue(isExpectedItem(execResult));
            }

            return new MethodTestResult<TResult>(execResult, errorOccured);
        }

        /// <summary>
        /// 非純粋メソッドのテスト(戻り値あり)
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>メソッドを実行する</li>
        /// <li>メソッド実行によるエラーの有無をテスト</li>
        /// <li>ソッド実行によりエラーが発生していない場合、メソッド実行によりプロパティ変更通知が意図したとおり発火していることをテスト</li>
        /// <li>メソッド実行によりエラーが発生していない場合、実行結果が意図した値であることをテスト</li>
        /// <li>メソッド実行によりエラーが発生していない場合、実行の状態が意図した状態であることをテスト</li>
        /// <li>ソッド実行によりエラーが発生した場合、メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        /// <li>メソッド実行によりエラーが発生した場合、メソッド実行前後で状態が変化していないことをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="isExpectedItem">取得した要素が意図した値であることを検証する処理</param>
        /// <param name="expectedNotifyPropertyChange">期待するプロパティ変更通知</param>
        /// <param name="isExpectedState">メソッド実行後のインスタンスがいとした状態であることを検証する処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static MethodTestResult<TResult> MutableMethod<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Func<TResult, bool> isExpectedItem,
            IEnumerable<string> expectedNotifyPropertyChange,
            Func<TTarget, bool> isExpectedState,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged, IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
        {
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };

            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (!errorOccured)
            {
                // プロパティ変更通知が発火していること
                var expectedNotifyPropertyChangeList = expectedNotifyPropertyChange.ToList();

                AssertEqualsNotifiedPropertyNames(
                    changedPropertyList,
                    expectedNotifyPropertyChangeList
                );

                // 取得した値が意図した値であること
                Assert.IsTrue(isExpectedItem(execResult));

                // 実行後のインスタンスが意図した状態であること
                Assert.IsTrue(isExpectedState(instance));
            }

            return new MethodTestResult<TResult>(execResult, errorOccured);
        }

        /// <summary>
        /// 非純粋メソッドのテスト(戻り値なし)
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>メソッドを実行する</li>
        /// <li>メソッド実行によるエラーの有無をテスト</li>
        /// <li>ソッド実行によりエラーが発生していない場合、メソッド実行によりプロパティ変更通知が意図したとおり発火していることをテスト</li>
        /// <li>メソッド実行によりエラーが発生していない場合、実行結果が意図した値であることをテスト</li>
        /// <li>メソッド実行によりエラーが発生していない場合、実行の状態が意図した状態であることをテスト</li>
        /// <li>ソッド実行によりエラーが発生した場合、メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        /// <li>メソッド実行によりエラーが発生した場合、メソッド実行前後で状態が変化していないことをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="expectedNotifyPropertyChange">期待するプロパティ変更通知</param>
        /// <param name="isExpectedState">メソッド実行後のインスタンスがいとした状態であることを検証する処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns>エラー有無</returns>
        public static bool MutableMethod<TTarget>(
            TTarget instance,
            Action<TTarget> execFunc,
            bool expectedThrowExecute,
            IEnumerable<string> expectedNotifyPropertyChange,
            Func<TTarget, bool> isExpectedState,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged, IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
        {
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                execFunc(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (!errorOccured)
            {
                // プロパティ変更通知が発火していること
                var expectedNotifyPropertyChangeList = expectedNotifyPropertyChange.ToList();

                AssertEqualsNotifiedPropertyNames(
                    expectedNotifyPropertyChangeList,
                    changedPropertyList
                );

                // 実行後のインスタンスが意図した状態であること
                Assert.IsTrue(isExpectedState(instance));
            }

            return errorOccured;
        }

        /// <summary>
        /// 比較処理のテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>比較メソッドを実行する</li>
        /// <li>メソッド実行によりエラーが発生しないことをテスト</li>
        /// <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        /// <li>メソッド実行前後で状態が変化していないことをテスト</li>
        /// <li>結果が意図した値であることをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="left">比較対象のインスタンスその1</param>
        /// <param name="right">比較対象のインスタンスその2</param>
        /// <param name="expected">期待する比較結果</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        public static void ItemEquals<TTarget>(
            TTarget left,
            TTarget? right,
            bool expected,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged, IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
        {
            PureMethod(
                left,
                target => target.ItemEquals(right),
                false,
                actual => expected == actual,
                logger
            );
        }

        /// <summary>
        /// DeepCloneメソッドのテスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>メソッドを実行する</li>
        /// <li>メソッド実行によりエラーが発生しないことをテスト</li>
        /// <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        /// <li>メソッド実行前後で状態が変化していないことをテスト</li>
        /// <li>取得した値が元のインスタンスとは別インスタンスであり、同値であることをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <returns></returns>
        public static void DeepClone<TTarget>(
            TTarget instance,
            Logger logger
        )
            where TTarget : INotifyPropertyChanged, IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
        {
            PureMethod(
                instance,
                target => target.DeepClone(),
                false,
                result => !ReferenceEquals(instance, result) && result.ItemEquals(instance),
                logger
            );
        }


        private static void AssertEqualsNotifiedPropertyNames(
            IReadOnlyList<string> expected,
            IReadOnlyList<string> actual
        )
        {
            Assert.AreEqual(expected.Count, actual.Count);
            expected.ForEach(
                expectedItem => { Assert.IsTrue(actual.Count(actualItem => expectedItem == actualItem) == 1); }
            );
        }

        #endregion

        #region StaticClass

        /// <summary>
        /// staticクラスの関数テスト
        /// </summary>
        /// <remarks>
        /// 以下の手順のテストを行う。
        /// <ul>
        /// <li>関数を実行する</li>
        /// <li>関数実行によるエラーの有無をテスト</li>
        /// <li>関数実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        /// </ul>
        /// </remarks>
        /// <param name="execFunc">関数実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="assertResult">
        ///     取得した要素が意図した値であることを検証する処理<br/>
        ///     <see langword="null"/> の場合実行しない。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TResult">関数返却型</typeparam>
        public static void StaticClassFunc<TResult>(
            Func<TResult> execFunc,
            bool expectedThrowExecute,
            Action<TResult> assertResult,
            Logger logger
        )
        {
            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            // 取得した値が意図した値であること
            if (!errorOccured)
            {
                assertResult?.Invoke(execResult);
            }
        }

        #endregion

        #region class

        /// <summary>
        /// メソッドテスト結果
        /// </summary>
        /// <param name="Result">メソッド返戻値</param>
        /// <param name="ErrorOccured">エラー有無</param>
        /// <typeparam name="T">メソッド返戻型</typeparam>
        public record MethodTestResult<T>(
            T Result,
            bool ErrorOccured
        );

        #endregion
    }
}
