using UnityEngine;

namespace SnakeSurvivors
{
    [CreateAssetMenu(fileName = "SpatialPartitioner", menuName = "ScriptableObjects/Collision/SpatialPartitioner", order = 0)]
    public class SPPartitioner : ScriptableObject
    {
        [SerializeField] private float partitionWidth;
        public float PartitionWidth => partitionWidth;
        
        [SerializeField] private float partitionHeight;
        public float PartitionHeight => partitionHeight;

        public SpatialPartitioner SpatialPartitioner { get; private set; }
        
        public void Attach(SpatialPartitioner spatialPartitioner)
        {
            if (SpatialPartitioner != null) return;
            
            SpatialPartitioner = spatialPartitioner;
        }
    }
}