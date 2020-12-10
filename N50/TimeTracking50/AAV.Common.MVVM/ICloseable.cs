using System.Threading.Tasks;
using MVVM.Common;

namespace AAV.Common.MVVM.AsLink
{
    public delegate Task WindowClosingEventHandler(object sender, CloseEventArgs e);

    public interface ICloseable
    {
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event WindowClosingEventHandler Closing;

        /// <summary>
        /// Raised before closing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Task BeforeClosing(CloseEventArgs e);

        /// <summary>
        /// Called after the window was closed
        /// </summary>
        void Closed(ClosedEventArgs e);
    }
}