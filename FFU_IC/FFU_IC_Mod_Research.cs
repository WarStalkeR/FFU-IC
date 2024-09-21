using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Modification Definitions
        private readonly Dictionary<string, int> TechVars =
            new Dictionary<string, int>() {
            { "TechVC1", 40 },
            { "TechVC2", 40 },
            { "TechVC3", 40 },
            { "TechVC4", 40 },
            { "TechVC5", 40 },
            { "TechVC6", 40 },
        };

        // Localization Definitions
        private readonly Dictionary<string, string[]> UnitLocStrings =
            new Dictionary<string, string[]>() {
            { "TechVC", new string[] { "VehicleLimitIncrease", "+{0} VEHICLE CAP", "+{0} VEHICLES CAP", "vehicles cap increase, all caps" }},
        };
        private readonly Dictionary<string, string[]> TechLocStrings =
            new Dictionary<string, string[]>() {
            { "TechVC", new string[] { "Increases vehicle limit by {0}.", "{0}=25" }},
        };

        // Reflection Helpers
        public void SetTechVehicleCapacity(ResearchNodeProto.ID refResearchID, int newVehCap) {
            if (pReg == null) { ModLog.Warning($"SetTechVehicleCapacity: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = FFU_IC_IDs.ResearchRef(pReg, refResearchID);
            if (refResearch == null) { ModLog.Warning($"SetTechVehicleCapacity: 'refReserach' is undefined!"); return; }
            refResearch.Units.ForEach(refUnit => {
                if (refUnit is VehicleLimitIncreaseUnlock) {
                    VehicleLimitIncreaseUnlock refUnitVehCap = (VehicleLimitIncreaseUnlock)refUnit;
                    ModLog.Info($"{refResearch.Id} Vehicle Capacity: {refUnitVehCap.LimitIncrease} -> {newVehCap}");
                    FieldInfo fieldLimit = typeof(VehicleLimitIncreaseUnlock).GetField("LimitIncrease", BindingFlags.Instance | BindingFlags.Public);
                    fieldLimit.SetValue(refUnit, newVehCap);
                }
            });
            FFU_IC_IDs.SyncProtoMod(refResearch);
        }
        public void SetTechUnitTitle<T>(ResearchNodeProto.ID refResearchID, string[] strSet, int refVal) {
            if (pReg == null) { ModLog.Warning($"SetTechUnitTitle: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = FFU_IC_IDs.ResearchRef(pReg, refResearchID);
            if (refResearch == null) { ModLog.Warning($"SetTechUnitTitle: 'refReserach' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetTechUnitTitle: 'strSet' is undefined!"); return; }
            refResearch.Units.ForEach(refUnit => {
                if (refUnit is T) {
                    TypeInfo typeUnit = typeof(T).GetTypeInfo();
                    FieldInfo fieldTitle = typeUnit.GetDeclaredField("<Title>k__BackingField");
                    if (fieldTitle != null) {
                        ModLog.Info($"{refResearch.Id} unit title modified.");
                        LocStr1Plural techLoc = Loc.Str1Plural(strSet[0], strSet[1], strSet[2], strSet[3]);
                        LocStrFormatted techVehCapTitle = techLoc.Format(refVal.ToString(), refVal);
                        fieldTitle.SetValue(refUnit, techVehCapTitle);
                    }
                }
            });
            FFU_IC_IDs.SyncProtoMod(refResearch);
        }
        public void SetTechDescription(ResearchNodeProto.ID refResearchID, string[] strSet, int refVal) {
            if (pReg == null) { ModLog.Warning($"SetTechDescription: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = FFU_IC_IDs.ResearchRef(pReg, refResearchID);
            if (refResearch == null) { ModLog.Warning($"SetTechDescription: 'refReserach' is undefined!"); return; }
            if (strSet == null) { ModLog.Warning($"SetTechDescription: 'strSet' is undefined!"); return; }
            LocStr1 locStr = Loc.Str1(refResearch.Id.Value + "__desc", strSet[0], strSet[1]);
            LocStr locDesc = LocalizationManager.CreateAlreadyLocalizedStr(refResearch.Id.Value + "_formatted", locStr.Format(refVal.ToString()).Value);
            TypeInfo typeProto = typeof(Mafi.Core.Prototypes.Proto).GetTypeInfo();
            FieldInfo fieldStrings = typeProto.GetDeclaredField("<Strings>k__BackingField");
            if (fieldStrings != null) {
                ModLog.Info($"{refResearch.Id} description modified.");
                Mafi.Core.Prototypes.Proto.Str currStr = (Mafi.Core.Prototypes.Proto.Str)fieldStrings.GetValue(refResearch);
                Mafi.Core.Prototypes.Proto.Str newStr = new Mafi.Core.Prototypes.Proto.Str(currStr.Name, locDesc);
                fieldStrings.SetValue(refResearch, newStr);
                FieldInfo fieldDesc = typeof(ResearchNodeProto).GetField("ResolvedDescription", BindingFlags.Instance | BindingFlags.Public);
                fieldDesc.SetValue(refResearch, refResearch.Strings.DescShort);
                FFU_IC_IDs.SyncProtoMod(refResearch);
            }
        }
        public void AddTechRecipe(ResearchNodeProto.ID refResearchID, MachineProto.ID refMachineID, RecipeProto.ID refNewUnitID, bool hideInUI = false, int index = -1) {
            if (pReg == null) { ModLog.Warning($"AddTechRecipe: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = FFU_IC_IDs.ResearchRef(pReg, refResearchID);
            MachineProto refMachine = FFU_IC_IDs.MachineRef(pReg, refMachineID);
            RecipeProto refNewUnit = FFU_IC_IDs.RecipeRef(pReg, refNewUnitID);
            if (refResearch == null) { ModLog.Warning($"AddTechRecipe: 'refReserach' is undefined!"); return; }
            if (refMachine == null) { ModLog.Warning($"AddTechRecipe: 'refMachine' is undefined!"); return; }
            if (refNewUnit == null) { ModLog.Warning($"AddTechRecipe: 'refNewUnit' is undefined!"); return; }
            ModLog.Info($"Added new unit {refNewUnit.Id} to research {refResearch.Id}.");
            Set<IUnlockNodeUnit> newUnitList = new Set<IUnlockNodeUnit>(0, null);
            int idx = 0;
            refResearch.Units.ForEach(refUnit => {
                if (index >= 0 && idx == index)
                    newUnitList.AddAndAssertNew(new RecipeUnlock(refNewUnit, refMachine, hideInUI));
                newUnitList.Add(refUnit);
                idx++;
            });
            if (index < 0) newUnitList.AddAndAssertNew(new RecipeUnlock(refNewUnit, refMachine, hideInUI));
            FieldInfo fieldUnits = typeof(ResearchNodeProto).GetField("Units", BindingFlags.Instance | BindingFlags.Public);
            fieldUnits.SetValue(refResearch, newUnitList.ToImmutableArray());
            FFU_IC_IDs.SyncProtoMod(refResearch);
        }
        public void RemoveTechRecipe(ResearchNodeProto.ID refResearchID, RecipeProto.ID refOldUnitID, bool hideInUI = false) {
            if (pReg == null) { ModLog.Warning($"RemoveTechRecipe: the ProtoRegistrator is not referenced!"); return; };
            ResearchNodeProto refResearch = FFU_IC_IDs.ResearchRef(pReg, refResearchID);
            RecipeProto refOldUnit = FFU_IC_IDs.RecipeRef(pReg, refOldUnitID);
            if (refResearch == null) { ModLog.Warning($"RemoveTechRecipe: 'refReserach' is undefined!"); return; }
            if (refOldUnit == null) { ModLog.Warning($"RemoveTechRecipe: 'refOldUnit' is undefined!"); return; }
            ModLog.Info($"Removed existing unit {refOldUnit.Id} from research {refResearch.Id}.");
            Set<IUnlockNodeUnit> newUnitList = new Set<IUnlockNodeUnit>(0, null);
            refResearch.Units.ForEach(refUnit => {
                if (!(refUnit is RecipeUnlock refRecipeUnlock) ||
                refRecipeUnlock.Proto.Id != refOldUnit.Id) {
                    newUnitList.Add(refUnit);
                }
            });
            FieldInfo fieldUnits = typeof(ResearchNodeProto).GetField("Units", BindingFlags.Instance | BindingFlags.Public);
            fieldUnits.SetValue(refResearch, newUnitList.ToImmutableArray());
            FFU_IC_IDs.SyncProtoMod(refResearch);
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
            LocalizationManager.IgnoreDuplicates();

            // Vehicle Capacity Modifications
            SetTechVehicleCapacity(Ids.Research.AdvancedLogisticsControl, TechVars["TechVC1"]);
            SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease2, TechVars["TechVC2"]);
            SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease3, TechVars["TechVC3"]);
            SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease4, TechVars["TechVC4"]);
            SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease5, TechVars["TechVC5"]);
            SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease6, TechVars["TechVC6"]);

            // Vehicle Capacity Unit Description
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.AdvancedLogisticsControl, UnitLocStrings["TechVC"], TechVars["TechVC1"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease2, UnitLocStrings["TechVC"], TechVars["TechVC2"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease3, UnitLocStrings["TechVC"], TechVars["TechVC3"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease4, UnitLocStrings["TechVC"], TechVars["TechVC4"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease5, UnitLocStrings["TechVC"], TechVars["TechVC5"]);
            SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease6, UnitLocStrings["TechVC"], TechVars["TechVC6"]);

            // Vehicle Capacity Tech Description
            SetTechDescription(Ids.Research.VehicleCapIncrease2, TechLocStrings["TechVC"], TechVars["TechVC2"]);
            SetTechDescription(Ids.Research.VehicleCapIncrease3, TechLocStrings["TechVC"], TechVars["TechVC3"]);
            SetTechDescription(Ids.Research.VehicleCapIncrease4, TechLocStrings["TechVC"], TechVars["TechVC4"]);
            SetTechDescription(Ids.Research.VehicleCapIncrease5, TechLocStrings["TechVC"], TechVars["TechVC5"]);
            SetTechDescription(Ids.Research.VehicleCapIncrease6, TechLocStrings["TechVC"], TechVars["TechVC6"]);

            // Super Steam Water Desalination Tech
            AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.ThermalDesalinator, Ids.Recipes.DesalinationFromSP);

            // Add Half Arc Scrap Smelting Recipes
            AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, index: 2);
            AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, index: 3);
            AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, index: 4);

            // Add Cold Arc Scrap Smelting Recipes
            AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap);
            AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap);
            AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken);

            // Add Cold Exhaust Scrubbing Recipe
            AddTechRecipe(Ids.Research.CarbonDioxideRecycling, Ids.Machines.ExhaustScrubber, FFU_IC_IDs.Recipes.ExhaustFilteringCold);

            // Add Graphite-Coal Shredding Recipe
            AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.Shredder, FFU_IC_IDs.Recipes.GraphiteCoalShredding, index: 7);

            // Vacuum Pumping + Vacuum Desalination
            AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.OceanWaterPumpT1, FFU_IC_IDs.Recipes.OceanVacuumPumping);
            AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.OceanWaterPumpLarge, FFU_IC_IDs.Recipes.OceanVacuumPumpingT2);
            AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumSP);
            AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumHP);
            AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumLP);

            // Gas Boiler Super Steam Generation
            AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.BoilerGas, FFU_IC_IDs.Recipes.SuperGenerationFuelGas);
            AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.BoilerGas, FFU_IC_IDs.Recipes.SuperGenerationHydrogen);
        }
    }
}