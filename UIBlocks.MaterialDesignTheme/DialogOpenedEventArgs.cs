using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIBlocks.MaterialDesignTheme
{
    public class DialogOpenedEventArgs:RoutedEventArgs
    {
        public DialogOpenedEventArgs(DialogSession session,RoutedEvent routedevent)
        {
            if(session==null) throw new ArgumentNullException(nameof(session));
            Session = session;
            RoutedEvent = routedevent;
        }
        public DialogSession  Session { get; }


    }
}
