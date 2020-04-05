// ========================================
// Project Name : WodiLib
// File Name    : GameIniData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// Game.iniデータクラス
    /// </summary>
    [Serializable]
    public class GameIniData : ModelBase<GameIniData>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>BGM再生コード値</summary>
        private const int UseBgmCode = 2;

        /// <summary>SE再生コード値</summary>
        private const int UseSeCode = 1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int startCode;

        /// <summary>
        /// Game.exe起動済み種別コード
        /// </summary>
        public int StartCode
        {
            get => startCode;
            set
            {
                startCode = value;
                NotifyPropertyChanged();
            }
        }

        private bool isSoftGraphicMode;

        /// <summary>
        /// ソフトウェア表示モードフラグ
        /// </summary>
        public bool IsSoftGraphicMode
        {
            get => isSoftGraphicMode;
            set
            {
                isSoftGraphicMode = value;
                NotifyPropertyChanged();
            }
        }

        private bool isWindowMode;

        /// <summary>
        /// ウィンドウモードフラグ
        /// </summary>
        public bool IsWindowMode
        {
            get => isWindowMode;
            set
            {
                isWindowMode = value;
                NotifyPropertyChanged();
            }
        }

        private bool isPlayBgm;

        /// <summary>
        /// BGM再生フラグ
        /// </summary>
        public bool IsPlayBgm
        {
            get => isPlayBgm;
            set
            {
                isPlayBgm = value;
                NotifyPropertyChanged();
            }
        }

        private bool isPlaySe;

        /// <summary>
        /// SE再生フラグ
        /// </summary>
        public bool IsPlaySe
        {
            get => isPlaySe;
            set
            {
                isPlaySe = value;
                NotifyPropertyChanged();
            }
        }

        private FrameSkipType frameSkipType = FrameSkipType.HighSpec;

        /// <summary>
        /// [NotNull] フレームスキップ種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public FrameSkipType FrameSkipType
        {
            get => frameSkipType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(FrameSkipType)));

                frameSkipType = value;
                NotifyPropertyChanged();
            }
        }

        private ProxyAddress proxyAddress = "";

        /// <summary>
        /// [NotNull] プロキシアドレス
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ProxyAddress ProxyAddress
        {
            get => proxyAddress;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ProxyAddress)));

                proxyAddress = value;
                NotifyPropertyChanged();
            }
        }

        private ProxyPort proxyPort;

        /// <summary>
        /// プロキシポート
        /// </summary>
        public ProxyPort ProxyPort
        {
            get => proxyPort;
            set
            {
                proxyPort = value;
                NotifyPropertyChanged();
            }
        }

        private bool canTakeScreenShot;

        /// <summary>
        /// スクリーンショット許可フラグ
        /// </summary>
        public bool CanTakeScreenShot
        {
            get => canTakeScreenShot;
            set
            {
                canTakeScreenShot = value;
                NotifyPropertyChanged();
            }
        }

        private bool canReset;

        /// <summary>
        /// 【Ver2.20以降】F12リセット許可フラグ
        /// </summary>
        public bool CanReset
        {
            get => canReset;
            set
            {
                canReset = value;
                NotifyPropertyChanged();
            }
        }

        private DisplayNumber displayNumber;

        /// <summary>
        /// 【Ver2.20以降】起動ディスプレイ番号
        /// </summary>
        public DisplayNumber DisplayNumber
        {
            get => displayNumber;
            set
            {
                displayNumber = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUseOldDirectX;

        /// <summary>
        /// 【Ver2.22以降】旧DirectXバージョン利用フラグ
        /// </summary>
        public bool IsUseOldDirectX
        {
            get => isUseOldDirectX;
            set
            {
                isUseOldDirectX = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameIniData()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">Iniデータ</param>
        internal GameIniData(GameIniRootData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            StartCode = data.Start.TryToInt() ?? 0;
            IsSoftGraphicMode = data.SoftModeFlag.TryToInt() == 1;
            IsWindowMode = data.WindowModeFlag.TryToInt() == 1;
            var seAndBgm = data.SEandBGM.TryToInt() ?? 0;
            IsPlayBgm = (seAndBgm & UseBgmCode) != 0;
            IsPlaySe = (seAndBgm & UseSeCode) != 0;
            try
            {
                FrameSkipType = FrameSkipType.FromCode(data.FrameSkip);
            }
            catch
            {
                FrameSkipType = FrameSkipType.HighSpec;
            }

            ProxyAddress = data.Proxy ?? "";
            ProxyPort = data.ProxyPort.TryToInt() ?? ProxyPort.Empty;
            CanTakeScreenShot = data.ScreenShotFlag.TryToInt() == 1;
            CanReset = data.F12_Reset.TryToInt() == 1;
            DisplayNumber = data.Display_Number.TryToInt() ?? 0;
            IsUseOldDirectX = data.Display_Number.TryToInt() == 1;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(GameIniData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return frameSkipType == other.frameSkipType
                   && proxyAddress == other.proxyAddress
                   && StartCode == other.StartCode
                   && IsSoftGraphicMode == other.IsSoftGraphicMode
                   && IsWindowMode == other.IsWindowMode
                   && IsPlayBgm == other.IsPlayBgm
                   && IsPlaySe == other.IsPlaySe
                   && ProxyPort == other.ProxyPort
                   && CanTakeScreenShot == other.CanTakeScreenShot
                   && CanReset == other.CanReset
                   && DisplayNumber == other.DisplayNumber
                   && IsUseOldDirectX == other.IsUseOldDirectX;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自身のプロパティ値をGameIniRootDataインスタンスに変換する。
        /// </summary>
        /// <returns>GameIniRootDataインスタンス</returns>
        internal GameIniRootData ToIniRootData()
        {
            return new GameIniRootData
            {
                Start = StartCode.ToString(),
                SoftModeFlag = IsSoftGraphicMode.ToIntString(),
                WindowModeFlag = IsWindowMode.ToIntString(),
                SEandBGM = ((IsPlayBgm ? UseBgmCode : 0)
                            + (IsPlaySe ? UseSeCode : 0)).ToString(),
                FrameSkip = FrameSkipType.Code,
                Proxy = ProxyAddress.ToString(),
                ProxyPort = ProxyPort.ToString(),
                ScreenShotFlag = CanTakeScreenShot.ToIntString(),
                F12_Reset = CanReset.ToIntString(),
                Display_Number = DisplayNumber.ToString(),
                Old_DirectX_Use = IsUseOldDirectX.ToIntString(),
            };
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
            info.AddValue(nameof(StartCode), StartCode);
            info.AddValue(nameof(IsSoftGraphicMode), IsSoftGraphicMode);
            info.AddValue(nameof(IsWindowMode), IsWindowMode);
            info.AddValue(nameof(IsPlayBgm), IsPlayBgm);
            info.AddValue(nameof(IsPlaySe), IsPlaySe);
            info.AddValue(nameof(frameSkipType), frameSkipType.Code);
            info.AddValue(nameof(proxyAddress), proxyAddress);
            info.AddValue(nameof(ProxyPort), ProxyPort);
            info.AddValue(nameof(CanTakeScreenShot), CanTakeScreenShot);
            info.AddValue(nameof(CanReset), CanReset);
            info.AddValue(nameof(DisplayNumber), DisplayNumber);
            info.AddValue(nameof(IsUseOldDirectX), IsUseOldDirectX);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected GameIniData(SerializationInfo info, StreamingContext context)
        {
            StartCode = info.GetValue<int>(nameof(StartCode));
            IsSoftGraphicMode = info.GetBoolean(nameof(IsSoftGraphicMode));
            IsWindowMode = info.GetBoolean(nameof(IsWindowMode));
            IsPlayBgm = info.GetBoolean(nameof(IsPlayBgm));
            IsPlaySe = info.GetBoolean(nameof(IsPlaySe));
            frameSkipType = FrameSkipType.FromCode(info.GetValue<string>(nameof(frameSkipType)));
            proxyAddress = info.GetValue<ProxyAddress>(nameof(proxyAddress));
            ProxyPort = info.GetInt32(nameof(ProxyPort));
            CanTakeScreenShot = info.GetBoolean(nameof(CanTakeScreenShot));
            CanReset = info.GetBoolean(nameof(CanReset));
            DisplayNumber = info.GetInt32(nameof(DisplayNumber));
            IsUseOldDirectX = info.GetBoolean(nameof(IsUseOldDirectX));
        }
    }
}