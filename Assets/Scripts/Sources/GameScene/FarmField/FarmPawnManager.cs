using System.Collections;
using System.Collections.Generic;
using RoachLite.Basic;
using RoachLite.Data;
using RoachLite.Utils;
using UnityEngine;

public class FarmPawnManager : MonoSingleton<FarmPawnManager> {

    private int farmPawnStartIndex;

    private int PawnIdentifier {
        get {
            farmPawnStartIndex++;
            return farmPawnStartIndex - 1;
        }
    }

    private Dictionary<int, BasePawnController> pawns;
    
    private HashSet<int> playerPawns;
    private Dictionary<string, int> playerPawnNameToId;
    
    private HashSet<int> enemyPawns;
    private Dictionary<string, int> enemyPawnNameToId;


    protected override void OnAwake() {
        base.OnAwake();
        pawns = new Dictionary<int, BasePawnController>();
        playerPawnNameToId = new Dictionary<string, int>();
        playerPawns = new HashSet<int>();
        enemyPawnNameToId = new Dictionary<string, int>();
        enemyPawns = new HashSet<int>();
    }

    public int GeneratePlayerPawn(string pn, Square coord) {
        if (playerPawnNameToId.ContainsKey(pn)) return playerPawnNameToId[pn];
        var po = PrefabUtils.CreatePawn(pn, 0, transform);
        po.transform.position = MapUtils.SquareToWorld(coord);
        var ctrl = po.GetComponent<BasePawnController>();
        ctrl.SetupIdentifier(PawnIdentifier, pn);
        ctrl.RegisterDestroyCallback(OnPlayerPawnDead);
        pawns[ctrl.Id] = ctrl;
        playerPawns.Add(ctrl.Id);
        playerPawnNameToId[pn] = ctrl.Id;
        return ctrl.Id;
    }

    public int PickRandomPlayerPawn() {
        var offset = NumberUtils.RandomInteger(playerPawns.Count - 1);
        var counter = 0;
        foreach (var id in playerPawns) {
            if (counter == offset) return id;
            counter++;
        }

        return int.MinValue;
    }

    public BasePawnController GetPlayerPawn(int id) {
        return pawns.TryGetElement(id);
    }

    public int GenerateEnemyPawn(string pn, Square coord) {
        if (enemyPawnNameToId.ContainsKey(pn)) return enemyPawnNameToId[pn];
        var po = PrefabUtils.CreatePawn(pn, 0, transform);
        po.transform.position = MapUtils.SquareToWorld(coord);
        var ctrl = po.GetComponent<BasePawnController>();
        ctrl.SetupIdentifier(PawnIdentifier, pn);
        ctrl.RegisterDestroyCallback(OnEnemyPawnDead);
        pawns[ctrl.Id] = ctrl;
        enemyPawns.Add(ctrl.Id);
        enemyPawnNameToId[pn] = ctrl.Id;
        return ctrl.Id;
    }

    public int PickRandomEnemyPawn() {
        var offset = NumberUtils.RandomInteger(enemyPawns.Count - 1);
        var counter = 0;
        foreach (var id in enemyPawns) {
            if (counter == offset) return id;
            counter++;
        }

        return int.MinValue;
    }

    public BasePawnController GetEnemyPawn(int id) {
        return pawns.TryGetElement(id);
    }

    public BasePawnController GetPawn(int id) {
        return pawns.TryGetElement(id);
    }

    private void OnPlayerPawnDead(GameObject po) {
        var ctrl = po.GetComponent<BasePawnController>();
        if (ctrl == null) return;
        pawns.Remove(ctrl.Id);
        playerPawns.Remove(ctrl.Id);
        playerPawnNameToId.Remove(ctrl.Name);
    }

    private void OnEnemyPawnDead(GameObject po) {
        var ctrl = po.GetComponent<BasePawnController>();
        if (ctrl == null) return;
        pawns.Remove(ctrl.Id);
        enemyPawns.Remove(ctrl.Id);
        enemyPawnNameToId.Remove(ctrl.Name);
    }
}
