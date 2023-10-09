using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing
{
    internal abstract class BaseTableOpener
    {
        public ITable TryOpen(string filePath)
        {
            try
            {
                return OpenTable(filePath);
            }
            catch (Exception ex)
            {
                throw new TableNotFoundException(filePath, ex);
            }
        }

        protected abstract ITable OpenTable(string filePath);
    }
}
