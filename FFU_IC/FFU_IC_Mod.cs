using Mafi;
using Mafi.Collections;
using Mafi.Core.Mods;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using FFU_Industrial_Lib;

namespace FFU_Industrial_Capacity {
    public static class FFU_IC_Base {
        public static string FullVersion = "0.1.4.0";
        public static IMod RefMod;
    }
    public sealed class FFU_IC_Mod : IMod {
		public string Name => "Industrial Capacity";
		public int Version => 2;
        public bool IsUiOnly => false;
        public Option<IConfig> ModConfig { get; }
        public FFU_IC_Mod() {
            ModLog.Info($"v{FFU_IC_Base.FullVersion}");
            FFU_IC_Base.RefMod = this;
        }
        public void Initialize(DependencyResolver resolver, bool gameWasLoaded) {
        }
        public void ChangeConfigs(Lyst<IConfig> configs) {
        }
        public void RegisterPrototypes(ProtoRegistrator registrator) {
            ModLog.Info($"Registering Prototypes...");
            // Industrial Lib Initialization
            FFU_ILib.ProtoReg = registrator;
            FFU_ILib.ModReference = FFU_IC_Base.RefMod;
            FFU_ILib.ModLoggerPrefix = Name;
            // 1st Stage Prototypes: Products
            registrator.RegisterData<FFU_IC_Mod_ProdSolid>();
            registrator.RegisterData<FFU_IC_Mod_ProdLoose>();
            registrator.RegisterData<FFU_IC_Mod_ProdFluid>();
            // 2nd Stage Prototypes: Other Data
            registrator.RegisterData<FFU_IC_Mod_Storages>();
            registrator.RegisterData<FFU_IC_Mod_Vehicles>();
            registrator.RegisterData<FFU_IC_Mod_Machinery>();
            registrator.RegisterData<FFU_IC_Mod_WorldSites>();
            // 3rd Stage Prototypes: Build Costs
            registrator.RegisterData<FFU_IC_Mod_BuildCosts>();
            // 4rd Stage Prototypes: Recipes
            registrator.RegisterData<FFU_IC_Mod_Recipes>();
            // 5th Stage Prototypes: Research
            registrator.RegisterData<FFU_IC_Mod_Research>();
            ModLog.Info($"Prototypes Registered!");
        }
        public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) {
        }
        public void InitializePatchers(DependencyResolver resolver) {
        }
        public void EarlyInit(DependencyResolver resolver) {
        }
    }
}