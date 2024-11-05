using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class ChatServiceTest
    {

        private ChatServiceImplementation _chatServiceImplementation;
        private Mock<IChatServiceCallback> _mockCallback;

        [TestInitialize]
        public void Setup()
        {
            _chatServiceImplementation = new ChatServiceImplementation();
            _mockCallback = new Mock<IChatServiceCallback>();
        }


        [TestMethod]
        public void TestSendMessageSuccessful()
        {
            string message = "Hello";
            var callbackChannel = _mockCallback.Object;

            var mockContextChannel = new Mock<IContextChannel>();
            mockContextChannel.Setup(c => c.State).Returns(CommunicationState.Opened);

            var callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();

            using (var scope = new OperationContextScope(mockContextChannel.Object))
            {
                _chatServiceImplementation.SendMessage(message);
            }

            _mockCallback.Verify(cb => cb.BroadcastMessage(message), Times.Once);
        }

    }
}
