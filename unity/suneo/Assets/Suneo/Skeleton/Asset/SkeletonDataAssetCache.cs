using System.Collections.Generic;

namespace Suneo
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// SkeletonDataAsset を保持し、検索、取得できる機能を提供します.
    /// - `TKey` Dictionaryで保管するため、任意の型のKeyを使用してください
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class SkeletonDataAssetCache<TKey>
    {
        //=== Field
        private Dictionary<TKey, SkeletonDataAsset> table = null;


        //=== Initialize

        protected SkeletonDataAssetCache()
        {
            this.table = new Dictionary<TKey, SkeletonDataAsset>();
        }

        public static SkeletonDataAssetCache<TKey> Create()
        {
            return new SkeletonDataAssetCache<TKey>();
        }


        //=== Control

        /// <summary>
        /// asset が null の場合追加せず、false を返します
        /// </summary>
        public bool Add( TKey key, SkeletonDataAsset asset )
        {
            if ( asset == null )
                return false;

            this.GetTable().Add(key, asset);

            return true;
        }

        /// <summary>
        /// 削除に失敗した場合、false を返します
        /// </summary>
        public bool Remove( TKey key )
        {
            return this.GetTable().Remove(key);
        }

        /// <summary>
        /// キャッシュ対象を全て削除します
        /// </summary>
        public void Clear()
        {
            this.GetTable().Clear();
        }

        /// <summary>
        /// key に該当するものがなければ null を返します
        /// </summary>
        public SkeletonDataAsset GetOrNull( TKey key )
        {
            SkeletonDataAsset asset = null;

            if ( this.GetTable().TryGetValue(key, out asset) == false )
            {
                return null;
            }

            return asset;
        }

        /// <summary>
        /// keyList に該当するものがあれば assetList に追加します
        /// - `assetList.Clear()` しません
        /// </summary>
        public void Select( List<TKey> keyList, ref List<SkeletonDataAsset> assetList )
        {
            foreach ( TKey key in keyList )
            {
                SkeletonDataAsset asset = this.GetOrNull(key);

                if ( asset == null )
                    continue;

                assetList.Add(asset);
            }
        }

        /// <summary>
        /// 保管している全てを assetList に追加します
        /// - `assetList.Clear()` しません
        /// </summary>
        public void SelectAll( ref List<SkeletonDataAsset> assetList )
        {
            assetList.AddRange(this.GetTable().Values);
        }


        //=== Asset Table

        private Dictionary<TKey, SkeletonDataAsset> GetTable()
        {
            return this.table;
        }
    }


    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// `string` をキーとした SkeletonDataAssetCache です
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class SkeletonDataAssetCacheByString : SkeletonDataAssetCache<string>
    {
        private SkeletonDataAssetCacheByString()
        : base()
        { return; }

        public static new SkeletonDataAssetCacheByString Create()
        {
            return new SkeletonDataAssetCacheByString();
        }
    }
    
}