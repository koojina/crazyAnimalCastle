using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    public GameObject Parent;
    private GameObject[] Chiled_objects;

    public void Start()
    {
        Chiled_objects = GetChild(Parent);
        Debug.Log(transform.childCount);
        for(int i = 0; i < transform.childCount; i ++)
        {
            if(Chiled_objects[i].transform.position.x % 2 != 0 )
            {
                Debug.Log(Chiled_objects[i].name);
            }
        }
    }

    GameObject[] GetChild(GameObject parent)
    {
        GameObject[] Child = new GameObject[parent.transform.childCount];
        for(int i = 0; i < parent.transform.childCount; i++)
        {
            Child[i] = parent.transform.GetChild(i).gameObject;
        }
        return Child;
    }
}