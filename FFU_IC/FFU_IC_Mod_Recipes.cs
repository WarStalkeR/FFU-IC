using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using System.Reflection;

namespace FFU_Industrial_Capacity {
    internal partial class FFU_IC_Mod_Recipes : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Reflection Helpers
        public RecipeProto RcRef(RecipeProto.ID refID) => FFU_IC_IDs.RecipeRef(pReg, refID);
        public ProductProto PdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);
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
            if (refRecipe == null) { ModLog.Warning($"SyncRecipeVariables: 'refRecipe' is undefined!"); return; }
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
            // ExampleUse();
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