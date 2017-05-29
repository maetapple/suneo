using System.Collections;
using UnityEngine;

#if false
using ManDom.Data;

namespace Suneo.Resource
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// １つのSpine Objectを構成するのに必要なResourceを集約します.
    /// - Resource.Path を参照します
    /// - ManDom.Data.ResourceManager でリソースを読み込みます
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Pack
    {
        //=== Variables        
        public Path      Path       { get; private set; }
        public Path      ShaderPath { get; private set; }
        public Texture2D Texture    { get; set; }
        public TextAsset Atlas      { get; set; }
        public TextAsset Skeleton   { get; set; }

        //=== Initialization

        /// <summary>
        /// Pathを基に生成して返します. Load()はしません.
        /// </summary>
        public static Pack Create( Path path )
        {
            return new Pack(path);
        }

        private Pack( Path path )
        {
            this.Path = path;
        }

        /// <summary>
        /// sourceのメンバの参照コピーします.
        /// </summary>
        public void Setup( Pack source )
        {
            this.Path           = source.Path;
            this.ShaderPath     = source.ShaderPath;
            this.Texture        = source.Texture;
            this.Atlas          = source.Atlas;
            this.Skeleton       = source.Skeleton;
        }

        //=== ID

        /// <summary>
        /// IDとしてSkeleton Pathから拡張子を抜いたものを返します
        /// - `skelton-file.skel` => `skelton-file`
        /// </summary>
        public string GetId()
        {
            return System.IO.Path.GetFileNameWithoutExtension(this.Path.Skeleton);
        }

        //=== Skeleton

        /// <summary>
        /// TextAsset Skeletonが Binaryファイルであれば true を返します
        /// </summary>
        public bool IsBinaryFormat()
        {
            return this.Skeleton.name.ToLower().Contains(".skel");
        }

        //=== Loading

        /// <summary>
        /// 各リソースをLoadします.
        /// - 正常終了すればメンバ変数にリソースがセットされています.
        /// </summary>
        public IEnumerator Load()
        {
            return this.Load(this.Path);
        }

        /// <summary>
        /// Pathを基にリソースをLoadします.
        /// </summary>
        private IEnumerator Load( Path path )
        {
            this.LoadRequest<Texture2D>(path.Texture);
            this.LoadRequest<TextAsset>(path.Atlas);
            this.LoadRequest<TextAsset>(path.Skeleton);

            yield return new WaitWhile(() => ResourceManager.Instance.IsLoadEnd() == false);

            this.Texture  = this.GetResource<Texture2D>(path.Texture);
            this.Atlas    = this.GetResource<TextAsset>(path.Atlas);
            this.Skeleton = this.GetResource<TextAsset>(path.Skeleton);

            yield break;
        }

        /// <summary>
        /// pathを基にリソースをLoadします.
        /// </summary>
        private void LoadRequest<T>( string path ) where T : UnityEngine.Object
        {
            if ( string.IsNullOrEmpty(path) == true )
                return;

            ResourceManager.Instance.LoadRequest<T>(path);
        }

        /// <summary>
        /// pathを基にリソースを取得します.
        /// </summary>
        private T GetResource<T>( string path ) where T : UnityEngine.Object
        {
            if ( string.IsNullOrEmpty(path) == true )
                return null;

            string name = System.IO.Path.GetFileName(path);
            return ResourceManager.Instance.GetAsset<T>(name);
        }

        /// <summary>
        /// 各リソースをUnloadします.
        /// </summary>
        public bool Unload()
        {            
            ResourceManager.Instance.UnloadAsset( System.IO.Path.GetFileName(this.Path.Texture)  );
            ResourceManager.Instance.UnloadAsset( System.IO.Path.GetFileName(this.Path.Atlas)    );
            ResourceManager.Instance.UnloadAsset( System.IO.Path.GetFileName(this.Path.Skeleton) );

            return true;
        }

    }
}
#endif