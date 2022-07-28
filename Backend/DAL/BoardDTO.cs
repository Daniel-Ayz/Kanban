using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardDTO : DTO
    {
        internal const string boardIdColumnName = "id";
        internal const string boardNameColumnName = "name";
        internal const string boardOwnerEmailColumnName = "ownerEmail";

        private int id;
        private string name;
        private string ownerEmail;
        internal int Id { get => id;  }
        internal string Name { get => name; }
        internal string OwnerEmail { get => ownerEmail; set { ownerEmail = value; ((BoardMapper)mapper).Update(id, boardOwnerEmailColumnName, ownerEmail); }  } 

        internal BoardDTO(int id, string name, string ownerEmail) : base(new BoardMapper())
        {
            this.id=id;
            this.name=name;
            this.ownerEmail=ownerEmail;
        }

        internal void Persist()
        {
            ((BoardMapper)mapper).Insert(this);
            ((BoardMapper)mapper).JoinBoard(ownerEmail, id);
        }
    }
}
