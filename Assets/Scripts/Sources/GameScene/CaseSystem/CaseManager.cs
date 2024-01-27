using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseManager : MonoBehaviour
{
    [SerializeField] private CaseViewController view;

    public List<int> CaseList;
    public List<int> CasePool;

    void Start()
    {
        // init caseSystem
        CaseList.Add(2000);
        view.AddCaseController(2000);

        CaseList = new List<int>();
        CasePool = new List<int>();

        InvokeRepeating("RandomCaseFromPoolToList", 10f, 30f);
    }

    void RandomCaseFromPoolToList()
    {
        Debug.Log("RandomCaseFromPoolToList");
        if (CasePool.Count <= 0)
        {
            Debug.Log("CasePool.Count <= 0");
            return;
        }

        System.Random random = new System.Random();
        int index = random.Next(CasePool.Count);
        if (!CaseList.Contains(CasePool[index]))
        {
            CaseList.Add(CasePool[index]);
            view.AddCaseController(CasePool[index]);
        }
        else
        {
            Debug.Log("CaseList.Contains(CasePool[index])");
        }
        CasePool.Remove(index);
    }
}
