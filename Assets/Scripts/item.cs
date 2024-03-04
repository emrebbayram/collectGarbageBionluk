using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="item",menuName ="create item")]

public class item : ScriptableObject
{
    public enum garbageType
    {
        None = 0,
        plastic,
        glass,
        metal,
        paper
    }

    [SerializeField]
    private GameObject _obj;
    [SerializeField]
    private float _previewDistance = 5f;
    [SerializeField]
    private float _groundDistance = 1f;
    [SerializeField]
    private garbageType _garbageType;

    public garbageType GarbageType => _garbageType;
    public GameObject Object => _obj;
    public float PreviewDistance => _previewDistance;
    public float groundDistance => _groundDistance;
}
