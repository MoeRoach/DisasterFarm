using System.Collections;
using System.Collections.Generic;
using RoachLite.Basic;
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
    private HashSet<int> enemyPawns;

    protected override void OnAwake() {
        base.OnAwake();
        pawns = new Dictionary<int, BasePawnController>();
        playerPawns = new HashSet<int>();
        enemyPawns = new HashSet<int>();
    }

    public void GeneratePlayerPawn() {
        
    }

    public void GenerateEnemyPawn() {
        
    }
}
