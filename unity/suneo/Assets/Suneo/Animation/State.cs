using Spine;

namespace Suneo.Animation
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Spine.AnimationState のラッパーです.
    /// - TrackEntry を扱います
    /// - trackIndex 毎に配列で区別して管理します
    /// - trackIndex はRegister()にて自動的に判定し、振り分けます
    /// - trackIndex 単位でのハンドリングが可能です    
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class State
    {
        //=== Variables
        private  AnimationState state = null;

        //=== Delegation

        public class EventArgument
        {
            public TrackEntry Entry { get; set; }
        }

        public delegate void HandleTrackEvent( Animation.State sender, EventArgument arg );
        public event HandleTrackEvent StartHandler, CompleteHandler, EndHandler, InterruptHandler = null;

        //=== Initialization

        /// <summary>
        /// 対象の AnimationState を渡してください
        /// </summary>
        public static State Create( AnimationState animState )
        {
            return new State(animState);
        }

        private State( AnimationState animState )
        {
            UnityEngine.Debug.Assert(animState != null, "Need AnimationState for Initialization.");
            this.state = animState;

            this.state.Start     += this.OnStarted;
            this.state.Complete  += this.OnCompleted;
            this.state.End       += this.OnEnded;
            this.state.Interrupt += this.OnInterrupted;
        }

        //=== AnimationState

        /// <summary>
        /// `this.state` への参照を返します
        /// </summary>
        public AnimationState GetState()
        {
            return this.state;
        }

        //=== Control

        /// <summary>
        /// アニメーションをセットします.
        /// - `trackIndex` 再生Track
        /// - `name`       アニメーション名
        /// - `loop`       true=ループ状態
        /// </summary>
        public TrackEntry Set( int trackIndex, string name, bool loop=false )
        {
            return this.GetState().SetAnimation(trackIndex, name, loop);
        }

        /// <summary>
        /// アニメーションを追加します.
        /// </summary>
        public TrackEntry Add( int trackIndex, string name, bool loop=false, float delay=0f )
        {
            return this.GetState().AddAnimation(trackIndex, name, loop, delay);
        }

        /// <summary>
        /// アニメーションをクリアします
        /// </summary>
        public void Clear( int trackIndex )
        {
            this.GetState().ClearTrack(trackIndex);
        }

        //=== Delegate Track Event Handling

        /// <summary>
        /// handlerをコールバックします.
        /// </summary>
        private void DelegateTrackEventHandling( HandleTrackEvent handler, TrackEntry entry )
        {
            if ( handler == null )
                return;

            EventArgument arg = new EventArgument();
            arg.Entry = entry;

            handler(this, arg);
        }

        /// <summary>
        /// Skeleton Animation 開始時にコールバックされます.
        /// </summary>
        private void OnStarted( TrackEntry entry )
        {
            this.DelegateTrackEventHandling(this.StartHandler, entry);
        }
        
        /// <summary>
        /// Skeleton Animation 終了時にコールバックされます.
        /// Loop=ON の場合は、１回終了の度に呼ばれます
        /// </summary>
        private void OnCompleted( TrackEntry entry )
        {
            this.DelegateTrackEventHandling(this.CompleteHandler, entry);
            // this.RemovePlayingAnim(_trackEntry);

            // Debug.LogFormat("#SpineModel Skeleton animation Completed.\n [name]{0}, [duration]{1}",
            //                   _trackEntry.Animation.Name, _trackEntry.Animation.Duration);   
        }

        /// <summary>
        /// Skeleton Animation が次に切り替わる時点でコールバックされます
        /// </summary>
        private void OnEnded( TrackEntry entry )
        {
            // this.RemovePlayingAnim(_trackEntry);            
            this.DelegateTrackEventHandling(this.EndHandler, entry);
        }

        /// <summary>
        /// 実行中の Skeleton Animation が中断された時にコールバックされます
        /// </summary>
        private void OnInterrupted( TrackEntry entry )
        {
            // this.RemovePlayingAnim(_trackEntry);
            this.DelegateTrackEventHandling(this.InterruptHandler, entry);
        }
                
    }
}