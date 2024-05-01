using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WavePlayer), true)]
public class WavePlayerEditor : Editor
{
    protected WavePlayer myTarget;
    public bool showEvents = false;
    public bool currentlyAlive = false;
    public Vector2 scrollPosition;

    void Awake()
    {
        myTarget = (WavePlayer)target;
    }

    /// <summary>
    /// Draws debug spawn locations to the viewport
    /// </summary>
    public void OnSceneGUI()
    {
        if (myTarget.debugSpawnPosition)
        {
            Vector3 spawnPos = myTarget.GetRandomSpawnPos() != null ? myTarget.GetRandomSpawnPos().position : Vector3.zero;
            Vector3 a = (!myTarget.spawnAtObject | myTarget.spawnObjects == null) ? myTarget.spawnlocation : spawnPos;
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(a.x + myTarget.randomSpawnPosition.x, a.y, a.z), new Vector3(a.x - myTarget.randomSpawnPosition.x, a.y, a.z));
            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(a.x, a.y + myTarget.randomSpawnPosition.y, a.z), new Vector3(a.x, a.y - myTarget.randomSpawnPosition.y, a.z));
            Handles.color = Color.blue;
            Handles.DrawLine(new Vector3(a.x, a.y, a.z + myTarget.randomSpawnPosition.z), new Vector3(a.x, a.y, a.z - myTarget.randomSpawnPosition.z));
        }

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        drawWaveSet();
        drawSpawnSettings();
        drawEvents();
        drawAlive();

        EditorUtility.SetDirty(myTarget);
        serializedObject.ApplyModifiedProperties();
    }

    public void drawWaveSet ()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        for (int i = 0; i < myTarget.WaveSet.Count; i++)
        {
            GUILayout.BeginHorizontal();
                GUILayout.Label("Wave " + i + ":", GUILayout.Width(70));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WaveSet").GetArrayElementAtIndex(i), GUIContent.none);

                if (GUILayout.Button("^", GUILayout.Width(25)))
                {
                    if (i > 0)
                    {
                        Wave temp = myTarget.WaveSet[i];
                        myTarget.WaveSet[i] = myTarget.WaveSet[i - 1];
                        myTarget.WaveSet[i - 1] = temp;
                    }
                }
                if (GUILayout.Button("v", GUILayout.Width(25)))
                {
                    if (i < myTarget.WaveSet.Count - 1)
                    {
                        Wave temp = myTarget.WaveSet[i];
                        myTarget.WaveSet[i] = myTarget.WaveSet[i + 1];
                        myTarget.WaveSet[i + 1] = temp;
                    }   
                }
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    myTarget.WaveSet.RemoveAt(i);
                }
                GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            myTarget.WaveSet.Add(new Wave());
        }
    }
    public void drawSpawnSettings ()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Spawn Location");
        GUILayout.BeginHorizontal();
        myTarget.spawnAtObject = GUILayout.Toggle(myTarget.spawnAtObject, "Spawn at Object");
        if (!myTarget.spawnAtObject)
        {
            GUILayout.Label("Spawn Location: ");
            myTarget.spawnlocation = EditorGUILayout.Vector3Field(GUIContent.none, myTarget.spawnlocation);
        }
        else
        {
            GUILayout.Label("Spawn At: ");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnObjects"), GUIContent.none);
        }
        GUILayout.EndHorizontal();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("randomSpawnPosition"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("debugSpawnPosition"));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    public void drawEvents ()
    {
        

        showEvents = EditorGUILayout.Foldout(showEvents, "Events");
        if(showEvents)
        {
            GUILayout.Label("Called after the final wave in a set has finished");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("finalWaveComplete"));
            GUILayout.Label("Called after all enemies in a wave have been spawned");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("allSpawned"));
            GUILayout.Label("Called after all enemies in a wave have been killed");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("waveHasEnded"));
            GUILayout.Label("Called when a wave begins");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("waveBegins")); 
            GUILayout.Label("Called when a wave is reset");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("resetWave"));
        }
    }
    public void drawAlive ()
    {
        currentlyAlive = EditorGUILayout.Foldout(currentlyAlive, "Currently Alive In Wave");
        if (currentlyAlive)
        {
            for (int i = 0; i < myTarget.alive.Count; i++)
            {
                if(myTarget.alive[i] != null)
                {
                    GUILayout.Label(myTarget.alive[i].name);
                } 
            }
            GUILayout.Label("Total: " + myTarget.alive.Count);
        }
    }
}

