using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIBlocks.MaterialDesign
{
    //要实现SmartHint,就要实现此接口。
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
