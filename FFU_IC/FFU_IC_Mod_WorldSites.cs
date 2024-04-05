using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.World.Entities;
using System.Collections.Generic;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldSites : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions
        public readonly Dictionary<string, double[]> MineProdData =
            new Dictionary<string, double[]>() {
            // Production, Seconds, Unity Upkeep, Max Level, Levels/Upgrade
            { "OilRig", new double[] { 18, 20.0, 0.05, 50, 5 }},
        };

        // Localization Definitions

        // Reflection Helpers
        public WorldMapMineProto WmRef(Mafi.Core.Entities.EntityProto.ID refID) => FFU_IC_IDs.WorldMineRef(pReg, refID);
        public void SetMineProductionData(WorldMapMineProto refMine, double[] mineData) {
            if (refMine == null) { ModLog.Warning($"SetVehicleDriveData: 'refMine' is undefined!"); return; }
            if (mineData == null) { ModLog.Warning($"SetVehicleDriveData: 'mineData' is undefined!"); return; }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // World Mine References
            WorldMapMineProto oilRig1 = WmRef(Ids.World.OilRigCost1);
            WorldMapMineProto oilRig2 = WmRef(Ids.World.OilRigCost2);
            WorldMapMineProto oilRig3 = WmRef(Ids.World.OilRigCost3);
            WorldMapMineProto waterWell = WmRef(Ids.World.WaterWell);
            WorldMapMineProto mineSulfur = WmRef(Ids.World.SulfurMine);
            WorldMapMineProto mineCoal = WmRef(Ids.World.CoalMine);
            WorldMapMineProto mineQuartz = WmRef(Ids.World.QuartzMine);
            WorldMapMineProto mineUranium = WmRef(Ids.World.UraniumMine);
            WorldMapMineProto mineLimestone = WmRef(Ids.World.LimestoneMine);

            // World Mine Modifications
        }
    }
}