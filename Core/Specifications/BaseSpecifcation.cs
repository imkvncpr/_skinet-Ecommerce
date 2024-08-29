using System.Linq.Expressions;
using Core.Interfaces;
using Core.Specifications;

namespace Core.Specifications
{
    public class BaseSpecifcation<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
    {
        protected BaseSpecifcation() : this(null){}
                public Expression<Func<T, bool>>? Criteria => criteria;

        public List<Expression<Func<T, object>>>? OrderBy {get; private set; }

        public List<Expression<Func<T, object>>>? OrderByDescending {get; private set; }

        public bool IsDistinct {get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if(Criteria != null)
            {
                query = query.Where(Criteria);
            }
            
            return query;
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy ??= new List<Expression<Func<T, object>>>();
            OrderBy.Add(orderByExpression);
        }

         protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending ??= new List<Expression<Func<T, object>>>();
            OrderByDescending.Add(orderByDescExpression);
            
        }

        protected void ApplyDistinct()
        {
           IsDistinct = true;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}

public class BaseSpecifcation<T, TResult>(Expression<Func<T, bool>>? criteria) : 
    BaseSpecifcation<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecifcation() : this(null){}
    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}

