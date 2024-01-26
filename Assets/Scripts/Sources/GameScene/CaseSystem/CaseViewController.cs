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
    private int selectIndex;
    [SerializeField] private Transform viewTrans;

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
        selectIndex = -1;
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
        Debug.Log(CurrentObj.GetComponent<CaseController>());
        Debug.Log(data);
        Debug.Log(data.image);
        image.sprite = Resources.Load(data.image, typeof(Sprite)) as Sprite;
        Debug.Log(Resources.Load(data.image, typeof(Sprite)));
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
        string pass = "", fail = "";
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[0].Pass)
        {
            pass += str + " ";
        }
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[0].Fail)
        {
            fail += str;
        }
        Debug.Log($"pass:[{pass}],fail:[{fail}]");
    }

    public void ClickBtn_1()
    {
        Debug.Log($"ClickBtn_1");
        string pass = "", fail = "";
        // Debug.Log(CurrentObj.GetComponent<CaseController>().Data.chooseList[1]);
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[1].Pass)
        {
            pass += str + " ";
        }
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[1].Fail)
        {
            fail += str;
        }
        Debug.Log($"pass:[{pass}],fail:[{fail}]");
    }

    public void ClickBtn_2()
    {
        Debug.Log($"ClickBtn_2");
        string pass = "", fail = "";
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[2].Pass)
        {
            pass += str + " ";
        }
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[2].Fail)
        {
            fail += str;
        }
        Debug.Log($"pass:[{pass}],fail:[{fail}]");
    }

    public void ClickBtn_3()
    {
        Debug.Log($"ClickBtn_3");
        string pass = "", fail = "";
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[3].Pass)
        {
            pass += str + " ";
        }
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[3].Fail)
        {
            fail += str;
        }
        Debug.Log($"pass:[{pass}],fail:[{fail}]");
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
