using System.Collections.Generic;

namespace Tgs.Global.ViewModels
{
    public class ResponseViewModel<TItem>
    {
        public IEnumerable<TItem> Items { get; set; } = default!;
    }
}