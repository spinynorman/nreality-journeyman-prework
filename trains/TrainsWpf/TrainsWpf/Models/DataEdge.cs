using GraphX;

namespace TrainsWpf.Models
{
    public class DataEdge : EdgeBase<DataVertex>
    {
        public DataEdge()
            : base(null, null, 1)
        {
        }

        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
            : base(source, target, weight)
        {
        }

        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
