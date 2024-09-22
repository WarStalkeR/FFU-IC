using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Research;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.World.Entities;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
        private static ProtoRegistrator _pReg = null;
        private static string _logPrefix = null;
        private static IMod _refMod = null;
        public static ProtoRegistrator ProtoReg {
            get { return _pReg; }
            set { _pReg = value; }
        }
        public static IMod ModReference {
            get { return _refMod; }
            set { _refMod = value; }
        }
        public static string ModLoggerPrefix {
            get { return _logPrefix; }
            set { _logPrefix = $"[{value}]"; }
        }
        private static EntityProto EntityRef(EntityProto.ID refID) => _pReg.PrototypesDb.Get<EntityProto>(refID).Value;
        private static StorageProto StorageRef(StaticEntityProto.ID refID) => _pReg.PrototypesDb.Get<StorageProto>(refID).Value;
        private static ThermalStorageProto ThermalRef(StaticEntityProto.ID refID) => _pReg.PrototypesDb.Get<ThermalStorageProto>(refID).Value;
        private static NuclearWasteStorageProto NuclearRef(StaticEntityProto.ID refID) => _pReg.PrototypesDb.Get<NuclearWasteStorageProto>(refID).Value;
        private static SettlementFoodModuleProto MarketRef(StaticEntityProto.ID refID) => _pReg.PrototypesDb.Get<SettlementFoodModuleProto>(refID).Value;
        private static MachineProto MachineRef(MachineProto.ID refID) => _pReg.PrototypesDb.Get<MachineProto>(refID).Value;
        private static TruckProto TruckRef(DynamicEntityProto.ID refID) => _pReg.PrototypesDb.Get<TruckProto>(refID).Value;
        private static ExcavatorProto ExcavRef(DynamicEntityProto.ID refID) => _pReg.PrototypesDb.Get<ExcavatorProto>(refID).Value;
        private static TreeHarvesterProto TrHarvRef(DynamicEntityProto.ID refID) => _pReg.PrototypesDb.Get<TreeHarvesterProto>(refID).Value;
        private static TreePlanterProto TrPlantRef(DynamicEntityProto.ID refID) => _pReg.PrototypesDb.Get<TreePlanterProto>(refID).Value;
        private static WorldMapMineProto WorldMineRef(EntityProto.ID refID) => _pReg.PrototypesDb.Get<WorldMapMineProto>(refID).Value;
        private static TransportProto TransportRef(EntityProto.ID refID) => _pReg.PrototypesDb.Get<TransportProto>(refID).Value;
        private static RecipeProto RecipeRef(RecipeProto.ID refID) => _pReg.PrototypesDb.Get<RecipeProto>(refID).Value;
        private static ProductProto ProductRef(ProductProto.ID refID) => _pReg.PrototypesDb.Get<ProductProto>(refID).Value;
        private static ResearchNodeProto ResearchRef(ResearchNodeProto.ID refID) => _pReg.PrototypesDb.Get<ResearchNodeProto>(refID).Value;
        private static void SyncProtoMod(Mafi.Core.Prototypes.Proto refEntity) {
            if (_refMod == null) { ModLog.Warning($"SyncProtoInfo: 'RefMod' is undefined!"); return; }
            if (_refMod.Name == refEntity.Mod.Name && _refMod.Version == refEntity.Mod.Version) return;
            refEntity.Mod = _refMod;
        }
    }
}