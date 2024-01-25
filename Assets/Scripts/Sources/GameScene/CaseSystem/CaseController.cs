using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class CaseController : MonoBehaviour
{

    private CaseData data;
    [SerializeField] private CaseViewController view;
    [SerializeField] private Image BG;

    public void ClearInfo()
    {
        data = null;
    }

    void Start()
    {
        CaseDataInit(2001);
    }

    void Update()
    {

    }

    public void CaseDataInit(int caseId)
    {
        Debug.Log($"CaseDataInit:{caseId}");
        ClearInfo();
        string fileContent = File.ReadAllText(Application.dataPath + $"/Scripts/Sources/GameScene/CaseSystem/Jsons/Case_{caseId}.json");
        if (string.IsNullOrEmpty(fileContent))
        {
            Debug.Log("string.IsNullOrEmpty(fileContent)");
            return;
        }
        data = JsonUtility.FromJson<CaseData>(fileContent);
        ColorUtility.TryParseHtmlString(data.color, out Color color);
        BG.color = color;
    }


    public void Click()
    {
        Debug.Log("Click");
        if (view == null)
        {
            Debug.Log("view == null");
            return;
        }
        view.data = data;
        view.UpdateView();
    }
}
