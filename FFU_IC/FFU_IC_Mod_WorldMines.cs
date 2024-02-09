using Mafi.Base;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Mods;
using Mafi.Core.World.Entities;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_WorldMines : IModData {
        // Modification Definitions

        // Localization Definitions

        // Reflection Helpers
        public WorldMapMineProto WorldMineRef(ProtoRegistrator pReg, EntityProto.ID refID) => pReg.PrototypesDb.Get<WorldMapMineProto>(refID).Value;

        public void RegisterData(ProtoRegistrator registrator) {
            // World Mine References
            WorldMapMineProto oilRig1 = WorldMineRef(registrator, Ids.World.OilRigCost1);
            WorldMapMineProto oilRig2 = WorldMineRef(registrator, Ids.World.OilRigCost2);
            WorldMapMineProto oilRig3 = WorldMineRef(registrator, Ids.World.OilRigCost3);
            WorldMapMineProto waterWell = WorldMineRef(registrator, Ids.World.WaterWell);
            WorldMapMineProto mineSulfur = WorldMineRef(registrator, Ids.World.SulfurMine);
            WorldMapMineProto mineCoal = WorldMineRef(registrator, Ids.World.CoalMine);
            WorldMapMineProto mineQuartz = WorldMineRef(registrator, Ids.World.QuartzMine);
            WorldMapMineProto mineUranium = WorldMineRef(registrator, Ids.World.UraniumMine);
            WorldMapMineProto mineLimestone = WorldMineRef(registrator, Ids.World.LimestoneMine);

        }
    }
}