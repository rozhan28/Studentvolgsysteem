using StudentSysteem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IVaardigheidService
    {
        public List<Vaardigheid> HaalAlleVaardighedenOp();
    }
}
