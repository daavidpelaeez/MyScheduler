using System;
using System.Collections.Generic;
using System.Text;

namespace MyScheduler.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Success cant have an error");
            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Failure should have almost 1 error");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("You cant access to a value if the result is an error");
                return _value;
            }
        }

        protected internal Result(bool isSuccess, T value, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty);
        public static new Result<T> Failure(string error) => new Result<T>(false, default!, error);
    }
}

