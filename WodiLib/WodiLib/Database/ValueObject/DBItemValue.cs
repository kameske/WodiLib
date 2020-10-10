// ========================================
// Project Name : WodiLib
// File Name    : DBItemValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目値
    /// </summary>
    [Serializable]
    public class DBItemValue : IConvertibleDBValueInt, IConvertibleDBValueString,
        IEquatable<DBItemValue>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>数値に変換できない理由</summary>
        private static readonly string NotCastIntReason = $"{nameof(Type)}が{nameof(DBItemType.Int)}ではないため";

        /// <summary>文字列に変換できない理由</summary>
        private static readonly string NotCastStringReason = $"{nameof(Type)}が{nameof(DBItemType.String)}ではないため";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>設定種別</summary>
        public DBItemType Type { get; }

        private readonly DBValueInt intValue = 0;

        /// <summary>数値設定値</summary>
        /// <exception cref="PropertyAccessException">設定種別が DBItemType.Int ではない場合</exception>
        public DBValueInt IntValue
        {
            get
            {
                if (Type != DBItemType.Int)
                    throw new PropertyAccessException(
                        ErrorMessage.NotAccess(NotCastIntReason));
                return intValue;
            }
        }

        private readonly DBValueString stringValue = "";

        /// <summary>文字列設定値</summary>
        /// <exception cref="PropertyAccessException">設定種別が DBItemType.String ではない場合</exception>
        public DBValueString StringValue
        {
            get
            {
                if (Type != DBItemType.String)
                    throw new PropertyAccessException(
                        ErrorMessage.NotAccess(NotCastStringReason));
                return stringValue;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="intValue">数値設定値</param>
        public DBItemValue(DBValueInt intValue)
        {
            Type = DBItemType.Int;
            this.intValue = intValue;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stringValue">文字列設定値</param>
        /// <exception cref="ArgumentNullException">stringValueがnullの場合</exception>
        public DBItemValue(DBValueString stringValue)
        {
            if (stringValue is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(stringValue)));

            Type = DBItemType.String;
            this.stringValue = stringValue;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Type);
            if (Type == DBItemType.Int)
            {
                sb.Append(IntValue);
            }
            else if (Type == DBItemType.String)
            {
                sb.Append(StringValue);
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DBItemValue) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = intValue.GetHashCode();
                hashCode = (hashCode * 397) ^ stringValue.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                return hashCode;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBValueIntに変換する。
        /// </summary>
        /// <returns>DBValueInt値</returns>
        /// <exception cref="InvalidOperationException">設定種別が DBItemType.Int ではない場合</exception>
        public DBValueInt ToDBValueInt()
        {
            if (Type != DBItemType.Int)
                throw new InvalidOperationException(
                    ErrorMessage.NotCast(NotCastIntReason));
            return IntValue;
        }

        /// <summary>
        /// DBValueStringに変換する。
        /// </summary>
        /// <returns>DBValueString値</returns>
        /// <exception cref="InvalidOperationException">設定種別が DBItemType.String ではない場合</exception>
        public DBValueString ToDBValueString()
        {
            if (Type != DBItemType.String)
                throw new InvalidOperationException(
                    ErrorMessage.NotCast(NotCastStringReason));
            return StringValue;
        }

        /// <summary>
        /// 自身の設定種別を基にデフォルト値を返却する。
        /// </summary>
        /// <returns>DBItemType.Int.DBItemDefaultValue または DBItemType.String.DBItemDefaultValue</returns>
        public DBItemValue GetDefaultValue()
        {
            if (Type == DBItemType.Int) return DBItemType.Int.DBItemDefaultValue;
            return DBItemType.String.DBItemDefaultValue;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemValue? other)
        {
            if (other is null) return false;

            if (Type != other.Type) return false;

            if (Type == DBItemType.Int)
            {
                if (IntValue != other.IntValue) return false;
            }
            else if (Type == DBItemType.String)
            {
                if (!StringValue.Equals(other.StringValue)) return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBValueInt -> DBItemValue への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DBItemValue(DBValueInt src)
        {
            var result = new DBItemValue(src);
            return result;
        }

        /// <summary>
        /// DBItemValue -> DBValueInt への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合、または
        ///     src が数値を含まない場合
        /// </exception>
        public static implicit operator DBValueInt(DBItemValue src)
        {
            if (src == null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(DBItemValue)));
            if (src.Type != DBItemType.Int)
                throw new InvalidCastException(
                    ErrorMessage.NotCast(NotCastIntReason));
            return src.ToDBValueInt();
        }

        /// <summary>
        /// DBValueString -> DBItemValue への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull("src")]
        public static implicit operator DBItemValue?(DBValueString? src)
        {
            if (src is null) return null;
            var result = new DBItemValue(src);
            return result;
        }

        /// <summary>
        /// DBItemValue -> DBValueString への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合、または
        ///     src が文字列を含まない場合
        /// </exception>
        public static implicit operator DBValueString(DBItemValue? src)
        {
            if (src is null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(DBItemValue)));
            if (src.Type != DBItemType.String)
                throw new InvalidCastException(
                    ErrorMessage.NotCast(NotCastStringReason));
            return src.ToDBValueString();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(DBItemValue? left, DBItemValue? right)
        {
            if (ReferenceEquals(left, right)) return true;

            if (left is null || right is null) return false;

            return left.Equals(right);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(DBItemValue? left, DBItemValue? right)
        {
            return !(left == right);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Commons
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public IEnumerable<byte> ToBinary()
        {
            if (Type == DBItemType.Int) return IntValue.ToBytes(Endian.Woditor);
            if (Type == DBItemType.String) return StringValue.ToWoditorStringBytes();
            throw new InvalidOperationException();
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
            info.AddValue(nameof(Type), Type.Code);
            info.AddValue(nameof(intValue), intValue);
            info.AddValue(nameof(stringValue), stringValue);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DBItemValue(SerializationInfo info, StreamingContext context)
        {
            Type = DBItemType.FromCode(info.GetInt32(nameof(Type)));
            intValue = info.GetInt32(nameof(intValue));
            stringValue = info.GetValue<DBValueString>(nameof(stringValue));
        }
    }
}
