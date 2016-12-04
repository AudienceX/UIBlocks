using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIBlocks.MaterialDesign
{
    public interface IHintProxy:IDisposable
    {
        object Content { get; }

        bool IsLoaded { get; }

        bool IsVisible { get; }

        event EventHandler ContentChanged;

        event EventHandler IsVisibleChanged;

        event EventHandler Loaded;
    }
}
