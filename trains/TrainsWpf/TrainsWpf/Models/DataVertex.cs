using GraphX;

namespace TrainsWpf.Models
{
    public class DataVertex : VertexBase
    {
        public DataVertex(string name)
        {
            Text = name;
        }

        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
