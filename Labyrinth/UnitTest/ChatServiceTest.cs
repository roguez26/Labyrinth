using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class ChatServiceTest : IChatServiceCallback
    {

        private ChatServiceImplementation _chatServiceImplementation;
        private string _message;

        [TestInitialize]
        public void Setup()
        {
            _chatServiceImplementation = new ChatServiceImplementation();
            //_chatServiceImplementation.SendMessage("Hello");
        }

        [TestMethod]
        public void TestSendMessageSuccessful()
        {
          
            Assert.IsTrue(true);
            
        }

        public void BroadcastMessage(string message)
        {
            _message = message;
        }
    }
}
