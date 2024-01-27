using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaseViewController : MonoBehaviour
{
    public GameObject CurrentObj { get; set; }
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private TextMeshProUGUI imageTitle;
    [SerializeField] private TextMeshProUGUI buttonText_1;
    [SerializeField] private TextMeshProUGUI buttonText_2;
    [SerializeField] private TextMeshProUGUI buttonText_3;
    // private int selectIndex;
    [SerializeField] private Transform viewTrans;
    [SerializeField] private CaseManager caseManager;

    public void ClearView()
    {
        Debug.Log($"ClearView");
        if (CurrentObj == null)
        {
            Debug.Log("CurrentObj == null");
            return;
        }

        Destroy(CurrentObj);

        CurrentObj = null;
        // selectIndex = -1;
        content.text = "";
        imageTitle.text = "";
        buttonText_1.text = "";
        buttonText_2.text = "";
        buttonText_3.text = "";
    }

    public void UpdateView()
    {
        if (CurrentObj == null)
        {
            Debug.Log("CurrentObj == null");
            return;
        }

        CaseData data = CurrentObj.GetComponent<CaseController>().Data;
        // Debug.Log(CurrentObj.GetComponent<CaseController>());
        // Debug.Log(data);
        // Debug.Log(data.image);
        image.sprite = Resources.Load(data.image, typeof(Sprite)) as Sprite;
        // Debug.Log(Resources.Load(data.image, typeof(Sprite)));
        content.text = data.caseContent;
        imageTitle.text = data.imageTitle;
        buttonText_1.text = data.chooseList[1].text;
        buttonText_2.text = data.chooseList[2].text;
        buttonText_3.text = data.chooseList[3].text;
    }

    public void AddToCaseList(int caseId)
    {
        //todo
    }

    void CaseTimeOut()
    {
        Debug.Log($"CaseTimeOut");
        ParseChoose(0);
    }

    public void ClickBtn_1()
    {
        Debug.Log($"ClickBtn_1");
        ParseChoose(1);
    }

    public void ClickBtn_2()
    {
        Debug.Log($"ClickBtn_2");
        ParseChoose(2);
    }

    public void ClickBtn_3()
    {
        Debug.Log($"ClickBtn_3");
        ParseChoose(3);
    }

    void ParseChoose(int index)
    {
        caseManager.CaseList.Remove(CurrentObj.GetComponent<CaseController>().Data.caseId);

        string pass = "", fail = "";
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[index].Pass)
        {
            pass += str + " ";
            ParseAction(str);
        }
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[index].Fail)
        {
            fail += str;
            ParseAction(str);
        }
        Debug.Log($"pass:[{pass}],fail:[{fail}]");
    }

    public void UpdateFromCaseList()
    {
        Debug.Log($"UpdateFromCaseList");
        Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
        if (childTransforms.Length <= 0)
        {
            Debug.Log($"childTransforms.Length <= 0");
            return;
        }

        CurrentObj = childTransforms[0].gameObject;
        UpdateView();
    }

    void ParseAction(string str)
    {
        string[] result = str.Split("_");
        if (result.Length < 2)
        {
            Debug.Log("result.Length < 2");
            return;
        }
        switch (result[0])
        {
            // case issue
            case "ListAdd":
                Debug.Log($"ListAdd {result[1]}");
                break;
            case "PoolAdd":
                Debug.Log($"PoolAdd {result[1]}");
                break;
            // attributes issue
            case "MoneyUp":
                Debug.Log($"MoneyUp {result[1]}");
                break;
            case "MoneyDown":
                Debug.Log($"MoneyDown {result[1]}");
                break;
            // action issue
            case "GoFarm":
                Debug.Log($"GoFarm {result[1]}");
                break;
            case "GoAttack":
                Debug.Log($"GoAttack {result[1]}");
                break;
            case "ThiefCome":
                Debug.Log($"ThiefCome {result[1]}");
                break;
            case "DamagerCome":
                Debug.Log($"DamagerCome {result[1]}");
                break;
            default:
                Debug.Log("default");
                break;
        }

    }

    public void AddCaseController(int caseId)
    {
        GameObject obj = Resources.Load("CaseSystemPrefabs/Case", typeof(GameObject)) as GameObject;
        Debug.Log(obj);
        obj = Instantiate(obj, viewTrans);
        obj.GetComponent<CaseController>().CaseDataInit(caseId);
        obj.GetComponent<CaseController>().View = this;
    }
}
