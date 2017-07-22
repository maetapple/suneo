using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
    private Suneo.SkeletonDataAssetCacheByString dataAssetCache = null;

    void Awake()
    {
        this.dataAssetCache = Suneo.SkeletonDataAssetCacheByString.Create();
    }

	void Start () 
    {
        StartCoroutine(this.Load());
	}

    private IEnumerator Load()
    {
        string dirPath = "Spine/SpineBoy";

        // asset creation
        {
            Texture2D texture = Resources.Load<Texture2D>(dirPath+"/spineboy");
            TextAsset atlas   = Resources.Load<TextAsset>(dirPath+"/spineboy.atlas");
            TextAsset data    = Resources.Load<TextAsset>(dirPath+"/spineboy-data");
            string    shader  = Suneo.SkeletonAsset.SpriteShaderName;

            Suneo.SkeletonAsset     asset     = Suneo.SkeletonAsset.Create(shader, texture, atlas, data);
            Suneo.SkeletonDataAsset dataAsset = Suneo.SkeletonDataAsset.Create(asset);

            this.dataAssetCache.Add(dirPath, dataAsset);
        }

        List<Vector3> posList = new List<Vector3>() {
            new Vector3( -4.0f, -2.0f, 0 ),
            new Vector3(  0.0f, -2.0f, 0 ),
            new Vector3(  4.0f, -2.0f, 0 ),
        };

        foreach ( Vector3 pos in posList )
        {
            Suneo.SkeletonDataAsset cache = this.dataAssetCache.GetOrNull(dirPath);

            if ( cache == null )
            {
                Debug.Assert(false, "FAILED cache creation");
            }
                        
            Suneo.Skeleton skeleton = Suneo.SpriteSkeleton.Create<Suneo.SpriteSkeleton>(cache);
            skeleton.gameObject.transform.position = pos;
        }

        yield return null;
    }
}
