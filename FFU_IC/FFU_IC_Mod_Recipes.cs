using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using System.Collections.Generic;
using System.Reflection;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_Recipes : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Modification Definitions

        // Localization Definitions

        // Reflection Helpers

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
        }
    }
}