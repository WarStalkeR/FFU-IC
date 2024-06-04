using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using System.Reflection;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Recipes : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Reference Helpers
        private RecipeProto RcRef(RecipeProto.ID refID) => FFU_IC_IDs.RecipeRef(pReg, refID);
        private ProductProto PdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);
        private MachineProto McRef(MachineProto.ID refID) => FFU_IC_IDs.MachineRef(pReg, refID);

        // Reflection Helpers
        public void ModifyRecipeTime(RecipeProto refRecipe, Duration newTime) {
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeTime: 'refRecipe' is undefined!"); return; }
            ModLog.Info($"{refRecipe.Id} Production Time: {refRecipe.Duration.Seconds}s -> {newTime.Seconds}s");
            TypeInfo typeProto = typeof(RecipeProto).GetTypeInfo();
            FieldInfo fieldDuration = typeProto.GetDeclaredField("<Duration>k__BackingField");
            if (fieldDuration != null) {
                fieldDuration.SetValue(refRecipe, newTime);
                SyncRecipeInternalVars(refRecipe);
                FFU_IC_IDs.SyncProtoMod(refRecipe);
            }
        }
        public void ModifyRecipeInput(RecipeProto refRecipe, ProductProto refProduct, int newAmount) {
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
        public void ModifyRecipeOutput(RecipeProto refRecipe, ProductProto refProduct, int newAmount) {
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
        public void SyncRecipeInternalVars(RecipeProto refRecipe) {
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

            // Recipe References
            RecipeProto conHiSteamT1 = RcRef(Ids.Recipes.SteamHpCondensation);
            RecipeProto conLoSteamT1 = RcRef(Ids.Recipes.SteamLpCondensation);
            RecipeProto conDepSteamT1 = RcRef(Ids.Recipes.SteamDepletedCondensation);
            RecipeProto conDepSteamT2 = RcRef(Ids.Recipes.SteamDepletedCondensationT2);

            // Product References
            ProductProto refWater = PdRef(Ids.Products.Water);

            // Rebalanced Steam Cooling Efficiency
            ModifyRecipeOutput(conHiSteamT1, refWater, 1);
            ModifyRecipeOutput(conDepSteamT1, refWater, 3);
            ModifyRecipeOutput(conDepSteamT2, refWater, 14);

            // Arc Furnace Half Scrap Recipes
            pReg.RecipeProtoBuilder
            .Start("Iron scrap smelting (arc half)", FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, Ids.Machines.ArcFurnace)
            .AddInput(8, Ids.Products.IronScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenIron, "*", false, false)
            .AddOutput(2, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Copper scrap smelting (arc half)", FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, Ids.Machines.ArcFurnace)
            .AddInput(8, Ids.Products.CopperScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenCopper, "*", false, false)
            .AddOutput(2, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Glass broken smelting (arc half)", FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, Ids.Machines.ArcFurnace)
            .AddInput(12, Ids.Products.BrokenGlass, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenGlass, "*", false, false)
            .AddOutput(2, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();

            // Arc Furnace Cold Scrap Recipes
            pReg.RecipeProtoBuilder
            .Start("Iron scrap smelting (arc cold)", FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap, Ids.Machines.ArcFurnace2)
            .AddInput(8, Ids.Products.IronScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(8, Ids.Products.ChilledWater, "D", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenIron, "*", false, false)
            .AddOutput(6, Ids.Products.SteamDepleted, "Z", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Copper scrap smelting (arc cold)", FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap, Ids.Machines.ArcFurnace2)
            .AddInput(8, Ids.Products.CopperScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(8, Ids.Products.ChilledWater, "D", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenCopper, "*", false, false)
            .AddOutput(6, Ids.Products.SteamDepleted, "Z", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Glass broken smelting (arc cold)", FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken, Ids.Machines.ArcFurnace2)
            .AddInput(12, Ids.Products.BrokenGlass, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(8, Ids.Products.ChilledWater, "D", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenGlass, "*", false, false)
            .AddOutput(6, Ids.Products.SteamDepleted, "Z", false, false)
            .BuildAndAdd();

            // Cold Exhaust Scrubbing Recipe
            pReg.RecipeProtoBuilder.Start("Exhaust filtering (cold)", FFU_IC_IDs.Recipes.ExhaustFilteringCold, Ids.Machines.ExhaustScrubber)
            .AddInput(30, Ids.Products.Exhaust, "*", false)
            .AddInput(4, Ids.Products.ChilledWater, "*", false)
            .SetDuration(10.Seconds())
            .AddOutput(1, Ids.Products.Sulfur, "Z", false, false)
            .AddOutput(12, Ids.Products.CarbonDioxide, "X", false, false)
            .AddOutput(4, Ids.Products.SteamDepleted, "Y", false, false)
            .BuildAndAdd();

            // Graphite-Coal Shredding Recipe
            pReg.RecipeProtoBuilder.Start("Shredding graphite", FFU_IC_IDs.Recipes.GraphiteCoalShredding, Ids.Machines.Shredder)
            .AddInput(10, Ids.Products.Graphite, "*", false)
            .SetDuration(10.Seconds())
            .AddOutput(5, Ids.Products.Coal, "*", false, false)
            .BuildAndAdd();
        }
        public void ExampleUse() {
            // Recipe References
            RecipeProto tempRecipe = RcRef(Ids.Recipes.SiliconSmeltingArc);

            // Recipe Modifications
            ModifyRecipeTime(tempRecipe, 15.Seconds());
            ModifyRecipeInput(tempRecipe, PdRef(Ids.Products.Sand), 6);
            ModifyRecipeOutput(tempRecipe, PdRef(Ids.Products.Slag), 2);
        }
    }
}