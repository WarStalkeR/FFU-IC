using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
        // Modification Definitions
        public const int vehLimit = 40;

        // Localization Definitions
        public readonly Dictionary<string, string[]> UnitLocStrings =
            new Dictionary<string, string[]>() {
            { "VehCap", new string[] { "VehicleLimitIncrease", "+{0} VEHICLE CAP", "+{0} VEHICLES CAP", "vehicles cap increase, all caps" }}
        };
        public readonly Dictionary<string, string[]> TechLocStrings =
            new Dictionary<string, string[]>() {
            { "VehCap", new string[] { "Increases vehicle limit by {0}.", "{0}=25" }}
        };

        // Reflection Helpers
        ResearchNodeProto ResearchRef(ProtoRegistrator pReg, ResearchNodeProto.ID rNodeID) => pReg.PrototypesDb.Get<ResearchNodeProto>(rNodeID).Value;
        void SetTechVehicleCapacity(ResearchNodeProto refReserach, int newVehCap) {
            refReserach.Units.ForEach(refUnit => {
                if (refUnit is VehicleLimitIncreaseUnlock) {
                    VehicleLimitIncreaseUnlock refUnitVehCap = (VehicleLimitIncreaseUnlock)refUnit;
                    ModLog.Info($"{refReserach.Id} Vehicle Capacity: {refUnitVehCap.LimitIncrease} -> {newVehCap}");
                    FieldInfo fieldLimit = typeof(VehicleLimitIncreaseUnlock).GetField("LimitIncrease", BindingFlags.Instance | BindingFlags.Public);
                    fieldLimit.SetValue(refUnit, newVehCap);
                }
            });
        }
        void SetTechUnitTitle(ResearchNodeProto refReserach, string[] strSet) {
            refReserach.Units.ForEach(refUnit => {
                if (refUnit is VehicleLimitIncreaseUnlock) {
                    int currLimit = (refUnit as VehicleLimitIncreaseUnlock).LimitIncrease;
                    TypeInfo typeUnit = typeof(VehicleLimitIncreaseUnlock).GetTypeInfo();
                    FieldInfo fieldTitle = typeUnit.GetDeclaredField("<Title>k__BackingField");
                    if (fieldTitle != null) {
                        LocStr1Plural techLoc = Loc.Str1Plural(strSet[0], strSet[1], strSet[2], strSet[3]);
                        LocStrFormatted techVehCapTitle = techLoc.Format(currLimit.ToString(), currLimit);
                        fieldTitle.SetValue(refUnit, techVehCapTitle);
                    }
                }
            });
        }
        void SetTechDescription(ResearchNodeProto refReserach, string[] strSet, int refVal = 0) {
            LocStr1 techLocStr = Loc.Str1(refReserach.Id.ToString() + "__desc", strSet[0], strSet[1]);
            LocStr newDesc = LocalizationManager.CreateAlreadyLocalizedStr(refReserach.Id.ToString() + "_formatted", techLocStr.Format(refVal.ToString()).Value);
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refReserach);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, newDesc);
                fieldStrings.SetValue(refReserach, newStr);
                FieldInfo fieldDesc = typeof(ResearchNodeProto).GetField("ResolvedDescription", BindingFlags.Instance | BindingFlags.Public);
                fieldDesc.SetValue(refReserach, refReserach.Strings.DescShort);
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Technology References
            ResearchNodeProto techVehCap1 = ResearchRef(registrator, Ids.Research.VehicleCapIncrease);
            ResearchNodeProto techVehCap2 = ResearchRef(registrator, Ids.Research.VehicleCapIncrease2);
            ResearchNodeProto techVehCap3 = ResearchRef(registrator, Ids.Research.VehicleCapIncrease3);
            ResearchNodeProto techVehCap4 = ResearchRef(registrator, Ids.Research.VehicleCapIncrease4);
            ResearchNodeProto techVehCap5 = ResearchRef(registrator, Ids.Research.VehicleCapIncrease5);
            ResearchNodeProto techVehCap6 = ResearchRef(registrator, Ids.Research.VehicleCapIncrease6);

            // Vehicle Capacity Modifications
            SetTechVehicleCapacity(techVehCap1, vehLimit);
            SetTechVehicleCapacity(techVehCap2, vehLimit);
            SetTechVehicleCapacity(techVehCap3, vehLimit);
            SetTechVehicleCapacity(techVehCap4, vehLimit);
            SetTechVehicleCapacity(techVehCap5, vehLimit);
            SetTechVehicleCapacity(techVehCap6, vehLimit);
            SetTechUnitTitle(techVehCap1, UnitLocStrings["VehCap"]);
            SetTechUnitTitle(techVehCap2, UnitLocStrings["VehCap"]);
            SetTechUnitTitle(techVehCap3, UnitLocStrings["VehCap"]);
            SetTechUnitTitle(techVehCap4, UnitLocStrings["VehCap"]);
            SetTechUnitTitle(techVehCap5, UnitLocStrings["VehCap"]);
            SetTechUnitTitle(techVehCap6, UnitLocStrings["VehCap"]);
            SetTechDescription(techVehCap1, TechLocStrings["VehCap"], vehLimit);
            SetTechDescription(techVehCap2, TechLocStrings["VehCap"], vehLimit);
            SetTechDescription(techVehCap3, TechLocStrings["VehCap"], vehLimit);
            SetTechDescription(techVehCap4, TechLocStrings["VehCap"], vehLimit);
            SetTechDescription(techVehCap5, TechLocStrings["VehCap"], vehLimit);
            SetTechDescription(techVehCap6, TechLocStrings["VehCap"], vehLimit);
        }
    }
}