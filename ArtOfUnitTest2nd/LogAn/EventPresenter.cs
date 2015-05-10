using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    // イベントリスナのテストで使うクラス(Chap5-9 etc)
    public class EventPresenter
    {
        private readonly IView _view;
        private readonly ILogger _logger;
        
        public EventPresenter(IView view)
        {
            _view = view;
            this._view.Loaded += OnLoaded;
        }

        public EventPresenter(IView view, ILogger logger)
        {
            _view = view;
            _logger = logger;

            // IViewで定義したイベントにメソッドをひもづける
            this._view.Loaded += OnLoaded;
            this._view.ErrorOccured += OnError;
        }

        private void OnLoaded()
        {
            _view.Render("Hello World");
        }

        private void OnError(string text)
        {
            _logger.LogError(text);
        }
    }

    public interface IView
    {
        event Action Loaded;
        event Action<string> ErrorOccured;
        void Render(string text);
    }
}
