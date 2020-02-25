// ========================================
// Project Name : WodiLib
// File Name    : TileSetSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイルセット設定
    /// </summary>
    [Serializable]
    public class TileSetSetting : IEquatable<TileSetSetting>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>設定数最大値</summary>
        public static readonly int SettingLengthMax = WodiLib.Map.TileTagNumberList.MaxCapacity;

        /// <summary>設定数最小値</summary>
        public static readonly int SettingLengthMin = WodiLib.Map.TileTagNumberList.MinCapacity;

        /// <summary>セパレータ</summary>
        public static readonly byte DataSeparator = 0xFF;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private TileSetName name = new TileSetName("");

        /// <summary>
        /// [NotNull] タイルセット設定名
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public TileSetName Name
        {
            get => name;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Name)));
                name = value;
            }
        }

        private BaseTileSetFileName baseTileSetFileName = "";

        /// <summary>
        /// [NotNull] 基本タイルファイル名
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public BaseTileSetFileName BaseTileSetFileName
        {
            get => baseTileSetFileName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(BaseTileSetFileName)));
                baseTileSetFileName = value;
            }
        }

        private readonly AutoTileFileNameList autoTileFileNameList = new AutoTileFileNameList();

        /// <summary>
        /// オートタイルファイル名リスト
        /// </summary>
        public IFixedLengthAutoTileFileNameList AutoTileFileNameList => autoTileFileNameList;

        private readonly TileTagNumberList tileTagNumberList = new TileTagNumberList();

        /// <summary>
        /// タイルタグ番号リスト
        /// </summary>
        public IFixedLengthTileTagNumberList TileTagNumberList => tileTagNumberList;

        private readonly TilePathSettingList tilePathSettingList = new TilePathSettingList();

        /// <summary>
        /// タイル通行設定リスト
        /// </summary>
        public IFixedLengthTilePathSettingList TilePathSettingList => tilePathSettingList;

        /// <summary>
        /// 設定数
        /// </summary>
        public int SettingLength => TileTagNumberList.Count;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TileSetSetting()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tileTagNumbers">[Nullable] 初期タイル番号リスト</param>
        /// <param name="tilePathSettings">[Nullable] 初期タイル通行設定リスト</param>
        /// <param name="autoTileFileNames">[Nullable] 初期オートタイルファイル名リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     tileTagNumbers, tilePathSettingsのいずれか一方のみnullの場合
        /// </exception>
        /// <exception cref="ArgumentException">tileTagNumbersとtilePathSettingsの要素数が異なる場合</exception>
        public TileSetSetting(TileTagNumberList tileTagNumbers = null,
            TilePathSettingList tilePathSettings = null,
            AutoTileFileNameList autoTileFileNames = null)
        {
            if (tileTagNumbers is null && !(tilePathSettings is null))
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileTagNumbers)));
            if (!(tileTagNumbers is null) && tilePathSettings is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tilePathSettings)));

            if (!(tileTagNumbers is null))
            {
                if (tilePathSettings.Count != tileTagNumbers.Count)
                    throw new ArgumentException(
                        ErrorMessage.LengthRange(nameof(tilePathSettings), $"{nameof(tileTagNumbers)}の要素数",
                            tilePathSettings.Count));

                tileTagNumberList = tileTagNumbers;
                tilePathSettingList = tilePathSettings;
            }

            if (!(autoTileFileNames is null))
            {
                autoTileFileNameList = autoTileFileNames;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 設定数を変更する。
        /// </summary>
        /// <param name="length">[Range(TileSetSetting.SettingLengthMin, TileSetSetting.SettingLengthMax)] 設定数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        public void ChangeSettingLength(int length)
        {
            if (length < SettingLengthMin || SettingLengthMax < length)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(length), SettingLengthMin, SettingLengthMax, length));

            tileTagNumberList.AdjustLength(length);
            tilePathSettingList.AdjustLength(length);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(TileSetSetting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return name.Equals(other.name)
                   && baseTileSetFileName.Equals(other.baseTileSetFileName)
                   && autoTileFileNameList.Equals(other.autoTileFileNameList)
                   && tileTagNumberList.Equals(other.tileTagNumberList)
                   && tilePathSettingList.Equals(other.tilePathSettingList);
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

            // 設定名
            result.AddRange(new WoditorString(Name).StringByte);

            // 基本タイルセットファイル名
            result.AddRange(new WoditorString(BaseTileSetFileName).StringByte);

            // オートタイルファイル名
            result.AddRange(autoTileFileNameList.ToBinary());

            // 固定値
            result.Add(DataSeparator);

            // タグ番号リスト
            result.AddRange(tileTagNumberList.ToBinary());

            // 固定値
            result.Add(DataSeparator);

            // 通行設定
            result.AddRange(tilePathSettingList.ToBinary());

            return result.ToArray();
        }
    }
}