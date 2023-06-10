using Mafi;
using Mafi.Collections;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;

namespace FFU_Industrial_Capacity {
	public sealed class FFU_IC_Mod : IMod {
		public string Name => "Industrial Capacity";
		public int Version => 1;
        public bool IsUiOnly => false;
        public FFU_IC_Mod() {
			Log.Info($"{Name}: Created!");
        }
        public void ChangeConfigs(Lyst<IConfig> configs) {
            Log.Info($"{Name}: No configurable features.");
        }
        public void Initialize(DependencyResolver resolver, bool gameWasLoaded) {
        }
        public void RegisterPrototypes(ProtoRegistrator registrator) {
            Log.Info($"{Name}: Registering prototypes...");
            registrator.RegisterData<FFU_IC_Mod_Vehicles>();
            registrator.RegisterData<FFU_IC_Mod_Research>();
            //registrator.RegisterData<FFU_IC_Mod_Zero_Fix>();
            Log.Info($"{Name}: Prototypes registered!");
        }
        public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) {
            Log.Info($"{Name}: No specific dependencies.");
        }
    }
}