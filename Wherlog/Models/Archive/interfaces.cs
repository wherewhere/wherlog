using Wherlog.Models.Post;

namespace Wherlog.Models.Archive
{
    public interface IArchiveDetail<out TInfo> : ICount<TInfo>, IPost<TInfo> where TInfo : IInfo;
}
