// ========================================
// Project Name : WodiLib
// File Name    : MapChipListCollectionChangedHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Map
{
    /// <summary>
    /// MapChipList クラス CollectionChanged イベントヘルパー
    /// </summary>
    internal static class MapChipListCollectionChangedHelper
    {
        public static void UpdateItemPropertyChangedEvent(
            NotifyCollectionChangedEventArgs args,
            List<IFixedLengthMapChipColumns> items,
            PropertyChangedEventHandler eventHandler)
        {
            // 要素0に係るイベントの場合のみ処理
            if (args.NewStartingIndex != 0 && args.OldStartingIndex != 0) return;

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // args.NewStartingIndex == 0 であることが確定
                    items[0].PropertyChanged += eventHandler;
                    break;

                case NotifyCollectionChangedAction.Move:
                    // 元々要素0だったインスタンスからイベントを除去
                    if (args.NewStartingIndex == 0)
                    {
                        items[1].PropertyChanged -= eventHandler;
                    }
                    else if (args.OldStartingIndex == 0)
                    {
                        ((IFixedLengthMapChipColumns) args.OldItems[0]).PropertyChanged -= eventHandler;
                    }

                    // 要素0にイベントを追加
                    items[0].PropertyChanged += eventHandler;
                    break;

                case NotifyCollectionChangedAction.Remove:
                    // args.OldStartingIndex == 0 であることが確定
                    items[0].PropertyChanged -= eventHandler;
                    break;

                case NotifyCollectionChangedAction.Replace:
                    ((IFixedLengthMapChipColumns) args.OldItems[0]).PropertyChanged -= eventHandler;
                    ((IFixedLengthMapChipColumns) args.NewItems[0]).PropertyChanged += eventHandler;
                    break;

                case NotifyCollectionChangedAction.Reset:
                    items[0].PropertyChanged += eventHandler;
                    break;
            }
        }
    }
}