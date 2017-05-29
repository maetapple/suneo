using UnityEngine;
using UnityEditor;

namespace Suneo.EditorExtension
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Inspector custom editor for `Suneo.SkeletonLoader`
    /// - `[CustomEditor]` 対象クラスを Suneo.SkeletonLoader とし、そのサブクラスも対象とします
    /// - `[CanEditMultipleObjects]` 同時に複数のオブジェクトに対して変更できるようにします
    /// </summary>
    //----------------------------------------------------------------------------------------------
    [CustomEditor(typeof(Suneo.SkeletonLoader), true)]
    [CanEditMultipleObjects]
    public class SkeletonLoaderInspector : Editor
    {
        private Suneo.SkeletonLoader GetLoader()
        {
            return this.target as Suneo.SkeletonLoader;
        }

        /// <summary>
        /// inspectorの更新時に呼ばれます
        /// </summary>
        public override void OnInspectorGUI()
        {
            // use default editor
            base.OnInspectorGUI();

            if ( GUILayout.Button("Load with Assets") == true )
            {
                this.GetLoader().Load();
            }
        }
    }

}
