using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Wave), true)]
public class WaveEditor : Editor
{
    protected Wave myTarget;

    private Texture2D makeTextureColour (Color c)
    {
        Texture2D t = new Texture2D(1,1);
        t.SetPixel(0, 0, c);
        t.Apply();
        return t;
    }
    void Awake()
    {
        myTarget = (Wave)target;

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUIStyle head = new GUIStyle();
        head.normal.background = makeTextureColour(new Color(0.690f, 0.690f, 0.690f, 1));
        

        GUIStyle body = new GUIStyle();
        body.normal.background = makeTextureColour(new Color(0.878f, 0.878f, 0.878f, 1));
        head.onHover.background = makeTextureColour(new Color(1f, 0f, 0f, 1));

        
        EditorGUIUtility.labelWidth = 100;
        EditorGUIUtility.fieldWidth = 20;


        if (GUILayout.Button("Collapse  All"))
        {
            foreach (WaveElement i in myTarget.elements)
            {
                i.vis = false;
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        for (int i = 0; i < myTarget.elements.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(head);

            GUILayout.Width(5);
            myTarget.elements[i].vis = EditorGUILayout.Foldout(myTarget.elements[i].vis, GUIContent.none);

            addField(i, "behaviour");
            if (GUILayout.Button("^", GUILayout.Width(25)))
            {
                if (i > 0)
                {
                    WaveElement temp = myTarget.elements[i];
                    myTarget.elements[i] = myTarget.elements[i - 1];
                    myTarget.elements[i - 1] = temp;
                }
            }
            if (GUILayout.Button("v", GUILayout.Width(25)))
            {
                if (i < myTarget.elements.Count - 1)
                {
                    WaveElement temp = myTarget.elements[i];
                    myTarget.elements[i] = myTarget.elements[i + 1];
                    myTarget.elements[i + 1] = temp;
                }
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                myTarget.elements.RemoveAt(i);
            }
            GUI.backgroundColor = Color.white;
            GUILayout.Label(("" + i), GUILayout.Width(25f));

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginVertical(body);
            if (myTarget.elements[i].vis)
            {
                switch (myTarget.elements[i].behaviour)
                {
                    case ElementBehaviour.BULK:
                        spawnItem(i);
                        addField(i, "bulkElements");
                        time(i, "time");
                        break;
                    case ElementBehaviour.SINGLE:
                        spawnItem(i);
                        break;
                    case ElementBehaviour.WAIT:
                        time(i, "time");
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Add New"))
        {
            myTarget.elements.Add(new WaveElement());
        }
        serializedObject.ApplyModifiedProperties();
    }
    public void addField (int index, string name)
    {
        if(index < serializedObject.FindProperty("elements").arraySize)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("elements").GetArrayElementAtIndex(index).FindPropertyRelative(name));
    }
    public void spawnItem (int index)
    {
        addField(index, "randomEnemy");
        if (!myTarget.elements[index].randomEnemy)
        {
            addField(index, "enemy");
        }
        else
        {
            addField(index, "enemySet");
        }
        addField(index, "isBoss");
    }
    public void time(int i, string name)
    {
        EditorGUILayout.BeginHorizontal();
        addField(i, "useTimeRange");
        if (myTarget.elements[i].useTimeRange)
        {
            addField(i, "timeRange");
        }
        else
        {
            addField(i, "time");
        }
        EditorGUILayout.EndHorizontal();
    }
    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        if (myTarget == null || myTarget.icon == null)
        {
            return null;
        }
        Texture2D image = new Texture2D(width, height);
        //if(image)
            //EditorUtility.CopySerialized(AssetPreview.GetAssetPreview(myTarget.icon), image);
        return null;
    }
}
