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
    private bool isLastOperateDone;

    private List<Square> enemySpawnPoints;
    // --- Test ---
    private List<Square> emptyGrounds;

    protected override void OnAwake() {
        base.OnAwake();
        isLastOperateDone = true;
        emptyGrounds = new List<Square>();
        operateQueue = new Queue<string>();
        enemySpawnPoints = new List<Square>();
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
        if (!isLastOperateDone || operateQueue.Count <= 0) return;
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

    private async void DoGoFarm(string pn) {
        var entryPoint = new Square(-8, 2);
        var pid = FarmPawnManager.Instance.GeneratePlayerPawn(pn, entryPoint);
        if (pid == int.MinValue) return;
        await UniTask.Yield();
        var pp = FarmPawnManager.Instance.GetPlayerPawn(pid);
        var targetList = new List<Square>();
        var tcount = NumberUtils.RandomInteger(3, 1);
        for (var i = 0; i < tcount; i++) {
            var dice = NumberUtils.RandomInteger(99);
            if (dice > 50) {
                if (emptyGrounds.Count > 0) {
                    var index = NumberUtils.RandomInteger(emptyGrounds.Count - 1);
                    targetList.Add(emptyGrounds[index]);
                } else {
                    var pc = FarmObjectManager.Instance.PickRandomPlantCoord();
                    if (pc == null) break;
                    targetList.Add(pc);
                }
            } else {
                var pc = FarmObjectManager.Instance.PickRandomPlantCoord();
                if (pc == null) {
                    if (emptyGrounds.Count <= 0) break;
                    var index = NumberUtils.RandomInteger(emptyGrounds.Count - 1);
                    targetList.Add(emptyGrounds[index]);
                } else targetList.Add(pc);
            }
        }

        PawnCommand cmd;
        foreach (var tc in targetList) {
            cmd = new PawnCommand(PawnCommand.CMD_STR_MOVE);
            cmd.PutVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION, MapUtils.SquareToWorld(tc));
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
            pp.SendCommand(cmd);
        
            cmd = new PawnCommand(PawnCommand.CMD_STR_PLANT);
            var ps = NumberUtils.RandomInteger(3);
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_PLANT_SERIAL, ps);
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_TIME_DURATION, 2f);
            pp.SendCommand(cmd);
        }
        
        cmd = new PawnCommand(PawnCommand.CMD_STR_MOVE);
        cmd.PutVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION, MapUtils.SquareToWorld(entryPoint));
        cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
        pp.SendCommand(cmd);
        
        cmd = new PawnCommand(PawnCommand.CMD_STR_DONE);
        pp.SendCommand(cmd);
    }

    private async void DoGoAttack(string pn) {
        var entryPoint = new Square(-8, 2);
        var pid = FarmPawnManager.Instance.GeneratePlayerPawn(pn, entryPoint);
        if (pid == int.MinValue) return;
        await UniTask.Yield();
        var pp = FarmPawnManager.Instance.GetPlayerPawn(pid);
        var epid = FarmPawnManager.Instance.PickRandomEnemyPawn();
        PawnCommand cmd;
        if (epid != int.MinValue) {
            cmd = new PawnCommand(PawnCommand.CMD_STR_FOLLOW);
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_PAWN_IDENTIFIER, epid);
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
            pp.SendCommand(cmd);
        
            cmd = new PawnCommand(PawnCommand.CMD_STR_ATTACK);
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_PAWN_IDENTIFIER, epid);
            pp.SendCommand(cmd);
        
            cmd = new PawnCommand(PawnCommand.CMD_STR_MOVE);
            cmd.PutVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION, MapUtils.SquareToWorld(entryPoint));
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
            pp.SendCommand(cmd);
        }
        
        cmd = new PawnCommand(PawnCommand.CMD_STR_DONE);
        pp.SendCommand(cmd);
    }

    private async void DoThiefCome(string pn) {
        var epi = NumberUtils.RandomInteger(enemySpawnPoints.Count - 1);
        var entryPoint = enemySpawnPoints[epi];
        var pid = FarmPawnManager.Instance.GenerateEnemyPawn(pn, entryPoint);
        if (pid == int.MinValue) return;
        await UniTask.Yield();
        var ep = FarmPawnManager.Instance.GetEnemyPawn(pid);
        var targetList = new List<Square>();
        var tcount = NumberUtils.RandomInteger(3, 1);
        for (var i = 0; i < tcount; i++) {
            var pc = FarmObjectManager.Instance.PickRandomPlantCoord();
            if (pc == null) break;
            targetList.Add(pc); 
        }

        PawnCommand cmd;
        foreach (var tc in targetList) {
            cmd = new PawnCommand(PawnCommand.CMD_STR_MOVE);
            cmd.PutVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION, MapUtils.SquareToWorld(tc));
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
            ep.SendCommand(cmd);

            var plant = FarmObjectManager.Instance.GetPlantAtCoord(tc);
            cmd = new PawnCommand(PawnCommand.CMD_STR_HARVEST);
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_PLANT_IDENTIFIER, plant);
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_TIME_DURATION, 2f);
            ep.SendCommand(cmd);
        }
        
        cmd = new PawnCommand(PawnCommand.CMD_STR_MOVE);
        cmd.PutVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION, MapUtils.SquareToWorld(entryPoint));
        cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
        ep.SendCommand(cmd);
        
        cmd = new PawnCommand(PawnCommand.CMD_STR_DONE);
        ep.SendCommand(cmd);
    }

    private async void DoDamagerCome(string pn) {
        var epi = NumberUtils.RandomInteger(enemySpawnPoints.Count - 1);
        var entryPoint = enemySpawnPoints[epi];
        var pid = FarmPawnManager.Instance.GenerateEnemyPawn(pn, entryPoint);
        if (pid == int.MinValue) return;
        await UniTask.Yield();
        var ep = FarmPawnManager.Instance.GetEnemyPawn(pid);
        var ppid = FarmPawnManager.Instance.PickRandomPlayerPawn();
        PawnCommand cmd;
        if (ppid != int.MinValue) {
            cmd = new PawnCommand(PawnCommand.CMD_STR_FOLLOW);
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_PAWN_IDENTIFIER, ppid);
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
            ep.SendCommand(cmd);
        
            cmd = new PawnCommand(PawnCommand.CMD_STR_ATTACK);
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_PAWN_IDENTIFIER, ppid);
            ep.SendCommand(cmd);
        
            cmd = new PawnCommand(PawnCommand.CMD_STR_MOVE);
            cmd.PutVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION, MapUtils.SquareToWorld(entryPoint));
            cmd.PutInteger(PawnCommand.CMD_ARG_KEY_RUN_AWAY, 1);
            cmd.PutFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED, 2f);
            ep.SendCommand(cmd);
        }
        
        cmd = new PawnCommand(PawnCommand.CMD_STR_DONE);
        ep.SendCommand(cmd);
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

        for (var x = -11; x <= 11; x++) {
            var top = new Square(x, 7);
            var bot = new Square(x, -7);
            enemySpawnPoints.Add(top);
            enemySpawnPoints.Add(bot);
        }

        for (var y = -6; y <= 6; y++) {
            var left = new Square(-11, y);
            var right = new Square(11, y);
            enemySpawnPoints.Add(left);
            enemySpawnPoints.Add(right);
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
        // var pawns = new List<string>(PawnConfigs.PawnNames);
        // for (var i = 0; i < 5; i++) {
        //     if (emptyGrounds.Count <= 0) break;
        //     var index = NumberUtils.RandomInteger(emptyGrounds.Count - 1);
        //     var sq = emptyGrounds[index];
        //     emptyGrounds.RemoveAt(index);
        //     if (FarmObjectManager.Instance.CheckCoordOccupiedByObject(sq)) continue;
        //     var pi = NumberUtils.RandomInteger(pawns.Count - 1);
        //     var pn = pawns[pi];
        //     FarmPawnManager.Instance.GeneratePlayerPawn(pn, sq);
        //     pawns.RemoveAt(pi);
        // }
    }

    private void TestPlants() {
        // for (var i = 0; i < 10; i++) {
        //     if (emptyGrounds.Count <= 0) break;
        //     var index = NumberUtils.RandomInteger(emptyGrounds.Count - 1);
        //     var sq = emptyGrounds[index];
        //     emptyGrounds.RemoveAt(index);
        //     if (FarmObjectManager.Instance.CheckCoordOccupiedByObject(sq)) continue;
        //     var ps = NumberUtils.RandomInteger(3);
        //     var ti = PlantConfigs.PlantFields[ps];
        //     ApplyFieldTile(sq.x, sq.y, ti);
        //     FarmObjectManager.Instance.GenerateFarmPlant(ps, sq);
        // }
    }

    public void PlantFarmPlant(int ps, int x, int y) {
        var sq = new Square(x, y);
        if (FarmObjectManager.Instance.CheckCoordOccupiedByObject(sq)) return;
        var ti = PlantConfigs.PlantFields[ps];
        ApplyFieldTile(sq.x, sq.y, ti);
        FarmObjectManager.Instance.GenerateFarmPlant(ps, sq);
    }

    public void HarvestFarmPlant(int x, int y) {
        ApplyFieldTile(x, y, int.MinValue);
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
        var tile = s == int.MinValue ? null : MapUtils.GetFieldTile(s);
        fieldsTilemap.SetTile(coord, tile);
    }

    public void DispatchOperation(string op) {
        operateQueue.Enqueue(op);
    }
}
