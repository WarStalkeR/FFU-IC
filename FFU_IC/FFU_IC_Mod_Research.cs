using Mafi.Base;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions
        public readonly Dictionary<string, int> TechVars =
            new Dictionary<string, int>() {
            { "TechVC1", 40 },
            { "TechVC2", 40 },
            { "TechVC3", 40 },
            { "TechVC4", 40 },
            { "TechVC5", 40 },
            { "TechVC6", 40 },
        };

        // Localization Definitions
        public readonly Dictionary<string, string[]> UnitLocStrings =
            new Dictionary<string, string[]>() {
            { "TechVC", new string[] { "VehicleLimitIncrease", "+{0} VEHICLE CAP", "+{0} VEHICLES CAP", "vehicles cap increase, all caps" }},
        };
        public readonly Dictionary<string, string[]> TechLocStrings =
            new Dictionary<string, string[]>() {
            { "TechVC", new string[] { "Increases vehicle limit by {0}.", "{0}=25" }},
        };

        // Reflection Helpers
        public ResearchNodeProto RnRef(ResearchNodeProto.ID refID) => FFU_IC_IDs.ResearchRef(pReg, refID);
        public void SetTechVehicleCapacity(ResearchNodeProto refReserach, int newVehCap) {
            if (refReserach == null) { ModLog.Warning($"SetTechVehicleCapacity: 'refReserach' is undefined!"); return; }
            refReserach.Units.ForEach(refUnit => {
                if (refUnit is VehicleLimitIncreaseUnlock) {
                    VehicleLimitIncreaseUnlock refUnitVehCap = (VehicleLimitIncreaseUnlock)refUnit;
                    ModLog.Info($"{refReserach.Id} Vehicle Capacity: {refUnitVehCap.LimitIncrease} -> {newVehCap}");
                    FieldInfo fieldLimit = typeof(VehicleLimitIncreaseUnlock).GetField("LimitIncrease", BindingFlags.Instance | BindingFlags.Public);
                    fieldLimit.SetValue(refUnit, newVehCap);
                }
            });
            FFU_IC_IDs.SyncProtoMod(refReserach);
        }
        public void SetTechUnitTitle<T>(ResearchNodeProto refReserach, string[] strSet, int refVal) {
            if (refReserach == null) { ModLog.Warning($"SetTechUnitTitle: 'refReserach' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetTechUnitTitle: 'strSet' is undefined!"); return; }
            refReserach.Units.ForEach(refUnit => {
                if (refUnit is T) {
                    TypeInfo typeUnit = typeof(T).GetTypeInfo();
                    FieldInfo fieldTitle = typeUnit.GetDeclaredField("<Title>k__BackingField");
                    if (fieldTitle != null) {
                        ModLog.Info($"{refReserach.Id} unit title modified.");
                        LocStr1Plural techLoc = Loc.Str1Plural(strSet[0], strSet[1], strSet[2], strSet[3]);
                        LocStrFormatted techVehCapTitle = techLoc.Format(refVal.ToString(), refVal);
                        fieldTitle.SetValue(refUnit, techVehCapTitle);
                    }
                }
            });
            FFU_IC_IDs.SyncProtoMod(refReserach);
        }
        public void SetTechDescription(ResearchNodeProto refReserach, string[] strSet, int refVal) {
            if (refReserach == null) { ModLog.Warning($"SetTechDescription: 'refReserach' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetTechDescription: 'strSet' is undefined!"); return; }
            LocStr1 locStr = Loc.Str1(refReserach.Id.Value + "__desc", strSet[0], strSet[1]);
            LocStr locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refReserach.Id.Value + "_formatted", locStr.Format(refVal.ToString()).Value);
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refReserach.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refReserach);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refReserach, newStr);
                FieldInfo fieldDesc = typeof(ResearchNodeProto).GetField("ResolvedDescription", BindingFlags.Instance | BindingFlags.Public);
                fieldDesc.SetValue(refReserach, refReserach.Strings.DescShort);
                FFU_IC_IDs.SyncProtoMod(refReserach);
            }
        }
        public void AddTechRecipe(ResearchNodeProto refReserach, MachineProto refMachine, RecipeProto refNewUnit, bool hideInUI = false) {
            if (refReserach == null) { ModLog.Warning($"AddTechRecipe: 'refReserach' is undefined!"); return; }
            if (refMachine == null) { ModLog.Warning($"AddTechRecipe: 'refMachine' is undefined!"); return; }
            if (refNewUnit == null) { ModLog.Warning($"AddTechRecipe: 'refNewUnit' is undefined!"); return; }
            ModLog.Info($"Added new unit {refNewUnit.Id} to research {refReserach.Id}.");
            Set<IUnlockNodeUnit> newUnitList = new Set<IUnlockNodeUnit>(0, null);
            refReserach.Units.ForEach(refUnit => {
                newUnitList.Add(refUnit);
            });
            newUnitList.AddAndAssertNew(new RecipeUnlock(refNewUnit, refMachine, hideInUI));
            FieldInfo fieldUnits = typeof(ResearchNodeProto).GetField("Units", BindingFlags.Instance | BindingFlags.Public);
            fieldUnits.SetValue(refReserach, newUnitList.ToImmutableArray());
            FFU_IC_IDs.SyncProtoMod(refReserach);
        }
        public void RemoveTechRecipe(ResearchNodeProto refReserach, RecipeProto refOldUnit, bool hideInUI = false) {
            if (refReserach == null) { ModLog.Warning($"RemoveTechRecipe: 'refReserach' is undefined!"); return; }
            if (refOldUnit == null) { ModLog.Warning($"RemoveTechRecipe: 'refOldUnit' is undefined!"); return; }
            ModLog.Info($"Removed existing unit {refOldUnit.Id} from research {refReserach.Id}.");
            Set<IUnlockNodeUnit> newUnitList = new Set<IUnlockNodeUnit>(0, null);
            refReserach.Units.ForEach(refUnit => {
                if (!(refUnit is RecipeUnlock refRecipeUnlock) ||
                refRecipeUnlock.Proto.Id != refOldUnit.Id) {
                    newUnitList.Add(refUnit);
                }
            });
            FieldInfo fieldUnits = typeof(ResearchNodeProto).GetField("Units", BindingFlags.Instance | BindingFlags.Public);
            fieldUnits.SetValue(refReserach, newUnitList.ToImmutableArray());
            FFU_IC_IDs.SyncProtoMod(refReserach);
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
            LocalizationManager.IgnoreDuplicates();

            // Technology References
            ResearchNodeProto techVehCap1 = RnRef(Ids.Research.AdvancedLogisticsControl);
            ResearchNodeProto techVehCap2 = RnRef(Ids.Research.VehicleCapIncrease2);
            ResearchNodeProto techVehCap3 = RnRef(Ids.Research.VehicleCapIncrease3);
            ResearchNodeProto techVehCap4 = RnRef(Ids.Research.VehicleCapIncrease4);
            ResearchNodeProto techVehCap5 = RnRef(Ids.Research.VehicleCapIncrease5);
            ResearchNodeProto techVehCap6 = RnRef(Ids.Research.VehicleCapIncrease6);

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
            SetTechDescription(techVehCap2, TechLocStrings["TechVC"], TechVars["TechVC2"]);
            SetTechDescription(techVehCap3, TechLocStrings["TechVC"], TechVars["TechVC3"]);
            SetTechDescription(techVehCap4, TechLocStrings["TechVC"], TechVars["TechVC4"]);
            SetTechDescription(techVehCap5, TechLocStrings["TechVC"], TechVars["TechVC5"]);
            SetTechDescription(techVehCap6, TechLocStrings["TechVC"], TechVars["TechVC6"]);
        }
    }
}