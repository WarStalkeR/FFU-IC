using Mafi;
using Mafi.Collections;
using Mafi.Core.Mods;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;

namespace FFU_Industrial_Capacity {
    public static class FFU_IC_Base {
        public static string Version = "0.1.3.0";
    }
    public sealed class FFU_IC_Mod : IMod {
		public string Name => "Industrial Capacity";
		public int Version => 1;
        public bool IsUiOnly => false;
        public FFU_IC_Mod() {
            ModLog.Info($"v{FFU_IC_Base.Version}");
        }
        public void ChangeConfigs(Lyst<IConfig> configs) {
        }
        public void Initialize(DependencyResolver resolver, bool gameWasLoaded) {
        }
        public void RegisterPrototypes(ProtoRegistrator registrator) {
            ModLog.Info($"Registering Prototypes...");
            registrator.RegisterData<FFU_IC_Mod_Storages>();
            registrator.RegisterData<FFU_IC_Mod_Vehicles>();
            registrator.RegisterData<FFU_IC_Mod_Machinery>();
            registrator.RegisterData<FFU_IC_Mod_WorldSites>();
            registrator.RegisterData<FFU_IC_Mod_Recipes>();
            registrator.RegisterData<FFU_IC_Mod_Research>();
            ModLog.Info($"Prototypes Registered!");
        }
        public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) {
        }
    }
}