using Mafi;
using Mafi.Base;
using Mafi.Base.Prototypes.Vehicles;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Mods;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Vehicles : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions
        public const int ceilMin = 2;
        Percent NoFuelMaxSpeedPerc = 40.Percent();
        public readonly Dictionary<string, int> TruckCapacity = 
            new Dictionary<string, int>() {
            { "T1", 100 },
            { "T2", 300 },
            { "T3", 900 },
        };
        public readonly Dictionary<string, int> ExcavCapacity = 
            new Dictionary<string, int>() {
            { "T1", 25 },
            { "T2", 75 },
            { "T3", 225 },
        };
        public readonly Dictionary<string, double[]> TruckDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 2.0, 1.2, 50, 0.06, 0.09, 60, 20, 2.5, 1.25, 1.25 }},
            { "T2", new double[] { 2.5, 1.4, 50, 0.06, 0.09, 60, 20, 2.5, 1.5, 1.5 }},
            { "T3", new double[] { 1.5, 1.0, 50, 0.02, 0.06, 60, 15, 3.0, 1.5, 1.5 }},
        };
        public readonly Dictionary<string, double[]> ExcavDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.2, 0.8, 50, 0.04, 0.06, 10, 2, 2 }},
            { "T2", new double[] { 0.8, 0.6, 50, 0.025, 0.05, 8, 1, 2.5 }},
            { "T3", new double[] { 0.5, 0.4, 40, 0.015, 0.03, 4.0, 0.4, 3 }},
        };
        public readonly Dictionary<string, double[]> TrHarvDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2 }},
            { "T2", new double[] { 1.5, 1.0, 50, 0.05, 0.06, 10, 2, 2 }},
        };
        public readonly Dictionary<string, double[]> TrPlantDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2 }},
        };

        // Localization Definitions
        public readonly Dictionary<string, string[]> TruckLocStrings = 
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Heavy duty pickup truck with max capacity of {0}. It can go under transports that are at height {1} or higher.", "truck description, for instance {0}=20,{1}=2" }},
            { "T2", new string[] { "Large industrial truck with max capacity of {0}. It can go under transports if they are at height {1} or higher.", "vehicle description, for instance {0}=20,{1}=2" }},
            { "T3A", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only loose products (coal for instance). It cannot go under transports.", "vehicle description, for instance {0}=150" }},
            { "T3B", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only liquid or gas products. It cannot go under transports.", "vehicle description, for instance {0}=150" }},
        };
        public readonly Dictionary<string, string[]> ExcavLocStrings =
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Suitable for mining any terrain with max bucket capacity of {0}. It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=6" }},
            { "T2", new string[] { "This is a serious mining machine with max bucket capacity of {0}! It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=18" }},
            { "T3", new string[] { "Extremely large excavator that can mine any terrain with ease. It has bucket capacity of {0}. It cannot go under transports due to its size, use ramps to cross them.", "vehicle description, for instance {0}=60" }},
        };

        // Reflection Helpers
        public TruckProto TrRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TruckRef(pReg, refID);
        public ExcavatorProto ExRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.ExcavRef(pReg, refID);
        public TreeHarvesterProto THrRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TrHarvRef(pReg, refID);
        public TreePlanterProto TPlRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TrPlantRef(pReg, refID);
        public void SetVehicleCapacity(TruckProto refTruck, int newTruckCap) {
            if (refTruck == null) { ModLog.Warning($"SetVehicleCapacity: 'refTruck' is undefined!"); return; }
            ModLog.Info($"{refTruck.Id} Capacity: {refTruck.CapacityBase} -> {newTruckCap}");
            FieldInfo fieldCapBase = typeof(TruckProto).GetField("CapacityBase", BindingFlags.Instance | BindingFlags.Public);
            fieldCapBase.SetValue(refTruck, new Quantity(newTruckCap));
            FFU_IC_IDs.SyncProtoMod(refTruck);
        }
        public void SetVehicleCapacity(ExcavatorProto refExcav, int newShovelCap) {
            if (refExcav == null) { ModLog.Warning($"SetVehicleCapacity: 'refExcav' is undefined!"); return; }
            ModLog.Info($"{refExcav.Id} Capacity: {refExcav.Capacity} -> {newShovelCap}");
            FieldInfo fieldCapBase = typeof(ExcavatorProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            fieldCapBase.SetValue(refExcav, new Quantity(newShovelCap));
            FFU_IC_IDs.SyncProtoMod(refExcav);
        }
        public void SetVehicleDriveData(TruckProto refTruck, double[] speedSet) {
            if (refTruck == null) { ModLog.Warning($"SetVehicleDriveData: 'refTruck' is undefined!"); return; }
            if (speedSet == null) { ModLog.Warning($"SetVehicleDriveData: 'speedSet' is undefined!"); return; }
            ModLog.Info($"{refTruck.Id} Speed F/B: {refTruck.DrivingData.MaxForwardsSpeed}/{refTruck.DrivingData.MaxBackwardsSpeed} -> {speedSet[0]}/{speedSet[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            fieldDriveData.SetValue(refTruck, new DrivingData(speedSet[0].Tiles(), speedSet[1].Tiles(), speedSet[2].Percent(), speedSet[3].Tiles(), speedSet[4].Tiles(), 
            speedSet[5].Degrees(), speedSet[6].Degrees(), speedSet[7].ToFix32(), speedSet[8].Tiles(), speedSet[9].Tiles(), NoFuelMaxSpeedPerc));
            FFU_IC_IDs.SyncProtoMod(refTruck);
        }
        public void SetVehicleDriveData(ExcavatorProto refExcav, double[] speedSet) {
            if (refExcav == null) { ModLog.Warning($"SetVehicleDriveData: 'refExcav' is undefined!"); return; }
            if (speedSet == null) { ModLog.Warning($"SetVehicleDriveData: 'speedSet' is undefined!"); return; }
            ModLog.Info($"{refExcav.Id} Speed F/B: {refExcav.DrivingData.MaxForwardsSpeed}/{refExcav.DrivingData.MaxBackwardsSpeed} -> {speedSet[0]}/{speedSet[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            fieldDriveData.SetValue(refExcav, new DrivingData(speedSet[0].Tiles(), speedSet[1].Tiles(), speedSet[2].Percent(), speedSet[3].Tiles(), speedSet[4].Tiles(),
            speedSet[5].Degrees(), speedSet[6].Degrees(), speedSet[7].ToFix32(), RelTile1f.Zero, RelTile1f.Zero, NoFuelMaxSpeedPerc));
            FFU_IC_IDs.SyncProtoMod(refExcav);
        }
        public void SetVehicleDriveData(TreeHarvesterProto refTrHarv, double[] speedSet) {
            if (refTrHarv == null) { ModLog.Warning($"SetVehicleDriveData: 'refTrHarv' is undefined!"); return; }
            if (speedSet == null) { ModLog.Warning($"SetVehicleDriveData: 'speedSet' is undefined!"); return; }
            ModLog.Info($"{refTrHarv.Id} Speed F/B: {refTrHarv.DrivingData.MaxForwardsSpeed}/{refTrHarv.DrivingData.MaxBackwardsSpeed} -> {speedSet[0]}/{speedSet[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            fieldDriveData.SetValue(refTrHarv, new DrivingData(speedSet[0].Tiles(), speedSet[1].Tiles(), speedSet[2].Percent(), speedSet[3].Tiles(), speedSet[4].Tiles(),
            speedSet[5].Degrees(), speedSet[6].Degrees(), speedSet[7].ToFix32(), RelTile1f.Zero, RelTile1f.Zero, NoFuelMaxSpeedPerc));
            FFU_IC_IDs.SyncProtoMod(refTrHarv);
        }
        public void SetVehicleDriveData(TreePlanterProto refTrPlant, double[] speedSet) {
            if (refTrPlant == null) { ModLog.Warning($"SetVehicleDriveData: 'refTrPlant' is undefined!"); return; }
            if (speedSet == null) { ModLog.Warning($"SetVehicleDriveData: 'speedSet' is undefined!"); return; }
            ModLog.Info($"{refTrPlant.Id} Speed F/B: {refTrPlant.DrivingData.MaxForwardsSpeed}/{refTrPlant.DrivingData.MaxBackwardsSpeed} -> {speedSet[0]}/{speedSet[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            fieldDriveData.SetValue(refTrPlant, new DrivingData(speedSet[0].Tiles(), speedSet[1].Tiles(), speedSet[2].Percent(), speedSet[3].Tiles(), speedSet[4].Tiles(),
            speedSet[5].Degrees(), speedSet[6].Degrees(), speedSet[7].ToFix32(), RelTile1f.Zero, RelTile1f.Zero, NoFuelMaxSpeedPerc));
            FFU_IC_IDs.SyncProtoMod(refTrPlant);
        }
        public void SetVehicleDescription(TruckProto refTruck, string[] strSet, bool canGoUnder) {
            if (refTruck == null) { ModLog.Warning($"SetVehicleDescription: 'refTruck' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetVehicleDescription: 'strSet' is undefined!"); return; }
            LocStr locDesc;
            if (canGoUnder) {
                LocStr2 locStr = Loc.Str2(refTruck.Id.Value + "__desc", strSet[0], strSet[1]);
                locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refTruck.Id.Value + "_formatted", locStr.Format(refTruck.CapacityBase.ToString(), ceilMin.ToString()).Value);
            } else {
                LocStr1 locStr = Loc.Str1(refTruck.Id.Value + "__desc", strSet[0], strSet[1]);
                locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refTruck.Id.Value + "_formatted", locStr.Format(refTruck.CapacityBase.ToString()).Value);
            }
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refTruck.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refTruck);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refTruck, newStr);
                FFU_IC_IDs.SyncProtoMod(refTruck);
            }
        }
        public void SetVehicleDescription(ExcavatorProto refExcav, string[] strSet) {
            if (refExcav == null) { ModLog.Warning($"SetVehicleDescription: 'refExcav' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetVehicleDescription: 'strSet' is undefined!"); return; }
            LocStr1 locStr = Loc.Str1(refExcav.Id.Value + "__desc", strSet[0], strSet[1]);
            LocStr newDesc = LocalizationManager.CreateAlreadyLocalizedStr(refExcav.Id.Value + "_formatted", locStr.Format(refExcav.Capacity.ToString()).Value);
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refExcav.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refExcav);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, newDesc);
                fieldStrings.SetValue(refExcav, newStr);
                FFU_IC_IDs.SyncProtoMod(refExcav);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // Vehicle References
            TruckProto refTruckT1 = TrRef(Ids.Vehicles.TruckT1.Id);
            TruckProto refTruckT2 = TrRef(Ids.Vehicles.TruckT2.Id);
            TruckProto refTruckT3A = TrRef(Ids.Vehicles.TruckT3Loose.Id);
            TruckProto refTruckT3B = TrRef(Ids.Vehicles.TruckT3Fluid.Id);
            ExcavatorProto refExcavT1 = ExRef(Ids.Vehicles.ExcavatorT1);
            ExcavatorProto refExcavT2 = ExRef(Ids.Vehicles.ExcavatorT2);
            ExcavatorProto refExcavT3 = ExRef(Ids.Vehicles.ExcavatorT3);
            TreeHarvesterProto refTrHarvT1 = THrRef(Ids.Vehicles.TreeHarvesterT1);
            TreeHarvesterProto refTrHarvT2 = THrRef(Ids.Vehicles.TreeHarvesterT2);
            TreePlanterProto refTrPlantT1 = TPlRef(Ids.Vehicles.TreePlanterT1);

            // Truck Modifications
            SetVehicleCapacity(refTruckT1, TruckCapacity["T1"]);
            SetVehicleCapacity(refTruckT2, TruckCapacity["T2"]);
            SetVehicleCapacity(refTruckT3A, TruckCapacity["T3"]);
            SetVehicleCapacity(refTruckT3B, TruckCapacity["T3"]);
            SetVehicleDriveData(refTruckT1, TruckDriveData["T1"]);
            SetVehicleDriveData(refTruckT2, TruckDriveData["T2"]);
            SetVehicleDriveData(refTruckT3A, TruckDriveData["T3"]);
            SetVehicleDriveData(refTruckT3B, TruckDriveData["T3"]);
            SetVehicleDescription(refTruckT1, TruckLocStrings["T1"], true);
            SetVehicleDescription(refTruckT2, TruckLocStrings["T2"], true);
            SetVehicleDescription(refTruckT3A, TruckLocStrings["T3A"], false);
            SetVehicleDescription(refTruckT3B, TruckLocStrings["T3B"], false);

            // Excavator Modifications
            SetVehicleCapacity(refExcavT1, ExcavCapacity["T1"]);
            SetVehicleCapacity(refExcavT2, ExcavCapacity["T2"]);
            SetVehicleCapacity(refExcavT3, ExcavCapacity["T3"]);
            SetVehicleDriveData(refExcavT1, ExcavDriveData["T1"]);
            SetVehicleDriveData(refExcavT2, ExcavDriveData["T2"]);
            SetVehicleDriveData(refExcavT3, ExcavDriveData["T3"]);
            SetVehicleDescription(refExcavT1, ExcavLocStrings["T1"]);
            SetVehicleDescription(refExcavT2, ExcavLocStrings["T2"]);
            SetVehicleDescription(refExcavT3, ExcavLocStrings["T3"]);

            // Tree Harvester Modifications
            SetVehicleDriveData(refTrHarvT1, TrHarvDriveData["T1"]);
            SetVehicleDriveData(refTrHarvT2, TrHarvDriveData["T2"]);

            // Tree Planter Modifications
            SetVehicleDriveData(refTrPlantT1, TrPlantDriveData["T1"]);
        }
    }
}