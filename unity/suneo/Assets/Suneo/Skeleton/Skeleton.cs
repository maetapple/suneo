using UnityEngine;

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

        private MeshRenderer          meshRenderer    = null;
        private SkeletonDataAsset     dataAsset       = null;
        private Animation.Controller  animController  = null;
        private Color.Controller      colorController = null;
        private ISkeletonAnimator     animator        = null;


        //=== Accessor

        public MeshRenderer         GetMeshRenderer(){ return this.meshRenderer; }
        public Animation.Controller GetAnimation()   { return this.animController; }
        public Color.Controller     GetColor()       { return this.colorController; }
        
        public  SkeletonDataAsset   GetSkeletonDataAsset() { return this.dataAsset; }
        private ISkeletonAnimator   GetSkeletonAnimator()  { return this.animator; }


        //=== Initialization

        /// <summary>
        /// 新規GameObjectに`TSkeleton`をAddComponentした形で生成したものを返します.
        /// </summary>
        public static Skeleton Create<TSkeleton>( SkeletonAsset asset ) where TSkeleton : Skeleton
        {
            GameObject go       = new GameObject(typeof(TSkeleton).Name);
            Skeleton   skeleton = Skeleton.AddToGameObject<TSkeleton>(go);

            skeleton.Setup(asset);

            return skeleton;
        }

        /// <summary>
        /// goにAddComponentした形で生成したものを返します.
        /// </summary>
        public static TSkeleton AddToGameObject<TSkeleton>( GameObject go ) where TSkeleton : Skeleton
        {
            TSkeleton model = go.AddComponent<TSkeleton>();

            return model;            
        }

        /// <summary>
        /// 自信がハンドリングするGameObjectをDestoryします
        /// </summary>
        public void Destroy( float delay=0 )
        {
            GameObject.Destroy(this.gameObject, delay);
        }


        public void Setup( SkeletonAsset asset )
        {
            // @TODO MeshRenderer じゃなく Rendererとして扱えないか？
            // MeshRenderer
            // this.InitRenderer();

            // SkeletonDataAsset
            this.dataAsset = SkeletonDataAsset.Create(asset);

            // Sub Class             
            this.GetSkeletonAnimator().Init(this.GetSkeletonDataAsset());

            // Animation Controller
            this.animController = Animation.Controller.Create(this.GetSkeletonAnimator().GetAnimationState());
            Debug.Assert(this.animController != null, "Need initialize AnimationController.");

            // Color Controller
            this.colorController = Color.Controller.Create(this.GetSkeletonAnimator().GetSkeleton());
            Debug.Assert(this.colorController != null, "Need initialize ColorController.");
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