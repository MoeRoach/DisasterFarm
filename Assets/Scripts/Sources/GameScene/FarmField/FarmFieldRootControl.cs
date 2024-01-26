using System.Collections;
using System.Collections.Generic;
using RoachLite.Basic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmFieldRootControl : MonoSingleton<FarmFieldRootControl> {

    private Tilemap groundTilemap;
    private Tilemap fieldsTilemap;
    private Tilemap plantsTilemap;

    private FarmObjectManager objectManager;
    private FarmPawnManager pawnManager;

    protected override void OnAwake() {
        base.OnAwake();
        groundTilemap = FindComponent<Tilemap>("MapRoot.Ground");
        fieldsTilemap = FindComponent<Tilemap>("MapRoot.Fields");
        plantsTilemap = FindComponent<Tilemap>("MapRoot.Plants");
    }
}
