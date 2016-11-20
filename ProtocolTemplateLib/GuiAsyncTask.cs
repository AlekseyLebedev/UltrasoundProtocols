using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace ProtocolTemplateLib
{
    /// <summary>
    /// Этот класс позволяет запускать задачи в асинхронном потоке. Обрабатывает ошибки, выдавая сообщение с
    /// предложением повторить задачу (можно переопределить)
    /// </summary>
    /// <typeparam name="T">Тип результата асинхронной задачи</typeparam>
    public class GuiAsyncTask<T>
    {
        // Обязательные свойства
        /// <summary>
        /// Асинхронно выполняемая задача. [Обязательное свойство]
        /// </summary>
        public Func<T> AsyncTask { get; set; }
        /// <summary>
        /// Синхронно (в потоке окна) выполнеямое действие. Обрабатывает результат, полученный от асинхронной задачи.
        /// [Обязательное свойство]
        /// </summary>
        public Action<T> SyncTask { get; set; }
        /// <summary>
        /// Надо ли предлагать пользователю повторить попытку в случае неудачи (Exception) в задаче. 
        /// Не влияет, если собственный обработчик исключений
        /// [Обязательное свойство]
        /// </summary>
        public bool RetryEnabled { get; set; }
        /// <summary>
        /// Dispatcher окна, для которого выполняется задача
        /// [Обязательное свойство]
        /// </summary>
        public Dispatcher Dispatcher { get; set; }

        // Опциональные свойства
        /// <summary>
        /// Загаловок сообщения об ошибке. Обязательно, если стандартный обработчик ошибок.
        /// </summary>
        public string ErrorTitle { get; set; }
        /// <summary>
        /// Действие, выполняемое при ошибки и отказе от повтора. Вызывается в потоке окна. [Опционально]
        /// </summary>
        public Action Fail { get; set; }
        /// <summary>
        /// Класс для ведения логов. Сообщения о создании задачи и постоновке её в очередь пишутся в Debug,
        /// ошибки в Error. [Опционально]
        /// </summary>
        public Logger Logger { get; set; }
        /// <summary>
        /// Сообщение, записываемое в лог (Имя задачи). [Опционально]. Если не задать, пишутся только ошибки и в их
        /// сообщении записывается заголовко сообщения об ошибке.
        /// </summary>
        public String InfoMessage { get; set; }
        /// <summary>
        /// Собственный обработчик ошибок. [Опционально]
        /// </summary>
        public Action<Exception> CustomExceptionAction { get; set; }

        public GuiAsyncTask()
        {
            AsyncTask = null;
            SyncTask = null;
            Fail = null;
            RetryEnabled = true;
            ErrorTitle = null;
            Dispatcher = null;
            Logger = null;
            InfoMessage = null;
            CustomExceptionAction = null;
        }

        public GuiAsyncTask(Func<T> asyncTask, Action<T> syncTask, Action fail, bool retry, string errorTitle,
            Dispatcher dispatcher, Logger logger, string info, Action<Exception> customExceptionHanler = null)
        {
            AsyncTask = asyncTask;
            SyncTask = syncTask;
            Fail = fail;
            RetryEnabled = retry;
            ErrorTitle = errorTitle;
            Dispatcher = dispatcher;
            Logger = logger;
            InfoMessage = info;
            CustomExceptionAction = customExceptionHanler;
        }

        public void Run()
        {
            // Проверки на правильность запуска
            if ((AsyncTask == null) || (SyncTask == null) || (Dispatcher == null))
            {
                throw new ArgumentNullException("Some required properties of GuiAsyncTask are null");
            }
            if ((CustomExceptionAction == null) && (ErrorTitle == null))
            {
                throw new ArgumentNullException("Standart GuiAsyncTask error handler requires ErrorTitle");
            }
            if ((Logger != null) && (ErrorTitle == null) && (InfoMessage == null))
            {
                throw new ArgumentNullException("Logger requires ErrorTitle or InfoMessage");
            }

            if (InfoMessage != null)
                Logger?.Debug("Put task: {0}", InfoMessage);

            ThreadPool.QueueUserWorkItem((x) =>
            {
                if (InfoMessage != null)
                    Logger?.Debug("Perfome task: {0}", InfoMessage);
                try
                {
                    T result = AsyncTask();
                    Dispatcher.Invoke(new Action(() => SyncTask(result)));
                }
                catch (Exception ex)
                {
                    Logger?.Error(ex, (InfoMessage == null ? ErrorTitle : "Error: {0}"), InfoMessage);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (CustomExceptionAction == null)
                        {
                            if (RetryEnabled)
                            {
                                if (MessageBox.Show(String.Format("{0}{1}{2}", ex.Message, Environment.NewLine,
                                    "Попробовать еще раз?"), ErrorTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
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
                        }
                        else
                        {
                            CustomExceptionAction(ex);
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

    /// <summary>
    /// Этот класс позволяет запускать задачи в асинхронном потоке. Обрабатывает ошибки, выдавая сообщение с
    /// предложением повторить задачу (можно переопределить)
    /// </summary>
    public class GuiAsyncTask
    {
        // Обязательные свойства
        /// <summary>
        /// Асинхронно выполняемая задача. [Обязательное свойство]
        /// </summary>
        public Action AsyncTask { get; set; }
        /// <summary>
        /// Синхронно (в потоке окна) выполнеямое действие. Обрабатывает результат, полученный от асинхронной задачи.
        /// [Обязательное свойство]
        /// </summary>
        public Action SyncTask { get; set; }
        /// <summary>
        /// Надо ли предлагать пользователю повторить попытку в случае неудачи (Exception) в задаче. 
        /// Не влияет, если собственный обработчик исключений
        /// [Обязательное свойство]
        /// </summary>
        public bool RetryEnabled { get; set; }
        /// <summary>
        /// Dispatcher окна, для которого выполняется задача
        /// [Обязательное свойство]
        /// </summary>
        public Dispatcher Dispatcher { get; set; }

        // Опциональные свойства
        /// <summary>
        /// Загаловок сообщения об ошибке. Обязательно, если стандартный обработчик ошибок.
        /// </summary>
        public string ErrorTitle { get; set; }
        /// <summary>
        /// Действие, выполняемое при ошибки и отказе от повтора. Вызывается в потоке окна. [Опционально]
        /// </summary>
        public Action Fail { get; set; }
        /// <summary>
        /// Класс для ведения логов. Сообщения о создании задачи и постоновке её в очередь пишутся в Debug,
        /// ошибки в Error. [Опционально]
        /// </summary>
        public Logger Logger { get; set; }
        /// <summary>
        /// Сообщение, записываемое в лог (Имя задачи). [Опционально]. Если не задать, пишутся только ошибки и в их
        /// сообщении записывается заголовко сообщения об ошибке.
        /// </summary>
        public String InfoMessage { get; set; }
        /// <summary>
        /// Собственный обработчик ошибок. [Опционально]
        /// </summary>
        public Action<Exception> CustomExceptionAction { get; set; }


        public GuiAsyncTask()
        {
            AsyncTask = null;
            SyncTask = null;
            Fail = null;
            RetryEnabled = true;
            ErrorTitle = null;
            Dispatcher = null;
            Logger = null;
            InfoMessage = null;
            CustomExceptionAction = null;
        }

        public GuiAsyncTask(Action asyncTask, Action syncTask, Action fail, bool retry, string errorTitle,
            Dispatcher dispatcher, Logger logger, string info, Action<Exception> customExceptionHanler = null)
        {
            AsyncTask = asyncTask;
            SyncTask = syncTask;
            Fail = fail;
            RetryEnabled = retry;
            ErrorTitle = errorTitle;
            Dispatcher = dispatcher;
            Logger = logger;
            InfoMessage = info;
            CustomExceptionAction = customExceptionHanler;
        }

        public void Run()
        {
            // Проверки на правильность запуска
            if ((AsyncTask == null) || (SyncTask == null) || (Dispatcher == null))
            {
                throw new ArgumentNullException("Some required properties of GuiAsyncTask are null");
            }
            if ((CustomExceptionAction == null) && (ErrorTitle == null))
            {
                throw new ArgumentNullException("Standart GuiAsyncTask error handler requires ErrorTitle");
            }
            if ((Logger != null) && (ErrorTitle == null) && (InfoMessage == null))
            {
                throw new ArgumentNullException("Logger requires ErrorTitle or InfoMessage");
            }

            if (InfoMessage != null)
                Logger?.Debug("Put task: {0}", InfoMessage);

            ThreadPool.QueueUserWorkItem((x) =>
            {
                if (InfoMessage != null)
                    Logger?.Debug("Perfome task: {0}", InfoMessage);
                try
                {
                    AsyncTask();
                    Dispatcher.Invoke(new Action(() => SyncTask()));
                }
                catch (Exception ex)
                {
                    Logger?.Error(ex, (InfoMessage == null ? ErrorTitle : "Error: {0}"), InfoMessage);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (CustomExceptionAction == null)
                        {
                            if (RetryEnabled)
                            {
                                if (MessageBox.Show(String.Format("{0}{1}{2}", ex.Message, Environment.NewLine,
                                    "Попробовать еще раз?"), ErrorTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
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
                        }
                        else
                        {
                            CustomExceptionAction(ex);
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

