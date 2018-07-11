using OneTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneTest.Interfaces
{
    public interface IToDoRepository
    {
        bool DoesItemExist(string id);
        IEnumerable<ToDoOneItem> All { get; }
        ToDoOneItem Find(string id);
        void Insert(ToDoOneItem item);
        void Update(ToDoOneItem item);
        void Delete(string id);
    }
}
