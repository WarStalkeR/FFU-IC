using Mafi.Core;
using Mafi;
using Mafi.Core.World.Entities;
using System.Reflection;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
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
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new world map mine parameters as <b>double[]</b> array:<br/>
        /// <c>double[] dataWaterWell = new double[] { 15, 10.0, 0.05, 20, 2 };</c>
        /// 
        /// <br/><br/>Apply new world map mine parameters via <b>WorldMapMineProto</b> identifier:<br/>
        /// <c>SetMineProduction(Ids.World.WaterWell, dataWaterWell);</c>
        /// </remarks>
        public static void SetMineProduction(Mafi.Core.Entities.EntityProto.ID refMineID, double[] mineData) {
            if (_pReg == null) { ModLog.Warning($"SetMineProduction: the ProtoRegistrator is not referenced!"); return; };
            WorldMapMineProto refMine = WorldMineRef(refMineID);
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
            SyncProtoMod(refMine);
        }
    }
}