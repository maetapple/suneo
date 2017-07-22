using UnityEngine;

namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// SkeletonAssetの素材となるデータを集約します
    /// </summary>
    //----------------------------------------------------------------------------------------------
    [System.Serializable]
    public class SkeletonAsset
    {
        public const string SpriteShaderName = "Spine/Skeleton";
        public const string ImageShaderName  = "Spine/SkeletonGraphic (Premultiply Alpha)";

        [SerializeField] public string    ShaderName = SpriteShaderName;
        [SerializeField] public Texture2D Texture    = null;
        [SerializeField] public TextAsset Atlas      = null;
        [SerializeField] public TextAsset Skeleton   = null;


        private SkeletonAsset()
        { return; }

        public static SkeletonAsset Create( string shader, Texture2D texture, TextAsset atlas, TextAsset skeleton )
        {
            SkeletonAsset asset = new SkeletonAsset();
            asset.ShaderName = shader;
            asset.Texture    = texture;
            asset.Atlas      = atlas;
            asset.Skeleton   = skeleton;

            return asset;
        }

        /// <summary>
        /// Texture, Atlas, Skeleton いずれかが null なら false を返します
        /// </summary>
        public bool IsAvailable()
        {
            if ( this.Texture  == null ) return false;
            if ( this.Atlas    == null ) return false;
            if ( this.Skeleton == null ) return false;

            return true;
        }
    }
}