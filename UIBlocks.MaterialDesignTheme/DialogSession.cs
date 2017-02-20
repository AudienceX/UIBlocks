using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UIBlocks.MaterialDesignTheme
{
    public class DialogSession
    {
        private readonly DialogHost _owner;

        internal DialogSession(DialogHost owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            _owner = owner;
        }

        /// <summary>
        /// 表示Dialog是否被关闭。
        /// </summary>
        public bool IsEnded { get; internal set; }

        public object Content => _owner.DialogContent;

        /// <summary>
        /// 更新Dialog中的内容。
        /// </summary>
        /// <param name="content"></param>
        public void UpdateContent(object content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            _owner.AssertTargetableContent();
            _owner.DialogContent = content;
            _owner.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => _owner.FocusPopup()));
        }

        /// <summary>
        /// 关闭Dialog。
        /// </summary>
        public void Close()
        {
            if (IsEnded) throw new InvalidOperationException("Dialog session has ended.");

            _owner.Close(null);
        }

        /// <summary>
        ///关闭对话框。
        /// </summary>
        /// <param name="parameter">Result parameter which will be returned in <see cref="DialogClosingEventArgs.Parameter"/> or from <see cref="DialogHost.Show(object)"/> method.</param>
        public void Close(object parameter)
        {
            if (IsEnded) throw new InvalidOperationException("Dialog session has ended.");

            _owner.Close(parameter);
        }

    }
}
