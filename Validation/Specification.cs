namespace DiagnosisRepositoryApi.Validation;

public class Specification<T> : ISpecification<T>
{
    public Func<T, bool> _predicate;

    public Specification(Func<T, bool> Predicate)
    {
        _predicate = Predicate;
    }

    public Specification<T> And(ISpecification<T> other)
    {
        return new Specification<T>(x=>this.isSatisfiedBy(x) && other.isSatisfiedBy(x));
    }

    public bool isSatisfiedBy(T entity)
    {
        return _predicate(entity);
    }

    public Specification<T> Or(ISpecification<T> other)
    {
        return new Specification<T>(x=>this.isSatisfiedBy(x) || other.isSatisfiedBy(x));
    }
}
