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

    public static class Either
    {
        public static Either<V, F> Some<V, F>(V value) =>
            Either<V, F>.Some(value);

        public static Either<V, string> SomeString<V>(V value) =>
            Either<V, string>.Some(value);

        public static Either<V, F> Fail<V, F>(F failure) =>
            Either<V, F>.Fail(failure);

        //public static Option<Nothing> None() =>
        //    Option<Nothing>.None;

        //public static Option<T> None<T>() =>
        //    Option<T>.None;

        public static Either<V, F> Catch<V, F>(Func<Either<V, F>> func)
        {
            try
            {
                return func();
            }
            catch (HasFailedException<F> e)
            {
                return e.Failure;
            }
        }
    }

    public readonly struct Either<V, F>
    {
        private readonly V value;
        private readonly F failure;
        public readonly EitherState State;

        public bool HasValue =>
            State == EitherState.Value;

        public bool HasFailed =>
            State == EitherState.Failed;

        public bool HasNone =>
            State == EitherState.None;

        public V Value => State switch
        {
            EitherState.Value => value,
            EitherState.Failed => throw new HasFailedException<F>("The either has failed with", failure),
            EitherState.None => throw new NoneException("The either has a 'none' value"),
            _ => throw new NotImplementedException(),
        };

        public F Failure => State switch
        {
            EitherState.Failed => failure,
            EitherState.Value => throw new HasValueException("The either has not failed, it has value", value),
            EitherState.None => throw new NoneException("The either has a 'none' value"),
            _ => throw new NotImplementedException(),
        };

        public void Match<T>(Func<V, T> value, Func<F, T> failure, Func<T> none)
        {

        }

        private Either(V value, F failure, EitherState state)
        {
            this.value = value;
            this.failure = failure;
            this.State = state;
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

        public static implicit operator V(Either<V, F> option) => option.Value;
        public static implicit operator Either<V, F>(V value) => IsNull(value)
            ? None
            : Some(value);

        public static implicit operator Either<V, F>(F failure) => IsNull(failure)
            ? None
            : Fail(failure);

        public override string ToString() => State switch
        {
            EitherState.Value => value.ToString(),
            EitherState.Failed => failure.ToString(),
            EitherState.None => "None",
            _ => throw new NotImplementedException(),
        };


    }
}
