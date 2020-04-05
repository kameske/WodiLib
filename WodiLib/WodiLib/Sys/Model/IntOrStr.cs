// ========================================
// Project Name : WodiLib
// File Name    : IntOrStr.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    ///     int、stringを持つクラス
    /// </summary>
    [Serializable]
    public class IntOrStr : ModelBase<IntOrStr>, ISerializable
    {
        private static string NotifyPropertyChangedName = "Value";

        private readonly Guid guidForHash = Guid.NewGuid();

        private int numValue;
        private string strValue;

        /// <summary>
        /// int値を持つインスタンスを生成するコンストラクタ
        /// </summary>
        /// <param name="intValue">設定値</param>
        public IntOrStr(int intValue)
        {
            numValue = intValue;
            strValue = null;
            InstanceIntOrStrType = IntOrStrType.Int;
        }

        /// <summary>
        /// string値を持つインスタンスを生成するコンストラクタ
        /// </summary>
        /// <param name="strValue">設定値</param>
        public IntOrStr(string strValue)
        {
            numValue = 0;
            this.strValue = strValue;
            InstanceIntOrStrType = IntOrStrType.Str;
        }

        /// <summary>
        /// int, string どちらの値も持つインスタンスを生成するコンストラクタ
        /// </summary>
        /// <param name="numValue">int設定値</param>
        /// <param name="strValue">string設定値</param>
        public IntOrStr(int numValue, string strValue)
        {
            this.numValue = numValue;
            this.strValue = strValue;
            InstanceIntOrStrType = IntOrStrType.IntAndStr;
        }

        /// <summary>
        /// int, string どちらも持たないインスタンスを生成するコンストラクタ
        /// </summary>
        public IntOrStr()
        {
            numValue = 0;
            strValue = null;
            InstanceIntOrStrType = IntOrStrType.None;
        }

        /// <summary>
        /// 保有する値の種類
        /// </summary>
        public IntOrStrType InstanceIntOrStrType { get; private set; }

        /// <summary>
        ///     <pre>すでに文字列を保有している場合、両方所有状態にする</pre>
        ///     <pre>数値を保有している場合はその値を上書きする。</pre>
        /// </summary>
        /// <param name="value">設定値</param>
        public void Merge(int value)
        {
            InstanceIntOrStrType = HasStr ? IntOrStrType.IntAndStr : IntOrStrType.Int;
            numValue = value;
            NotifyPropertyChanged(NotifyPropertyChangedName);
        }

        /// <summary>
        ///     <pre>すでに数値を保有している場合、両方所有状態にする</pre>
        ///     <pre>文字列を保有している場合はその値を上書きする。</pre>
        /// </summary>
        /// <param name="value">設定値</param>
        public void Merge(string value)
        {
            InstanceIntOrStrType = HasInt ? IntOrStrType.IntAndStr : IntOrStrType.Str;
            strValue = value;
            NotifyPropertyChanged(NotifyPropertyChangedName);
        }

        /// <summary>
        ///     数値、文字列を引数で与えられたインスタンスの内容で上書きする。
        /// </summary>
        /// <param name="value">設定値</param>
        public void Merge(IntOrStr value)
        {
            if (value.HasInt) numValue = value.ToInt();
            if (value.HasStr) strValue = value.ToStr();

            if ((HasInt || value.HasInt) && (HasStr || value.HasStr)) InstanceIntOrStrType = IntOrStrType.IntAndStr;
            else if (HasInt || value.HasInt) InstanceIntOrStrType = IntOrStrType.Int;
            else if (HasStr || value.HasStr) InstanceIntOrStrType = IntOrStrType.Str;
            else InstanceIntOrStrType = IntOrStrType.None;

            NotifyPropertyChanged(NotifyPropertyChangedName);
        }

        /// <summary>数値保有フラグ</summary>
        public bool HasInt =>
            InstanceIntOrStrType == IntOrStrType.Int || InstanceIntOrStrType == IntOrStrType.IntAndStr;

        /// <summary>文字列保有フラグ</summary>
        public bool HasStr =>
            InstanceIntOrStrType == IntOrStrType.Str || InstanceIntOrStrType == IntOrStrType.IntAndStr;

        /// <summary>数値/文字列いずれかのみ保有フラグ</summary>
        public bool IsOneSideValue => HasInt != HasStr;

        /// <summary>
        ///     intに変換する。
        /// </summary>
        /// <returns>保有する数値</returns>
        /// <exception cref="InvalidCastException">保有する値がintではない場合</exception>
        public int ToInt()
        {
            if (!HasInt) throw new InvalidCastException();
            return numValue;
        }

        /// <summary>
        ///     stringに変換する。
        /// </summary>
        /// <returns>保有する文字列</returns>
        /// <exception cref="InvalidCastException">保有する値がstringではない場合</exception>
        public string ToStr()
        {
            if (!HasStr) throw new InvalidCastException();
            return strValue;
        }

        /// <summary>
        /// 内容を文字列化する。
        /// </summary>
        /// <returns>文字列化した内容</returns>
        public string ToValueString()
        {
            if (InstanceIntOrStrType == IntOrStrType.IntAndStr) return $"({ToInt()}, {ToStr()})";
            if (InstanceIntOrStrType == IntOrStrType.Int) return $"{ToInt()}";
            if (InstanceIntOrStrType == IntOrStrType.Str) return ToStr();
            return "";
        }

        /// <summary>
        /// int -> IntOrStr 暗黙型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator IntOrStr(int src)
        {
            return new IntOrStr(src);
        }

        /// <summary>
        /// string -> IntOrStr 暗黙型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator IntOrStr(string src)
        {
            if (src is null) return null;
            return new IntOrStr(src);
        }

        /// <summary>
        /// Tuple&lt;int, string> -> IntOrStr 暗黙型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator IntOrStr(Tuple<int, string> tuple)
        {
            if (tuple is null) return null;
            return new IntOrStr(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (int, string) -> IntOrStr 暗黙型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator IntOrStr(ValueTuple<int, string> tuple)
        {
            return new IntOrStr(tuple.Item1, tuple.Item2);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Type: {InstanceIntOrStrType}, Value: \"{ToValueString()}\"";
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IntOrStr) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return guidForHash.GetHashCode();
        }


        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(IntOrStr other)
        {
            if (other is null) return false;

            if (InstanceIntOrStrType != other.InstanceIntOrStrType) return false;
            if (HasInt && numValue != other.numValue) return false;
            if (HasStr && !strValue.Equals(other.strValue)) return false;

            return true;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(guidForHash), guidForHash);
            info.AddValue(nameof(numValue), numValue);
            info.AddValue(nameof(strValue), strValue);
            info.AddValue(nameof(InstanceIntOrStrType), InstanceIntOrStrType.Id);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected IntOrStr(SerializationInfo info, StreamingContext context)
        {
            guidForHash = info.GetValue<Guid>(nameof(guidForHash));
            numValue = info.GetInt32(nameof(numValue));
            strValue = info.GetValue<string>(nameof(strValue));
            InstanceIntOrStrType = IntOrStrType.FromId(info.GetValue<string>(nameof(InstanceIntOrStrType)));
        }
    }
}