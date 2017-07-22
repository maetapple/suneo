using UnityEngine;

namespace Suneo
{
    [ExecuteInEditMode]    
    [RequireComponent(typeof(Suneo.Skeleton))]
    [UnityEngine.AddComponentMenu("Suneo/Skeleton/SkeletonLoader")]
    public class SkeletonLoader : MonoBehaviour 
    {
        //=== At inspector
        [SerializeField] public SkeletonAsset asset = null;

        //=== Field
        private Skeleton skeleton = null;


        private Skeleton GetSkeleton()
        {
            return this.skeleton;
        }

        private void AssignSkeleton()
        {
            this.skeleton = this.gameObject.GetComponentInChildren<Skeleton>();
            Debug.Assert(this.skeleton != null, "Failed get component: Skeleton");
        }

        public void Load()
        {
            if ( this.asset.IsAvailable() == false )
            {
                Debug.Assert(false, "Not load");
                return;
            }

            this.GetSkeleton().Setup(this.asset);
        }


        //=== Unity event function

        private void Awake()
        {
            this.AssignSkeleton();
            this.Reset();
        }


        //=== Editor mode only

        /// <summary>
        /// インスペクターのコンテキストメニューにある`Reset`ボタンやコンポーネントを初めて追加するときに呼び出されます.
        /// </summary>
        private void Reset()
        {
            SpriteSkeleton sprite = this.GetSkeleton() as SpriteSkeleton;

            if ( sprite != null )
            {
                this.asset = SkeletonAsset.Create(SkeletonAsset.SpriteShaderName, null, null, null);
                return;
            }

            ImageSkeleton image = this.GetSkeleton() as ImageSkeleton;

            if ( image != null )
            {
                this.asset = SkeletonAsset.Create(SkeletonAsset.ImageShaderName, null, null, null);
            }
        }
    }
}