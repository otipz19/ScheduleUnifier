using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing
{
    internal interface ITableOpener
    {
        public ITable TryOpen(string filePath);

        public void Close();
    }
}
