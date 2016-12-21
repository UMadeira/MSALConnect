using System.Threading.Tasks;

namespace MSALConnect.Services
{
    public interface IGraphService {
        Task<string> GetMyEmailAddress();

        Task<byte[]> GetPhoto();

        Task<byte[]> GetUserPhoto( string aUser );

        Task<string> SendEmail( IMessageRequest aMessage );
    }
}