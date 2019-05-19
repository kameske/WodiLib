// ========================================
// Project Name : WodiLib
// File Name    : DBItemValuesList.MadeInstanceManager.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Database
{
    public partial class DBItemValuesList
    {
        private class MadeInstanceManager
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>生成した値リストインスタンスの弱参照</summary>
            private List<WeakReference<DBItemValueList>> MadeInstanceList { get; }
                = new List<WeakReference<DBItemValueList>>();

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public void Add(WeakReference<DBItemValueList> weakReference) => MadeInstanceList.Add(weakReference);

            public void Clear() => MadeInstanceList.Clear();

            /// <summary>
            /// DB項目種別変更を生成したすべてのインスタンスに反映する。
            /// 更新された項目は、
            ///     値種別が変化した場合はデフォルト値で初期化され、
            ///     値種別が変化しなかった場合は値の変更は起こらない。
            /// </summary>
            /// <param name="itemId">項目ID</param>
            /// <param name="type">変更後の値種別</param>
            public void ReflectChangedValueType(ItemId itemId, DBItemType type)
            {
                Refresh();

                // タイプ変化チェックメソッド 後のロジックのため匿名関数にする
                var typeCheckFunc = new Func<DBItemValueList, bool>(target => target[itemId].Type == type);

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    // タイプ変化チェックは最初の1回だけ行えばいい
                    if (typeCheckFunc != null)
                    {
                        if (typeCheckFunc.Invoke(target)) break;
                        typeCheckFunc = null;
                    }

                    target[itemId] = type.DBItemDefaultValue;
                }
            }

            /// <summary>
            /// DB項目種別変更を生成したすべてのインスタンスに反映する。
            /// </summary>
            /// <param name="itemId">項目ID</param>
            /// <param name="value">変更後の値</param>
            public void ReflectChangedValue(ItemId itemId, DBItemValue value)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;
                    target[itemId] = value;
                }
            }

            /// <summary>
            /// DB項目追加を生成したすべてのインスタンスに反映する。
            /// 追加された項目はデフォルト値で初期化される。
            /// </summary>
            /// <param name="type">値種別</param>
            public void ReflectAddValueType(DBItemType type)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;
                    target.AddForValuesListInstanceManager(type.DBItemDefaultValue);
                }
            }

            /// <summary>
            /// DB項目追加を生成したすべてのインスタンスに反映する。
            /// 追加された項目は引数で指定された値で初期化される。
            /// </summary>
            /// <param name="value">値</param>
            public void ReflectAddValue(DBItemValue value)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;
                    target.AddForValuesListInstanceManager(value);
                }
            }

            /// <summary>
            /// DB項目追加を生成したすべてのインスタンスに反映する。
            /// 追加された項目はデフォルト値で初期化される。
            /// </summary>
            /// <param name="types">値種別リスト</param>
            public void ReflectAddValueTypeRange(IReadOnlyCollection<DBItemType> types)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    var defaultValues = types.Select(x => x.DBItemDefaultValue).ToList();
                    target.AddRangeForValuesListInstanceManager(defaultValues);
                }
            }

            /// <summary>
            /// DB項目追加を生成したすべてのインスタンスに反映する。
            /// 追加された項目は引数で指定された値で初期化される。
            /// </summary>
            /// <param name="values">値リスト</param>
            public void ReflectAddValueRange(IReadOnlyCollection<DBItemValue> values)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    target.AddRangeForValuesListInstanceManager(values);
                }
            }

            /// <summary>
            /// DB項目挿入を生成したすべてのインスタンスに反映する。
            /// 挿入された項目はデフォルト値で初期化される。
            /// </summary>
            /// <param name="itemId">項目ID</param>
            /// <param name="type">値種別</param>
            public void ReflectInsertValueType(ItemId itemId, DBItemType type)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;
                    target.InsertForValuesListInstanceManager(itemId, type.DBItemDefaultValue);
                }
            }

            /// <summary>
            /// DB項目挿入を生成したすべてのインスタンスに反映する。
            /// 挿入された項目は引数で指定された値で初期化される。
            /// </summary>
            /// <param name="itemId">項目ID</param>
            /// <param name="value">値</param>
            public void ReflectInsertValue(ItemId itemId, DBItemValue value)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;
                    target.InsertForValuesListInstanceManager(itemId, value);
                }
            }


            /// <summary>
            /// DB項目挿入を生成したすべてのインスタンスに反映する。
            /// 挿入された項目はデフォルト値で初期化される。
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <param name="types">値種別リスト</param>
            public void ReflectInsertValueTypeRange(int index, IReadOnlyCollection<DBItemType> types)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    var defaultValues = types.Select(x => x.DBItemDefaultValue).ToList();
                    target.InsertRangeForValuesListInstanceManager(index, defaultValues);
                }
            }

            /// <summary>
            /// DB項目挿入を生成したすべてのインスタンスに反映する。
            /// 挿入された項目は引数で指定された値で初期化される。
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <param name="values">値リスト</param>
            public void ReflectInsertValueRange(int index, IReadOnlyCollection<DBItemValue> values)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    target.InsertRangeForValuesListInstanceManager(index, values);
                }
            }

            /// <summary>
            /// DB項目除去を生成したすべてのインスタンスに反映する。
            /// </summary>
            /// <param name="itemId">項目ID</param>
            public void ReflectRemoveAt(ItemId itemId)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    target.RemoveAtForValuesListInstanceManager(itemId);
                }
            }

            /// <summary>
            /// DB項目除去を生成したすべてのインスタンスに反映する。
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <param name="count">削除する要素数</param>
            public void ReflectRemoveRange(int index, int count)
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    target.RemoveRangeForValuesListInstanceManager(index, count);
                }
            }

            /// <summary>
            /// DB項目初期化を生成したすべてのインスタンスに反映する。
            /// </summary>
            public void ReflectClear()
            {
                Refresh();

                foreach (var reference in MadeInstanceList)
                {
                    if (!reference.TryGetTarget(out var target)) continue;

                    target.ClearForValuesListInstanceManager();
                }
            }

            public void Refresh()
            {
                MadeInstanceList.RemoveAll(x => !x.TryGetTarget(out _));
            }
        }
    }
}