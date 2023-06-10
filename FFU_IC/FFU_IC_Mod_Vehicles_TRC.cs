using Mafi;
using Mafi.Base;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Vehicles : IModData {
        public void Register_VehiclesTRC(ProtoRegistrator registrator) {
            DrivingEntityProto TruckRef(DynamicEntityProto.ID rExcavID) => registrator.PrototypesDb.Get<DrivingEntityProto>(rExcavID).Value;
            void TruckRemoveFromDB(DynamicEntityProto.ID rExcavID) => registrator.PrototypesDb.RemoveOrThrow(rExcavID);

            Percent NoFuelMaxSpeedPerc = 40.Percent();
            LocStr2 truckStrT1 = Loc.Str2(Ids.Vehicles.TruckT1.Id.ToString() + "__desc", "Heavy duty pickup truck with max capacity of {0}. It can go under transports that are at height {1} or higher.", "truck description, for instance {0}=20,{1}=2");
            LocStr2 truckStrT2 = Loc.Str2(Ids.Vehicles.TruckT2.Id.ToString() + "__desc", "Large industrial truck with max capacity of {0}. It can go under transports if they are at height {1} or higher.", "vehicle description, for instance {0}=20,{1}=2");
            LocStr1 truckStrT3a = Loc.Str1(Ids.Vehicles.TruckT3Loose.Id.ToString() + "__desc", "Large hauling truck with max capacity of {0}. This type can transport only loose products (coal for instance). It cannot go under transports.", "vehicle description, for instance {0}=150");
            LocStr1 truckStrT3b = Loc.Str1(Ids.Vehicles.TruckT3Fluid.Id.ToString() + "__desc", "Large hauling truck with max capacity of {0}. This type can transport only liquid or gas products. It cannot go under transports.", "vehicle description, for instance {0}=150");

            TruckRemoveFromDB(Ids.Vehicles.TruckT1.Id);
            TruckRemoveFromDB(Ids.Vehicles.TruckT2.Id);
            TruckRemoveFromDB(Ids.Vehicles.TruckT3Loose.Id);
            TruckRemoveFromDB(Ids.Vehicles.TruckT3Fluid.Id);

            registrator.TruckProtoBuilder.Start("Pickup", Ids.Vehicles.TruckT1.Id)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.TruckT1.Id.ToString() + "_formatted", truckStrT1.Format(truckCapT1.ToString(), 2.ToString()).Value))
                .SetCosts(Costs.Vehicles.TruckT1)
                .SetDurationToBuild(60.Seconds())
                .SetCapacity(truckCapT1)
                .SetDumpingDistance(2.Tiles(), 7.Tiles())
                .SetSizeInMeters(10.0, 2.5, 2.0)
                .SetWheelDiameter(0.8.Meters())
                .SetDrivingData(new DrivingData(truckFwdSpeedT1.Tiles(), truckBckSpeedT1.Tiles(), 50.Percent(), 0.06.Tiles(), 0.09.Tiles(), 60.Degrees(), 20.Degrees(), 2.5.ToFix32(), 1.25.Tiles(), 1.25.Tiles(), NoFuelMaxSpeedPerc))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.CanPassUnder, 100.Percent()))
                .SetDisruptionByDistance(16, 8)
                .SetPrefabPath("Assets/Base/Vehicles/ModularTruckSmall/TruckS_Base.prefab")
                .SetTerrainContactPointsOffsets(new RelTile2f(1.75.ToFix32(), 0.75.ToFix32()), new RelTile2f(1.75.ToFix32(), 0.75.ToFix32()))
                .SetSteeringWheelsSubmodelPaths("Truck-low-front-left", "Truck-low-front-right")
                .SetStaticWheelsSubmodelPaths("Truck-low-back-left", "Truck-low-back-right")
                .SetDumpedThicknessByDistanceMeters(1f, 0.8f, 0.5f)
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.TruckT1.Id).SetReserve(2.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(5), 11.4.Minutes()).BuildTank())
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.4f, new RelTile3f(-1, 0, 0), 50f, 0.3.Tiles()))
                .SetEngineSound("Assets/Base/Vehicles/ModularTruckSmall/Audio/Engine.prefab")
                .AddAttachment(new AttachmentProto(Ids.Vehicles.TruckT1.AttachmentTank, (ProductProto x) => x is FluidProductProto, new AttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckSmall/TruckS_Tank.prefab", null, delegate (ProductProto product) {
                    if (product.Id.Value == Ids.Products.Water.Value) return new ColorRgba(13157631);
                    return (product.Id.Value == Ids.Products.CrudeOil.Value) ? new ColorRgba(7829628) : ColorRgba.Empty;
                }), keepOnEvenIfNotNeeded: false))
                .AddAttachment(new FlatBedAttachmentProto(Ids.Vehicles.TruckT1.AttachmentFlatBed, (ProductProto x) => x is CountableProductProto, new FlatBedAttachmentProto.Gfx(1, "Assets/Base/Vehicles/ModularTruckSmall/TruckS_Flat.prefab"), keepOnEvenIfNotNeeded: true))
                .AddAttachment(new DumpAttachmentProto(Ids.Vehicles.TruckT1.AttachmentDump, new DumpAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckSmall/TruckS_Dump.prefab", "Box170/PileSmooth", "Box170/PileRough", new LoosePileTextureParams(0.8f), new Vector3f(2, 0.05.ToFix32(), 0), new Vector3f(2, 1.2.ToFix32(), 0))))
                .SetNextTier(TruckRef(Ids.Vehicles.TruckT2.Id))
                .BuildAndAdd();
            registrator.TruckProtoBuilder.Start("Truck", Ids.Vehicles.TruckT2.Id)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.TruckT2.Id.ToString() + "_formatted", truckStrT2.Format(truckCapT2.ToString(), 2.ToString()).Value))
                .SetCosts(Costs.Vehicles.TruckT2)
                .SetDurationToBuild(120.Seconds())
                .SetCapacity(truckCapT2)
                .SetDumpingDistance(3.Tiles(), 8.Tiles())
                .SetSizeInMeters(10.0, 2.5, 2.0)
                .SetWheelDiameter(1.2.Meters())
                .SetDrivingData(new DrivingData(truckFwdSpeedT2.Tiles(), truckBckSpeedT2.Tiles(), 50.Percent(), 0.06.Tiles(), 0.09.Tiles(), 60.Degrees(), 20.Degrees(), 2.5.ToFix32(), 1.5.Tiles(), 1.5.Tiles(), NoFuelMaxSpeedPerc))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.CanPassUnder, 100.Percent()))
                .SetDisruptionByDistance(22, 10)
                .SetPrefabPath("Assets/Base/Vehicles/ModularTruck/TruckBase.prefab")
                .SetTerrainContactPointsOffsets(new RelTile2f(2, 0.75.ToFix32()), new RelTile2f(2, 0.75.ToFix32()))
                .SetSteeringWheelsSubmodelPaths("wheel_front_left", "wheel_front_right")
                .SetStaticWheelsSubmodelPaths("wheel_middle_left", "wheel_middle_right", "wheel_back1_left", "wheel_back1_right", "wheel_back2_left", "wheel_back2_right")
                .SetDumpedThicknessByDistanceMeters(1.5f, 1.2f, 0.6f, 0.4f)
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.TruckT2.Id).SetReserve(2.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(15), 14.Minutes()).BuildTank())
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.8f, new RelTile3f(0, 0, 0), 50f, 0.3.Tiles()))
                .SetEngineSound("Assets/Base/Vehicles/ModularTruck/Audio/Engine.prefab")
                .AddAttachment(new AttachmentProto(Ids.Vehicles.TruckT2.AttachmentTank, (ProductProto x) => x is FluidProductProto, new AttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruck/Truck_Tank.prefab", null, delegate (ProductProto product) {
                    if (product.Id.Value == Ids.Products.Water.Value) return new ColorRgba(13157631);
                    return (product.Id.Value == Ids.Products.CrudeOil.Value) ? new ColorRgba(7829628) : ColorRgba.Empty;
                }), keepOnEvenIfNotNeeded: false))
                .AddAttachment(new FlatBedAttachmentProto(Ids.Vehicles.TruckT2.AttachmentFlatBed, (ProductProto x) => x is CountableProductProto, new FlatBedAttachmentProto.Gfx(2, "Assets/Base/Vehicles/ModularTruck/Truck_Flat.prefab"), keepOnEvenIfNotNeeded: true))
                .AddAttachment(new DumpAttachmentProto(Ids.Vehicles.TruckT2.AttachmentDump, new DumpAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruck/Truck_Dump.prefab", "Object010/PileSmooth", "Object010/PileRough", LoosePileTextureParams.Default, new Vector3f(2.6.ToFix32(), 0.2.ToFix32(), 0), new Vector3f(2.6.ToFix32(), 1.9.ToFix32(), 0))))
                .BuildAndAdd();
            registrator.TruckProtoBuilder.Start("Haul truck (dump)", Ids.Vehicles.TruckT3Loose.Id)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.TruckT3Loose.Id.ToString() + "_formatted", truckStrT3a.Format(truckCapT3.ToString()).Value))
                .SetCosts(Costs.Vehicles.TruckT3)
                .SetDurationToBuild(240.Seconds())
                .SetCapacity(truckCapT3)
                .SetDumpingDistance(4.Tiles(), 9.Tiles())
                .SetSizeInMeters(12.0, 7.0, 6.0)
                .SetWheelDiameter(3.9.Meters())
                .SetDrivingData(new DrivingData(truckFwdSpeedT3.Tiles(), truckBckSpeedT3.Tiles(), 50.Percent(), 0.02.Tiles(), 0.06.Tiles(), 60.Degrees(), 15.Degrees(), 3.0.ToFix32(), 1.5.Tiles(), 1.5.Tiles(), NoFuelMaxSpeedPerc))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(5), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 100.Percent()))
                .SetDisruptionByDistance(0, 32, 32)
                .SetPrefabPath("Assets/Base/Vehicles/ModularTruckT3/TruckT3Base.prefab")
                .SetTerrainContactPointsOffsets(new RelTile2f(1.5.ToFix32(), 1.5.ToFix32()), new RelTile2f(1.5.ToFix32(), 1.5.ToFix32()))
                .SetSteeringWheelsSubmodelPaths("wheel_front_left", "wheel_front_right")
                .SetStaticWheelsSubmodelPaths("wheel_back")
                .SetDumpedThicknessByDistanceMeters(1.5f, 1.5f, 1.2f, 0.8f)
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.TruckT3Loose.Id).SetReserve(2.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(36), 16.Minutes()).BuildTank())
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f(0, -1.5.ToFix32(), 0), 50f, 0.2.Tiles()))
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f(0, 1.5.ToFix32(), 0), 50f, 0.2.Tiles()))
                .SetEngineSound("Assets/Base/Vehicles/ModularTruckT3/Audio/Engine.prefab")
                .SetFixedProductType(LooseProductProto.ProductType)
                .AddAttachment(new DumpAttachmentProto(Ids.Vehicles.TruckT3Loose.AttachmentDump, new DumpAttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckT3/TruckT3Dump.prefab", "korba/PileSmooth", "korba/PileSmooth", LoosePileTextureParams.Default, "Main")))
                .BuildAndAdd();
            registrator.TruckProtoBuilder.Start("Haul truck (tank)", Ids.Vehicles.TruckT3Fluid.Id)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.TruckT3Fluid.Id.ToString() + "_formatted", truckStrT3a.Format(truckCapT3.ToString()).Value))
                .SetCosts(Costs.Vehicles.TruckT3)
                .SetDurationToBuild(240.Seconds())
                .SetCapacity(truckCapT3)
                .SetDumpingDistance(4.Tiles(), 9.Tiles())
                .SetSizeInMeters(12.0, 7.0, 6.0)
                .SetWheelDiameter(3.9.Meters())
                .SetDrivingData(new DrivingData(truckFwdSpeedT3.Tiles(), truckBckSpeedT3.Tiles(), 50.Percent(), 0.02.Tiles(), 0.06.Tiles(), 60.Degrees(), 15.Degrees(), 3.0.ToFix32(), 1.5.Tiles(), 1.5.Tiles(), NoFuelMaxSpeedPerc))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(5), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 100.Percent()))
                .SetDisruptionByDistance(0, 32, 32)
                .SetPrefabPath("Assets/Base/Vehicles/ModularTruckT3/TruckT3Base.prefab")
                .SetTerrainContactPointsOffsets(new RelTile2f(1.5.ToFix32(), 1.5.ToFix32()), new RelTile2f(1.5.ToFix32(), 1.5.ToFix32()))
                .SetSteeringWheelsSubmodelPaths("wheel_front_left", "wheel_front_right")
                .SetStaticWheelsSubmodelPaths("wheel_back")
                .SetDumpedThicknessByDistanceMeters(1.5f, 1.5f, 1.2f, 0.8f)
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.TruckT3Fluid.Id).SetReserve(2.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(36), 16.Minutes()).BuildTank())
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f(0, -1.5.ToFix32(), 0), 50f, 0.2.Tiles()))
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f(0, 1.5.ToFix32(), 0), 50f, 0.2.Tiles()))
                .SetEngineSound("Assets/Base/Vehicles/ModularTruckT3/Audio/Engine.prefab")
                .SetFixedProductType(FluidProductProto.ProductType)
                .AddAttachment(new AttachmentProto(Ids.Vehicles.TruckT3Fluid.AttachmentTank, (ProductProto x) => x.Type == FluidProductProto.ProductType, new AttachmentProto.Gfx("Assets/Base/Vehicles/ModularTruckT3/TruckT3Tank.prefab"), keepOnEvenIfNotNeeded: true))
                .BuildAndAdd();
        }
    }
}