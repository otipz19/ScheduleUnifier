using ConsoleApp.Parsing.Exceptions;

namespace ConsoleApp.Parsing.TableOpeners
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
                throw new NotFoundTableException(filePath, ex);
            }
        }

        protected abstract void OpenDocument(string filePath);
    }
}
