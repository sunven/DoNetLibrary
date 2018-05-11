using System;
using System.Threading;

namespace DoNetLibrary
{
    /// <summary>
    /// RetryStrategies
    /// </summary>
    public class Retry
    {
        private int _currentAttemptCount;
        private readonly int _maxAttemptCount;
        private readonly object _timerCallbackParam;
        private readonly Action _successAction;
        private readonly Action _failAction;
        private readonly Action _completeActon;

        private bool _isClose;

        public Timer Timer { get; set; }

        public Retry(Func<object, bool> fun, object timerCallbackParam, Action successAction, Action failAction, Action completeActon, int retryInterval, int delayInterval, int maxAttemptCount)
        {
            _successAction = successAction;
            _failAction = failAction;
            _completeActon = completeActon;
            _timerCallbackParam = timerCallbackParam;
            _maxAttemptCount = maxAttemptCount;
            Timer = new Timer(c =>
            {
                RetryFunc(s => fun(c));
            }, timerCallbackParam, delayInterval, retryInterval);
        }

        public void RetryFunc(Func<object, bool> fun)
        {
            var result = fun(_timerCallbackParam);
            if (result)
            {
                //成功了就终止
                _successAction?.Invoke();
                CloseTimer();
            }
            else
            {
                _failAction?.Invoke();
            }

            if (_maxAttemptCount == 0)
            {
                return;
            }
            if (++_currentAttemptCount == _maxAttemptCount)
            {
                //尝试到了最大次数就终止
                CloseTimer();
            }
        }

        public void CloseTimer()
        {
            if (!_isClose)
            {
                Timer.Dispose();
                _isClose = true;
            }
            _completeActon?.Invoke();
        }
    }
}