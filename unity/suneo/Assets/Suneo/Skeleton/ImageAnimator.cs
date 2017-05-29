using Spine.Unity;

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
        private SkeletonGraphic animator = null;

        //=== Initialization

        public static ImageAnimator Create( SkeletonGraphic graphic )
        {
            return new ImageAnimator(graphic);
        }

        private ImageAnimator( SkeletonGraphic graphic )
        {
            this.animator = graphic;
        }


        //=== Animator

        public SkeletonGraphic GetAnimator()
        {
            return this.animator;
        }

        void ISkeletonAnimator.Init( Spine.Unity.SkeletonDataAsset data )
        {
            this.GetAnimator().skeletonDataAsset = data;
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

        public SkeletonGraphic GetSkeletonGraphic()
        {
            return this.GetAnimator();
        }
    }
}