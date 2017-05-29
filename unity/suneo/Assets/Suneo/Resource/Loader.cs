using System.Collections;
using System.Collections.Generic;

/**
# USAGE
```
// Setup
string texture  = string.Format("SpineModel/CharaSD-{0:D5}/CharaSD-{1:D5}", id, id);
string atlas    = string.Format("{0}.atlas", texture);
string skeleton = string.Format("{0}.skel", texture);

Resource.Path path = Resource.Path.Create(texture, atlas, skeleton);
Resource.Pack pack = Resource.Pack.Create(path);

// Loading
Resource.Loader loader = Resource.Loader.Create();
yield return loader.LoadFromCacheOrResources(pack);
```
 */

 #if false
namespace Suneo.Resource
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resource.PackをResource.Cache もしくは リソースファイルからLoadします.
    /// - LoadしたResourceは Pack.GetId()をkeyとしてキャッシュします
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Loader
    {
        //=== Variables
        private Cache cache = null;


        //=== Initialization

        public static Loader Create()
        {
            return new Loader();
        }

        private Loader()
        {
            this.cache = Cache.Create();
        }

        //=== Pack

        /// <summary>
        /// idに該当するキャッシュがあれば返します. なければ null を返します
        /// </summary>
        public Pack GetPack( string id )
        {
            Pack pack = this.GetCache().GetOrNull(id);
            UnityEngine.Debug.Assert(pack != null, "Pack: Failed get Pack from cache.");

            return pack;
        }

        //=== Loading

        /// <summary>
        /// packのリソースをロードします.
        /// - あればキャッシュを使用します
        /// </summary>
        public IEnumerator LoadFromCacheOrResources( Pack pack )
        {
            Pack cache = this.GetCache().GetOrNull(pack.GetId());

            if ( cache != null )
            {
                pack.Setup(cache);
                yield break;
            }

            yield return pack.Load();

            this.GetCache().Register(pack);
        }

        /// <summary>
        /// `LoadFromCacheOrResources(Pack)`のリスト対応です.
        /// </summary>
        public IEnumerator LoadFromCacheOrResources( List<Pack> list )
        {
            foreach ( Pack pack in list )
            {
                yield return pack.Load();
            }

            yield break;
        }

        /// <summary>
        /// packのリソース参照を解除し、キャッシュを削除します.
        /// </summary>
        public void Unload( Pack pack )
        {
            if ( pack.Unload() == true )
            {
                this.GetCache().Remove(pack.GetId());
            }
        }

        //=== Cache

        /// <summary>
        /// キャッシュへの参照を返します.
        /// </summary>
        private Cache GetCache()
        {
            return this.cache;
        }

        /// <summary>
        /// キャッシュされているPackを全てUnload()します.
        /// </summary>
        public void ClearCache()
        {
            List<Pack> list = this.GetCache().GetAll();

            foreach ( Pack pack in list )
            {
                this.Unload(pack);
            }

            this.GetCache().Clear();
        }
    }
}
#endif