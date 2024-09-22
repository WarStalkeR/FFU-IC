using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using FFU_Industrial_Lib;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Recipes : IModData {
        public void RegisterData(ProtoRegistrator registrator) {
            // Rebalanced T1 High Steam Cooling
            FFU_ILib.ModifyRecipeTime(Ids.Recipes.SteamHpCondensation, 20.Seconds());
            FFU_ILib.ModifyRecipeInput(Ids.Recipes.SteamHpCondensation, Ids.Products.SteamHi, 8);
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.SteamHpCondensation, Ids.Products.Water, 4);

            // Rebalanced T1 Low Steam Cooling
            FFU_ILib.ModifyRecipeTime(Ids.Recipes.SteamLpCondensation, 20.Seconds());
            FFU_ILib.ModifyRecipeInput(Ids.Recipes.SteamLpCondensation, Ids.Products.SteamLo, 8);
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.SteamLpCondensation, Ids.Products.Water, 5);

            // Rebalanced T1 Depleted Steam Cooling
            FFU_ILib.ModifyRecipeTime(Ids.Recipes.SteamDepletedCondensation, 20.Seconds());
            FFU_ILib.ModifyRecipeInput(Ids.Recipes.SteamDepletedCondensation, Ids.Products.SteamDepleted, 8);
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.SteamDepletedCondensation, Ids.Products.Water, 6);

            // Rebalanced T2 Depleted Steam Cooling
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.SteamDepletedCondensationT2, Ids.Products.Water, 14);

            // Rebalanced Super Steam Desalination
            FFU_ILib.ModifyRecipeInput(Ids.Recipes.DesalinationFromSP, Ids.Products.Seawater, 27);
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.DesalinationFromSP, Ids.Products.Water, 21);

            // Rebalanced Depleted Steam Desalination
            FFU_ILib.ModifyRecipeTime(Ids.Recipes.DesalinationFromDepleted, 10.Seconds());
            FFU_ILib.ModifyRecipeInput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.Seawater, 4);
            FFU_ILib.ModifyRecipeInput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.SteamDepleted, 4);
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.Water, 7);
            FFU_ILib.ModifyRecipeOutput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.Brine, 1);

            // Arc Furnace Half Scrap Recipes
            registrator.RecipeProtoBuilder.Start("Iron scrap smelting (arc half)", 
                FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, Ids.Machines.ArcFurnace)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.IronScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddOutput(8, Ids.Products.MoltenIron, "*")
            .AddOutput(2, Ids.Products.Exhaust, "E")
            .BuildAndAdd();
            registrator.RecipeProtoBuilder.Start("Copper scrap smelting (arc half)", 
                FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, Ids.Machines.ArcFurnace)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.CopperScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddOutput(8, Ids.Products.MoltenCopper, "*")
            .AddOutput(2, Ids.Products.Exhaust, "E")
            .BuildAndAdd();
            registrator.RecipeProtoBuilder.Start("Glass broken smelting (arc half)", 
                FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, Ids.Machines.ArcFurnace)
            .SetDuration(20.Seconds())
            .AddInput(12, Ids.Products.BrokenGlass, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddOutput(8, Ids.Products.MoltenGlass, "*")
            .AddOutput(2, Ids.Products.Exhaust, "E")
            .BuildAndAdd();

            // Arc Furnace Cold Scrap Recipes
            registrator.RecipeProtoBuilder.Start("Iron scrap smelting (arc cold)", 
                FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap, Ids.Machines.ArcFurnace2)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.IronScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddInput(8, Ids.Products.ChilledWater, "D")
            .AddOutput(8, Ids.Products.MoltenIron, "*")
            .AddOutput(6, Ids.Products.SteamDepleted, "Z")
            .BuildAndAdd();
            registrator.RecipeProtoBuilder.Start("Copper scrap smelting (arc cold)", 
                FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap, Ids.Machines.ArcFurnace2)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.CopperScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddInput(8, Ids.Products.ChilledWater, "D")
            .AddOutput(8, Ids.Products.MoltenCopper, "*")
            .AddOutput(6, Ids.Products.SteamDepleted, "Z")
            .BuildAndAdd();
            registrator.RecipeProtoBuilder.Start("Glass broken smelting (arc cold)", 
                FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken, Ids.Machines.ArcFurnace2)
            .SetDuration(20.Seconds())
            .AddInput(12, Ids.Products.BrokenGlass, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddInput(8, Ids.Products.ChilledWater, "D")
            .AddOutput(8, Ids.Products.MoltenGlass, "*")
            .AddOutput(6, Ids.Products.SteamDepleted, "Z")
            .BuildAndAdd();

            // Cold Exhaust Scrubbing Recipe
            registrator.RecipeProtoBuilder.Start("Exhaust filtering (cold)", 
                FFU_IC_IDs.Recipes.ExhaustFilteringCold, Ids.Machines.ExhaustScrubber)
            .SetDuration(10.Seconds())
            .AddInput(30, Ids.Products.Exhaust, "*")
            .AddInput(4, Ids.Products.ChilledWater, "*")
            .AddOutput(1, Ids.Products.Sulfur, "Z")
            .AddOutput(12, Ids.Products.CarbonDioxide, "X")
            .AddOutput(4, Ids.Products.SteamDepleted, "Y")
            .BuildAndAdd();

            // Graphite-Coal Shredding Recipe
            registrator.RecipeProtoBuilder.Start("Shredding graphite", 
                FFU_IC_IDs.Recipes.GraphiteCoalShredding, Ids.Machines.Shredder)
            .SetDuration(10.Seconds())
            .AddInput(10, Ids.Products.Graphite, "*")
            .AddOutput(5, Ids.Products.Coal, "*")
            .BuildAndAdd();

            // Vacuum Pumping Advanced Recipes
            registrator.RecipeProtoBuilder.Start("Ocean vacuum pumping", 
                FFU_IC_IDs.Recipes.OceanVacuumPumping, Ids.Machines.OceanWaterPumpT1)
            .SetDuration(10.Seconds())
            .AddOutput(36, Ids.Products.Seawater, "X")
            .BuildAndAddBypass();
            registrator.RecipeProtoBuilder.Start("Ocean vacuum pumping II", 
                FFU_IC_IDs.Recipes.OceanVacuumPumpingT2, Ids.Machines.OceanWaterPumpLarge)
            .SetDuration(10.Seconds())
            .AddOutput(36, Ids.Products.Seawater, "X")
            .BuildAndAddBypass();

            // Vacuum Desalination Advanced Recipes
            registrator.RecipeProtoBuilder.Start("Vacuum desalination", 
                FFU_IC_IDs.Recipes.DesalinationVacuumSP, Ids.Machines.ThermalDesalinator)
            .SetDuration(10.Seconds())
            .AddInput(54, Ids.Products.Seawater, "A")
            .AddInput(1, Ids.Products.SteamSp, "B")
            .AddOutput(48, Ids.Products.Water, "W")
            .AddOutput(7, Ids.Products.Brine, "X")
            .BuildAndAddBypass();
            registrator.RecipeProtoBuilder.Start("Vacuum desalination",
                FFU_IC_IDs.Recipes.DesalinationVacuumHP, Ids.Machines.ThermalDesalinator)
            .SetDuration(10.Seconds())
            .AddInput(36, Ids.Products.Seawater, "A")
            .AddInput(2, Ids.Products.SteamHi, "B")
            .AddOutput(31, Ids.Products.Water, "W")
            .AddOutput(7, Ids.Products.Brine, "X")
            .BuildAndAddBypass();
            registrator.RecipeProtoBuilder.Start("Vacuum desalination",
                FFU_IC_IDs.Recipes.DesalinationVacuumLP, Ids.Machines.ThermalDesalinator)
            .SetDuration(10.Seconds())
            .AddInput(24, Ids.Products.Seawater, "A")
            .AddInput(4, Ids.Products.SteamLo, "B")
            .AddOutput(24, Ids.Products.Water, "W")
            .AddOutput(4, Ids.Products.Brine, "X")
            .BuildAndAddBypass();

            // Gas Boiler Super Steam Recipes
            registrator.RecipeProtoBuilder.Start("Super steam generation",
                FFU_IC_IDs.Recipes.SuperGenerationFuelGas, Ids.Machines.BoilerGas)
            .SetDuration(10.Seconds())
            .AddInput(4, Ids.Products.Water, "B")
            .AddInput(8, Ids.Products.FuelGas, "A")
            .AddOutput(4, Ids.Products.SteamSp, "X")
            .AddOutput(8, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd();
            registrator.RecipeProtoBuilder.Start("Super steam generation",
                FFU_IC_IDs.Recipes.SuperGenerationHydrogen, Ids.Machines.BoilerGas)
            .SetDuration(10.Seconds())
            .AddInput(4, Ids.Products.Water, "B")
            .AddInput(8, Ids.Products.Hydrogen, "A")
            .AddOutput(4, Ids.Products.SteamSp, "X")
            .AddOutput(4, Ids.Products.SteamDepleted, "Y")
            .BuildAndAdd();
        }
    }
}