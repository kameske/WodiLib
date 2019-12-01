using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WodiLib.Test.Tools
{
    /**
     * Serializableテスト用クラス
     * Serializable属性を持つオブジェクトのディープコピーを行う
     */
    public static class DeepCloner
    {
        public static T DeepClone<T>(T src)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, src);

            stream.Position = 0;

            var f2 = new BinaryFormatter();
            var deserialize = f2.Deserialize(stream);
            var clone = (T) deserialize;
            return clone;
        }
    }
}