using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otter {
    /// <summary>
    /// A Component to manage and process queue of events.
    /// </summary>
    public class EventQueue : Component {

        #region Public Fields

        /// <summary>
        /// The list of EventQueueEvents to execute.
        /// </summary>
        public List<EventQueueEvent> Events = new List<EventQueueEvent>();

        #endregion Public Fields

        #region Private Fields

        bool isFreshEvent = true;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The current event that is being executed.
        /// </summary>
        public EventQueueEvent CurrentEvent { get; private set; }

        /// <summary>
        /// True if the number of events in the queue is greater than zero.
        /// </summary>
        public bool HasEvents {
            get {
                return Events.Count > 0;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Add events to the queue.
        /// </summary>
        /// <param name="evt">The events to add.</param>
        public void Add(params EventQueueEvent[] evt) {
            Events.AddRange(evt);
        }

        /// <summary>
        /// Push events into the front of the queue.
        /// </summary>
        /// <param name="evt">The events to push.</param>
        public void Push(params EventQueueEvent[] evt) {
            Events.InsertRange(0, evt);
        }

        public override void Update() {
            base.Update();

            if (CurrentEvent == null) {
                NextEvent();
            }

            while (CurrentEvent != null) {
                if (isFreshEvent) {
                    isFreshEvent = false;
                    CurrentEvent.EventQueue = this;
                    CurrentEvent.Start();
                    CurrentEvent.Enter();
                }

                CurrentEvent.Update();
                CurrentEvent.Timer += Entity.Game.DeltaTime;

                if (CurrentEvent.IsFinished) {
                    isFreshEvent = true;
                    CurrentEvent.Exit();
                    CurrentEvent.EventQueue = null;
                    Events.Remove(CurrentEvent);
                    NextEvent();
                }
                else {
                    break;
                }
            }
            
        }

        #endregion Public Methods

        #region Private Methods

        void NextEvent() {
            if (Events.Count > 0) {
                CurrentEvent = Events[0];
            }
            else {
                CurrentEvent = null;
            }
        }

        #endregion Private Methods
    }

    public class EventQueueEvent {

        #region Public Fields

        /// <summary>
        /// The EventQueue that this event belongs to.
        /// </summary>
        public EventQueue EventQueue;

        /// <summary>
        /// The elapsed time for this event.
        /// </summary>
        public float Timer = 0;

        #endregion Public Fields

        #region Public Properties

        /// <summary>
        /// Whether or not the Event has finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Called when the Event first starts.
        /// </summary>
        public virtual void Enter() {

        }

        /// <summary>
        /// Called when the Event finishes and is cleared from the queue.
        /// </summary>
        public virtual void Exit() {

        }

        /// <summary>
        /// Finishes the event.
        /// </summary>
        public void Finish() {
            IsFinished = true;
        }

        /// <summary>
        /// Starts the event.
        /// </summary>
        public void Start() {
            IsFinished = false;
            Timer = 0;
        }

        /// <summary>
        /// Called every update from the EventQueue.
        /// </summary>
        public virtual void Update() {

        }

        #endregion Public Methods
    }
}
