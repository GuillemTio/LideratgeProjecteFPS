using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CPoolElements
{
    private List<GameObject> m_ElementsList = new();
    private int m_CurrentIndex;
    private GameObject m_Prefab;

    public CPoolElements (GameObject prefab, int num, Transform parent)
    {
        m_Prefab = prefab;
    }

    public GameObject GetNextElement()
    {
        return GameObject.Instantiate(m_Prefab, new Vector3(-1000,-1000,0f),Quaternion.identity);
    }
}
