using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestoyer : MonoBehaviour
{
    public float time;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
