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
    private Sprite _image;
    [SerializeField]
    private string _name;
    [SerializeField]
    private garbageType _garbageType;

    public Sprite Image => _image;
    public string Name => _name;
    public garbageType GarbageType => _garbageType;
}
