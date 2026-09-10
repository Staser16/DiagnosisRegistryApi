namespace DiagnosisRepositoryApi.Validation;

public interface ISpecification<T>
{
    public bool isSatisfiedBy(T entity);
    public Specification<T> And(ISpecification<T> other);
    public Specification<T> Or(ISpecification<T> other);
}
