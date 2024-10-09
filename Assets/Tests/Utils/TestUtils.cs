using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.Utils
{
    public static class TestUtils
    {
        public static object ReflectionGetConstFieldValue(this Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            if (field == null)
            {
                type = type.BaseType;
                while (type != null)
                {
                    field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
                    if (field != null)
                    {
                        break;
                    }
                    type = type.BaseType;
                }
            }

            Assert.NotNull(field);
            return field.GetRawConstantValue();
        }

        public static void LogStartTestInformation(string methodName)
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{methodName}");
        }

        public static object ReflectionGetFieldValue(this object o, string fieldName)
        {
            var field = o.GetType().GetField(fieldName,BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field, $"Field '{fieldName}' returned null from reflection");
            return field.GetValue(o);
        }

        public static void ReflectionSetFieldValue(this object o, string fieldName, object value)
        {
            var field = o.GetType().GetField(fieldName,BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field, $"Field '{fieldName}' returned null from reflection");
            field.SetValue(o, value);
        }

        public static void SimulateButtonClick(Button button)
        {
            Assert.NotNull(button, "Button to simulate click is null");
            button.onClick?.Invoke();
        }

        public static Material GetNewTestMaterial()
        {
            var material = new Material(Shader.Find(TestStrings.TestShaderName));
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Mode", 2);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            material.color = new Color(1f, 0f, 0f, .6f);
            return material;
        }

        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

    }
}