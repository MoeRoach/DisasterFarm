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
    
    // --- Test ---
    private List<Square> emptyGrounds;

    protected override void OnAwake() {
        base.OnAwake();
        emptyGrounds = new List<Square>();
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
        var split = op.Split('_');
        if (split.Length != 2) return;
        var cmd = split[0];
        var arg = split[1];
        switch (cmd) {
            case PawnCommand.OP_CMD_GOFARM: // 种田指令，随机选择一块田地，移动过去，开始工作即可
                DoGoFarm(arg);
                break;
            case PawnCommand.OP_CMD_GOATTACK: // 攻击指令，随机选择一个敌人，移动过去，开始攻击即可
                DoGoAttack(arg);
                break;
            case PawnCommand.OP_CMD_THIEFCOME: // 偷窃和破坏指令，随机选择植物或者房屋，移动过去，开始破坏即可
                DoThiefCome(arg);
                break;
            case PawnCommand.OP_CMD_DAMAGERCOME: // 破坏指令，随机选择农民或者植物，移动过去，开始破坏即可
                DoDamagerCome(arg);
                break;
        }
    }

    private void DoGoFarm(string pn) {
        
    }

    private void DoGoAttack(string pn) {
        
    }

    private void DoThiefCome(string pn) {
        
    }

    private void DoDamagerCome(string pn) {
        
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
        TestPlants();
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
                    }
                }
                
                dataService.farmData.tiles[tile.Id] = tile;
                counter++;
                if (counter < 500) continue;
                counter = 0;
                await UniTask.Yield();
            }
        }
        
        await UniTask.Yield();
        var pivot = new Square(-5, 1);
        var size = new Square(7, 5);
        GenerateFarmBlock(pivot, size);
        pivot.x += 8;
        GenerateFarmBlock(pivot, size);
        pivot.y -= 6;
        GenerateFarmBlock(pivot, size);
        pivot.x -= 8;
        GenerateFarmBlock(pivot, size);
    }

    private void GenerateFarmBlock(Square pivot, Square size) {
        for (var x = 0; x < size.x; x++) {
            for (var y = 0; y < size.y; y++) {
                var sq = new Square(pivot.x + x, pivot.y + y);
                var tile = dataService.farmData.tiles[sq.Sid];
                if (x == 0) {
                    if (y == 0) {
                        tile.ground = MapUtils.GROUND_INDEX_LEFT_BOT;
                    } else if (y == size.y - 1) {
                        tile.ground = MapUtils.GROUND_INDEX_LEFT_TOP;
                    } else {
                        tile.ground = MapUtils.GROUND_INDEX_LEFT;
                    }
                } else if (x == size.x - 1) {
                    if (y == 0) {
                        tile.ground = MapUtils.GROUND_INDEX_RIGHT_BOT;
                    } else if (y == size.y - 1) {
                        tile.ground = MapUtils.GROUND_INDEX_RIGHT_TOP;
                    } else {
                        tile.ground = MapUtils.GROUND_INDEX_RIGHT;
                    }
                } else {
                    if (y == 0) {
                        tile.ground = MapUtils.GROUND_INDEX_BOT;
                    } else if (y == size.y - 1) {
                        tile.ground = MapUtils.GROUND_INDEX_TOP;
                    } else {
                        tile.ground = MapUtils.GROUND_INDEX_CENTER;
                    }
                }

                var dice = NumberUtils.RandomInteger(99);
                if (dice > 25) continue;
                emptyGrounds.Add(tile.coord.Clone());
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
        var hq = new Square(-9, 3);
        FarmObjectManager.Instance.GenerateObject(PrefabUtils.PREFAB_NAME_FARM_HOUSE, hq);
        var ppc = new Square(-5, 5);
        FarmObjectManager.Instance.GenerateObject(PrefabUtils.PREFAB_NAME_FARM_POOL, ppc);
        ppc.x += 8;
        FarmObjectManager.Instance.GenerateObject(PrefabUtils.PREFAB_NAME_FARM_POOL, ppc);
        ppc.y -= 6;
        FarmObjectManager.Instance.GenerateObject(PrefabUtils.PREFAB_NAME_FARM_POOL, ppc);
        ppc.x -= 8;
        FarmObjectManager.Instance.GenerateObject(PrefabUtils.PREFAB_NAME_FARM_POOL, ppc);
    }
    
    private void GenerateInitPawn() {
        var pawns = new List<string>(PawnConfigs.PawnNames);
        for (var i = 0; i < 5; i++) {
            if (emptyGrounds.Count <= 0) break;
            var index = NumberUtils.RandomInteger(emptyGrounds.Count - 1);
            var sq = emptyGrounds[index];
            emptyGrounds.RemoveAt(index);
            if (FarmObjectManager.Instance.CheckCoordOccupiedByObject(sq)) continue;
            var pi = NumberUtils.RandomInteger(pawns.Count - 1);
            var pn = pawns[pi];
            FarmPawnManager.Instance.GeneratePlayerPawn(pn, sq);
            pawns.RemoveAt(pi);
        }
    }

    private void TestPlants() {
        for (var i = 0; i < 10; i++) {
            if (emptyGrounds.Count <= 0) break;
            var index = NumberUtils.RandomInteger(emptyGrounds.Count - 1);
            var sq = emptyGrounds[index];
            emptyGrounds.RemoveAt(index);
            if (FarmObjectManager.Instance.CheckCoordOccupiedByObject(sq)) continue;
            var ps = NumberUtils.RandomInteger(3);
            var ti = PlantConfigs.PlantFields[ps];
            ApplyFieldTile(sq.x, sq.y, ti);
            FarmObjectManager.Instance.GenerateFarmPlant(ps, sq);
        }
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
            groundTilemap.SetTile(coord, gs);
        }
    }

    public void ApplyFieldTile(int x, int y, int s) {
        var coord = MapUtils.SquareToCoordinate(x, y);
        var tile = MapUtils.GetFieldTile(s);
        fieldsTilemap.SetTile(coord, tile);
    }

    public void DispatchOperation(string op) {
        operateQueue.Enqueue(op);
    }
}
