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
            { "T1", new string[] { "Heavy duty pickup truck with max capacity of {0}. It can go under transports that are at height {1} or higher.", "truck description, for instance {0}=20,{1}=2" }},
            { "T2", new string[] { "Large industrial truck with max capacity of {0}. It can go under transports if they are at height {1} or higher.", "vehicle description, for instance {0}=20,{1}=2" }},
            { "T3F", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only liquid or gas products. It cannot go under transports.", "vehicle description, for instance {0}=150" }},
            { "T3L", new string[] { "Large hauling truck with max capacity of {0}. This type can transport only loose products (coal for instance). It cannot go under transports.", "vehicle description, for instance {0}=150" }},
        };
        private readonly Dictionary<string, string[]> ExcavLocStrings =
            new Dictionary<string, string[]>() {
            { "T1", new string[] { "Suitable for mining any terrain with max bucket capacity of {0}. It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=6" }},
            { "T2", new string[] { "This is a serious mining machine with max bucket capacity of {0}! It is too tall and it cannot go under transports, use ramps to cross them.", "vehicle description, for instance {0}=18" }},
            { "T3", new string[] { "Extremely large excavator that can mine any terrain with ease. It has bucket capacity of {0}. It cannot go under transports due to its size, use ramps to cross them.", "vehicle description, for instance {0}=60" }},
        };

        // Reference Helpers
        private TruckProto TrRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TruckRef(pReg, refID);
        private ExcavatorProto ExRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.ExcavRef(pReg, refID);
        private TreeHarvesterProto THrRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TrHarvRef(pReg, refID);
        private TreePlanterProto TPlRef(DynamicEntityProto.ID refID) => FFU_IC_IDs.TrPlantRef(pReg, refID);

        // Reflection Helpers
        /// <remarks>
        /// Modifies transportation capacity of trucks. Requires <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target truck for modification:<br/>
        /// <c>TruckProto refTruck = registrator.PrototypesDb.Get&lt;TruckProto&gt;(Ids.Vehicles.TruckT1.Id).Value;</c>
        /// 
        /// <br/><br/>Define new truck capacity as 'int' variable:<br/>
        /// <c>int newTruckCapacity = 100;</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced truck:<br/>
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
        /// Modifies 'shovel' capacity of excavators. Requires <c>integer</c> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target excavator for modification:<br/>
        /// <c>ExcavatorProto refExcavator = registrator.PrototypesDb.Get&lt;ExcavatorProto&gt;(Ids.Vehicles.ExcavatorT1).Value;</c>
        /// 
        /// <br/><br/>Define new excavator capacity as 'int' variable:<br/>
        /// <c>int newExcavatorCapacity = 25;</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced excavator:<br/>
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
        /// Modifies drive and motion parameters of trucks. The <c>double</c> array requires these parameters: <b>maxForwardsSpeed</b>, 
        /// <b>maxBackwardsSpeed</b>, <b>steeringSpeedMult</b>, <b>acceleration</b>, <b>braking</b>, <b>maxSteeringAngle</b>, 
        /// <b>maxSteeringSpeed</b>, <b>breakingConservativness</b>, <b>steeringAxleOffset</b> and <b>nonSteeringAxleOffset</b>. 
        /// For reference just use original values.<br/><br/>
        /// 
        /// <b>Note:</b> using negative values (e.g. -1) in <c>double</c> array will result in values being taken from original driving data.<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target truck for modification:<br/>
        /// <c>TruckProto refTruck = registrator.PrototypesDb.Get&lt;TruckProto&gt;(Ids.Vehicles.TruckT1.Id).Value;</c>
        /// 
        /// <br/><br/>Define new truck driving parameters as 'double' array:<br/>
        /// <c>double[] truckDriveData = new double[] { 2.0, 1.2, 50, 0.06, 0.09, 60, 20, 2.5, 1.25, 1.25 };</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced truck:<br/>
        /// <c>SetVehicleDriveData(refTruck, truckDriveData);</c>
        /// </remarks>
        public void SetVehicleDriveData(TruckProto refTruck, double[] driveData) {
            if (refTruck == null) { ModLog.Warning($"SetVehicleDriveData: 'refTruck' is undefined!"); return; }
            if (driveData == null) { ModLog.Warning($"SetVehicleDriveData: 'driveData' is undefined!"); return; }
            if (driveData.Length != 10) { ModLog.Warning($"SetVehicleDriveData: 'driveData' count is incorrect!"); return; }
            ModLog.Info($"{refTruck.Id} Speed F/B: {refTruck.DrivingData.MaxForwardsSpeed}/{refTruck.DrivingData.MaxBackwardsSpeed} -> {driveData[0]}/{driveData[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            DrivingData truckDriveData = new DrivingData(
                driveData[0] >= 0 ? driveData[0].Tiles() : refTruck.DrivingData.MaxForwardsSpeed,
                driveData[1] >= 0 ? driveData[1].Tiles() : refTruck.DrivingData.MaxBackwardsSpeed,
                driveData[2] >= 0 ? driveData[2].Percent() : refTruck.DrivingData.SteeringSpeedMult,
                driveData[3] >= 0 ? driveData[3].Tiles() : refTruck.DrivingData.Acceleration,
                driveData[4] >= 0 ? driveData[4].Tiles() : refTruck.DrivingData.Braking,
                driveData[5] >= 0 ? driveData[5].Degrees() : refTruck.DrivingData.MaxSteeringAngle,
                driveData[6] >= 0 ? driveData[6].Degrees() : refTruck.DrivingData.MaxSteeringSpeed,
                driveData[7] >= 0 ? driveData[7].ToFix32() : refTruck.DrivingData.BrakingConservativness,
                driveData[8] >= 0 ? (driveData[8] == 0 ? RelTile1f.Zero : driveData[8].Tiles()) : refTruck.DrivingData.SteeringAxleOffset,
                driveData[9] >= 0 ? (driveData[9] == 0 ? RelTile1f.Zero : driveData[9].Tiles()) : refTruck.DrivingData.NonSteeringAxleOffset);
            fieldDriveData.SetValue(refTruck, truckDriveData);
            FFU_IC_IDs.SyncProtoMod(refTruck);
        }
        /// <remarks>
        /// Modifies drive and motion parameters of excavators. The <c>double</c> array requires these parameters: <b>maxForwardsSpeed</b>, 
        /// <b>maxBackwardsSpeed</b>, <b>steeringSpeedMult</b>, <b>acceleration</b>, <b>breaking</b>, <b>maxSteeringAngle</b>, 
        /// <b>maxSteeringSpeed</b>, <b>breakingConservativness</b>, <b>steeringAxleOffset</b> and <b>nonSteeringAxleOffset</b>. 
        /// For reference just use original values.<br/><br/>
        /// 
        /// <b>Note:</b> using negative values (e.g. -1) in <c>double</c> array will result in values being taken from original driving data.<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target excavator for modification:<br/>
        /// <c>ExcavatorProto refExcavator = registrator.PrototypesDb.Get&lt;ExcavatorProto&gt;(Ids.Vehicles.ExcavatorT1).Value;</c>
        /// 
        /// <br/><br/>Define new excavator driving parameters as 'double' array:<br/>
        /// <c>double[] excavatorDriveData = new double[] { 1.2, 0.8, 50, 0.04, 0.06, 10, 2, 2, 0, 0 };</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced excavator:<br/>
        /// <c>SetVehicleDriveData(refExcavator, excavatorDriveData);</c>
        /// </remarks>
        public void SetVehicleDriveData(ExcavatorProto refExcav, double[] driveData) {
            if (refExcav == null) { ModLog.Warning($"SetVehicleDriveData: 'refExcav' is undefined!"); return; }
            if (driveData == null) { ModLog.Warning($"SetVehicleDriveData: 'driveData' is undefined!"); return; }
            if (driveData.Length != 10) { ModLog.Warning($"SetVehicleDriveData: 'driveData' count is incorrect!"); return; }
            ModLog.Info($"{refExcav.Id} Speed F/B: {refExcav.DrivingData.MaxForwardsSpeed}/{refExcav.DrivingData.MaxBackwardsSpeed} -> {driveData[0]}/{driveData[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            DrivingData excavDriveData = new DrivingData(
                driveData[0] >= 0 ? driveData[0].Tiles() : refExcav.DrivingData.MaxForwardsSpeed,
                driveData[1] >= 0 ? driveData[1].Tiles() : refExcav.DrivingData.MaxBackwardsSpeed,
                driveData[2] >= 0 ? driveData[2].Percent() : refExcav.DrivingData.SteeringSpeedMult,
                driveData[3] >= 0 ? driveData[3].Tiles() : refExcav.DrivingData.Acceleration,
                driveData[4] >= 0 ? driveData[4].Tiles() : refExcav.DrivingData.Braking,
                driveData[5] >= 0 ? driveData[5].Degrees() : refExcav.DrivingData.MaxSteeringAngle,
                driveData[6] >= 0 ? driveData[6].Degrees() : refExcav.DrivingData.MaxSteeringSpeed,
                driveData[7] >= 0 ? driveData[7].ToFix32() : refExcav.DrivingData.BrakingConservativness,
                driveData[8] >= 0 ? (driveData[8] == 0 ? RelTile1f.Zero : driveData[8].Tiles()) : refExcav.DrivingData.SteeringAxleOffset,
                driveData[9] >= 0 ? (driveData[9] == 0 ? RelTile1f.Zero : driveData[9].Tiles()) : refExcav.DrivingData.NonSteeringAxleOffset);
            fieldDriveData.SetValue(refExcav, excavDriveData);
            FFU_IC_IDs.SyncProtoMod(refExcav);
        }
        /// <remarks>
        /// Modifies drive and motion parameters of tree harvesters. The <c>double</c> array requires these parameters: <b>maxForwardsSpeed</b>, 
        /// <b>maxBackwardsSpeed</b>, <b>steeringSpeedMult</b>, <b>acceleration</b>, <b>breaking</b>, <b>maxSteeringAngle</b>, 
        /// <b>maxSteeringSpeed</b>, <b>breakingConservativness</b>, <b>steeringAxleOffset</b> and <b>nonSteeringAxleOffset</b>. 
        /// For reference just use original values.<br/><br/>
        /// 
        /// <b>Note:</b> using negative values (e.g. -1) in <c>double</c> array will result in values being taken from original driving data.<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target tree harvester for modification:<br/>
        /// <c>TreeHarvesterProto refHarvester = registrator.PrototypesDb.Get&lt;TreeHarvesterProto&gt;(Ids.Vehicles.TreeHarvesterT1).Value;</c>
        /// 
        /// <br/><br/>Define new tree harvester driving parameters as 'double' array:<br/>
        /// <c>double[] harvesterDriveData = new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 };</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced tree harvester:<br/>
        /// <c>SetVehicleDriveData(refHarvester, harvesterDriveData);</c>
        /// </remarks>
        public void SetVehicleDriveData(TreeHarvesterProto refTrHarv, double[] driveData) {
            if (refTrHarv == null) { ModLog.Warning($"SetVehicleDriveData: 'refTrHarv' is undefined!"); return; }
            if (driveData == null) { ModLog.Warning($"SetVehicleDriveData: 'driveData' is undefined!"); return; }
            if (driveData.Length != 10) { ModLog.Warning($"SetVehicleDriveData: 'driveData' count is incorrect!"); return; }
            ModLog.Info($"{refTrHarv.Id} Speed F/B: {refTrHarv.DrivingData.MaxForwardsSpeed}/{refTrHarv.DrivingData.MaxBackwardsSpeed} -> {driveData[0]}/{driveData[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            DrivingData harvDriveData = new DrivingData(
                driveData[0] >= 0 ? driveData[0].Tiles() : refTrHarv.DrivingData.MaxForwardsSpeed,
                driveData[1] >= 0 ? driveData[1].Tiles() : refTrHarv.DrivingData.MaxBackwardsSpeed,
                driveData[2] >= 0 ? driveData[2].Percent() : refTrHarv.DrivingData.SteeringSpeedMult,
                driveData[3] >= 0 ? driveData[3].Tiles() : refTrHarv.DrivingData.Acceleration,
                driveData[4] >= 0 ? driveData[4].Tiles() : refTrHarv.DrivingData.Braking,
                driveData[5] >= 0 ? driveData[5].Degrees() : refTrHarv.DrivingData.MaxSteeringAngle,
                driveData[6] >= 0 ? driveData[6].Degrees() : refTrHarv.DrivingData.MaxSteeringSpeed,
                driveData[7] >= 0 ? driveData[7].ToFix32() : refTrHarv.DrivingData.BrakingConservativness,
                driveData[8] >= 0 ? (driveData[8] == 0 ? RelTile1f.Zero : driveData[8].Tiles()) : refTrHarv.DrivingData.SteeringAxleOffset,
                driveData[9] >= 0 ? (driveData[9] == 0 ? RelTile1f.Zero : driveData[9].Tiles()) : refTrHarv.DrivingData.NonSteeringAxleOffset);
            fieldDriveData.SetValue(refTrHarv, harvDriveData);
            FFU_IC_IDs.SyncProtoMod(refTrHarv);
        }
        /// <remarks>
        /// Modifies drive and motion parameters of tree planters. The <c>double</c> array requires these parameters: <b>maxForwardsSpeed</b>, 
        /// <b>maxBackwardsSpeed</b>, <b>steeringSpeedMult</b>, <b>acceleration</b>, <b>breaking</b>, <b>maxSteeringAngle</b>, 
        /// <b>maxSteeringSpeed</b>, <b>breakingConservativness</b>, <b>steeringAxleOffset</b> and <b>nonSteeringAxleOffset</b>. 
        /// For reference just use original values.<br/><br/>
        /// 
        /// <b>Note:</b> using negative values (e.g. -1) in <c>double</c> array will result in values being taken from original driving data.<br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target tree planter for modification:<br/>
        /// <c>TreePlanterProto refPlanter = registrator.PrototypesDb.Get&lt;TreePlanterProto&gt;(Ids.Vehicles.TreePlanterT1).Value;</c>
        /// 
        /// <br/><br/>Define new tree planter driving parameters as 'double' array:<br/>
        /// <c>double[] planterDriveData = new double[] { 1.0, 0.7, 50, 0.04, 0.06, 8, 1.5, 2, 0, 0 };</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced tree planter:<br/>
        /// <c>SetVehicleDriveData(refPlanter, planterDriveData);</c>
        /// </remarks>
        public void SetVehicleDriveData(TreePlanterProto refTrPlant, double[] driveData) {
            if (refTrPlant == null) { ModLog.Warning($"SetVehicleDriveData: 'refTrPlant' is undefined!"); return; }
            if (driveData == null) { ModLog.Warning($"SetVehicleDriveData: 'driveData' is undefined!"); return; }
            if (driveData.Length != 10) { ModLog.Warning($"SetVehicleDriveData: 'driveData' count is incorrect!"); return; }
            ModLog.Info($"{refTrPlant.Id} Speed F/B: {refTrPlant.DrivingData.MaxForwardsSpeed}/{refTrPlant.DrivingData.MaxBackwardsSpeed} -> {driveData[0]}/{driveData[1]}");
            FieldInfo fieldDriveData = typeof(DrivingEntityProto).GetField("DrivingData", BindingFlags.Instance | BindingFlags.Public);
            DrivingData plantDriveData = new DrivingData(
                driveData[0] >= 0 ? driveData[0].Tiles() : refTrPlant.DrivingData.MaxForwardsSpeed,
                driveData[1] >= 0 ? driveData[1].Tiles() : refTrPlant.DrivingData.MaxBackwardsSpeed,
                driveData[2] >= 0 ? driveData[2].Percent() : refTrPlant.DrivingData.SteeringSpeedMult,
                driveData[3] >= 0 ? driveData[3].Tiles() : refTrPlant.DrivingData.Acceleration,
                driveData[4] >= 0 ? driveData[4].Tiles() : refTrPlant.DrivingData.Braking,
                driveData[5] >= 0 ? driveData[5].Degrees() : refTrPlant.DrivingData.MaxSteeringAngle,
                driveData[6] >= 0 ? driveData[6].Degrees() : refTrPlant.DrivingData.MaxSteeringSpeed,
                driveData[7] >= 0 ? driveData[7].ToFix32() : refTrPlant.DrivingData.BrakingConservativness,
                driveData[8] >= 0 ? (driveData[8] == 0 ? RelTile1f.Zero : driveData[8].Tiles()) : refTrPlant.DrivingData.SteeringAxleOffset,
                driveData[9] >= 0 ? (driveData[9] == 0 ? RelTile1f.Zero : driveData[9].Tiles()) : refTrPlant.DrivingData.NonSteeringAxleOffset);
            fieldDriveData.SetValue(refTrPlant, plantDriveData);
            FFU_IC_IDs.SyncProtoMod(refTrPlant);
        }
        /// <remarks>
        /// Modifies description of trucks. The <c>string</c> array requires default localization text parameters.
        /// For reference just use original values.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target truck for modification:<br/>
        /// <c>TruckProto refTruck = registrator.PrototypesDb.Get&lt;TruckProto&gt;(Ids.Vehicles.TruckT1.Id).Value;</c>
        /// 
        /// <br/><br/>Define new truck localization strings as 'string' array:<br/>
        /// <c>string[] truckLocString = new string[] { "Truck with capacity of {0}. Will go under height {1} or higher.", "truck description" };</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced truck:<br/>
        /// <c>SetVehicleDescription(refTruck, truckLocString);</c>
        /// </remarks>
        public void SetVehicleDescription(TruckProto refTruck, string[] strSet, bool canGoUnder) {
            if (refTruck == null) { ModLog.Warning($"SetVehicleDescription: 'refTruck' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetVehicleDescription: 'strSet' is undefined!"); return; }
            if (strSet.Length != 2) { ModLog.Warning($"SetVehicleDescription: 'strSet' count is incorrect!"); return; }
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
        /// <remarks>
        /// Modifies description of excavators. The <c>string</c> array requires default localization text parameters.
        /// For reference just use original values.<br/><br/>
        /// 
        /// <br/><u>Usage Example (in 'RegisterData' function)</u>
        /// 
        /// <br/><br/>Reference target excavator for modification:<br/>
        /// <c>ExcavatorProto refExcavator = registrator.PrototypesDb.Get&lt;ExcavatorProto&gt;(Ids.Vehicles.ExcavatorT1).Value;</c>
        /// 
        /// <br/><br/>Define new excavator localization strings as 'string' array:<br/>
        /// <c>string[] excavLocString = new string[] { "Excavator with capacity of {0}. Will no go under belts/pipes.", "excavator description" };</c>
        /// 
        /// <br/><br/>Apply new capacity value to the referenced excavator:<br/>
        /// <c>SetVehicleDescription(refExcavator, excavLocString);</c>
        /// </remarks>
        public void SetVehicleDescription(ExcavatorProto refExcav, string[] strSet) {
            if (refExcav == null) { ModLog.Warning($"SetVehicleDescription: 'refExcav' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetVehicleDescription: 'strSet' is undefined!"); return; }
            if (strSet.Length != 2) { ModLog.Warning($"SetVehicleDescription: 'strSet' count is incorrect!"); return; }
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
            SetVehicleDescription(refTruckT3F, TruckLocStrings["T3F"], false);
            SetVehicleDescription(refTruckT3L, TruckLocStrings["T3L"], false);
            SetVehicleDescription(refTruckT2H, TruckLocStrings["T2"], true);
            SetVehicleDescription(refTruckT3FH, TruckLocStrings["T3F"], false);
            SetVehicleDescription(refTruckT3LH, TruckLocStrings["T3L"], false);

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