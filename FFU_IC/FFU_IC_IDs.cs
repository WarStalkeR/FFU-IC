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

            // Missing T1 Robotics Assembly Recipes
            public static readonly RecipeProto.ID UraniumRodsAssemblyT4 = new RecipeProto.ID("UraniumRodsAssemblyT4");
            public static readonly RecipeProto.ID MechPartsAssemblyT4Iron = new RecipeProto.ID("MechPartsAssemblyT4Iron");

            // Missing T2 Robotics Assembly Recipes
            public static readonly RecipeProto.ID UraniumRodsAssemblyT5 = new RecipeProto.ID("UraniumRodsAssemblyT5");
            public static readonly RecipeProto.ID UraniumEnrichedAssemblyT5 = new RecipeProto.ID("UraniumEnrichedAssemblyT5");
            public static readonly RecipeProto.ID MechPartsAssemblyT5Iron = new RecipeProto.ID("MechPartsAssemblyT5Iron");
            public static readonly RecipeProto.ID MechPartsAssemblyT5Steel = new RecipeProto.ID("MechPartsAssemblyT5Steel");

            // Arc Furnace Half Scrap Recipes
            public static readonly RecipeProto.ID IronSmeltingArcHalfScrap = new RecipeProto.ID("IronSmeltingArcHalfScrap");
            public static readonly RecipeProto.ID CopperSmeltingArcHalfScrap = new RecipeProto.ID("CopperSmeltingArcHalfScrap");
            public static readonly RecipeProto.ID GlassSmeltingArcHalfWithBroken = new RecipeProto.ID("GlassSmeltingArcHalfWithBroken");

            // Arc Furnace Cold Scrap Recipes
            public static readonly RecipeProto.ID IronSmeltingArcColdScrap = new RecipeProto.ID("IronSmeltingArcColdScrap");
            public static readonly RecipeProto.ID CopperSmeltingArcColdScrap = new RecipeProto.ID("CopperSmeltingArcColdScrap");
            public static readonly RecipeProto.ID GlassSmeltingArcColdWithBroken = new RecipeProto.ID("GlassSmeltingArcColdWithBroken");

            // Cold Exhaust Scrubbing Recipe
            public static readonly RecipeProto.ID ExhaustFilteringCold = new RecipeProto.ID("ExhaustFilteringCold");

            // Graphite-Coal Shredding Recipe
            public static readonly RecipeProto.ID GraphiteCoalShredding = new RecipeProto.ID("GraphiteCoalShredding");

            // Vacuum Pumping Advanced Recipes
            public static readonly RecipeProto.ID OceanVacuumPumping = new RecipeProto.ID("OceanVacuumPumping");
            public static readonly RecipeProto.ID OceanVacuumPumpingT2 = new RecipeProto.ID("OceanVacuumPumpingT2");

            // Vacuum Desalination Advanced Recipes
            public static readonly RecipeProto.ID DesalinationVacuumLP = new RecipeProto.ID("DesalinationVacuumLP");
            public static readonly RecipeProto.ID DesalinationVacuumHP = new RecipeProto.ID("DesalinationVacuumHP");
            public static readonly RecipeProto.ID DesalinationVacuumSP = new RecipeProto.ID("DesalinationVacuumSP");

            // Gas Boiler Super Steam Recipes
            public static readonly RecipeProto.ID SuperGenerationFuelGas = new RecipeProto.ID("SuperGenerationFuelGas");
            public static readonly RecipeProto.ID SuperGenerationHydrogen = new RecipeProto.ID("SuperGenerationHydrogen");
        }
        public static class Research {
            public static readonly ResearchNodeProto.ID None = new ResearchNodeProto.ID("None");
        }
    }
}