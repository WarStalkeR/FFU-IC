using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.World.Entities;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldSites : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private readonly Dictionary<string, double[]> MineProdData =
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

        // Reference Helpers
        private WorldMapMineProto WmRef(Mafi.Core.Entities.EntityProto.ID refID) => FFU_IC_IDs.WorldMineRef(pReg, refID);
        private ProductProto ProdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);

        // Reflection Helpers
        /// <remarks>
        /// Modifies various World Map Mines (such as Coal/Uranium) parameters, except the resource that mine produces. 
        /// The <c>double</c> array requires these parameters: <b>Produced Per Step</b>, <b>Production Duration</b>,
        /// <b>Monthly Unity Per Level</b>, <b>Max Mine Level</b> and <b>Levels Per Upgrade</b>.<br/><br/>
        /// 
        /// <b>Produced Per Step</b> - how much resource produced per level. If you will choose 8 for example, it will be amount * 8.<br/>
        /// <b>Production Duration</b> - how often resource is produced. Defined in seconds. E.g. every 10, 20 or 40 seconds.<br/>
        /// <b>Monthly Unity Per Level</b> - how much unity consumed per active level each month. E.g. 0.5 * 8 every month is 4.<br/>
        /// <b>Max Mine Level</b> - what is the max level of the mine it can be upgraded to. E.g. can be upgraded to level 16 or 20.<br/>
        /// <b>Levels Per Upgrade</b> - how much levels added to mine with each upgrade. E.g. every upgrade gives 2 or 3 more levels.<br/>
        /// <b>Note:</b> make sure that <b>Max Mine Level</b> modulus of <b>Levels Per Upgrade</b> is always zero without exception!<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference world map mine for modification:<br/>
        /// <c>WorldMapMineProto mineWaterWell = registrator.PrototypesDb.Get&lt;WorldMapMineProto&gt;(Ids.World.WaterWell).Value;</c>
        /// 
        /// <br/><br/>Define new world map mine parameters as 'double' array:<br/>
        /// <c>double[] dataWaterWell = new double[] { 15, 10.0, 0.05, 20, 2 };</c>
        /// 
        /// <br/><br/>Apply new world map mine parameters to the referenced mine:<br/>
        /// <c>SetMineProduction(mineWaterWell, dataWaterWell);</c>
        /// </remarks>
        public void SetMineProduction(WorldMapMineProto refMine, double[] mineData) {
            if (refMine == null) { ModLog.Warning($"SetMineProduction: 'refMine' is undefined!"); return; }
            if (mineData == null) { ModLog.Warning($"SetMineProduction: 'mineData' is undefined!"); return; }
            if (mineData.Length != 5) { ModLog.Warning($"SetMineProduction: 'mineData' count is incorrect!"); return; }
            if (mineData[3] % mineData[4] != 0) { ModLog.Warning($"SetMineProduction: 'MaxLevel' modulus of 'LevelsPerUpgrade' should be zero!"); return; }
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

            // World Site References
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