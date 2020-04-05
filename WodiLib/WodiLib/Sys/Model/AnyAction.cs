// ========================================
// Project Name : WodiLib
// File Name    : AnyAction.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// いずれかのActionデリゲートを保有するクラス
    /// </summary>
    [Obsolete("Ver1.3削除クラス")]
    internal class AnyAction<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ActionType Type { get; }

        private Action Action1 { get; }

        private Action<int> Action2 { get; }

        private Action<int, T> Action3 { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public AnyAction(Action action)
        {
            Type = ActionType.Type1;
            Action1 = action;
        }

        public AnyAction(Action<int> action)
        {
            Type = ActionType.Type2;
            Action2 = action;
        }

        public AnyAction(Action<int, T> action)
        {
            Type = ActionType.Type3;
            Action3 = action;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Action を実行する。
        /// </summary>
        /// <param name="params">引数</param>
        public void Execute(params object[] @params)
        {
            switch (Type)
            {
                case ActionType.Type1:
                    Action1.Invoke();
                    break;
                case ActionType.Type2:
                    Action2.Invoke((int) @params[0]);
                    break;
                case ActionType.Type3:
                    Action3.Invoke((int) @params[0], (T) @params[1]);
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Enum
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private enum ActionType
        {
            Type1,
            Type2,
            Type3,
        }
    }
}