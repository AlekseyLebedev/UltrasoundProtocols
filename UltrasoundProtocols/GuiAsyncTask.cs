using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
namespace UltrasoundProtocols
{
    class GuiAsyncTask<T>
    {
        private Func<T> AsyncTask;
        private Action<T> SyncTask;
        private Action Fail;
        private bool RunOnceMore;
        private string ErrorTitle;
        private Dispatcher Dispatcher_;
        private Logger Logger_;
        private String Info;

        public GuiAsyncTask(Func<T> asyncTask, Action<T> syncTask, Action fail, bool retry, string errorTitle, Dispatcher dispatcher, Logger logger, string info)
        {
            AsyncTask = asyncTask;
            SyncTask = syncTask;
            Fail = fail;
            RunOnceMore = retry;
            ErrorTitle = errorTitle;
            Dispatcher_ = dispatcher;
            Logger_ = logger;
            Info = info;
        }

        public void Run()
        {
            if (Info != null)
            {
                Logger_.Debug("Put task: {0}", Info);
            }
            ThreadPool.QueueUserWorkItem((x) =>
            {
                Logger_.Debug("Perfome task: {0}", Info);
                try
                {
                    T result = AsyncTask();
                    Dispatcher_.Invoke(new Action(() => SyncTask(result)));
                }
                catch (Exception ex)
                {
                    Logger_.Error(ex, (Info == null ? ErrorTitle : "Error: {0}"), Info);
                    Dispatcher_.Invoke(new Action(() =>
                    {
                        if (RunOnceMore)
                        {
                            if (MessageBox.Show(String.Format("{0}{1}{2}", ex.Message, Environment.NewLine,
                                "Попробовать еще раз"), ErrorTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                            {
                                Run();
                            }
                            else
                            {
                                InvokeFail();
                            }
                        }
                        else
                        {
                            MessageBox.Show(ex.Message, ErrorTitle, System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                            InvokeFail();
                        }
                    }));
                }

            });
        }

        private void InvokeFail()
        {
            if (Fail != null)
            {
                Fail();
            }
        }
    }
}
