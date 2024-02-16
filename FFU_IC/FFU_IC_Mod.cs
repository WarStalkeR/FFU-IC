using Mafi;
using Mafi.Collections;
using Mafi.Core.Mods;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;

namespace FFU_Industrial_Capacity {
    public static class FFU_IC_Base {
        public static string FullVersion = "0.1.3.0";
        public static IMod RefMod;
    }
    public sealed class FFU_IC_Mod : IMod {
		public string Name => "Industrial Capacity";
		public int Version => 1;
        public bool IsUiOnly => false;
        public FFU_IC_Mod() {
            ModLog.Info($"v{FFU_IC_Base.FullVersion}");
            FFU_IC_Base.RefMod = this;
        }
        public void ChangeConfigs(Lyst<IConfig> configs) {
        }
        public void Initialize(DependencyResolver resolver, bool gameWasLoaded) {
        }
        public void RegisterPrototypes(ProtoRegistrator registrator) {
            ModLog.Info($"Registering Prototypes...");
            // 1st Stage Prototypes: Products
            registrator.RegisterData<FFU_IC_Mod_ProdSolid>();
            registrator.RegisterData<FFU_IC_Mod_ProdLoose>();
            registrator.RegisterData<FFU_IC_Mod_ProdFluid>();
            // 2nd Stage Prototypes: Other Data
            registrator.RegisterData<FFU_IC_Mod_Storages>();
            registrator.RegisterData<FFU_IC_Mod_Vehicles>();
            registrator.RegisterData<FFU_IC_Mod_Machinery>();
            registrator.RegisterData<FFU_IC_Mod_WorldSites>();
            // 3rd Stage Prototypes: Recipes
            registrator.RegisterData<FFU_IC_Mod_Recipes>();
            // 4th Stage Prototypes: Research
            registrator.RegisterData<FFU_IC_Mod_Research>();
            ModLog.Info($"Prototypes Registered!");
        }
        public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) {
        }
    }
}