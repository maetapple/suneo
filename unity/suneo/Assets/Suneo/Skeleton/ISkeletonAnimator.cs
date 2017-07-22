
namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Spine.ISkeletonAnimation サブクラスをハンドリングするためのインターフェイスです
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public interface ISkeletonAnimator
    {
        /// <summary>
        /// SkeletonDataAsset による Skeleton 初期化を実装してください.
        /// </summary>
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