using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;


namespace TPS.Engine.Features.StateMachine
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public abstract class StateMachine
    {
        public static readonly IState InitialState = new InitialState();

        public IState Current { get => _current;}

        private IState _current = InitialState;

        protected abstract IReadOnlyDictionary<int, (IState From, IState To)> Transitions { get; }

        private static class UniqueNumberHolder
        {
            public static int Value;
        }

        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private static class UniqueId<T>
        {
            public static int Value { get; } = UniqueNumberHolder.Value++;
        }

        public static int CreateKey<T>() => UniqueId<T>.Value;

        public async UniTask TransitAsync<TTrigger>(CancellationToken cancellation = default)
        {
            if (cancellation.IsCancellationRequested) return;


            if (Transitions.TryGetValue(UniqueId<TTrigger>.Value, out var transit))
            {
                if (_current == transit.From)
                {
                    if (_current is IState.IWithExitAction exit) await exit.OnExitAsync(cancellation);

                    _current = transit.To;

                    if (_current is IState.IWithEnterAction enter) await enter.OnEnterAsync(cancellation);

                    if (_current is IState.IWithUpdateLoop update) await foreach
                    (
                        var _ in UniTaskAsyncEnumerable
                            .EveryUpdate(update.LoopTiming)
                            .WithCancellation(cancellation)
                    ) {
                        await update.OnUpdate(cancellation);
                    }
                }
            }
        }

        public sealed class Deterministic : StateMachine
        {
            public Deterministic(IEnumerable<KeyValuePair<int, (IState From, IState To)>> transitions)
            {
                Transitions = new Dictionary<int, (IState From, IState To)>(transitions);
            }

            protected override IReadOnlyDictionary<int, (IState From, IState To)> Transitions { get; }
        }
    }


}
