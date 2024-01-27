using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoachLite;
using RoachLite.Common;
using RoachLite.Services;
using RoachLite.Services.Broadcast;
using RoachLite.Services.Message;

public class CaseManager : MonoBehaviour
{
    [SerializeField] private CaseViewController view;

    public List<int> CaseList;
    public List<int> CasePool;

    private void Awake()
    {
        RoachLite.UniverseController.Initialize();
    }

    void Start()
    {
        // init caseSystem
        CaseList.Add(2000);
        view.CurrentObj = view.AddCaseController(2000);
        view.UpdateView();

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
