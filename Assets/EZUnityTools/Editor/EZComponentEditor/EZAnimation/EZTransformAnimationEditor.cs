/*
 * Author:      熊哲
 * CreateTime:  11/2/2017 5:30:26 PM
 * Description:
 * 
*/
using EZComponent.EZAnimation;
using UnityEditor;
using UnityEngine;

namespace EZComponentEditor.EZAnimation
{
    [CustomEditor(typeof(EZTransformAnimation), true), CanEditMultipleObjects]
    public class EZTransformAnimationEditor : EZAnimationEditor
    {
        private SerializedProperty m_DrivePosition;
        private SerializedProperty m_DriveRotation;
        private SerializedProperty m_DriveScale;

        private bool originFoldout = true;

        private Color positionColor = Color.red;
        private Color rotationColor = Color.green;
        private Color scaleColor = Color.blue;

        protected override void OnEnable()
        {
            m_DrivePosition = serializedObject.FindProperty("m_DrivePosition");
            m_DriveRotation = serializedObject.FindProperty("m_DriveRotation");
            m_DriveScale = serializedObject.FindProperty("m_DriveScale");
            base.OnEnable();
            phaseList.elementHeightCallback = GetPhaseListElementHeight;
        }

        protected override void DrawPhaseListHeader(Rect rect)
        {
            rect.x += headerIndent; rect.width -= headerIndent; rect.y += 1;
            float width = rect.width / 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Start Value");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "End Value");
        }

        private int GetLineCount()
        {
            int line = 1;
            if (m_DrivePosition.boolValue) line++;
            if (m_DriveRotation.boolValue) line++;
            if (m_DriveScale.boolValue) line++;
            return line;
        }
        private float GetPhaseListElementHeight(int index)
        {
            return GetLineCount() * phaseList.elementHeight;
        }
        protected override void DrawPhaseListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty phase = phaseList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty startValue = phase.FindPropertyRelative("m_StartValue");
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty curve = phase.FindPropertyRelative("m_Curve");

            rect.y += 1; float width = rect.width / 2;

            float x1 = rect.x, x2 = rect.x + width, space = 10;
            Color bgColor = GUI.backgroundColor;
            if (m_DrivePosition.boolValue)
            {
                GUI.backgroundColor = positionColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Position"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Position"), GUIContent.none);
                rect.y += rect.height;
            }
            if (m_DriveRotation.boolValue)
            {
                GUI.backgroundColor = rotationColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                rect.y += rect.height;
            }
            if (m_DriveScale.boolValue)
            {
                GUI.backgroundColor = scaleColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Scale"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Scale"), GUIContent.none);
                rect.y += rect.height;
            }
            GUI.backgroundColor = bgColor;

            float labelWidth = 60; float propertyWidth = width - labelWidth - space;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, lineHeight), "Duration");
            EditorGUI.PropertyField(new Rect(rect.x + labelWidth, rect.y, propertyWidth, lineHeight), duration, GUIContent.none);
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, lineHeight), "Curve");
            curve.animationCurveValue = EditorGUI.CurveField(new Rect(rect.x + labelWidth, rect.y, propertyWidth, lineHeight), curve.animationCurveValue, Color.green, new Rect(0, 0, duration.floatValue, 1));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZUnityEditor.EZEditorGUIUtility.ScriptTitle(target);
            EditorGUILayout.PropertyField(m_Loop);
            EditorGUILayout.PropertyField(m_RestartOnEnable);
            originFoldout = EditorGUILayout.Foldout(originFoldout, "Driver");
            if (originFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_DrivePosition);
                EditorGUILayout.PropertyField(m_DriveRotation);
                EditorGUILayout.PropertyField(m_DriveScale);
                EditorGUI.indentLevel--;
            }
            phaseList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}