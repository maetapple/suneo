namespace Suneo
{    
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Canvas上で扱う2D Object Skeletonです.
    /// - ISkeletonAnimator として `ImageAnimator`を扱います
    /// - `Spine.SkeletonGraphic` を扱います
    /// </summary>
    //----------------------------------------------------------------------------------------------
    [UnityEngine.AddComponentMenu("Suneo/Skeleton/ImageSkeleton")]
    [UnityEngine.RequireComponent(typeof(Spine.Unity.SkeletonGraphic))]
    public class ImageSkeleton : Skeleton
    {
        //=== Initialization

        public static ImageSkeleton Create( SkeletonAsset asset )
        {
            return Skeleton.Create<ImageSkeleton>(asset);
        }

        public static ImageSkeleton Create( SkeletonDataAsset dataAsset )
        {
            return Skeleton.Create<ImageSkeleton>(dataAsset);
        }

        public static ImageSkeleton AddToGameObject( UnityEngine.GameObject go )
        {
            return Skeleton.AddToGameObject<ImageSkeleton>(go);
        }


        private void Awake()
        {
            this.InitAnimator();
        }

        private void InitAnimator()
        {
            Spine.Unity.SkeletonGraphic graphic = this.gameObject.GetComponentInChildren<Spine.Unity.SkeletonGraphic>();

            if ( graphic == null )
            {
                graphic = this.gameObject.AddComponent<Spine.Unity.SkeletonGraphic>();
            }

            ISkeletonAnimator animator = ImageAnimator.Create(graphic);
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