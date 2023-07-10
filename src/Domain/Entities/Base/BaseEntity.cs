namespace CoverGoChallenge.src.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }
    }
}
