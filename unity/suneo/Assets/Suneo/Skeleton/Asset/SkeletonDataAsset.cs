using UnityEngine;

namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Spine.Unity.SkeletonDataAsset のラッパーです
    /// - SkeletonAsset にて初期化します
    /// - Material, AtlasAssetを１つだけ扱うSkeletonDataAssetとして生成します
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class SkeletonDataAsset
    {
        //=== Field
        private Spine.Unity.SkeletonDataAsset dataAsset = null;


        //=== Initialization

        private SkeletonDataAsset()
        { return; }

        public static SkeletonDataAsset Create( SkeletonAsset asset )
        {
            SkeletonDataAsset data = new SkeletonDataAsset();
            data.InitDataAsset(asset);

            return data;
        }


        //=== Accessor

        /// <summary>
        /// 正常に初期化されていない場合 null を返します
        /// <_summary>
        public Spine.Unity.SkeletonDataAsset GetDataAsset()
        {
            return this.dataAsset;
        }

        /// <summary>
        /// 正常に初期化されていない場合 null を返します
        /// <_summary>
        public Spine.Unity.AtlasAsset GetAtlasAsset()
        {
            return this.GetDataAsset().atlasAssets[0];
        }

        /// <summary>
        /// 正常に初期化されていない場合 null を返します
        /// <_summary>
        public Material GetMaterial()
        {
            return this.GetAtlasAsset().materials[0];
        }



        //=== Make assets

        private void InitDataAsset( SkeletonAsset asset )
        {
            Material material = this.MakeMaterial(asset.ShaderName, asset.Texture);

            if ( material == null )
            {
                material = this.MakeMaterial(SkeletonAsset.SpriteShaderName, asset.Texture);
            }

            Spine.Unity.AtlasAsset        atlasAsset = this.MakeAtlasAsset(asset.Atlas, material);
            Spine.Unity.SkeletonDataAsset dataAsset  = this.MakeDataAsset(asset.Skeleton, atlasAsset);

            this.dataAsset = dataAsset;
        }


        private Material MakeMaterial( string shaderName, Texture2D texture )
        {
            Shader shader = Shader.Find(shaderName);

            if ( shader == null )
                return null;

            Material mat = new Material(shader);
            mat.mainTexture = texture;

            return mat;
        }


        private Spine.Unity.AtlasAsset MakeAtlasAsset( TextAsset atlas, Material material )
        {
            Material[] materials  = new Material[1]{ material };
            bool       initialize = true;

            return Spine.Unity.AtlasAsset.CreateRuntimeInstance(atlas, materials, initialize);
        }


        private Spine.Unity.SkeletonDataAsset MakeDataAsset( TextAsset              skeleton, 
                                                             Spine.Unity.AtlasAsset atlas,
                                                             float                  scale=0.01f )
        {
            bool initialize = true;

            return Spine.Unity.SkeletonDataAsset.CreateRuntimeInstance(skeleton, atlas, initialize, scale);
        }
        
    }
}