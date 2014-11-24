namespace Orc.GraphExplorer.Services
{
    using Models;

    public class DataVertexFactory : IDataVertexFactory
    {
        private const int FakeVertexId = -666;

        public DataVertex CreateFakeVertex()
        {
            return new DataVertex(FakeVertexId);
        }

        private static int _maxId = 0;     

        public DataVertex CreateVertex()
        {
            return new DataVertex(++_maxId);
        }

        public DataVertex CreateVertex(int id)
        {
            if (id > _maxId)
            {
                _maxId = id + 1;
            }

            return new DataVertex(id);
        }

        public bool IsFakeVertex(DataVertex vertex)
        {
            return vertex.ID == FakeVertexId;
        }
    }
}