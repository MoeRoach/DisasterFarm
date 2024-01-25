using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaseViewController : MonoBehaviour
{
    public CaseData data { get; set; }
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
        data = null;
        selectIndex = -1;
        content.text = "";
        imageTitle.text = "";
        buttonText_1.text = "";
        buttonText_2.text = "";
        buttonText_3.text = "";
    }

    public void UpdateView()
    {
        if (data == null)
        {
            Debug.Log("data == null");
            return;
        }

        image.sprite = Resources.Load(data.image, typeof(Sprite)) as Sprite;
        Debug.Log(data.image);
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
        //todo
    }

    public void ClickBtn_1()
    {
        //todo
    }

    public void ClickBtn_2()
    {
        GameObject obj = Resources.Load("CaseSystemPrefabs/Case", typeof(GameObject)) as GameObject;
        Debug.Log(obj);
        Instantiate(obj, viewTrans);
    }

    public void ClickBtn_3()
    {
        //todo
    }


}
