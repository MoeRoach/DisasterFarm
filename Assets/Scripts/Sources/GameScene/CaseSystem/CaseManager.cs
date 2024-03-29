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
        // CaseList.Add(2000);
        // view.AddCaseController(2000);
        // view.CurrentObj = view.AddCaseController(2000);
        // view.UpdateView();

        CaseList.Add(2001);
        view.AddCaseController(2001);
        CaseList.Add(2004);
        view.AddCaseController(2004);
        CaseList.Add(2007);
        view.AddCaseController(2007);
        CaseList.Add(2010);
        view.AddCaseController(2010);
        CaseList.Add(2013);
        view.AddCaseController(2013);
        CaseList.Add(2016);
        view.AddCaseController(2016);

        CaseList.Add(3002);
        view.AddCaseController(3002);
        CaseList.Add(3005);
        view.AddCaseController(3005);

        CaseList.Add(4001);
        view.AddCaseController(4001);

        CaseList = new List<int>();
        CasePool = new List<int>();

        InvokeRepeating("RandomCaseFromPoolToList", 10f, 3f);
    }

    void Update()
    {
        if (view.CurrentObj == null)
        {
            view.UpdateFromCaseList();
        }
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
