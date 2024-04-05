using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.World.Entities;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldSites : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions
        public readonly Dictionary<string, double[]> MineProdData =
            new Dictionary<string, double[]>() {
            { "Oil", new double[] { 25, 20.0, 0.05, 20, 2 }},
            { "Water", new double[] { 15, 10.0, 0.05, 20, 2 }},
            { "Rock", new double[] { 30, 20.0, 0.05, 20, 2 }},
            { "Coal", new double[] { 25, 20.0, 0.05, 20, 2 }},
            { "Sulfur", new double[] { 15, 20.0, 0.05, 20, 2 }},
            { "Limestone", new double[] { 15, 20.0, 0.05, 20, 2 }},
            { "Quartz", new double[] { 20, 20.0, 0.05, 20, 2 }},
            { "Uranium", new double[] { 10, 20.0, 0.05, 20, 2 }},
        };

        // Localization Definitions

        // Reflection Helpers
        public WorldMapMineProto WmRef(Mafi.Core.Entities.EntityProto.ID refID) => FFU_IC_IDs.WorldMineRef(pReg, refID);
        public ProductProto ProdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);
        public void SetMineProduction(WorldMapMineProto refMine, double[] mineData) {
            if (refMine == null) { ModLog.Warning($"SetMineProduction: 'refMine' is undefined!"); return; }
            if (mineData == null) { ModLog.Warning($"SetMineProduction: 'mineData' is undefined!"); return; }
            ModLog.Info($"{refMine.Id} " +
                $"Output: {refMine.ProducedProductPerStep.Quantity} -> {mineData[0]}, " +
                $"Cycle: {refMine.ProductionDuration.Seconds}s -> {mineData[1]}s, " +
                $"Upkeep: {refMine.MonthlyUpointsPerLevel.Value}u -> {mineData[2]}u, " +
                $"Levels: {refMine.MaxLevel} -> {mineData[3]}, " +
                $"Upgrade: {refMine.LevelsPerUpgrade} -> {mineData[4]}");
            FieldInfo fieldMineOutput = typeof(WorldMapMineProto).GetField("ProducedProductPerStep", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldMineCycle = typeof(WorldMapMineProto).GetField("ProductionDuration", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldMineUpkeep = typeof(WorldMapMineProto).GetField("MonthlyUpointsPerLevel", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldMineLevel = typeof(WorldMapMineProto).GetField("MaxLevel", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldMineUpgrade = typeof(WorldMapMineProto).GetField("LevelsPerUpgrade", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldMineInit = typeof(WorldMapMineProto).GetField("Level", BindingFlags.Instance | BindingFlags.Public);
            fieldMineOutput.SetValue(refMine, new ProductQuantity(ProdRef(refMine.ProducedProductPerStep.Product.Id), ((int)mineData[0]).Quantity()));
            fieldMineCycle.SetValue(refMine, mineData[1].Seconds());
            fieldMineUpkeep.SetValue(refMine, mineData[2].Upoints());
            fieldMineLevel.SetValue(refMine, (int)mineData[3]);
            fieldMineUpgrade.SetValue(refMine, (int)mineData[4]);
            fieldMineInit.SetValue(refMine, (int)mineData[4]);
            FFU_IC_IDs.SyncProtoMod(refMine);
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // World Mine References
            WorldMapMineProto oilRig1 = WmRef(Ids.World.OilRigCost1);
            WorldMapMineProto oilRig2 = WmRef(Ids.World.OilRigCost2);
            WorldMapMineProto oilRig3 = WmRef(Ids.World.OilRigCost3);
            WorldMapMineProto waterWell = WmRef(Ids.World.WaterWell);
            WorldMapMineProto mineRock = WmRef(Ids.World.RockMine);
            WorldMapMineProto mineCoal = WmRef(Ids.World.CoalMine);
            WorldMapMineProto mineSulfur = WmRef(Ids.World.SulfurMine);
            WorldMapMineProto mineLimestone = WmRef(Ids.World.LimestoneMine);
            WorldMapMineProto mineQuartz = WmRef(Ids.World.QuartzMine);
            WorldMapMineProto mineUranium = WmRef(Ids.World.UraniumMine);

            // World Mine Modifications
            SetMineProduction(oilRig1, MineProdData["Oil"]);
            SetMineProduction(oilRig2, MineProdData["Oil"]);
            SetMineProduction(oilRig3, MineProdData["Oil"]);
            SetMineProduction(waterWell, MineProdData["Water"]);
            SetMineProduction(mineRock, MineProdData["Rock"]);
            SetMineProduction(mineCoal, MineProdData["Coal"]);
            SetMineProduction(mineSulfur, MineProdData["Sulfur"]);
            SetMineProduction(mineLimestone, MineProdData["Limestone"]);
            SetMineProduction(mineQuartz, MineProdData["Quartz"]);
            SetMineProduction(mineUranium, MineProdData["Uranium"]);
        }
    }
}