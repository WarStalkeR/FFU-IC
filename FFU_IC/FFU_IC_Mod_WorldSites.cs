using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.World.Entities;
using System.Collections.Generic;
using System.Reflection;

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

        // Reflection Helpers
        /// <remarks>
        /// Modifies various parameters of a <b>WorldMapMineProto</b>, except the resource that mine produces. 
        /// 
        /// <br/><br/>The <b>double[]</b> array requires these parameters:<br/>
        /// <b>ProducedProductPerStep</b> - how much resource produced per level. If you will choose 8 for example, it will be amount * 8.<br/>
        /// <b>ProductionDuration</b> - how often resource is produced. Defined in seconds. E.g. every 10, 20 or 40 seconds.<br/>
        /// <b>MonthlyUpointsPerLevel</b> - how much unity consumed per active level each month. E.g. 0.5 * 8 every month is 4.<br/>
        /// <b>MaxLevel</b> - what is the max level of the mine it can be upgraded to. E.g. can be upgraded to level 16 or 20.<br/>
        /// <b>LevelsPerUpgrade</b> - how much levels added to mine with each upgrade. E.g. every upgrade gives 2 or 3 more levels.<br/>
        /// 
        /// <br/><b>Note:</b> make sure that <b>MaxLevel</b> modulus of <b>LevelsPerUpgrade</b> is always zero without exception!<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference the <b>ProtoRegistrator</b> to access prototypes database:<br/>
        /// <c>pReg = registrator;</c>
        /// 
        /// <br/><br/>Define new world map mine parameters as <b>double[]</b> array:<br/>
        /// <c>double[] dataWaterWell = new double[] { 15, 10.0, 0.05, 20, 2 };</c>
        /// 
        /// <br/><br/>Apply new world map mine parameters via <b>WorldMapMineProto</b> identifier:<br/>
        /// <c>SetMineProduction(Ids.World.WaterWell, dataWaterWell);</c>
        /// </remarks>
        public void SetMineProduction(Mafi.Core.Entities.EntityProto.ID refMineID, double[] mineData) {
            if (pReg == null) { ModLog.Warning($"SetMineProduction: the ProtoRegistrator is not referenced!"); return; };
            WorldMapMineProto refMine = FFU_IC_IDs.WorldMineRef(pReg, refMineID);
            if (refMine == null) { ModLog.Warning($"SetMineProduction: can't find WorldMapMineProto reference!"); return; }
            if (mineData == null) { ModLog.Warning($"SetMineProduction: 'mineData' is undefined!"); return; }
            if (mineData.Length != 5) { ModLog.Warning($"SetMineProduction: 'mineData' count is incorrect!"); return; }
            if (mineData[3] % mineData[4] != 0) { ModLog.Warning($"SetMineProduction: 'MaxLevel' modulus of 'LevelsPerUpgrade' should be zero!"); return; }
            ModLog.Info($"{refMine.Id}. " +
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
            fieldMineOutput.SetValue(refMine, new ProductQuantity(refMine.ProducedProductPerStep.Product, ((int)mineData[0]).Quantity()));
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

            // World Mine Modifications
            SetMineProduction(Ids.World.OilRigCost1, MineProdData["Oil"]);
            SetMineProduction(Ids.World.OilRigCost2, MineProdData["Oil"]);
            SetMineProduction(Ids.World.OilRigCost3, MineProdData["Oil"]);
            SetMineProduction(Ids.World.WaterWell, MineProdData["Water"]);
            SetMineProduction(Ids.World.RockMine, MineProdData["Rock"]);
            SetMineProduction(Ids.World.CoalMine, MineProdData["Coal"]);
            SetMineProduction(Ids.World.SulfurMine, MineProdData["Sulfur"]);
            SetMineProduction(Ids.World.LimestoneMine, MineProdData["Limestone"]);
            SetMineProduction(Ids.World.QuartzMine, MineProdData["Quartz"]);
            SetMineProduction(Ids.World.UraniumMine, MineProdData["Uranium"]);
        }
    }
}