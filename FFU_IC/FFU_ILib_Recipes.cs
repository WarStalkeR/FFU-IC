using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using System.Reflection;

namespace FFU_Industrial_Lib {
    public static partial class FFU_ILib {
        public static void ModifyRecipeTime(RecipeProto.ID refRecipeID, Duration newTime) {
            if (_pReg == null) { ModLog.Warning($"ModifyRecipeTime: the ProtoRegistrator is not referenced!"); return; };
            RecipeProto refRecipe = RecipeRef(refRecipeID);
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeTime: 'refRecipe' is undefined!"); return; }
            ModLog.Info($"{refRecipe.Id} Production Time: {refRecipe.Duration.Seconds}s -> {newTime.Seconds}s");
            TypeInfo typeProto = typeof(RecipeProto).GetTypeInfo();
            FieldInfo fieldDuration = typeProto.GetDeclaredField("<Duration>k__BackingField");
            if (fieldDuration != null) {
                fieldDuration.SetValue(refRecipe, newTime);
                SyncProtoMod(refRecipe);
            }
        }

        public static void ModifyRecipeInput(RecipeProto.ID refRecipeID, ProductProto.ID refProductID, int newAmount) {
            if (_pReg == null) { ModLog.Warning($"ModifyRecipeInput: the ProtoRegistrator is not referenced!"); return; };
            RecipeProto refRecipe = RecipeRef(refRecipeID);
            ProductProto refProduct = ProductRef(refProductID);
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeInput: 'refRecipe' is undefined!"); return; }
            if (refProduct == null) { ModLog.Warning($"ModifyRecipeInput: 'refProduct' is undefined!"); return; }
            foreach (RecipeInput refInput in refRecipe.AllInputs) {
                if (refInput.Product.Id == refProduct.Id) {
                    ModLog.Info($"{refRecipe.Id} Input {refProduct.Id} Quantity: {refInput.Quantity.Value} -> {newAmount}");
                    FieldInfo fieldQuantity = typeof(RecipeProduct).GetField("Quantity", BindingFlags.Instance | BindingFlags.Public);
                    fieldQuantity.SetValue(refInput, new Quantity(newAmount));
                    SyncRecipeInternalVars(refRecipe);
                    SyncProtoMod(refRecipe);
                    break;
                }
            }
        }

        public static void ModifyRecipeOutput(RecipeProto.ID refRecipeID, ProductProto.ID refProductID, int newAmount) {
            if (_pReg == null) { ModLog.Warning($"ModifyRecipeOutput: the ProtoRegistrator is not referenced!"); return; };
            RecipeProto refRecipe = RecipeRef(refRecipeID);
            ProductProto refProduct = ProductRef(refProductID);
            if (refRecipe == null) { ModLog.Warning($"ModifyRecipeOutput: 'refRecipe' is undefined!"); return; }
            if (refProduct == null) { ModLog.Warning($"ModifyRecipeOutput: 'refProduct' is undefined!"); return; }
            foreach (RecipeOutput refOutput in refRecipe.AllOutputs) {
                if (refOutput.Product.Id == refProduct.Id) {
                    ModLog.Info($"{refRecipe.Id} Output {refProduct.Id} Quantity: {refOutput.Quantity.Value} -> {newAmount}");
                    FieldInfo fieldQuantity = typeof(RecipeProduct).GetField("Quantity", BindingFlags.Instance | BindingFlags.Public);
                    fieldQuantity.SetValue(refOutput, new Quantity(newAmount));
                    SyncRecipeInternalVars(refRecipe);
                    SyncProtoMod(refRecipe);
                    break;
                }
            }
        }

        private static void SyncRecipeInternalVars(RecipeProto refRecipe) {
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
    }
}