using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SerializableScriptableObject : ScriptableObject
{
    [SerializeField, HideInInspector] private string guid;

    public string Guid
    {
        get
        {
            return guid;
        }
        set
        {
            guid = value;
        }
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        var path = AssetDatabase.GetAssetPath(this);
        guid = AssetDatabase.AssetPathToGUID(path);
    }
#endif
}
