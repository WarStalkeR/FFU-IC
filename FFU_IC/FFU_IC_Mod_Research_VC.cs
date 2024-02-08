using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Localization;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
        public void Register_ResearchVC(ProtoRegistrator registrator) {
            ResearchNodeProto NodeRef(ResearchNodeProto.ID rNodeID) => registrator.PrototypesDb.Get<ResearchNodeProto>(rNodeID).Value;
            void NodeRemoveFromDB(ResearchNodeProto.ID rNodeID) => registrator.PrototypesDb.RemoveOrThrow(rNodeID);
            void NodeSetPosition(ResearchNodeProto.ID rNodeID, Vector2i rPos) => NodeRef(rNodeID).GridPosition = rPos;
            void NodeAddParent(ResearchNodeProto.ID rNodeID, ResearchNodeProto.ID pNodeID) => NodeRef(rNodeID).AddParent(NodeRef(pNodeID));

            string vehCapIcon = "Assets/Base/Icons/VehicleLimitIncrease.svg";
            LocStr1 vehCapRefStr = Loc.Str1(Ids.Research.VehicleCapIncrease.ToString() + "__desc", "Increases vehicle limit by {0}.", "{0}=40");
            LocStr vehCapDesc = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Research.VehicleCapIncrease.ToString() + "_formatted20", vehCapRefStr.Format(vehCapLimit.ToString()).Value);

            NodeRemoveFromDB(Ids.Research.VehicleCapIncrease);
            NodeRemoveFromDB(Ids.Research.VehicleCapIncrease2);
            NodeRemoveFromDB(Ids.Research.VehicleCapIncrease3);
            NodeRemoveFromDB(Ids.Research.VehicleCapIncrease4);
            NodeRemoveFromDB(Ids.Research.VehicleCapIncrease5);
            NodeRemoveFromDB(Ids.Research.VehicleCapIncrease6);

            registrator.ResearchNodeProtoBuilder.Start("Vehicles management", Ids.Research.VehicleCapIncrease)
                .Description(vehCapDesc)
                .AddVehicleCapIncrease(vehCapLimit, vehCapIcon)
            .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Vehicles management II", Ids.Research.VehicleCapIncrease2)
                .Description(vehCapDesc)
                .AddVehicleCapIncrease(vehCapLimit, vehCapIcon)
            .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Vehicles management III", Ids.Research.VehicleCapIncrease3)
                .Description(vehCapDesc)
                .AddVehicleCapIncrease(vehCapLimit, vehCapIcon)
                .AddLayoutEntityToUnlock(Ids.Buildings.BarrierStraight1)
                .AddLayoutEntityToUnlock(Ids.Buildings.BarrierCorner)
                .AddLayoutEntityToUnlock(Ids.Buildings.BarrierCross)
                .AddLayoutEntityToUnlock(Ids.Buildings.BarrierEnding)
                .AddLayoutEntityToUnlock(Ids.Buildings.BarrierTee)
            .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Vehicles management IV", Ids.Research.VehicleCapIncrease4)
                .Description(vehCapDesc)
                .AddVehicleCapIncrease(vehCapLimit, vehCapIcon)
            .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Vehicles management V", Ids.Research.VehicleCapIncrease5)
                .Description(vehCapDesc)
                .AddVehicleCapIncrease(vehCapLimit, vehCapIcon)
            .BuildAndAdd();
            registrator.ResearchNodeProtoBuilder.Start("Vehicles management VI", Ids.Research.VehicleCapIncrease6)
                .Description(vehCapDesc)
                .AddVehicleCapIncrease(vehCapLimit, vehCapIcon)
            .BuildAndAdd();

            NodeSetPosition(Ids.Research.VehicleCapIncrease, new Vector2i(24, 22));
            NodeSetPosition(Ids.Research.VehicleCapIncrease2, new Vector2i(36, 24));
            NodeSetPosition(Ids.Research.VehicleCapIncrease3, new Vector2i(52, 40));
            NodeSetPosition(Ids.Research.VehicleCapIncrease4, new Vector2i(60, 26));
            NodeSetPosition(Ids.Research.VehicleCapIncrease5, new Vector2i(92, 29));
            NodeSetPosition(Ids.Research.VehicleCapIncrease6, new Vector2i(116, 11));

            NodeAddParent(Ids.Research.VehicleCapIncrease, Ids.Research.FuelStation);
            NodeAddParent(Ids.Research.VehicleCapIncrease2, Ids.Research.ResearchLab2);
            NodeAddParent(Ids.Research.VehicleCapIncrease3, Ids.Research.CaptainsOffice2);
            NodeAddParent(Ids.Research.VehicleCapIncrease4, Ids.Research.ResearchLab3);
            NodeAddParent(Ids.Research.VehicleCapIncrease5, Ids.Research.ResearchLab4);
            NodeAddParent(Ids.Research.VehicleCapIncrease6, Ids.Research.ResearchLab5);
        }
    }
}