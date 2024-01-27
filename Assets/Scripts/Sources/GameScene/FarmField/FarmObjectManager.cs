using System.Collections;
using System.Collections.Generic;
using RoachLite.Basic;
using RoachLite.Data;
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
    private Dictionary<int, Square> plantCoords;
    private Dictionary<string, int> plantMap;

    protected override void OnAwake() {
        base.OnAwake();
        plants = new Dictionary<int, BasePlantController>();
        plantCoords = new Dictionary<int, Square>();
        plantMap = new Dictionary<string, int>();
    }

    public void GenerateFarmPlant(int serial) {
        
    }

    public void RegisterPlant(BasePlantController pc) {
        plantMap[pc.Coord.Sid] = pc.Id;
        plantCoords[pc.Id] = pc.Coord.Clone();
    }

    public void UpdatePlantMap(BasePlantController pc) {
        if (!plantCoords.ContainsKey(pc.Id)) return;
        var prevCoord = plantCoords[pc.Id];
        if (plantMap.ContainsKey(prevCoord.Sid)) {
            plantMap.Remove(prevCoord.Sid);
        }

        plantMap[pc.Coord.Sid] = pc.Id;
        plantCoords[pc.Id] = pc.Coord.Clone();
    }
}
