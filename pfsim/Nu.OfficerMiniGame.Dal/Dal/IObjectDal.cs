using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Dal.Dal
{
    public interface IObjectDal<T> where T : class
    {
        List<string> GetNames();

        T Get(string name);

        void Update(string name, T crewMember);

        bool Create(string name, T crewMember);

        void Delete(string name);

        bool Exists(string name);

    }
}