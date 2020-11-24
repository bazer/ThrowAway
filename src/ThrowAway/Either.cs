using System;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public enum EitherState
    {
        None = 0,
        Failed = 1,
        Value = 2
    }

    class Either
    {
    }

    public readonly struct Either<V, F>
    {
        private readonly V value;
        private readonly F failure;
        private readonly EitherState state;

        public bool HasValue =>
            state == EitherState.Value;

        public bool HasFailed =>
            state == EitherState.Failed;

        public bool HasNone =>
            state == EitherState.None;

        public V Value => state switch
        {
            EitherState.Value => value,
            EitherState.Failed => throw new FailException("The either has failed"),
            EitherState.None => throw new NoneException("The either has a 'none' value"),
            _ => throw new NotImplementedException(),
        };

        public F Failure => state switch
        {
            EitherState.Failed => failure,
            EitherState.Value => throw new ValueException("The either has a value, it hasn't failed"),
            EitherState.None => throw new NoneException("The either has a 'none' value"),
            _ => throw new NotImplementedException(),
        };

        private Either(V value, F failure, EitherState state)
        {
            this.value = value;
            this.failure = failure;
            this.state = state;
        }

        public static Either<V, F> Some(V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Either<V, F>(value, default, EitherState.Value);
        }

        public static Either<V, F> Fail(F failure)
        {
            if (IsNull(failure))
                throw new ValueIsNullException("'Fail' cannot be called with a 'null' value");

            return new Either<V, F>(default, failure, EitherState.Failed);
        }

        public static Either<V, F> None
            => new Either<V, F>(default, default, EitherState.None);

        public override string ToString() => state switch
        {
            EitherState.Value => value.ToString(),
            EitherState.Failed => failure.ToString(),
            EitherState.None => "None",
            _ => throw new NotImplementedException(),
        };
    }
}
