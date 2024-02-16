using Mafi.Core.Mods;
using Mafi.Core.Products;

namespace FFU_Industrial_Capacity {
	internal partial class FFU_IC_Mod_ProdSolid : IModData {
        // Modification Variables
        ProtoRegistrator pReg = null;

        // Reflection Helpers
        public ProductProto PdRef(ProductProto.ID refID) => FFU_IC_IDs.ProductRef(pReg, refID);

        public void RegisterData(ProtoRegistrator registrator) {
            // Variables Initialization
            pReg = registrator;
        }
    }
}