using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Vehicles : IModData {
        public void Register_VehiclesEXC(ProtoRegistrator registrator) {
            DrivingEntityProto ExcavRef(DynamicEntityProto.ID rExcavID) => registrator.PrototypesDb.Get<DrivingEntityProto>(rExcavID).Value;
            void ExcavRemoveFromDB(DynamicEntityProto.ID rExcavID) => registrator.PrototypesDb.RemoveOrThrow(rExcavID);

            Percent NoFuelMaxSpeedPerc = 40.Percent();
            LocStr1 excavStrT1 = Loc.Str1(Ids.Vehicles.ExcavatorT1.ToString() + "__desc", "Suitable for mining any terrain with max bucket capacity of {0}. It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=6");
            LocStr1 excavStrT2 = Loc.Str1(Ids.Vehicles.ExcavatorT2.ToString() + "__desc", "This is a serious mining machine with max bucket capacity of {0}! It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=18");
            LocStr1 excavStrT3 = Loc.Str1(Ids.Vehicles.ExcavatorT3.ToString() + "__desc", "Extremely large excavator that can mine any terrain with ease. It has bucket capacity of {0}. It cannot go under transports due to its size, use ramps to cross them.", "vehicle description, for instance {0}=60");

            ExcavRemoveFromDB(Ids.Vehicles.ExcavatorT1);
            ExcavRemoveFromDB(Ids.Vehicles.ExcavatorT2);
            ExcavRemoveFromDB(Ids.Vehicles.ExcavatorT3);

            registrator.ExcavatorProtoBuilder.Start("Small excavator", Ids.Vehicles.ExcavatorT1)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.ExcavatorT1.ToString() + "_formatted", excavStrT1.Format(excavCapT1.ToString()).Value))
                .SetCosts(Costs.Vehicles.ExcavatorT1)
                .SetDurationToBuild(100.Seconds())
                .SetCapacity(excavCapT1)
                .SetSizeInMeters(9.0, 4.5, 3.5)
                .SetMaxMiningDistance(new RelTile1i(2), new RelTile1i(5))
                .SetMinedThicknessByDistanceMeters(1.5f, 1f, 0.5f)
                .SetDrivingData(new DrivingData(excavFwdSpeedT1.Tiles(), excavBckSpeedT1.Tiles(), 50.Percent(), 0.04.Tiles(), 0.06.Tiles(), 10.Degrees(), 2.Degrees(), 2, RelTile1f.Zero, RelTile1f.Zero, NoFuelMaxSpeedPerc))
                .SetCabinDriver(new RotatingCabinDriverProto(10.Degrees(), 1.5.Degrees(), 2.Degrees(), 2.5.ToFix32()))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 40.Percent()))
                .SetDisruptionByDistance(0, 48)
                .SetTerrainContactPointsOffsets(new RelTile2f(1.075.ToFix32(), 1), new RelTile2f(1.075.ToFix32(), 1))
                .SetAnimationTimings(Duration.FromKeyframes(33), Duration.FromKeyframes(69), 3, Duration.FromKeyframes(15), Duration.FromKeyframes(60), Duration.FromKeyframes(73), Duration.FromKeyframes(23))
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.ExcavatorT1).SetReserve(2.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(10), 10.Minutes()).BuildTank())
                .SetTracksParameters(2.6.Meters(), 3.3.Meters())
                .SetPrefabPath("Assets/Base/Vehicles/ExcavatorT1.prefab")
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 1.8f, new RelTile3f(0, 0, 0), 60f, 0.1.Tiles(), 0.5f))
                .SetEngineSound("Assets/Base/Vehicles/ExcavatorT1/Audio/Engine.prefab")
                .SetMovementSound("Assets/Base/Vehicles/ExcavatorT1/Audio/Treads.prefab")
                .SetDigSounds(ImmutableArray.Create("Assets/Base/Vehicles/ExcavatorT1/Audio/Dig1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dig2.prefab"))
                .SetDumpSounds(ImmutableArray.Create("Assets/Base/Vehicles/ExcavatorT1/Audio/Dump1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump2.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump3.prefab"))
                .SetSubmodelNames("excavator-T1_body", "Base/TrackLeft", "Base/TrackRight", "excavator-T1_body/excavator-T1_rameno/Dummy_radlice/bagr_radlice", "SmoothHalf", "SmoothFull", "RoughHalf", "RoughFull")
                .SetAnimationNames("Idle", "PrepareMine", "Mine", "PrepareDump", "Dump")
                .SetIsTruckSupportedFunc((TruckProto truckProto) => (!truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType) && truckProto.Id != Ids.Vehicles.TruckT3Loose.Id)
                .SetNextTier(ExcavRef(Ids.Vehicles.ExcavatorT2))
                .BuildAndAdd();
            registrator.ExcavatorProtoBuilder.Start("Excavator", Ids.Vehicles.ExcavatorT2)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.ExcavatorT2.ToString() + "_formatted", excavStrT2.Format(excavCapT2.ToString()).Value))
                .SetCosts(Costs.Vehicles.ExcavatorT2)
                .SetDurationToBuild(200.Seconds())
                .SetCapacity(excavCapT2)
                .SetSizeInMeters(10.5, 5.5, 5.5)
                .SetMaxMiningDistance(new RelTile1i(3), new RelTile1i(7))
                .SetMinedThicknessByDistanceMeters(2f, 1.5f, 1f, 0.5f)
                .SetDrivingData(new DrivingData(excavFwdSpeedT2.Tiles(), excavBckSpeedT2.Tiles(), 50.Percent(), 0.025.Tiles(), 0.05.Tiles(), 8.Degrees(), 1.Degrees(), 2.5.ToFix32(), RelTile1f.Zero, RelTile1f.Zero, NoFuelMaxSpeedPerc))
                .SetCabinDriver(new RotatingCabinDriverProto(8.Degrees(), 1.Degrees(), 2.Degrees(), 2.5.ToFix32()))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(3), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 30.Percent()))
                .SetDisruptionByDistance(0, 64)
                .SetTerrainContactPointsOffsets(new RelTile2f(1.5.ToFix32(), 1.25.ToFix32()), new RelTile2f(1.5.ToFix32(), 1.25.ToFix32()))
                .SetAnimationTimings(Duration.FromKeyframes(30), Duration.FromKeyframes(70), 4, Duration.FromKeyframes(11), Duration.FromKeyframes(57), Duration.FromKeyframes(73), Duration.FromKeyframes(23))
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.ExcavatorT2).SetReserve(2.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(27), 12.Minutes()).BuildTank())
                .SetTracksParameters(3.6.Meters(), 3.55.Meters())
                .SetPrefabPath("Assets/Base/Vehicles/ExcavatorT2.prefab")
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2f, new RelTile3f(0, -1, 0), 55f, 0.1.Tiles(), 1f))
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2f, new RelTile3f(0, 1, 0), 55f, 0.1.Tiles(), 1f))
                .SetEngineSound("Assets/Base/Vehicles/ExcavatorT2/Audio/Engine.prefab")
                .SetMovementSound("Assets/Base/Vehicles/ExcavatorT2/Audio/Treads.prefab")
                .SetDigSounds(ImmutableArray.Create("Assets/Base/Vehicles/ExcavatorT1/Audio/Dig1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dig2.prefab"))
                .SetDumpSounds(ImmutableArray.Create("Assets/Base/Vehicles/ExcavatorT1/Audio/Dump1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump2.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump3.prefab"))
                .SetSubmodelNames("excavator-T2_body", "TrackLeft", "TrackRight", "excavator-T2_body/excavator-T1_rameno/Dummy_radlice005/ShovelPileParent", "SmoothHalf", "SmoothFull", "RoughHalf", "RoughFull")
                .SetAnimationNames("Idle", "PrepareMine", "Mine", "PrepareDump", "Dump")
                .SetIsTruckSupportedFunc((TruckProto truckProto) => (!truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType) && truckProto.Id != Ids.Vehicles.TruckT3Loose.Id)
                .SetNextTier(ExcavRef(Ids.Vehicles.ExcavatorT3))
                .BuildAndAdd();
            registrator.ExcavatorProtoBuilder.Start("Mega excavator", Ids.Vehicles.ExcavatorT3)
                .Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Vehicles.ExcavatorT3.ToString() + "_formatted", excavStrT3.Format(excavCapT3.ToString()).Value))
                .SetCosts(Costs.Vehicles.ExcavatorT3)
                .SetDurationToBuild(300.Seconds())
                .SetCapacity(excavCapT3)
                .SetSizeInMeters(13.0, 8.0, 8.0)
                .SetMaxMiningDistance(new RelTile1i(4), new RelTile1i(9))
                .SetMinedThicknessByDistanceMeters(2f, 2f, 1.5f, 1f, 0.5f)
                .SetDrivingData(new DrivingData(excavFwdSpeedT3.Tiles(), excavBckSpeedT3.Tiles(), 40.Percent(), 0.015.Tiles(), 0.03.Tiles(), 4.0.Degrees(), 0.4.Degrees(), 3.ToFix32(), RelTile1f.Zero, RelTile1f.Zero, NoFuelMaxSpeedPerc))
                .SetCabinDriver(new RotatingCabinDriverProto(6.Degrees(), 0.8.Degrees(), 1.5.Degrees(), 3.0.ToFix32()))
                .SetPathFindingParams(new VehiclePathFindingParams(new RelTile1i(5), SteepnessPathability.SlightSlopeAllowed, HeightClearancePathability.NoPassingUnder, 20.Percent()))
                .SetDisruptionByDistance(0, 0, 64)
                .SetTerrainContactPointsOffsets(new RelTile2f(1.75.ToFix32(), 2), new RelTile2f(1.75.ToFix32(), 2))
                .SetAnimationTimings(Duration.FromKeyframes(30), Duration.FromKeyframes(85), 5, Duration.FromKeyframes(11), Duration.FromKeyframes(50), Duration.FromKeyframes(120), Duration.FromKeyframes(15))
                .SetFuelTank((FuelTankProtoBuilder tb) => tb.Start(Ids.Vehicles.ExcavatorT3).SetReserve(3.Minutes()).SetProduct(Ids.Products.Diesel, new Quantity(70), 14.Minutes()).BuildTank())
                .SetTracksParameters(7.0.Meters(), 3.04.Meters())
                .SetPrefabPath("Assets/Base/Vehicles/ExcavatorT3.prefab")
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f(0, -1.5.ToFix32(), 0), 60f, 0.05.Tiles(), 3f))
                .AddDustSource(new DustParticlesSpec("Assets/Base/Vehicles/Dust/VehicleDustParticleSystem.prefab", 2.5f, new RelTile3f(0, 1.5.ToFix32(), 0), 60f, 0.05.Tiles(), 2f))
                .SetEngineSound("Assets/Base/Vehicles/ExcavatorT3/Audio/Engine.prefab")
                .SetMovementSound("Assets/Base/Vehicles/ExcavatorT3/Audio/Treads.prefab")
                .SetDigSounds(ImmutableArray.Create("Assets/Base/Vehicles/ExcavatorT1/Audio/Dig1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dig2.prefab"))
                .SetDumpSounds(ImmutableArray.Create("Assets/Base/Vehicles/ExcavatorT1/Audio/Dump1.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump2.prefab", "Assets/Base/Vehicles/ExcavatorT1/Audio/Dump3.prefab"))
                .SetSubmodelNames("rotate_base", "TrackLeft", "TrackRight", "rotate_base/base/ROtate_rameno/_bone-base/Dummy002/_bone-ramenoA/_bone-ramenoB/_ramenoB/_radlice", "SmoothHalf", "SmoothFull", "RoughHalf", "RoughFull")
                .SetAnimationNames("Idle", "PrepareMine", "Mine", "PrepareDump", "Dump")
                .SetIsTruckSupportedFunc((TruckProto truckProto) => (!truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType) && truckProto.Id != Ids.Vehicles.TruckT1.Id)
                .BuildAndAdd();
        }
    }
}