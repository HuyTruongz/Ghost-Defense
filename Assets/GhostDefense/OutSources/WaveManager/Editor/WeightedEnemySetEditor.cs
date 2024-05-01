using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(WeightedEnemySet), true)]
public class WeightedEnemySetEditor : Editor
{
    protected WeightedEnemySet myTarget;
    void Awake()
    {
        myTarget = (WeightedEnemySet)target;
    }

    public override void OnInspectorGUI()
    {
        myTarget.updateTotal();
        serializedObject.Update();
        EditorGUIUtility.labelWidth = 50;
        EditorGUIUtility.fieldWidth = 20;

        if (GUILayout.Button("Add"))
        {
            myTarget.enemies.Add(new WeightedEnemy());
        }

        for (int i = 0; i < myTarget.enemies.Count; i ++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 60;
            float per = (float)myTarget.enemies[i].weight / myTarget.total * 100.0f;

            GUILayout.Label(Math.Round(per,2) + "%", GUILayout.MaxWidth(50));
            if (myTarget.enemies[i].weight < 0)
            {
                myTarget.enemies[i].weight = 0;
            }

            var enemies = serializedObject.FindProperty("enemies");

            if(enemies.arraySize > i)
            {
                var enemy = enemies.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("spawn"));
                EditorGUILayout.PropertyField(enemy.FindPropertyRelative("weight"));
            }
           

            if (GUILayout.Button("^", GUILayout.Width(25)))
            {
                if(i > 0)
                {
                    WeightedEnemy temp = myTarget.enemies[i];
                    myTarget.enemies[i] = myTarget.enemies[i - 1];
                    myTarget.enemies[i - 1] = temp;
                }
            }
            if (GUILayout.Button("v", GUILayout.Width(25)) )
            {
                if (i < myTarget.enemies.Count -1)
                {
                    WeightedEnemy temp = myTarget.enemies[i];
                    myTarget.enemies[i] = myTarget.enemies[i + 1];
                    myTarget.enemies[i + 1] = temp;
                }
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                myTarget.enemies.RemoveAt(i);
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }
    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        return null;
    }
}
