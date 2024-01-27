using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RoachLite.Basic;
using RoachLite.Data;
using RoachLite.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmFieldRootControl : MonoSingleton<FarmFieldRootControl> {

    private Tilemap terrainTilemap;
    private Tilemap groundTilemap;
    private Tilemap fieldsTilemap;
    private Tilemap structureTilemap;

    private FarmDataService dataService;
    private FarmObjectManager objectManager;
    private FarmPawnManager pawnManager;

    private Queue<string> operateQueue;

    protected override void OnAwake() {
        base.OnAwake();
        operateQueue = new Queue<string>();
        terrainTilemap = FindComponent<Tilemap>("MapRoot.Terrain");
        groundTilemap = FindComponent<Tilemap>("MapRoot.Ground");
        fieldsTilemap = FindComponent<Tilemap>("MapRoot.Fields");
        structureTilemap = FindComponent<Tilemap>("MapRoot.Structure");
    }

    protected override void OnStart() {
        base.OnStart();
        dataService = FarmDataService.Instance;
        objectManager = FarmObjectManager.Instance;
        pawnManager = FarmPawnManager.Instance;
        RegisterUpdateFunction(1, UpdateOperateQueue);
        InitFarm();
    }

    private void UpdateOperateQueue() {
        if (operateQueue.Count <= 0) return;
        var op = operateQueue.Dequeue();
        ProcessOperation(op);
    }

    private void ProcessOperation(string op) {
        // 先拆解指令，取得指令本体和参数，随后执行
    }

    private async void InitFarm() {
        // 先生成地图数据
        await GenerateFarmTileData();
        await UniTask.Yield();
        // 按照数据将Tilemap铺好
        await GenerateFarmMap();
        await UniTask.Yield();
        // 生成初始单位
        GenerateInitPawn();
        await UniTask.Yield();
        // 通知完成初始化
    }

    private async UniTask GenerateFarmTileData() {
        var counter = 0;
        await UniTask.Yield();
        for (var x = -25; x < 25; x++) {
            for (var y = -25; y < 25; y++) {
                ApplyGrassTile(x, y);
                counter++;
                if (counter < 500) continue;
                counter = 0;
                await UniTask.Yield();
            }
        }

        counter = 0;
        await UniTask.Yield();
        for (var x = -10; x <= 10; x++) {
            for (var y = -6; y <= 6; y++) {
                var tile = new FarmTile(x, y) {ground = -1, field = -1, structure = -1};
                if (x == -10) {
                    if (y == -6) {
                        tile.structure = MapUtils.FENCE_INDEX_LEFT_BOT;
                    } else if (y == 6) {
                        tile.structure = MapUtils.FENCE_INDEX_LEFT_TOP;
                    } else {
                        tile.structure = MapUtils.FENCE_INDEX_LEFT;
                    }
                } else if (x == 10) {
                    if (y == -6) {
                        tile.structure = MapUtils.FENCE_INDEX_RIGHT_BOT;
                    } else if (y == 6) {
                        tile.structure = MapUtils.FENCE_INDEX_RIGHT_TOP;
                    } else {
                        tile.structure = MapUtils.FENCE_INDEX_RIGHT;
                    }
                } else {
                    if (y == -6) {
                        tile.structure = MapUtils.FENCE_INDEX_BOT;
                    } else if (y == 6) {
                        tile.structure = MapUtils.FENCE_INDEX_TOP;
                    } else {
                        tile.structure = -1;
                        if (x == -9) {
                            if (y == -5) {
                                tile.ground = MapUtils.GROUND_INDEX_LEFT_BOT;
                            } else if (y == 5) {
                                tile.ground = MapUtils.GROUND_INDEX_LEFT_TOP;
                            } else {
                                tile.ground = MapUtils.GROUND_INDEX_LEFT;
                            }
                        } else if (x == 9) {
                            if (y == -5) {
                                tile.ground = MapUtils.GROUND_INDEX_RIGHT_BOT;
                            } else if (y == 5) {
                                tile.ground = MapUtils.GROUND_INDEX_RIGHT_TOP;
                            } else {
                                tile.ground = MapUtils.GROUND_INDEX_RIGHT;
                            }
                        } else {
                            tile.ground = MapUtils.GROUND_INDEX_CENTER;
                        }
                    }
                }
                
                dataService.farmData.tiles[tile.Id] = tile;
                counter++;
                if (counter < 500) continue;
                counter = 0;
                await UniTask.Yield();
            }
        }
    }

    private async UniTask GenerateFarmMap() {
        foreach (var tid in dataService.farmData.tiles.Keys) {
            var tile = dataService.farmData.tiles[tid];
            ApplyFarmTile(tile);
        }

        await UniTask.Yield();
        // 生成物体
    }
    
    private void GenerateInitPawn() {
        
    }

    private void ApplyGrassTile(int x, int y) {
        var coord = MapUtils.SquareToCoordinate(x, y);
        var tile = MapUtils.GetGrassTile();
        groundTilemap.SetTile(coord, tile);
    }

    private void ApplyFarmTile(FarmTile tile) {
        var coord = MapUtils.SquareToCoordinate(tile.coord);
        if (tile.structure >= 0) {
            var ts = MapUtils.GetFenceTile(tile.structure);
            structureTilemap.SetTile(coord, ts);
        }

        if (tile.ground >= 0) {
            var gs = MapUtils.GetGroundTile(tile.ground);
            
        }

        if (tile.field == 0) return;
        var fs = MapUtils.GetGroundTile(tile.field);
        fieldsTilemap.SetTile(coord, fs);
    }

    public void DispatchOperation(string op) {
        operateQueue.Enqueue(op);
    }
}
