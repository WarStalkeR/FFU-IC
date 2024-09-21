using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Research;

namespace FFU_Industrial_Capacity {
    public static class FFU_IC_IDs {
        public static class Buildings {
            public static readonly StaticEntityProto.ID None = new StaticEntityProto.ID("None");
        }
        public static class Machines {
            public static readonly MachineProto.ID None = new MachineProto.ID("None");
        }
        public static class Vehicles {
            public static readonly DynamicEntityProto.ID None = new DynamicEntityProto.ID("None");
        }
        public static class World {
            public static readonly EntityProto.ID None = new EntityProto.ID("None");
        }
        public static class Recipes {
            public static readonly RecipeProto.ID None = new RecipeProto.ID("None");
            public static readonly RecipeProto.ID IronSmeltingArcHalfScrap = new RecipeProto.ID("IronSmeltingArcHalfScrap");
            public static readonly RecipeProto.ID CopperSmeltingArcHalfScrap = new RecipeProto.ID("CopperSmeltingArcHalfScrap");
            public static readonly RecipeProto.ID GlassSmeltingArcHalfWithBroken = new RecipeProto.ID("GlassSmeltingArcHalfWithBroken");
            public static readonly RecipeProto.ID IronSmeltingArcColdScrap = new RecipeProto.ID("IronSmeltingArcColdScrap");
            public static readonly RecipeProto.ID CopperSmeltingArcColdScrap = new RecipeProto.ID("CopperSmeltingArcColdScrap");
            public static readonly RecipeProto.ID GlassSmeltingArcColdWithBroken = new RecipeProto.ID("GlassSmeltingArcColdWithBroken");
            public static readonly RecipeProto.ID ExhaustFilteringCold = new RecipeProto.ID("ExhaustFilteringCold");
            public static readonly RecipeProto.ID GraphiteCoalShredding = new RecipeProto.ID("GraphiteCoalShredding");
            public static readonly RecipeProto.ID OceanVacuumPumping = new RecipeProto.ID("OceanVacuumPumping");
            public static readonly RecipeProto.ID OceanVacuumPumpingT2 = new RecipeProto.ID("OceanVacuumPumpingT2");
            public static readonly RecipeProto.ID DesalinationVacuumLP = new RecipeProto.ID("DesalinationVacuumLP");
            public static readonly RecipeProto.ID DesalinationVacuumHP = new RecipeProto.ID("DesalinationVacuumHP");
            public static readonly RecipeProto.ID DesalinationVacuumSP = new RecipeProto.ID("DesalinationVacuumSP");
            public static readonly RecipeProto.ID SuperGenerationFuelGas = new RecipeProto.ID("SuperGenerationFuelGas");
            public static readonly RecipeProto.ID SuperGenerationHydrogen = new RecipeProto.ID("SuperGenerationHydrogen");
        }
        public static class Research {
            public static readonly ResearchNodeProto.ID None = new ResearchNodeProto.ID("None");
        }
    }
}