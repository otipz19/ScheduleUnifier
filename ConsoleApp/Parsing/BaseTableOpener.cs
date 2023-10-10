using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing
{
    internal abstract class BaseTableOpener
    {
        public BaseTableOpener(string filePath)
        {
            TryOpenDocument(filePath);
        }

        private void TryOpenDocument(string filePath)
        {
            try
            {
                OpenDocument(filePath);
            }
            catch (Exception ex)
            {
                throw new TableNotFoundException(filePath, ex);
            }
        }

        protected abstract void OpenDocument(string filePath);
    }
}
