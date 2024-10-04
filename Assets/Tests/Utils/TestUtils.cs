using System;
using System.Reflection;
using NUnit.Framework;

namespace Tests.Utils
{
    public static class TestUtils
    {
        const string ErrorGettingFieldFromReflection = "Exception getting field with reflection: ";

        public static object ReflectionGetConstFieldValue(this object o, string fieldName)
        {
            try {
                return o.GetType().GetField(fieldName,BindingFlags.Static | BindingFlags.NonPublic).GetValue(o);
            }
            catch (Exception e) {
                Assert.Fail(ErrorGettingFieldFromReflection + e);
            }
            return default;
        }

        public static object ReflectionGetFieldValue(this object o, string fieldName)
        {
            try {
                return o.GetType().GetField(fieldName,BindingFlags.Instance | BindingFlags.NonPublic).GetValue(o);
            }
            catch (Exception e) {
                Assert.Fail(ErrorGettingFieldFromReflection + e);
            }
            return default;
        }

        public static void ReflectionSetFieldValue(this object o, string fieldName, object value)
        {
            try {
                o.GetType().GetField(fieldName,BindingFlags.Instance | BindingFlags.NonPublic).SetValue(o, value);
            }
            catch (Exception e) {
                Assert.Fail(ErrorGettingFieldFromReflection + e);
            }
        }
    }
}