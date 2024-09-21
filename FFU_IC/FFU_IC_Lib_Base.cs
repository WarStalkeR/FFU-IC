using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Research;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.World.Entities;

namespace FFU_Industrial_Capacity {
    public static partial class FFU_IC_Lib {
        private static ProtoRegistrator pReg = null;
        public static ProtoRegistrator ProtoReg {
            get { return pReg; }
            set { pReg = value; }
        }
        private static StorageProto StorageRef(StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<StorageProto>(refID).Value;
        private static ThermalStorageProto ThermalRef(StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<ThermalStorageProto>(refID).Value;
        private static NuclearWasteStorageProto NuclearRef(StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<NuclearWasteStorageProto>(refID).Value;
        private static SettlementFoodModuleProto MarketRef(StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<SettlementFoodModuleProto>(refID).Value;
        private static MachineProto MachineRef(MachineProto.ID refID) => pReg.PrototypesDb.Get<MachineProto>(refID).Value;
        private static TruckProto TruckRef(DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TruckProto>(refID).Value;
        private static ExcavatorProto ExcavRef(DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<ExcavatorProto>(refID).Value;
        private static TreeHarvesterProto TrHarvRef(DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TreeHarvesterProto>(refID).Value;
        private static TreePlanterProto TrPlantRef(DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TreePlanterProto>(refID).Value;
        private static WorldMapMineProto WorldMineRef(EntityProto.ID refID) => pReg.PrototypesDb.Get<WorldMapMineProto>(refID).Value;
        private static TransportProto TransportRef(EntityProto.ID refID) => pReg.PrototypesDb.Get<TransportProto>(refID).Value;
        private static RecipeProto RecipeRef(RecipeProto.ID refID) => pReg.PrototypesDb.Get<RecipeProto>(refID).Value;
        private static ProductProto ProductRef(ProductProto.ID refID) => pReg.PrototypesDb.Get<ProductProto>(refID).Value;
        private static ResearchNodeProto ResearchRef(ResearchNodeProto.ID refID) => pReg.PrototypesDb.Get<ResearchNodeProto>(refID).Value;
        private static void SyncProtoMod(Mafi.Core.Prototypes.Proto refEntity) {
            if (FFU_IC_Base.RefMod == null) { ModLog.Warning($"SyncProtoInfo: 'RefMod' is undefined!"); return; }
            if (FFU_IC_Base.RefMod.Name == refEntity.Mod.Name && FFU_IC_Base.RefMod.Version == refEntity.Mod.Version) return;
            refEntity.Mod = FFU_IC_Base.RefMod;
        }
    }
}