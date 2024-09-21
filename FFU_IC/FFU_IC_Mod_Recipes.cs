using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using System;
using System.Reflection;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Recipes : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Reflection Helpers
        public void ModifyRecipeTime(RecipeProto.ID refRecipeID, Duration newTime) {
            if (pReg == null) { ModLog.Warning($"ModifyRecipeTime: the ProtoRegistrator is not referenced!"); return; };
            RecipeProto refRecipe = FFU_IC_IDs.RecipeRef(pReg, refRecipeID);
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeTime: 'refRecipe' is undefined!"); return; }
            ModLog.Info($"{refRecipe.Id} Production Time: {refRecipe.Duration.Seconds}s -> {newTime.Seconds}s");
            TypeInfo typeProto = typeof(RecipeProto).GetTypeInfo();
            FieldInfo fieldDuration = typeProto.GetDeclaredField("<Duration>k__BackingField");
            if (fieldDuration != null) {
                fieldDuration.SetValue(refRecipe, newTime);
                FFU_IC_IDs.SyncProtoMod(refRecipe);
            }
        }
        public void ModifyRecipeInput(RecipeProto.ID refRecipeID, ProductProto.ID refProductID, int newAmount) {
            if (pReg == null) { ModLog.Warning($"ModifyRecipeInput: the ProtoRegistrator is not referenced!"); return; };
            RecipeProto refRecipe = FFU_IC_IDs.RecipeRef(pReg, refRecipeID);
            ProductProto refProduct = FFU_IC_IDs.ProductRef(pReg, refProductID);
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeInput: 'refRecipe' is undefined!"); return; }
            if (refProduct == null) { ModLog.Warning($"ModifyRecipeInput: 'refProduct' is undefined!"); return; }
            foreach (RecipeInput refInput in refRecipe.AllInputs) {
                if (refInput.Product.Id == refProduct.Id) {
                    ModLog.Info($"{refRecipe.Id} Input {refProduct.Id} Quantity: {refInput.Quantity.Value} -> {newAmount}");
                    FieldInfo fieldQuantity = typeof(RecipeProduct).GetField("Quantity", BindingFlags.Instance | BindingFlags.Public);
                    fieldQuantity.SetValue(refInput, new Quantity(newAmount));
                    SyncRecipeInternalVars(refRecipe);
                    FFU_IC_IDs.SyncProtoMod(refRecipe);
                    break;
                }
            }
        }
        public void ModifyRecipeOutput(RecipeProto.ID refRecipeID, ProductProto.ID refProductID, int newAmount) {
            if (pReg == null) { ModLog.Warning($"ModifyRecipeOutput: the ProtoRegistrator is not referenced!"); return; };
            RecipeProto refRecipe = FFU_IC_IDs.RecipeRef(pReg, refRecipeID);
            ProductProto refProduct = FFU_IC_IDs.ProductRef(pReg, refProductID);
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeOutput: 'refRecipe' is undefined!"); return; }
            if (refProduct == null) { ModLog.Warning($"ModifyRecipeOutput: 'refProduct' is undefined!"); return; }
            foreach (RecipeOutput refOutput in refRecipe.AllOutputs) {
                if (refOutput.Product.Id == refProduct.Id) {
                    ModLog.Info($"{refRecipe.Id} Output {refProduct.Id} Quantity: {refOutput.Quantity.Value} -> {newAmount}");
                    FieldInfo fieldQuantity = typeof(RecipeProduct).GetField("Quantity", BindingFlags.Instance | BindingFlags.Public);
                    fieldQuantity.SetValue(refOutput, new Quantity(newAmount));
                    SyncRecipeInternalVars(refRecipe);
                    FFU_IC_IDs.SyncProtoMod(refRecipe);
                    break;
                }
            }
        }
        private void SyncRecipeInternalVars(RecipeProto refRecipe) {
            if (refRecipe == null) { ModLog.Warning($"SyncRecipeProcedures: 'refRecipe' is undefined!"); return; }
            TypeInfo typeProto = typeof(RecipeProto).GetTypeInfo();

            // Update All Input/Output Fields
            FieldInfo fieldAllUserVisibleInputs = typeProto.GetDeclaredField("<AllUserVisibleInputs>k__BackingField");
            FieldInfo fieldAllUserVisibleOutputs = typeProto.GetDeclaredField("<AllUserVisibleOutputs>k__BackingField");
            FieldInfo fieldOutputsAtEnd = typeProto.GetField("OutputsAtEnd", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldOutputsAtStart = typeProto.GetField("OutputsAtStart", BindingFlags.Instance | BindingFlags.Public);
            fieldAllUserVisibleInputs.SetValue(refRecipe, refRecipe.AllInputs.Filter((RecipeInput x) => !x.HideInUi));
            fieldAllUserVisibleOutputs.SetValue(refRecipe, refRecipe.AllOutputs.Filter((RecipeOutput x) => !x.HideInUi));
            fieldOutputsAtEnd.SetValue(refRecipe, refRecipe.AllOutputs.Filter((RecipeOutput x) => !x.TriggerAtStart));
            fieldOutputsAtStart.SetValue(refRecipe, refRecipe.AllOutputs.Filter((RecipeOutput x) => x.TriggerAtStart));

            // Update Greatest Common Divisor
            int commonDivisor = refRecipe.MinUtilization == Percent.Hundred ? 1 : 0;
            FieldInfo fieldQuantitiesGcd = typeProto.GetField("QuantitiesGcd", BindingFlags.Instance | BindingFlags.Public);
            ImmutableArray<RecipeProduct> allProducts = refRecipe.AllInputs.As<RecipeProduct>().Concat(refRecipe.AllOutputs.As<RecipeProduct>());
            if (commonDivisor == 0 && !allProducts.IsEmpty) commonDivisor = MafiMath.Gcd(allProducts.Select((RecipeProduct x) => x.Quantity.Value));
            if (commonDivisor > 0) fieldQuantitiesGcd.SetValue(refRecipe, commonDivisor);
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // Rebalanced T1 High Steam Cooling
            ModifyRecipeTime(Ids.Recipes.SteamHpCondensation, 20.Seconds());
            ModifyRecipeInput(Ids.Recipes.SteamHpCondensation, Ids.Products.SteamHi, 8);
            ModifyRecipeOutput(Ids.Recipes.SteamHpCondensation, Ids.Products.Water, 4);

            // Rebalanced T1 Low Steam Cooling
            ModifyRecipeTime(Ids.Recipes.SteamLpCondensation, 20.Seconds());
            ModifyRecipeInput(Ids.Recipes.SteamLpCondensation, Ids.Products.SteamLo, 8);
            ModifyRecipeOutput(Ids.Recipes.SteamLpCondensation, Ids.Products.Water, 5);

            // Rebalanced T1 Depleted Steam Cooling
            ModifyRecipeTime(Ids.Recipes.SteamDepletedCondensation, 20.Seconds());
            ModifyRecipeInput(Ids.Recipes.SteamDepletedCondensation, Ids.Products.SteamDepleted, 8);
            ModifyRecipeOutput(Ids.Recipes.SteamDepletedCondensation, Ids.Products.Water, 6);

            // Rebalanced T2 Depleted Steam Cooling
            ModifyRecipeOutput(Ids.Recipes.SteamDepletedCondensationT2, Ids.Products.Water, 14);

            // Rebalanced Super Steam Desalination
            ModifyRecipeInput(Ids.Recipes.DesalinationFromSP, Ids.Products.Seawater, 27);
            ModifyRecipeOutput(Ids.Recipes.DesalinationFromSP, Ids.Products.Water, 21);

            // Rebalanced Depleted Steam Desalination
            ModifyRecipeTime(Ids.Recipes.DesalinationFromDepleted, 10.Seconds());
            ModifyRecipeInput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.Seawater, 4);
            ModifyRecipeInput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.SteamDepleted, 4);
            ModifyRecipeOutput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.Water, 7);
            ModifyRecipeOutput(Ids.Recipes.DesalinationFromDepleted, Ids.Products.Brine, 1);

            // Arc Furnace Half Scrap Recipes
            pReg.RecipeProtoBuilder.Start("Iron scrap smelting (arc half)", 
                FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, Ids.Machines.ArcFurnace)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.IronScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddOutput(8, Ids.Products.MoltenIron, "*")
            .AddOutput(2, Ids.Products.Exhaust, "E")
            .BuildAndAdd();
            pReg.RecipeProtoBuilder.Start("Copper scrap smelting (arc half)", 
                FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, Ids.Machines.ArcFurnace)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.CopperScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddOutput(8, Ids.Products.MoltenCopper, "*")
            .AddOutput(2, Ids.Products.Exhaust, "E")
            .BuildAndAdd();
            pReg.RecipeProtoBuilder.Start("Glass broken smelting (arc half)", 
                FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, Ids.Machines.ArcFurnace)
            .SetDuration(20.Seconds())
            .AddInput(12, Ids.Products.BrokenGlass, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddOutput(8, Ids.Products.MoltenGlass, "*")
            .AddOutput(2, Ids.Products.Exhaust, "E")
            .BuildAndAdd();

            // Arc Furnace Cold Scrap Recipes
            pReg.RecipeProtoBuilder.Start("Iron scrap smelting (arc cold)", 
                FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap, Ids.Machines.ArcFurnace2)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.IronScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddInput(8, Ids.Products.ChilledWater, "D")
            .AddOutput(8, Ids.Products.MoltenIron, "*")
            .AddOutput(6, Ids.Products.SteamDepleted, "Z")
            .BuildAndAdd();
            pReg.RecipeProtoBuilder.Start("Copper scrap smelting (arc cold)", 
                FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap, Ids.Machines.ArcFurnace2)
            .SetDuration(20.Seconds())
            .AddInput(8, Ids.Products.CopperScrap, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddInput(8, Ids.Products.ChilledWater, "D")
            .AddOutput(8, Ids.Products.MoltenCopper, "*")
            .AddOutput(6, Ids.Products.SteamDepleted, "Z")
            .BuildAndAdd();
            pReg.RecipeProtoBuilder.Start("Glass broken smelting (arc cold)", 
                FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken, Ids.Machines.ArcFurnace2)
            .SetDuration(20.Seconds())
            .AddInput(12, Ids.Products.BrokenGlass, "*")
            .AddInput(1, Ids.Products.Graphite, "*")
            .AddInput(8, Ids.Products.ChilledWater, "D")
            .AddOutput(8, Ids.Products.MoltenGlass, "*")
            .AddOutput(6, Ids.Products.SteamDepleted, "Z")
            .BuildAndAdd();

            // Cold Exhaust Scrubbing Recipe
            pReg.RecipeProtoBuilder.Start("Exhaust filtering (cold)", 
                FFU_IC_IDs.Recipes.ExhaustFilteringCold, Ids.Machines.ExhaustScrubber)
            .SetDuration(10.Seconds())
            .AddInput(30, Ids.Products.Exhaust, "*")
            .AddInput(4, Ids.Products.ChilledWater, "*")
            .AddOutput(1, Ids.Products.Sulfur, "Z")
            .AddOutput(12, Ids.Products.CarbonDioxide, "X")
            .AddOutput(4, Ids.Products.SteamDepleted, "Y")
            .BuildAndAdd();

            // Graphite-Coal Shredding Recipe
            pReg.RecipeProtoBuilder.Start("Shredding graphite", 
                FFU_IC_IDs.Recipes.GraphiteCoalShredding, Ids.Machines.Shredder)
            .SetDuration(10.Seconds())
            .AddInput(10, Ids.Products.Graphite, "*")
            .AddOutput(5, Ids.Products.Coal, "*")
            .BuildAndAdd();

            // Vacuum Pumping Advanced Recipes
            pReg.RecipeProtoBuilder.Start("Ocean vacuum pumping", 
                FFU_IC_IDs.Recipes.OceanVacuumPumping, Ids.Machines.OceanWaterPumpT1)
            .SetDuration(10.Seconds())
            .AddOutput(36, Ids.Products.Seawater, "X")
            .BuildAndAddBypass();
            pReg.RecipeProtoBuilder.Start("Ocean vacuum pumping II", 
                FFU_IC_IDs.Recipes.OceanVacuumPumpingT2, Ids.Machines.OceanWaterPumpLarge)
            .SetDuration(10.Seconds())
            .AddOutput(36, Ids.Products.Seawater, "X")
            .BuildAndAddBypass();

            // Vacuum Desalination Advanced Recipes
            pReg.RecipeProtoBuilder.Start("Vacuum desalination", 
                FFU_IC_IDs.Recipes.DesalinationVacuumSP, Ids.Machines.ThermalDesalinator)
            .SetDuration(10.Seconds())
            .AddInput(54, Ids.Products.Seawater, "A")
            .AddInput(1, Ids.Products.SteamSp, "B")
            .AddOutput(48, Ids.Products.Water, "W")
            .AddOutput(7, Ids.Products.Brine, "X")
            .BuildAndAddBypass();
            pReg.RecipeProtoBuilder.Start("Vacuum desalination",
                FFU_IC_IDs.Recipes.DesalinationVacuumHP, Ids.Machines.ThermalDesalinator)
            .SetDuration(10.Seconds())
            .AddInput(36, Ids.Products.Seawater, "A")
            .AddInput(2, Ids.Products.SteamHi, "B")
            .AddOutput(31, Ids.Products.Water, "W")
            .AddOutput(7, Ids.Products.Brine, "X")
            .BuildAndAddBypass();
            pReg.RecipeProtoBuilder.Start("Vacuum desalination",
                FFU_IC_IDs.Recipes.DesalinationVacuumLP, Ids.Machines.ThermalDesalinator)
            .SetDuration(10.Seconds())
            .AddInput(24, Ids.Products.Seawater, "A")
            .AddInput(4, Ids.Products.SteamLo, "B")
            .AddOutput(24, Ids.Products.Water, "W")
            .AddOutput(4, Ids.Products.Brine, "X")
            .BuildAndAddBypass();

            // Gas Boiler Super Steam Recipes
            pReg.RecipeProtoBuilder.Start("Super steam generation",
                FFU_IC_IDs.Recipes.SuperGenerationFuelGas, Ids.Machines.BoilerGas)
            .SetDuration(10.Seconds())
            .AddInput(4, Ids.Products.Water, "B")
            .AddInput(8, Ids.Products.FuelGas, "A")
            .AddOutput(4, Ids.Products.SteamSp, "X")
            .AddOutput(8, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd();
            pReg.RecipeProtoBuilder.Start("Super steam generation",
                FFU_IC_IDs.Recipes.SuperGenerationHydrogen, Ids.Machines.BoilerGas)
            .SetDuration(10.Seconds())
            .AddInput(4, Ids.Products.Water, "B")
            .AddInput(8, Ids.Products.Hydrogen, "A")
            .AddOutput(4, Ids.Products.SteamSp, "X")
            .AddOutput(4, Ids.Products.SteamDepleted, "Y")
            .BuildAndAdd();

        }
        public void ExampleUse() {
            // Recipe Modifications
            ModifyRecipeTime(Ids.Recipes.SiliconSmeltingArc, 15.Seconds());
            ModifyRecipeInput(Ids.Recipes.SiliconSmeltingArc, Ids.Products.Sand, 6);
            ModifyRecipeOutput(Ids.Recipes.SiliconSmeltingArc, Ids.Products.Slag, 2);
        }
    }
}