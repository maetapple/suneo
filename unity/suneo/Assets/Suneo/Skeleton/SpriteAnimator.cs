
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

        private Spine.Unity.SkeletonAnimation animator = null;


        //=== Initialization

        public static SpriteAnimator Create( Spine.Unity.SkeletonAnimation animator )
        {
            return new SpriteAnimator(animator);
        }

        private SpriteAnimator( Spine.Unity.SkeletonAnimation animator )
        {
            this.animator = animator;
        }


        //=== Animator

        public Spine.Unity.SkeletonAnimation GetAnimator()
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


        //=== SkeletonAnimation

        public Spine.Unity.SkeletonAnimation GetSkeletonAnimation()
        {
            return this.GetAnimator();
        }
    }
}