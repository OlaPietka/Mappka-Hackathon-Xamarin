using System.IO;
using System.Threading.Tasks;

namespace MappkaMobile.Interfaces
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}
