using Mafi.Base;
using Mafi.Core.Mods;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Research : IResearchNodesData, IModData {
        //Research Capacity Bonus
        public const int vehCapLimit = 40;
        public void RegisterData(ProtoRegistrator registrator) {
            Register_ResearchVC(registrator); //Mafi.Base.Prototypes.Research.LogisticsResearchData
        }
    }
}