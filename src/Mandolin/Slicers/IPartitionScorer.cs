namespace Mandolin.Slicers
{
    using System.Text;
    using System.Threading.Tasks;

    public interface IPartitionScorer
    {
        int Score(string testMethod);
    }
}
