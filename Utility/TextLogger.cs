namespace SQMS.Utility
{
    public class TextLogger : ILogInFile
    {
        private readonly string _filePath = String.Empty;

        public TextLogger()
        {
            try
            {
                _filePath = Path.GetFullPath(ApplicationSetting.GetErrorLogFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Log write file path setup issue. " + ex.Message);
            }
        }

        public void LogWrite(string message)
        {
            FileStream file = null;
            StreamWriter sw = null;
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (!File.Exists(_filePath + fileName))
                {
                    if (!Directory.Exists(_filePath)) Directory.CreateDirectory(_filePath);

                    try
                    {
                        file = new FileStream(_filePath + fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
                        sw = new StreamWriter(file);
                        sw.WriteLine(message);
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        file = new FileStream(_filePath + fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                        sw = new StreamWriter(file);
                        sw.WriteLine(message);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            finally
            {
                sw.Close();
                file.Close();
            }
        }
    }
}
