using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseManager : MonoBehaviour
{
    [SerializeField] private CaseViewController view;
    // Start is called before the first frame update
    void Start()
    {
        view.AddCaseController(2001);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
