
#if false
namespace Suneo.Resource
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resource.Pack で参照するResourceのPathを集約します.
    /// - `Texture`  "xxx/spine.png"
    /// - `Atlas`    "xxx/spine.atlas"
    /// - `Skeleton` "xxx/spine.json" or "xxx/spine.skel"
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Path
    {
        //=== Variables
        public string Texture  { get; private set; }
        public string Atlas    { get; private set; }
        public string Skeleton { get; private set; }
        public string Shader   { get; private set; }

        //=== Initialization

        /// <summary>
        /// 各 file path を指定してください
        /// </summary>
        public static Path Create( string texture, string atlas, string skeleton, string shader)
        {
            return new Path(texture, atlas, skeleton, shader);
        }

        /// <summary>
        /// 各 file path を指定してください
        /// </summary>
        private Path( string texture, string atlas, string skeleton, string shader )
        {
            this.Texture    = texture;
            this.Atlas      = atlas;
            this.Skeleton   = skeleton;
            this.Shader     = shader;
        }

        //=== Path

        /// <summary>
        /// Texture Path を基に Unityにimportしたときに生成されるAtlasAssetへのPathを作成して返します
        /// - `Character-01` => `Character-01_Atlas`
        /// </summary>
        public string CreateToAtlasAsset()
        {
            return this.Texture + "_Atlas";
        }

        /// <summary>
        /// Texture Path を基に Unityにimportしたときに生成されるMaterialへのPathを作成して返します
        /// - `Character-01` => `Character-01_Material`
        /// </summary>
        public string CreateToMaterial()
        {
            return this.Texture + "_Material";
        }

        /// <summary>
        /// Texture Path を基に Unityにimportしたときに生成されるSkeletonDataAssetへのPathを作成して返します
        /// - `Character-01` => `Character-01_SkeletonData`
        /// </summary>
        public string CreateToSkeletonDataAsset()
        {
            return this.Texture + "_SkeletonData";
        }
    }
}
#endif