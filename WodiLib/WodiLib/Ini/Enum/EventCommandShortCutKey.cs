// ========================================
// Project Name : WodiLib
// File Name    : EventCommandShortCutKey.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// ショートカットキー
    /// </summary>
    public class EventCommandShortCutKey : TypeSafeEnum<EventCommandShortCutKey>
    {
        /// <summary>0</summary>
        public static readonly EventCommandShortCutKey Zero;

        /// <summary>1</summary>
        public static readonly EventCommandShortCutKey One;

        /// <summary>2</summary>
        public static readonly EventCommandShortCutKey Two;

        /// <summary>3</summary>
        public static readonly EventCommandShortCutKey Three;

        /// <summary>4</summary>
        public static readonly EventCommandShortCutKey Four;

        /// <summary>5</summary>
        public static readonly EventCommandShortCutKey Five;

        /// <summary>6</summary>
        public static readonly EventCommandShortCutKey Six;

        /// <summary>7</summary>
        public static readonly EventCommandShortCutKey Seven;

        /// <summary>8</summary>
        public static readonly EventCommandShortCutKey Eight;

        /// <summary>9</summary>
        public static readonly EventCommandShortCutKey Nine;

        /// <summary>A</summary>
        public static readonly EventCommandShortCutKey A;

        /// <summary>B</summary>
        public static readonly EventCommandShortCutKey B;

        /// <summary>C</summary>
        public static readonly EventCommandShortCutKey C;

        /// <summary>D</summary>
        public static readonly EventCommandShortCutKey D;

        /// <summary>E</summary>
        public static readonly EventCommandShortCutKey E;

        /// <summary>F</summary>
        public static readonly EventCommandShortCutKey F;

        /// <summary>G</summary>
        public static readonly EventCommandShortCutKey G;

        /// <summary>H</summary>
        public static readonly EventCommandShortCutKey H;

        /// <summary>I</summary>
        public static readonly EventCommandShortCutKey I;

        /// <summary>J</summary>
        public static readonly EventCommandShortCutKey J;

        /// <summary>K</summary>
        public static readonly EventCommandShortCutKey K;

        /// <summary>L</summary>
        public static readonly EventCommandShortCutKey L;

        /// <summary>M</summary>
        public static readonly EventCommandShortCutKey M;

        /// <summary>N</summary>
        public static readonly EventCommandShortCutKey N;

        /// <summary>O</summary>
        public static readonly EventCommandShortCutKey O;

        /// <summary>P</summary>
        public static readonly EventCommandShortCutKey P;

        /// <summary>Q</summary>
        public static readonly EventCommandShortCutKey Q;

        /// <summary>R</summary>
        public static readonly EventCommandShortCutKey R;

        /// <summary>S</summary>
        public static readonly EventCommandShortCutKey S;

        /// <summary>T</summary>
        public static readonly EventCommandShortCutKey T;

        /// <summary>U</summary>
        public static readonly EventCommandShortCutKey U;

        /// <summary>V</summary>
        public static readonly EventCommandShortCutKey V;

        /// <summary>W</summary>
        public static readonly EventCommandShortCutKey W;

        /// <summary>X</summary>
        public static readonly EventCommandShortCutKey X;

        /// <summary>Y</summary>
        public static readonly EventCommandShortCutKey Y;

        /// <summary>Z</summary>
        public static readonly EventCommandShortCutKey Z;

        /// <summary>未使用項目の設定値</summary>
        public static EventCommandShortCutKey None => One;

        static EventCommandShortCutKey()
        {
            Zero = new EventCommandShortCutKey(nameof(Zero), "0");
            One = new EventCommandShortCutKey(nameof(One), "1");
            Two = new EventCommandShortCutKey(nameof(Two), "2");
            Three = new EventCommandShortCutKey(nameof(Three), "3");
            Four = new EventCommandShortCutKey(nameof(Four), "4");
            Five = new EventCommandShortCutKey(nameof(Five), "5");
            Six = new EventCommandShortCutKey(nameof(Six), "6");
            Seven = new EventCommandShortCutKey(nameof(Seven), "7");
            Eight = new EventCommandShortCutKey(nameof(Eight), "8");
            Nine = new EventCommandShortCutKey(nameof(Nine), "9");
            A = new EventCommandShortCutKey(nameof(A), "A");
            B = new EventCommandShortCutKey(nameof(B), "B");
            C = new EventCommandShortCutKey(nameof(C), "C");
            D = new EventCommandShortCutKey(nameof(D), "D");
            E = new EventCommandShortCutKey(nameof(E), "E");
            F = new EventCommandShortCutKey(nameof(F), "F");
            G = new EventCommandShortCutKey(nameof(G), "G");
            H = new EventCommandShortCutKey(nameof(H), "H");
            I = new EventCommandShortCutKey(nameof(I), "I");
            J = new EventCommandShortCutKey(nameof(J), "J");
            K = new EventCommandShortCutKey(nameof(K), "K");
            L = new EventCommandShortCutKey(nameof(L), "L");
            M = new EventCommandShortCutKey(nameof(M), "M");
            N = new EventCommandShortCutKey(nameof(N), "N");
            O = new EventCommandShortCutKey(nameof(O), "O");
            P = new EventCommandShortCutKey(nameof(P), "P");
            Q = new EventCommandShortCutKey(nameof(Q), "Q");
            R = new EventCommandShortCutKey(nameof(R), "R");
            S = new EventCommandShortCutKey(nameof(S), "S");
            T = new EventCommandShortCutKey(nameof(T), "T");
            U = new EventCommandShortCutKey(nameof(U), "U");
            V = new EventCommandShortCutKey(nameof(V), "V");
            W = new EventCommandShortCutKey(nameof(W), "W");
            X = new EventCommandShortCutKey(nameof(X), "X");
            Y = new EventCommandShortCutKey(nameof(Y), "Y");
            Z = new EventCommandShortCutKey(nameof(Z), "Z");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public EventCommandShortCutKey(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static EventCommandShortCutKey FromCode(string code)
        {
            if (code is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(code)));

            try
            {
                return _FindFirst(x => x.Code == code);
            }
            catch
            {
                var exception = new ArgumentException($"{nameof(EventCommandShortCutKey)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">[Nullable] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static EventCommandShortCutKey FromCodeOrDefault(string code)
        {
            if (code is null || code.Equals(string.Empty)) return None;
            return FromCode(code);
        }
    }
}