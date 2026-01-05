using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface IClusterRepository
    {
        public Cluster? HaalOp();
        public List<Cluster> HaalAlleClustersOp();
    }
}
