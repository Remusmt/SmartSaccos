using NUnit.Framework;
using SmartSaccos.ApplicationCore.Services;
using System.Threading.Tasks;

namespace SmartSaccos.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            MessageService messageService = new MessageService();
            bool sent = await messageService.SendEmail("remusmt@gmail.com", "Test", "From sacco");
            Assert.IsTrue(sent);
        }
    }
}