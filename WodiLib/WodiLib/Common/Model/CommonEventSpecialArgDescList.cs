// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgDescList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント引数特殊指定情報リスト
    /// </summary>
    [Serializable]
    internal class CommonEventSpecialArgDescList : IEquatable<CommonEventSpecialArgDescList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダチェックディジット
        /// </summary>
        public static readonly byte[] Header =
        {
            0x8F
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 文字列引数名用リストオフセット
        /// </summary>
        private const int StrArgListOffset = 5;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>変数タイプ数</summary>
        private int Count => argTypeList.Count;

        /// <summary>引数特殊指定情報リスト</summary>
        private readonly List<ICommonEventSpecialArgDesc> argTypeList = new List<ICommonEventSpecialArgDesc>
        {
            new CommonEventSpecialNumberArgDesc(), new CommonEventSpecialNumberArgDesc(),
            new CommonEventSpecialNumberArgDesc(),
            new CommonEventSpecialNumberArgDesc(), new CommonEventSpecialNumberArgDesc(),
            new CommonEventSpecialStringArgDesc(), new CommonEventSpecialStringArgDesc(),
            new CommonEventSpecialStringArgDesc(),
            new CommonEventSpecialStringArgDesc(), new CommonEventSpecialStringArgDesc()
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 選択肢リストの文字列をバイト配列データにしてリストに格納する。
        /// </summary>
        /// <param name="specialStringArgCasesList">対象選択肢リスト</param>
        private static IEnumerable<byte> MakeSpecialArgCaseStringByte(
            IEnumerable<List<string>> specialStringArgCasesList)
        {
            var result = new List<byte>();

            foreach (var caseStringList in specialStringArgCasesList)
            {
                // 選択肢数
                result.AddRange(caseStringList.Count.ToBytes(Endian.Woditor));

                // 選択肢文字列
                foreach (var caseString in caseStringList)
                {
                    var caseWoditorString = new WoditorString(caseString);
                    result.AddRange(caseWoditorString.StringByte);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 選択肢リストの数値をバイト配列データにしてリストに格納する。
        /// </summary>
        /// <param name="specialNumberArgCasesList">対象選択肢リスト</param>
        private static IEnumerable<byte> MakeSpecialArgCaseIntByte(
            IEnumerable<List<int>> specialNumberArgCasesList)
        {
            var result = new List<byte>();

            foreach (var caseNumberList in specialNumberArgCasesList)
            {
                // 選択肢数
                result.AddRange(caseNumberList.Count.ToBytes(Endian.Woditor));

                // 選択肢値
                foreach (var caseInt in caseNumberList)
                {
                    result.AddRange(caseInt.ToBytes(Endian.Woditor));
                }
            }

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 数値引数の情報を更新する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="desc">[NotNull] 情報</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">descがnullの場合</exception>
        public void UpdateSpecialNumberArgDesc(CommonEventNumberArgIndex index,
            CommonEventSpecialNumberArgDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));
            argTypeList[index] = desc;
        }

        /// <summary>
        /// 数値引数の情報を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>情報インスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public CommonEventSpecialNumberArgDesc GetSpecialNumberArgDesc(CommonEventNumberArgIndex index)
        {
            return (CommonEventSpecialNumberArgDesc) argTypeList[index];
        }

        /// <summary>
        /// 文字列引数の情報を更新する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="desc">[NotNull] 情報</param>
        /// <exception cref="ArgumentNullException">descがnullの場合</exception>
        public void UpdateSpecialStringArgDesc(CommonEventStringArgIndex index,
            CommonEventSpecialStringArgDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));
            argTypeList[index + StrArgListOffset] = desc;
        }

        /// <summary>
        /// 文字列引数の情報を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>情報インスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public CommonEventSpecialStringArgDesc GetSpecialStringArgDesc(CommonEventStringArgIndex index)
        {
            return (CommonEventSpecialStringArgDesc) argTypeList[index + StrArgListOffset];
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(CommonEventSpecialArgDescList other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return argTypeList.SequenceEqual(other.argTypeList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(Header);

            // 引数名数
            var argLength = Count;
            result.AddRange(argLength.ToBytes(Endian.Woditor));

            // 引数名
            var argNames = argTypeList.Select(x => x.ArgName);
            foreach (var argName in argNames)
            {
                result.AddRange(argName.ToWoditorStringBytes());
            }

            // 特殊指定数
            result.AddRange(argLength.ToBytes(Endian.Woditor));

            // 特殊指定種別
            var specialArgTypes = argTypeList.Select(x => x.ArgType);
            result.AddRange(specialArgTypes.Select(specialArgType => specialArgType.Code));

            // 特殊指定文字列パラメータ数
            result.AddRange(argLength.ToBytes(Endian.Woditor));

            // 特殊指定文字列パラメータ
            var specialStringArgCasesList = argTypeList.Select(x => x.GetAllSpecialCaseDescription());
            result.AddRange(MakeSpecialArgCaseStringByte(specialStringArgCasesList));

            // 特殊指定数値パラメータ数
            result.AddRange(argLength.ToBytes(Endian.Woditor));

            // 特殊指定数値
            var specialNumberArgCasesList = argTypeList.Select(x => x.GetAllSpecialCaseNumber());
            result.AddRange(MakeSpecialArgCaseIntByte(specialNumberArgCasesList));

            // 数値引数初期値数
            var specialNumberArgDescList = argTypeList.Where(x => x is CommonEventSpecialNumberArgDesc).ToList();
            result.AddRange(specialNumberArgDescList.Count.ToBytes(Endian.Woditor));

            // 数値引数初期値
            var initValueList = specialNumberArgDescList.Select(x => x.InitValue);
            foreach (var initValue in initValueList)
            {
                result.AddRange(initValue.ToBytes(Endian.Woditor));
            }

            return result.ToArray();
        }
    }
}