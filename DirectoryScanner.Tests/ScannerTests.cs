using DirectoryScanner.Model;

namespace DirectoryScanner.Tests
{
    public class Tests
    {
        private CancellationTokenSource _cancelTokenSource;

        private Scanner _scanner;

        [SetUp]
        public void Setup()
        {
            _scanner = new Scanner(10);
            _cancelTokenSource = new CancellationTokenSource();
        }

        [Test]
        public void Test_RootSize()
        {
            string path = @"C:\Users\ilyad\Documents\Ucheba";
            long size = 0;

            IDirectoryComponent root = _scanner.StartScanner(path, _cancelTokenSource.Token);
            foreach(var dir in root.ChildNodes.Where(child => child.Type == ComponentType.Directory))
            {
                size += dir.Size; 
            }

            Assert.That(size, Is.EqualTo(root.Size));
        }

        [Test]
        public void Test_Cancellation()
        {
            string path = @"C:\Users\ilyad\Documents\Ucheba\DirectoryScanner\\";

            var task = Task<DirectoryComponent>.Factory.StartNew(() => 
                (DirectoryComponent)_scanner.StartScanner(path, _cancelTokenSource.Token));

            for(int i = 0; i < 15000; i++)
            {
                Console.WriteLine("_");
            }
            _cancelTokenSource.Cancel();

            var result = task.Result;
            Assert.That(result.Size, Is.LessThan(15167488));
        }
    }
}