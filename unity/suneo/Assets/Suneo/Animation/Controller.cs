using System.Collections.Generic;
using Spine;

namespace Suneo.Animation
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// AnimationStateを通してアニメーションを設定します
    /// - `SkeletonAnimation`, `SkeletonGraphic` の `AnimationState` を対象とします
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Controller
    {
        //=== Variables
        private State  state  = null;
        private Keeper keeper = null;

        //=== Delegation

        public class EventArgument
        {}

        public delegate void HandleEvent( Controller sender, EventArgument arg );
        private Dictionary<int, List<HandleEvent>> finishHandler = null;

        //=== Initialization

        /// <summary>
        /// 対象の AnimationState を渡してください
        /// </summary>
        public static Controller Create( AnimationState animState )
        {
            return new Controller(animState);
        }

        private Controller( AnimationState animState )
        {
            this.state  = Animation.State.Create(animState);
            this.keeper = Animation.Keeper.Create();
            this.finishHandler = new Dictionary<int, List<HandleEvent>>();

            this.GetState().CompleteHandler  += this.OnFinishAnimation;
            this.GetState().EndHandler       += this.OnFinishAnimation;
            this.GetState().InterruptHandler += this.OnFinishAnimation; 
        }

        //=== Animation State

        /// <summary>
        /// `this.state` への参照を返します.
        /// </summary>
        public State GetState()
        {
            return this.state;
        }

        /// <summary>
        /// アニメーションを設定します
        /// - 既にSet(), Add()されているものはリセットされます
        /// </summary>
        public TrackEntry Set( int trackIndex, string name, bool loop=false)
        {
            TrackEntry entry = this.GetState().Set(trackIndex, name, loop);
            this.ClearPlaying(trackIndex);
            this.RegisterPlaying(entry);

            return entry;
        }

        /// <summary>
        /// アニメーションを追加します
        /// </summary>
        public TrackEntry Add( int trackIndex, string name, bool loop=false, float delay=0f )
        {
            TrackEntry entry = this.GetState().Add(trackIndex, name, loop, delay);
            this.RegisterPlaying(entry);

            return entry;
        }

        /// <summary>
        /// アニメーション設定をクリアします
        /// </summary>
        public void Clear( int trackIndex )
        {
            this.GetState().Clear(trackIndex);
            this.ClearPlaying();
        }

        /// <summary>
        /// アニメーション終了時のコールバックです.
        /// </summary>
        private void OnFinishAnimation( State sender, State.EventArgument arg )
        {
            TrackEntry entry = arg.Entry;

            this.RemovePlaying(entry);
            this.DelegateIfFinished(entry);

            // Debug.LogFormat("#Animation.Controller >> State animation complete.\n [name]{0}, [duration]{1}",
            //                    entry.Animation.Name, entry.Animation.Duration);            
        }

        //=== Animation Keeper

        /// <summary>
        /// `this.keeper` への参照を返します
        /// </summary>
        public Keeper GetKeeper()
        {
            return this.keeper;
        }

        /// <summary>
        /// 再生中のアニメーションとして登録します
        /// - Loop=ON のentryは登録されません
        /// </summary>
        private void RegisterPlaying( TrackEntry entry )
        {
            if ( entry.Loop == true )
                return;

            this.GetKeeper().Register(entry);
        }

        /// <summary>
        /// 終了したアニメーションとして、登録解除します
        /// </summary>
        private void RemovePlaying( TrackEntry entry )
        {
            this.GetKeeper().Remove(entry);
        }

        /// <summary>
        /// trackIndex に該当するアニメーションを全て登録解除します
        /// </summary>
        private void ClearPlaying( int trackIndex )
        {
            this.GetKeeper().Clear(trackIndex);
        }
        
        /// <summary>
        /// アニメーションを全て登録解除します
        /// </summary>
        private void ClearPlaying()
        {
            this.GetKeeper().Clear();
        }

        /// <summary>
        /// trackIndex に該当するアニメーションが再生中の場合 true を返します
        /// </summary>
        public bool IsPlaying( int trackIndex )
        {
            return !(this.GetKeeper().IsEmpty(trackIndex));
        }        

        //=== Event Handling

        /// <summary>
        /// 一連のモーション再生終了時のコールバックをセットします.
        /// - TrackIndex 毎に設定します
        /// - Loopの場合は１回目の終了のみです
        /// </summary>
        public void AddFinishHandler( HandleEvent handler, int trackIndex )
        {
            List<HandleEvent> list = null;

            if ( this.finishHandler.TryGetValue(trackIndex, out list) == false )
            {
                list = new List<HandleEvent>();
                this.finishHandler[trackIndex] = list;
            }

            list.Add(handler);
        }

        /// <summary>
        /// セットされたアニメーションの再生が終了していれば、コールバックを実行します.
        /// </summary>
        private void DelegateIfFinished( TrackEntry entry )
        {
            if ( this.GetKeeper().IsEmpty(entry.TrackIndex) == false )
                return;

            //Debug.LogFormat("#Animation.Controller >> Finished keeping animation [track index]{0}.", entry.TrackIndex);

            List<HandleEvent> list = null;

            if ( this.finishHandler.TryGetValue(entry.TrackIndex, out list) == false )
                return;
            
            this.DelegateEventHandling(list);
            this.ClearStateHandling(list);
        }

        /// <summary>
        /// コールバックを実行します.
        /// </summary>
        private void DelegateEventHandling( List<HandleEvent> list )
        {
            if ( list.Count <= 0 )
                return;
            
            EventArgument arg = new EventArgument();

            foreach ( HandleEvent handler in list )
            {
                handler(this, arg);
            }
        }

        /// <summary>
        /// 登録されているコールバックを解放します.
        /// </summary>
        private void ClearStateHandling( List<HandleEvent> list )
        {
            list.Clear();
        }        

    }
}