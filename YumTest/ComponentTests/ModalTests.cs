using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yum.Components.Shared;

namespace YumTest.ComponentTests
{
    
    public class ModalTests : TestContext
    {
        TestContext _context;
        private readonly string SomeChildContent = "<h1 class='child-content'>Test child content</h1>";
        public ModalTests()
        {
            _context = new TestContext();
        }

        [Fact]
        public void DisplaysChildContent()
        {
            IRenderedFragment cut = _context.RenderComponent<BootstrapModal>(parameters => parameters
                .AddChildContent(SomeChildContent)
            );

            var content = cut.Find("h1.child-content");
            content.MarkupMatches(SomeChildContent);
        }
    }
}
