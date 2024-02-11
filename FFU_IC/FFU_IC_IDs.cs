using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.World.Entities;

namespace FFU_Industrial_Capacity {
    public static class FFU_IC_IDs {
        public static StorageProto StorageRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<StorageProto>(refID).Value;
        public static ThermalStorageProto ThermalRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<ThermalStorageProto>(refID).Value;
        public static NuclearWasteStorageProto NuclearRef(ProtoRegistrator pReg, StaticEntityProto.ID refID) => pReg.PrototypesDb.Get<NuclearWasteStorageProto>(refID).Value;
        public static MachineProto MachineRef(ProtoRegistrator pReg, MachineProto.ID refID) => pReg.PrototypesDb.Get<MachineProto>(refID).Value;
        public static TruckProto TruckRef(ProtoRegistrator pReg, DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<TruckProto>(refID).Value;
        public static ExcavatorProto ExcavRef(ProtoRegistrator pReg, DynamicEntityProto.ID refID) => pReg.PrototypesDb.Get<ExcavatorProto>(refID).Value;
        public static WorldMapMineProto WorldMineRef(ProtoRegistrator pReg, EntityProto.ID refID) => pReg.PrototypesDb.Get<WorldMapMineProto>(refID).Value;
        public static RecipeProto RecipeRef(ProtoRegistrator pReg, RecipeProto.ID refID) => pReg.PrototypesDb.Get<RecipeProto>(refID).Value;
        public static ResearchNodeProto ResearchRef(ProtoRegistrator pReg, ResearchNodeProto.ID refID) => pReg.PrototypesDb.Get<ResearchNodeProto>(refID).Value;
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
        }
        public static class Research {
            public static readonly ResearchNodeProto.ID None = new ResearchNodeProto.ID("None");
        }
    }
}