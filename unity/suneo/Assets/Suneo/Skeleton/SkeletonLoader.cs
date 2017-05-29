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

        //=== Variables
        private Skeleton skeleton = null;


        private Skeleton GetSkeleton()
        {
            return this.skeleton;
        }

        public void Load()
        {
            if ( this.asset.texture == null || this.asset.atlas == null || this.asset.skeleton == null )
            {
                Debug.Assert(false, "Not load");
                return;
            }

            this.GetSkeleton().Setup(this.asset);
        }


        /// <summary>
        /// インスペクターのコンテキストメニューにある`Reset`ボタンやコンポーネントを初めて追加するときに呼び出されます.
        /// </summary>
        private void Reset()
        {
            this.skeleton = this.gameObject.GetComponentInChildren<Skeleton>();
            Debug.Assert(this.skeleton != null, "Failed get component: Skeleton");

            SpriteSkeleton sprite = this.skeleton as SpriteSkeleton;

            if ( sprite != null )
            {
                this.asset = SkeletonAsset.Create(SkeletonAsset.SpriteShaderName, null, null, null);
                return;
            }

            ImageSkeleton image = this.skeleton as ImageSkeleton;

            if ( image != null )
            {
                this.asset = SkeletonAsset.Create(SkeletonAsset.ImageShaderName, null, null, null);
            }
        }
    }
}