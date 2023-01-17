using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CometaPrototypes.CharacterController2D
{
    [CustomEditor(typeof(CharacterController2D))]
    [CanEditMultipleObjects]
    public class CharacterController2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterController2D controller = (CharacterController2D)target;

            if (controller.State != null)
            {
                EditorGUILayout.LabelField("Grounded", controller.State.IsGrounded.ToString());
                EditorGUILayout.LabelField("Falling", controller.State.IsFalling.ToString());
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Colliding Below", controller.State.IsGrounded.ToString());
                EditorGUILayout.Space();

            }

            DrawDefaultInspector();
        }
    }
}