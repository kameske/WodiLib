// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolveDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Common;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// 変数アドレスイベントコマンド文字列解決用情報クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EventCommandSentenceResolveDesc
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>コモンイベントID</summary>
        public CommonEventId? CommonEventId { get; set; }

        private readonly Stack<BranchType> branchTypeStack = new Stack<BranchType>();

        /// <summary>選択肢種別スタック</summary>
        public IReadOnlyCollection<BranchType> BranchTypeStack => branchTypeStack;

        private readonly Stack<IReadOnlyList<string>> choiceCaseStack
            = new Stack<IReadOnlyList<string>>();

        /// <summary>選択肢リストスタック</summary>
        public IReadOnlyCollection<IReadOnlyList<string>> ChoiceCaseStack => choiceCaseStack;

        /// <summary>現在参照中の選択肢種別</summary>
        public BranchType CurrentBranchType => branchTypeStack.Peek();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 分岐情報をセットする。
        /// </summary>
        /// <remarks>
        ///     イベントコマンド文字列取得処理中に分岐開始のイベントコマンド中でセットされる。
        /// </remarks>
        /// <param name="type">分岐種別</param>
        /// <param name="cases">分岐リスト</param>
        /// <exception cref="ArgumentNullException">
        ///    type, cases が null の場合、
        ///     または cases に null 要素が含まれる場合
        /// </exception>
        public void StartBranch(BranchType type, List<string> cases)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));
            if (cases is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(cases)));
            if (cases.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(cases)));


            branchTypeStack.Push(type);
            choiceCaseStack.Push(cases);
        }

        /// <summary>
        /// 分岐終了時の処理を行う。
        /// </summary>
        public void EndBranch()
        {
            branchTypeStack.Pop();
            choiceCaseStack.Pop();
        }

        //        public string

        /// <summary>
        /// 選択肢スタックから、選択肢番号を指定して選択肢文字列を取得する。
        /// </summary>
        /// <param name="caseNumber">[Range(0, 0～9)] 選択肢番号</param>
        /// <returns>選択肢文字列</returns>
        /// <exception cref="IndexOutOfRangeException">対応する選択肢番号が存在しない場合</exception>
        public string GetCase(int caseNumber)
        {
            return choiceCaseStack.Peek()[caseNumber];
        }
    }
}