using Mafi;
using Mafi.Base;
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
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private const int ceilMin = 2;
        private readonly Dictionary<string, int> TruckCapacity = 
            new Dictionary<string, int>() {
            { "T1", 100 },
            { "T2", 300 },
            { "T3", 900 },
        };
        private readonly Dictionary<string, int> ExcavCapacity = 
            new Dictionary<string, int>() {
            { "T1", 25 },
            { "T2", 75 },
            { "T3", 225 },
        };
        private readonly Dictionary<string, double[]> TruckDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 2.0, 1.2, 50, 0.06, 0.09, 60, 20, 2.5, 1.25, 1.25 }},
            { "T2", new double[] { 2.5, 1.4, 50, 0.06, 0.09, 60, 20, 2.5, 1.5, 1.5 }},
            { "T3", new double[] { 1.5, 1.0, 50, 0.02, 0.06, 60, 15, 3.0, 1.5, 1.5 }},
            { "T2H", new double[] { 3.0, 1.6, 50, 0.06, 0.09, 60, 20, 2.5, 1.5, 1.5 }},
            { "T3H", new double[] { 1.8, 1.2, 50, 0.02, 0.06, 60, 15, 3.0, 1.5, 1.5 }},
        };
        private readonly Dictionary<string, double[]> ExcavDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.2, 0.8, 50, 0.04, 0.06, 10, 2, 2, 0, 0 }},
            { "T2", new double[] { 0.8, 0.6, 50, 0.025, 0.05, 8, 1, 2.5, 0, 0 }},
            { "T3", new double[] { 0.5, 0.4, 40, 0.015, 0.03, 4.0, 0.4, 3, 0, 0 }},
            { "T2H", new double[] { 1.0, 0.7, 50, 0.025, 0.05, 8, 1, 2.5, 0, 0 }},
            { "T3H", new double[] { 0.7, 0.5, 40, 0.015, 0.03, 4.0, 0.4, 3, 0, 0 }},
        };
        private readonly Dictionary<string, double[]> TrHarvDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 }},
            { "T2", new double[] { 1.5, 1.0, 50, 0.05, 0.06, 10, 2, 2, 0, 0 }},
            { "T2H", new double[] { 1.8, 1.2, 50, 0.05, 0.06, 10, 2, 2, 0, 0 }},
        };
        private readonly Dictionary<string, double[]> TrPlantDriveData =
            new Dictionary<string, double[]>() {
            { "T1", new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 }},
            { "T1H", new double[] { 1.2, 0.8, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 }},
        };

        // Localization Definitions
        private readonly Dictionary<string, string[]> TruckLocStrings = 
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Heavy duty pickup truck with max capacity of {0}. It can go under transports that are at height {1} or higher.", "truck description, for instance {0}=20,{1}=2", "100" }},
            { "T2", new string[] { "Large industrial truck with max capacity of {0}. It can go under transports if they are at height {1} or higher.", "vehicle description, for instance {0}=20,{1}=2", "300" }},
            { "T3F", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only liquid or gas products. It cannot go under transports.", "vehicle description, for instance {0}=150", "900" }},
            { "T3L", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only loose products (coal for instance). It cannot go under transports.", "vehicle description, for instance {0}=150", "900" }},
        };
        private readonly Dictionary<string, string[]> ExcavLocStrings =
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Suitable for mining any terrain with max bucket capacity of {0}. It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=6", "25" }},
            { "T2", new string[] { "This is a serious mining machine with max bucket capacity of {0}! It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=18", "75" }},
            { "T3", new string[] { "Extremely large excavator that can mine any terrain with ease. It has bucket capacity of {0}. It cannot go under transports due to its size, use ramps to cross them.", "vehicle description, for instance {0}=60", "225" }},
        };

        // Reference Helpers
        private TruckProto TrRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TruckRef(pReg, refID);
        private ExcavatorProto ExRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.ExcavRef(pReg, refID);
        private TreeHarvesterProto THrRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TrHarvRef(pReg, refID);
        private TreePlanterProto TPlRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TrPlantRef(pReg, refID);

        // Reflection Helpers
        /// <remarks>
        /// Modifies transportation capacity of a <b>TruckProto</b>. Requires <b>int</b> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference <b>TruckProto</b> for modification:<br/>
        /// <c>TruckProto refTruck = registrator.PrototypesDb.Get&lt;TruckProto&gt;(Ids.Vehicles.TruckT1.Id).Value;</c>
        /// 
        /// <br/><br/>Define new truck capacity as <b>int</b> variable:<br/>
        /// <c>int newTruckCapacity = 100;</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced <b>TruckProto</b>:<br/>
        /// <c>SetVehicleCapacity(refTruck, newTruckCapacity);</c>
        /// </remarks>
        public void SetVehicleCapacity(TruckProto refTruck, int newTruckCap) {
            if (refTruck == null) { ModLog.Warning($"SetVehicleCapacity: 'refTruck' is undefined!"); return; }
            ModLog.Info($"{refTruck.Id} Capacity: {refTruck.CapacityBase} -> {newTruckCap}");
            FieldInfo fieldCapBase = typeof(TruckProto).GetField("CapacityBase", BindingFlags.Instance | BindingFlags.Public);
            fieldCapBase.SetValue(refTruck, new Quantity(newTruckCap));
            FFU_IC_IDs.SyncProtoMod(refTruck);
        }
        /// <remarks>
        /// Modifies 'shovel' capacity of a <b>ExcavatorProto</b>. Requires <b>int</b> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference <b>ExcavatorProto</b> for modification:<br/>
        /// <c>ExcavatorProto refExcavator = registrator.PrototypesDb.Get&lt;ExcavatorProto&gt;(Ids.Vehicles.ExcavatorT1).Value;</c>
        /// 
        /// <br/><br/>Define new excavator capacity as <b>int</b> variable:<br/>
        /// <c>int newExcavatorCapacity = 25;</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced <b>ExcavatorProto</b>:<br/>
        /// <c>SetVehicleCapacity(refExcavator, newExcavatorCapacity);</c>
        /// </remarks>
        public void SetVehicleCapacity(ExcavatorProto refExcav, int newShovelCap) {
            if (refExcav == null) { ModLog.Warning($"SetVehicleCapacity: 'refExcav' is undefined!"); return; }
            ModLog.Info($"{refExcav.Id} Capacity: {refExcav.Capacity} -> {newShovelCap}");
            FieldInfo fieldCapBase = typeof(ExcavatorProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
            fieldCapBase.SetValue(refExcav, new Quantity(newShovelCap));
            FFU_IC_IDs.SyncProtoMod(refExcav);
        }
        /// <remarks>
        /// Modifies drive parameters of any <b>DrivingEntityProto</b> (i.e. <i>TruckProto</i>, <i>ExcavatorProto</i>, 
        /// <i>TreeHarvesterProto</i> or <i>TreePlanterProto</i>). 
        /// 
        /// <br/><br/>The <b>double[]</b> array requires these parameters: 
        /// <br/><b>maxForwardsSpeed</b>, <b>maxBackwardsSpeed</b>, <b>steeringSpeedMult</b>, <b>acceleration</b>, 
        /// <br/><b>braking</b>, <b>maxSteeringAngle</b>, <b>maxSteeringSpeed</b>, <b>breakingConservativness</b>, 
        /// <br/><b>steeringAxleOffset</b> and <b>nonSteeringAxleOffset</b>. 
        /// For reference just use the original values.<br/><br/>
        /// 
        /// <b>Note:</b> using negative values (e.g. -1) in <c>double</c> array will result in values being taken from original driving data.<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference <b>DrivingEntityProto</b> for modification (using relevant prototype):<br/>
        /// <c>TruckProto refVehicle = registrator.PrototypesDb.Get&lt;TruckProto&gt;(Ids.Vehicles.TruckT1.Id).Value;</c><br/>
        /// <c>ExcavatorProto refVehicle = registrator.PrototypesDb.Get&lt;ExcavatorProto&gt;(Ids.Vehicles.ExcavatorT1).Value;</c><br/>
        /// <c>TreeHarvesterProto refVehicle = registrator.PrototypesDb.Get&lt;TreeHarvesterProto&gt;(Ids.Vehicles.TreeHarvesterT1).Value;</c><br/>
        /// <c>TreePlanterProto refVehicle = registrator.PrototypesDb.Get&lt;TreePlanterProto&gt;(Ids.Vehicles.TreePlanterT1).Value;</c>
        /// 
        /// <br/><br/>Define new vehicle driving parameters as <b>double[]</b> array:<br/>
        /// <c>double[] vehicleDriveData = new double[] { 2.0, 1.2, 50, 0.06, 0.09, 60, 20, 2.5, 1.25, 1.25 };</c>
        /// 
        /// <br/><br/>Apply new vehicle driving parameters to the referenced <b>DrivingEntityProto</b>:<br/>
        /// <c>SetVehicleDriveData(refVehicle, vehicleDriveData);</c>
        /// </remarks>
        public void SetVehicleDriveData(DrivingEntityProto refVehicle, double[] driveData) {
            if (refVehicle == null) { ModLog.Warning($"SetVehicleDriveData: 'refVehicle' is undefined!"); return; }
            if (driveData == null) { ModLog.Warning($"SetVehicleDriveData: 'driveData' is undefined!"); return; }
            if (driveData.Length != 10) { ModLog.Warning($"SetVehicleDriveData: 'driveData' count is incorrect!"); return; }
            ModLog.Info($"{refVehicle.Id}. " +
                $"FwSpeed: {refVehicle.DrivingData.MaxForwardsSpeed} -> {(driveData[0] >= 0 ? driveData[0] : "_")}, " +
                $"BkSpeed: {refVehicle.DrivingData.MaxBackwardsSpeed} -> {(driveData[1] >= 0 ? driveData[1] : "_")}, " +
                $"StrMult: {refVehicle.DrivingData.SteeringSpeedMult} -> {(driveData[2] >= 0 ? driveData[2] : "_")}, " +
                $"Accel: {refVehicle.DrivingData.Acceleration} -> {(driveData[3] >= 0 ? driveData[3] : "_")}, " +
                $"Brake: {refVehicle.DrivingData.Braking} -> {(driveData[4] >= 0 ? driveData[4] : "_")}, " +
                $"MxStrAng: {refVehicle.DrivingData.MaxSteeringAngle} -> {(driveData[5] >= 0 ? driveData[5] : "_")}, " +
                $"MxStrSpd: {refVehicle.DrivingData.MaxSteeringSpeed} -> {(driveData[6] >= 0 ? driveData[6] : "_")}, " +
                $"BrkCons: {refVehicle.DrivingData.BrakingConservativness} -> {(driveData[7] >= 0 ? driveData[7] : "_")}, " +
                $"StrAxle: {refVehicle.DrivingData.SteeringAxleOffset} -> {(driveData[8] >= 0 ? driveData[8] : "_")}, " +
                $"NoStrAxle: {refVehicle.DrivingData.NonSteeringAxleOffset} -> {(driveData[9] >= 0 ? driveData[9] : "_")}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            DrivingData vehicleDriveData = new DrivingData(
                driveData[0] >= 0 ? driveData[0].Tiles() : refVehicle.DrivingData.MaxForwardsSpeed,
                driveData[1] >= 0 ? driveData[1].Tiles() : refVehicle.DrivingData.MaxBackwardsSpeed,
                driveData[2] >= 0 ? driveData[2].Percent() : refVehicle.DrivingData.SteeringSpeedMult,
                driveData[3] >= 0 ? driveData[3].Tiles() : refVehicle.DrivingData.Acceleration,
                driveData[4] >= 0 ? driveData[4].Tiles() : refVehicle.DrivingData.Braking,
                driveData[5] >= 0 ? driveData[5].Degrees() : refVehicle.DrivingData.MaxSteeringAngle,
                driveData[6] >= 0 ? driveData[6].Degrees() : refVehicle.DrivingData.MaxSteeringSpeed,
                driveData[7] >= 0 ? driveData[7].ToFix32() : refVehicle.DrivingData.BrakingConservativness,
                driveData[8] >= 0 ? (driveData[8] == 0 ? RelTile1f.Zero : driveData[8].Tiles()) : refVehicle.DrivingData.SteeringAxleOffset,
                driveData[9] >= 0 ? (driveData[9] == 0 ? RelTile1f.Zero : driveData[9].Tiles()) : refVehicle.DrivingData.NonSteeringAxleOffset);
            fieldDriveData.SetValue(refVehicle, vehicleDriveData);
            FFU_IC_IDs.SyncProtoMod(refVehicle);
        }
        /// <remarks>
        /// Modifies description of a <b>DrivingEntityProto</b> (i.e. <i>TruckProto</i> or <i>ExcavatorProto</i>). For reference just use the original description values.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference <b>DrivingEntityProto</b> for modification (using relevant prototype):<br/>
        /// <c>TruckProto refVehicle = registrator.PrototypesDb.Get&lt;TruckProto&gt;(Ids.Vehicles.TruckT1.Id).Value;</c><br/>
        /// <c>ExcavatorProto refVehicle = registrator.PrototypesDb.Get&lt;ExcavatorProto&gt;(Ids.Vehicles.ExcavatorT1).Value;</c>
        /// 
        /// <br/><br/>Define new vehicle localization as <b>string[]</b> array (using relevant strings):<br/>
        /// <c>string[] vehicleLocString = new string[] { "Truck with capacity of {0}. Will go under height {1} or higher.", "truck description" };</c><br/>
        /// <c>string[] vehicleLocString = new string[] { "Excavator with capacity of {0}. Will not go under belts/pipes.", "excavator description" };</c>
        /// 
        /// <br/><br/>Apply new description strings to the referenced <b>DrivingEntityProto</b>:<br/>
        /// <c>SetVehicleDescription(refVehicle, vehicleLocString);</c>
        /// </remarks>
        public void SetVehicleDescription(DrivingEntityProto refVehicle, string[] strSet, bool canGoUnder = false) {
            if (refVehicle == null) { ModLog.Warning($"SetVehicleDescription: 'refVehicle' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetVehicleDescription: 'strSet' is undefined!"); return; }
            if (strSet.Length != 3) { ModLog.Warning($"SetVehicleDescription: 'strSet' count is incorrect!"); return; }
            LocStr locDesc;
            if (canGoUnder) {
                LocStr2 locStr = Loc.Str2(refVehicle.Id.Value + "__desc", strSet[0], strSet[1]);
                locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refVehicle.Id.Value + "_formatted", locStr.Format(strSet[2], ceilMin.ToString()).Value);
            } else {
                LocStr1 locStr = Loc.Str1(refVehicle.Id.Value + "__desc", strSet[0], strSet[1]);
                locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refVehicle.Id.Value + "_formatted", locStr.Format(strSet[2]).Value);
            }
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refVehicle.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refVehicle);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refVehicle, newStr);
                FFU_IC_IDs.SyncProtoMod(refVehicle);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
            LocalizationManager.IgnoreDuplicates();

            // Vehicle References - Diesel
            TruckProto refTruckT1 = TrRef(Ids.Vehicles.TruckT1.Id);
            TruckProto refTruckT2 = TrRef(Ids.Vehicles.TruckT2.Id);
            TruckProto refTruckT3F = TrRef(Ids.Vehicles.TruckT3Fluid.Id);
            TruckProto refTruckT3L = TrRef(Ids.Vehicles.TruckT3Loose.Id);
            ExcavatorProto refExcavT1 = ExRef(Ids.Vehicles.ExcavatorT1);
            ExcavatorProto refExcavT2 = ExRef(Ids.Vehicles.ExcavatorT2);
            ExcavatorProto refExcavT3 = ExRef(Ids.Vehicles.ExcavatorT3);
            TreeHarvesterProto refTrHarvT1 = THrRef(Ids.Vehicles.TreeHarvesterT1);
            TreeHarvesterProto refTrHarvT2 = THrRef(Ids.Vehicles.TreeHarvesterT2);
            TreePlanterProto refTrPlantT1 = TPlRef(Ids.Vehicles.TreePlanterT1);

            // Vehicle References - Hydrogen
            TruckProto refTruckT2H = TrRef(Ids.Vehicles.TruckT2H.Id);
            TruckProto refTruckT3FH = TrRef(Ids.Vehicles.TruckT3FluidH.Id);
            TruckProto refTruckT3LH = TrRef(Ids.Vehicles.TruckT3LooseH.Id);
            ExcavatorProto refExcavT2H = ExRef(Ids.Vehicles.ExcavatorT2H);
            ExcavatorProto refExcavT3H = ExRef(Ids.Vehicles.ExcavatorT3H);
            TreeHarvesterProto refTrHarvT2H = THrRef(Ids.Vehicles.TreeHarvesterT2H);
            TreePlanterProto refTrPlantT1H = TPlRef(Ids.Vehicles.TreePlanterT1H);

            // Truck Modifications - Capacity
            SetVehicleCapacity(refTruckT1, TruckCapacity["T1"]);
            SetVehicleCapacity(refTruckT2, TruckCapacity["T2"]);
            SetVehicleCapacity(refTruckT3F, TruckCapacity["T3"]);
            SetVehicleCapacity(refTruckT3L, TruckCapacity["T3"]);
            SetVehicleCapacity(refTruckT2H, TruckCapacity["T2"]);
            SetVehicleCapacity(refTruckT3FH, TruckCapacity["T3"]);
            SetVehicleCapacity(refTruckT3LH, TruckCapacity["T3"]);

            // Truck Modifications - Drive Data
            SetVehicleDriveData(refTruckT1, TruckDriveData["T1"]);
            SetVehicleDriveData(refTruckT2, TruckDriveData["T2"]);
            SetVehicleDriveData(refTruckT3F, TruckDriveData["T3"]);
            SetVehicleDriveData(refTruckT3L, TruckDriveData["T3"]);
            SetVehicleDriveData(refTruckT2H, TruckDriveData["T2H"]);
            SetVehicleDriveData(refTruckT3FH, TruckDriveData["T3H"]);
            SetVehicleDriveData(refTruckT3LH, TruckDriveData["T3H"]);

            // Truck Modifications - Localization
            SetVehicleDescription(refTruckT1, TruckLocStrings["T1"], true);
            SetVehicleDescription(refTruckT2, TruckLocStrings["T2"], true);
            SetVehicleDescription(refTruckT3F, TruckLocStrings["T3F"]);
            SetVehicleDescription(refTruckT3L, TruckLocStrings["T3L"]);
            SetVehicleDescription(refTruckT2H, TruckLocStrings["T2"], true);
            SetVehicleDescription(refTruckT3FH, TruckLocStrings["T3F"]);
            SetVehicleDescription(refTruckT3LH, TruckLocStrings["T3L"]);

            // Excavator Modifications - Capacity
            SetVehicleCapacity(refExcavT1, ExcavCapacity["T1"]);
            SetVehicleCapacity(refExcavT2, ExcavCapacity["T2"]);
            SetVehicleCapacity(refExcavT3, ExcavCapacity["T3"]);
            SetVehicleCapacity(refExcavT2H, ExcavCapacity["T2"]);
            SetVehicleCapacity(refExcavT3H, ExcavCapacity["T3"]);

            // Excavator Modifications - Drive Data
            SetVehicleDriveData(refExcavT1, ExcavDriveData["T1"]);
            SetVehicleDriveData(refExcavT2, ExcavDriveData["T2"]);
            SetVehicleDriveData(refExcavT3, ExcavDriveData["T3"]);
            SetVehicleDriveData(refExcavT2H, ExcavDriveData["T2H"]);
            SetVehicleDriveData(refExcavT3H, ExcavDriveData["T3H"]);

            // Excavator Modifications - Localization
            SetVehicleDescription(refExcavT1, ExcavLocStrings["T1"]);
            SetVehicleDescription(refExcavT2, ExcavLocStrings["T2"]);
            SetVehicleDescription(refExcavT3, ExcavLocStrings["T3"]);
            SetVehicleDescription(refExcavT2H, ExcavLocStrings["T2"]);
            SetVehicleDescription(refExcavT3H, ExcavLocStrings["T3"]);

            // Tree Harvester Modifications - Drive Data
            SetVehicleDriveData(refTrHarvT1, TrHarvDriveData["T1"]);
            SetVehicleDriveData(refTrHarvT2, TrHarvDriveData["T2"]);
            SetVehicleDriveData(refTrHarvT2H, TrHarvDriveData["T2H"]);

            // Tree Planter Modifications - Drive Data
            SetVehicleDriveData(refTrPlantT1, TrPlantDriveData["T1"]);
            SetVehicleDriveData(refTrPlantT1H, TrPlantDriveData["T1H"]);
        }
    }
}