namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// WorldSpaceで扱う3D Object Skeletonです.
    /// - ISkeletonAnimator として `SpriteAnimator`を扱います
    /// - `Spine.SkeletonAnimation` を扱います
    /// </summary>
    //----------------------------------------------------------------------------------------------
    [UnityEngine.AddComponentMenu("Suneo/Skeleton/SpriteSkeleton")]
    [UnityEngine.RequireComponent(typeof(Spine.Unity.SkeletonAnimation))]
    public class SpriteSkeleton : Skeleton
    {
        //=== Initialization

        public static SpriteSkeleton Create( SkeletonAsset asset )
        {
            return Skeleton.Create<SpriteSkeleton>(asset);
        }

        public static SpriteSkeleton Create( SkeletonDataAsset dataAsset )
        {
            return Skeleton.Create<SpriteSkeleton>(dataAsset);
        }

        public static SpriteSkeleton AddToGameObject( UnityEngine.GameObject go )
        {
            return Skeleton.AddToGameObject<SpriteSkeleton>(go);
        }
        

        private void Awake()
        {
            this.InitAnimator();
        }

        private void InitAnimator()
        {
            Spine.Unity.SkeletonAnimation animation = this.gameObject.GetComponentInChildren<Spine.Unity.SkeletonAnimation>();

            if ( animation == null )
            {
                Spine.Unity.SkeletonDataAsset asset = this.GetSkeletonDataAsset().GetDataAsset();
                animation = Spine.Unity.SkeletonAnimation.AddToGameObject(this.gameObject, asset);
            }

            ISkeletonAnimator animator = SpriteAnimator.Create(animation);
            this.SetAnimator(animator);
        }


        //=== Editor mode only

        /// <summary>
        /// インスペクターのコンテキストメニューにある`Reset`ボタンやコンポーネントを初めて追加するときに呼び出されます.
        /// </summary>
        private void Reset()
        {
            this.InitAnimator();
        }
    }
}