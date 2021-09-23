using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Attributes.BinaryOperateAttribute;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Binary
{
    /// <summary>
    ///     プロパティ値ディクショナリラッパー
    /// </summary>
    internal class PropertyValueResolver
    {
        public string OperationCode => Values[MyAttr.Operation.Name]!;
        public string[] OtherTypes => Values.GetArrayValue(MyAttr.OtherTypes.Name)!;
        public string InnerCastType => Values[MyAttr.InnerCastType.Name]!;
        public string ReturnType => Values[MyAttr.ReturnType.Name]!;
        public bool IsLeft => OtherPosition.Equals(BinaryOperateOtherPosition.Code_Left.ToString());

        private string OtherPosition => Values[MyAttr.OtherPosition.Name]!;

        private PropertyValues Values { get; }

        public PropertyValueResolver(PropertyValues values)
        {
            Values = values;
        }
    }
}
