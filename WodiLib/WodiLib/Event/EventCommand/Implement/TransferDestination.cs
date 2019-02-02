// ========================================
// Project Name : WodiLib
// File Name    : TransferDestination.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・場所移動（移動先指定）
    /// </summary>
    public class TransferDestination : TransferBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.TransferDestination;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>対象</summary>
        public int Target
        {
            get => _Target;
            set => _Target = value;
        }

        /// <summary>移動先マップ</summary>
        public int DestinationMapId
        {
            get => _DestinationMapId;
            set => _DestinationMapId = value;
        }

        /// <summary>同じマップ</summary>
        public bool IsSameMap
        {
            get => _IsSameMap;
            set => _IsSameMap = value;
        }

        /// <summary>移動先座標X</summary>
        public int PositionX
        {
            get => _PositionX;
            set => _PositionX = value;
        }

        /// <summary>移動先座標Y</summary>
        public int PositionY
        {
            get => _PositionY;
            set => _PositionY = value;
        }

        /// <summary>精密座標</summary>
        public bool IsPreciseCoordinates
        {
            get => _IsPreciseCoordinates;
            set => _IsPreciseCoordinates = value;
        }

        /// <summary>[NotNull] 場所移動時トランジションオプション</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public TransferOption TransferOption
        {
            get => _TransferOption;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TransferOption)));
                _TransferOption = value;
            }
        }
    }
}