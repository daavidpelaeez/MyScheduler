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
                throw new InvalidOperationException("Un resultado exitoso no puede tener error.");
            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Un resultado fallido debe tener un error.");

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
                    throw new InvalidOperationException("No puedes acceder a Value cuando el resultado es fallo.");
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

