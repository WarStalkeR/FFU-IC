using Mafi.Base;
using Mafi.Core.Entities;
using Mafi.Core.Mods;
using Mafi.Core.World.Entities;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldSites : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions

        // Localization Definitions

        // Reflection Helpers

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // World Mine References
            WorldMapMineProto oilRig1 = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.OilRigCost1);
            WorldMapMineProto oilRig2 = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.OilRigCost2);
            WorldMapMineProto oilRig3 = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.OilRigCost3);
            WorldMapMineProto waterWell = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.WaterWell);
            WorldMapMineProto mineSulfur = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.SulfurMine);
            WorldMapMineProto mineCoal = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.CoalMine);
            WorldMapMineProto mineQuartz = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.QuartzMine);
            WorldMapMineProto mineUranium = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.UraniumMine);
            WorldMapMineProto mineLimestone = FFU_IC_IDs.WorldMineRef(pReg, Ids.World.LimestoneMine);

        }
    }
}