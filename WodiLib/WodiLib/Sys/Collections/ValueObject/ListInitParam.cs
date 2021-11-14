using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// コンストラクタパラメータ
    /// </summary>
    [CommonMultiValueObject]
    public partial record ListInitParam<T> : IContainerCreatableParam
    {
        /// <inheritdoc cref="IContainerCreatableParam.KeyName"/>
        public WodiLibContainerKeyName KeyName { get; init; }

        /// <summary>初期化要素</summary>
        public IEnumerable<T> InitItems { get; init; }
    }
}
