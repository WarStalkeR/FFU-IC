using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Recipes : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions

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
        public void ModifyRecipeInput(RecipeProto refRecipe) {
            foreach (RecipeInput refInput in refRecipe.AllInputs) {
                ModLog.Info($"Recipe: {refRecipe.Id}, Index {refRecipe.AllInputs.IndexOf(refInput)}, Input: {refInput.Product.Id}, Quantity: {refInput.Quantity.Value}");
            }
        }
        public void ModifyRecipeOutput(RecipeProto refRecipe) {
            foreach (RecipeOutput refOutput in refRecipe.AllOutputs) {
                ModLog.Info($"Recipe: {refRecipe.Id}, Index {refRecipe.AllOutputs.IndexOf(refOutput)}, Output: {refOutput.Product.Id}, Quantity: {refOutput.Quantity.Value}");
            }
        }

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;

            // Recipe References
            RecipeProto tempRecipe = FFU_IC_IDs.RecipeRef(pReg, Ids.Recipes.SiliconSmeltingArc);
            ModLog.Info($"BEFORE: {tempRecipe.Duration.Seconds}");
            ModifyRecipeTime(tempRecipe, 15.Seconds());
            ModLog.Info($"AFTER: {tempRecipe.Duration.Seconds}");
            ModifyRecipeInput(tempRecipe);
            ModifyRecipeOutput(tempRecipe);
        }
    }
}