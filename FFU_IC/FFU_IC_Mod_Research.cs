using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using FFU_Industrial_Lib;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
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

        public void RegisterData(ProtoRegistrator registrator) {
            // Registrator Initialization
            LocalizationManager.IgnoreDuplicates();

            // Vehicle Capacity Modifications
            FFU_ILib.SetTechVehicleCapacity(Ids.Research.AdvancedLogisticsControl, TechVars["TechVC1"]);
            FFU_ILib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease2, TechVars["TechVC2"]);
            FFU_ILib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease3, TechVars["TechVC3"]);
            FFU_ILib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease4, TechVars["TechVC4"]);
            FFU_ILib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease5, TechVars["TechVC5"]);
            FFU_ILib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease6, TechVars["TechVC6"]);

            // Vehicle Capacity Unit Description
            FFU_ILib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.AdvancedLogisticsControl, UnitLocStrings["TechVC"], TechVars["TechVC1"]);
            FFU_ILib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease2, UnitLocStrings["TechVC"], TechVars["TechVC2"]);
            FFU_ILib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease3, UnitLocStrings["TechVC"], TechVars["TechVC3"]);
            FFU_ILib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease4, UnitLocStrings["TechVC"], TechVars["TechVC4"]);
            FFU_ILib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease5, UnitLocStrings["TechVC"], TechVars["TechVC5"]);
            FFU_ILib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease6, UnitLocStrings["TechVC"], TechVars["TechVC6"]);

            // Vehicle Capacity Tech Description
            FFU_ILib.SetTechDescription(Ids.Research.VehicleCapIncrease2, TechLocStrings["TechVC"], TechVars["TechVC2"]);
            FFU_ILib.SetTechDescription(Ids.Research.VehicleCapIncrease3, TechLocStrings["TechVC"], TechVars["TechVC3"]);
            FFU_ILib.SetTechDescription(Ids.Research.VehicleCapIncrease4, TechLocStrings["TechVC"], TechVars["TechVC4"]);
            FFU_ILib.SetTechDescription(Ids.Research.VehicleCapIncrease5, TechLocStrings["TechVC"], TechVars["TechVC5"]);
            FFU_ILib.SetTechDescription(Ids.Research.VehicleCapIncrease6, TechLocStrings["TechVC"], TechVars["TechVC6"]);

            // Make All Research Recipes Visible
            FFU_ILib.UnhideRecipes(Ids.Research.VehicleAssembly2);
            FFU_ILib.UnhideRecipes(Ids.Research.VehicleAssembly3);
            FFU_ILib.UnhideRecipes(Ids.Research.Hospital);
            FFU_ILib.UnhideRecipes(Ids.Research.MedicalSupplies2);

            // Super Steam Water Desalination Tech
            FFU_ILib.AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.ThermalDesalinator, Ids.Recipes.DesalinationFromSP);

            // Missing Manual Assembly Recipes
            FFU_ILib.AddTechRecipe(Ids.Research.ResearchLab2, Ids.Machines.AssemblyManual, FFU_IC_IDs.Recipes.LabEquipment1AssemblyT0, index: 1);

            // Missing Electric Assembly Recipes
            FFU_ILib.AddTechRecipe(Ids.Research.VehicleAssembly2, Ids.Machines.AssemblyElectrified, FFU_IC_IDs.Recipes.VehicleParts2AssemblyT0, index: 4);
            FFU_ILib.AddTechRecipe(Ids.Research.ResearchLab3, Ids.Machines.AssemblyElectrified, FFU_IC_IDs.Recipes.LabEquipment2AssemblyT0, index: 1);

            // Missing Robotic Assembly Recipes
            FFU_ILib.AddTechRecipe(Ids.Research.BasicComputing, Ids.Machines.AssemblyRoboticT1, FFU_IC_IDs.Recipes.MechPartsAssemblyT4Iron, index: 7);
            FFU_ILib.AddTechRecipe(Ids.Research.FoodPacking, Ids.Machines.AssemblyRoboticT1, FFU_IC_IDs.Recipes.FoodPackAssemblyT4Meat, index: 1);
            FFU_ILib.AddTechRecipe(Ids.Research.FoodPacking, Ids.Machines.AssemblyRoboticT1, FFU_IC_IDs.Recipes.FoodPackAssemblyT4Eggs);
            FFU_ILib.AddTechRecipe(Ids.Research.UraniumEnrichment, Ids.Machines.AssemblyRoboticT1, FFU_IC_IDs.Recipes.UraniumRodsAssemblyT4, index: 8);
            FFU_ILib.AddTechRecipe(Ids.Research.NuclearReactor2, Ids.Machines.AssemblyRoboticT1, Ids.Recipes.UraniumEnrichedAssemblyT1);

            // Missing Adv. Robotic Assembly Recipes
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.CpBrAssemblyT5, index: 1);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.CpSlAssemblyT5, index: 2);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.Cp2AssemblyT5, index: 3);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.Cp3AssemblyT5, index: 4);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.MechPartsAssemblyT5Iron, index: 6);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.MechPartsAssemblyT5Steel, index: 7);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.VehicleParts1AssemblyT5, index: 8);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.LabEquipment1AssemblyT5, index: 9);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.LabEquipment2AssemblyT5, index: 10);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.LabEquipment3AssemblyT5, index: 11);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.LabEquipment4AssemblyT5, index: 12);
            FFU_ILib.AddTechRecipe(Ids.Research.Assembler3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.HouseholdGoodsAssemblyT5, index: 13);
            FFU_ILib.AddTechRecipe(Ids.Research.VehicleAssembly2, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.VehicleParts2AssemblyT5);
            FFU_ILib.AddTechRecipe(Ids.Research.VehicleAssembly3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.VehicleParts3AssemblyT5);
            FFU_ILib.AddTechRecipe(Ids.Research.Hospital, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.MedicalEquipmentAssemblyT5, index: 3);
            FFU_ILib.AddTechRecipe(Ids.Research.Hospital, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.MedicalSuppliesAssemblyT5);
            FFU_ILib.AddTechRecipe(Ids.Research.MedicalSupplies2, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.MedicalSupplies2AssemblyT5);
            FFU_ILib.AddTechRecipe(Ids.Research.MedicalSupplies3, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.MedicalSupplies3AssemblyT5, index: 5);
            FFU_ILib.AddTechRecipe(Ids.Research.FoodPacking, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.FoodPackAssemblyT5Meat, index: 2);
            FFU_ILib.AddTechRecipe(Ids.Research.FoodPacking, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.FoodPackAssemblyT5Eggs);
            FFU_ILib.AddTechRecipe(Ids.Research.UraniumEnrichment, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.UraniumRodsAssemblyT5, index: 9);
            FFU_ILib.AddTechRecipe(Ids.Research.NuclearReactor2, Ids.Machines.AssemblyRoboticT2, FFU_IC_IDs.Recipes.UraniumEnrichedAssemblyT5);

            // Add Half Arc Scrap Smelting Recipes
            FFU_ILib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, index: 2);
            FFU_ILib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, index: 3);
            FFU_ILib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, index: 4);

            // Add Cold Arc Scrap Smelting Recipes
            FFU_ILib.AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap);
            FFU_ILib.AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap);
            FFU_ILib.AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken);

            // Add Cold Exhaust Scrubbing Recipe
            FFU_ILib.AddTechRecipe(Ids.Research.CarbonDioxideRecycling, Ids.Machines.ExhaustScrubber, FFU_IC_IDs.Recipes.ExhaustFilteringCold);

            // Add Graphite-Coal Shredding Recipe
            FFU_ILib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.Shredder, FFU_IC_IDs.Recipes.GraphiteCoalShredding, index: 7);

            // Vacuum Pumping + Vacuum Desalination
            FFU_ILib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.OceanWaterPumpT1, FFU_IC_IDs.Recipes.OceanVacuumPumping);
            FFU_ILib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.OceanWaterPumpLarge, FFU_IC_IDs.Recipes.OceanVacuumPumpingT2);
            FFU_ILib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumSP);
            FFU_ILib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumHP);
            FFU_ILib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumLP);

            // Gas Boiler Super Steam Generation
            FFU_ILib.AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.BoilerGas, FFU_IC_IDs.Recipes.SuperGenerationFuelGas);
            FFU_ILib.AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.BoilerGas, FFU_IC_IDs.Recipes.SuperGenerationHydrogen);
        }
    }
}