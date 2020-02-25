// ========================================
// Project Name : WodiLib
// File Name    : TileCannotPassingFlags.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行方向（通過不可）フラグ
    /// </summary>
    /// <remarks>
    /// 各プロパティが "true" のとき、その方向への移動は不可能。
    /// 「trueのときに通行可能」ではないことに注意。
    ///
    /// すべてのプロパティを "true" にすることはできない。
    /// その場合は「通行不可」と同義のため。
    /// </remarks>
    [Serializable]
    public class TileCannotPassingFlags : IEquatable<TileCannotPassingFlags>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>下方向コード</summary>
        private const int DownCode = 0x01;

        /// <summary>左方向コード</summary>
        private const int LeftCode = 0x02;

        /// <summary>右方向コード</summary>
        private const int RightCode = 0x04;

        /// <summary>上方向コード</summary>
        private const int UpCode = 0x08;

        private readonly string PropertyAllTrueErrorMessage
            = $"{nameof(Down)}, {nameof(Left)}, {nameof(Right)}, " +
              $"{nameof(Up)}のうちいずれかは{false}である必要があります。";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool down;

        /// <summary>
        /// 下方向通過不可フラグ
        /// </summary>
        public bool Down
        {
            get => down;
            set
            {
                if (!value)
                {
                    down = false;
                    return;
                }

                // Value == true
                if (IsAllTrue(Left, Right, Up))
                {
                    throw new PropertyException(PropertyAllTrueErrorMessage);
                }

                down = true;
            }
        }

        private bool left;

        /// <summary>
        /// 左方向通過不可フラグ
        /// </summary>
        public bool Left
        {
            get => left;
            set
            {
                if (!value)
                {
                    left = false;
                    return;
                }

                // Value == true
                if (IsAllTrue(Down, Right, Up))
                {
                    throw new PropertyException(PropertyAllTrueErrorMessage);
                }

                left = true;
            }
        }

        private bool right;

        /// <summary>
        /// 右方向通過不可フラグ
        /// </summary>
        public bool Right
        {
            get => right;
            set
            {
                if (!value)
                {
                    right = false;
                    return;
                }

                // Value == true
                if (IsAllTrue(Down, Left, Up))
                {
                    throw new PropertyException(PropertyAllTrueErrorMessage);
                }

                right = true;
            }
        }

        private bool up;

        /// <summary>
        /// 上方向通過不可フラグ
        /// </summary>
        public bool Up
        {
            get => up;
            set
            {
                if (!value)
                {
                    up = false;
                    return;
                }

                // Value == true
                if (IsAllTrue(Down, Left, Right))
                {
                    throw new PropertyException(PropertyAllTrueErrorMessage);
                }

                up = true;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TileCannotPassingFlags()
        {
            Down = false;
            Left = false;
            Right = false;
            Up = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cannotPassingFlagCode">通行不可設定フラグコード値</param>
        public TileCannotPassingFlags(int cannotPassingFlagCode)
        {
            Down = (cannotPassingFlagCode & DownCode) != 0;
            Left = (cannotPassingFlagCode & LeftCode) != 0;
            Right = (cannotPassingFlagCode & RightCode) != 0;
            Up = (cannotPassingFlagCode & UpCode) != 0;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(TileCannotPassingFlags other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Down != other.Down) return false;
            if (Left != other.Left) return false;
            if (Right != other.Right) return false;
            if (Up != other.Up) return false;

            return true;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static bool IsAllTrue(params bool[] targets)
        {
            return targets.All(x => x);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コード値変換する。
        /// </summary>
        /// <returns>コード値</returns>
        public int ToCode()
        {
            var code = 0;
            code += Down ? DownCode : 0;
            code += Left ? LeftCode : 0;
            code += Right ? RightCode : 0;
            code += Up ? UpCode : 0;

            return code;
        }
    }
}