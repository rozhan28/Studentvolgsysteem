using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IFeedbackFormRepositories
    {
        public void CreateTable(string commandText);
        public void InsertMultipleWithTransaction(List<string> linesToInsert);
    }
}
