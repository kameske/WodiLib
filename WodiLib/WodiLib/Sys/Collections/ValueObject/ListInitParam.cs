using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// コンストラクタパラメータ
    /// </summary>
    [CommonMultiValueObject]
    public partial record ListInitParam<T> : IContainerCreatableParam
    {
        /// <inheritdoc cref="IContainerCreatableParam.ContainerKeyName"/>
        public WodiLibContainerKeyName ContainerKeyName { get; init; } = WodiLibContainer.TargetKeyName;

        /// <summary>初期化要素</summary>
        public IEnumerable<T> InitItems { get; init; } = Array.Empty<T>();
    }
}
