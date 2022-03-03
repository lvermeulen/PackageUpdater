namespace PackageUpdater.Abstractions
{
    public class InspectFolderResult
    {
        public bool IsSuccess { get; }
        public Repository Repository { get; }
        public string Error { get; }

        protected InspectFolderResult(Repository repository)
        {
            IsSuccess = true;
            Repository = repository;
        }
        
        protected InspectFolderResult(string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static InspectFolderResult Success(Repository repository) => new InspectFolderResult(repository);
        public static InspectFolderResult Failure(string error = null) => new InspectFolderResult(error);
    }
}
