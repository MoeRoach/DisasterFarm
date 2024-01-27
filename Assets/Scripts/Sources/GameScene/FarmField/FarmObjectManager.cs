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

    private Dictionary<int, BaseFarmObject> objects;
    private Dictionary<int, Square> objectCoords;
    private Dictionary<string, int> objectMap;

    protected override void OnAwake() {
        base.OnAwake();
        plants = new Dictionary<int, BasePlantController>();
        plantCoords = new Dictionary<int, Square>();
        plantMap = new Dictionary<string, int>();
        
        objects = new Dictionary<int, BaseFarmObject>();
        objectCoords = new Dictionary<int, Square>();
        objectMap = new Dictionary<string, int>();
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

    public void UnregisterPlant(BasePlantController pc) {
        plantCoords.Remove(pc.Id);
        plantMap.Remove(pc.Coord.Sid);
    }

    public void GenerateObject(string on, Square coord) {
        var pos = MapUtils.SquareToWorld(coord);
        
    }

    public void RegisterObject(BaseFarmObject ob) {
        for (var x = 0; x < ob.Area.x; x++) {
            for (var y = 0; y < ob.Area.y; y++) {
                var sq = new Square(ob.Coord.x + x, ob.Coord.y + y);
                objectMap[sq.Sid] = ob.Id;
            }
        }

        objectCoords[ob.Id] = ob.Coord.Clone();
    }

    public void UnregisterObject(BaseFarmObject ob) {
        objectCoords.Remove(ob.Id);
        for (var x = 0; x < ob.Area.x; x++) {
            for (var y = 0; y < ob.Area.y; y++) {
                var sq = new Square(ob.Coord.x + x, ob.Coord.y + y);
                objectMap.Remove(sq.Sid);
            }
        }
    }

    public bool CheckCoordOccupiedByPlant(Square sq) {
        return plantMap.ContainsKey(sq.Sid);
    }

    public int GetPlantAtCoord(Square sq) {
        return plantMap.ContainsKey(sq.Sid) ? plantMap[sq.Sid] : int.MinValue;
    }

    public bool CheckCoordOccupiedByObject(Square sq) {
        return objectMap.ContainsKey(sq.Sid);
    }

    public int GetObjectAtCoord(Square sq) {
        return objectMap.ContainsKey(sq.Sid) ? objectMap[sq.Sid] : int.MinValue;
    }
}
