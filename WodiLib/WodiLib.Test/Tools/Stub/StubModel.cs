// ========================================
// Project Name : WodiLib.Test
// File Name    : StubModel.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="ModelBase{TChild}"/> スタブ用
    /// </summary>
    public class StubModel : ModelBase<StubModel>
    {
        public string StringValue
        {
            get => stringValue;
            set => SetField(ref stringValue, value);
        }

        private string stringValue;

        public StubModel(string str = "")
        {
            stringValue = str;
        }

        public override bool ItemEquals(StubModel? other)
        {
            return other is not null && StringValue.Equals(other.StringValue);
        }

        public override StubModel DeepClone()
        {
            return new StubModel(StringValue);
        }
    }
}
