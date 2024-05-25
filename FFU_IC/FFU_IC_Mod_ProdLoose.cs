using Mafi.Core.Mods;
using Mafi.Core.Products;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_ProdLoose : IModData {
        // Modification Variables
        private ProtoRegistrator pReg = null;

        // Reference Helpers
        private ProductProto PdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);

        // Reflection Helpers

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
        }
    }
}