using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace UIBlocks.MaterialDesignTheme
{
    public enum DialogHostOpenDialogCommandDataContextSource
    {
        /// <summary>
        /// The data context from the sender element (typically a <see cref="Button"/>) 
        /// is applied to the content.
        /// </summary>
        SenderElement,
        /// <summary>
        /// The data context from the <see cref="DialogHost"/> is applied to the content.
        /// </summary>
        DialogHostInstance,
        /// <summary>
        /// The data context is explicitly set to <c>null</c>.
        /// </summary>
        None
    }
    public delegate void DialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs);
    public delegate void DialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs);
    public class DialogHost:ContentControl
    {
        public const string PopupPartName = "PART_Popup";
        public const string PopupContentPartName = "PART_PopupContentElement";
        public const string ContentCoverGridName = "PART_ContentCoverGrid";
        public const string OpenStateName = "Open";
        public const string ClosedStateName = "Closed";

        public static RoutedCommand OpenDialogCommand =new RoutedCommand();
        public static RoutedCommand CloseDialogCommand=new RoutedCommand();

        private static readonly HashSet<DialogHost> LoadedInstances=new HashSet<DialogHost>();

        private readonly ManualResetEvent _asyncShowWaitHandle= new ManualResetEvent(false);
        private DialogOpenedEventHandler _asyncShowOpenedEventHandler;
        private DialogClosingEventHandler _asyncShowClosingEventHandler;

        private DialogOpenedEventHandler _attachedDialogOpenedEventHandler;
        private DialogClosingEventHandler _attachedDialogClosingEventHandler;
        private Popup _popup;
        private ContentControl _popupContentControl;
        private Grid _contentCoverGrid;
        private DialogSession _session;
        private object _closeDialogExecutionParameter;
        private IInputElement _restoreFocus;
        private Action _closeCleanUp = () => { };

        static DialogHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHost),new FrameworkPropertyMetadata(typeof(DialogHost)));
        }

        public DialogHost()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            CommandBindings.Add(new CommandBinding(CloseDialogCommand, CloseDialogHandler, CloseDialogCanExecute));
            CommandBindings.Add(new CommandBinding(OpenDialogCommand, OpenDialogHandler));
        }

        #region Show overloads,Show方法重载
        /// <summary>
        /// 显示一个对话框。要使用, 一个 <see cref="DialogHost"/> 实例必须在 visual tree 中(通常在一个窗口的XAML代码根部)。
        /// </summary>
        /// <param name="content">要显示的内容 (可以是一个control或者viewmodel).</param>
        /// <returns>Task的结果是一个用于关闭dialog的参数，通常被传递给<see cref="CloseDialogCommand"/></returns>
        public static async Task<object> Show(object content)
        {
            return await Show(content, null, null);
        }

        /// <summary>
        /// 显示一个对话框。要使用, 一个 <see cref="DialogHost"/> 实例必须在 visual tree 中(通常在一个窗口的XAML代码根部)。
        /// </summary>
        /// <param name="content">要显示的内容 (可以是一个control或者viewmodel).</param>      
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>        
        /// <returns>Task的结果是一个用于关闭dialog的参数，通常被传递给<see cref="CloseDialogCommand"/></returns>
        public static async Task<object> Show(object content, DialogOpenedEventHandler openedEventHandler)
        {
            return await Show(content, null, openedEventHandler, null);
        }

        /// <summary>
        /// 显示一个对话框。要使用, 一个 <see cref="DialogHost"/> 实例必须在 visual tree 中(通常在一个窗口的XAML代码根部)。
        /// </summary>
        /// <param name="content">要显示的内容 (可以是一个control或者viewmodel).</param>      
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <returns>Task的结果是一个用于关闭dialog的参数，通常被传递给<see cref="CloseDialogCommand"/></returns>
        public static async Task<object> Show(object content, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(content, null, null, closingEventHandler);
        }

        /// <summary>
        /// 显示一个对话框。要使用, 一个 <see cref="DialogHost"/> 实例必须在 visual tree 中(通常在一个窗口的XAML代码根部)。
        /// </summary>
        /// <param name="content">要显示的内容 (可以是一个control或者viewmodel).</param>           
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <returns>Task的结果是一个用于关闭dialog的参数，通常被传递给<see cref="CloseDialogCommand"/></returns>
        public static async Task<object> Show(object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(content, null, openedEventHandler, closingEventHandler);
        }

        /// <summary>
        /// Shows a modal dialog. To use, a <see cref="DialogHost"/> instance must be in a visual tree (typically this may be specified towards the root of a Window's XAML).
        /// </summary>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="dialogIdentifier"><see cref="Identifier"/> of the instance where the dialog should be shown. Typically this will match an identifer set in XAML. <c>null</c> is allowed.</param>
        /// <returns>Task result is the parameter used to close the dialog, typically what is passed to the <see cref="CloseDialogCommand"/> command.</returns>
        public static async Task<object> Show(object content, object dialogIdentifier)
        {
            return await Show(content, dialogIdentifier, null, null);
        }

        /// <summary>
        /// Shows a modal dialog. To use, a <see cref="DialogHost"/> instance must be in a visual tree (typically this may be specified towards the root of a Window's XAML).
        /// </summary>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="dialogIdentifier"><see cref="Identifier"/> of the instance where the dialog should be shown. Typically this will match an identifer set in XAML. <c>null</c> is allowed.</param>
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <returns>Task result is the parameter used to close the dialog, typically what is passed to the <see cref="CloseDialogCommand"/> command.</returns>
        public static Task<object> Show(object content, object dialogIdentifier, DialogOpenedEventHandler openedEventHandler)
        {
            return Show(content, dialogIdentifier, openedEventHandler, null);
        }

        /// <summary>
        /// Shows a modal dialog. To use, a <see cref="DialogHost"/> instance must be in a visual tree (typically this may be specified towards the root of a Window's XAML).
        /// </summary>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="dialogIdentifier"><see cref="Identifier"/> of the instance where the dialog should be shown. Typically this will match an identifer set in XAML. <c>null</c> is allowed.</param>        
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <returns>Task result is the parameter used to close the dialog, typically what is passed to the <see cref="CloseDialogCommand"/> command.</returns>
        public static Task<object> Show(object content, object dialogIdentifier, DialogClosingEventHandler closingEventHandler)
        {
            return Show(content, dialogIdentifier, null, closingEventHandler);
        }

        /// <summary>
        /// Shows a modal dialog. To use, a <see cref="DialogHost"/> instance must be in a visual tree (typically this may be specified towards the root of a Window's XAML).
        /// </summary>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="dialogIdentifier"><see cref="Identifier"/>定义了Dialog应该在哪里显示。通常该属性在XAML中设置 ，值可以为<c>null</c> .</param>
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <returns>Task result is the parameter used to close the dialog, typically what is passed to the <see cref="CloseDialogCommand"/> command.</returns>
        public static async Task<object> Show(object content, object dialogIdentifier, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            if (LoadedInstances.Count == 0)
                throw new InvalidOperationException("没有DialogHost实例被加载。");
            LoadedInstances.First().Dispatcher.VerifyAccess();

            var targets = LoadedInstances.Where(dh => Equals(dh.Identifier, dialogIdentifier)).ToList();
            if (targets.Count == 0)
                throw new InvalidOperationException("没有DialogHost实例拥有和dialogIndetifier参数匹配的Identifier属性。");
            if (targets.Count > 1)
                throw new InvalidOperationException("Multiple viable DialogHosts.  Specify a unique Identifier on each DialogHost, especially where multiple Windows are a concern.");

            return await targets[0].ShowInternal(content, openedEventHandler, closingEventHandler);
        }

        internal async Task<object> ShowInternal(object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            if (IsOpen)
                throw new InvalidOperationException("对话框已经被打开。");
            AssertTargetableContent();
            DialogContent = content;
            _asyncShowOpenedEventHandler = openedEventHandler;
            _asyncShowClosingEventHandler = closingEventHandler;
            SetCurrentValue(IsOpenProperty, true);

            var task = new Task(() =>
            {
                _asyncShowWaitHandle.WaitOne();
            });
            task.Start();

            await task;

            _asyncShowOpenedEventHandler = null;
            _asyncShowClosingEventHandler = null;

            return _closeDialogExecutionParameter;
        }
        #endregion

        public override void OnApplyTemplate()
        {
            if (_contentCoverGrid != null)
                _contentCoverGrid.MouseLeftButtonUp -= ContentCoverGridOnMouseLeftButtonUp;

            _popup = GetTemplateChild(PopupPartName) as Popup;
            _popupContentControl = GetTemplateChild(PopupContentPartName) as ContentControl;
            _contentCoverGrid = GetTemplateChild(ContentCoverGridName) as Grid;

            if (_contentCoverGrid != null)
                _contentCoverGrid.MouseLeftButtonUp += ContentCoverGridOnMouseLeftButtonUp;

            VisualStateManager.GoToState(this, SelectState(), false);

            base.OnApplyTemplate();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadedInstances.Remove(this);
            SetCurrentValue(IsOpenProperty, false);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadedInstances.Add(this);
        }

        /// <summary>
        /// 是否在点击对话框外位置后关闭对话框
        /// </summary>
        private void ContentCoverGridOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (CloseOnClickAway)
                Close(CloseOnClickAwayParameter);
        }

        /// <summary>
        /// 返回当前开启状态
        /// </summary>
        private string SelectState()
        {
            return IsOpen ? OpenStateName : ClosedStateName;
        }


        internal void AssertTargetableContent()
        {
            var existindBinding = BindingOperations.GetBindingExpression(this, DialogContentProperty);
            if (existindBinding != null)
                throw new InvalidOperationException(
                    "Content cannot be passed to a dialog via the OpenDialog if DialogContent already has a binding.");
        }

        internal UIElement FocusPopup()
        {
            var child = _popup?.Child;
            if (child == null) return null;

            child.Focus();
            //TraversalRequest类，将焦点移到下一个控件。
            child.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            return child;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null && !window.IsActive)
                window.Activate();
            base.OnPreviewMouseDown(e);
        }


        private void OpenDialogHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (executedRoutedEventArgs.Handled) return;

            var dependencyObject = executedRoutedEventArgs.OriginalSource as DependencyObject;
            if (dependencyObject != null)
            {
                _attachedDialogOpenedEventHandler = GetDialogOpenedAttached(dependencyObject);
                _attachedDialogClosingEventHandler = GetDialogClosingAttached(dependencyObject);
            }

            if (executedRoutedEventArgs.Parameter != null)
            {
                AssertTargetableContent();

                if (_popupContentControl != null)
                {
                    switch (OpenDialogCommandDataContextSource)
                    {
                        case DialogHostOpenDialogCommandDataContextSource.SenderElement:
                            _popupContentControl.DataContext =
                                (executedRoutedEventArgs.OriginalSource as FrameworkElement)?.DataContext;
                            break;
                        case DialogHostOpenDialogCommandDataContextSource.DialogHostInstance:
                            _popupContentControl.DataContext = DataContext;
                            break;
                        case DialogHostOpenDialogCommandDataContextSource.None:
                            _popupContentControl.DataContext = null;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                DialogContent = executedRoutedEventArgs.Parameter;
            }

            SetCurrentValue(IsOpenProperty, true);

            executedRoutedEventArgs.Handled = true;
        }

        private void CloseDialogCanExecute(object sender, CanExecuteRoutedEventArgs canExecuteRoutedEventArgs)
        {
            canExecuteRoutedEventArgs.CanExecute = _session != null;
        }


        private void CloseDialogHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (executedRoutedEventArgs.Handled) return;

            Close(executedRoutedEventArgs.Parameter);

            executedRoutedEventArgs.Handled = true;
        }


        internal void Close(object parameter)
        {
            var dialogClosingEventArgs = new DialogClosingEventArgs(_session, parameter, DialogClosingEvent);

            _session.IsEnded = true;

            //多种可能关闭Dialog的方式
            OnDialogClosing(dialogClosingEventArgs);
            _attachedDialogClosingEventHandler?.Invoke(this, dialogClosingEventArgs);
            DialogClosingCallback?.Invoke(this, dialogClosingEventArgs);
            _asyncShowClosingEventHandler?.Invoke(this, dialogClosingEventArgs);

            if (!dialogClosingEventArgs.IsCancelled)
                SetCurrentValue(IsOpenProperty, false);
            else
                _session.IsEnded = false;

            _closeDialogExecutionParameter = parameter;
        }


        private static void WatchWindowActivation(DialogHost dialogHost)
        {
            var window = Window.GetWindow(dialogHost);
            if (window != null)
            {
                window.Activated += dialogHost.WindowOnActivated;
                window.Deactivated += dialogHost.WindowOnDeactivated;
                dialogHost._closeCleanUp = () =>
                {
                    window.Activated -= dialogHost.WindowOnActivated;
                    window.Deactivated -= dialogHost.WindowOnDeactivated;
                };
            }
            else
            {
                dialogHost._closeCleanUp = () => { };
            }
        }

        private void WindowOnDeactivated(object sender, EventArgs eventArgs)
        {
            _restoreFocus = _popup != null ? FocusManager.GetFocusedElement((Window)sender) : null;
        }

        private void WindowOnActivated(object sender, EventArgs eventArgs)
        {
            if (_restoreFocus != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Keyboard.Focus(_restoreFocus);
                }));
            }
        }

        #region OpenDialog相关事件和属性
        public static readonly RoutedEvent DialogOpenedEvent =EventManager.RegisterRoutedEvent("DialogOpened",RoutingStrategy.Bubble,typeof(DialogOpenedEventHandler),typeof(DialogHost));
        public event DialogOpenedEventHandler DialogOpened
        {
            add { AddHandler(DialogOpenedEvent, value); }
            remove { RemoveHandler(DialogOpenedEvent, value); }
        }

        #region DialogOpenedAttached，附加属性，用于添加打开Dialog委托
        public static readonly DependencyProperty DialogOpenedAttachedProperty = DependencyProperty.RegisterAttached(
    "DialogOpenedAttached", typeof(DialogOpenedEventHandler), typeof(DialogHost), new PropertyMetadata(default(DialogOpenedEventHandler)));
        public static void SetDialogOpenedAttached(DependencyObject element, DialogOpenedEventHandler value)
        {
            element.SetValue(DialogOpenedAttachedProperty, value);
        }
        public static DialogOpenedEventHandler GetDialogOpenedAttached(DependencyObject element)
        {
            return (DialogOpenedEventHandler)element.GetValue(DialogOpenedAttachedProperty);
        }

        #endregion

        #region 依赖属性DialogOpenedCallback，Dialog开启回调函数
        public static readonly DependencyProperty DialogOpenedCallbackProperty = DependencyProperty.Register(
    nameof(DialogOpenedCallback), typeof(DialogOpenedEventHandler), typeof(DialogHost), new PropertyMetadata(default(DialogOpenedEventHandler)));
        public DialogOpenedEventHandler DialogOpenedCallback
        {
            get { return (DialogOpenedEventHandler)GetValue(DialogOpenedCallbackProperty); }
            set { SetValue(DialogOpenedCallbackProperty, value); }
        }

        #endregion


        protected void OnDialogOpened(DialogOpenedEventArgs eventArgs)
        {
            RaiseEvent(eventArgs);
        }

        #endregion

        #region CloseDialog相关事件和属性
        public static readonly RoutedEvent DialogClosingEvent =EventManager.RegisterRoutedEvent("DialogClosing",RoutingStrategy.Bubble,typeof(DialogClosingEventHandler),typeof(DialogHost));
        /// <summary>
        /// 在Dialog关闭前触发
        /// </summary>
        public event DialogClosingEventHandler DialogClosing
        {
            add { AddHandler(DialogClosingEvent, value); }
            remove { RemoveHandler(DialogClosingEvent, value); }
        }

        #region DialogClosingAttached附加属性，添加关闭Dialog委托
        public static readonly DependencyProperty DialogClosingAttachedProperty = DependencyProperty.RegisterAttached(
            "DialogClosingAttached", typeof(DialogClosingEventHandler), typeof(DialogHost), new PropertyMetadata(default(DialogClosingEventHandler)));
        public static void SetDialogClosingAttached(DependencyObject element, DialogClosingEventHandler value)
        {
            element.SetValue(DialogClosingAttachedProperty, value);
        }
        public static DialogClosingEventHandler GetDialogClosingAttached(DependencyObject element)
        {
            return (DialogClosingEventHandler)element.GetValue(DialogClosingAttachedProperty);
        }
        #endregion

        #region DialogClosingCallback，依赖属性,添加关闭窗口委托
        public static readonly DependencyProperty DialogClosingCallbackProperty = DependencyProperty.Register(
    nameof(DialogClosingCallback), typeof(DialogClosingEventHandler), typeof(DialogHost), new PropertyMetadata(default(DialogClosingEventHandler)));
        public DialogClosingEventHandler DialogClosingCallback
        {
            get { return (DialogClosingEventHandler)GetValue(DialogClosingCallbackProperty); }
            set { SetValue(DialogClosingCallbackProperty, value); }
        }
        #endregion

        protected void OnDialogClosing(DialogClosingEventArgs eventArgs)
        {
            RaiseEvent(eventArgs);
        }

        #endregion

        #region 各种依赖属性
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(
    nameof(Identifier), typeof(object), typeof(DialogHost), new PropertyMetadata(default(object)));
        /// <summary>
        /// Identifier which is used in conjunction with <see cref="Show(object)"/> to determine where a dialog should be shown.
        /// </summary>
        public object Identifier
        {
            get { return GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
    nameof(DialogContent), typeof(object), typeof(DialogHost), new PropertyMetadata(default(object)));
        public object DialogContent
        {
            get { return (object)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        public static readonly DependencyProperty DialogContentTemplateProperty = DependencyProperty.Register(
    nameof(DialogContentTemplate), typeof(DataTemplate), typeof(DialogHost), new PropertyMetadata(default(DataTemplate)));
        public DataTemplate DialogContentTemplate
        {
            get { return (DataTemplate)GetValue(DialogContentTemplateProperty); }
            set { SetValue(DialogContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty DialogContentTemplateSelectorProperty = DependencyProperty.Register(
    nameof(DialogContentTemplateSelector), typeof(DataTemplateSelector), typeof(DialogHost), new PropertyMetadata(default(DataTemplateSelector)));
        public DataTemplateSelector DialogContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DialogContentTemplateSelectorProperty); }
            set { SetValue(DialogContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty DialogContentStringFormatProperty = DependencyProperty.Register(
    nameof(DialogContentStringFormat), typeof(string), typeof(DialogHost), new PropertyMetadata(default(string)));
        public string DialogContentStringFormat
        {
            get { return (string)GetValue(DialogContentStringFormatProperty); }
            set { SetValue(DialogContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty DialogMarginProperty = DependencyProperty.Register(
    "DialogMargin", typeof(Thickness), typeof(DialogHost), new PropertyMetadata(default(Thickness)));
        public Thickness DialogMargin
        {
            get { return (Thickness)GetValue(DialogMarginProperty); }
            set { SetValue(DialogMarginProperty, value); }
        }

        public static readonly DependencyProperty OpenDialogCommandDataContextSourceProperty = DependencyProperty.Register(
            nameof(OpenDialogCommandDataContextSource), typeof(DialogHostOpenDialogCommandDataContextSource), typeof(DialogHost), new PropertyMetadata(default(DialogHostOpenDialogCommandDataContextSource)));
        /// <summary>
        /// Defines how a data context is sourced for a dialog if a <see cref="FrameworkElement"/>
        /// is passed as the command parameter when using <see cref="DialogHost.OpenDialogCommand"/>.
        /// </summary>
        public DialogHostOpenDialogCommandDataContextSource OpenDialogCommandDataContextSource
        {
            get { return (DialogHostOpenDialogCommandDataContextSource)GetValue(OpenDialogCommandDataContextSourceProperty); }
            set { SetValue(OpenDialogCommandDataContextSourceProperty, value); }
        }

        public static readonly DependencyProperty CloseOnClickAwayProperty = DependencyProperty.Register(
    "CloseOnClickAway", typeof(bool), typeof(DialogHost), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Indicates whether the dialog will close if the user clicks off the dialog, on the obscured background.
        /// </summary>
        public bool CloseOnClickAway
        {
            get { return (bool)GetValue(CloseOnClickAwayProperty); }
            set { SetValue(CloseOnClickAwayProperty, value); }
        }

        public static readonly DependencyProperty CloseOnClickAwayParameterProperty = DependencyProperty.Register(
    "CloseOnClickAwayParameter", typeof(object), typeof(DialogHost), new PropertyMetadata(default(object)));
        /// <summary>
        /// Parameter to provide to close handlers if an close due to click away is instigated.
        /// </summary>
        public object CloseOnClickAwayParameter
        {
            get { return (object)GetValue(CloseOnClickAwayParameterProperty); }
            set { SetValue(CloseOnClickAwayParameterProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
         nameof(IsOpen), typeof(bool), typeof(DialogHost), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsOpenPropertyChangedCallback));

        private static void IsOpenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var dialogHost = (DialogHost)dependencyObject;

            if (dialogHost._popupContentControl != null)
                ValidationEditor.SetSuppress(dialogHost._popupContentControl, !dialogHost.IsOpen);
            VisualStateManager.GoToState(dialogHost, dialogHost.SelectState(), !TransitionEditor.GetDisableTransitions(dialogHost));

            if (dialogHost.IsOpen)
            {
                WatchWindowActivation(dialogHost);
            }
            else
            {
                dialogHost._asyncShowWaitHandle.Set();
                dialogHost._attachedDialogClosingEventHandler = null;
                dialogHost._session.IsEnded = true;
                dialogHost._session = null;
                dialogHost._closeCleanUp();

                return;
            }

            dialogHost._asyncShowWaitHandle.Reset();
            dialogHost._session = new DialogSession(dialogHost);

            var dialogOpenedEventArgs = new DialogOpenedEventArgs(dialogHost._session, DialogOpenedEvent);
            dialogHost.OnDialogOpened(dialogOpenedEventArgs);
            dialogHost._attachedDialogOpenedEventHandler?.Invoke(dialogHost, dialogOpenedEventArgs);
            dialogHost.DialogOpenedCallback?.Invoke(dialogHost, dialogOpenedEventArgs);
            dialogHost._asyncShowOpenedEventHandler?.Invoke(dialogHost, dialogOpenedEventArgs);

            dialogHost.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                var child = dialogHost.FocusPopup();
                Task.Delay(300).ContinueWith(t => child.Dispatcher.BeginInvoke(new Action(() => child.InvalidateVisual())));
            }));
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        #endregion

    }
}
