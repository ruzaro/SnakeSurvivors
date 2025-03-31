namespace SnakeSurvivors
{
    // TODO remove this
    public static class Singletons
    {
        public static readonly SpatialPartitioner<Enemy> EnemySpatialPartitioner = new(Const.Partitions.Width, Const.Partitions.Height);
    }
}