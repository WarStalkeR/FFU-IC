using Mafi;
using Mafi.Collections;
using Mafi.Core.Mods;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;

namespace FFU_Industrial_Capacity {
	public sealed class FFU_IC_Mod : IMod {
		public string Name => "Industrial Capacity";
		public int Version => 1;
        public bool IsUiOnly => false;
        public FFU_IC_Mod() {
            ModLog.Info($"Created!");
        }
        public void ChangeConfigs(Lyst<IConfig> configs) {
            ModLog.Info($"No configurable features.");
        }
        public void Initialize(DependencyResolver resolver, bool gameWasLoaded) {
        }
        public void RegisterPrototypes(ProtoRegistrator registrator) {
            ModLog.Info($"Registering prototypes...");
            registrator.RegisterData<FFU_IC_Mod_Storages>();
            registrator.RegisterData<FFU_IC_Mod_Vehicles>();
            registrator.RegisterData<FFU_IC_Mod_Research>();
            //registrator.RegisterData<FFU_IC_Mod_WorldMines>();
            ModLog.Info($"Prototypes registered!");
        }
        public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) {
            ModLog.Info($"No specific dependencies.");
        }
    }
}