using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Suneo.SkeletonDataAssetCache を使用した場合のパフォーマンステストです
    /// - `UserCache` を切り替えることで Skeleton の描画数、FPSなどの変化をみます
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class AssetCachePerformance : MonoBehaviour 
    {
        //=== At Inspector

        [SerializeField] bool UseCache = false;
        [SerializeField] GameObject SpriteAdditionAreaObject = null;


        //=== Field

        private Suneo.SkeletonDataAssetCacheByString    dataAssetCache = null;
        private Dictionary<string, Suneo.SkeletonAsset> assetTable     = null;
        private Dictionary<string, string>              animationTable = null;

        private Rect additionAreaForSprite = Rect.zero;


        //=== Initialization

        void Start ()
        {
            this.assetTable = new Dictionary<string, Suneo.SkeletonAsset>();
            this.InitAssetTable(ref this.assetTable);

            this.dataAssetCache = Suneo.SkeletonDataAssetCacheByString.Create();
            this.InitDataAssetCache(this.dataAssetCache, this.assetTable);

            this.additionAreaForSprite = this.ComputeRectOnAreaObject(this.SpriteAdditionAreaObject);

            // assetTableのKeyと対象のSpineで実行するアニメーション名のTableを作ります
            this.animationTable = new Dictionary<string, string>() {
                { "Spine/SpineBoy/spineboy", "run"  },
                { "Spine/SpineGirl/Doi"    , "main" }   
            };
        }


        private void InitAssetTable( ref Dictionary<string, Suneo.SkeletonAsset> table )
        {
            List<string> basePathList = new List<string>() {
                "Spine/SpineBoy/spineboy",
                "Spine/SpineGirl/Doi",
                // "Spine/Dragon/dragon",
            };

            table.Clear();
            
            foreach ( string path in basePathList )
            {
                Suneo.SkeletonAsset asset = this.MakeAsset(path);
                table.Add(path, asset);
            }
        }


        private Suneo.SkeletonAsset MakeAsset( string texturePath )
        {
            Texture2D texture = Resources.Load<Texture2D>(texturePath);
            TextAsset atlas   = Resources.Load<TextAsset>(texturePath+".atlas");
            TextAsset data    = Resources.Load<TextAsset>(texturePath+"-data");
            string    shader  = Suneo.SkeletonAsset.SpriteShaderName;

            return Suneo.SkeletonAsset.Create(shader, texture, atlas, data);
        }


        private void InitDataAssetCache( Suneo.SkeletonDataAssetCacheByString cache, 
                                         Dictionary<string, Suneo.SkeletonAsset> assetTable )
        {
            cache.Clear();

            foreach ( KeyValuePair<string, Suneo.SkeletonAsset> pair in assetTable )
            {
                Suneo.SkeletonAsset     asset = pair.Value;
                Suneo.SkeletonDataAsset data  = Suneo.SkeletonDataAsset.Create(asset);

                string key = pair.Key;
                cache.Add(key, data);
            }
        }


        private void Update()
        {
            string key = this.ElectGeneratingDataAssetKey(this.assetTable);
            Suneo.SkeletonDataAsset dataAsset = null;

            if ( this.UseCache == true )
            {
                dataAsset = this.dataAssetCache.GetOrNull(key);
            }
            else
            {
                Suneo.SkeletonAsset asset = this.assetTable[key];
                dataAsset = Suneo.SkeletonDataAsset.Create(asset);
            }

            Suneo.Skeleton skeleton = Suneo.SpriteSkeleton.Create(dataAsset);
            skeleton.gameObject.transform.position = this.ElectPositionAtRandom(ref this.additionAreaForSprite);

            string animName = this.animationTable[key];
            skeleton.GetAnimation().Set(trackIndex:0, name:animName, loop:true);

            DestructionTimer timer = skeleton.gameObject.AddComponent<DestructionTimer>();
            timer.StartUp(0.5f);
        }


        //=== Compute generating parameter

        private Rect ComputeRectOnAreaObject( GameObject area )
        {
            if ( area == null )
                return Rect.zero;

            Renderer renderer = area.GetComponentInChildren<Renderer>();
            Vector3  size     = renderer.bounds.size;
            Vector3  center   = area.transform.position;

            Rect rect = new Rect(
                center.x + (size.x * 0.5f * -1.0f),
                center.y + size.y * 0.5f * -1.0f,
                size.x,
                size.y
            );
            
            return rect;
        }

        private Vector3 ElectPositionAtRandom( ref Rect rect )
        {
            Vector2 size = rect.size;
            Vector2 pos  = rect.position;
            Vector3 min  = new Vector3(pos.x, pos.y, 0);
            Vector3 max  = new Vector3(pos.x + size.x, pos.y + size.y, 0);

            Vector3 result = new Vector3(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                0
            );

            return result;
        }

        private string ElectGeneratingDataAssetKey( Dictionary<string, Suneo.SkeletonAsset> table )
        {            
            List<string> keyList = new List<string>(table.Keys);
            int          count   = keyList.Count;
            int          index   = UnityEngine.Random.Range(1, 10) % count;

            return keyList[index];
        }
    }
}