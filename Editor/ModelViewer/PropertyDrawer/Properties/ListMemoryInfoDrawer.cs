using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class ListMemoryInfoDrawer<T> : MemoryInfoDrawer<List<T>>
    {
        private List<MemoryInfoDrawerBase> _drawersList;
        private Type _propertyDrawerType;
        
        private bool _foldOut;
        
        public ListMemoryInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
            _drawersList = new List<MemoryInfoDrawerBase>();
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

            if (value.Count != _drawersList.Count)
                RegeneratePropertyDrawerList();
            
            for (var ii = 0; ii < value.Count; ii++)
            {
                var piece = value[ii];
                PieceGUI(piece, ii);
            }
            
            EditorGUILayout.EndVertical();
        }

        private void RegeneratePropertyDrawerList()
        {
        }

        private void PieceGUI(T piece, int listIndex)
        {
        }
    }
}