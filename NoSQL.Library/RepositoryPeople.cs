namespace NoSQL.Library
{

    public abstract class RepositoryPeopleAbstract:
        Repository<People>,
        IRepository<People>
    {
        public RepositoryPeopleAbstract(Documents doc)
            :base(doc, "people")
        {
        }
    }

    public class RepositoryPeople : RepositoryPeopleAbstract
    {
        public RepositoryPeople(Documents doc) :
            base(doc)
        {
        }
    }
}
