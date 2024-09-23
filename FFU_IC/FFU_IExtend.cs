using Mafi.Core.Factory.Recipes;

namespace FFU_Industrial_Lib {
    public static class FFU_IExtend {
        /// <remarks>
        /// Same as <b>BuildAndAdd</b> for <b>RecipeProto</b>, but ignores recipe similarity validation.<br/><br/>
        /// 
        /// Required, if you intend to add additional recipe variants with same inputs/outputs, but different throughput.
        /// </remarks>
        public static RecipeProto BuildAddBypass(this RecipeProtoBuilder.State buildState) {
            buildState.verifyRecipeIo();
            RecipeProto recipeProto = buildState.AddToDb(new RecipeProto(buildState.m_protoId, 
                buildState.Strings, buildState.ValueOrThrow(buildState.m_duration, "Duration"), 
                buildState.m_inputs.ToImmutableArray(), buildState.m_outputs.ToImmutableArray(), 
                buildState.m_minimumUtilization, buildState.m_productsDestroyReason, 
                buildState.m_disableSourceProductsConversionLoss));
            buildState.m_machine.m_recipes.Add(recipeProto);
            return recipeProto;
        }
        /// <remarks>
        /// Same as <b>BuildAndAdd</b> for <b>RecipeProto</b>, but sorts all the recipes in the relevant <b>MachineProto</b>.<br/><br/>
        /// 
        /// Allows to keep recipe list sorted, when you add new recipes to the list.
        /// </remarks>
        public static RecipeProto BuildAddSortAll(this RecipeProtoBuilder.State buildState) {
            buildState.verifyRecipeIo();
            RecipeProto recipeProto = buildState.AddToDb(new RecipeProto(buildState.m_protoId,
                buildState.Strings, buildState.ValueOrThrow(buildState.m_duration, "Duration"),
                buildState.m_inputs.ToImmutableArray(), buildState.m_outputs.ToImmutableArray(),
                buildState.m_minimumUtilization, buildState.m_productsDestroyReason,
                buildState.m_disableSourceProductsConversionLoss));
            buildState.m_machine.AddRecipe(recipeProto);
            buildState.m_machine.m_recipes.Sort();
            return recipeProto;
        }
        /// <remarks>
        /// Same as <b>BuildAndAdd</b> for <b>RecipeProto</b>, but inserts the recipe in the relevant <b>MachineProto</b> at specific index.<br/><br/>
        /// 
        /// Allows to keep recipe list sorted manually, when you add new recipes to the list.
        /// </remarks>
        public static RecipeProto BuildAddAtIndex(this RecipeProtoBuilder.State buildState, int index = -1) {
            buildState.verifyRecipeIo();
            RecipeProto recipeProto = buildState.AddToDb(new RecipeProto(buildState.m_protoId,
                buildState.Strings, buildState.ValueOrThrow(buildState.m_duration, "Duration"),
                buildState.m_inputs.ToImmutableArray(), buildState.m_outputs.ToImmutableArray(),
                buildState.m_minimumUtilization, buildState.m_productsDestroyReason,
                buildState.m_disableSourceProductsConversionLoss));
            buildState.m_machine.AddRecipe(recipeProto);
            if (index >= 0) {
                buildState.m_machine.m_recipes.Remove(recipeProto);
                buildState.m_machine.m_recipes.Insert(index, recipeProto);
            }
            return recipeProto;
        }
    }
}
