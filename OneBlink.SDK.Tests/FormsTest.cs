using System;
using Xunit;
using OneBlink.SDK;

namespace unit_tests
{
    public class FormsTest
    {
        [Fact]
        public void can_be_constructed()
        {
            Forms forms = new Forms("a", "b");
            Assert.NotNull(forms);
        }

        [Fact]
        public void throws_error_if_keys_empty() {
            try
            {
                Forms forms = new Forms("", "");
            }
            catch (Exception ex) {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public async void can_search_forms() {
            Forms forms = new Forms("a", "b"); // TODO Get creds from config
            string response = await forms.search(null, null, null);
            Assert.NotNull(response);
            // Console.WriteLine("Response: " + response);
        }

        [Fact]
        public async void can_search_forms_with_all_params() {
            Forms forms = new Forms("a", "b"); // TODO Get creds from config
            string response = await forms.search(true, true, "Location test");
            Assert.NotNull(response);
            // Console.WriteLine("Response: " + response);
        }
    }
}