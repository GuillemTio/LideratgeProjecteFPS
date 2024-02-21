using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class CPoolElements
{
    private List<GameObject> m_ElementsList = new();
    int m_CurrentIndex;

    public CPoolElements (GameObject prefab, int num, Transform parent)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject l_Instance = GameObject.Instantiate(prefab);
            m_ElementsList.Add(l_Instance);
            l_Instance.transform.SetParent(parent);
        }
    }

    public GameObject GetNextElement()
    {
        m_CurrentIndex++;
        if (m_CurrentIndex >= m_ElementsList.Count)
            m_CurrentIndex = 0;
        return m_ElementsList[m_CurrentIndex];
    }
}
