using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TPS.Engine.Features.StateMachine
{
    public interface IState
    {
        public interface IWithEnterAction
        {
            UniTask OnEnterAsync(CancellationToken cancellation = default);
        }

        public interface IWithExitAction
        {
            UniTask OnExitAsync(CancellationToken cancellation = default);
        }

        public interface IWithUpdateLoop
        {
            UniTask OnUpdate(CancellationToken cancellation = default);
            PlayerLoopTiming LoopTiming { get; }
        }
    }

    internal sealed class InitialState : IState { }
}
