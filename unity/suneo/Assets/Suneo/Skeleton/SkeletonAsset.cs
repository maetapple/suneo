using UnityEngine;

namespace Suneo
{
    [System.Serializable]
    public class SkeletonAsset
    {
        public const string SpriteShaderName = "Spine/Skeleton";
        public const string ImageShaderName  = "Spine/SkeletonGraphic (Premultiply Alpha)";

        [SerializeField] public string    shader   = SpriteShaderName;
        [SerializeField] public Texture2D texture  = null;
        [SerializeField] public TextAsset atlas    = null;
        [SerializeField] public TextAsset skeleton = null;

        // public string    ShaderName { get{ return this.shader;   } }
        // public Texture2D Texture    { get{ return this.texture;  } }
        // public TextAsset Atlas      { get{ return this.atlas;    } }
        // public TextAsset Skeleton   { get{ return this.skeleton; } }

        private SkeletonAsset(){}
        public static SkeletonAsset Create( string shader, Texture2D texture, TextAsset atlas, TextAsset skeleton )
        {
            SkeletonAsset asset = new SkeletonAsset();
            asset.shader   = shader;
            asset.texture  = texture;
            asset.atlas    = atlas;
            asset.skeleton = skeleton;

            return asset;
        }
    }
}
