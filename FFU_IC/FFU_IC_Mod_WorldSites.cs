using Mafi.Base;
using Mafi.Core.Entities;
using Mafi.Core.Mods;
using Mafi.Core.World.Entities;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldSites : IModData {
        // Modification Variables
        ProtoRegistrator protoReg = null;

        // Modification Definitions

        // Localization Definitions

        // Reflection Helpers
        public WorldMapMineProto WorldMineRef(EntityProto.ID refID) => protoReg.PrototypesDb.Get<WorldMapMineProto>(refID).Value;

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            protoReg = registrator;

            // World Mine References
            WorldMapMineProto oilRig1 = WorldMineRef(Ids.World.OilRigCost1);
            WorldMapMineProto oilRig2 = WorldMineRef(Ids.World.OilRigCost2);
            WorldMapMineProto oilRig3 = WorldMineRef(Ids.World.OilRigCost3);
            WorldMapMineProto waterWell = WorldMineRef(Ids.World.WaterWell);
            WorldMapMineProto mineSulfur = WorldMineRef(Ids.World.SulfurMine);
            WorldMapMineProto mineCoal = WorldMineRef(Ids.World.CoalMine);
            WorldMapMineProto mineQuartz = WorldMineRef(Ids.World.QuartzMine);
            WorldMapMineProto mineUranium = WorldMineRef(Ids.World.UraniumMine);
            WorldMapMineProto mineLimestone = WorldMineRef(Ids.World.LimestoneMine);

        }
    }
}