using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class ListMemberInfoDrawer<T> : MemberInfoDrawer<List<T>>
    {
        private bool _foldOut;
        
        public ListMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }

        public override void OnDrawGUI()
        {
            _foldOut = EditorGUILayout.Foldout(_foldOut, _memberInfo.Name);
            
            if(!_foldOut)
                return;

            EditorGUILayout.BeginVertical("box");
            base.OnDrawGUI();

            var value = GetPropertyValue();
            if (value == null)
            {
                if (GUILayout.Button("NULL")) 
                    SetValue(new List<T>());

                EditorGUILayout.EndVertical();
                return;
            }

            for (var ii = 0; ii < value.Count; ii++)
            {
                var piece = value[ii];
                PieceGUI(piece, ii);
            }
            
            EditorGUILayout.EndVertical();
        }

        private void PieceGUI(T piece, int listIndex)
        {
        }
    }
}