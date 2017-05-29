using UnityEngine;
using Spine.Unity;

namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Spine Skeletonを扱う各機能を集約したベースクラスです
    /// - サブクラスとしてWorldSpaceで扱う`Sprite`, Canvas上で扱う`Image` があります
    /// - `ISkeletonAnimator` Spine.Skeletonの派生クラスを扱うインターフェイス
    /// - `Resource`  初期化時に必要です
    /// - `Animation` Skeleton Animation Control
    /// - `Color`     Color設定
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public abstract class Skeleton : MonoBehaviour 
    {        
        //=== Variables

        private Material              material        = null;
        private MeshRenderer          meshRenderer    = null;
        private AtlasAsset            atlasAsset      = null;
        private SkeletonDataAsset     dataAsset       = null;
        private Animation.Controller  animController  = null;
        private Color.Controller      colorController = null;
        private ISkeletonAnimator     animator        = null;


        //=== Accessor

        public Material             GetMaterial()    { return this.material; }
        public MeshRenderer         GetMeshRenderer(){ return this.meshRenderer; }
        public AtlasAsset           GetAtlasAsset()  { return this.atlasAsset; }
        public SkeletonDataAsset    GetDataAsset()   { return this.dataAsset; }
        public Animation.Controller GetAnimation()   { return this.animController; }
        public Color.Controller     GetColor()       { return this.colorController; }
        
        private ISkeletonAnimator   GetSkeletonAnimator() { return this.animator; }


        //=== Initialization

        /// <summary>
        /// 新規GameObjectに<T>をAddComponentした形で生成したものを返します.
        /// Resource.PackによるSetup()も実行します.
        /// </summary>
        // public static T Create<T>( Resource.Pack pack ) where T : Skeleton
        // {
        //     GameObject go       = new GameObject(typeof(T).Name);
        //     T          skeleton = Skeleton.AddToGameObject<T>(go);

        //     skeleton.Setup(pack);

        //     return skeleton;
        // }

        public static Skeleton Create<T>( SkeletonAsset asset ) where T : Skeleton
        {
            GameObject go       = new GameObject(typeof(T).Name);
            Skeleton   skeleton = Skeleton.AddToGameObject<T>(go);

            skeleton.Setup(asset);

            return skeleton;
        }

        /// <summary>
        /// goにAddComponentした形で生成したものを返します.
        /// </summary>
        public static T AddToGameObject<T>( GameObject go ) where T : Skeleton
        {
            T model = go.AddComponent<T>();

            return model;            
        }

        /// <summary>
        /// 自信がハンドリングするGameObjectをDestoryします
        /// </summary>
        public void Destroy( float delay=0 )
        {
            GameObject.Destroy(this.gameObject, delay);
        }


        /// <summary>
        /// Load済みのResource.Packを渡してください.
        /// </summary>
        // public void Setup( Resource.Pack pack )
        // {
        //     if ( pack == null )
        //     {
        //         Debug.Assert(pack != null, "Skeleton: Need loaded Resource.Pack for setup.");
        //         return;
        //     }

        //     SkeletonAsset asset = SkeletonAsset.Create(pack.ShaderPath.Shader, pack.Texture, pack.Atlas, pack.Skeleton);
        //     this.Setup(asset);
        // }


        public void Setup( SkeletonAsset asset )
        {
            // Material
            this.material = this.CreateMaterial(asset.shader, asset.texture);
            
            if ( this.material == null )
            {
                this.material = this.CreateMaterial(SkeletonAsset.SpriteShaderName, asset.texture);
            }

            // AtlasAsset 
            this.atlasAsset = this.CreateAtlasAsset(asset.atlas, this.GetMaterial());

            // SkeletonDataAsset
            this.dataAsset = this.CreateSkeletonDataAsset(asset.skeleton, this.GetAtlasAsset());

            // @TODO MeshRenderer じゃなく Rendererとして扱えないか？
            // MeshRenderer
            // this.InitRenderer();

            // Sub Class             
            this.GetSkeletonAnimator().Init(this.GetDataAsset());

            // Animation Controller
            this.animController = Animation.Controller.Create(this.GetSkeletonAnimator().GetAnimationState());
            Debug.Assert(this.animController != null, "Need initialize AnimationController.");

            // Color Controller
            this.colorController = Color.Controller.Create(this.GetSkeletonAnimator().GetSkeleton());
            Debug.Assert(this.colorController != null, "Need initialize ColorController.");
        }


        private Material CreateMaterial( string shaderName, Texture2D texture )
        {
            Shader shader = Shader.Find(shaderName);

            if ( shader == null )
                return null;

            Material mat = new Material(shader);
            mat.mainTexture = texture;

            return mat;
        }

        private AtlasAsset CreateAtlasAsset( TextAsset atlas, Material material )
        {
            Material[] materials  = new Material[1]{ material };
            bool       initialize = true;

            return AtlasAsset.CreateRuntimeInstance(atlas, materials, initialize);
        }

        private SkeletonDataAsset CreateSkeletonDataAsset( TextAsset skeleton, AtlasAsset atlas, float scale=0.01f )
        {
            bool initialize = true;

            return SkeletonDataAsset.CreateRuntimeInstance(skeleton, atlas, initialize, scale);
        }

        private void InitRenderer()
        {
            MeshRenderer renderer = this.gameObject.GetComponentInChildren<MeshRenderer>();

            if ( renderer == null )
            {
                renderer = this.gameObject.AddComponent<MeshRenderer>();
            }
            
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder     = 0;

            this.meshRenderer = renderer;            
        }

        //=== ISkeletonAnimator

        protected void SetAnimator( ISkeletonAnimator animator )
        {
            this.animator = animator;
        }
    }
}