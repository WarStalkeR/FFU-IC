using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
        // Modification Variables
        ProtoRegistrator protoReg = null;

        // Modification Definitions
        public readonly Dictionary<string, int> TechVars =
            new Dictionary<string, int>() {
            { "TechVC1", 40 },
            { "TechVC2", 40 },
            { "TechVC3", 40 },
            { "TechVC4", 40 },
            { "TechVC5", 40 },
            { "TechVC6", 40 }
        };

        // Localization Definitions
        public readonly Dictionary<string, string[]> UnitLocStrings =
            new Dictionary<string, string[]>() {
            { "TechVC", new string[] { "VehicleLimitIncrease", "+{0} VEHICLE CAP", "+{0} VEHICLES CAP", "vehicles cap increase, all caps" }}
        };
        public readonly Dictionary<string, string[]> TechLocStrings =
            new Dictionary<string, string[]>() {
            { "TechVC", new string[] { "Increases vehicle limit by {0}.", "{0}=25" }}
        };

        // Reflection Helpers
        public ResearchNodeProto ResearchRef(ResearchNodeProto.ID refID) => protoReg.PrototypesDb.Get<ResearchNodeProto>(refID).Value;
        public void SetTechVehicleCapacity(ResearchNodeProto refReserach, int newVehCap) {
            refReserach.Units.ForEach(refUnit => {
                if (refUnit is VehicleLimitIncreaseUnlock) {
                    VehicleLimitIncreaseUnlock refUnitVehCap = (VehicleLimitIncreaseUnlock)refUnit;
                    ModLog.Info($"{refReserach.Id} Vehicle Capacity: {refUnitVehCap.LimitIncrease} -> {newVehCap}");
                    FieldInfo fieldLimit = typeof(VehicleLimitIncreaseUnlock).GetField("LimitIncrease", BindingFlags.Instance | BindingFlags.Public);
                    fieldLimit.SetValue(refUnit, newVehCap);
                }
            });
        }
        public void SetTechUnitTitle<T>(ResearchNodeProto refReserach, string[] strSet, int refVal) {
            refReserach.Units.ForEach(refUnit => {
                if (refUnit is T) {
                    TypeInfo typeUnit = typeof(T).GetTypeInfo();
                    FieldInfo fieldTitle = typeUnit.GetDeclaredField("<Title>k__BackingField");
                    if (fieldTitle != null) {
                        LocStr1Plural techLoc = Loc.Str1Plural(strSet[0], strSet[1], strSet[2], strSet[3]);
                        LocStrFormatted techVehCapTitle = techLoc.Format(refVal.ToString(), refVal);
                        fieldTitle.SetValue(refUnit, techVehCapTitle);
                    }
                }
            });
        }
        public void SetTechDescription(ResearchNodeProto refReserach, string[] strSet, int refVal) {
            LocStr1 locStr = Loc.Str1(refReserach.Id.Value + "__desc", strSet[0], strSet[1]);
            LocStr locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refReserach.Id.Value + "_formatted", locStr.Format(refVal.ToString()).Value);
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refReserach);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refReserach, newStr);
                FieldInfo fieldDesc = typeof(ResearchNodeProto).GetField("ResolvedDescription", BindingFlags.Instance | BindingFlags.Public);
                fieldDesc.SetValue(refReserach, refReserach.Strings.DescShort);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            protoReg = registrator;

            // Technology References
            ResearchNodeProto techVehCap1 = ResearchRef(Ids.Research.VehicleCapIncrease);
            ResearchNodeProto techVehCap2 = ResearchRef(Ids.Research.VehicleCapIncrease2);
            ResearchNodeProto techVehCap3 = ResearchRef(Ids.Research.VehicleCapIncrease3);
            ResearchNodeProto techVehCap4 = ResearchRef(Ids.Research.VehicleCapIncrease4);
            ResearchNodeProto techVehCap5 = ResearchRef(Ids.Research.VehicleCapIncrease5);
            ResearchNodeProto techVehCap6 = ResearchRef(Ids.Research.VehicleCapIncrease6);

            // Vehicle Capacity Modifications
            SetTechVehicleCapacity(techVehCap1, TechVars["TechVC1"]);
            SetTechVehicleCapacity(techVehCap2, TechVars["TechVC2"]);
            SetTechVehicleCapacity(techVehCap3, TechVars["TechVC3"]);
            SetTechVehicleCapacity(techVehCap4, TechVars["TechVC4"]);
            SetTechVehicleCapacity(techVehCap5, TechVars["TechVC5"]);
            SetTechVehicleCapacity(techVehCap6, TechVars["TechVC6"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(techVehCap1, UnitLocStrings["TechVC"], TechVars["TechVC1"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(techVehCap2, UnitLocStrings["TechVC"], TechVars["TechVC2"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(techVehCap3, UnitLocStrings["TechVC"], TechVars["TechVC3"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(techVehCap4, UnitLocStrings["TechVC"], TechVars["TechVC4"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(techVehCap5, UnitLocStrings["TechVC"], TechVars["TechVC5"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(techVehCap6, UnitLocStrings["TechVC"], TechVars["TechVC6"]);
            SetTechDescription(techVehCap1, TechLocStrings["TechVC"], TechVars["TechVC1"]);
            SetTechDescription(techVehCap2, TechLocStrings["TechVC"], TechVars["TechVC2"]);
            SetTechDescription(techVehCap3, TechLocStrings["TechVC"], TechVars["TechVC3"]);
            SetTechDescription(techVehCap4, TechLocStrings["TechVC"], TechVars["TechVC4"]);
            SetTechDescription(techVehCap5, TechLocStrings["TechVC"], TechVars["TechVC5"]);
            SetTechDescription(techVehCap6, TechLocStrings["TechVC"], TechVars["TechVC6"]);
        }
    }
}