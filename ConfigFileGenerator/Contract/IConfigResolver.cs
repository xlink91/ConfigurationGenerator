using System;
using System.Linq.Expressions;

namespace ConfigFileGenerator.Contract
{
    public interface IConfigResolver
    {
        TResult Resolve<TEntity, TResult>(Expression<Func<TEntity, TResult>> member);
    }
}
