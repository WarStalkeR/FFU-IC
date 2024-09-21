using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;

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

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            FFU_IC_Lib.ProtoReg = registrator;
            LocalizationManager.IgnoreDuplicates();

            // Vehicle Capacity Modifications
            FFU_IC_Lib.SetTechVehicleCapacity(Ids.Research.AdvancedLogisticsControl, TechVars["TechVC1"]);
            FFU_IC_Lib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease2, TechVars["TechVC2"]);
            FFU_IC_Lib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease3, TechVars["TechVC3"]);
            FFU_IC_Lib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease4, TechVars["TechVC4"]);
            FFU_IC_Lib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease5, TechVars["TechVC5"]);
            FFU_IC_Lib.SetTechVehicleCapacity(Ids.Research.VehicleCapIncrease6, TechVars["TechVC6"]);

            // Vehicle Capacity Unit Description
            FFU_IC_Lib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.AdvancedLogisticsControl, UnitLocStrings["TechVC"], TechVars["TechVC1"]);
            FFU_IC_Lib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease2, UnitLocStrings["TechVC"], TechVars["TechVC2"]);
            FFU_IC_Lib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease3, UnitLocStrings["TechVC"], TechVars["TechVC3"]);
            FFU_IC_Lib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease4, UnitLocStrings["TechVC"], TechVars["TechVC4"]);
            FFU_IC_Lib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease5, UnitLocStrings["TechVC"], TechVars["TechVC5"]);
            FFU_IC_Lib.SetTechUnitTitle<VehicleLimitIncreaseUnlock>(Ids.Research.VehicleCapIncrease6, UnitLocStrings["TechVC"], TechVars["TechVC6"]);

            // Vehicle Capacity Tech Description
            FFU_IC_Lib.SetTechDescription(Ids.Research.VehicleCapIncrease2, TechLocStrings["TechVC"], TechVars["TechVC2"]);
            FFU_IC_Lib.SetTechDescription(Ids.Research.VehicleCapIncrease3, TechLocStrings["TechVC"], TechVars["TechVC3"]);
            FFU_IC_Lib.SetTechDescription(Ids.Research.VehicleCapIncrease4, TechLocStrings["TechVC"], TechVars["TechVC4"]);
            FFU_IC_Lib.SetTechDescription(Ids.Research.VehicleCapIncrease5, TechLocStrings["TechVC"], TechVars["TechVC5"]);
            FFU_IC_Lib.SetTechDescription(Ids.Research.VehicleCapIncrease6, TechLocStrings["TechVC"], TechVars["TechVC6"]);

            // Super Steam Water Desalination Tech
            FFU_IC_Lib.AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.ThermalDesalinator, Ids.Recipes.DesalinationFromSP);

            // Add Half Arc Scrap Smelting Recipes
            FFU_IC_Lib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, index: 2);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, index: 3);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.ArcFurnace, FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, index: 4);

            // Add Cold Arc Scrap Smelting Recipes
            FFU_IC_Lib.AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.ArcFurnace2, Ids.Machines.ArcFurnace2, FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken);

            // Add Cold Exhaust Scrubbing Recipe
            FFU_IC_Lib.AddTechRecipe(Ids.Research.CarbonDioxideRecycling, Ids.Machines.ExhaustScrubber, FFU_IC_IDs.Recipes.ExhaustFilteringCold);

            // Add Graphite-Coal Shredding Recipe
            FFU_IC_Lib.AddTechRecipe(Ids.Research.PolySiliconProduction, Ids.Machines.Shredder, FFU_IC_IDs.Recipes.GraphiteCoalShredding, index: 7);

            // Vacuum Pumping + Vacuum Desalination
            FFU_IC_Lib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.OceanWaterPumpT1, FFU_IC_IDs.Recipes.OceanVacuumPumping);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.OceanWaterPumpLarge, FFU_IC_IDs.Recipes.OceanVacuumPumpingT2);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumSP);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumHP);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.VacuumDesalination, Ids.Machines.ThermalDesalinator, FFU_IC_IDs.Recipes.DesalinationVacuumLP);

            // Gas Boiler Super Steam Generation
            FFU_IC_Lib.AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.BoilerGas, FFU_IC_IDs.Recipes.SuperGenerationFuelGas);
            FFU_IC_Lib.AddTechRecipe(Ids.Research.SuperPressSteam, Ids.Machines.BoilerGas, FFU_IC_IDs.Recipes.SuperGenerationHydrogen);
        }
    }
}