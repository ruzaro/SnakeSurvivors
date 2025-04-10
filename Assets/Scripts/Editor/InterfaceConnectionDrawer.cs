using System.Linq;
using Pickups;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(InterfaceField<>))]
    public class InterfaceConnectionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var objProp = property.FindPropertyRelative("obj");

            var label = new Label();
            var objectField = new ObjectField
            {
                objectType = typeof(Object),
                allowSceneObjects = true,
                value = objProp.objectReferenceValue
            };

            var interfaceName = GetGenericTypeName(property);

            label.text = property.name;

            var inputField = objectField.hierarchy[0].hierarchy[0].hierarchy[1] as Label;
            
            UpdateLabel(inputField, objProp.objectReferenceValue?.name, interfaceName);

            inputField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue == "None (Object)")
                {
                    UpdateLabel(inputField, null, interfaceName);
                    return;
                }
                
                UpdateLabel(inputField, evt.newValue, interfaceName);
            });
            
            var selector = objectField.hierarchy[0].hierarchy[1];
            selector.style.display = DisplayStyle.None;

            objectField.RegisterValueChangedCallback(evt =>
            {
                if (IsValidInterfaceTarget(evt.newValue, interfaceName))
                {
                    objProp.objectReferenceValue = evt.newValue;
                    objProp.serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    objectField.SetValueWithoutNotify(objProp.objectReferenceValue);
                }
            });

            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            
            label.style.flexGrow = 1;
            objectField.style.flexGrow = 1;
            
            row.Add(label);
            row.Add(objectField);
            
            container.Add(row);

            return container;
        }

        private static void UpdateLabel(Label label, string value, string interfaceName) => 
            label.text = value ?? $"None ({interfaceName})";

        private static string GetGenericTypeName(SerializedProperty property)
        {
            var typeOf = property.boxedValue.GetType();
            var firstGeneric = typeOf.GenericTypeArguments[0];
            return firstGeneric.Name;
        }

        private static bool IsValidInterfaceTarget(Object obj, string interfaceName)
        {
            if (obj == null) return true;

            var objType = obj.GetType();
            var interfaces = objType.GetInterfaces();

            return obj switch
            {
                GameObject go => go.GetComponent(interfaceName) != null,
                Component comp => comp.GetComponent(interfaceName) != null,
                _ => interfaces.Any(i => i.Name == interfaceName)
            };
        }
    }
}