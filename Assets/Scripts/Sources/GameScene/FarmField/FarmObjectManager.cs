using System.Collections;
using System.Collections.Generic;
using RoachLite.Basic;
using UnityEngine;

public class FarmObjectManager : MonoSingleton<FarmObjectManager> {

    private int farmObjectStartIndex;

    private int ObjectIdentifier {
        get {
            farmObjectStartIndex++;
            return farmObjectStartIndex - 1;
        }
    }

    private Dictionary<int, BasePlantController> plants;

    protected override void OnAwake() {
        base.OnAwake();
        plants = new Dictionary<int, BasePlantController>();
    }

    public void GenerateFarmPlant() {
        
    }
}
