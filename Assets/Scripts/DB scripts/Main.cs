using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public Web web;

    void Start()
    {
        Instance = this;
        web = GetComponent<Web>();
    }
}

