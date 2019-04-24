namespace PerformanceConsole
{
    using System;

    using Voogd.Library.Messaging;

    /// <summary>
        /// Soorten van retry
        /// Immediate wordt direct opnieuw geprobeerd
        /// Redeliver wordt 3 seconden vertraagd
        /// NoRetry hierbij verdwijnt het commando volledig
        /// ImmediateError gelijk op de error bus
        /// </summary>
        public enum MyMessageRetry
        {
            ImmediateRetry,
            RedeliverRetry,
            NoRetry,
            ImmediateError
        }

        /// <summary>
        /// Wrapper voor exceptions van commandos
        /// </summary>
        public class MyMessageHandlerException : Exception
        {
            /// <summary>
            /// Initieert een message exception gebaseerd op een commando
            /// </summary>
            public MyMessageHandlerException(Command cmd, Exception ex, MyMessageRetry errorRetry = MyMessageRetry.RedeliverRetry)
                : base(cmd.ToString(), ex)
            {
                this.Command = cmd;
                this.ErrorRetry = errorRetry;
            }

            /// <summary>
            /// Initieert een message exception gebaseerd op een commando
            /// </summary>
            public MyMessageHandlerException(Event evt, Exception ex, MyMessageRetry errorRetry = MyMessageRetry.RedeliverRetry)
                : base(evt.ToString(), ex)
            {
                this.Event = evt;
                this.ErrorRetry = errorRetry;
            }

            /// <summary>
            /// Initieert een message exception
            /// bij de message wordt aangegeven hoe de message geretried moet worden.
            /// Met deze methode kan het domain aangeven wat de vervolg actie moet zijn
            /// </summary>
            public MyMessageHandlerException(Exception ex, MyMessageRetry errorRetry, int retries = 0, TimeSpan? delay = null)
                : base(ex.ToString(), ex)
            {
                this.ErrorRetry = errorRetry;
                this.Retries = retries;
                this.Delay = delay;
            }


            /// <summary>
            /// Hoe moet er geretried worden
            /// </summary>
            public MyMessageRetry ErrorRetry { get; }

            /// <summary>
            /// Het oorspronkelijke commando
            /// </summary>
            public Command Command { get; }


            /// <summary>
            /// Het oorspronkelijke event
            /// </summary>
            public Event Event { get; }

            /// <summary>
            /// Maximum aantal retries
            /// </summary>
            public int Retries { get; }

            /// <summary>
            /// Delay tussen retries
            /// </summary>
            public TimeSpan? Delay { get; }

        }
    }