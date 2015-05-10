using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    using Xunit;
    using NSubstitute;
    public class EventPresenterTest
    {
        [Fact]
        public void ctor_WhenViewIsLoaded_CallsViewRender()
        {
            var mockView = Substitute.For<IView>();
            var p = new EventPresenter(mockView);

            // NSubstituteでイベントをトリガする
            mockView.Loaded += Raise.Event<Action>();

            // Viewが呼ばれたかをチェックする
            mockView.Received()
                    .Render(Arg.Is<string>(s => s.Contains("Hello World")));
        }

        [Fact]
        public void ctor_WhenViewHasError_CallsLogger()
        {
            var stubView = Substitute.For<IView>();
            var mockLogger = Substitute.For<ILogger>();

            var p = new EventPresenter(stubView, mockLogger);
            // エラーをシミュレーションする
            stubView.ErrorOccured += Raise.Event<Action<string>>("fake error");

            // モックを使って、ログが呼ばれたかをチェックする
            mockLogger.Received()
                      .LogError(Arg.Is<string>(s => s.Contains("fake error")));
        }
    }
}
