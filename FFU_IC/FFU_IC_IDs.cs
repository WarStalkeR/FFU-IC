using Mafi;
using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
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
using System.Reflection;

namespace FFU_Industrial_Capacity {
    public static class FFU_IC_IDs {
        public static StorageProto StorageRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<StorageProto>(refID).Value;
        public static ThermalStorageProto ThermalRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<ThermalStorageProto>(refID).Value;
        public static NuclearWasteStorageProto NuclearRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<NuclearWasteStorageProto>(refID).Value;
        public static SettlementFoodModuleProto MarketRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<SettlementFoodModuleProto>(refID).Value;
        public static MachineProto MachineRef(ProtoRegistrator pReg, MachineProto.ID refID) => pReg.PrototypesDb.Get<MachineProto>(refID).Value;
        public static TruckProto TruckRef(ProtoRegistrator pReg, DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TruckProto>(refID).Value;
        public static ExcavatorProto ExcavRef(ProtoRegistrator pReg, DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<ExcavatorProto>(refID).Value;
        public static TreeHarvesterProto TrHarvRef(ProtoRegistrator pReg, DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TreeHarvesterProto>(refID).Value;
        public static TreePlanterProto TrPlantRef(ProtoRegistrator pReg, DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TreePlanterProto>(refID).Value;
        public static WorldMapMineProto WorldMineRef(ProtoRegistrator pReg, EntityProto.ID refID) => pReg.PrototypesDb.Get<WorldMapMineProto>(refID).Value;
        public static TransportProto TransportRef(ProtoRegistrator pReg, EntityProto.ID refID) => pReg.PrototypesDb.Get<TransportProto>(refID).Value;
        public static RecipeProto RecipeRef(ProtoRegistrator pReg, RecipeProto.ID refID) => pReg.PrototypesDb.Get<RecipeProto>(refID).Value;
        public static ProductProto ProductRef(ProtoRegistrator pReg, ProductProto.ID refID) => pReg.PrototypesDb.Get<ProductProto>(refID).Value;
        public static ResearchNodeProto ResearchRef(ProtoRegistrator pReg, ResearchNodeProto.ID refID) => pReg.PrototypesDb.Get<ResearchNodeProto>(refID).Value;
        public static void setTechPosition(ProtoRegistrator pReg, ResearchNodeProto.ID nodeId, Vector2i nodePos) {
            Option<ResearchNodeProto> optNode = pReg.PrototypesDb.Get<ResearchNodeProto>(nodeId);
            if (optNode.IsNone) {
                Log.Warning(string.Format("Failed to set position of research node: {0}", nodeId));
                return;
            }
            optNode.Value.GridPosition = nodePos;
        }
        public static void setTechParent(ProtoRegistrator pReg, ResearchNodeProto.ID parentId, ResearchNodeProto.ID ofId) {
            Option<ResearchNodeProto> optParent = pReg.PrototypesDb.Get<ResearchNodeProto>(parentId);
            Option<ResearchNodeProto> optOf = pReg.PrototypesDb.Get<ResearchNodeProto>(ofId);
            if (optParent.IsNone || optOf.IsNone) {
                Log.Warning(string.Format("Failed to set research relationship: {0} => {1}", ofId, parentId));
                return;
            }
            optOf.Value.AddParent(optParent.Value);
        }
        public static void SyncProtoMod(Mafi.Core.Prototypes.Proto refEntity) {
            if (FFU_IC_Base.RefMod == null) { ModLog.Warning($"SyncProtoInfo: 'RefMod' is undefined!"); return; }
            if (FFU_IC_Base.RefMod.Name == refEntity.Mod.Name && FFU_IC_Base.RefMod.Version == refEntity.Mod.Version) return;
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldMod = typeProto.GetDeclaredField("<Mod>k__BackingField");
            if (fieldMod != null) fieldMod.SetValue(refEntity, FFU_IC_Base.RefMod);
        }
        public static class Buildings {
            public static readonly StaticEntityProto.ID None = new StaticEntityProto.ID("None");
        }
        public static class Machines {
            public static readonly MachineProto.ID None = new MachineProto.ID("None");
        }
        public static class Vehicles {
            public static readonly DynamicEntityProto.ID None = new DynamicEntityProto.ID("None");
        }
        public static class World {
            public static readonly EntityProto.ID None = new EntityProto.ID("None");
        }
        public static class Recipes {
            public static readonly RecipeProto.ID None = new RecipeProto.ID("None");
            public static readonly RecipeProto.ID IronSmeltingArcHalfScrap = new RecipeProto.ID("IronSmeltingArcHalfScrap");
            public static readonly RecipeProto.ID CopperSmeltingArcHalfScrap = new RecipeProto.ID("CopperSmeltingArcHalfScrap");
            public static readonly RecipeProto.ID GlassSmeltingArcHalfWithBroken = new RecipeProto.ID("GlassSmeltingArcHalfWithBroken");
            public static readonly RecipeProto.ID IronSmeltingArcColdScrap = new RecipeProto.ID("IronSmeltingArcColdScrap");
            public static readonly RecipeProto.ID CopperSmeltingArcColdScrap = new RecipeProto.ID("CopperSmeltingArcColdScrap");
            public static readonly RecipeProto.ID GlassSmeltingArcColdWithBroken = new RecipeProto.ID("GlassSmeltingArcColdWithBroken");
            public static readonly RecipeProto.ID ExhaustFilteringCold = new RecipeProto.ID("ExhaustFilteringCold");
        }
        public static class Research {
            public static readonly ResearchNodeProto.ID None = new ResearchNodeProto.ID("None");
        }
    }
}