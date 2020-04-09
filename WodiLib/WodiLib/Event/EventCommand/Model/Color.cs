// ========================================
// Project Name : WodiLib
// File Name    : Color.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 色クラス
    /// </summary>
    [Serializable]
    public class Color : ModelBase<Color>
    {
        private const string EventCommandSentenceFormat = "R[{0}] G[{1}] B[{2}]";

        private int r = 100;

        /// <summary>赤（int）</summary>
        public int R
        {
            get => r;
            set
            {
                r = value;
                NotifyPropertyChanged();
            }
        }

        private int g = 100;

        /// <summary>緑（int）</summary>
        public int G
        {
            get => g;
            set
            {
                g = value;
                NotifyPropertyChanged();
            }
        }

        private int b = 100;

        /// <summary>青（int）</summary>
        public int B
        {
            get => b;
            set
            {
                b = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>赤（byte）</summary>
        public byte ByteR
        {
            get => (byte) R;
            set => R = value;
        }

        /// <summary>緑（byte）</summary>
        public byte ByteG
        {
            get => (byte) G;
            set => G = value;
        }

        /// <summary>青（byte）</summary>
        public byte ByteB
        {
            get => (byte) B;
            set => B = value;
        }

        /// <summary>
        /// イベントコマンド文字列を取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        public string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var rStr = resolver.GetNumericVariableAddressStringIfVariableAddress(R, type, desc);
            var gStr = resolver.GetNumericVariableAddressStringIfVariableAddress(G, type, desc);
            var bStr = resolver.GetNumericVariableAddressStringIfVariableAddress(B, type, desc);

            return string.Format(EventCommandSentenceFormat,
                rStr, gStr, bStr);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(Color other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return R == other.R
                   && G == other.G
                   && B == other.B;
        }
    }
}