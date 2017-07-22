
namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// 2D Objectとして扱うため、`Spine.SkeletonGraphic`をラップした ISkeletonAnimator です
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class ImageAnimator : ISkeletonAnimator
    {
        //=== Variables
        private Spine.Unity.SkeletonGraphic animator = null;

        //=== Initialization

        public static ImageAnimator Create( Spine.Unity.SkeletonGraphic graphic )
        {
            return new ImageAnimator(graphic);
        }

        private ImageAnimator( Spine.Unity.SkeletonGraphic graphic )
        {
            this.animator = graphic;
        }


        //=== Animator

        public Spine.Unity.SkeletonGraphic GetAnimator()
        {
            return this.animator;
        }

        void ISkeletonAnimator.Init( SkeletonDataAsset data )
        {
            this.GetAnimator().skeletonDataAsset = data.GetDataAsset();
            this.GetAnimator().Initialize(true);
        }

        /// <summary>
        /// Spine.AnimationState もしくは null を返すようにしてください.
        /// </summary>
        Spine.AnimationState ISkeletonAnimator.GetAnimationState()
        {
            return this.GetAnimator().AnimationState;
        }

        /// <summary>
        /// Spine.Skeleton もしくは null を返すようにしてください.
        /// </summary>        
        Spine.Skeleton ISkeletonAnimator.GetSkeleton()
        {
            return this.GetAnimator().Skeleton;
        }


        //=== SkeletonGraphic

        public Spine.Unity.SkeletonGraphic GetSkeletonGraphic()
        {
            return this.GetAnimator();
        }
    }
}