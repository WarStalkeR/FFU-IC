using Mafi.Core.Mods;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Storages : IModData {
        //Default Capacity
        public const int storageCapT1 = 600;
        public const int storageCapT2 = 1800;
        public const int storageCapT3 = 9000;
        public const int storageCapT4 = 27000;
        //Nuclear Capacity
        public const int radWasteCap = 12000;
        public const int safeWasteCap = 3000;
        //Thermal Capacity
        public const int thermalCap = 15000;
        public void RegisterData(ProtoRegistrator registrator) {
        }
    }
}