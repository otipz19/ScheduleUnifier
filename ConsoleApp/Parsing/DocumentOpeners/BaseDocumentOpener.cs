using ScheduleUnifier.Parsing.Exceptions;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    internal abstract class BaseDocumentOpener
    {
        public BaseDocumentOpener(string filePath)
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
