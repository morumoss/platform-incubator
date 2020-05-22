using Moq;
using MorumOSSWebChat.Hubs;
using MorumOSSWebChat.Models;
using NUnit.Framework;
using SignalR_UnitTestingSupport.Hubs;
using System;

namespace MorusOSSWebChatNUnitTestProject
{
    public class Tests
    {
        private string testOutput;

        [SetUp]
        public void Setup()
        {
            //This unit test suite is looking for the test file created from the client side on the desktop with the default name.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\testOutput.txt";
            testOutput = System.IO.File.ReadAllText(path);
        }

        [Test]
        public void AdminViewUserInfoOutputsInfo()
        {
            Assert.True(testOutput.Contains("adminViewUserInfo:Pass"));
        }

        [Test]
        public void AdminBlockUserUserInfoShowsBlocked()
        {
            Assert.True(testOutput.Contains("adminBlockUser:Pass"));
        }

        [Test]
        public void AdmiClearChatChatHasNoEntries()
        {
            Assert.True(testOutput.Contains("adminClearChat:Pass"));
        }

        [Test]
        public void SendMessageChatHasOneEntry()
        {
            Assert.True(testOutput.Contains("sendMessage:Pass"));
        }

        [Test]
        public void UserClearChatChatHasNoEntries()
        {
            Assert.True(testOutput.Contains("userClearChat:Pass"));
        }

        [Test]
        public void AdminDeleteMessageChatHassMessageThenNoMessage()
        {
            Assert.True(testOutput.Contains("adminDeleteMessage:Pass"));
        }

        [Test]
        public void AdminToggleModeratorUserInfoChanges()
        {
            Assert.True(testOutput.Contains("adminToggleMod:Pass"));
        }

        [Test]
        public void AddWordFilterFilterBlocksWordTest()
        {
            Assert.True(testOutput.Contains("addWordFilter:Pass"));
        }

        [Test]
        public void UserShowConnectedUsersNewEntryInChat()
        {
            Assert.True(testOutput.Contains("userShowConnectedUsers:Pass"));
        }
    }
}