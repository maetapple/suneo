using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class DestructionTimer : MonoBehaviour 
    {
        public void StartUp( float remainingSec )
        {
            this.StartCoroutine(this.KeepWaitingForDestuction(remainingSec));
        }

        private IEnumerator KeepWaitingForDestuction( float sec )
        {
            yield return new WaitForSeconds(sec);

            GameObject.Destroy(this.gameObject);
        }
    }
}