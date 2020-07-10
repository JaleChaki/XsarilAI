namespace XsarilAI.Configuration.Reader {
	public interface IConfigurationReader {

		IConfiguration Read(string filename);

	}
}
