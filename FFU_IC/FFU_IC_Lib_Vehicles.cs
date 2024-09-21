using Mafi;
using System.Reflection;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;

namespace FFU_Industrial_Capacity {
    public static partial class FFU_IC_Lib {
        /// <remarks>
        /// Modifies transportation/'shovel' capacity of a <b>TruckProto</b>/<b>ExcavatorProto</b>. Requires <b>int</b> value.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new capacity as <b>int</b> variable:<br/>
        /// <c>int newTruckCapacity = 100;</c><br/>
        /// <c>int newExcavatorCapacity = 25;</c>
        /// 
        /// <br/><br/>Apply new capacity value via <b>TruckProto</b>/<b>ExcavatorProto</b> identifier:<br/>
        /// <c>SetVehicleCapacity(Ids.Vehicles.TruckT1.Id, newTruckCapacity);</c><br/>
        /// <c>SetVehicleCapacity(Ids.Vehicles.ExcavatorT1, newExcavatorCapacity);</c>
        /// </remarks>
        public static void SetVehicleCapacity(DynamicEntityProto.ID refVehicleID, int newVehicleCap) {
            if (pReg == null) { ModLog.Warning($"SetVehicleCapacity: the ProtoRegistrator is not referenced!"); return; };
            TruckProto refTruck = TruckRef(refVehicleID);
            ExcavatorProto refExcav = ExcavRef(refVehicleID);
            if (refTruck == null && refExcav == null) { ModLog.Warning($"SetVehicleCapacity: can't find TruckProto or ExcavatorProto reference!"); return; }
            if (newVehicleCap <= 0) { ModLog.Warning($"SetVehicleCapacity: 'newVehicleCap' is invalid!"); return; }
            if (refTruck != null) {
                ModLog.Info($"{refTruck.Id} Capacity: {refTruck.CapacityBase} -> {newVehicleCap}");
                FieldInfo fieldCapBase = typeof(TruckProto).GetField("CapacityBase", BindingFlags.Instance | BindingFlags.Public);
                fieldCapBase.SetValue(refTruck, new Quantity(newVehicleCap));
                SyncProtoMod(refTruck);
            }
            if (refExcav != null) {
                ModLog.Info($"{refExcav.Id} Capacity: {refExcav.Capacity} -> {newVehicleCap}");
                FieldInfo fieldCapBase = typeof(ExcavatorProto).GetField("Capacity", BindingFlags.Instance | BindingFlags.Public);
                fieldCapBase.SetValue(refExcav, new Quantity(newVehicleCap));
                SyncProtoMod(refExcav);
            }
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
        /// <b>Note:</b> using negative values (e.g. -1) in <c>double[]</c> array will result in values being taken from original driving data.<br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Define new vehicle driving parameters as <b>double[]</b> array:<br/>
        /// <c>double[] vehicleDriveData = new double[] { 2.0, 1.2, 50, 0.06, 0.09, 60, 20, 2.5, 1.25, 1.25 };</c>
        /// 
        /// <br/><br/>Apply new vehicle driving parameters via <b>DrivingEntityProto</b> identifier:<br/>
        /// <c>SetVehicleDriveData(Ids.Vehicles.TruckT1.Id, vehicleDriveData);</c><br/>
        /// <c>SetVehicleDriveData(Ids.Vehicles.ExcavatorT1, vehicleDriveData);</c><br/>
        /// <c>SetVehicleDriveData(Ids.Vehicles.TreeHarvesterT1, vehicleDriveData);</c><br/>
        /// <c>SetVehicleDriveData(Ids.Vehicles.TreePlanterT1, vehicleDriveData);</c>
        /// </remarks>
        public static void SetVehicleDriveData(DynamicEntityProto.ID refVehicleID, double[] driveData) {
            if (pReg == null) { ModLog.Warning($"SetVehicleDriveData: the ProtoRegistrator is not referenced!"); return; };
            DrivingEntityProto refVehicle = TruckRef(refVehicleID);
            if (refVehicle == null) refVehicle = ExcavRef(refVehicleID);
            if (refVehicle == null) refVehicle = TrHarvRef(refVehicleID);
            if (refVehicle == null) refVehicle = TrPlantRef(refVehicleID);
            if (refVehicle == null) { ModLog.Warning($"SetVehicleDriveData: can't find DrivingEntityProto reference!"); return; }
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
            SyncProtoMod(refVehicle);
        }

        /// <remarks>
        /// Modifies description of a <b>DrivingEntityProto</b> (<i>TruckProto</i> or <i>ExcavatorProto</i> only). For reference use description values in example below.<br/><br/>
        /// 
        /// <br/><u>Usage Example (within 'RegisterData' scope)</u>
        /// 
        /// <br/><br/>Activate the <b>LocalizationManager</b> override to avoid exceptions:<br/>
        /// <c>LocalizationManager.IgnoreDuplicates();</c>
        /// 
        /// <br/><br/>Define new vehicle localization as <b>string[]</b> array (using relevant strings):<br/>
        /// <c>string[] vehicleLocString = new string[] { "Truck with capacity of {0}. Will go under height {1} or higher.", "truck capacity value" };</c><br/>
        /// <c>string[] vehicleLocString = new string[] { "Excavator with capacity of {0}. Will not go under belts/pipes.", "excavator capacity value" };</c>
        /// 
        /// <br/><br/>Apply new description strings via <b>DrivingEntityProto</b> identifier:<br/>
        /// <c>SetVehicleDescription(Ids.Vehicles.TruckT1.Id, vehicleLocString);</c>
        /// </remarks>
        public static void SetVehicleDescription(DynamicEntityProto.ID refVehicleID, string[] strSet, bool canGoUnder = false, int ceilMin = 2) {
            if (pReg == null) { ModLog.Warning($"SetVehicleDriveData: the ProtoRegistrator is not referenced!"); return; };
            DrivingEntityProto refVehicle = TruckRef(refVehicleID);
            if (refVehicle == null) refVehicle = ExcavRef(refVehicleID);
            if (refVehicle == null) { ModLog.Warning($"SetVehicleDescription: can't find DrivingEntityProto reference!"); return; }
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
                SyncProtoMod(refVehicle);
            }
        }
    }
}