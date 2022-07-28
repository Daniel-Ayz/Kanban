using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL
{
    internal abstract class DTO
    {
        protected Mapper mapper;

        internal Mapper Mapper { get { return mapper; } }
        protected DTO(Mapper map)
        {
            mapper = map;
        }
    }
}
