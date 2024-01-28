using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoachLite.Basic;
using RoachLite.Data;
using RoachLite.Utils;

public class CaseViewController : MonoSingleton<CaseViewController>
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
    public void AddToPool(int caseId)
    {
        caseManager.CasePool.Add(caseId);
    }

    public void AddToPool(string str)
    {
        switch (str)
        {
            case SpriteUtils.SPRITE_NAME_PIG:
                AddToPool(2002);
                break;
            case SpriteUtils.SPRITE_NAME_BULL:
                AddToPool(2005);
                break;
            case SpriteUtils.SPRITE_NAME_COW:
                AddToPool(2008);
                break;
            case SpriteUtils.SPRITE_NAME_CAPYBARA:
                AddToPool(2011);
                break;
            case SpriteUtils.SPRITE_NAME_GOAT:
                AddToPool(2014);
                break;
            case SpriteUtils.SPRITE_NAME_MOUSE:
                AddToPool(2017);
                break;
            default:
                Debug.Log($"AddToPool error");
                break;
        }
    }

    public void GoFarmDone(string str)
    {
        switch (str)
        {
            case SpriteUtils.SPRITE_NAME_PIG:
                AddToPool(2002);
                break;
            case SpriteUtils.SPRITE_NAME_BULL:
                AddToPool(2005);
                break;
            case SpriteUtils.SPRITE_NAME_COW:
                AddToPool(2008);
                break;
            case SpriteUtils.SPRITE_NAME_CAPYBARA:
                AddToPool(2011);
                break;
            case SpriteUtils.SPRITE_NAME_GOAT:
                AddToPool(2014);
                break;
            case SpriteUtils.SPRITE_NAME_MOUSE:
                AddToPool(2017);
                break;
            default:
                Debug.Log($"AddToPool error");
                break;
        }
    }

    public void GoThiefDone(string str)
    {
        switch (str)
        {
            case SpriteUtils.SPRITE_NAME_PIG:
                AddToPool(2001);
                break;
            case SpriteUtils.SPRITE_NAME_BULL:
                AddToPool(2004);
                break;
            case SpriteUtils.SPRITE_NAME_COW:
                AddToPool(2007);
                break;
            case SpriteUtils.SPRITE_NAME_CAPYBARA:
                AddToPool(2010);
                break;
            case SpriteUtils.SPRITE_NAME_GOAT:
                AddToPool(2013);
                break;
            case SpriteUtils.SPRITE_NAME_MOUSE:
                AddToPool(2016);
                break;
            default:
                Debug.Log($"AddToPool error");
                break;
        }
    }

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
        image.sprite = null;
    }

    public void UpdateView()
    {
        if (CurrentObj == null)
        {
            Debug.Log("CurrentObj == null");
            return;
        }

        Debug.Log(CurrentObj.GetComponent<CaseController>());
        CaseData data = CurrentObj.GetComponent<CaseController>().Data;
        // Debug.Log(data);
        Debug.Log(data.image);
        image.sprite = SpriteUtils.GetCaseImageSprite(data.image);
        Debug.Log(image.sprite);
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

    bool StringCompare(int value1, string str, int value2)
    {
        switch (str)
        {
            case ">":
                return value1 > value2;
            case ">=":
                return value1 >= value2;
            case "<":
                return value1 < value2;
            case "<=":
                return value1 <= value2;
            case "=":
                return value1 == value2;
            default:
                Debug.Log($"StringCompare error");
                break;
        }
        return true;
    }

    void ParseChoose(int index)
    {
        caseManager.CaseList.Remove(CurrentObj.GetComponent<CaseController>().Data.caseId);

        string pass = "", fail = "";
        bool isPass = true;
        int number;
        foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[index].checkList)
        {
            string[] result = str.Split("_");
            if (result.Length < 3)
            {
                Debug.Log("result.Length < 3");
                return;
            }
            else
            {
                Debug.Log("checkList is ok");
            }
            switch (result[0])
            {
                case "Money":
                    if (int.TryParse(result[2], out number))
                    {
                        isPass = StringCompare(PlayerInfo.Money, result[2], number);
                    }
                    break;
                case "HP":
                    if (int.TryParse(result[2], out number))
                    {
                        isPass = StringCompare(PlayerInfo.HP, result[2], number);
                    }
                    break;
                case "Strength":
                    if (int.TryParse(result[2], out number))
                    {
                        isPass = StringCompare(PlayerInfo.Strength, result[2], number);
                    }
                    break;
                case "Crop":
                    if (int.TryParse(result[2], out number))
                    {
                        isPass = StringCompare(PlayerInfo.Crop, result[2], number);
                    }
                    break;
                case "Fame":
                    if (int.TryParse(result[2], out number))
                    {
                        isPass = StringCompare(PlayerInfo.Fame, result[2], number);
                    }
                    break;
                case "Charisma":
                    if (int.TryParse(result[2], out number))
                    {
                        isPass = StringCompare(PlayerInfo.Charisma, result[2], number);
                    }
                    break;
                default:

                    break;
            }
            if (!isPass)
                break;
        }
        if (isPass)
        {
            foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[index].Pass)
            {
                pass += str + " ";
                ParseAction(str);
            }
        }
        else
        {
            foreach (var str in CurrentObj.GetComponent<CaseController>().Data.chooseList[index].Fail)
            {
                fail += str;
                ParseAction(str);
            }
        }


        Debug.Log($"pass:[{pass}],fail:[{fail}]");
    }

    public void UpdateFromCaseList()
    {
        Debug.Log($"UpdateFromCaseList");
        if (viewTrans.childCount <= 0)
        {
            Debug.Log($"viewTrans.childCount <= 0");
            return;
        }

        CurrentObj = viewTrans.GetChild(0).gameObject;
        Debug.Log($"CurrentObj caseId {CurrentObj.GetComponent<CaseController>().Data.caseId}");
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
        int number;
        switch (result[0])
        {
            // case issue
            case "ListAdd":
                Debug.Log($"ListAdd {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    caseManager.CaseList.Add(number);
                    AddCaseController(number);
                }
                break;
            case "PoolAdd":
                if (int.TryParse(result[1], out number))
                {
                    Debug.Log($"PoolAdd {result[1]}");
                    caseManager.CasePool.Add(number);
                }
                else
                {
                    Debug.Log($"PoolAdd fail");
                }
                break;
            // attributes issue
            case "MoneyUp":
                Debug.Log($"MoneyUp {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Money += number;
                }
                break;
            case "MoneyDown":
                Debug.Log($"MoneyDown {result[1]}");
                if (result[1] == "All")
                {
                    PlayerInfo.Money = 0;
                }
                else if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Money -= number;
                }
                break;
            case "HPUp":
                Debug.Log($"HPUp {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.HP += number;
                }
                break;
            case "HPDown":
                Debug.Log($"HPDown {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.HP -= number;
                }
                break;
            case "StrengthUp":
                Debug.Log($"StrengthUp {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Strength += number;
                }
                break;
            case "StrengthDown":
                Debug.Log($"StrengthDown {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Strength -= number;
                }
                break;
            case "CropUp":
                Debug.Log($"CropUp {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Crop += number;
                }
                break;
            case "CropDown":
                Debug.Log($"CropDown {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Crop -= number;
                }
                break;
            case "FameUp":
                Debug.Log($"FameUp {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Fame += number;
                }
                break;
            case "FameDown":
                Debug.Log($"FameDown {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Fame -= number;
                }
                break;
            case "CharismaUp":
                Debug.Log($"CharismaUp {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Charisma += number;
                }
                break;
            case "CharismaDown":
                Debug.Log($"CharismaDown {result[1]}");
                if (int.TryParse(result[1], out number))
                {
                    PlayerInfo.Charisma -= number;
                }
                break;
            // action issue
            case "GoFarm":
                Debug.Log($"GoFarm {result[1]}");
                FarmFieldRootControl.Instance.DispatchOperation(str);
                break;
            case "GoAttack":
                Debug.Log($"GoAttack {result[1]}");
                FarmFieldRootControl.Instance.DispatchOperation(str);
                break;
            case "ThiefCome":
                Debug.Log($"ThiefCome {result[1]}");
                FarmFieldRootControl.Instance.DispatchOperation(str);
                break;
            case "DamagerCome":
                Debug.Log($"DamagerCome {result[1]}");
                FarmFieldRootControl.Instance.DispatchOperation(str);
                break;
            default:
                Debug.Log("default");
                break;
        }

    }

    public GameObject AddCaseController(int caseId)
    {
        GameObject obj = Resources.Load("CaseSystemPrefabs/Case", typeof(GameObject)) as GameObject;
        Debug.Log(obj);
        obj = Instantiate(obj, viewTrans);
        obj.GetComponent<CaseController>().CaseDataInit(caseId);
        obj.GetComponent<CaseController>().View = this;

        return obj;
    }
}
