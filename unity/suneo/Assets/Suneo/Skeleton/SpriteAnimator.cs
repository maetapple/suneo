using Spine.Unity;

namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// 3D Objectとして扱うため、`Spine.SkeletonAnimation`をラップした ISkeletonAnimator です
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class SpriteAnimator : ISkeletonAnimator
    {
        //=== Variables

        private SkeletonAnimation animator = null;


        //=== Initialization

        public static SpriteAnimator Create( SkeletonAnimation animator )
        {
            return new SpriteAnimator(animator);
        }

        private SpriteAnimator( SkeletonAnimation animator )
        {
            this.animator = animator;
        }


        //=== Animator

        public SkeletonAnimation GetAnimator()
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


        //=== SkeletonAnimation

        public SkeletonAnimation GetSkeletonAnimation()
        {
            return this.GetAnimator();
        }
    }
}