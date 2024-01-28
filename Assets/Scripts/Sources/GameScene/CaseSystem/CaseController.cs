using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class CaseController : MonoBehaviour
{

    public CaseData Data;
    public CaseViewController View;
    private Image BG;
    [SerializeField] Image signal;
    private float timer = 0;

    public void ClearInfo()
    {
        Debug.Log($"ClearInfo");
        Data = null;
    }

    void Start()
    {

    }

    void Update()
    {
        TimeOutProcess();
    }

    void TimeOutProcess()
    {
        timer += Time.deltaTime;
        GetComponent<Image>().fillAmount = 1.0f - Mathf.Lerp(0, 1, timer / Data.timeout);
        if (GetComponent<Image>().fillAmount <= 0)
        {
            View.ClearView();
            CaseViewController.Instance.AddToPool(Data.caseId);
            Destroy(gameObject);
        }
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
        Data = JsonUtility.FromJson<CaseData>(fileContent);
        Debug.Log(Data.color);
        ColorUtility.TryParseHtmlString(Data.color, out Color color);
        GetComponent<Image>().color = color;

        signal.sprite = SpriteUtils.GetCaseSignalSprite(Data.signal);
    }


    public void Click()
    {
        Debug.Log("Click");
        if (View == null)
        {
            Debug.Log("View == null");
            return;
        }
        if (Data == null)
        {
            Debug.Log("Data == null");
            return;
        }
        View.CurrentObj = gameObject;
        View.UpdateView();
        // Debug.Log($"add case {gameObject.GetComponent<CaseController>().Data.caseId}");
        // Debug.Log($"add case {View.CurrentObj.GetComponent<CaseController>().Data.caseId}");
    }
}
