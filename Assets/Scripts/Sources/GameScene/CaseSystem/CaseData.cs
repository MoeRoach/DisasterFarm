using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CaseData
{
    public int caseId;
    public string name;
    public string color;
    public string signal;
    public int timeout;
    public string caseContent;
    public string image;
    public string imageTitle;
    public ChooseData[] chooseList;
}

[System.Serializable]
public class ChooseData
{
    public int index;
    public string text;
    public string[] checkList;
    public string[] Pass;
    public string[] Fail;
}