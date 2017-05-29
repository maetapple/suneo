
namespace Suneo
{
    public interface ISkeletonAnimator
    {
        void Init( Spine.Unity.SkeletonDataAsset data );

        /// <summary>
        /// Spine.AnimationState もしくは null を返すようにしてください.
        /// </summary>
        Spine.AnimationState GetAnimationState();

        /// <summary>
        /// Spine.Skeleton もしくは null を返すようにしてください.
        /// </summary>        
        Spine.Skeleton GetSkeleton();
    }
}