using System;
using System.Collections.Generic;

namespace GObject
{
    /// <summary>
    /// Base class for signal based events.
    /// </summary>
    public class SignalArgs : EventArgs
    {
        #region Properties

        private object[] Args { get; set; }

        #endregion

        #region Constructors

        public SignalArgs()
        {
            Args = Array.Empty<object>();
        }

        internal SignalArgs(params Value[] args)
            : this()
        {
            SetArgs(args);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override this method to populate a SignalArgs-derived type
        /// from an array of Values.
        /// </summary>
        public virtual void SetArgs(Value[] args) {}

        #endregion
    }

    public class Signal
    {
        #region Properties

        /// <summary>
        /// The name of the GSignal.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The signal flags.
        /// </summary>
        public SignalFlags Flags { get; }

        /// <summary>
        /// The return type of signal handlers.
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// The type of parameters in signal handlers.
        /// </summary>
        public Type[] ParamTypes { get; }

        #endregion
        
        #region Constructors

        internal Signal(string name, SignalFlags flags, Type returnType, Type[] paramTypes)
        {
            Name = name;
            Flags = flags;
            ReturnType = returnType;
            ParamTypes = paramTypes;
        }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Registers a new GSignal into this type.
        /// </summary>
        /// <param name="name">The name of the GSignal to create.</param>
        /// <param name="flags">The GSignal flags.</param>
        /// <param name="returnType">The type of the value returned by the handlers of this GSignal.</param>
        /// <param name="paramTypes">
        /// The types list for each parameters given to handlers of this GSignal,
        /// in the order they appear.</param>
        /// <returns>
        /// An instance of <see cref="Signal"/> which describes the registered signal.
        /// </returns>
        public static Signal Register(string name, SignalFlags flags, Type returnType, params Type[] paramTypes)
        {
            return new Signal(name, flags, returnType, paramTypes);
        }

        /// <summary>
        /// Registers a new GSignal into this type.
        /// </summary>
        /// <param name="name">The name of the GSignal to create.</param>
        /// <param name="flags">The GSignal flags.</param>
        /// <returns>
        /// An instance of <see cref="Signal"/> which describes the registered signal.
        /// </returns>
        public static Signal Register(string name, SignalFlags flags = SignalFlags.run_last)
        {
            return new Signal(name, flags, Type.None, Array.Empty<Type>());
        }

        /// <summary>
        /// Wraps an existing GSignal.
        /// </summary>
        /// <param name="name">The name of the GSignal to wrap.</param>
        /// <returns>
        /// An instance of <see cref="Signal"/> which describes the signal to wrap.
        /// </returns>
        public static Signal Wrap(string name)
        {
            // Here only the signal name is relevant, other parameters are not used.
            return new Signal(name, SignalFlags.run_last, Type.None, Array.Empty<Type>());
        }
        
        /// <summary>
        /// Connects an <paramref name="action"/> to this signal.
        /// </summary>
        /// <param name="o">The object on which connect the handler.</param>
        /// <param name="action">The signal handler function.</param>
        /// <param name="after">
        /// Define if this action must be called before or after the default handler of this signal.
        /// </param>
        public void Connect(Object o, Action action, bool after = false)
        {
            if (action == null)
                return;

            o.RegisterEvent(Name, action, after);
        }

        /// <summary>
        /// Connects an <paramref name="action"/> to this signal.
        /// </summary>
        /// <param name="o">The object on which connect the handler.</param>
        /// <param name="action">The signal handler function.</param>
        /// <param name="after">
        /// Define if this action must be called before or after the default handler of this signal.
        /// </param>
        public void Connect(Object o, ActionRefValues action, bool after = false)
        {
            if (action == null)
                return;

            o.RegisterEvent(Name, action, after);
        }

        /// <summary>
        /// Disconnects an <paramref name="action"/> previously connected to this signal.
        /// </summary>
        /// <param name="o">The object from which disconnect the handler.</param>
        /// <param name="action">The signal handler function.</param>
        public void Disconnect(Object o, Action action)
        {
            if (action == null)
                return;

            o.UnregisterEvent(action);
        }

        /// <summary>
        /// Disconnects an <paramref name="action"/> previously connected to this signal.
        /// </summary>
        /// <param name="o">The object from which disconnect the handler.</param>
        /// <param name="action">The signal handler function.</param>
        public void Disconnect(Object o, ActionRefValues action)
        {
            if (action == null)
                return;

            o.UnregisterEvent(action);
        }
        #endregion
    }
    
    /// <summary>
    /// Describes a GSignal.
    /// </summary>
    public class Signal<T> : Signal where T : SignalArgs, new()
    {
        #region Fields
        
        private static readonly Dictionary<EventHandler<T>, ActionRefValues> Handlers = new Dictionary<EventHandler<T>, ActionRefValues>();
        
        #endregion
        
        #region Constructors

        internal Signal(string name, SignalFlags flags, Type returnType, Type[] paramTypes) : base(name, flags, returnType, paramTypes){ }

        #endregion

        #region Methods

        /// <summary>
        /// Wraps an existing GSignal.
        /// </summary>
        /// <param name="name">The name of the GSignal to wrap.</param>
        /// <returns>
        /// An instance of <see cref="Signal"/> which describes the signal to wrap.
        /// </returns>
        public new static Signal<T> Wrap(string name)
        {
            // Here only the signal name is relevant, other parameters are not used.
            return new Signal<T>(name, SignalFlags.run_last, Type.None, Array.Empty<Type>());
        }

        /// <summary>
        /// Connects an <paramref name="action"/> to this signal.
        /// </summary>
        /// <param name="o">The object on which connect the handler.</param>
        /// <param name="action">The signal handler function.</param>
        /// <param name="after">
        /// Define if this action must be called before or after the default handler of this signal.
        /// </param>
        public void Connect(Object o, EventHandler<T> action, bool after = false)
        {
            if (action == null)
                return;

            if (!Handlers.TryGetValue(action, out ActionRefValues callback))
            {
                callback = (ref Value[] values) =>
                {
                    var args = new T();
                    args.SetArgs(values);
                    action(o, args);
                };
            }

            o.RegisterEvent(Name, callback, after);
            Handlers[action] = callback;
        }

        /// <summary>
        /// Disconnects an <paramref name="action"/> previously connected to this signal.
        /// </summary>
        /// <param name="o">The object from which disconnect the handler.</param>
        /// <param name="action">The signal handler function.</param>
        public void Disconnect(Object o, EventHandler<T> action)
        {
            if (action == null)
                return;

            if (!Handlers.TryGetValue(action, out ActionRefValues callback))
                return;

            o.UnregisterEvent(callback);
            Handlers.Remove(action);
        }

        #endregion
    }
}
