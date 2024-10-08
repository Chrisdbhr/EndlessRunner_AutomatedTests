using System.Runtime.Serialization;

namespace Tests.Utils
{
    public static class TestableMonobehaviourFactory
    {
        public static T Create<T>() {
            return (T)FormatterServices.GetUninitializedObject(typeof(T));
        }
    }
}