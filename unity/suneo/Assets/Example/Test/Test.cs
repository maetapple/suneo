using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(this.Load());
	}

    private IEnumerator Load()
    {
        Texture2D texture = Resources.Load<Texture2D>("Spine/SpineBoy/spineboy");
        TextAsset atlas   = Resources.Load<TextAsset>("Spine/SpineBoy/spineboy.atlas");
        TextAsset data    = Resources.Load<TextAsset>("Spine/SpineBoy/spineboy-data");
        string    shader  = Suneo.SkeletonAsset.SpriteShaderName;

        Suneo.SkeletonAsset asset    = Suneo.SkeletonAsset.Create(shader, texture, atlas, data);
        Suneo.Skeleton      skeleton = Suneo.SpriteSkeleton.Create<Suneo.SpriteSkeleton>(asset);

        yield return null;
    }
}
