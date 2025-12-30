using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class ClusterService : IClusterService
{
    private readonly IClusterRepository _clusterRepository;

    public ClusterService(IClusterRepository clusterRepository)
    {
        _clusterRepository = clusterRepository;
    }
    
    public Cluster? HaalOp()
    {
        return _clusterRepository.HaalOp();
    }

    public List<Cluster> HaalAlleClustersOp()
    {
        List<Cluster> clusters = _clusterRepository.HaalAlleClustersOp();
        return clusters;
    }
}