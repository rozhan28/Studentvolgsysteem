using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IClusterService
{
    public Cluster? HaalOp();
    public IEnumerable<Cluster> HaalAlleClustersOp();
}