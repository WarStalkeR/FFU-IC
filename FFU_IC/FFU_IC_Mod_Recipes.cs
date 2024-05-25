using Mafi;
using Mafi.Base;
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
                    FFU_IC_IDs.SyncProtoMod(refRecipe);
                    break;
                }
            }
        }
        public void SyncRecipeProcedures(RecipeProto refRecipe) {
            if (refRecipe == null) { ModLog.Warning($"SyncRecipeProcedures: 'refRecipe' is undefined!"); return; }
            TypeInfo typeProto = typeof(RecipeProto).GetTypeInfo();
            FieldInfo fieldAllUserVisibleInputs = typeProto.GetDeclaredField("<AllUserVisibleInputs>k__BackingField");
            FieldInfo fieldAllUserVisibleOutputs = typeProto.GetDeclaredField("<AllUserVisibleOutputs>k__BackingField");
            FieldInfo fieldOutputsAtEnd = typeProto.GetField("OutputsAtEnd", BindingFlags.Instance | BindingFlags.Public);
            FieldInfo fieldOutputsAtStart = typeProto.GetField("OutputsAtStart", BindingFlags.Instance | BindingFlags.Public);
            fieldAllUserVisibleInputs.SetValue(refRecipe, refRecipe.AllInputs.Filter((RecipeInput x) => !x.HideInUi));
            fieldAllUserVisibleOutputs.SetValue(refRecipe, refRecipe.AllOutputs.Filter((RecipeOutput x) => !x.HideInUi));
            fieldOutputsAtEnd.SetValue(refRecipe, refRecipe.AllOutputs.Filter((RecipeOutput x) => !x.TriggerAtStart));
            fieldOutputsAtStart.SetValue(refRecipe, refRecipe.AllOutputs.Filter((RecipeOutput x) => x.TriggerAtStart));
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // Machinery References
            MachineProto arcFurnaceT1 = McRef(Ids.Machines.ArcFurnace);
            MachineProto arcFurnaceT2 = McRef(Ids.Machines.ArcFurnace2);

            // Arc Furnace Half Scrap Recipes
            pReg.RecipeProtoBuilder
            .Start("Iron scrap smelting (arc half)", FFU_IC_IDs.Recipes.IronSmeltingArcHalfScrap, arcFurnaceT1)
            .AddInput(8, Ids.Products.IronScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenIron, "*", false, false)
            .AddOutput(2, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Copper scrap smelting (arc half)", FFU_IC_IDs.Recipes.CopperSmeltingArcHalfScrap, arcFurnaceT1)
            .AddInput(8, Ids.Products.CopperScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenCopper, "*", false, false)
            .AddOutput(2, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Glass broken smelting (arc half)", FFU_IC_IDs.Recipes.GlassSmeltingArcHalfWithBroken, arcFurnaceT1)
            .AddInput(12, Ids.Products.BrokenGlass, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenGlass, "*", false, false)
            .AddOutput(2, Ids.Products.Exhaust, "E", false, false)
            .BuildAndAdd();

            // Arc Furnace Cold Scrap Recipes
            pReg.RecipeProtoBuilder
            .Start("Iron scrap smelting (arc cold)", FFU_IC_IDs.Recipes.IronSmeltingArcColdScrap, arcFurnaceT2)
            .AddInput(8, Ids.Products.IronScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(8, Ids.Products.ChilledWater, "D", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenIron, "*", false, false)
            .AddOutput(6, Ids.Products.SteamDepleted, "Z", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Copper scrap smelting (arc cold)", FFU_IC_IDs.Recipes.CopperSmeltingArcColdScrap, arcFurnaceT2)
            .AddInput(8, Ids.Products.CopperScrap, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(8, Ids.Products.ChilledWater, "D", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenCopper, "*", false, false)
            .AddOutput(6, Ids.Products.SteamDepleted, "Z", false, false)
            .BuildAndAdd();
            pReg.RecipeProtoBuilder
            .Start("Glass broken smelting (arc cold)", FFU_IC_IDs.Recipes.GlassSmeltingArcColdWithBroken, arcFurnaceT2)
            .AddInput(12, Ids.Products.BrokenGlass, "*", false)
            .AddInput(1, Ids.Products.Graphite, "*", false)
            .AddInput(8, Ids.Products.ChilledWater, "D", false)
            .SetDuration(20.Seconds())
            .AddOutput(8, Ids.Products.MoltenGlass, "*", false, false)
            .AddOutput(6, Ids.Products.SteamDepleted, "Z", false, false)
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