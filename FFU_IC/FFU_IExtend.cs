using Mafi.Core.Factory.Recipes;

namespace FFU_Industrial_Lib {
    public static class FFU_IExtend {
        /// <remarks>
        /// Same as <b>BuildAndAdd</b> for <b>RecipeProto</b>, but ignores recipe similarity validation.<br/><br/>
        /// 
        /// Required, if you intend to add additional recipe variants with same inputs/outputs, but different throughput.
        /// </remarks>
        public static RecipeProto BuildAndAddBypass(this RecipeProtoBuilder.State buildState) {
            buildState.verifyRecipeIo();
            RecipeProto recipeProto = buildState.AddToDb(new RecipeProto(buildState.m_protoId, 
                buildState.Strings, buildState.ValueOrThrow(buildState.m_duration, "Duration"), 
                buildState.m_inputs.ToImmutableArray(), buildState.m_outputs.ToImmutableArray(), 
                buildState.m_minimumUtilization, buildState.m_productsDestroyReason, 
                buildState.m_disableSourceProductsConversionLoss));
            buildState.m_machine.m_recipes.Add(recipeProto);
            return recipeProto;
        }
    }
}
