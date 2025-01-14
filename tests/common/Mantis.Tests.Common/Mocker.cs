﻿using System.Linq.Expressions;
using Moq;

namespace Mantis.Tests.Common
{
    public abstract class Mocker
    {
        public abstract object GetInstance();
    }

    public class Mocker<T> : Mocker
        where T : class
    {
        private T? _override;
        private readonly Mock<T> _instance = new();

        public static Mocker<T> Create()
        {
            Mocker<T> builder = new();

            return builder;
        }

        public Mocker<T> Setup<TResult>(Expression<Func<T, TResult>> expression, TResult result)
        {
            this._instance.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> result)
        {
            this._instance.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1>(Expression<Func<T, TResult>> expression, Func<T1, TResult> result)
        {
            this._instance.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1, T2>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> result)
        {
            this._instance.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1, T2, T3>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> result)
        {
            this._instance.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1, T2, T3, T4>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> result)
        {
            this._instance.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression)
        {
            this._instance.Verify(expression);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression, string failMessage)
        {
            this._instance.Verify(expression, failMessage);

            return this;
        }


        public Mocker<T> Verify(Expression<Action<T>> expression, Times times, string failMessage)
        {
            this._instance.Verify(expression, times, failMessage);

            return this;
        }


        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression)
        {
            this._instance.Verify(expression);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression, string failMessage)
        {
            this._instance.Verify(expression, failMessage);

            return this;
        }


        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression, Times times, string failMessage)
        {
            this._instance.Verify(expression, times, failMessage);

            return this;
        }

        public void SetInstance(T instance)
        {
            this._override = instance;
        }

        public Mock<T> AsMock()
        {
            return this._instance;
        }

        public override T GetInstance()
        {
            return this._override ?? this._instance.Object;
        }

        public Lazy<T> GetLazy()
        {
            return new Lazy<T>(this.GetInstance);
        }

        public Lazy<TOut> GetLazy<TOut>()
            where TOut : class
        {
            return new Lazy<TOut>(() => this.GetInstance() as TOut ?? throw new NotImplementedException());
        }

        public static implicit operator Mock<T>(Mocker<T> mocker)
        {
            return mocker.AsMock();
        }

        public static implicit operator T(Mocker<T> mocker)
        {
            return mocker.GetInstance();
        }
    }
}