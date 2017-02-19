using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQL.Library;
namespace NoSQL.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            Documents doc = new Documents();
            RepositoryPeople rep = new RepositoryPeople(doc);

            //People p = new People();
            //p.Active = true;
            //p.Name = "Pessoa 2";

            //var result = rep.Insert(p).Result;

            var get = rep.Get("7a6afeec-a378-46f1-a3ad-d25fa6b99fec").Result;
            get.Name = "Alterando registro 7a6afeec-a378-46f1-a3ad-d25fa6b99fec";

            var r = rep.Update(get).Result;

        }
    }
}
