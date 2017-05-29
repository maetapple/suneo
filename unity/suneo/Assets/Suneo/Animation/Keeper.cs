using System.Collections.Generic;
using UnityEngine;
using Spine;

/**
# USAGE
```
TrackEntry entry = SkeletonAnimation.state.SetAnimation(trackIndex, name, loop);
this.spineModelAnimKeeper.Register(entry);

private void OnCompleted( TrackEntry entry )
{
    this.spineModelAnimKeeper.Remove(entry);
}
```
 */

namespace Suneo.Animation
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Skeleton Animation を保持するための機能を提供します.
    /// - TrackEntry を扱います
    /// - trackIndex 毎に配列で区別して管理します
    /// - trackIndex はRegister()にて自動的に判定し、振り分けます
    /// - trackIndex 単位でのハンドリングが可能です    
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Keeper
    {
        //=== Variables
        private Dictionary<int, List<TrackEntry>> container = null;

        //=== Initialization

        public static Keeper Create()
        {
            return new Keeper();
        }

        private Keeper()
        {
            this.container = new Dictionary<int, List<TrackEntry>>();
        }

        //=== Control

        /// <summary>
        /// trackIndex に該当する配列をクリアします
        /// </summary>
        public void Clear( int trackIndex )
        {
            List<TrackEntry> list = this.GetListOrNull(trackIndex);

            if ( list == null )
                return;
            
            list.Clear();
        }

        /// <summary>
        /// 全てのTrackEntryをクリアします
        /// </summary>
        public void Clear()
        {
            foreach ( int trackIndex in this.GetKeyCollection() )
            {
                this.Clear(trackIndex);
            }
        }

        /// <summary>
        /// 管理対象として追加します
        /// </summary>
        public void Register( TrackEntry entry )
        {
            int trackIndex = entry.TrackIndex;
            List<TrackEntry> list = this.GetListOrNull(trackIndex);

            if ( list == null )
            {
                list = this.CreateList(trackIndex);
                return;
            }

            list.Add(entry);            
        }

        /// <summary>
        /// 管理対象から外します
        /// </summary>
        public void Remove( TrackEntry entry )
        {
            int trackIndex = entry.TrackIndex;
            List<TrackEntry> list = this.GetListOrNull(trackIndex);

            if ( list == null )
                return;

            list.Remove(entry);
        }

        /// <summary>
        /// trackIndexに該当する配列が空なら true を返します
        /// </summary>
        public bool IsEmpty( int trackIndex )
        {
            List<TrackEntry> list = this.GetListOrNull(trackIndex);

            if ( list == null )
                return true;

            return ( list.Count <= 0 );
        }

        /// <summary>
        /// TrackEntryの登録が１つもなければ true を返します
        /// </summary>
        public bool IsEmpty()
        {
            foreach ( int trackIndex in this.GetKeyCollection() )
            {
                if ( this.IsEmpty(trackIndex) == false )
                    return false;
            }

            return true;
        }

        //=== Container

        /// <summary>
        /// コンテナ Dictionary を返します
        /// </summary>
        private Dictionary<int, List<TrackEntry>> GetContainer()
        {
            return this.container;
        }

        /// <summary>
        /// Dictionary.Keys です.
        /// </summary>
        private Dictionary<int, List<TrackEntry>>.KeyCollection GetKeyCollection()
        {
            return this.GetContainer().Keys;
        }

        /// <summary>
        /// Dictionary.Values です.
        /// </summary>
        private Dictionary<int, List<TrackEntry>>.ValueCollection GetValueCollection()
        {
            return this.GetContainer().Values;
        }

        /// <summary>
        /// trackIndex に該当する配列があれば返します
        /// </summary>
        private List<TrackEntry> GetListOrNull( int trackIndex )
        {
            List<TrackEntry> list = null;

            if ( this.GetContainer().TryGetValue(trackIndex, out list) == false )
                return null;

            return list;
        }
        
        /// <summary>
        /// trackIndex をKeyとして新たに配列を作成します.
        /// </summary>
        private List<TrackEntry> CreateList( int trackIndex )
        {
            List<TrackEntry> list = new List<TrackEntry>();
            this.GetContainer()[trackIndex] = list;

            return list;
        }

        //=== Debug

        public void OutputLog()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.AppendLine("SpineModel Playing anim log.");

            foreach ( int trackIndex in this.GetKeyCollection() )
            {
                List<TrackEntry> list = this.GetListOrNull(trackIndex);

                if ( list == null )
                    continue;

                builder.AppendFormat("[{0}] {1}\n", trackIndex, list.Count);
            }

           Debug.Log(builder.ToString());
        }

    }
}