
namespace Suneo.Color
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Spine.Skeletonを通してカラーを操作します.
    /// - `SkeletonAnimation`, `SkeletonGraphic` の `Skeleton` を対象とします
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Controller
    {
        //=== Variables
        private Spine.Skeleton skeleton = null;


        //=== Initialization

        /// <summary>
        /// 対象の Spine.Skeleton を渡してください
        /// </summary>
        public static Controller Create( Spine.Skeleton skeleton )
        {
            return new Controller(skeleton);
        }

        private Controller( Spine.Skeleton skeleton )
        {
            this.skeleton = skeleton;
        }

        //=== Renderer

        /// <summary>
        /// Spine.Skeleton の参照を返します
        /// </summary>
        private Spine.Skeleton GetSkeleton()
        {
            return this.skeleton;
        }

        //=== Skeleton Color

        /// <summary>
        /// カラーをセットします.
        /// </summary>
        public void Set( UnityEngine.Color color )
        {
            Spine.Skeleton skeleton = this.GetSkeleton();
            skeleton.a = color.a;
            skeleton.r = color.r;
            skeleton.g = color.g;
            skeleton.b = color.b;
        }

        /// <summary>
        /// `a < 0`なら現在の値を適用します
        /// </summary>
        public void Set( float r, float g, float b, float a=-1.0f )
        {
            if ( a < 0 )
            {
                a = this.GetSkeleton().A;
            }

            this.Set(new UnityEngine.Color(r, g, b, a));
        }

        /// <summary>
        /// Alphaをセットします.
        /// </summary>
        public void SetAlpha( float alpha )
        {
            Spine.Skeleton skeleton = this.GetSkeleton();
            skeleton.a = alpha;
        }

        /// <summary>
        /// カラーを返します.
        /// </summary>
        public UnityEngine.Color Get()
        {
            Spine.Skeleton skeleton = this.GetSkeleton();

            return new UnityEngine.Color(skeleton.r, skeleton.g, skeleton.b, skeleton.a);
        }
    }
}