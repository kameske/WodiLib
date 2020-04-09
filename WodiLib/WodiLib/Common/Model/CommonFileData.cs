// ========================================
// Project Name : WodiLib
// File Name    : CommonFileData.cs
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
    /// コモンファイルデータクラス
    /// </summary>
    [Serializable]
    public class CommonFileData : ModelBase<CommonFileData>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダ</summary>
        public static readonly byte[] Header =
        {
            0x6C, 0x38, 0x0C, 0x03
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventList commonEventList = new CommonEventList();

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventList CommonEventList
        {
            get => commonEventList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CommonEventList)));

                commonEventList = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベントリストをセットする。
        /// </summary>
        /// <param name="commonEvents">[NotNull] コモンイベントリスト</param>
        /// <exception cref="ArgumentNullException">commonEventsがnullの場合</exception>
        /// <exception cref="ArgumentException">commonEventsの要素数が0の場合</exception>
        [Obsolete("CommonEventList プロパティを直接操作してください。 Ver1.4で削除します。")]
        public void SetCommonEventList(IEnumerable<CommonEvent> commonEvents)
        {
            if (commonEvents is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            var eventList = commonEvents.ToArray();

            if (eventList.Length == 0)
                throw new ArgumentException(
                    $"{nameof(commonEvents)}の要素数は1以上である必要があります。");

            CommonEventList = new CommonEventList(eventList);
        }

        /// <summary>
        /// すべてのコモンイベントを返す。
        /// </summary>
        /// <returns>コモンイベント</returns>
        [Obsolete("CommonEventList プロパティを直接操作してください。 Ver1.4で削除します。")]
        public IEnumerable<CommonEvent> GetAllCommonEvent()
        {
            return CommonEventList.ToList();
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(CommonFileData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CommonEventList.Equals(other.CommonEventList);
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

            // コモンイベント
            result.AddRange(CommonEventList.ToBinary());

            return result.ToArray();
        }
    }
}