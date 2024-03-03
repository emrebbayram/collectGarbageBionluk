using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemObj : MonoBehaviour
{
    [SerializeField]
    private item _item;

    public item item => _item;
}
