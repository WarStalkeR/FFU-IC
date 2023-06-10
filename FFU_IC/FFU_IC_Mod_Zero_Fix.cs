using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Localization;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Zero_Fix : IResearchNodesData, IModData {
        //NOTE: Sadly, all researches that contain or depends on "replaced" items have to be "replaced" as well.
        //It will be great if we will get something like registrator.PrototypesDb.replaceProtoByID(ID, proto);
        public void RegisterData(ProtoRegistrator registrator) {
            ResearchNodeProto NodeRef(ResearchNodeProto.ID rNodeID) => registrator.PrototypesDb.Get<ResearchNodeProto>(rNodeID).Value;
            void NodeRemoveFromDB(ResearchNodeProto.ID rNodeID) => registrator.PrototypesDb.RemoveOrThrow(rNodeID);
            void NodeSetPosition(ResearchNodeProto.ID rNodeID, Vector2i rPos) => NodeRef(rNodeID).GridPosition = rPos;
            void NodeAddParent(ResearchNodeProto.ID rNodeID, ResearchNodeProto.ID pNodeID) => NodeRef(rNodeID).AddParent(NodeRef(pNodeID));
            
            NodeRemoveFromDB(Ids.Research.TerrainLeveling);
            NodeRemoveFromDB(Ids.Research.AdvancedLogisticsControl);
            NodeRemoveFromDB(Ids.Research.TrucksCapacityEdict);

            registrator.ResearchNodeProtoBuilder.Start("Terrain leveling", Ids.Research.TerrainLeveling)
                .UseDescriptionFrom(IdsCore.Technology.TerrainLeveling)
                .AddTechnologyToUnlock(IdsCore.Technology.TerrainLeveling)
                .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Advanced logistics control", Ids.Research.AdvancedLogisticsControl)
                .Description("Provides extra management of logistics and dumping.", "description of a research node in the research tree")
                .AddTechnologyToUnlock(IdsCore.Technology.CustomRoutes)
                .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Trucks overloading edict", Ids.Research.TrucksCapacityEdict)
                .AddEdictToUnlock(Ids.Edicts.TruckCapacityIncrease)
                .AddEdictToUnlock(Ids.Edicts.TruckCapacityIncreaseT2)
                .BuildAndAdd();

            NodeSetPosition(Ids.Research.TerrainLeveling, new Vector2i(28, 22));
            NodeSetPosition(Ids.Research.AdvancedLogisticsControl, new Vector2i(28, 26));
            NodeSetPosition(Ids.Research.TrucksCapacityEdict, new Vector2i(64, 26));

            NodeAddParent(Ids.Research.TerrainLeveling, Ids.Research.VehicleCapIncrease);
            NodeAddParent(Ids.Research.AdvancedLogisticsControl, Ids.Research.VehicleCapIncrease);
            NodeAddParent(Ids.Research.TrucksCapacityEdict, Ids.Research.VehicleCapIncrease4);
        }
    }
}